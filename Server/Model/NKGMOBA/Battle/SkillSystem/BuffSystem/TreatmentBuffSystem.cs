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
    public class TreatmentBuffSystem: ABuffSystemBase
    {
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
            float finalTreatValue;
            finalTreatValue = BuffDataCalculateHelper.CalculateCurrentData(this, this.BuffData);

            //TODO:进行相关治疗影响操作，例如减疗，增疗等，应该和伤害计算差不多处理（比如香炉会增加治疗量），这里暂时先只考虑受方
            //TODO:同样的治疗量这一数据也要做一个类似DamageData的数据体
            finalTreatValue = this.TheUnitBelongto.GetComponent<DataModifierComponent>().BaptismData("Treat", finalTreatValue);

            this.TheUnitBelongto.GetComponent<HeroDataComponent>().NumericComponent.ApplyChange(NumericType.Hp, finalTreatValue);
            //抛出治疗事件，需要监听治疗的Buff需要监听此事件
            Game.Scene.GetComponent<BattleEventSystem>().Run($"{EventIdType.ExcuteTreate}{this.TheUnitFrom.Id}", finalTreatValue);
            Game.Scene.GetComponent<BattleEventSystem>().Run($"{EventIdType.TakeTreate}{this.GetBuffTarget()}", finalTreatValue);

            this.BuffState = BuffState.Finished;
        }

        public override void OnFinished()
        {
        }
    }
}