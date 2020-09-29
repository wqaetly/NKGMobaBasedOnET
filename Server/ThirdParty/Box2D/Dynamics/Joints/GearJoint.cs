using System.Diagnostics;
using System.Numerics;
using Box2DSharp.Common;

namespace Box2DSharp.Dynamics.Joints
{
    /// A gear joint is used to connect two joints together. Either joint
    /// can be a revolute or prismatic joint. You specify a gear ratio
    /// to bind the motions together:
    /// coordinate1 + ratio * coordinate2 = constant
    /// The ratio can be negative or positive. If one joint is a revolute joint
    /// and the other joint is a prismatic joint, then the ratio will have units
    /// of length or units of 1/length.
    /// @warning You have to manually destroy the gear joint if joint1 or joint2
    /// is destroyed.
    public class GearJoint : Joint
    {
        // Body A is connected to body C
        // Body B is connected to body D
        private readonly Body _bodyC;

        private readonly Body _bodyD;

        private readonly float _constant;

        private readonly Joint _joint1;

        private readonly Joint _joint2;

        // Solver shared
        private readonly Vector2 _localAnchorA;

        private readonly Vector2 _localAnchorB;

        private readonly Vector2 _localAnchorC;

        private readonly Vector2 _localAnchorD;

        private readonly Vector2 _localAxisC;

        private readonly Vector2 _localAxisD;

        private readonly float _referenceAngleA;

        private readonly float _referenceAngleB;

        private readonly JointType _typeA;

        private readonly JointType _typeB;

        private float _iA, _iB, _iC, _iD;

        private float _impulse;

        // Solver temp
        private int _indexA, _indexB, _indexC, _indexD;

        private Vector2 _jvAc, _jvBd;

        private float _jwA, _jwB, _jwC, _jwD;

        private Vector2 _lcA, _lcB, _lcC, _lcD;

        private float _mA, _mB, _mC, _mD;

        private float _mass;

        private float _ratio;

        public GearJoint(GearJointDef def) : base(def)
        {
            _joint1 = def.Joint1;
            _joint2 = def.Joint2;

            _typeA = _joint1.JointType;
            _typeB = _joint2.JointType;

            Debug.Assert(_typeA == JointType.RevoluteJoint || _typeA == JointType.PrismaticJoint);
            Debug.Assert(_typeB == JointType.RevoluteJoint || _typeB == JointType.PrismaticJoint);

            float coordinateA, coordinateB;

            // TODO_ERIN there might be some problem with the joint edges in b2Joint.

            _bodyC = _joint1.BodyA;
            BodyA = _joint1.BodyB;

            // Get geometry of joint1
            var xfA = BodyA.Transform;
            var aA = BodyA.Sweep.A;
            var xfC = _bodyC.Transform;
            var aC = _bodyC.Sweep.A;

            if (_typeA == JointType.RevoluteJoint)
            {
                var revolute = (RevoluteJoint) def.Joint1;
                _localAnchorC = revolute.LocalAnchorA;
                _localAnchorA = revolute.LocalAnchorB;
                _referenceAngleA = revolute.ReferenceAngle;
                _localAxisC.SetZero();

                coordinateA = aA - aC - _referenceAngleA;
            }
            else
            {
                var prismatic = (PrismaticJoint) def.Joint1;
                _localAnchorC = prismatic.LocalAnchorA;
                _localAnchorA = prismatic.LocalAnchorB;
                _referenceAngleA = prismatic.ReferenceAngle;
                _localAxisC = prismatic.LocalXAxisA;

                var pC = _localAnchorC;
                var pA = MathUtils.MulT(
                    xfC.Rotation,
                    MathUtils.Mul(xfA.Rotation, _localAnchorA) + (xfA.Position - xfC.Position));
                coordinateA = Vector2.Dot(pA - pC, _localAxisC);
            }

            _bodyD = _joint2.BodyA;
            BodyB = _joint2.BodyB;

            // Get geometry of joint2
            var xfB = BodyB.Transform;
            var aB = BodyB.Sweep.A;
            var xfD = _bodyD.Transform;
            var aD = _bodyD.Sweep.A;

            if (_typeB == JointType.RevoluteJoint)
            {
                var revolute = (RevoluteJoint) def.Joint2;
                _localAnchorD = revolute.LocalAnchorA;
                _localAnchorB = revolute.LocalAnchorB;
                _referenceAngleB = revolute.ReferenceAngle;
                _localAxisD.SetZero();

                coordinateB = aB - aD - _referenceAngleB;
            }
            else
            {
                var prismatic = (PrismaticJoint) def.Joint2;
                _localAnchorD = prismatic.LocalAnchorA;
                _localAnchorB = prismatic.LocalAnchorB;
                _referenceAngleB = prismatic.ReferenceAngle;
                _localAxisD = prismatic.LocalXAxisA;

                var pD = _localAnchorD;
                var pB = MathUtils.MulT(
                    xfD.Rotation,
                    MathUtils.Mul(xfB.Rotation, _localAnchorB) + (xfB.Position - xfD.Position));
                coordinateB = Vector2.Dot(pB - pD, _localAxisD);
            }

            _ratio = def.Ratio;

            _constant = coordinateA + _ratio * coordinateB;

            _impulse = 0.0f;
        }

