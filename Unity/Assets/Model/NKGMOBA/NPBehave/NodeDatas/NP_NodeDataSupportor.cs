//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年8月20日 19:36:15
//------------------------------------------------------------

using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;
using Sirenix.OdinInspector;

namespace ETModel
{
    public class NP_NodeDataSupportor
    {
        [LabelText("此行为树结点")]
        public long id;

        [LabelText("此行为树")]
        public NP_NodeDataBase MNpNodeDataBase;
    }
}