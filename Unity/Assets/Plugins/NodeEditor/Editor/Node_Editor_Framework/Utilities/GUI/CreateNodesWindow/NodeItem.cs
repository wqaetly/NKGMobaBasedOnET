//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2020年8月16日 12:32:58
//------------------------------------------------------------

using UnityEditor.IMGUI.Controls;

namespace NodeEditorFramework.Utilities.CreateNodesWindow
{
    public class NodeItem: AdvancedDropdownItem
    {
        /// <summary>
        /// 完整名称，就相当于NodeTypeData的adres
        /// </summary>
        public string NodeId;

        public NodeItem(string name, string nodeId = ""): base(name)
        {
            this.NodeId = nodeId;
        }
    }
}