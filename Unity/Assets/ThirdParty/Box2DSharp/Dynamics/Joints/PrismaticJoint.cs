using System;
using System.Diagnostics;
using System.Numerics;
using Box2DSharp.Common;

namespace Box2DSharp.Dynamics.Joints
{
    /// A prismatic joint. This joint provides one degree of freedom: translation
    /// along an axis fixed in bodyA. Relative rotation is prevented. You can
    /// use a joint limit to restrict the range of motion and a joint motor to
    /// drive the motion or to model joint friction.
    ///
    /// Linear constraint (point-to-line)
    /// d = p2 - p1 = x2 + r2 - x1 - r1
    /// C = dot(perp, d)
    /// Cdot = dot(d, cross(w1, perp)) + dot(perp, v2 + cross(w2, r2) - v1 - cross(w1, r1))
    ///      = -dot(perp, v1) - dot(cross(d + r1, perp), w1) + dot(perp, v2) + dot(cross(r2, perp), v2)
    /// J = [-perp, -cross(d + r1, perp), perp, cross(r2,perp)]
    ///
    /// Angular constraint
    /// C = a2 - a1 + a_initial
    /// Cdot = w2 - w1
    /// J = [0 0 -1 0 0 1]
    ///
    /// K = J * invM * JT
    ///
    /// J = [-a -s1 a s2]
    ///     [0  -1  0  1]
    /// a = perp
    /// s1 = cross(d + r1, a) = cross(p2 - x1, a)
    /// s2 = cross(r2, a) = cross(p2 - x2, a)
    /// Motor/Limit linear constraint
    /// C = dot(ax1, d)
    /// Cdot = = -dot(ax1, v1) - dot(cross(d + r1, ax1), w1) + dot(ax1, v2) + dot(cross(r2, ax1), v2)
    /// J = [-ax1 -cross(d+r1,ax1) ax1 cross(r2,ax1)]
    /// Block Solver
    /// We develop a block solver that includes the joint limit. This makes the limit stiff (inelastic) even
    /// when the mass has poor distribution (leading to large torques about the joint anchor points).
    ///
    /// The Jacobian has 3 rows:
    /// J = [-uT -s1 uT s2] // linear
    ///     [0   -1   0  1] // angular
    ///     [-vT -a1 vT a2] // limit
    ///
    /// u = perp
    /// v = axis
    /// s1 = cross(d + r1, u), s2 = cross(r2, u)
    /// a1 = cross(d + r1, v), a2 = cross(r2, v)
    /// M * (v2 - v1) = JT * df
    /// J * v2 = bias
    ///
    /// v2 = v1 + invM * JT * df
    /// J * (v1 + invM * JT * df) = bias
    /// K * df = bias - J * v1 = -Cdot
    /// K = J * invM * JT
    /// Cdot = J * v1 - bias
    ///
    /// Now solve for f2.
    /// df = f2 - f1
    /// K * (f2 - f1) = -Cdot
    /// f2 = invK * (-Cdot) + f1
    ///
    /// Clamp accumulated limit impulse.
    /// lower: f2(3) = max(f2(3), 0)
    /// upper: f2(3) = min(f2(3), 0)
    ///
    /// Solve for correct f2(1:2)
    /// K(1:2, 1:2) * f2(1:2) = -Cdot(1:2) - K(1:2,3) * f2(3) + K(1:2,1:3) * f1
    ///                       = -Cdot(1:2) - K(1:2,3) * f2(3) + K(1:2,1:2) * f1(1:2) + K(1:2,3) * f1(3)
    /// K(1:2, 1:2) * f2(1:2) = -Cdot(1:2) - K(1:2,3) * (f2(3) - f1(3)) + K(1:2,1:2) * f1(1:2)
    /// f2(1:2) = invK(1:2,1:2) * (-Cdot(1:2) - K(1:2,3) * (f2(3) - f1(3))) + f1(1:2)
    ///
    /// Now compute impulse to be applied:
    /// df = f2 - f1
    public class PrismaticJoint : Joint
    {
        private readonly Vector2 _localYAxisA;

        // Solver shared
        internal readonly Vector2 LocalAnchorA;

        internal readonly Vector2 LocalAnchorB;

        internal readonly Vector2 LocalXAxisA;

        internal readonly float ReferenceAngle;

        private float _a1, _a2;

        private Vector2 _axis, _perp;

        private bool _enableLimit;

        private bool _enableMotor;

