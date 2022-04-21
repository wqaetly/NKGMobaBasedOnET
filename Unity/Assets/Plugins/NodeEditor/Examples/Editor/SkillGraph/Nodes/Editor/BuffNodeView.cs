//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2021年6月17日 20:39:20
//------------------------------------------------------------

using ET;
using GraphProcessor;
using UnityEngine.UIElements;

namespace Plugins.NodeEditor
{
    [NodeCustomEditor(typeof(BuffNodeBase))]
    public class BuffNodeView: BaseNodeView
    {
        public override void Enable()
        {
            BuffNodeDataBase nodeDataBase = (this.nodeTarget as BuffNodeBase).GetBuffNodeData();
            TextField textField = new TextField();
            if (nodeDataBase is NormalBuffNodeData normalBuffNodeData)
            {
                textField.value = normalBuffNodeData.BuffDes;
                textField.RegisterValueChangedCallback((changedDes) => { normalBuffNodeData.BuffDes = changedDes.newValue; });
            }
            else if(nodeDataBase is SkillDesNodeData skillDesNodeData)
            {
                textField.value = skillDesNodeData.SkillName;
            }
            textField.style.marginTop = 4;
            textField.style.marginBottom = 4;

            controlsContainer.Add(textField);
        }
    }
}