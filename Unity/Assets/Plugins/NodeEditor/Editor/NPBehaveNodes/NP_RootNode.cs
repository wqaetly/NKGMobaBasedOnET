//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年8月20日 8:00:48
//------------------------------------------------------------

using ETModel;
using NodeEditorFramework;
using Plugins.NodeEditor.Editor.Canvas;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace Plugins.NodeEditor.Editor.NPBehaveNodes
{
    [Node(false, "NPBehave行为树/根结点", typeof (NPBehaveCanvas))]
    public class NP_RootNode: NP_NodeBase
    {
        /// <summary>
        /// 内部ID
        /// </summary>
        private const string Id = "行为树根节点";

        /// <summary>
        /// 内部ID
        /// </summary>
        public override string GetID => Id;

        [LabelText("根结点数据")]
        public NP_RootNodeData MRootNodeData;

        public override NP_NodeDataBase NP_GetNodeData()
        {
            return this.MRootNodeData;
        }
        
        private void OnEnable()
        {
            if (MRootNodeData == null)
            {
                this.MRootNodeData = new NP_RootNodeData();
            }

            MRootNodeData.NodeDes = "根结点";
        }

        public override void NodeGUI()
        {
            EditorGUILayout.TextField(MRootNodeData.NodeDes);
        }
    }
}