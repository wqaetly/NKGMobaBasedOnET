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
        public Dictionary<long, ABuffSystemBase> TempBuffsToBeAdded = new Dictionary<long, ABuffSystemBase>();

        /// <summary>
        /// Buff链表
        /// </summary>
        private LinkedList<ABuffSystemBase> m_Buffs = new LinkedList<ABuffSystemBase>();

        /// <summary>
        /// 用于查找的——基于Buff生效方式
        /// </summary>
        private Dictionary<BuffWorkTypes, ABuffSystemBase> m_BuffsForFind_BuffWorkType = new Dictionary<BuffWorkTypes, ABuffSystemBase>();

        /// <summary>
        /// 用于查找的——基于Buff的Id
        /// </summary>
        private Dictionary<long, ABuffSystemBase> m_BuffsForFind_BuffId = new Dictionary<long, ABuffSystemBase>();

        private LinkedListNode<ABuffSystemBase> m_Current, m_Next;

        public void Update()
        {
            //把Buff从临时列表加入到正式列表
            foreach (var tempBuff in this.TempBuffsToBeAdded)
            {
                this.AddBuff2Real(tempBuff.Value);
            }

            this.TempBuffsToBeAdded.Clear();

            this.m_Current = m_Buffs.First;
            //轮询链表
            while (this.m_Current != null)
            {
                ABuffSystemBase aBuff = this.m_Current.Value;
                if (aBuff.BuffState == BuffState.Waiting)
                {
                    aBuff.OnExecute();
                }
                else if (aBuff.BuffState == BuffState.Running)
                {
                    aBuff.OnUpdate();
                    this.m_Current = this.m_Current.Next;
                }
                else
                {
                    aBuff.OnFinished();
                    this.m_Next = this.m_Current.Next;
                    m_Buffs.Remove(this.m_Current);
                    m_BuffsForFind_BuffWorkType.Remove(this.m_Current.Value.BuffData.BuffWorkType);
                    this.m_BuffsForFind_BuffId.Remove(this.m_Current.Value.BuffData.BuffId);
                    // Log.Info(
                    //     $"移除一个Buff，Id为{this.m_Current.Value.BuffData.BuffId},BuffManager是否还有?:{this.FindBuffById(this.m_Current.Value.BuffData.BuffId)}");
                    this.m_Current = this.m_Next;
                }
            }
        }

        #region 添加，移除Buff

        /// <summary>
        /// 添加Buff到真实链表，禁止外部调用
        /// </summary>
        /// <param name="aBuff"></param>
        private void AddBuff2Real(ABuffSystemBase aBuff)
        {
            m_Buffs.AddLast(aBuff);

            if (this.m_BuffsForFind_BuffWorkType.ContainsKey(aBuff.BuffData.BuffWorkType))
            {
                m_BuffsForFind_BuffWorkType[aBuff.BuffData.BuffWorkType] = aBuff;
            }
            else
            {
                m_BuffsForFind_BuffWorkType.Add(aBuff.BuffData.BuffWorkType, aBuff);
            }

            if (this.m_BuffsForFind_BuffId.ContainsKey(aBuff.BuffData.BuffId))
            {
                this.m_BuffsForFind_BuffId[aBuff.BuffData.BuffId] = aBuff;
            }
            else
            {
                this.m_BuffsForFind_BuffId.Add(aBuff.BuffData.BuffId, aBuff);
            }

            // Log.Info($"把ID为{aBuff.BuffData.BuffId}的buff加入检索表");
        }

        /// <summary>
        /// 移除Buff(下一帧才真正移除)
        /// </summary>
        /// <param name="buffId">要移除的BuffId</param>
        public void RemoveBuff(long buffId)
        {
            ABuffSystemBase aBuffSystemBase = GetBuffById(buffId);
            if (aBuffSystemBase != null)
            {
                aBuffSystemBase.BuffState = BuffState.Finished;
            }
        }

        #endregion

        #region 查询BuffSystem

        /// <summary>
        /// 通过作用方式查找Buff
        /// </summary>
        /// <param name="buffWorkTypes"></param>
        /// <returns></returns>
        public bool FindBuffByWorkType(BuffWorkTypes buffWorkTypes)
        {
            if (this.m_BuffsForFind_BuffWorkType.TryGetValue(buffWorkTypes, out ABuffSystemBase _temp))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 通过标识ID查找Buff
        /// </summary>
        /// <param name="buffId">BuffData的标识ID</param>
        /// <returns></returns>
        public bool FindBuffById(long buffId)
        {
            if (this.m_BuffsForFind_BuffId.TryGetValue(buffId, out ABuffSystemBase _temp))
            {
                return true;
            }

            return false;
        }

        #endregion

        #region 获取BuffSystem

        /// <summary>
        /// 通过作用方式获得Buff
        /// </summary>
        /// <param name="buffWorkTypes"></param>
        public ABuffSystemBase GetBuffByWorkType(BuffWorkTypes buffWorkTypes)
        {
            if (m_BuffsForFind_BuffWorkType.TryGetValue(buffWorkTypes, out ABuffSystemBase _temp))
            {
                return _temp;
            }

            //Log.Error($"查找{buffWorkTypes}失败");
            return null;
        }

        /// <summary>
        /// 通过标识ID获得Buff
        /// </summary>
        /// <param name="buffId">BuffData的标识ID</param>
        public ABuffSystemBase GetBuffById(long buffId)
        {
            if (this.m_BuffsForFind_BuffId.TryGetValue(buffId, out ABuffSystemBase _temp))
            {
                return _temp;
            }

            return null;
        }

        /// <summary>
        /// 移除并返回临时列表中的一个Buff
        /// </summary>
        /// <param name="buffId">BuffData的标识ID</param>
        /// <returns></returns>
        public ABuffSystemBase GetBuffById_FromTempDic(long buffId)
        {
            if (this.TempBuffsToBeAdded.TryGetValue(buffId, out var temp))
            {
                return temp;
            }

            return null;
        }

        #endregion
    }
}