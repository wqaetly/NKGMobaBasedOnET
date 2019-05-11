using System;
using System.Numerics;
using Box2DSharp.Common;

namespace Box2DSharp.Dynamics.Joints
{
    /// A wheel joint. This joint provides two degrees of freedom: translation
    /// along an axis fixed in bodyA and rotation in the plane. In other words, it is a point to
    /// line constraint with a rotational motor and a linear spring/damper.
    /// This joint is designed for vehicle suspensions.
    public class WheelJoint : Joint
    {
        // Solver shared
        private readonly Vector2 _localAnchorA;

        private readonly Vector2 _localAnchorB;

        private readonly Vector2 _localXAxisA;

        private readonly Vector2 _localYAxisA;

        private Vector2 _ax, _ay;

        private float _bias;

        private float _dampingRatio;

        private bool _enableMotor;

        private float _frequencyHz;

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

        private float _maxMotorTorque;

        private float _motorImpulse;

        private float _motorMass;

        private float _motorSpeed;

        private float _sAx, _sBx;

        private float _sAy, _sBy;

        private float _springImpulse;

        private float _springMass;

        internal WheelJoint(WheelJointDef def) : base(def)
        {
            _localAnchorA = def.LocalAnchorA;
            _localAnchorB = def.LocalAnchorB;
            _localXAxisA = def.LocalAxisA;
            _localYAxisA = MathUtils.Cross(1.0f, _localXAxisA);

            _mass = 0.0f;
            _impulse = 0.0f;
            _motorMass = 0.0f;
            _motorImpulse = 0.0f;
            _springMass = 0.0f;
            _springImpulse = 0.0f;

            _maxMotorTorque = def.MaxMotorTorque;
            _motorSpeed = def.MotorSpeed;
            _enableMotor = def.EnableMotor;

            _frequencyHz = def.FrequencyHz;
            _dampingRatio = def.DampingRatio;

            _bias = 0.0f;
            _gamma = 0.0f;

            _ax.SetZero();
            _ay.SetZero();
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

        /// The local joint axis relative to bodyA.
        public Vector2 GetLocalAxisA()
        {
            return _localXAxisA;
        }

        /// Get the current joint translation, usually in meters.
        public float GetJointTranslation()
        {
            var bA = BodyA;
            var bB = BodyB;

            var pA = bA.GetWorldPoint(_localAnchorA);
            var pB = bB.GetWorldPoint(_localAnchorB);
            var d = pB - pA;
            var axis = bA.GetWorldVector(_localXAxisA);

            var translation = Vector2.Dot(d, axis);
            return translation;
        }

        /// Get the current joint linear speed, usually in meters per second.
        public float GetJointLinearSpeed()
        {
            var bA = BodyA;
            var bB = BodyB;

            var rA = MathUtils.Mul(bA.Transform.Rotation, _localAnchorA - bA.Sweep.LocalCenter);
            var rB = MathUtils.Mul(bB.Transform.Rotation, _localAnchorB - bB.Sweep.LocalCenter);
            var p1 = bA.Sweep.C + rA;
            var p2 = bB.Sweep.C + rB;
            var d = p2 - p1;
            var axis = MathUtils.Mul(bA.Transform.Rotation, _localXAxisA);

            var vA = bA.LinearVelocity;
            var vB = bB.LinearVelocity;
            var wA = bA.AngularVelocity;
            var wB = bB.AngularVelocity;

            var speed = Vector2.Dot(d, MathUtils.Cross(wA, axis))
                      + Vector2.Dot(axis, vB + MathUtils.Cross(wB, rB) - vA - MathUtils.Cross(wA, rA));
            return speed;
        }

        /// Get the current joint angle in radians.
        public float GetJointAngle()
        {
            var bA = BodyA;
            var bB = BodyB;
            return bB.Sweep.A - bA.Sweep.A;
        }

        /// Get the current joint angular speed in radians per second.
        public float GetJointAngularSpeed()
        {
            var wA = BodyA.AngularVelocity;
            var wB = BodyB.AngularVelocity;
            return wB - wA;
        }

        /// Is the joint motor enabled?
        public bool IsMotorEnabled()
        {
            return _enableMotor;
        }

        /// Enable/disable the joint motor.
        public void EnableMotor(bool flag)
        {
            if (flag != _enableMotor)
            {
                BodyA.IsAwake = true;
                BodyB.IsAwake = true;
                _enableMotor = flag;
            }
        }

        /// Set the motor speed, usually in radians per second.
        public void SetMotorSpeed(float speed)
        {
            if (!speed.Equals(_motorSpeed))
            {
                BodyA.IsAwake = true;
                BodyB.IsAwake = true;
                _motorSpeed = speed;
            }
        }

        /// Get the motor speed, usually in radians per second.
        public float GetMotorSpeed()
        {
            return _motorSpeed;
        }

        /// Set/Get the maximum motor force, usually in N-m.
        public void SetMaxMotorTorque(float torque)
        {
            if (torque != _maxMotorTorque)
            {
                BodyA.IsAwake = true;
                BodyB.IsAwake = true;
                _maxMotorTorque = torque;
            }
        }

        public float GetMaxMotorTorque()
        {
            return _maxMotorTorque;
        }

        /// Get the current motor torque given the inverse time step, usually in N-m.
        public float GetMotorTorque(float inv_dt)
        {
            return inv_dt * _motorImpulse;
        }

        /// Set/Get the spring frequency in hertz. Setting the frequency to zero disables the spring.
        public void SetSpringFrequencyHz(float hz)
        {
            _frequencyHz = hz;
        }

        public float GetSpringFrequencyHz()
        {
            return _frequencyHz;
        }

        /// Set/Get the spring damping ratio
        public void SetSpringDampingRatio(float ratio)
        {
            _dampingRatio = ratio;
        }

        public float GetSpringDampingRatio()
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
            return inv_dt * (_impulse * _ay + _springImpulse * _ax);
        }

