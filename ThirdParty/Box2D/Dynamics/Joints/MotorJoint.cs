using System.Diagnostics;
using System.Numerics;
using Box2DSharp.Common;

namespace Box2DSharp.Dynamics.Joints
{
    /// A motor joint is used to control the relative motion
    /// between two bodies. A typical usage is to control the movement
    /// of a dynamic body with respect to the ground.
    public class MotorJoint : Joint
    {
        private float _angularError;

        private float _angularImpulse;

        private float _angularMass;

        private float _angularOffset;

        private float _correctionFactor;

        // Solver temp
        private int _indexA;

        private int _indexB;

        private float _invIa;

        private float _invIb;

        private float _invMassA;

        private float _invMassB;

        private Vector2 _linearError;

        private Vector2 _linearImpulse;

        private Matrix2x2 _linearMass;

        // Solver shared
        private Vector2 _linearOffset;

        private Vector2 _localCenterA;

        private Vector2 _localCenterB;

        private float _maxForce;

        private float _maxTorque;

        private Vector2 _rA;

        private Vector2 _rB;

        internal MotorJoint(MotorJointDef def) : base(def)
        {
            _linearOffset = def.LinearOffset;
            _angularOffset = def.AngularOffset;

            _linearImpulse.SetZero();
            _angularImpulse = 0.0f;

            _maxForce = def.MaxForce;
            _maxTorque = def.MaxTorque;
            _correctionFactor = def.CorrectionFactor;
        }

        /// Set/get the target linear offset, in frame A, in meters.
        public void SetLinearOffset(in Vector2 linearOffset)
        {
            if (!linearOffset.X.Equals(_linearOffset.X) || !linearOffset.Y.Equals(_linearOffset.Y))
            {
                BodyA.IsAwake = true;
                BodyB.IsAwake = true;
                _linearOffset = linearOffset;
            }
        }

        public Vector2 GetLinearOffset()
        {
            return _linearOffset;
        }

        /// Set/get the target angular offset, in radians.
        public void SetAngularOffset(float angularOffset)
        {
            if (!angularOffset.Equals(_angularOffset))
            {
                BodyA.IsAwake = true;
                BodyB.IsAwake = true;
                _angularOffset = angularOffset;
            }
        }

        public float GetAngularOffset()
        {
            return _angularOffset;
        }

        /// Set the maximum friction force in N.
        public void SetMaxForce(float force)
        {
            Debug.Assert(force.IsValid() && force >= 0.0f);
            _maxForce = force;
        }

        /// Get the maximum friction force in N.
        public float GetMaxForce()
        {
            return _maxForce;
        }

        /// Set the maximum friction torque in N*m.
        public void SetMaxTorque(float torque)
        {
            Debug.Assert(torque.IsValid() && torque >= 0.0f);
            _maxTorque = torque;
        }

        /// Get the maximum friction torque in N*m.
        public float GetMaxTorque()
        {
            return _maxTorque;
        }

        /// Set the position correction factor in the range [0,1].
        public void SetCorrectionFactor(float factor)
        {
            Debug.Assert(factor.IsValid() && 0.0f <= factor && factor <= 1.0f);
            _correctionFactor = factor;
        }

        /// Get the position correction factor in the range [0,1].
        public float GetCorrectionFactor()
        {
            return _correctionFactor;
        }

        /// <inheritdoc />
        public override Vector2 GetAnchorA()
        {
            return BodyA.GetPosition();
        }

        /// <inheritdoc />
        public override Vector2 GetAnchorB()
        {
            return BodyB.GetPosition();
        }

        /// <inheritdoc />
        public override Vector2 GetReactionForce(float invDt)
        {
            return invDt * _linearImpulse;
        }

        /// <inheritdoc />
        public override float GetReactionTorque(float invDt)
        {
            return invDt * _angularImpulse;
        }

        /// Dump to Logger.Log
        public override void Dump()
        {
            var indexA = BodyA.IslandIndex;
            var indexB = BodyB.IslandIndex;

            DumpLogger.Log("  b2MotorJointDef jd;");
            DumpLogger.Log($"  jd.bodyA = bodies[{indexA}];");
            DumpLogger.Log($"  jd.bodyB = bodies[{indexB}];");
            DumpLogger.Log($"  jd.collideConnected = bool({CollideConnected});");
            DumpLogger.Log($"  jd.linearOffset.Set({_linearOffset.X}, {_linearOffset.Y});");
            DumpLogger.Log($"  jd.angularOffset = {_angularOffset};");
            DumpLogger.Log($"  jd.maxForce = {_maxForce};");
            DumpLogger.Log($"  jd.maxTorque = {_maxTorque};");
            DumpLogger.Log($"  jd.correctionFactor = {_correctionFactor};");
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

            // Compute the effective mass matrix.
            _rA = MathUtils.Mul(qA, _linearOffset - _localCenterA);
            _rB = MathUtils.Mul(qB, -_localCenterB);

            // J = [-I -r1_skew I r2_skew]
            // r_skew = [-ry; rx]

            // Matlab
            // K = [ mA+r1y^2*iA+mB+r2y^2*iB,  -r1y*iA*r1x-r2y*iB*r2x,          -r1y*iA-r2y*iB]
            //     [  -r1y*iA*r1x-r2y*iB*r2x, mA+r1x^2*iA+mB+r2x^2*iB,           r1x*iA+r2x*iB]
            //     [          -r1y*iA-r2y*iB,           r1x*iA+r2x*iB,                   iA+iB]

            float mA = _invMassA, mB = _invMassB;
            float iA = _invIa, iB = _invIb;

            // Upper 2 by 2 of K for point to point
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

            _linearError = cB + _rB - cA - _rA;
            _angularError = aB - aA - _angularOffset;

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
            var invH = data.Step.InvDt;

            // Solve angular friction
            {
                var cdot = wB - wA + invH * _correctionFactor * _angularError;
                var impulse = -_angularMass * cdot;

                var oldImpulse = _angularImpulse;
                var maxImpulse = h * _maxTorque;
                _angularImpulse = MathUtils.Clamp(_angularImpulse + impulse, -maxImpulse, maxImpulse);
                impulse = _angularImpulse - oldImpulse;

                wA -= iA * impulse;
                wB += iB * impulse;
            }

            // Solve linear friction
            {
                var cdot = vB
                         + MathUtils.Cross(wB, _rB)
                         - vA
                         - MathUtils.Cross(wA, _rA)
                         + invH * _correctionFactor * _linearError;

                var impulse = -MathUtils.Mul(_linearMass, cdot);
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