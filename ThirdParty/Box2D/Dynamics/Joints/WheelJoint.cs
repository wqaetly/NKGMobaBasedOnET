using System;
using System.Diagnostics;
using System.Numerics;
using Box2DSharp.Common;

namespace Box2DSharp.Dynamics.Joints
{
    /// <summary>
    /// A wheel joint. This joint provides two degrees of freedom: translation
    /// along an axis fixed in bodyA and rotation in the plane. In other words, it is a point to
    /// line constraint with a rotational motor and a linear spring/damper. The spring/damper is
    /// initialized upon creation. This joint is designed for vehicle suspensions.
    /// </summary>
    public class WheelJoint : Joint
    {
        private readonly Vector2 _localAnchorA;

        private readonly Vector2 _localAnchorB;

        private readonly Vector2 _localXAxisA;

        private readonly Vector2 _localYAxisA;

        private float _impulse;

        private float _motorImpulse;

        private float _springImpulse;

        private float _lowerImpulse;

        private float _upperImpulse;

        private float _translation;

        private float _lowerTranslation;

        private float _upperTranslation;

        private float _maxMotorTorque;

        private float _motorSpeed;

        private bool _enableLimit;

        private bool _enableMotor;

        private float _stiffness;

        private float _damping;

        // Solver temp
        private int _indexA;

        private int _indexB;

        private Vector2 _localCenterA;

        private Vector2 _localCenterB;

        private float _invMassA;

        private float _invMassB;

        private float _invIA;

        private float _invIB;

        private Vector2 _ax, _ay;

        private float _sAx, _sBx;

        private float _sAy, _sBy;

        private float _mass;

        private float _motorMass;

        private float _axialMass;

        private float _springMass;

        private float _bias;

        private float _gamma;

        internal WheelJoint(WheelJointDef def)
            : base(def)
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

            _axialMass = 0.0f;
            _lowerImpulse = 0.0f;
            _upperImpulse = 0.0f;
            _lowerTranslation = def.LowerTranslation;
            _upperTranslation = def.UpperTranslation;
            _enableLimit = def.EnableLimit;

            _maxMotorTorque = def.MaxMotorTorque;
            _motorSpeed = def.MotorSpeed;
            _enableMotor = def.EnableMotor;

            _bias = 0.0f;
            _gamma = 0.0f;

            _ax.SetZero();
            _ay.SetZero();

            _stiffness = def.Stiffness;
            _damping = def.Damping;
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

        /// Is the joint limit enabled?
        public bool IsLimitEnabled() => _enableLimit;

        /// Enable/disable the joint translation limit.
        public void EnableLimit(bool flag)
        {
            if (flag != _enableLimit)
            {
                BodyA.IsAwake = true;
                BodyB.IsAwake = true;
                _enableLimit = flag;
                _lowerImpulse = 0.0f;
                _upperImpulse = 0.0f;
            }
        }

        /// Get the lower joint translation limit, usually in meters.
        public float GetLowerLimit() => _lowerTranslation;

        /// Get the upper joint translation limit, usually in meters.
        public float GetUpperLimit() => _upperTranslation;

