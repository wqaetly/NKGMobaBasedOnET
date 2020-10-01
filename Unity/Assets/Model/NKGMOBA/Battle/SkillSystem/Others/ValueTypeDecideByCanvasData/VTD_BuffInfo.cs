//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2020年9月17日 20:54:29
//------------------------------------------------------------

using Sirenix.OdinInspector;

namespace ETModel
{
    public class VTD_BuffInfo
    {
        [BoxGroup("Buff节点Id信息")]
        [HideLabel]
        public VTD_Id BuffNodeId;

        [LabelText("添加Buff层数")]
        public int Layers = 1;
    }
}