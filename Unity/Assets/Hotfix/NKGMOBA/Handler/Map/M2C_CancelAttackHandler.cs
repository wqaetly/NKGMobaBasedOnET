//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2020年12月6日 16:13:53
//------------------------------------------------------------

using ETModel;

namespace ETHotfix
{
    [MessageHandler]
    public class M2C_CancelAttackHandler: AMHandler<M2C_CancelAttack>
    {
        protected override async ETTask Run(ETModel.Session session, M2C_CancelAttack message)
        {
            Unit unit = ETModel.Game.Scene.GetComponent<UnitComponent>().Get(message.UnitId);
            unit.GetComponent<CommonAttackComponent>().CancelCommonAttack();
            ETModel.Log.Error("收到取消攻击指令");
            await ETTask.CompletedTask;
        }
    }
}