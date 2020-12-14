//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2020年1月21日 17:41:28
//------------------------------------------------------------

using ETModel;

namespace ETHotfix
{
    [MessageHandler]
    public class M2C_ChangeUnitAttributeValueHandler: AMHandler<M2C_ChangeUnitAttribute>
    {
        protected override ETTask Run(ETModel.Session session, M2C_ChangeUnitAttribute message)
        {
            Game.EventSystem.Run(EventIdType.ChangeUnitAttribute, message.UnitId, message.NumericType, message.ChangeValue);
            return ETTask.CompletedTask;
        }
    }
}