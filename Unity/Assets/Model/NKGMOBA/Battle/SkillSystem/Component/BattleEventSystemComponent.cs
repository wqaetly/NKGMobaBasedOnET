//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年9月21日 15:10:39
//------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace ET
{
    /// <summary>
    /// 战斗系统中的事件系统组件，一场战斗挂载一个
    /// </summary>
    public class BattleEventSystemComponent : Entity
    {
        public readonly Dictionary<string, LinkedList<ISkillSystemEvent>> AllEvents =
            new Dictionary<string, LinkedList<ISkillSystemEvent>>();

        /// <summary>
        /// 缓存的结点字典
        /// </summary>
        public readonly Dictionary<string, LinkedListNode<ISkillSystemEvent>> CachedNodes =
            new Dictionary<string, LinkedListNode<ISkillSystemEvent>>();

        /// <summary>
        /// 临时结点字典
        /// </summary>
        public readonly Dictionary<string, LinkedListNode<ISkillSystemEvent>> TempNodes =
            new Dictionary<string, LinkedListNode<ISkillSystemEvent>>();
    }
}