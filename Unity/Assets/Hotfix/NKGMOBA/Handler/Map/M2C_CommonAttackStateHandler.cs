//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2020年10月21日 20:42:05
//------------------------------------------------------------

using ETModel;

namespace ETHotfix
{
    [MessageHandler]
    public class M2C_CommonAttackStateHandler: AMHandler<M2C_CommonAttackState>
    {
        protected override ETTask Run(ETModel.Session session, M2C_CommonAttackState message)
        {
            ETModel.Game.Scene.GetComponent<UnitComponent>().Get(message.UnitId).GetComponent<CommonAttackComponent>().CanAttack = message.CanAttack;
            return ETTask.CompletedTask;
        }
    }
}