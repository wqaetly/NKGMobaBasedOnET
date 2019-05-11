using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
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
    public struct JointEdge
    {
        /// provides quick access to the other body attached.
        public Body Other;

        /// the joint
        public Joint Joint;

        public LinkedListNode<JointEdge> Node;
    }

    /// Joint definitions are used to construct joints.
    public class JointDef
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
    }

    public abstract class Joint
    {
        /// <summary>
        /// A物体
        /// </summary>
        public Body BodyA;

        /// <summary>
        /// B物体
        /// </summary>
        public Body BodyB;

        public readonly bool CollideConnected;

        /// <summary>
        /// 关节A头
        /// </summary>
        public JointEdge EdgeA;

        /// <summary>
        /// 关节B头
        /// </summary>
        public JointEdge EdgeB;

        /// <summary>
        /// 关节索引值,只用于Dump
        /// </summary>
        public int Index;

        /// <summary>
        /// 当前关节是否已经在孤岛中
        /// </summary>
        public bool IslandFlag;

        /// <summary>
        /// 关节链表节点
        /// </summary>
        public LinkedListNode<Joint> Node;

        /// <summary>
        /// 用户数据
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
        public bool IsActive => BodyA.IsActive && BodyB.IsActive;

        /// Get collide connected.
        /// Note: modifying the collide connect flag won't work correctly because
        /// the flag is only checked when fixture AABBs begin to overlap.
        public bool IsCollideConnected => CollideConnected;

        /// <summary>
        /// 关节类型
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