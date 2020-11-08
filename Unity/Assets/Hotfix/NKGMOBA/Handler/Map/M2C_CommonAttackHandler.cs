//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2020年10月21日 20:42:05
//------------------------------------------------------------

using ETModel;

namespace ETHotfix
{
    [MessageHandler]
    public class M2C_CommonAttackHandler: AMHandler<M2C_CommonAttack>
    {
        protected override ETTask Run(ETModel.Session session, M2C_CommonAttack message)
        {
            UnitComponent unitComponent = ETModel.Game.Scene.GetComponent<UnitComponent>();
            unitComponent.Get(message.AttackCasterId).GetComponent<CommonAttackComponent>()
                    .CommonAttackStart(unitComponent.Get(message.TargetUnitId));
            return ETTask.CompletedTask;
        }
    }
}