        /// <inheritdoc />
        public override float GetReactionTorque(float inv_dt)
        {
            return inv_dt * _motorImpulse;
        }

        /// Dump to Logger.Log
        public override void Dump()
        {
            var indexA = BodyA.IslandIndex;
            var indexB = BodyB.IslandIndex;

            DumpLogger.Log("  b2WheelJointDef jd;");
            DumpLogger.Log($"  jd.bodyA = bodies[{indexA}];");
            DumpLogger.Log($"  jd.bodyB = bodies[{indexB}];");
            DumpLogger.Log($"  jd.collideConnected = bool({CollideConnected});");
            DumpLogger.Log($"  jd.localAnchorA.Set({_localAnchorA.X}, {_localAnchorA.Y});");
            DumpLogger.Log($"  jd.localAnchorB.Set({_localAnchorB.X}, {_localAnchorB.Y});");
            DumpLogger.Log($"  jd.localAxisA.Set({_localXAxisA.X}, {_localXAxisA.Y});");
            DumpLogger.Log($"  jd.enableMotor = bool({_enableMotor});");
            DumpLogger.Log($"  jd.motorSpeed = {_motorSpeed};");
            DumpLogger.Log($"  jd.maxMotorTorque = {_maxMotorTorque};");
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

            float mA = _invMassA, mB = _invMassB;
            float iA = _invIa, iB = _invIb;

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

            // Compute the effective masses.
            var rA = MathUtils.Mul(qA, _localAnchorA - _localCenterA);
            var rB = MathUtils.Mul(qB, _localAnchorB - _localCenterB);
            var d = cB + rB - cA - rA;

            // Point to line constraint
            {
                _ay = MathUtils.Mul(qA, _localYAxisA);
                _sAy = MathUtils.Cross(d + rA, _ay);
                _sBy = MathUtils.Cross(rB, _ay);

                _mass = mA + mB + iA * _sAy * _sAy + iB * _sBy * _sBy;

                if (_mass > 0.0f)
                {
                    _mass = 1.0f / _mass;
                }
            }

            // Spring constraint
            _springMass = 0.0f;
            _bias = 0.0f;
            _gamma = 0.0f;
            if (_frequencyHz > 0.0f)
            {
                _ax = MathUtils.Mul(qA, _localXAxisA);
                _sAx = MathUtils.Cross(d + rA, _ax);
                _sBx = MathUtils.Cross(rB, _ax);

                var invMass = mA + mB + iA * _sAx * _sAx + iB * _sBx * _sBx;

                if (invMass > 0.0f)
                {
                    _springMass = 1.0f / invMass;

                    var C = Vector2.Dot(d, _ax);

                    // Frequency
                    var omega = 2.0f * Settings.Pi * _frequencyHz;

                    // Damping coefficient
                    var damp = 2.0f * _springMass * _dampingRatio * omega;

                    // Spring stiffness
                    var k = _springMass * omega * omega;

                    // magic formulas
                    var h = data.Step.Dt;
                    _gamma = h * (damp + h * k);
                    if (_gamma > 0.0f)
                    {
                        _gamma = 1.0f / _gamma;
                    }

                    _bias = C * h * k * _gamma;

                    _springMass = invMass + _gamma;
                    if (_springMass > 0.0f)
                    {
                        _springMass = 1.0f / _springMass;
                    }
                }
            }
            else
            {
                _springImpulse = 0.0f;
            }

            // Rotational motor
            if (_enableMotor)
            {
                _motorMass = iA + iB;
                if (_motorMass > 0.0f)
                {
                    _motorMass = 1.0f / _motorMass;
                }
            }
            else
            {
                _motorMass = 0.0f;
                _motorImpulse = 0.0f;
            }

            if (data.Step.WarmStarting)
            {
                // Account for variable time step.
                _impulse *= data.Step.DtRatio;
                _springImpulse *= data.Step.DtRatio;
                _motorImpulse *= data.Step.DtRatio;

                var P = _impulse * _ay + _springImpulse * _ax;
                var LA = _impulse * _sAy + _springImpulse * _sAx + _motorImpulse;
                var LB = _impulse * _sBy + _springImpulse * _sBx + _motorImpulse;

                vA -= _invMassA * P;
                wA -= _invIa * LA;

                vB += _invMassB * P;
                wB += _invIb * LB;
            }
            else
            {
                _impulse = 0.0f;
                _springImpulse = 0.0f;
                _motorImpulse = 0.0f;
            }

            data.Velocities[_indexA].V = vA;
            data.Velocities[_indexA].W = wA;
            data.Velocities[_indexB].V = vB;
            data.Velocities[_indexB].W = wB;
        }

