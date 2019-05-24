using System;
using System.Diagnostics;
using System.Numerics;
using Box2DSharp.Common;

namespace Box2DSharp.Dynamics.Joints
{
    /// The pulley joint is connected to two bodies and two fixed ground points.
    /// The pulley supports a ratio such that:
    /// length1 + ratio * length2 <= constant
    /// Yes, the force transmitted is scaled by the ratio.
    /// Warning: the pulley joint can get a bit squirrelly by itself. They often
    /// work better when combined with prismatic joints. You should also cover the
    /// the anchor points with static shapes to prevent one side from going to
    /// zero length.
    public class PulleyJoint : Joint
    {
        private readonly float _constant;

        private readonly float _lengthA;

        private readonly float _lengthB;

        // Solver shared
        private readonly Vector2 _localAnchorA;

        private readonly Vector2 _localAnchorB;

        private readonly float _ratio;

        private Vector2 _groundAnchorA;

        private Vector2 _groundAnchorB;

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

        private Vector2 _uA;

        private Vector2 _uB;

        public PulleyJoint(PulleyJointDef def) : base(def)
        {
            _groundAnchorA = def.GroundAnchorA;
            _groundAnchorB = def.GroundAnchorB;
            _localAnchorA = def.LocalAnchorA;
            _localAnchorB = def.LocalAnchorB;

            _lengthA = def.LengthA;
            _lengthB = def.LengthB;

            Debug.Assert(!def.Ratio.Equals(0.0f));
            _ratio = def.Ratio;

            _constant = def.LengthA + _ratio * def.LengthB;

            _impulse = 0.0f;
        }

        /// Get the first ground anchor.
        public Vector2 GetGroundAnchorA()
        {
            return _groundAnchorA;
        }

        /// Get the second ground anchor.
        public Vector2 GetGroundAnchorB()
        {
            return _groundAnchorB;
        }

        /// Get the current length of the segment attached to bodyA.
        public float GetLengthA()
        {
            return _lengthA;
        }

        /// Get the current length of the segment attached to bodyB.
        public float GetLengthB()
        {
            return _lengthB;
        }

        /// Get the pulley ratio.
        public float GetRatio()
        {
            return _ratio;
        }

        /// Get the current length of the segment attached to bodyA.
        public float GetCurrentLengthA()
        {
            var p = BodyA.GetWorldPoint(_localAnchorA);
            var s = _groundAnchorA;
            var d = p - s;
            return d.Length();
        }

        /// Get the current length of the segment attached to bodyB.
        public float GetCurrentLengthB()
        {
            var p = BodyB.GetWorldPoint(_localAnchorB);
            var s = _groundAnchorB;
            var d = p - s;
            return d.Length();
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
            var P = _impulse * _uB;
            return inv_dt * P;
        }