        private Vector3 _impulse;

        // Solver temp
        private int _indexA;

        private int _indexB;

        private float _invIa;

        private float _invIb;

        private float _invMassA;

        private float _invMassB;

        private Matrix3x3 _k;

        private LimitState _limitState;

        private Vector2 _localCenterA;

        private Vector2 _localCenterB;

        private float _lowerTranslation;

        private float _maxMotorForce;

        private float _motorImpulse;

        private float _motorMass;

        private float _motorSpeed;

        private float _s1, _s2;

        private float _upperTranslation;

        internal PrismaticJoint(PrismaticJointDef def) : base(def)
        {
            LocalAnchorA = def.LocalAnchorA;
            LocalAnchorB = def.LocalAnchorB;
            LocalXAxisA = def.LocalAxisA;
            LocalXAxisA = Vector2.Normalize(LocalXAxisA);
            _localYAxisA = MathUtils.Cross(1.0f, LocalXAxisA);
            ReferenceAngle = def.ReferenceAngle;

            _impulse.SetZero();
            _motorMass = 0.0f;
            _motorImpulse = 0.0f;

            _lowerTranslation = def.LowerTranslation;
            _upperTranslation = def.UpperTranslation;
            _maxMotorForce = def.MaxMotorForce;
            _motorSpeed = def.MotorSpeed;
            _enableLimit = def.EnableLimit;
            _enableMotor = def.EnableMotor;
            _limitState = LimitState.InactiveLimit;

            _axis.SetZero();
            _perp.SetZero();
        }

        /// The local anchor point relative to bodyA's origin.
        public Vector2 GetLocalAnchorA()
        {
            return LocalAnchorA;
        }

        /// The local anchor point relative to bodyB's origin.
        public Vector2 GetLocalAnchorB()
        {
            return LocalAnchorB;
        }

        /// The local joint axis relative to bodyA.
        public Vector2 GetLocalAxisA()
        {
            return LocalXAxisA;
        }

        /// Get the reference angle.
        public float GetReferenceAngle()
        {
            return ReferenceAngle;
        }

        /// Get the current joint translation, usually in meters.
        public float GetJointTranslation()
        {
            var pA = BodyA.GetWorldPoint(LocalAnchorA);
            var pB = BodyB.GetWorldPoint(LocalAnchorB);
            var d = pB - pA;
            var axis = BodyA.GetWorldVector(LocalXAxisA);

            var translation = Vector2.Dot(d, axis);
            return translation;
        }

        /// Get the current joint translation speed, usually in meters per second.
        public float GetJointSpeed()
        {
            var bA = BodyA;
            var bB = BodyB;

            var rA = MathUtils.Mul(bA.Transform.Rotation, LocalAnchorA - bA.Sweep.LocalCenter);
            var rB = MathUtils.Mul(bB.Transform.Rotation, LocalAnchorB - bB.Sweep.LocalCenter);
            var p1 = bA.Sweep.C + rA;
            var p2 = bB.Sweep.C + rB;
            var d = p2 - p1;
            var axis = MathUtils.Mul(bA.Transform.Rotation, LocalXAxisA);

            var vA = bA.LinearVelocity;
            var vB = bB.LinearVelocity;
            var wA = bA.AngularVelocity;
            var wB = bB.AngularVelocity;

            var speed = Vector2.Dot(d, MathUtils.Cross(wA, axis))
                      + Vector2.Dot(axis, vB + MathUtils.Cross(wB, rB) - vA - MathUtils.Cross(wA, rA));
            return speed;
        }

        /// Is the joint limit enabled?
        public bool IsLimitEnabled()
        {
            return _enableLimit;
        }

        /// Enable/disable the joint limit.
        public void EnableLimit(bool flag)
        {
            if (flag != _enableLimit)
            {
                BodyA.IsAwake = true;
                BodyB.IsAwake = true;
                _enableLimit = flag;
                _impulse.Z = 0.0f;
            }
        }

        /// Get the lower joint limit, usually in meters.
        public float GetLowerLimit()
        {
            return _lowerTranslation;
        }

        /// Get the upper joint limit, usually in meters.
        public float GetUpperLimit()
        {
            return _upperTranslation;
        }

