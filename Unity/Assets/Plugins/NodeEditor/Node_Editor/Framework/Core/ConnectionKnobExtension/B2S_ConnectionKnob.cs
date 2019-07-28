//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年7月28日 23:36:27
//------------------------------------------------------------

using System.Collections.Generic;
using ETMode;
using Sirenix.OdinInspector;

namespace NodeEditorFramework
{
    public partial class ValueConnectionKnob
    {
        [InfoBox("如果想要删除某一条曲线，请前往曲线的发出者，而不是接受者",InfoMessageType.Error)]
        [LabelText("同Connections顺序，其连接的结点标识为")]
        public List<string> b2sInfo = new List<string>();
        
        [Button("尝试自动读取所连接结点信息", 25), GUIColor(0.4f, 0.8f, 1)]
        public void AddAllNodeData()
        {
            b2sInfo.Clear();
            foreach (var VARIABLE in this.connections)
            {
                this.b2sInfo.Add(VARIABLE.body.B2SCollisionRelation_GetNodeData().Flag);
            }
        }
    }
}