//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年8月12日 17:59:14
//------------------------------------------------------------

using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace Plugins.NodeEditor
{
    public class B2S_PrefabData
    {
        [LabelText("相关碰撞结点id")]
        public List<long> colliderNodeIDs = new List<long>();
    }
}