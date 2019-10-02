//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年9月16日 21:12:08
//------------------------------------------------------------

using System.Collections.Generic;

namespace ETModel
{
    [ObjectSystem]
    public class BuffManagerCompoenntUpdateSystem: UpdateSystem<BuffManagerComponent>
    {
        public override void Update(BuffManagerComponent self)
        {
            self.Update();
        }
    }

    public class BuffManagerComponent: Component
    {
        /// <summary>
        /// Buff链表
        /// </summary>
        public LinkedList<BuffSystemBase> m_Buffs = new LinkedList<BuffSystemBase>();

        /// <summary>
        /// 用于查找的——基于Buff生效方式
        /// </summary>
        public Dictionary<BuffWorkTypes, List<BuffSystemBase>> m_BuffsForFind_BuffWorkType = new Dictionary<BuffWorkTypes, List<BuffSystemBase>>();

        /// <summary>
        /// 用于查找的——基于Buff的ID
        /// </summary>
        public Dictionary<int, BuffSystemBase> m_BuffsForFind_BuffFlagID = new Dictionary<int, BuffSystemBase>();

        public void Update()
        {
            //指向首链表地址
            LinkedListNode<BuffSystemBase> current = m_Buffs.First;
            //轮询链表
            while (current != null)
            {
                BuffSystemBase buff = current.Value;
                if (buff.MBuffState == BuffState.Waiting)
                {
                    buff.OnExecute();
                }
                else if (buff.MBuffState == BuffState.Running)
                {
                    buff.OnUpdate();
                }
                else
                {
                    buff.OnFinished();
                    current = current.Next;
                    LinkedListNode<BuffSystemBase> next = current.Next;
                    m_Buffs.Remove(current);
                    m_BuffsForFind_BuffWorkType[current.Value.MSkillBuffDataBase.BuffWorkType].Remove(current.Value);
                    m_BuffsForFind_BuffFlagID.Remove(current.Value.MSkillBuffDataBase.FlagId);
                    current = next;
                    continue;
                }

                current = current.Next;
            }
        }

        public void AddBuff(BuffSystemBase buff)
        {
            m_Buffs.AddLast(buff);
            if (this.m_BuffsForFind_BuffWorkType.ContainsKey(buff.MSkillBuffDataBase.BuffWorkType))
            {
                m_BuffsForFind_BuffWorkType[buff.MSkillBuffDataBase.BuffWorkType].Add(buff);
            }
            else
            {
                m_BuffsForFind_BuffWorkType.Add(buff.MSkillBuffDataBase.BuffWorkType, new List<BuffSystemBase>() { buff });
            }

            if (!this.m_BuffsForFind_BuffFlagID.ContainsKey(buff.MSkillBuffDataBase.FlagId))
            {
                this.m_BuffsForFind_BuffFlagID.Add(buff.MSkillBuffDataBase.FlagId, buff);
            }
        }

        /// <summary>
        /// 通过作用方式获得Buff
        /// </summary>
        /// <param name="buffWorkTypes"></param>
        public List<BuffSystemBase> GetBuffByWorkType(BuffWorkTypes buffWorkTypes)
        {
            if (m_BuffsForFind_BuffWorkType.TryGetValue(buffWorkTypes, out List<BuffSystemBase> _temp))
            {
                return _temp;
            }

            Log.Error($"查找{buffWorkTypes}失败");
            return null;
        }

        /// <summary>
        /// 通过作用方式查找Buff
        /// </summary>
        /// <param name="buffWorkTypes"></param>
        /// <returns></returns>
        public bool FindBuffByWorkType(BuffWorkTypes buffWorkTypes)
        {
            if (m_BuffsForFind_BuffWorkType.ContainsKey(buffWorkTypes))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 通过标识ID获得Buff
        /// </summary>
        /// <param name="flagID">BuffData的标识ID</param>
        public BuffSystemBase GetBuffByFlagID(int flagID)
        {
            if (this.m_BuffsForFind_BuffFlagID.TryGetValue(flagID, out BuffSystemBase _temp))
            {
                return _temp;
            }

            Log.Error($"查找{flagID}Buff失败");
            return null;
        }

        /// <summary>
        /// 通过标识ID查找Buff
        /// </summary>
        /// <param name="flagID">BuffData的标识ID</param>
        /// <returns></returns>
        public bool FindBuffByFlagID(int flagID)
        {
            if (this.m_BuffsForFind_BuffFlagID.ContainsKey(flagID))
            {
                return true;
            }

            return false;
        }
    }
}