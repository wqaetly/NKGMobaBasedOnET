using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using System.Xml.XPath;
using Box2DSharp.Common;

namespace Box2DSharp.Dynamics.Joints
{
    public enum JointType
    {
        UnknownJoint,

        RevoluteJoint,

        PrismaticJoint,

        DistanceJoint,

        PulleyJoint,

        MouseJoint,

        GearJoint,

        WheelJoint,

        WeldJoint,

        FrictionJoint,

        RopeJoint,

        MotorJoint
    }

    public enum LimitState
    {
        InactiveLimit,

        AtLowerLimit,

        AtUpperLimit,

        EqualLimits
    }

    /// A joint edge is used to connect bodies and joints together
    /// in a joint graph where each body is a node and each joint
    /// is an edge. A joint edge belongs to a doubly linked list
    /// maintained in each attached body. Each joint has two joint
    /// nodes, one for each attached body.
    public struct JointEdge : IDisposable
    {
        /// provides quick access to the other body attached.
        public Body Other;

        /// the joint
        public Joint Joint;

        public LinkedListNode<JointEdge> Node;

        /// <inheritdoc />
        public void Dispose()
        {
            Other = null;
            Joint = null;
            Node = null;
        }
    }

    /// Joint definitions are used to construct joints.
    public class JointDef : IDisposable
    {
        /// The first attached body.
        public Body BodyA;

        /// The second attached body.
        public Body BodyB;

        /// Set this flag to true if the attached bodies should collide.
        public bool CollideConnected;

        /// The joint type is set automatically for concrete joint types.
        public JointType JointType = JointType.UnknownJoint;

        /// Use this to attach application specific data to your joints.
        public object UserData;

        /// <inheritdoc />
        public virtual void Dispose()
        {
            BodyA = null;
            BodyB = null;
            UserData = null;
        }
    }

    public abstract class Joint
    {
        /// <summary>
        /// A??????
        /// </summary>
        public Body BodyA;

        /// <summary>
        /// B??????
        /// </summary>
        public Body BodyB;

        public readonly bool CollideConnected;

        /// <summary>
        /// ??????A???
        /// </summary>
        public JointEdge EdgeA;

        /// <summary>
        /// ??????B???
        /// </summary>
        public JointEdge EdgeB;

        /// <summary>
        /// ???????????????,?????????Dump
        /// </summary>
        public int Index;

        /// <summary>
        /// ????????????????????????????????????
        /// </summary>
        public bool IslandFlag;

        /// <summary>
        /// ??????????????????
        /// </summary>
        public LinkedListNode<Joint> Node;

        /// <summary>
        /// ????????????
        /// </summary>
        public object UserData;

        internal Joint(JointDef def)
        {
            Debug.Assert(def.BodyA != def.BodyB);

            JointType = def.JointType;
            BodyA = def.BodyA;
            BodyB = def.BodyB;
            Index = 0;
            CollideConnected = def.CollideConnected;
            IslandFlag = false;
            UserData = def.UserData;
            EdgeA = new JointEdge();
            EdgeB = new JointEdge();
        }

        /// Get the next joint the world joint list.
        /// Short-cut function to determine if either body is inactive.
        public bool IsEnabled => BodyA.IsEnabled && BodyB.IsEnabled;

        /// Get collide connected.
        /// Note: modifying the collide connect flag won't work correctly because
        /// the flag is only checked when fixture AABBs begin to overlap.
        public bool IsCollideConnected => CollideConnected;

        /// <summary>
        /// ????????????
        /// </summary>
        public JointType JointType { get; }

        /// Get the anchor point on bodyA in world coordinates.
        public abstract Vector2 GetAnchorA();

        /// Get the anchor point on bodyB in world coordinates.
        public abstract Vector2 GetAnchorB();

        /// Get the reaction force on bodyB at the joint anchor in Newtons.
        public abstract Vector2 GetReactionForce(float inv_dt);

        /// Get the reaction torque on bodyB in N*m.
        public abstract float GetReactionTorque(float inv_dt);

        /// Dump this joint to the log file.
        public virtual void Dump()
        {
            DumpLogger.Log("// Dump is not supported for this joint type.\n");
        }

        /// Shift the origin for any points stored in world coordinates.
        public virtual void ShiftOrigin(in Vector2 newOrigin)
        { }

        /// <summary>
        /// /// Debug draw this joint
        /// </summary>
        /// <param name="drawer"></param>
        public virtual void Draw(IDrawer drawer)
        {
            var xf1 = BodyA.GetTransform();
            var xf2 = BodyB.GetTransform();
            var x1 = xf1.Position;
            var x2 = xf2.Position;
            var p1 = GetAnchorA();
            var p2 = GetAnchorB();

            var color = Color.FromArgb(0.5f, 0.8f, 0.8f);

            switch (JointType)
            {
            case JointType.DistanceJoint:
                drawer.DrawSegment(p1, p2, color);
                break;

            case JointType.PulleyJoint:
            {
                var pulley = (PulleyJoint)this;
                var s1 = pulley.GetGroundAnchorA();
                var s2 = pulley.GetGroundAnchorB();
                drawer.DrawSegment(s1, p1, color);
                drawer.DrawSegment(s2, p2, color);
                drawer.DrawSegment(s1, s2, color);
            }
                break;

            case JointType.MouseJoint:
            {
                var c = Color.FromArgb(0.0f, 1.0f, 0.0f);
                drawer.DrawPoint(p1, 4.0f, c);
                drawer.DrawPoint(p2, 4.0f, c);

                drawer.DrawSegment(p1, p2, Color.FromArgb(0.8f, 0.8f, 0.8f));
            }
                break;

            default:
                drawer.DrawSegment(x1, p1, color);
                drawer.DrawSegment(p1, p2, color);
                drawer.DrawSegment(x2, p2, color);
                break;
            }
        }

        internal abstract void InitVelocityConstraints(in SolverData data);

        internal abstract void SolveVelocityConstraints(in SolverData data);

        // This returns true if the position errors are within tolerance.
        internal abstract bool SolvePositionConstraints(in SolverData data);

        internal static Joint Create(JointDef jointDef)
        {
            switch (jointDef)
            {
            case DistanceJointDef def:
                return new DistanceJoint(def);
            case WheelJointDef def:
                return new WheelJoint(def);
            case MouseJointDef def:
                return new MouseJoint(def);
            case WeldJointDef def:
                return new WeldJoint(def);
            case PulleyJointDef def:
                return new PulleyJoint(def);
            case RevoluteJointDef def:
                return new RevoluteJoint(def);
            case RopeJointDef def:
                return new RopeJoint(def);
            case FrictionJointDef def:
                return new FrictionJoint(def);
            case GearJointDef def:
                return new GearJoint(def);
            case MotorJointDef def:
                return new MotorJoint(def);
            case PrismaticJointDef def:
                return new PrismaticJoint(def);
            default:
                throw new ArgumentOutOfRangeException(nameof(jointDef.JointType));
            }
        }
    }
}