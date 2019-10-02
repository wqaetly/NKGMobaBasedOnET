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
        /// <summary>
        /// 自身下一个时间点
        /// </summary>
        private float selfNextimer;

        public override void OnInit(BuffDataBase BuffDataBase, Unit theUnitFrom, Unit theUnitBelongto)
        {
            //设置Buff来源Unit和归属Unit
            this.theUnitFrom = theUnitFrom;
            this.theUnitBelongto = theUnitBelongto;
            this.MSkillBuffDataBase = BuffDataBase;

            this.MaxLimitTime = TimeHelper.ClientNow() + this.MSkillBuffDataBase.SustainTime;

            this.MBuffState = BuffState.Waiting;
        }

        public override void OnExecute()
        {
            //强制类型转换为Buff事件
            ListenBuffDataBase temp = (ListenBuffDataBase) MSkillBuffDataBase;
            Game.Scene.GetComponent<BattleEventSystem>().RegisterEvent(temp.EventId, temp.ListenBuffEventBase);
            this.MBuffState = BuffState.Running;
        }

        public override void OnUpdate()
        {
            if (TimeHelper.ClientNow() > this.MaxLimitTime)
            {
                //强制类型转换为Buff事件
                ListenBuffDataBase temp = (ListenBuffDataBase) MSkillBuffDataBase;
                Game.Scene.GetComponent<BattleEventSystem>().UnRegisterEvent(temp.EventId, temp.ListenBuffEventBase);
                this.MBuffState = BuffState.Finished;
            }
        }

        public override void OnFinished()
        {
        }
    }
}