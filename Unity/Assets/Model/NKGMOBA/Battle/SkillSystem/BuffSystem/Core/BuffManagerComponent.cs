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
        /// 所有将要被添加的Buff都要先进这个字典，然后经过筛选的Buff才能加入m_Buffs链表
        /// </summary>
        public Dictionary<long, BuffSystemBase> m_TempBuffsToBeAdded = new Dictionary<long, BuffSystemBase>();

        /// <summary>
        /// Buff链表
        /// </summary>
        private LinkedList<BuffSystemBase> m_Buffs = new LinkedList<BuffSystemBase>();

        /// <summary>
        /// 用于查找的——基于Buff生效方式
        /// </summary>
        public Dictionary<BuffWorkTypes, BuffSystemBase> m_BuffsForFind_BuffWorkType = new Dictionary<BuffWorkTypes, BuffSystemBase>();

        /// <summary>
        /// 用于查找的——基于Buff的ID
        /// </summary>
        public Dictionary<int, BuffSystemBase> m_BuffsForFind_BuffFlagID = new Dictionary<int, BuffSystemBase>();

        private LinkedListNode<BuffSystemBase> current, next;

        public void Update()
        {
            //把Buff从临时列表加入到正式列表
            foreach (var VARIABLE in m_TempBuffsToBeAdded)
            {
                this.AddBuff2Real(VARIABLE.Value);
            }

            m_TempBuffsToBeAdded.Clear();

            current = m_Buffs.First;
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
                    current = current.Next;
                }
                else
                {
                    buff.OnFinished();
                    next = current.Next;
                    m_Buffs.Remove(current);
                    m_BuffsForFind_BuffWorkType.Remove(current.Value.MSkillBuffDataBase.BuffWorkType);
                    m_BuffsForFind_BuffFlagID.Remove(current.Value.MSkillBuffDataBase.FlagId);
                    Log.Info(
                        $"移除一个Buff，ID为{current.Value.MSkillBuffDataBase.FlagId},BuffManager是否还有?:{this.FindBuffByFlagID(current.Value.MSkillBuffDataBase.FlagId)}");
                    current = next;
                }
            }
        }

        /// <summary>
        /// 添加Buff到真是链表，禁止外部调用
        /// </summary>
        /// <param name="buff"></param>
        private void AddBuff2Real(BuffSystemBase buff)
        {
            m_Buffs.AddLast(buff);

            if (this.m_BuffsForFind_BuffWorkType.ContainsKey(buff.MSkillBuffDataBase.BuffWorkType))
            {
                m_BuffsForFind_BuffWorkType[buff.MSkillBuffDataBase.BuffWorkType] = buff;
            }
            else
            {
                m_BuffsForFind_BuffWorkType.Add(buff.MSkillBuffDataBase.BuffWorkType, buff);
            }

            if (this.m_BuffsForFind_BuffFlagID.ContainsKey(buff.MSkillBuffDataBase.FlagId))
            {
                m_BuffsForFind_BuffFlagID[buff.MSkillBuffDataBase.FlagId] = buff;
            }
            else
            {
                m_BuffsForFind_BuffFlagID.Add(buff.MSkillBuffDataBase.FlagId, buff);
            }
            Log.Info($"把ID为{buff.MSkillBuffDataBase.FlagId}的buff加入检索表");
        }

        /// <summary>
        /// 通过作用方式获得Buff
        /// </summary>
        /// <param name="buffWorkTypes"></param>
        public BuffSystemBase GetBuffByWorkType(BuffWorkTypes buffWorkTypes)
        {
            if (m_BuffsForFind_BuffWorkType.TryGetValue(buffWorkTypes, out BuffSystemBase _temp))
            {
                return _temp;
            }

            //Log.Error($"查找{buffWorkTypes}失败");
            return null;
        }

        /// <summary>
        /// 通过作用方式查找Buff
        /// </summary>
        /// <param name="buffWorkTypes"></param>
        /// <returns></returns>
        public bool FindBuffByWorkType(BuffWorkTypes buffWorkTypes)
        {
            if (this.m_BuffsForFind_BuffWorkType.TryGetValue(buffWorkTypes, out BuffSystemBase _temp))
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

            Log.Info($"查找{flagID}Buff失败");
            return null;
        }

        /// <summary>
        /// 移除并返回临时列表中的一个Buff
        /// </summary>
        /// <param name="flagID">BuffData的标识ID</param>
        /// <returns></returns>
        public BuffSystemBase FindBuffByFlagID_FromTempDic(int flagID)
        {
            if (this.m_TempBuffsToBeAdded.TryGetValue(flagID, out var temp))
            {
                return temp;
            }

            return null;
        }

        /// <summary>
        /// 通过标识ID查找Buff
        /// </summary>
        /// <param name="flagID">BuffData的标识ID</param>
        /// <returns></returns>
        public bool FindBuffByFlagID(int flagID)
        {
            if (this.m_BuffsForFind_BuffFlagID.TryGetValue(flagID, out BuffSystemBase _temp))
            {
                return true;
            }

            return false;
        }
    }
}