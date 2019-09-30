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

        public Dictionary<BuffWorkTypes, List<BuffSystemBase>> m_BuffsForFind = new Dictionary<BuffWorkTypes, List<BuffSystemBase>>();

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
                    m_BuffsForFind[current.Value.MSkillBuffDataBase.Base_BuffExtraWork].Remove(current.Value);
                    current = next;
                    continue;
                }

                current = current.Next;
            }
        }

        public void AddBuff(BuffSystemBase buff)
        {
            m_Buffs.AddLast(buff);
            if (this.m_BuffsForFind.ContainsKey(buff.MSkillBuffDataBase.Base_BuffExtraWork))
            {
                m_BuffsForFind[buff.MSkillBuffDataBase.Base_BuffExtraWork].Add(buff);
            }
            else
            {
                m_BuffsForFind.Add(buff.MSkillBuffDataBase.Base_BuffExtraWork, new List<BuffSystemBase>() { buff });
            }
        }

        /// <summary>
        /// 通过作用方式获得Buff
        /// </summary>
        /// <param name="buffWorkTypes"></param>
        public List<BuffSystemBase> GetBuffByWorkType(BuffWorkTypes buffWorkTypes)
        {
            if (m_BuffsForFind.TryGetValue(buffWorkTypes, out List<BuffSystemBase> _temp))
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
            if (m_BuffsForFind.ContainsKey(buffWorkTypes))
            {
                return true;
            }

            return false;
        }
    }
}