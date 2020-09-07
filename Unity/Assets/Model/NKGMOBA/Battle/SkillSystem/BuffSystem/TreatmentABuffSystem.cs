//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年10月2日 12:24:34
//------------------------------------------------------------

namespace ETModel
{
    /// <summary>
    /// 这里使用的瞬时治疗，如果要做持续治疗，参考持续伤害部分
    /// </summary>
    public class TreatmentABuffSystem: ABuffSystemBase
    {
        /// <summary>
        /// 最终治疗量
        /// </summary>
        private float m_FinalTreatValue;

        public override void OnInit(BuffDataBase buffData, Unit theUnitFrom, Unit theUnitBelongto)
        {
            //设置Buff来源Unit和归属Unit
            this.TheUnitFrom = theUnitFrom;
            this.TheUnitBelongto = theUnitBelongto;
            this.BuffData = buffData;

            BuffTimerAndOverlayHelper.CalculateTimerAndOverlay(this, this.BuffData);
        }

        public override void OnExecute()
        {
            this.m_FinalTreatValue = BuffDataCalculateHelper.CalculateCurrentData(this, this.BuffData);

            //TODO:进行相关治疗影响操作，例如减疗，增疗等

            this.TheUnitBelongto.GetComponent<HeroDataComponent>().CurrentLifeValue += this.m_FinalTreatValue;
            Game.EventSystem.Run(EventIdType.ChangeMP, this.TheUnitBelongto.Id, this.m_FinalTreatValue);
            Log.Info($"受到了治疗，治疗量为{this.m_FinalTreatValue}");

            this.BuffState = BuffState.Finished;
        }

        public override void OnFinished()
        {
        }
    }
}