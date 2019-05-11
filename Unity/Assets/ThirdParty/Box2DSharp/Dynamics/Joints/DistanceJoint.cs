using System;
using System.Numerics;
using Box2DSharp.Common;

namespace Box2DSharp.Dynamics.Joints
{
    /// A distance joint constrains two points on two bodies
    /// to remain at a fixed distance from each other. You can view
    /// this as a massless, rigid rod.
    public class DistanceJoint : Joint
    {
        // Solver shared
        private readonly Vector2 _localAnchorA;

        private readonly Vector2 _localAnchorB;

        private float _bias;

        private float _gamma;

        private float _impulse;

        // Solver temp
        private int _indexA;

        private int _indexB;

        private float _invIa;

        private float _invIb;

        private float _invMassA;

        private float _invMassB;

        private Vector2 _localCenterA;

        private Vector2 _localCenterB;

        private float _mass;

        private Vector2 _rA;

        private Vector2 _rB;

        private Vector2 _u;

        internal DistanceJoint(DistanceJointDef def) : base(def)
        {
            _localAnchorA = def.LocalAnchorA;
            _localAnchorB = def.LocalAnchorB;
            Length = def.Length;
            FrequencyHz = def.FrequencyHz;
            DampingRatio = def.DampingRatio;
            _impulse = 0.0f;
            _gamma = 0.0f;
            _bias = 0.0f;
        }

        /// Set/get the natural length.
        /// Manipulating the length can lead to non-physical behavior when the frequency is zero.
        public float Length { get; set; }

        /// Set/get frequency in Hz.
        public float FrequencyHz { get; set; }

        /// Set/get damping ratio.

        public float DampingRatio { get; set; }

        public override Vector2 GetAnchorA()
        {
            return BodyA.GetWorldPoint(_localAnchorA);
        }

        public override Vector2 GetAnchorB()
        {
            return BodyB.GetWorldPoint(_localAnchorB);
        }

        /// Get the reaction force given the inverse time step.
        /// Unit is N.
        public override Vector2 GetReactionForce(float inv_dt)
        {
            var F = inv_dt * _impulse * _u;
            return F;
        }

        /// Get the reaction torque given the inverse time step.
        /// Unit is N*m. This is always zero for a distance joint.
        public override float GetReactionTorque(float inv_dt)
        {
            return 0.0f;
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

        /// Dump joint to dmLog
        public override void Dump()
        {
            var indexA = BodyA.IslandIndex;
            var indexB = BodyB.IslandIndex;

            DumpLogger.Log("  b2DistanceJointDef jd;");
            DumpLogger.Log($"  jd.bodyA = bodies[{indexA}];");
            DumpLogger.Log($"  jd.bodyB = bodies[{indexB}];");
            DumpLogger.Log($"  jd.collideConnected = bool({CollideConnected});");
            DumpLogger.Log($"  jd.localAnchorA.Set({_localAnchorA.X}, {_localAnchorA.Y});");
            DumpLogger.Log($"  jd.localAnchorB.Set({_localAnchorB.X}, {_localAnchorB.Y});");
            DumpLogger.Log($"  jd.length = {Length};");
            DumpLogger.Log($"  jd.frequencyHz = {FrequencyHz};");
            DumpLogger.Log($"  jd.dampingRatio = {DampingRatio};");
            DumpLogger.Log($"  joints[{Index}] = m_world.CreateJoint(&jd);");
        }

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

            var cA = data.Positions[_indexA].Center;
            var aA = data.Positions[_indexA].Angle;
            var vA = data.Velocities[_indexA].V;
            var wA = data.Velocities[_indexA].W;

            var cB = data.Positions[_indexB].Center;
            var aB = data.Positions[_indexB].Angle;
            var vB = data.Velocities[_indexB].V;
            var wB = data.Velocities[_indexB].W;

            var qA = new Rotation(aA);
            var qB = new Rotation(aB);

            _rA = MathUtils.Mul(qA, _localAnchorA - _localCenterA);
            _rB = MathUtils.Mul(qB, _localAnchorB - _localCenterB);
            _u = cB + _rB - cA - _rA;

            // Handle singularity.
            var length = _u.Length();
            if (length > Settings.LinearSlop)
            {
                _u *= 1.0f / length;
            }
            else
            {
                _u.Set(0.0f, 0.0f);
            }

            var crAu = MathUtils.Cross(_rA, _u);
            var crBu = MathUtils.Cross(_rB, _u);
            var invMass = _invMassA + _invIa * crAu * crAu + _invMassB + _invIb * crBu * crBu;

            // Compute the effective mass matrix.
            _mass = Math.Abs(invMass) > Settings.Epsilon ? 1.0f / invMass : 0.0f;

            if (FrequencyHz > 0.0f)
            {
                var C = length - Length;

                // Frequency
                var omega = 2.0f * Settings.Pi * FrequencyHz;

                // Damping coefficient
                var d = 2.0f * _mass * DampingRatio * omega;

                // Spring stiffness
                var k = _mass * omega * omega;

                // magic formulas
                var h = data.Step.Dt;
                _gamma = h * (d + h * k);
                _gamma = !_gamma.Equals(0.0f) ? 1.0f / _gamma : 0.0f;
                _bias = C * h * k * _gamma;

                invMass += _gamma;
                _mass = Math.Abs(invMass) > Settings.Epsilon ? 1.0f / invMass : 0.0f;
            }
            else
            {
                _gamma = 0.0f;
                _bias = 0.0f;
            }

            if (data.Step.WarmStarting)
            {
                // Scale the impulse to support a variable time step.
                _impulse *= data.Step.DtRatio;

                var P = _impulse * _u;
                vA -= _invMassA * P;
                wA -= _invIa * MathUtils.Cross(_rA, P);
                vB += _invMassB * P;
                wB += _invIb * MathUtils.Cross(_rB, P);
            }
            else
            {
                _impulse = 0.0f;
            }

            data.Velocities[_indexA].V = vA;
            data.Velocities[_indexA].W = wA;
            data.Velocities[_indexB].V = vB;
            data.Velocities[_indexB].W = wB;
        }