        /// Get the first joint.
        public Joint GetJoint1()
        {
            return _joint1;
        }

        /// Get the second joint.
        public Joint GetJoint2()
        {
            return _joint2;
        }

        /// Set/Get the gear ratio.
        public void SetRatio(float ratio)
        {
            Debug.Assert(ratio.IsValid());
            _ratio = ratio;
        }

        public float GetRatio()
        {
            return _ratio;
        }

        /// <inheritdoc />
        public override void Dump()
        {
            var indexA = BodyA.IslandIndex;
            var indexB = BodyB.IslandIndex;

            var index1 = _joint1.Index;
            var index2 = _joint2.Index;

            DumpLogger.Log("  b2GearJointDef jd;");
            DumpLogger.Log($"  jd.bodyA = bodies[{indexA}];");
            DumpLogger.Log($"  jd.bodyB = bodies[{indexB}];");
            DumpLogger.Log($"  jd.collideConnected = bool({CollideConnected});");
            DumpLogger.Log("  jd.joint1 = joints[index1];");
            DumpLogger.Log($"  jd.joint2 = joints[{index2}];");
            DumpLogger.Log($"  jd.ratio = {_ratio};");
            DumpLogger.Log($"  joints[{Index}] = m_world.CreateJoint(&jd);");
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
            var P = _impulse * _jvAc;
            return inv_dt * P;
        }

        /// <inheritdoc />
        public override float GetReactionTorque(float inv_dt)
        {
            var L = _impulse * _jwA;
            return inv_dt * L;
        }

