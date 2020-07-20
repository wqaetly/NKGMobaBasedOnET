//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年8月12日 18:01:01
//------------------------------------------------------------

using ETModel;
using Plugins.NodeEditor;

namespace NodeEditorFramework
{
    public abstract partial class Node
    {
        /// <summary>
        /// 获取结点数据
        /// </summary>
        /// <returns></returns>
        public virtual B2S_PrefabData Prefab_GetNodeData()
        {
            return null;
        }
    }
}