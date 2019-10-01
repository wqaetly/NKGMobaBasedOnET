//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年10月1日 21:22:19
//------------------------------------------------------------

namespace ETModel
{
    /// <summary>
    /// 绑定一个状态
    /// </summary>
    public class BindStateBuffSystem: BuffSystemBase
    {
        /// <summary>
        /// 当前叠加数
        /// </summary>
        public int CurrentOverlay;

        /// <summary>
        /// 自身下一个时间点
        /// </summary>
        private float selfNextimer;

        /// <summary>
        /// 最大时间限制（持续几秒）
        /// </summary>
        private float maxTime;

        public override void OnInit(BuffDataBase BuffDataBase, Unit theUnitFrom, Unit theUnitBelongto)
        {
            //设置Buff来源Unit和归属Unit
            this.theUnitFrom = theUnitFrom;
            this.theUnitBelongto = theUnitBelongto;
            this.MSkillBuffDataBase = BuffDataBase;

            BindStateBuffSystem tempSystem =
                    (BindStateBuffSystem) theUnitBelongto.GetComponent<BuffManagerComponent>().GetBuffByFlagID(BuffDataBase.FlagId);

            BindStateBuffData tempData = tempSystem.MSkillBuffDataBase as BindStateBuffData;

            if (theUnitBelongto.GetComponent<BuffManagerComponent>().FindBuffByFlagID(BuffDataBase.FlagId))
            {
                //可以叠加，并且当前层数未达到最高层
                if (tempData.CanOverlay &&
                    tempSystem.CurrentOverlay < tempData.MaxOverlay)
                {
                    //如果是有限时长的
                    if (tempData.SustainTime + 1 > 0)
                    {
                        tempSystem.maxTime += tempData.SustainTime;
                    }

                    tempSystem.CurrentOverlay++;
                }

                this.MBuffState = BuffState.Finished;
            }
            else
            {
                //如果是有限时长的
                if (tempData.SustainTime + 1 > 0)
                {
                    maxTime = TimeHelper.ClientNow() + ((SustainDamageBuffData) this.MSkillBuffDataBase).SustainTime;
                }

                tempSystem.CurrentOverlay++;

                this.MBuffState = BuffState.Waiting;
            }
        }

        public override void OnExecute()
        {
            BindStateBuffData tempData = MSkillBuffDataBase as BindStateBuffData;
            foreach (var VARIABLE in tempData.OriBuff)
            {
                this.theUnitBelongto.GetComponent<BuffManagerComponent>()
                        .AddBuff(Game.Scene.GetComponent<BuffPoolComponent>().AcquireBuff(VARIABLE, theUnitFrom, theUnitBelongto));
            }

            this.MBuffState = BuffState.Running;
        }

        public override void OnUpdate()
        {
            //只有不是永久Buff的情况下才会执行Update判断
            if ((this.MSkillBuffDataBase as BindStateBuffData).SustainTime + 1 > 0)
            {
                if (TimeHelper.ClientNow() > maxTime)
                {
                    this.MBuffState = BuffState.Finished;
                }
            }
        }

        public override void OnFinished()
        {
        }
    }
}