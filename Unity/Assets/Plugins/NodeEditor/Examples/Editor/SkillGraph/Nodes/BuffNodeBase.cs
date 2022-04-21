//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年9月23日 18:36:38
//------------------------------------------------------------

using ET;
using GraphProcessor;
using UnityEditor;
using UnityEngine;

namespace Plugins.NodeEditor
{
    public class BuffNodeBase: BaseNode
    {
        [Input("InputBuff", allowMultiple = true)]
        [HideInInspector]
        public BuffNodeBase PrevNode;
        
        [Output("OutputBuff", allowMultiple = true)]
        [HideInInspector]
        public BuffNodeBase NextNode;

        public override Color color => Color.green;

        public virtual void AutoAddLinkedBuffs()
        {
            
        }
        
        public virtual BuffNodeDataBase GetBuffNodeData()
        {
            return null;
        }
    }
}