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
    public class TreatmentBuffSystem: BuffSystemBase
    {
        /// <summary>
        /// 最终治疗量
        /// </summary>
        public float FinalTreatValue;

        public override void OnInit(BuffDataBase BuffDataBase, Unit theUnitFrom, Unit theUnitBelongto)
        {
            //设置Buff来源Unit和归属Unit
            this.theUnitFrom = theUnitFrom;
            this.theUnitBelongto = theUnitBelongto;
            this.MSkillBuffDataBase = BuffDataBase;

            BuffTimerAndOverlayHelper.CalculateTimerAndOverlay(this, this.MSkillBuffDataBase);
        }

        public override void OnExecute()
        {
            this.FinalTreatValue = BuffDataCalculateHelper.CalculateCurrentData(this, this.MSkillBuffDataBase);

            //TODO:进行相关治疗影响操作，例如减疗，增疗等

            
            this.theUnitBelongto.GetComponent<HeroDataComponent>().CurrentLifeValue += this.FinalTreatValue;
            Log.Info($"受到了治疗，治疗量为{FinalTreatValue}");

            this.MBuffState = BuffState.Finished;
        }

        public override void OnFinished()
        {

        }
    }
}