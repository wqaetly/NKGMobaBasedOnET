//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年7月25日 19:28:38
//------------------------------------------------------------

using ETModel;

namespace NodeEditorFramework
{
    public abstract partial class Node
    {
        /// <summary>
        /// 获取结点数据
        /// </summary>
        /// <returns></returns>
        public virtual NodeDataForHero HeroData_GetNodeData()
        {
            return null;
        }
    }
}