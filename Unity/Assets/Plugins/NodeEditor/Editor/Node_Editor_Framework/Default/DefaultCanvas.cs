//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2020年1月10日 20:23:42
//------------------------------------------------------------

using System;
using ETModel;
using NodeEditorFramework;

namespace Plugins.NodeEditor.Node_Editor.Default
{
    [CannotShowInToolBarCanvasType]
    [NodeCanvasType("默认Canvas")]
    public class DefaultCanvas: NodeCanvas
    {
        public override string canvasName => Name;
        
        public string Name = "默认Canvas";
    }
}