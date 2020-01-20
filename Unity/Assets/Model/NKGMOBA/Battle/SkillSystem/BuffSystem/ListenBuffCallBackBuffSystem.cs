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
    public class ListenBuffCallBackBuffSystem: BuffSystemBase
    {
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
            //强制类型转换为Buff事件
            ListenBuffDataBase temp = (ListenBuffDataBase) MSkillBuffDataBase;
            foreach (var VARIABLE in temp.EventIds)
            {
                Log.Info($"订阅了{VARIABLE}");
                Game.Scene.GetComponent<BattleEventSystem>().RegisterEvent($"{VARIABLE}{this.theUnitFrom.Id}", temp.ListenBuffEventBase);
            }

            this.MBuffState = BuffState.Running;
        }

        public override void OnUpdate()
        {
            //只有不是永久Buff的情况下才会执行Update判断
            if (this.MSkillBuffDataBase.SustainTime + 1 > 0)
            {
                if (TimeHelper.Now() > this.MaxLimitTime)
                {
                    //强制类型转换为Buff事件
                    ListenBuffDataBase temp = (ListenBuffDataBase) MSkillBuffDataBase;
                    foreach (var VARIABLE in temp.EventIds)
                    {
                        Game.Scene.GetComponent<BattleEventSystem>().UnRegisterEvent($"{VARIABLE}{this.theUnitFrom.Id}", temp.ListenBuffEventBase);
                    }

                    this.MBuffState = BuffState.Finished;
                }
            }
        }

        public override void OnFinished()
        {
        }
    }
}