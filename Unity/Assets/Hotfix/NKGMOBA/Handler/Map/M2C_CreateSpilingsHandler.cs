//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2020年1月21日 13:41:32
//------------------------------------------------------------

using ETModel;
using UnityEngine;

namespace ETHotfix
{
    [MessageHandler]
    public class M2C_CreateSpilingsHandler: AMHandler<M2C_CreateSpilings>
    {
        protected override async ETTask Run(ETModel.Session session, M2C_CreateSpilings message)
        {
            SpilingInfo spilingInfo = message.Spilings;
            if (spilingInfo == null)
            {
                ETModel.Log.Error("收到的木桩回调信息为空");
            }

            RoleCamp roleCamp = (RoleCamp) spilingInfo.RoleCamp;
            //创建木桩
            Unit unit = UnitFactory.CreateSpiling(spilingInfo.UnitId, spilingInfo.ParentUnitId, roleCamp);

            //因为血条需要，创建热更层unit
            HotfixUnit hotfixUnit = HotfixUnitFactory.CreateHotfixUnit(unit);
            hotfixUnit.AddComponent<FallingFontComponent>();
            unit.Position = new Vector3(spilingInfo.X, spilingInfo.Y, spilingInfo.Z);

            unit.AddComponent<HeroDataComponent, long>(10001);

            // 创建头顶Bar
            Game.EventSystem.Run(EventIdType.CreateHeadBar, spilingInfo.UnitId);
            // 挂载头顶Bar
            hotfixUnit.AddComponent<HeroHeadBarComponent, Unit, FUI>(unit,
                Game.Scene.GetComponent<FUIComponent>().Get(spilingInfo.UnitId));

            await ETTask.CompletedTask;
        }
    }
}