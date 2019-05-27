//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年5月17日 20:57:44
//------------------------------------------------------------

using System.Collections.Generic;
using System.Runtime.Serialization;
using Sirenix.OdinInspector;

namespace ETModel
{
    public class BaseNodeData
    {
        [LabelText("此节点所归属的技能ID")]
        public int BelongToSkillId;

        [LabelText("此节点ID为")]
        public int NodeID;

        [LabelText("此节点前结点ID为")]
        public List<int> PreNodeIds = new List<int>();

        [LabelText("此节点后结点ID为")]
        public List<int> NextNodeIds = new List<int>();
    }
}