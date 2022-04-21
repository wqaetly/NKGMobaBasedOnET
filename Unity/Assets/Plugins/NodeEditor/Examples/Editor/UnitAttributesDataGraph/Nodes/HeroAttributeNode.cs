//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2021年6月18日 20:19:35
//------------------------------------------------------------

using ET;
using GraphProcessor;

namespace Plugins.NodeEditor
{
    [NodeMenuItem("Unit属性数据/英雄属性数据", typeof(UnitAttributesDataGraph))]
    public class HeroAttributeNode: UnitAttributesNodeBase
    {
        private const string name = "英雄属性结点";
        
        public HeroAttributesNodeData HeroArrtibutesData = new HeroAttributesNodeData();

        public override UnitAttributesNodeDataBase UnitAttributesData_GetNodeData()
        {
            return HeroArrtibutesData;
        }
    }
}