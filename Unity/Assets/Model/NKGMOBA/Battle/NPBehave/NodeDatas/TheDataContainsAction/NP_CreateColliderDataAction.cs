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
        public long CollisionsRelationSupportId;

        [InfoBox("1xxxx为矩形，2xxxx为圆形，3xxxx为多边形")]
        [LabelText("碰撞体数据Id")]
        public long ColliderDataId;

        public override Action GetActionToBeDone()
        {
            this.Action = this.CreateColliderData;
            return this.Action;
        }

        public void CreateColliderData()
        {
            Game.EventSystem.Run(EventIdType.CreateCollider, this.Unitid, this.CollisionsRelationSupportId, this.ColliderDataId);
        }
    }
}