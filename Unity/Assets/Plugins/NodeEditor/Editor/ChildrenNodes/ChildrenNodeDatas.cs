//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年12月22日 15:21:05
//------------------------------------------------------------

using System.Collections.Generic;
using NodeEditorFramework;
using NodeEditorFramework.Standard;
using Plugins.NodeEditor.Editor.Canvas;
using Sirenix.OdinInspector;
using UnityEditor;

namespace Plugins.NodeEditor.Editor.ChildrenNodes
{
    public class ChildrenNodeDatas
    {
        [LabelText("子图")]
        public NodeCanvas ChildrenCanvas;

        [Button("打开这个子图", 25), GUIColor(0.4f, 0.8f, 1)]
        public void OpenThisCanvas()
        {
            if (this.ChildrenCanvas != null)
            {
                //要确保保存成功才能加载目标图
                if (!NodeEditorWindow.editorInterface.AssertSavaCanvasSuccessfully())
                {
                    return;
                }

                string NodeCanvasPath = AssetDatabase.GetAssetPath(this.ChildrenCanvas);
                NodeEditorWindow.OpenNodeEditor().canvasCache.LoadNodeCanvas(NodeCanvasPath);
            }
        }
    }
}