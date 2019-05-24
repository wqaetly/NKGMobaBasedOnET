using ETModel;
using Vector3 = UnityEngine.Vector3;

namespace ETHotfix
{
    [MessageHandler]
    public class M2C_CreateUnitsHandler: AMHandler<M2C_CreateUnits>
    {
        protected override void Run(ETModel.Session session, M2C_CreateUnits message)
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

                unit.Position = new Vector3(unitInfo.X, unitInfo.Y, unitInfo.Z);
            }
        }
    }
}