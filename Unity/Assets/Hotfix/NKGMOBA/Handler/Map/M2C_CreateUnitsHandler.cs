using ETModel;
using Vector3 = UnityEngine.Vector3;

namespace ETHotfix
{
    [MessageHandler]
    public class M2C_CreateUnitsHandler: AMHandler<M2C_CreateUnits>
    {
        protected override async ETTask Run(ETModel.Session session, M2C_CreateUnits message)
        {
            UnitComponent unitComponent = ETModel.Game.Scene.GetComponent<UnitComponent>();

            foreach (UnitInfo unitInfo in message.Units)
            {
                if (unitComponent.Get(unitInfo.UnitId) != null)
                {
                    continue;
                }

                //根据不同ID，创建小骷髅
                Unit unit = UnitFactory.Create(unitInfo.UnitId);
                //因为血条需要，创建热更层unit
                HotfixUnit hotfixUnit = HotfixUnitFactory.CreateHotfixUnit(unit);

                unit.Position = new Vector3(unitInfo.X, unitInfo.Y, unitInfo.Z);

                // 创建血条
                Game.EventSystem.Run(EventIdType.CreateHeadBar, unitInfo.UnitId);

                // 增加头顶Bar
                hotfixUnit.AddComponent<HeroHeadBarComponent, Unit, FUI>(unit,
                    Game.Scene.GetComponent<FUIComponent>().Get(unitInfo.UnitId));
            }
            await ETTask.CompletedTask;
        }
    }
}