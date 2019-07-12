using System.Numerics;
using Box2DSharp.Common;

namespace Box2DSharp.Dynamics.Joints
{
    /// Distance joint definition. This requires defining an
    /// anchor point on both bodies and the non-zero length of the
    /// distance joint. The definition uses local anchor points
    /// so that the initial configuration can violate the constraint
    /// slightly. This helps when saving and loading a game.
    /// @warning Do not use a zero or short length.
    public class DistanceJointDef : JointDef
    {
        /// The damping ratio. 0 = no damping, 1 = critical damping.
        public float DampingRatio;

        /// The mass-spring-damper frequency in Hertz. A value of 0
        /// disables softness.
        public float FrequencyHz;

        /// The natural length between the anchor points.
        public float Length;

        /// The local anchor point relative to bodyA's origin.
        public Vector2 LocalAnchorA;

        /// The local anchor point relative to bodyB's origin.
        public Vector2 LocalAnchorB;

        public DistanceJointDef()
        {
            JointType = JointType.DistanceJoint;
            LocalAnchorA.Set(0.0f, 0.0f);
            LocalAnchorB.Set(0.0f, 0.0f);
            Length = 1.0f;
            FrequencyHz = 0.0f;
            DampingRatio = 0.0f;
        }

        /// Initialize the bodies, anchors, and length using the world
        /// anchors.
        public void Initialize(
            Body b1,
            Body b2,
            in Vector2 anchor1,
            in Vector2 anchor2)
        {
            BodyA = b1;
            BodyB = b2;
            LocalAnchorA = BodyA.GetLocalPoint(anchor1);
            LocalAnchorB = BodyB.GetLocalPoint(anchor2);
            var d = anchor2 - anchor1;
            Length = d.Length();
        }
    }
}