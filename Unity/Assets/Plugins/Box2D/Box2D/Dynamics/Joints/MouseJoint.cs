using System.Diagnostics;
using System.Numerics;
using Box2DSharp.Common;

namespace Box2DSharp.Dynamics.Joints
{
    /// A mouse joint is used to make a point on a body track a
    /// specified world point. This a soft constraint with a maximum
    /// force. This allows the constraint to stretch and without
    /// applying huge forces.
    /// NOTE: this joint is not documented in the manual because it was
    /// developed to be used in the testbed. If you want to learn how to
    /// use the mouse joint, look at the testbed.
    public class MouseJoint : Joint
    {
        private readonly Vector2 _localAnchorB;

        private float _beta;

        private Vector2 _C;

        private float _dampingRatio;

        private float _frequencyHz;

        private float _gamma;

        // Solver shared
        private Vector2 _impulse;

        // Solver temp
        private int _indexB;

        private float _invIb;

        private float _invMassB;

        private Vector2 _localCenterB;

        private Matrix2x2 _mass;

        private float _maxForce;

        private Vector2 _rB;

        private Vector2 _targetA;

        internal MouseJoint(MouseJointDef def) : base(def)
        {
            Debug.Assert(def.Target.IsValid());
            Debug.Assert(def.MaxForce.IsValid() && def.MaxForce >= 0.0f);
            Debug.Assert(def.FrequencyHz.IsValid() && def.FrequencyHz >= 0.0f);
            Debug.Assert(def.DampingRatio.IsValid() && def.DampingRatio >= 0.0f);

            _targetA = def.Target;
            _localAnchorB = MathUtils.MulT(BodyB.GetTransform(), _targetA);

            _maxForce = def.MaxForce;
            _impulse.SetZero();

            _frequencyHz = def.FrequencyHz;
            _dampingRatio = def.DampingRatio;

            _beta = 0.0f;
            _gamma = 0.0f;
        }

        /// Implements b2Joint.
        /// Use this to update the target point.
        public void SetTarget(in Vector2 target)
        {
            if (target != _targetA)
            {
                BodyB.IsAwake = true;
                _targetA = target;
            }
        }

        public Vector2 GetTarget()
        {
            return _targetA;
        }

        /// Set/get the maximum force in Newtons.
        public void SetMaxForce(float force)
        {
            _maxForce = force;
        }

        public float GetMaxForce()
        {
            return _maxForce;
        }

        /// Set/get the frequency in Hertz.
        public void SetFrequency(float hz)
        {
            _frequencyHz = hz;
        }

        public float GetFrequency()
        {
            return _frequencyHz;
        }

        /// Set/get the damping ratio (dimensionless).
        public void SetDampingRatio(float ratio)
        {
            _dampingRatio = ratio;
        }

        public float GetDampingRatio()
        {
            return _dampingRatio;
        }

        /// <inheritdoc />
        public override void ShiftOrigin(in Vector2 newOrigin)
        {
            _targetA -= newOrigin;
        }

        /// <inheritdoc />
        public override Vector2 GetAnchorA()
        {
            return _targetA;
        }

        /// <inheritdoc />
        public override Vector2 GetAnchorB()
        {
            return BodyB.GetWorldPoint(_localAnchorB);
        }

        /// <inheritdoc />
        public override Vector2 GetReactionForce(float inv_dt)
        {
            return inv_dt * _impulse;
        }

        /// <inheritdoc />
        public override float GetReactionTorque(float inv_dt)
        {
            return inv_dt * 0.0f;
        }

        /// The mouse joint does not support dumping.
        public override void Dump()
        {
            DumpLogger.Log("Mouse joint dumping is not supported.");
        }

        /// <inheritdoc />
        internal override void InitVelocityConstraints(in SolverData data)
        {
            _indexB = BodyB.IslandIndex;
            _localCenterB = BodyB.Sweep.LocalCenter;
            _invMassB = BodyB.InvMass;
            _invIb = BodyB.InverseInertia;

            var cB = data.Positions[_indexB].Center;
            var aB = data.Positions[_indexB].Angle;
            var vB = data.Velocities[_indexB].V;
            var wB = data.Velocities[_indexB].W;

            var qB = new Rotation(aB);

            var mass = BodyB.Mass;

            // Frequency
            var omega = 2.0f * Settings.Pi * _frequencyHz;

            // Damping coefficient
            var d = 2.0f * mass * _dampingRatio * omega;

            // Spring stiffness
            var k = mass * (omega * omega);

            // magic formulas
            // gamma has units of inverse mass.
            // beta has units of inverse time.
            var h = data.Step.Dt;
            Debug.Assert(d + h * k > Settings.Epsilon);
            _gamma = h * (d + h * k);
            if (!_gamma.Equals(0.0f))
            {
                _gamma = 1.0f / _gamma;
            }

            _beta = h * k * _gamma;

            // Compute the effective mass matrix.
            _rB = MathUtils.Mul(qB, _localAnchorB - _localCenterB);

            // K    = [(1/m1 + 1/m2) * eye(2) - skew(r1) * invI1 * skew(r1) - skew(r2) * invI2 * skew(r2)]
            //      = [1/m1+1/m2     0    ] + invI1 * [r1.Y*r1.Y -r1.X*r1.Y] + invI2 * [r1.Y*r1.Y -r1.X*r1.Y]
            //        [    0     1/m1+1/m2]           [-r1.X*r1.Y r1.X*r1.X]           [-r1.X*r1.Y r1.X*r1.X]
            var K = new Matrix2x2();
            K.Ex.X = _invMassB + _invIb * _rB.Y * _rB.Y + _gamma;
            K.Ex.Y = -_invIb * _rB.X * _rB.Y;
            K.Ey.X = K.Ex.Y;
            K.Ey.Y = _invMassB + _invIb * _rB.X * _rB.X + _gamma;

            _mass = K.GetInverse();

            _C = cB + _rB - _targetA;
            _C *= _beta;

            // Cheat with some damping
            wB *= 0.98f;

            if (data.Step.WarmStarting)
            {
                _impulse *= data.Step.DtRatio;
                vB += _invMassB * _impulse;
                wB += _invIb * MathUtils.Cross(_rB, _impulse);
            }
            else
            {
                _impulse.SetZero();
            }

            data.Velocities[_indexB].V = vB;
            data.Velocities[_indexB].W = wB;
        }

        /// <inheritdoc />
        internal override void SolveVelocityConstraints(in SolverData data)
        {
            var vB = data.Velocities[_indexB].V;
            var wB = data.Velocities[_indexB].W;

            // Cdot = v + cross(w, r)
            var cdot = vB + MathUtils.Cross(wB, _rB);
            var impulse = MathUtils.Mul(_mass, -(cdot + _C + _gamma * _impulse));

            var oldImpulse = _impulse;
            _impulse += impulse;
            var maxImpulse = data.Step.Dt * _maxForce;
            if (_impulse.LengthSquared() > maxImpulse * maxImpulse)
            {
                _impulse *= maxImpulse / _impulse.Length();
            }

            impulse = _impulse - oldImpulse;

            vB += _invMassB * impulse;
            wB += _invIb * MathUtils.Cross(_rB, impulse);

            data.Velocities[_indexB].V = vB;
            data.Velocities[_indexB].W = wB;
        }

        /// <inheritdoc />
        internal override bool SolvePositionConstraints(in SolverData data)
        {
            return true;
        }
    }
}