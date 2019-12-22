//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年12月10日 15:10:25
//------------------------------------------------------------

using NodeEditorFramework;
using NodeEditorFramework.Standard;
using UnityEditor;

namespace Plugins.NodeEditor.Editor.Canvas
{
    public static class CanvasOpenHelper
    {
        [MenuItem("Assets/Node_Editor_Framework/OpenThisFileInNodeEdtior",false,10000)]
        static void OpenThisFileInNodeEdtior()
        {
            string NodeCanvasPath = AssetDatabase.GetAssetPath((NodeCanvas)Selection.activeObject);
            NodeEditorWindow.OpenNodeEditor().canvasCache.LoadNodeCanvas(NodeCanvasPath);
        }
    }
}