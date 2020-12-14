//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年5月31日 10:51:11
//------------------------------------------------------------

using ETHotfix;
using ETModel;

namespace ETHotfix
{
    [Event(EventIdType.CreateHeadBar)]
    public class LoginSuccess_CreateHeadBar: AEvent<long>
    {
        public override void Run(long fuiId)
        {
            ETModel.Game.Scene.GetComponent<FUIPackageComponent>().AddPackage(FUIPackage.FUIHeadBar);
            var hotfixui = FUIHeadBar.CreateInstance();
            //默认将会以Id为Name，也可以自定义Name，方便查询和管理，这里使用血条归属的Unit id作为Name
            hotfixui.Name = fuiId.ToString();
            //Log.Info($"这个英雄血条id为{hotfixui.Name}");
            hotfixui.MakeFullScreen();
            Game.Scene.GetComponent<FUIComponent>().Add(hotfixui, true);
        }
    }

    [NumericWatcher(NumericType.MaxHp)]
    public class ChangeHPBar_Density: INumericWatcher
    {
        public void Run(long id, float value)
        {
            Game.Scene.GetComponent<M5V5GameComponent>().GetHotfixUnit(id).GetComponent<HeroHeadBarComponent>().SetDensityOfBar(value);
        }
    }

    [NumericWatcher(NumericType.Hp)]
    public class ChangeHPValue: INumericWatcher
    {
        public void Run(long id, float value)
        {
            FUIHeadBar headBar = Game.Scene.GetComponent<FUIComponent>().Get(id) as FUIHeadBar;
            headBar.Bar_HP.self.TweenValue(UnitComponent.Instance.Get(id).GetComponent<HeroDataComponent>().GetAttribute(NumericType.Hp),
                0.2f);
        }
    }

    [NumericWatcher(NumericType.MaxMp)]
    public class ChangeMPBar_Max: INumericWatcher
    {
        public void Run(long id, float value)
        {
            FUIHeadBar headBar = Game.Scene.GetComponent<FUIComponent>().Get(id) as FUIHeadBar;
            headBar.Bar_MP.self.max = value;
        }
    }

    [NumericWatcher(NumericType.Mp)]
    public class ChangeMPValue: INumericWatcher
    {
        public void Run(long id, float value)
        {
            FUIHeadBar headBar = Game.Scene.GetComponent<FUIComponent>().Get(id) as FUIHeadBar;
            headBar.Bar_MP.self.TweenValue(UnitComponent.Instance.Get(id).GetComponent<HeroDataComponent>().GetAttribute(NumericType.Mp), 0.2f);
        }
    }
}