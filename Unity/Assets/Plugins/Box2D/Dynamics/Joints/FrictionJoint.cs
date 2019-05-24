using System.Diagnostics;
using System.Numerics;
using Box2DSharp.Common;

namespace Box2DSharp.Dynamics.Joints
{
    /// Friction joint. This is used for top-down friction.
    /// It provides 2D translational friction and angular friction.
    public class FrictionJoint : Joint
    {
        private float _angularImpulse;

        private float _angularMass;

        // Solver temp
        private int _indexA;

        private int _indexB;

        private float _invIa;

        private float _invIb;

        private float _invMassA;

        private float _invMassB;

        // Solver shared
        private Vector2 _linearImpulse;

        private Matrix2x2 _linearMass;

        private Vector2 _localAnchorA;

        private Vector2 _localAnchorB;

        private Vector2 _localCenterA;

        private Vector2 _localCenterB;

        private float _maxForce;

        private float _maxTorque;

        private Vector2 _rA;

        private Vector2 _rB;

        internal FrictionJoint(FrictionJointDef def) : base(def)
        {
            _localAnchorA = def.LocalAnchorA;
            _localAnchorB = def.LocalAnchorB;

            _linearImpulse.SetZero();
            _angularImpulse = 0.0f;

            _maxForce = def.MaxForce;
            _maxTorque = def.MaxTorque;
        }

        /// Get/Set the maximum friction force in N.

        public float MaxForce
        {
            get => _maxForce;
            set
            {
                Debug.Assert(value.IsValid() && value >= 0.0f);
                _maxForce = value;
            }
        }

        public float MaxTorque
        {
            get => _maxTorque;
            set
            {
                Debug.Assert(value.IsValid() && value >= 0.0f);
                _maxTorque = value;
            }
        }

        /// The local anchor point relative to bodyA's origin.
        public Vector2 GetLocalAnchorA()
        {
            return _localAnchorA;
        }

        /// The local anchor point relative to bodyB's origin.
        public Vector2 GetLocalAnchorB()
        {
            return _localAnchorB;
        }

        /// <inheritdoc />
        public override Vector2 GetAnchorA()
        {
            return BodyA.GetWorldPoint(_localAnchorA);
        }

        /// <inheritdoc />
        public override Vector2 GetAnchorB()
        {
            return BodyB.GetWorldPoint(_localAnchorB);
        }

        /// <inheritdoc />
        public override Vector2 GetReactionForce(float inv_dt)
        {
            return inv_dt * _linearImpulse;
        }

        /// <inheritdoc />
        public override float GetReactionTorque(float inv_dt)
        {
            return inv_dt * _angularImpulse;
        }

        /// Dump joint to dmLog
        public override void Dump()
        { }

