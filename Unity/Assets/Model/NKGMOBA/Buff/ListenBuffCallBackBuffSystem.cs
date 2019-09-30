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

            //取得归属Unit的Hero数据，用以计算数据
            HeroDataComponent theUnitFromHeroData = this.theUnitFrom.GetComponent<HeroDataComponent>();
        }

        public override void OnExecute()
        {
            //强制类型转换为Buff事件
            ListenBuffDataBase temp = (ListenBuffDataBase) MSkillBuffDataBase;
            Game.Scene.GetComponent<BattleEventSystem>().RegisterEvent(temp.EventId, temp.ListenBuffEventBase);
        }

        public override void OnFinished()
        {
            
        }
    }
}