using System.Numerics;
using Box2DSharp.Common;

namespace Box2DSharp.Dynamics.Joints
{
    /// Wheel joint definition. This requires defining a line of
    /// motion using an axis and an anchor point. The definition uses local
    /// anchor points and a local axis so that the initial configuration
    /// can violate the constraint slightly. The joint translation is zero
    /// when the local anchor points coincide in world space. Using local
    /// anchors and a local axis helps when saving and loading a game.
    public class WheelJointDef : JointDef
    {
        /// The local anchor point relative to bodyA's origin.
        public Vector2 LocalAnchorA;

        /// The local anchor point relative to bodyB's origin.
        public Vector2 LocalAnchorB;

        /// The local translation axis in bodyA.
        public Vector2 LocalAxisA;

        /// Enable/disable the joint limit.
        public bool EnableLimit;

        /// The lower translation limit, usually in meters.
        public float LowerTranslation;

        /// The upper translation limit, usually in meters.
        public float UpperTranslation;

        /// Enable/disable the joint motor.
        public bool EnableMotor;

        /// The maximum motor torque, usually in N-m.
        public float MaxMotorTorque;

        /// The desired motor speed in radians per second.
        public float MotorSpeed;

        /// Suspension stiffness. Typically in units N/m.
        public float Stiffness;

        /// Suspension damping. Typically in units of N*s/m.
        public float Damping;

        public WheelJointDef()
        {
            JointType = JointType.WheelJoint;
            LocalAnchorA.SetZero();
            LocalAnchorB.SetZero();
            LocalAxisA.Set(1.0f, 0.0f);
            EnableLimit = false;
            LowerTranslation = 0.0f;
            UpperTranslation = 0.0f;
            EnableMotor = false;
            MaxMotorTorque = 0.0f;
            MotorSpeed = 0.0f;
            Stiffness = 0.0f;
            Damping = 0.0f;
        }

        /// Initialize the bodies, anchors, axis, and reference angle using the world
        /// anchor and world axis.
        public void Initialize(Body bA, Body bB, in Vector2 anchor, in Vector2 axis)
        {
            BodyA = bA;
            BodyB = bB;
            LocalAnchorA = BodyA.GetLocalPoint(anchor);
            LocalAnchorB = BodyB.GetLocalPoint(anchor);
            LocalAxisA = BodyA.GetLocalVector(axis);
        }
    }
}