        /// <inheritdoc />
        internal override void InitVelocityConstraints(in SolverData data)
        {
            _indexA = BodyA.IslandIndex;
            _indexB = BodyB.IslandIndex;
            _indexC = _bodyC.IslandIndex;
            _indexD = _bodyD.IslandIndex;
            _lcA = BodyA.Sweep.LocalCenter;
            _lcB = BodyB.Sweep.LocalCenter;
            _lcC = _bodyC.Sweep.LocalCenter;
            _lcD = _bodyD.Sweep.LocalCenter;
            _mA = BodyA.InvMass;
            _mB = BodyB.InvMass;
            _mC = _bodyC.InvMass;
            _mD = _bodyD.InvMass;
            _iA = BodyA.InverseInertia;
            _iB = BodyB.InverseInertia;
            _iC = _bodyC.InverseInertia;
            _iD = _bodyD.InverseInertia;

            var aA = data.Positions[_indexA].Angle;
            var vA = data.Velocities[_indexA].V;
            var wA = data.Velocities[_indexA].W;

            var aB = data.Positions[_indexB].Angle;
            var vB = data.Velocities[_indexB].V;
            var wB = data.Velocities[_indexB].W;

            var aC = data.Positions[_indexC].Angle;
            var vC = data.Velocities[_indexC].V;
            var wC = data.Velocities[_indexC].W;

            var aD = data.Positions[_indexD].Angle;
            var vD = data.Velocities[_indexD].V;
            var wD = data.Velocities[_indexD].W;

            Rotation qA = new Rotation(aA), qB = new Rotation(aB), qC = new Rotation(aC), qD = new Rotation(aD);

            _mass = 0.0f;

            if (_typeA == JointType.RevoluteJoint)
            {
                _jvAc.SetZero();
                _jwA = 1.0f;
                _jwC = 1.0f;
                _mass += _iA + _iC;
            }
            else
            {
                var u = MathUtils.Mul(qC, _localAxisC);
                var rC = MathUtils.Mul(qC, _localAnchorC - _lcC);
                var rA = MathUtils.Mul(qA, _localAnchorA - _lcA);
                _jvAc = u;
                _jwC = MathUtils.Cross(rC, u);
                _jwA = MathUtils.Cross(rA, u);
                _mass += _mC + _mA + _iC * _jwC * _jwC + _iA * _jwA * _jwA;
            }

            if (_typeB == JointType.RevoluteJoint)
            {
                _jvBd.SetZero();
                _jwB = _ratio;
                _jwD = _ratio;
                _mass += _ratio * _ratio * (_iB + _iD);
            }
            else
            {
                var u = MathUtils.Mul(qD, _localAxisD);
                var rD = MathUtils.Mul(qD, _localAnchorD - _lcD);
                var rB = MathUtils.Mul(qB, _localAnchorB - _lcB);
                _jvBd = _ratio * u;
                _jwD = _ratio * MathUtils.Cross(rD, u);
                _jwB = _ratio * MathUtils.Cross(rB, u);
                _mass += _ratio * _ratio * (_mD + _mB) + _iD * _jwD * _jwD + _iB * _jwB * _jwB;
            }

            // Compute effective mass.
            _mass = _mass > 0.0f ? 1.0f / _mass : 0.0f;

            if (data.Step.WarmStarting)
            {
                vA += _mA * _impulse * _jvAc;
                wA += _iA * _impulse * _jwA;
                vB += _mB * _impulse * _jvBd;
                wB += _iB * _impulse * _jwB;
                vC -= _mC * _impulse * _jvAc;
                wC -= _iC * _impulse * _jwC;
                vD -= _mD * _impulse * _jvBd;
                wD -= _iD * _impulse * _jwD;
            }
            else
            {
                _impulse = 0.0f;
            }

            data.Velocities[_indexA].V = vA;
            data.Velocities[_indexA].W = wA;
            data.Velocities[_indexB].V = vB;
            data.Velocities[_indexB].W = wB;
            data.Velocities[_indexC].V = vC;
            data.Velocities[_indexC].W = wC;
            data.Velocities[_indexD].V = vD;
            data.Velocities[_indexD].W = wD;
        }

        /// <inheritdoc />
        internal override void SolveVelocityConstraints(in SolverData data)
        {
            var vA = data.Velocities[_indexA].V;
            var wA = data.Velocities[_indexA].W;
            var vB = data.Velocities[_indexB].V;
            var wB = data.Velocities[_indexB].W;
            var vC = data.Velocities[_indexC].V;
            var wC = data.Velocities[_indexC].W;
            var vD = data.Velocities[_indexD].V;
            var wD = data.Velocities[_indexD].W;

            var Cdot = Vector2.Dot(_jvAc, vA - vC) + Vector2.Dot(_jvBd, vB - vD);
            Cdot += _jwA * wA - _jwC * wC + (_jwB * wB - _jwD * wD);

            var impulse = -_mass * Cdot;
            _impulse += impulse;

            vA += _mA * impulse * _jvAc;
            wA += _iA * impulse * _jwA;
            vB += _mB * impulse * _jvBd;
            wB += _iB * impulse * _jwB;
            vC -= _mC * impulse * _jvAc;
            wC -= _iC * impulse * _jwC;
            vD -= _mD * impulse * _jvBd;
            wD -= _iD * impulse * _jwD;

            data.Velocities[_indexA].V = vA;
            data.Velocities[_indexA].W = wA;
            data.Velocities[_indexB].V = vB;
            data.Velocities[_indexB].W = wB;
            data.Velocities[_indexC].V = vC;
            data.Velocities[_indexC].W = wC;
            data.Velocities[_indexD].V = vD;
            data.Velocities[_indexD].W = wD;
        }