        /// Set the joint translation limits, usually in meters.
        public void SetLimits(float lower, float upper)
        {
            Debug.Assert(lower <= upper);
            if (!lower.Equals(_lowerTranslation) || !upper.Equals(_upperTranslation))
            {
                BodyA.IsAwake = true;
                BodyB.IsAwake = true;
                _lowerTranslation = lower;
                _upperTranslation = upper;
                _lowerImpulse = 0.0f;
                _upperImpulse = 0.0f;
            }
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
            if (!torque.Equals(_maxMotorTorque))
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

        /// Access spring stiffness
        public void SetStiffness(float stiffness) => _stiffness = stiffness;

        public float GetStiffness() => _stiffness;

        /// Access damping
        public void SetDamping(float damping) => _damping = damping;

        public float GetDamping() => _damping;

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
            throw new NotImplementedException();
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
            _invIA = BodyA.InverseInertia;
            _invIB = BodyB.InverseInertia;

            float mA = _invMassA, mB = _invMassB;
            float iA = _invIA, iB = _invIB;

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
            _ax = MathUtils.Mul(qA, _localXAxisA);
            _sAx = MathUtils.Cross(d + rA, _ax);
            _sBx = MathUtils.Cross(rB, _ax);

            var invMass = mA + mB + iA * _sAx * _sAx + iB * _sBx * _sBx;
            if (invMass > 0.0f)
            {
                _axialMass = 1.0f / invMass;
            }
            else
            {
                _axialMass = 0.0f;
            }

            _springMass = 0.0f;
            _bias = 0.0f;
            _gamma = 0.0f;

            if (_stiffness > 0.0f && invMass > 0.0f)
            {
                _springMass = 1.0f / invMass;

                var C = Vector2.Dot(d, _ax);

                // magic formulas
                var h = data.Step.Dt;
                _gamma = h * (_damping + h * _stiffness);
                if (_gamma > 0.0f)
                {
                    _gamma = 1.0f / _gamma;
                }

                _bias = C * h * _stiffness * _gamma;

                _springMass = invMass + _gamma;
                if (_springMass > 0.0f)
                {
                    _springMass = 1.0f / _springMass;
                }
            }
            else
            {
                _springImpulse = 0.0f;
            }

            if (_enableLimit)
            {
                _translation = Vector2.Dot(_ax, d);
            }
            else
            {
                _lowerImpulse = 0.0f;
                _upperImpulse = 0.0f;
            }

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

                var axialImpulse = _springImpulse + _lowerImpulse - _upperImpulse;
                var P = _impulse * _ay + axialImpulse * _ax;
                var LA = _impulse * _sAy + axialImpulse * _sAx + _motorImpulse;
                var LB = _impulse * _sBy + axialImpulse * _sBx + _motorImpulse;

                vA -= _invMassA * P;
                wA -= _invIA * LA;

                vB += _invMassB * P;
                wB += _invIB * LB;
            }
            else
            {
                _impulse = 0.0f;
                _springImpulse = 0.0f;
                _motorImpulse = 0.0f;
                _lowerImpulse = 0.0f;
                _upperImpulse = 0.0f;
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
            float iA = _invIA, iB = _invIB;

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
            if (_enableLimit)
            {
                // Lower limit
                {
                    var C = _translation - _lowerTranslation;
                    var Cdot = Vector2.Dot(_ax, vB - vA) + _sBx * wB - _sAx * wA;
                    var impulse = -_axialMass * (Cdot + Math.Max(C, 0.0f) * data.Step.InvDt);
                    var oldImpulse = _lowerImpulse;
                    _lowerImpulse = Math.Max(_lowerImpulse + impulse, 0.0f);
                    impulse = _lowerImpulse - oldImpulse;

                    var P = impulse * _ax;
                    var LA = impulse * _sAx;
                    var LB = impulse * _sBx;

                    vA -= mA * P;
                    wA -= iA * LA;
                    vB += mB * P;
                    wB += iB * LB;
                }

                // Upper limit
                // Note: signs are flipped to keep C positive when the constraint is satisfied.
                // This also keeps the impulse positive when the limit is active.
                {
                    var C = _upperTranslation - _translation;
                    var Cdot = Vector2.Dot(_ax, vA - vB) + _sAx * wA - _sBx * wB;
                    var impulse = -_axialMass * (Cdot + Math.Max(C, 0.0f) * data.Step.InvDt);
                    var oldImpulse = _upperImpulse;
                    _upperImpulse = Math.Max(_upperImpulse + impulse, 0.0f);
                    impulse = _upperImpulse - oldImpulse;

                    var P = impulse * _ax;
                    var LA = impulse * _sAx;
                    var LB = impulse * _sBx;

                    vA += mA * P;
                    wA += iA * LA;
                    vB -= mB * P;
                    wB -= iB * LB;
                }
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

            var linearError = 0.0f;

            if (_enableLimit)
            {
                var qA = new Rotation(aA);
                var qB = new Rotation(aB);

                var rA = MathUtils.Mul(qA, _localAnchorA - _localCenterA);
                var rB = MathUtils.Mul(qB, _localAnchorB - _localCenterB);
                var d = (cB - cA) + rB - rA;

                var ax = MathUtils.Mul(qA, _localXAxisA);
                var sAx = MathUtils.Cross(d + rA, _ax);
                var sBx = MathUtils.Cross(rB, _ax);

                var C = 0.0f;
                var translation = Vector2.Dot(ax, d);
                if (Math.Abs(_upperTranslation - _lowerTranslation) < 2.0f * Settings.LinearSlop)
                {
                    C = translation;
                }
                else if (translation <= _lowerTranslation)
                {
                    C = Math.Min(translation - _lowerTranslation, 0.0f);
                }
                else if (translation >= _upperTranslation)
                {
                    C = Math.Max(translation - _upperTranslation, 0.0f);
                }

                if (!C.Equals(0))
                {
                    var invMass = _invMassA + _invMassB + _invIA * sAx * sAx + _invIB * sBx * sBx;
                    var impulse = 0.0f;
                    if (!invMass.Equals(0))
                    {
                        impulse = -C / invMass;
                    }

                    var P = impulse * ax;
                    var LA = impulse * sAx;
                    var LB = impulse * sBx;

                    cA -= _invMassA * P;
                    aA -= _invIA * LA;
                    cB += _invMassB * P;
                    aB += _invIB * LB;

                    linearError = Math.Abs(C);
                }
            }

            // Solve perpendicular constraint
            {
                var qA = new Rotation(aA);
                var qB = new Rotation(aB);

                var rA = MathUtils.Mul(qA, _localAnchorA - _localCenterA);
                var rB = MathUtils.Mul(qB, _localAnchorB - _localCenterB);
                var d = (cB - cA) + rB - rA;

                var ay = MathUtils.Mul(qA, _localYAxisA);

                var sAy = MathUtils.Cross(d + rA, ay);
                var sBy = MathUtils.Cross(rB, ay);

                var C = Vector2.Dot(d, ay);

                var invMass = _invMassA + _invMassB + _invIA * _sAy * _sAy + _invIB * _sBy * _sBy;

                var impulse = 0.0f;
                if (!invMass.Equals(0))
                {
                    impulse = -C / invMass;
                }

                var P = impulse * ay;
                var LA = impulse * sAy;
                var LB = impulse * sBy;

                cA -= _invMassA * P;
                aA -= _invIA * LA;
                cB += _invMassB * P;
                aB += _invIB * LB;

                linearError = Math.Max(linearError, Math.Abs(C));
            }

            data.Positions[_indexA].Center = cA;
            data.Positions[_indexA].Angle = aA;
            data.Positions[_indexB].Center = cB;
            data.Positions[_indexB].Angle = aB;

            return linearError <= Settings.LinearSlop;
        }

        /// <inheritdoc />
        public override void Draw(IDrawer drawer)
        {
            var xfA = BodyA.GetTransform();
            var xfB = BodyB.GetTransform();
            var pA = MathUtils.Mul(xfA, _localAnchorA);
            var pB = MathUtils.Mul(xfB, _localAnchorB);

            var axis = MathUtils.Mul(xfA.Rotation, _localXAxisA);

            var c1 = Color.FromArgb(0.7f, 0.7f, 0.7f);
            var c2 = Color.FromArgb(0.3f, 0.9f, 0.3f);
            var c3 = Color.FromArgb(0.9f, 0.3f, 0.3f);
            var c4 = Color.FromArgb(0.3f, 0.3f, 0.9f);
            var c5 = Color.FromArgb(0.4f, 0.4f, 0.4f);

            drawer.DrawSegment(pA, pB, c5);

            if (_enableLimit)
            {
                var lower = pA + _lowerTranslation * axis;
                var upper = pA + _upperTranslation * axis;
                var perp = MathUtils.Mul(xfA.Rotation, _localYAxisA);
                drawer.DrawSegment(lower, upper, c1);
                drawer.DrawSegment(lower - 0.5f * perp, lower + 0.5f * perp, c2);
                drawer.DrawSegment(upper - 0.5f * perp, upper + 0.5f * perp, c3);
            }
            else
            {
                drawer.DrawSegment(pA - 1.0f * axis, pA + 1.0f * axis, c1);
            }

            drawer.DrawPoint(pA, 5.0f, c1);
            drawer.DrawPoint(pB, 5.0f, c4);
        }
    }
}