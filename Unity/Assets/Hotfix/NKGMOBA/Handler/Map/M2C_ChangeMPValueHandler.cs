//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2020年1月21日 17:41:28
//------------------------------------------------------------

using ETModel;

namespace ETHotfix
{
    [MessageHandler]
    public class M2C_ChangeMPValueHandler: AMHandler<M2C_ChangeHeroMP>
    {
        protected override ETTask Run(ETModel.Session session, M2C_ChangeHeroMP message)
        {
            //Log.Info("接收到蓝量改变事件");
            ETModel.Game.Scene.GetComponent<UnitComponent>().Get(message.UnitId).GetComponent<HeroDataComponent>().CurrentMagicValue +=
                    message.ChangeMPValue;
            Game.EventSystem.Run(EventIdType.ChangeMPValue, message.UnitId, message.ChangeMPValue);
            return ETTask.CompletedTask;
        }
    }
}