        /// <inheritdoc />
        internal override bool SolvePositionConstraints(in SolverData data)
        {
            var cA = data.Positions[_indexA].Center;
            var aA = data.Positions[_indexA].Angle;
            var cB = data.Positions[_indexB].Center;
            var aB = data.Positions[_indexB].Angle;
            var cC = data.Positions[_indexC].Center;
            var aC = data.Positions[_indexC].Angle;
            var cD = data.Positions[_indexD].Center;
            var aD = data.Positions[_indexD].Angle;

            var qA = new Rotation(aA);
            var qB = new Rotation(aB);
            var qC = new Rotation(aC);
            var qD = new Rotation(aD);

            var linearError = 0.0f;

            float coordinateA, coordinateB;

            var JvAC = new Vector2();
            var JvBD = new Vector2();
            float JwA, JwB, JwC, JwD;
            var mass = 0.0f;

            if (_typeA == JointType.RevoluteJoint)
            {
                JvAC.SetZero();
                JwA = 1.0f;
                JwC = 1.0f;
                mass += _iA + _iC;

                coordinateA = aA - aC - _referenceAngleA;
            }
            else
            {
                var u = MathUtils.Mul(qC, _localAxisC);
                var rC = MathUtils.Mul(qC, _localAnchorC - _lcC);
                var rA = MathUtils.Mul(qA, _localAnchorA - _lcA);
                JvAC = u;
                JwC = MathUtils.Cross(rC, u);
                JwA = MathUtils.Cross(rA, u);
                mass += _mC + _mA + _iC * JwC * JwC + _iA * JwA * JwA;

                var pC = _localAnchorC - _lcC;
                var pA = MathUtils.MulT(qC, rA + (cA - cC));
                coordinateA = Vector2.Dot(pA - pC, _localAxisC);
            }

            if (_typeB == JointType.RevoluteJoint)
            {
                JvBD.SetZero();
                JwB = _ratio;
                JwD = _ratio;
                mass += _ratio * _ratio * (_iB + _iD);

                coordinateB = aB - aD - _referenceAngleB;
            }
            else
            {
                var u = MathUtils.Mul(qD, _localAxisD);
                var rD = MathUtils.Mul(qD, _localAnchorD - _lcD);
                var rB = MathUtils.Mul(qB, _localAnchorB - _lcB);
                JvBD = _ratio * u;
                JwD = _ratio * MathUtils.Cross(rD, u);
                JwB = _ratio * MathUtils.Cross(rB, u);
                mass += _ratio * _ratio * (_mD + _mB) + _iD * JwD * JwD + _iB * JwB * JwB;

                var pD = _localAnchorD - _lcD;
                var pB = MathUtils.MulT(qD, rB + (cB - cD));
                coordinateB = Vector2.Dot(pB - pD, _localAxisD);
            }

            var C = coordinateA + _ratio * coordinateB - _constant;

            var impulse = 0.0f;
            if (mass > 0.0f)
            {
                impulse = -C / mass;
            }

            cA += _mA * impulse * JvAC;
            aA += _iA * impulse * JwA;
            cB += _mB * impulse * JvBD;
            aB += _iB * impulse * JwB;
            cC -= _mC * impulse * JvAC;
            aC -= _iC * impulse * JwC;
            cD -= _mD * impulse * JvBD;
            aD -= _iD * impulse * JwD;

            data.Positions[_indexA].Center = cA;
            data.Positions[_indexA].Angle = aA;
            data.Positions[_indexB].Center = cB;
            data.Positions[_indexB].Angle = aB;
            data.Positions[_indexC].Center = cC;
            data.Positions[_indexC].Angle = aC;
            data.Positions[_indexD].Center = cD;
            data.Positions[_indexD].Angle = aD;

            // TODO_ERIN not implemented
            return linearError < Settings.LinearSlop;
        }
    }
}