        /// <inheritdoc />
        public override float GetReactionTorque(float inv_dt)
        {
            return 0.0f;
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

            // Get the pulley axes.
            _uA = cA + _rA - _groundAnchorA;
            _uB = cB + _rB - _groundAnchorB;

            var lengthA = _uA.Length();
            var lengthB = _uB.Length();

            if (lengthA > 10.0f * Settings.LinearSlop)
            {
                _uA *= 1.0f / lengthA;
            }
            else
            {
                _uA.SetZero();
            }

            if (lengthB > 10.0f * Settings.LinearSlop)
            {
                _uB *= 1.0f / lengthB;
            }
            else
            {
                _uB.SetZero();
            }

            // Compute effective mass.
            var ruA = MathUtils.Cross(_rA, _uA);
            var ruB = MathUtils.Cross(_rB, _uB);

            var mA = _invMassA + _invIa * ruA * ruA;
            var mB = _invMassB + _invIb * ruB * ruB;

            _mass = mA + _ratio * _ratio * mB;

            if (_mass > 0.0f)
            {
                _mass = 1.0f / _mass;
            }

            if (data.Step.WarmStarting)
            {
                // Scale impulses to support variable time steps.
                _impulse *= data.Step.DtRatio;

                // Warm starting.
                var PA = -_impulse * _uA;
                var PB = -_ratio * _impulse * _uB;

                vA += _invMassA * PA;
                wA += _invIa * MathUtils.Cross(_rA, PA);
                vB += _invMassB * PB;
                wB += _invIb * MathUtils.Cross(_rB, PB);
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

            var vpA = vA + MathUtils.Cross(wA, _rA);
            var vpB = vB + MathUtils.Cross(wB, _rB);

            var Cdot = -Vector2.Dot(_uA, vpA) - _ratio * Vector2.Dot(_uB, vpB);
            var impulse = -_mass * Cdot;
            _impulse += impulse;

            var PA = -impulse * _uA;
            var PB = -_ratio * impulse * _uB;
            vA += _invMassA * PA;
            wA += _invIa * MathUtils.Cross(_rA, PA);
            vB += _invMassB * PB;
            wB += _invIb * MathUtils.Cross(_rB, PB);

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

            // Get the pulley axes.
            var uA = cA + rA - _groundAnchorA;
            var uB = cB + rB - _groundAnchorB;

            var lengthA = uA.Length();
            var lengthB = uB.Length();

            if (lengthA > 10.0f * Settings.LinearSlop)
            {
                uA *= 1.0f / lengthA;
            }
            else
            {
                uA.SetZero();
            }

            if (lengthB > 10.0f * Settings.LinearSlop)
            {
                uB *= 1.0f / lengthB;
            }
            else
            {
                uB.SetZero();
            }

            // Compute effective mass.
            var ruA = MathUtils.Cross(rA, uA);
            var ruB = MathUtils.Cross(rB, uB);

            var mA = _invMassA + _invIa * ruA * ruA;
            var mB = _invMassB + _invIb * ruB * ruB;

            var mass = mA + _ratio * _ratio * mB;

            if (mass > 0.0f)
            {
                mass = 1.0f / mass;
            }

            var C = _constant - lengthA - _ratio * lengthB;
            var linearError = Math.Abs(C);

            var impulse = -mass * C;

            var PA = -impulse * uA;
            var PB = -_ratio * impulse * uB;

            cA += _invMassA * PA;
            aA += _invIa * MathUtils.Cross(rA, PA);
            cB += _invMassB * PB;
            aB += _invIb * MathUtils.Cross(rB, PB);

            data.Positions[_indexA].Center = cA;
            data.Positions[_indexA].Angle = aA;
            data.Positions[_indexB].Center = cB;
            data.Positions[_indexB].Angle = aB;

            return linearError < Settings.LinearSlop;
        }

        /// <inheritdoc />
        public override void Dump()
        {
            var indexA = BodyA.IslandIndex;
            var indexB = BodyB.IslandIndex;

            DumpLogger.Log("  b2PulleyJointDef jd;");
            DumpLogger.Log($"  jd.bodyA = bodies[{indexA}];");
            DumpLogger.Log($"  jd.bodyB = bodies[{indexB}];");
            DumpLogger.Log($"  jd.collideConnected = bool({CollideConnected});");
            DumpLogger.Log($"  jd.groundAnchorA.Set({_groundAnchorA.X}, {_groundAnchorA.Y});");
            DumpLogger.Log($"  jd.groundAnchorB.Set({_groundAnchorB.X}, {_groundAnchorB.Y});");
            DumpLogger.Log($"  jd.localAnchorA.Set({_localAnchorA.X}, {_localAnchorA.Y});");
            DumpLogger.Log($"  jd.localAnchorB.Set({_localAnchorB.X}, {_localAnchorB.Y});");
            DumpLogger.Log($"  jd.lengthA = {_lengthA};");
            DumpLogger.Log($"  jd.lengthB = {_lengthB};");
            DumpLogger.Log($"  jd.ratio = {_ratio};");
            DumpLogger.Log($"  joints[{Index}] = m_world.CreateJoint(&jd);");
        }

        /// <inheritdoc />
        public override void ShiftOrigin(in Vector2 newOrigin)
        {
            _groundAnchorA -= newOrigin;
            _groundAnchorB -= newOrigin;
        }
    }
}