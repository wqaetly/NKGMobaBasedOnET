using System;
using System.Numerics;
using Box2DSharp.Common;

namespace Box2DSharp.Dynamics.Joints
{
    /// A rope joint enforces a maximum distance between two points
    /// on two bodies. It has no other effect.
    /// Warning: if you attempt to change the maximum length during
    /// the simulation you will get some non-physical behavior.
    /// A model that would allow you to dynamically modify the length
    /// would have some sponginess, so I chose not to implement it
    /// that way. See b2DistanceJoint if you want to dynamically
    /// control length.
    public class RopeJoint : Joint
    {
        // Solver shared
        private readonly Vector2 _localAnchorA;

        private readonly Vector2 _localAnchorB;

        private float _impulse;

        // Solver temp
        private int _indexA;

        private int _indexB;

        private float _invIa;

        private float _invIb;

        private float _invMassA;

        private float _invMassB;

        private float _length;

        private Vector2 _localCenterA;

        private Vector2 _localCenterB;

        private float _mass;

        private float _maxLength;

        private Vector2 _rA;

        private Vector2 _rB;

        private LimitState _state;

        private Vector2 _u;

        internal RopeJoint(RopeJointDef def) : base(def)
        {
            _localAnchorA = def.LocalAnchorA;
            _localAnchorB = def.LocalAnchorB;

            _maxLength = def.MaxLength;

            _mass = 0.0f;
            _impulse = 0.0f;
            _state = LimitState.InactiveLimit;
            _length = 0.0f;
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

        /// Set/Get the maximum length of the rope.
        public void SetMaxLength(float length)
        {
            _maxLength = length;
        }

        public float GetMaxLength()
        {
            return _maxLength;
        }

        public LimitState GetLimitState()
        {
            return _state;
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
            var F = inv_dt * _impulse * _u;
            return F;
        }

        /// <inheritdoc />
        public override float GetReactionTorque(float inv_dt)
        {
            return 0.0f;
        }

        /// Dump joint to dmLog
        public override void Dump()
        {
            var indexA = BodyA.IslandIndex;
            var indexB = BodyB.IslandIndex;

            DumpLogger.Log("  b2RopeJointDef jd;");
            DumpLogger.Log($"  jd.bodyA = bodies[{indexA}];");
            DumpLogger.Log($"  jd.bodyB = bodies[{indexB}];");
            DumpLogger.Log($"  jd.collideConnected = bool({CollideConnected});");
            DumpLogger.Log($"  jd.localAnchorA.Set({_localAnchorA.X}, {_localAnchorA.Y});");
            DumpLogger.Log($"  jd.localAnchorB.Set({_localAnchorB.X}, {_localAnchorB.Y});");
            DumpLogger.Log($"  jd.maxLength = {_maxLength};");
            DumpLogger.Log($"  joints[{Index}] = m_world.CreateJoint(&jd);");
        }

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

            _length = _u.Length();

            var C = _length - _maxLength;
            if (C > 0.0f)
            {
                _state = LimitState.AtUpperLimit;
            }
            else
            {
                _state = LimitState.InactiveLimit;
            }

            if (_length > Settings.LinearSlop)
            {
                _u *= 1.0f / _length;
            }
            else
            {
                _u.SetZero();
                _mass = 0.0f;
                _impulse = 0.0f;
                return;
            }

            // Compute effective mass.
            var crA = MathUtils.Cross(_rA, _u);
            var crB = MathUtils.Cross(_rB, _u);
            var invMass = _invMassA + _invIa * crA * crA + _invMassB + _invIb * crB * crB;

            _mass = !invMass.Equals(0.0f) ? 1.0f / invMass : 0.0f;

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

        /// <inheritdoc />
        internal override void SolveVelocityConstraints(in SolverData data)
        {
            var vA = data.Velocities[_indexA].V;
            var wA = data.Velocities[_indexA].W;
            var vB = data.Velocities[_indexB].V;
            var wB = data.Velocities[_indexB].W;

            // Cdot = dot(u, v + cross(w, r))
            var vpA = vA + MathUtils.Cross(wA, _rA);
            var vpB = vB + MathUtils.Cross(wB, _rB);
            var C = _length - _maxLength;
            var cdot = Vector2.Dot(_u, vpB - vpA);

            // Predictive constraint.
            if (C < 0.0f)
            {
                cdot += data.Step.InvDt * C;
            }

            var impulse = -_mass * cdot;
            var oldImpulse = _impulse;
            _impulse = Math.Min(0.0f, _impulse + impulse);
            impulse = _impulse - oldImpulse;

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

        /// <inheritdoc />
        internal override bool SolvePositionConstraints(in SolverData data)
        {
            var cA = data.Positions[_indexA].Center;
            var aA = data.Positions[_indexA].Angle;
            var cB = data.Positions[_indexB].Center;
            var aB = data.Positions[_indexB].Angle;

            var qA = new Rotation(aA);
            var qB = new Rotation(aB);

            var rA = MathUtils.Mul(qA, _localAnchorA - _localCenterA);
            var rB = MathUtils.Mul(qB, _localAnchorB - _localCenterB);
            var u = cB + rB - cA - rA;

            var length = u.Normalize();
            var C = length - _maxLength;

            C = MathUtils.Clamp(C, 0.0f, Settings.MaxLinearCorrection);

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

            return length - _maxLength < Settings.LinearSlop;
        }
    }
}