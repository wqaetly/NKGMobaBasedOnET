//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年7月25日 20:19:41
//------------------------------------------------------------

using ETMode;

namespace NodeEditorFramework
{
    public abstract partial class Node
    {
        /// <summary>
        /// 获取结点数据
        /// </summary>
        /// <returns></returns>
        public virtual B2S_CollisionInstance B2SCollisionRelation_GetNodeData()
        {
            return null;
        }
    }
}