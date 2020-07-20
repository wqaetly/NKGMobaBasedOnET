//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年7月25日 19:13:34
//------------------------------------------------------------

using ETModel;
using Sirenix.OdinInspector;

namespace NodeEditorFramework
{
    public abstract partial class Node
    {
        /// <summary>
        /// 获取结点数据
        /// </summary>
        /// <returns></returns>
        public virtual SkillBaseNodeData Skill_GetNodeData()
        {
            return null;
        }
    }
}