        /// <inheritdoc />
        internal override void SolveVelocityConstraints(in SolverData data)
        {
            float mA = _invMassA, mB = _invMassB;
            float iA = _invIa, iB = _invIb;

            var vA = data.Velocities[_indexA].V;
            var wA = data.Velocities[_indexA].W;
            var vB = data.Velocities[_indexB].V;
            var wB = data.Velocities[_indexB].W;

            // Solve spring constraint
            {
                var Cdot = Vector2.Dot(_ax, vB - vA) + _sBx * wB - _sAx * wA;
                var impulse = -_springMass * (Cdot + _bias + _gamma * _springImpulse);
                _springImpulse += impulse;

                var P = impulse * _ax;
                var LA = impulse * _sAx;
                var LB = impulse * _sBx;

                vA -= mA * P;
                wA -= iA * LA;

                vB += mB * P;
                wB += iB * LB;
            }

            // Solve rotational motor constraint
            {
                var Cdot = wB - wA - _motorSpeed;
                var impulse = -_motorMass * Cdot;

                var oldImpulse = _motorImpulse;
                var maxImpulse = data.Step.Dt * _maxMotorTorque;
                _motorImpulse = MathUtils.Clamp(_motorImpulse + impulse, -maxImpulse, maxImpulse);
                impulse = _motorImpulse - oldImpulse;

                wA -= iA * impulse;
                wB += iB * impulse;
            }

            // Solve point to line constraint
            {
                var Cdot = Vector2.Dot(_ay, vB - vA) + _sBy * wB - _sAy * wA;
                var impulse = -_mass * Cdot;
                _impulse += impulse;

                var P = impulse * _ay;
                var LA = impulse * _sAy;
                var LB = impulse * _sBy;

                vA -= mA * P;
                wA -= iA * LA;

                vB += mB * P;
                wB += iB * LB;
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

            var rA = MathUtils.Mul(qA, _localAnchorA - _localCenterA);
            var rB = MathUtils.Mul(qB, _localAnchorB - _localCenterB);
            var d = cB - cA + rB - rA;

            var ay = MathUtils.Mul(qA, _localYAxisA);

            var sAy = MathUtils.Cross(d + rA, ay);
            var sBy = MathUtils.Cross(rB, ay);

            var C = Vector2.Dot(d, ay);

            var k = _invMassA + _invMassB + _invIa * _sAy * _sAy + _invIb * _sBy * _sBy;

            float impulse;
            if (!k.Equals(0.0f))
            {
                impulse = -C / k;
            }
            else
            {
                impulse = 0.0f;
            }

            var P = impulse * ay;
            var LA = impulse * sAy;
            var LB = impulse * sBy;

            cA -= _invMassA * P;
            aA -= _invIa * LA;
            cB += _invMassB * P;
            aB += _invIb * LB;

            data.Positions[_indexA].Center = cA;
            data.Positions[_indexA].Angle = aA;
            data.Positions[_indexB].Center = cB;
            data.Positions[_indexB].Angle = aB;

            return Math.Abs(C) <= Settings.LinearSlop;
        }
    }
}