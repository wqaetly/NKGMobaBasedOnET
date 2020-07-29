//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年5月21日 19:41:22
//------------------------------------------------------------

using ETHotfix;
using ETModel;

namespace ETHotfix
{
    [Event(EventIdType.EnterMapFinish)]
    public class Show5v5MapUI: AEvent
    {
        public override void Run()
        {
            this.ShowUI();
        }

        public void ShowUI()
        {
            //加载UI资源
            ETModel.Game.Scene.GetComponent<FUIPackageComponent>().AddPackage(FUIPackage.FUI5v5Map);
            //创建UI实例
            var hotfixui = FUI5V5Map.CreateInstance();
            //默认将会以Id为Name，也可以自定义Name，方便查询和管理
            hotfixui.Name = FUIPackage.FUI5v5Map;
            //设置UI为全屏大小
            hotfixui.MakeFullScreen();
            //将UI注册到FUIComponent中，正式显示
            Game.Scene.GetComponent<FUIComponent>().Add(hotfixui, true);
        }
    }

    [Event(EventIdType.ChangeHPValue)]
    public class Map_ChangeHP: AEvent<long, float>
    {
        public override void Run(long a, float b)
        {
            if (a != ETModel.Game.Scene.GetComponent<UnitComponent>().MyUnit.Id) return;
            FUI5V5Map fui5V5Map = Game.Scene.GetComponent<FUIComponent>().Get(FUI5V5Map.UIPackageName) as FUI5V5Map;
            fui5V5Map.RedProBar.self.TweenValue(
                ETModel.Game.Scene.GetComponent<UnitComponent>().MyUnit.GetComponent<HeroDataComponent>().CurrentLifeValue, 0.2f);
            fui5V5Map.RedText.text = $"{fui5V5Map.RedProBar.self.value}/{fui5V5Map.RedProBar.self.max}";
        }
    }

    [Event(EventIdType.ChangeHPMax)]
    public class Map_ChangeHPBarMax: AEvent<long, float>
    {
        public override void Run(long a, float b)
        {
            FUI5V5Map fui5V5Map = Game.Scene.GetComponent<FUIComponent>().Get(FUI5V5Map.UIPackageName) as FUI5V5Map;
            //第一次抛出事件的时候可能UI还没有加载出来
            if (fui5V5Map == null) return;
            if (a != ETModel.Game.Scene.GetComponent<UnitComponent>().MyUnit.Id) return;
            fui5V5Map.RedProBar.self.max = b;
            fui5V5Map.RedText.text = $"{fui5V5Map.RedProBar.self.value}/{fui5V5Map.RedProBar.self.max}";
        }
    }

    [Event(EventIdType.ChangeMPMax)]
    public class Map_ChangeMPBar_Max: AEvent<long, float>
    {
        public override void Run(long a, float b)
        {
            if (a != ETModel.Game.Scene.GetComponent<UnitComponent>().MyUnit.Id) return;
            FUI5V5Map fui5V5Map = Game.Scene.GetComponent<FUIComponent>().Get(FUI5V5Map.UIPackageName) as FUI5V5Map;

            fui5V5Map.BlueProBar.self.max = b;
            fui5V5Map.BlueText.text = $"{fui5V5Map.BlueProBar.self.value}/{fui5V5Map.BlueProBar.self.max}";
        }
    }

    [Event(EventIdType.ChangeMPValue)]
    public class Map_ChangeMPValue: AEvent<long, float>
    {
        public override void Run(long a, float b)
        {
            if (a != ETModel.Game.Scene.GetComponent<UnitComponent>().MyUnit.Id) return;
            FUI5V5Map fui5V5Map = Game.Scene.GetComponent<FUIComponent>().Get(FUI5V5Map.UIPackageName) as FUI5V5Map;
            fui5V5Map.BlueProBar.self.TweenValue(
                ETModel.Game.Scene.GetComponent<UnitComponent>().MyUnit.GetComponent<HeroDataComponent>().CurrentMagicValue, 0.2f);
            fui5V5Map.BlueText.text = $"{fui5V5Map.BlueProBar.self.value}/{fui5V5Map.BlueProBar.self.max}";
        }
    }
}