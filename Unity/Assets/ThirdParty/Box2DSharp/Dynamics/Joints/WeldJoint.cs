using System;
using System.Numerics;
using Box2DSharp.Common;

namespace Box2DSharp.Dynamics.Joints
{
    /// A weld joint essentially glues two bodies together. A weld joint may
    /// distort somewhat because the island constraint solver is approximate.
    public class WeldJoint : Joint
    {
        // Solver shared
        private readonly Vector2 _localAnchorA;

        private readonly Vector2 _localAnchorB;

        private readonly float _referenceAngle;

        private float _bias;

        private float _dampingRatio;

        private float _frequencyHz;

        private float _gamma;

        private Vector3 _impulse;

        // Solver temp
        private int _indexA;

        private int _indexB;

        private float _invIa;

        private float _invIb;

        private float _invMassA;

        private float _invMassB;

        private Vector2 _localCenterA;

        private Vector2 _localCenterB;

        private Matrix3x3 _mass;

        private Vector2 _rA;

        private Vector2 _rB;

        internal WeldJoint(WeldJointDef def) : base(def)
        {
            _localAnchorA = def.LocalAnchorA;
            _localAnchorB = def.LocalAnchorB;
            _referenceAngle = def.ReferenceAngle;
            _frequencyHz = def.FrequencyHz;
            _dampingRatio = def.DampingRatio;

            _impulse.SetZero();
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

        /// Get the reference angle.
        public float GetReferenceAngle()
        {
            return _referenceAngle;
        }

        /// Set/get frequency in Hz.
        public void SetFrequency(float hz)
        {
            _frequencyHz = hz;
        }

        public float GetFrequency()
        {
            return _frequencyHz;
        }

        /// Set/get damping ratio.
        public void SetDampingRatio(float ratio)
        {
            _dampingRatio = ratio;
        }

        public float GetDampingRatio()
        {
            return _dampingRatio;
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
            var P = new Vector2(_impulse.X, _impulse.Y);
            return inv_dt * P;
        }

        /// <inheritdoc />
        public override float GetReactionTorque(float inv_dt)
        {
            return inv_dt * _impulse.Z;
        }

        /// Dump to Logger.Log
        public override void Dump()
        {
            var indexA = BodyA.IslandIndex;
            var indexB = BodyB.IslandIndex;

            DumpLogger.Log("  b2WeldJointDef jd;");
            DumpLogger.Log($"  jd.bodyA = bodies[{indexA}];");
            DumpLogger.Log($"  jd.bodyB = bodies[{indexB}];");
            DumpLogger.Log($"  jd.collideConnected = bool({CollideConnected});");
            DumpLogger.Log($"  jd.localAnchorA.Set({_localAnchorA.X}, {_localAnchorA.Y});");
            DumpLogger.Log($"  jd.localAnchorB.Set({_localAnchorB.X}, {_localAnchorB.Y});");
            DumpLogger.Log($"  jd.referenceAngle = {_referenceAngle};");
            DumpLogger.Log($"  jd.frequencyHz = {_frequencyHz};");
            DumpLogger.Log($"  jd.dampingRatio = {_dampingRatio};");
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

            var aA = data.Positions[_indexA].Angle;
            var vA = data.Velocities[_indexA].V;
            var wA = data.Velocities[_indexA].W;

            var aB = data.Positions[_indexB].Angle;
            var vB = data.Velocities[_indexB].V;
            var wB = data.Velocities[_indexB].W;

            var qA = new Rotation(aA);
            var qB = new Rotation(aB);

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

            var K = new Matrix3x3();
            K.Ex.X = mA + mB + _rA.Y * _rA.Y * iA + _rB.Y * _rB.Y * iB;
            K.Ey.X = -_rA.Y * _rA.X * iA - _rB.Y * _rB.X * iB;
            K.Ez.X = -_rA.Y * iA - _rB.Y * iB;
            K.Ex.Y = K.Ey.X;
            K.Ey.Y = mA + mB + _rA.X * _rA.X * iA + _rB.X * _rB.X * iB;
            K.Ez.Y = _rA.X * iA + _rB.X * iB;
            K.Ex.Z = K.Ez.X;
            K.Ey.Z = K.Ez.Y;
            K.Ez.Z = iA + iB;

            if (_frequencyHz > 0.0f)
            {
                K.GetInverse22(ref _mass);

                var invM = iA + iB;
                var m = invM > 0.0f ? 1.0f / invM : 0.0f;

                var C = aB - aA - _referenceAngle;

                // Frequency
                var omega = 2.0f * Settings.Pi * _frequencyHz;

                // Damping coefficient
                var d = 2.0f * m * _dampingRatio * omega;

                // Spring stiffness
                var k = m * omega * omega;

                // magic formulas
                var h = data.Step.Dt;
                _gamma = h * (d + h * k);
                _gamma = !_gamma.Equals(0.0f) ? 1.0f / _gamma : 0.0f;
                _bias = C * h * k * _gamma;

                invM += _gamma;
                _mass.Ez.Z = !invM.Equals(0.0f) ? 1.0f / invM : 0.0f;
            }
            else if (K.Ez.Z.Equals(0.0f))
            {
                K.GetInverse22(ref _mass);
                _gamma = 0.0f;
                _bias = 0.0f;
            }
            else
            {
                K.GetSymInverse33(ref _mass);
                _gamma = 0.0f;
                _bias = 0.0f;
            }

            if (data.Step.WarmStarting)
            {
                // Scale impulses to support a variable time step.
                _impulse *= data.Step.DtRatio;

                var P = new Vector2(_impulse.X, _impulse.Y);

                vA -= mA * P;
                wA -= iA * (MathUtils.Cross(_rA, P) + _impulse.Z);

                vB += mB * P;
                wB += iB * (MathUtils.Cross(_rB, P) + _impulse.Z);
            }
            else
            {
                _impulse.SetZero();
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

            if (_frequencyHz > 0.0f)
            {
                var Cdot2 = wB - wA;

                var impulse2 = -_mass.Ez.Z * (Cdot2 + _bias + _gamma * _impulse.Z);
                _impulse.Z += impulse2;

                wA -= iA * impulse2;
                wB += iB * impulse2;

                var Cdot1 = vB + MathUtils.Cross(wB, _rB) - vA - MathUtils.Cross(wA, _rA);

                var impulse1 = -MathUtils.Mul22(_mass, Cdot1);
                _impulse.X += impulse1.X;
                _impulse.Y += impulse1.Y;

                var P = impulse1;

                vA -= mA * P;
                wA -= iA * MathUtils.Cross(_rA, P);

                vB += mB * P;
                wB += iB * MathUtils.Cross(_rB, P);
            }
            else
            {
                var cdot1 = vB + MathUtils.Cross(wB, _rB) - vA - MathUtils.Cross(wA, _rA);
                var cdot2 = wB - wA;
                var cdot = new Vector3(cdot1.X, cdot1.Y, cdot2);

                var impulse = -MathUtils.Mul(_mass, cdot);
                _impulse += impulse;

                var P = new Vector2(impulse.X, impulse.Y);

                vA -= mA * P;
                wA -= iA * (MathUtils.Cross(_rA, P) + impulse.Z);

                vB += mB * P;
                wB += iB * (MathUtils.Cross(_rB, P) + impulse.Z);
            }

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

            float mA = _invMassA, mB = _invMassB;
            float iA = _invIa, iB = _invIb;

            var rA = MathUtils.Mul(qA, _localAnchorA - _localCenterA);
            var rB = MathUtils.Mul(qB, _localAnchorB - _localCenterB);

            float positionError, angularError;

            var K = new Matrix3x3();
            K.Ex.X = mA + mB + rA.Y * rA.Y * iA + rB.Y * rB.Y * iB;
            K.Ey.X = -rA.Y * rA.X * iA - rB.Y * rB.X * iB;
            K.Ez.X = -rA.Y * iA - rB.Y * iB;
            K.Ex.Y = K.Ey.X;
            K.Ey.Y = mA + mB + rA.X * rA.X * iA + rB.X * rB.X * iB;
            K.Ez.Y = rA.X * iA + rB.X * iB;
            K.Ex.Z = K.Ez.X;
            K.Ey.Z = K.Ez.Y;
            K.Ez.Z = iA + iB;

            if (_frequencyHz > 0.0f)
            {
                var C1 = cB + rB - cA - rA;

                positionError = C1.Length();
                angularError = 0.0f;

                var P = -K.Solve22(C1);

                cA -= mA * P;
                aA -= iA * MathUtils.Cross(rA, P);

                cB += mB * P;
                aB += iB * MathUtils.Cross(rB, P);
            }
            else
            {
                var C1 = cB + rB - cA - rA;
                var C2 = aB - aA - _referenceAngle;

                positionError = C1.Length();
                angularError = Math.Abs(C2);

                var C = new Vector3(C1.X, C1.Y, C2);

                var impulse = new Vector3();
                if (K.Ez.Z > 0.0f)
                {
                    impulse = -K.Solve33(C);
                }
                else
                {
                    var impulse2 = -K.Solve22(C1);
                    impulse.Set(impulse2.X, impulse2.Y, 0.0f);
                }

                var P = new Vector2(impulse.X, impulse.Y);

                cA -= mA * P;
                aA -= iA * (MathUtils.Cross(rA, P) + impulse.Z);

                cB += mB * P;
                aB += iB * (MathUtils.Cross(rB, P) + impulse.Z);
            }

            data.Positions[_indexA].Center = cA;
            data.Positions[_indexA].Angle = aA;
            data.Positions[_indexB].Center = cB;
            data.Positions[_indexB].Angle = aB;

            return positionError <= Settings.LinearSlop && angularError <= Settings.AngularSlop;
        }
    }
}