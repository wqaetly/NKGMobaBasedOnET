//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2020年1月21日 17:41:10
//------------------------------------------------------------

using ETModel;

namespace ETHotfix
{
    [MessageHandler]
    public class M2C_ChangeHPValueHandler: AMHandler<M2C_ChangeHeroHP>
    {
        protected override ETTask Run(ETModel.Session session, M2C_ChangeHeroHP message)
        {
            UnitComponent.Instance.Get(message.UnitId).GetComponent<HeroDataComponent>().CurrentLifeValue +=
                    message.ChangeHPValue;
            Game.EventSystem.Run(EventIdType.ChangeHPValue, message.UnitId, message.ChangeHPValue);
            return ETTask.CompletedTask;
        }
    }
}