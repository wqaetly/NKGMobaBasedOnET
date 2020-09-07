//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年9月21日 16:04:06
//------------------------------------------------------------

namespace ETModel
{
    /// <summary>
    /// 监听Buff回调
    /// </summary>
    public class ListenABuffCallBackABuffSystem: ABuffSystemBase
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
            //强制类型转换为Buff事件
            ListenBuffCallBackBuffData temp = (ListenBuffCallBackBuffData) this.BuffData;

            Game.Scene.GetComponent<BattleEventSystem>().RegisterEvent($"{temp.EventId}{this.TheUnitFrom.Id}", temp.ListenBuffEventNormal);

            this.BuffState = BuffState.Running;
        }

        public override void OnUpdate()
        {
            //只有不是永久Buff的情况下才会执行Update判断
            if (this.BuffData.SustainTime + 1 > 0)
            {
                if (TimeHelper.Now() > this.MaxLimitTime)
                {
                    this.BuffState = BuffState.Finished;
                }
            }
        }

        public override void OnFinished()
        {
            //强制类型转换为Buff事件
            ListenBuffCallBackBuffData temp = (ListenBuffCallBackBuffData) this.BuffData;
            Game.Scene.GetComponent<BattleEventSystem>().UnRegisterEvent($"{temp.EventId}{this.TheUnitFrom.Id}", temp.ListenBuffEventNormal);
        }
    }
}