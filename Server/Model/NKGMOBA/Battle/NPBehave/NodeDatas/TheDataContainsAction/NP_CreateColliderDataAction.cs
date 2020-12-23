//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年9月27日 22:26:39
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using Sirenix.OdinInspector;

namespace ETModel
{
    [Title("创建一个碰撞体", TitleAlignment = TitleAlignments.Centered)]
    public class NP_CreateColliderAction: NP_ClassForStoreAction
    {
        [LabelText("碰撞体碰撞关系数据载体Id")]
        public int CollisionsRelationSupportIdInExcel;

        [InfoBox("在碰撞关系数据载体中的节点Id，而不是真正的碰撞体数据Id", InfoMessageType.Warning)]
        [LabelText("碰撞体数据节点Id")]
        public long CollisionRelationNodeDataId;

        [LabelText("碰撞体身上的行为树Id")]
        public int ColliderNPBehaveTreeIdInExcel;

        public override Action GetActionToBeDone()
        {
            this.Action = this.CreateColliderData;
            return this.Action;
        }

        public void CreateColliderData()
        {
            ConfigComponent configComponent = Game.Scene.GetComponent<ConfigComponent>();
            Log.Info("创建了碰撞体");
            Game.EventSystem.Run(EventIdType.CreateCollider, this.Unitid, this.CollisionsRelationSupportIdInExcel, this.CollisionRelationNodeDataId,
                this.ColliderNPBehaveTreeIdInExcel);
        }
    }
}