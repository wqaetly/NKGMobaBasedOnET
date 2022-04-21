//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2021年6月18日 20:19:14
//------------------------------------------------------------

using ET;
using GraphProcessor;

namespace Plugins.NodeEditor
{
    public class UnitAttributesNodeBase: BaseNode
    {
        public override bool isRenamable => true;
        public virtual UnitAttributesNodeDataBase UnitAttributesData_GetNodeData()
        {
            return null;
        }
    }
}