        /// <inheritdoc />
        internal override void InitVelocityConstraints(in SolverData data)
        {
            _indexA = BodyA.IslandIndex;
            _indexB = BodyB.IslandIndex;
            _localCenterA = BodyA.Sweep.LocalCenter;
            _localCenterB = BodyB.Sweep.LocalCenter;
            _invMassA = BodyA.InvMass;
            _invMassB = BodyB.InvMass;
            _invIa = BodyA.InverseInertia;
            _invIb = BodyB.InverseInertia;

            var aA = data.Positions[_indexA].Angle;
            var vA = data.Velocities[_indexA].V;
            var wA = data.Velocities[_indexA].W;

            var aB = data.Positions[_indexB].Angle;
            var vB = data.Velocities[_indexB].V;
            var wB = data.Velocities[_indexB].W;

            var qA = new Rotation(aA);
            var qB = new Rotation(aB);

            // Compute the effective mass matrix.
            _rA = MathUtils.Mul(qA, _localAnchorA - _localCenterA);
            _rB = MathUtils.Mul(qB, _localAnchorB - _localCenterB);

            // J = [-I -r1_skew I r2_skew]
            //     [ 0       -1 0       1]
            // r_skew = [-ry; rx]

            // Matlab
            // K = [ mA+r1y^2*iA+mB+r2y^2*iB,  -r1y*iA*r1x-r2y*iB*r2x,          -r1y*iA-r2y*iB]
            //     [  -r1y*iA*r1x-r2y*iB*r2x, mA+r1x^2*iA+mB+r2x^2*iB,           r1x*iA+r2x*iB]
            //     [          -r1y*iA-r2y*iB,           r1x*iA+r2x*iB,                   iA+iB]

            float mA = _invMassA, mB = _invMassB;
            float iA = _invIa, iB = _invIb;

            var K = new Matrix2x2();
            K.Ex.X = mA + mB + iA * _rA.Y * _rA.Y + iB * _rB.Y * _rB.Y;
            K.Ex.Y = -iA * _rA.X * _rA.Y - iB * _rB.X * _rB.Y;
            K.Ey.X = K.Ex.Y;
            K.Ey.Y = mA + mB + iA * _rA.X * _rA.X + iB * _rB.X * _rB.X;

            _linearMass = K.GetInverse();

            _angularMass = iA + iB;
            if (_angularMass > 0.0f)
            {
                _angularMass = 1.0f / _angularMass;
            }

            if (data.Step.WarmStarting)
            {
                // Scale impulses to support a variable time step.
                _linearImpulse *= data.Step.DtRatio;
                _angularImpulse *= data.Step.DtRatio;

                var P = new Vector2(_linearImpulse.X, _linearImpulse.Y);
                vA -= mA * P;
                wA -= iA * (MathUtils.Cross(_rA, P) + _angularImpulse);
                vB += mB * P;
                wB += iB * (MathUtils.Cross(_rB, P) + _angularImpulse);
            }
            else
            {
                _linearImpulse.SetZero();
                _angularImpulse = 0.0f;
            }

            data.Velocities[_indexA].V = vA;
            data.Velocities[_indexA].W = wA;
            data.Velocities[_indexB].V = vB;
            data.Velocities[_indexB].W = wB;
        }

        /// <inheritdoc />
        internal override void SolveVelocityConstraints(in SolverData data)
        {
            var vA = data.Velocities[_indexA].V;
            var wA = data.Velocities[_indexA].W;
            var vB = data.Velocities[_indexB].V;
            var wB = data.Velocities[_indexB].W;

            float mA = _invMassA, mB = _invMassB;
            float iA = _invIa, iB = _invIb;

            var h = data.Step.Dt;

            // Solve angular friction
            {
                var Cdot = wB - wA;
                var impulse = -_angularMass * Cdot;

                var oldImpulse = _angularImpulse;
                var maxImpulse = h * _maxTorque;
                _angularImpulse = MathUtils.Clamp(_angularImpulse + impulse, -maxImpulse, maxImpulse);
                impulse = _angularImpulse - oldImpulse;

                wA -= iA * impulse;
                wB += iB * impulse;
            }

            // Solve linear friction
            {
                var Cdot = vB + MathUtils.Cross(wB, _rB) - vA - MathUtils.Cross(wA, _rA);

                var impulse = -MathUtils.Mul(_linearMass, Cdot);
                var oldImpulse = _linearImpulse;
                _linearImpulse += impulse;

                var maxImpulse = h * _maxForce;

                if (_linearImpulse.LengthSquared() > maxImpulse * maxImpulse)
                {
                    _linearImpulse.Normalize();
                    _linearImpulse *= maxImpulse;
                }

                impulse = _linearImpulse - oldImpulse;

                vA -= mA * impulse;
                wA -= iA * MathUtils.Cross(_rA, impulse);

                vB += mB * impulse;
                wB += iB * MathUtils.Cross(_rB, impulse);
            }

            data.Velocities[_indexA].V = vA;
            data.Velocities[_indexA].W = wA;
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