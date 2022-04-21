//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年9月27日 22:26:39
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ET
{
    /// <summary>
    /// 创建碰撞体Action
    /// 创建碰撞体会创建一个UnitA作为碰撞体的载体，然后为其添加碰撞相关组件让其变成一个游戏中的碰撞体
    /// 碰撞体一般都会有一个BelongToUnit，指向释放者
    /// Box2D碰撞体每帧都会同步UnitA的Transform信息作为自己在Box2D世界中的Transform信息
    /// 所以FollowUnit选项只是让我们决定UnitA的Transform是否每帧由BelongToUnit决定
    /// </summary>
    [Title("创建一个碰撞体", TitleAlignment = TitleAlignments.Centered)]
    public class NP_CreateColliderAction : NP_ClassForStoreAction
    {
        [LabelText("碰撞关系配置Id")] [Tooltip("Excel配置表中Id")]
        public int CollisionsRelationSupportIdInExcel;

        [LabelText("行为树配置表Id")] [Tooltip("Excel配置表中Id")]
        public int ColliderNPBehaveTreeIdInExcel;

        /// <summary>
        /// 比如诺克释放了Q技能，这里如果为True，Q技能的碰撞体就会跟随诺克
        /// </summary>
        [LabelText("Pos是否跟随释放的Unit")] public bool FollowUnitPos;

        [LabelText("Rot是否跟随释放的Unit")] public bool FollowUnitRot;

        /// <summary>
        /// 只在跟随Unit时有效，因为不跟随Unit说明是世界空间的碰撞体，
        /// </summary>
        [ShowIf(nameof(FollowUnitPos))] [LabelText("相对于释放者的偏移量")]
        public Vector3 Offset;

        /// <summary>
        /// 只在不跟随Unit时有效，跟随Unit代表使用BelongToUnit的Transform
        /// </summary>
        [HideIf(nameof(FollowUnitRot))] [LabelText("旋转角度")]
        public NP_BlackBoardRelationData Angle = new NP_BlackBoardRelationData();

        /// <summary>
        /// 只在不跟随Unit时有效，因为不跟随Unit说明是世界空间的碰撞体，
        /// </summary>
        [HideIf(nameof(FollowUnitPos))] [LabelText("目标位置")]
        public Vector3 TargetPos;

        public override Action GetActionToBeDone()
        {
            this.Action = this.CreateColliderData;
            return this.Action;
        }

        public void CreateColliderData()
        {
#if SERVER
            int colliderDataConfigId = Server_B2SCollisionRelationConfigCategory.Instance
                .Get(CollisionsRelationSupportIdInExcel)
                .B2S_ColliderConfigId;

            float angle = !this.FollowUnitRot
                ? Angle.GetBlackBoardValue<float>(this.BelongtoRuntimeTree.GetBlackboard())
                : 0;

            Unit colliderUnit = UnitFactory
                .CreateSpecialColliderUnit(BelongToUnit.BelongToRoom, BelongToUnit, colliderDataConfigId,
                    CollisionsRelationSupportIdInExcel, ColliderNPBehaveTreeIdInExcel, FollowUnitPos, FollowUnitRot,
                    Offset, TargetPos, angle);

            // 让客户端创建独特的Unit，用于承载行为树同步
            LSF_CreateColliderCmd lsfCreateColliderCmd =
                ReferencePool.Acquire<LSF_CreateColliderCmd>().Init(BelongToUnit.Id) as LSF_CreateColliderCmd;
            lsfCreateColliderCmd.SelfId = colliderUnit.Id;
            lsfCreateColliderCmd.BelongtoUnitId = BelongToUnit.Id;
            lsfCreateColliderCmd.CollisionsRelationSupportIdInExcel = CollisionsRelationSupportIdInExcel;
            lsfCreateColliderCmd.ColliderNPBehaveTreeIdInExcel = ColliderNPBehaveTreeIdInExcel;
            lsfCreateColliderCmd.Angle = angle;
            lsfCreateColliderCmd.OffsetX = Offset.x;
            lsfCreateColliderCmd.OffsetZ = Offset.z;
            lsfCreateColliderCmd.TargetPosX = TargetPos.x;
            lsfCreateColliderCmd.TargetPosZ = TargetPos.z;
            lsfCreateColliderCmd.FollowUnitPos = FollowUnitPos;
            lsfCreateColliderCmd.FollowUnitRot = FollowUnitRot;

            LSF_Component lsfComponent = BelongToUnit.BelongToRoom.GetComponent<LSF_Component>();
            lsfComponent.AddCmdToSendQueue(lsfCreateColliderCmd);
#endif
        }
    }
}