        /// Set the joint limits, usually in meters.
        public void SetLimits(float lower, float upper)
        {
            Debug.Assert(lower <= upper);
            if (lower != _lowerTranslation || upper != _upperTranslation)
            {
                BodyA.IsAwake = true;
                BodyB.IsAwake = true;
                _lowerTranslation = lower;
                _upperTranslation = upper;
                _impulse.Z = 0.0f;
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

        /// Set the motor speed, usually in meters per second.
        public void SetMotorSpeed(float speed)
        {
            if (speed != _motorSpeed)
            {
                BodyA.IsAwake = true;
                BodyB.IsAwake = true;
                _motorSpeed = speed;
            }
        }

        /// Get the motor speed, usually in meters per second.
        public float GetMotorSpeed()
        {
            return _motorSpeed;
        }

        /// Set the maximum motor force, usually in N.
        public void SetMaxMotorForce(float force)
        {
            if (Math.Abs(force - _maxMotorForce) > 0.000001f)
            {
                BodyA.IsAwake = true;
                BodyB.IsAwake = true;
                _maxMotorForce = force;
            }
        }

        public float GetMaxMotorForce()
        {
            return _maxMotorForce;
        }

        /// Get the current motor force given the inverse time step, usually in N.
        public float GetMotorForce(float inv_dt)
        {
            return inv_dt * _motorImpulse;
        }

        /// <inheritdoc />
        public override Vector2 GetAnchorA()
        {
            return BodyA.GetWorldPoint(LocalAnchorA);
        }

        /// <inheritdoc />
        public override Vector2 GetAnchorB()
        {
            return BodyB.GetWorldPoint(LocalAnchorB);
        }

        /// <inheritdoc />
        public override Vector2 GetReactionForce(float inv_dt)
        {
            return inv_dt * (_impulse.X * _perp + (_motorImpulse + _impulse.Z) * _axis);
        }

        /// <inheritdoc />
        public override float GetReactionTorque(float inv_dt)
        {
            return inv_dt * _impulse.Y;
        }

        /// Dump to b2Log
        public override void Dump()
        { }

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

            // Compute the effective masses.
            var rA = MathUtils.Mul(qA, LocalAnchorA - _localCenterA);
            var rB = MathUtils.Mul(qB, LocalAnchorB - _localCenterB);
            var d = cB - cA + rB - rA;

            float mA = _invMassA, mB = _invMassB;
            float iA = _invIa, iB = _invIb;

            // Compute motor Jacobian and effective mass.
            {
                _axis = MathUtils.Mul(qA, LocalXAxisA);
                _a1 = MathUtils.Cross(d + rA, _axis);
                _a2 = MathUtils.Cross(rB, _axis);

                _motorMass = mA + mB + iA * _a1 * _a1 + iB * _a2 * _a2;
                if (_motorMass > 0.0f)
                {
                    _motorMass = 1.0f / _motorMass;
                }
            }

            // Prismatic constraint.
            {
                _perp = MathUtils.Mul(qA, _localYAxisA);

                _s1 = MathUtils.Cross(d + rA, _perp);
                _s2 = MathUtils.Cross(rB, _perp);

                var k11 = mA + mB + iA * _s1 * _s1 + iB * _s2 * _s2;
                var k12 = iA * _s1 + iB * _s2;
                var k13 = iA * _s1 * _a1 + iB * _s2 * _a2;
                var k22 = iA + iB;
                if (k22.Equals(0.0f))
                {
                    // For bodies with fixed rotation.
                    k22 = 1.0f;
                }

                var k23 = iA * _a1 + iB * _a2;
                var k33 = mA + mB + iA * _a1 * _a1 + iB * _a2 * _a2;

                _k.Ex.Set(k11, k12, k13);
                _k.Ey.Set(k12, k22, k23);
                _k.Ez.Set(k13, k23, k33);
            }

            // Compute motor and limit terms.
            if (_enableLimit)
            {
                var jointTranslation = Vector2.Dot(_axis, d);
                if (Math.Abs(_upperTranslation - _lowerTranslation) < 2.0f * Settings.LinearSlop)
                {
                    _limitState = LimitState.EqualLimits;
                }
                else if (jointTranslation <= _lowerTranslation)
                {
                    if (_limitState != LimitState.AtLowerLimit)
                    {
                        _limitState = LimitState.AtLowerLimit;
                        _impulse.Z = 0.0f;
                    }
                }
                else if (jointTranslation >= _upperTranslation)
                {
                    if (_limitState != LimitState.AtUpperLimit)
                    {
                        _limitState = LimitState.AtUpperLimit;
                        _impulse.Z = 0.0f;
                    }
                }
                else
                {
                    _limitState = LimitState.InactiveLimit;
                    _impulse.Z = 0.0f;
                }
            }
            else
            {
                _limitState = LimitState.InactiveLimit;
                _impulse.Z = 0.0f;
            }

            if (_enableMotor == false)
            {
                _motorImpulse = 0.0f;
            }

            if (data.Step.WarmStarting)
            {
                // Account for variable time step.
                _impulse *= data.Step.DtRatio;
                _motorImpulse *= data.Step.DtRatio;

                var P = _impulse.X * _perp + (_motorImpulse + _impulse.Z) * _axis;
                var LA = _impulse.X * _s1 + _impulse.Y + (_motorImpulse + _impulse.Z) * _a1;
                var LB = _impulse.X * _s2 + _impulse.Y + (_motorImpulse + _impulse.Z) * _a2;

                vA -= mA * P;
                wA -= iA * LA;

                vB += mB * P;
                wB += iB * LB;
            }
            else
            {
                _impulse.SetZero();
                _motorImpulse = 0.0f;
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

            float mA = _invMassA, mB = _invMassB;
            float iA = _invIa, iB = _invIb;

            // Solve linear motor constraint.
            if (_enableMotor && _limitState != LimitState.EqualLimits)
            {
                var Cdot = Vector2.Dot(_axis, vB - vA) + _a2 * wB - _a1 * wA;
                var impulse = _motorMass * (_motorSpeed - Cdot);
                var oldImpulse = _motorImpulse;
                var maxImpulse = data.Step.Dt * _maxMotorForce;
                _motorImpulse = MathUtils.Clamp(_motorImpulse + impulse, -maxImpulse, maxImpulse);
                impulse = _motorImpulse - oldImpulse;

                var P = impulse * _axis;
                var LA = impulse * _a1;
                var LB = impulse * _a2;

                vA -= mA * P;
                wA -= iA * LA;

                vB += mB * P;
                wB += iB * LB;
            }

            Vector2 Cdot1;
            Cdot1.X = Vector2.Dot(_perp, vB - vA) + _s2 * wB - _s1 * wA;
            Cdot1.Y = wB - wA;

            if (_enableLimit && _limitState != LimitState.InactiveLimit)
            {
                // Solve prismatic and limit constraint in block form.
                float Cdot2;
                Cdot2 = Vector2.Dot(_axis, vB - vA) + _a2 * wB - _a1 * wA;
                var Cdot = new Vector3(Cdot1.X, Cdot1.Y, Cdot2);

                var f1 = _impulse;
                var df = _k.Solve33(-Cdot);
                _impulse += df;

                if (_limitState == LimitState.AtLowerLimit)
                {
                    _impulse.Z = Math.Max(_impulse.Z, 0.0f);
                }
                else if (_limitState == LimitState.AtUpperLimit)
                {
                    _impulse.Z = Math.Min(_impulse.Z, 0.0f);
                }

                // f2(1:2) = invK(1:2,1:2) * (-Cdot(1:2) - K(1:2,3) * (f2(3) - f1(3))) + f1(1:2)
                var b = -Cdot1 - (_impulse.Z - f1.Z) * new Vector2(_k.Ez.X, _k.Ez.Y);
                var f2r = _k.Solve22(b) + new Vector2(f1.X, f1.Y);
                _impulse.X = f2r.X;
                _impulse.Y = f2r.Y;

                df = _impulse - f1;

                var P = df.X * _perp + df.Z * _axis;
                var LA = df.X * _s1 + df.Y + df.Z * _a1;
                var LB = df.X * _s2 + df.Y + df.Z * _a2;

                vA -= mA * P;
                wA -= iA * LA;

                vB += mB * P;
                wB += iB * LB;
            }
            else
            {
                // Limit is inactive, just solve the prismatic constraint in block form.
                var df = _k.Solve22(-Cdot1);
                _impulse.X += df.X;
                _impulse.Y += df.Y;

                var P = df.X * _perp;
                var LA = df.X * _s1 + df.Y;
                var LB = df.X * _s2 + df.Y;

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

        // A velocity based solver computes reaction forces(impulses) using the velocity constraint solver.Under this context,
        // the position solver is not there to resolve forces.It is only there to cope with integration error.
        //
        // Therefore, the pseudo impulses in the position solver do not have any physical meaning.Thus it is okay if they suck.
        //
        // We could take the active state from the velocity solver.However, the joint might push past the limit when the velocity
        // solver indicates the limit is inactive.
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

            // Compute fresh Jacobians
            var rA = MathUtils.Mul(qA, LocalAnchorA - _localCenterA);
            var rB = MathUtils.Mul(qB, LocalAnchorB - _localCenterB);
            var d = cB + rB - cA - rA;

            var axis = MathUtils.Mul(qA, LocalXAxisA);
            var a1 = MathUtils.Cross(d + rA, axis);
            var a2 = MathUtils.Cross(rB, axis);
            var perp = MathUtils.Mul(qA, _localYAxisA);

            var s1 = MathUtils.Cross(d + rA, perp);
            var s2 = MathUtils.Cross(rB, perp);

            var impulse = new Vector3();
            var C1 = new Vector2();
            C1.X = Vector2.Dot(perp, d);
            C1.Y = aB - aA - ReferenceAngle;

            var linearError = Math.Abs(C1.X);
            var angularError = Math.Abs(C1.Y);

            var active = false;
            var C2 = 0.0f;
            if (_enableLimit)
            {
                var translation = Vector2.Dot(axis, d);
                if (Math.Abs(_upperTranslation - _lowerTranslation) < 2.0f * Settings.LinearSlop)
                {
                    // Prevent large angular corrections
                    C2 = MathUtils.Clamp(
                        translation,
                        -Settings.MaxLinearCorrection,
                        Settings.MaxLinearCorrection);
                    linearError = Math.Max(linearError, Math.Abs(translation));
                    active = true;
                }
                else if (translation <= _lowerTranslation)
                {
                    // Prevent large linear corrections and allow some slop.
                    C2 = MathUtils.Clamp(
                        translation - _lowerTranslation + Settings.LinearSlop,
                        -Settings.MaxLinearCorrection,
                        0.0f);
                    linearError = Math.Max(linearError, _lowerTranslation - translation);
                    active = true;
                }
                else if (translation >= _upperTranslation)
                {
                    // Prevent large linear corrections and allow some slop.
                    C2 = MathUtils.Clamp(
                        translation - _upperTranslation - Settings.LinearSlop,
                        0.0f,
                        Settings.MaxLinearCorrection);
                    linearError = Math.Max(linearError, translation - _upperTranslation);
                    active = true;
                }
            }

            if (active)
            {
                var k11 = mA + mB + iA * s1 * s1 + iB * s2 * s2;
                var k12 = iA * s1 + iB * s2;
                var k13 = iA * s1 * a1 + iB * s2 * a2;
                var k22 = iA + iB;
                if (k22.Equals(0.0f))
                {
                    // For fixed rotation
                    k22 = 1.0f;
                }

                var k23 = iA * a1 + iB * a2;
                var k33 = mA + mB + iA * a1 * a1 + iB * a2 * a2;

                var K = new Matrix3x3();
                K.Ex.Set(k11, k12, k13);
                K.Ey.Set(k12, k22, k23);
                K.Ez.Set(k13, k23, k33);

                var C = new Vector3();
                C.X = C1.X;
                C.Y = C1.Y;
                C.Z = C2;

                impulse = K.Solve33(-C);
            }
            else
            {
                var k11 = mA + mB + iA * s1 * s1 + iB * s2 * s2;
                var k12 = iA * s1 + iB * s2;
                var k22 = iA + iB;
                if (k22.Equals(0.0f))
                {
                    k22 = 1.0f;
                }

                var K = new Matrix2x2();
                K.Ex.Set(k11, k12);
                K.Ey.Set(k12, k22);

                var impulse1 = K.Solve(-C1);
                impulse.X = impulse1.X;
                impulse.Y = impulse1.Y;
                impulse.Z = 0.0f;
            }

            var P = impulse.X * perp + impulse.Z * axis;
            var LA = impulse.X * s1 + impulse.Y + impulse.Z * a1;
            var LB = impulse.X * s2 + impulse.Y + impulse.Z * a2;

            cA -= mA * P;
            aA -= iA * LA;
            cB += mB * P;
            aB += iB * LB;

            data.Positions[_indexA].Center = cA;
            data.Positions[_indexA].Angle = aA;
            data.Positions[_indexB].Center = cB;
            data.Positions[_indexB].Angle = aB;

            return linearError <= Settings.LinearSlop && angularError <= Settings.AngularSlop;
        }
    }
}