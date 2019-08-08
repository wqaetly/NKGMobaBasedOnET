//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年7月25日 16:38:32
//------------------------------------------------------------

using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using Sirenix.OdinInspector;

namespace ETMode
{
    /// <summary>
    /// 碰撞实例
    /// </summary>
    public class B2S_CollisionInstance
    {        
        [LabelText("此结点标识")]
        [BsonIgnore]
        public string Flag;

        [LabelText("此结点ID")]
        public long nodeDataId;

        [LabelText("此结点所使用的碰撞体ID")]
        public List<long> collisionId;

        [LabelText("是否跟随Unit进行同步")]
        public bool FollowUnit;

        [InfoBox("此设置在碰撞事件分发那作为大分类依据（请前往Canvas处点击“自动配置所有Node数据”）")]
        [LabelText("此结点归属Group")]
        [DisableInEditorMode]
        public string BelongGroup;

        [LabelText("与此结点有碰撞关系的结点ID")]
        [BsonIgnore]
        [DisableInEditorMode]
        public List<long> CollisionRelations = new List<long>();
    }
}