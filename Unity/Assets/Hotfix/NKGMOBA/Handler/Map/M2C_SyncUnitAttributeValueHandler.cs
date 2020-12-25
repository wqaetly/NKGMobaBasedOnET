//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2020年12月13日 17:53:06
//------------------------------------------------------------

using ETModel;

namespace ETHotfix
{
    [MessageHandler]
    public class M2C_SyncUnitAttributeValueHandler: AMHandler<M2C_SyncUnitAttribute>
    {
        protected override ETTask Run(ETModel.Session session, M2C_SyncUnitAttribute message)
        {
            UnitComponent.Instance.Get(message.UnitId).GetComponent<HeroDataComponent>().NumericComponent[(NumericType) message.NumericType] =
                    message.FinalValue;
            return ETTask.CompletedTask;
        }
    }
}