using ProtoBuf;

namespace ET
{
    [ProtoContract]
    public class LSF_CreateColliderCmd : ALSF_Cmd
    {
        public const uint CmdType = LSF_CmdType.CreateCollider;


        [ProtoMember(1)] public long BelongtoUnitId;

        [ProtoMember(2)] public long SelfId;

        [ProtoMember(3)] public int CollisionsRelationSupportIdInExcel;

        [ProtoMember(4)] public int ColliderNPBehaveTreeIdInExcel;

        /// <summary>
        /// 比如诺克释放了Q技能，这里如果为True，Q技能的碰撞体就会跟随诺克
        /// </summary>
        [ProtoMember(5)] public bool FollowUnitPos;

        [ProtoMember(6)] public bool FollowUnitRot;

        /// <summary>
        /// 只在跟随Unit时有效，因为不跟随Unit说明是世界空间的碰撞体，
        /// </summary>
        [ProtoMember(7)] public float OffsetX;
        [ProtoMember(8)] public float OffsetZ;

        /// <summary>
        /// 只在不跟随Unit时有效，跟随Unit代表使用BelongToUnit的Transform
        /// </summary>
        [ProtoMember(9)] public float Angle;

        /// <summary>
        /// 只在不跟随Unit时有效，因为不跟随Unit说明是世界空间的碰撞体，
        /// </summary>
        [ProtoMember(10)] public float TargetPosX;
        [ProtoMember(11)] public float TargetPosZ;

        public override ALSF_Cmd Init(long unitId)
        {
            this.LockStepStateFrameSyncDataType = CmdType;
            this.UnitId = unitId;

            return this;
        }

        public override void Clear()
        {
            base.Clear();
            ColliderNPBehaveTreeIdInExcel = 0;
            BelongtoUnitId = 0;
        }
    }
}