        internal override void SolveVelocityConstraints(in SolverData data)
        {
            var vA = data.Velocities[_indexA].V;
            var wA = data.Velocities[_indexA].W;
            var vB = data.Velocities[_indexB].V;
            var wB = data.Velocities[_indexB].W;

            // Cdot = dot(u, v + cross(w, r))
            var vpA = vA + MathUtils.Cross(wA, _rA);
            var vpB = vB + MathUtils.Cross(wB, _rB);
            var Cdot = Vector2.Dot(_u, vpB - vpA);

            var impulse = -_mass * (Cdot + _bias + _gamma * _impulse);
            _impulse += impulse;

            var P = impulse * _u;
            vA -= _invMassA * P;
            wA -= _invIa * MathUtils.Cross(_rA, P);
            vB += _invMassB * P;
            wB += _invIb * MathUtils.Cross(_rB, P);

            data.Velocities[_indexA].V = vA;
            data.Velocities[_indexA].W = wA;
            data.Velocities[_indexB].V = vB;
            data.Velocities[_indexB].W = wB;
        }

        internal override bool SolvePositionConstraints(in SolverData data)
        {
            if (FrequencyHz > 0.0f)
            {
                // There is no position correction for soft distance constraints.
                return true;
            }

            var cA = data.Positions[_indexA].Center;
            var aA = data.Positions[_indexA].Angle;
            var cB = data.Positions[_indexB].Center;
            var aB = data.Positions[_indexB].Angle;

            var qA = new Rotation(aA);
            var qB = new Rotation(aB);

            var rA = MathUtils.Mul(qA, _localAnchorA - _localCenterA);
            var rB = MathUtils.Mul(qB, _localAnchorB - _localCenterB);
            var u = cB + rB - cA - rA;

            var length = u.Length();
            u = Vector2.Normalize(u);
            var C = length - Length;
            C = MathUtils.Clamp(C, -Settings.MaxLinearCorrection, Settings.MaxLinearCorrection);

            var impulse = -_mass * C;
            var P = impulse * u;

            cA -= _invMassA * P;
            aA -= _invIa * MathUtils.Cross(rA, P);
            cB += _invMassB * P;
            aB += _invIb * MathUtils.Cross(rB, P);

            data.Positions[_indexA].Center = cA;
            data.Positions[_indexA].Angle = aA;
            data.Positions[_indexB].Center = cB;
            data.Positions[_indexB].Angle = aB;

            return Math.Abs(C) < Settings.LinearSlop;
        }
    }
}