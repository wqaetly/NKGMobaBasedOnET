//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年6月1日 11:18:14
//------------------------------------------------------------

using ETModel;

namespace ETHotfix
{
    public class HotfixUnitFactory
    {
        public static HotfixUnit CreateHotfixUnit(Unit unit)
        {
            HotfixUnit hotfixUnit = ComponentFactory.CreateWithId<HotfixUnit, Unit>(unit.Id, unit);
            hotfixUnit.AddComponent<PlayerHeroControllerComponent>();
            Game.Scene.GetComponent<M5V5GameComponent>().AddHotfixUnit(hotfixUnit.Id, hotfixUnit);
            return hotfixUnit;
        }
    }
}