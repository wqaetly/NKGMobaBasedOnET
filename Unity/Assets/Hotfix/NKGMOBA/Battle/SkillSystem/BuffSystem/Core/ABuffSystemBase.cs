//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年9月16日 21:39:43
//------------------------------------------------------------

using Sirenix.OdinInspector;

namespace ET
{
    public abstract class ABuffSystemBase<T> : IBuffSystem, IReference where T : BuffDataBase
    {
        public NP_RuntimeTree BelongtoRuntimeTree { get; set; }
        public BuffState BuffState { get; set; }
        public int CurrentOverlay { get; set; }

        /// <summary>
        /// 持续时间（目标帧）（到达这个帧就会移除）
        /// </summary>
        public uint MaxLimitFrame { get; set; }

        public BuffDataBase BuffData { get; set; }
        public long BuffNodeId { get; set; }
        public Unit TheUnitFrom { get; set; }
        public Unit TheUnitBelongto { get; set; }

        /// <summary>
        /// 获取自身的BuffData数据，自动转型为T
        /// </summary>
        public T GetBuffDataWithTType => BuffData as T;

        public void Init(BuffDataBase buffData, Unit theUnitFrom, Unit theUnitBelongto, uint currentFrame)
        {
            //设置Buff来源Unit和归属Unit
            this.TheUnitFrom = theUnitFrom;
            this.TheUnitBelongto = theUnitBelongto;
            this.BuffData = buffData as T;
            
            // 如果没加入成功，说明已有同一个Buff，需要进入对象池
            if (BuffTimerAndOverlayHelper.CalculateTimerAndOverlay(this, currentFrame))
            {
                this.BuffState = BuffState.Waiting;
                OnInit(buffData, theUnitFrom, theUnitBelongto, currentFrame);
            }
            else
            {
                ReferencePool.Release(this);
            }
        }

        public void Excute(uint currentFrame)
        {
            switch (this.BuffData.SustainTime)
            {
                case 0:
                    this.BuffState = BuffState.Finished;
                    break;
                case -1:
                    this.BuffState = BuffState.Forever;
                    break;
                default:
                    this.BuffState = BuffState.Running;
                    break;
            }

            this.OnExecute(currentFrame);
        }

        public void Update(uint currentFrame)
        {
            if (currentFrame >= MaxLimitFrame && this.BuffState != BuffState.Forever)
            {
                this.BuffState = BuffState.Finished;
            }
            else
            {
                this.OnUpdate(currentFrame);
            }
        }

        public void Finished(uint currentFrame)
        {
            this.OnFinished(currentFrame);
        }

        public void Refresh(uint currentFrame)
        {
            this.OnRefreshed(currentFrame);
        }

        /// <summary>
        /// 初始化buff数据
        /// </summary>
        /// <param name="buffData">Buff数据</param>
        /// <param name="theUnitFrom">来自哪个Unit</param>
        /// <param name="theUnitBelongto">寄生于哪个Unit</param>
        public virtual void OnInit(BuffDataBase buffData, Unit theUnitFrom, Unit theUnitBelongto, uint currentFrame)
        {
        }

        /// <summary>
        /// Buff触发
        /// </summary>
        public abstract void OnExecute(uint currentFrame);

        /// <summary>
        /// Buff持续
        /// </summary>
        public virtual void OnUpdate(uint currentFrame)
        {
        }

        /// <summary>
        /// 重置Buff用
        /// </summary>
        public virtual void OnFinished(uint currentFrame)
        {
        }

        /// <summary>
        /// 刷新，用于刷新Buff状态
        /// </summary>
        public virtual void OnRefreshed(uint currentFrame)
        {
        }

        public void Clear()
        {
            BelongtoRuntimeTree = null;
            BuffState = BuffState.Waiting;
            CurrentOverlay = 0;
            MaxLimitFrame = 0;
            BuffData = null;
            TheUnitFrom = null;
            TheUnitBelongto = null;
        }
    }
}