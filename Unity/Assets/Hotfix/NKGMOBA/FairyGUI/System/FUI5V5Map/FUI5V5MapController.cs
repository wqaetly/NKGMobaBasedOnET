//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年5月21日 19:41:22
//------------------------------------------------------------

using ETHotfix;
using ETModel;
using UnityEngine;

namespace ETHotfix
{
    [Event(EventIdType.EnterMapFinish)]
    public class Show5v5MapUI: AEvent
    {
        public override void Run()
        {
            //加载UI资源
            this.RunInternal().Coroutine();
            //创建UI实例
            var hotfixui = FUI5V5Map.CreateInstance();
            //默认将会以Id为Name，也可以自定义Name，方便查询和管理
            hotfixui.Name = FUIPackage.FUI5v5Map;
            hotfixui.GObject.sortingOrder = 39;
            //设置UI为全屏大小
            hotfixui.MakeFullScreen();
            //将UI注册到FUIComponent中，正式显示
            Game.Scene.GetComponent<FUIComponent>().Add(hotfixui, true);
        }

        private async ETVoid RunInternal()
        {
            await ETModel.Game.Scene.GetComponent<FUIPackageComponent>().AddPackageAsync(FUIPackage.FUI5v5Map);;
        }
    }

    [NumericWatcher(NumericType.Hp)]
    public class Map_ChangeHP: INumericWatcher
    {
        public void Run(long a, float b)
        {
            if (a != UnitComponent.Instance.MyUnit.Id) return;
            FUI5V5Map fui5V5Map = Game.Scene.GetComponent<FUIComponent>().Get(FUI5V5Map.UIPackageName) as FUI5V5Map;
            fui5V5Map.RedProBar.self.TweenValue(UnitComponent.Instance.MyUnit.GetComponent<HeroDataComponent>().GetAttribute(NumericType.Hp), 0.2f);
            fui5V5Map.RedText.text = $"{fui5V5Map.RedProBar.self.value}/{fui5V5Map.RedProBar.self.max}";
        }
    }

    [NumericWatcher(NumericType.MaxHp)]
    public class Map_ChangeHPBarMax: INumericWatcher
    {
        public void Run(long id, float value)
        {
            FUI5V5Map fui5V5Map = Game.Scene.GetComponent<FUIComponent>().Get(FUI5V5Map.UIPackageName) as FUI5V5Map;
            //第一次抛出事件的时候可能UI还没有加载出来
            if (fui5V5Map == null) return;
            if (id != UnitComponent.Instance.MyUnit.Id) return;
            fui5V5Map.RedProBar.self.max = value;
            fui5V5Map.RedText.text = $"{fui5V5Map.RedProBar.self.value}/{fui5V5Map.RedProBar.self.max}";
        }
    }

    [NumericWatcher(NumericType.MaxMp)]
    public class Map_ChangeMPBar_Max: INumericWatcher
    {
        public void Run(long a, float b)
        {
            if (a != UnitComponent.Instance.MyUnit.Id) return;
            FUI5V5Map fui5V5Map = Game.Scene.GetComponent<FUIComponent>().Get(FUI5V5Map.UIPackageName) as FUI5V5Map;

            fui5V5Map.BlueProBar.self.max = b;
            fui5V5Map.BlueText.text = $"{fui5V5Map.BlueProBar.self.value}/{fui5V5Map.BlueProBar.self.max}";
        }
    }

    [NumericWatcher(NumericType.Mp)]
    public class Map_ChangeMPValue: INumericWatcher
    {
        public void Run(long a, float b)
        {
            if (a != UnitComponent.Instance.MyUnit.Id) return;
            FUI5V5Map fui5V5Map = Game.Scene.GetComponent<FUIComponent>().Get(FUI5V5Map.UIPackageName) as FUI5V5Map;
            fui5V5Map.BlueProBar.self.TweenValue(UnitComponent.Instance.MyUnit.GetComponent<HeroDataComponent>().GetAttribute(NumericType.Mp), 0.2f);
            fui5V5Map.BlueText.text = $"{fui5V5Map.BlueProBar.self.value}/{fui5V5Map.BlueProBar.self.max}";
        }
    }

    [NumericWatcher(NumericType.Attack)]
    public class Map_ChangeAttack: INumericWatcher
    {
        public void Run(long a, float b)
        {
            if (a != UnitComponent.Instance.MyUnit.Id) return;
            FUI5V5Map fui5V5Map = Game.Scene.GetComponent<FUIComponent>().Get(FUI5V5Map.UIPackageName) as FUI5V5Map;

            fui5V5Map.AttackInfo.text = ((int) b).ToString();
        }
    }

    [NumericWatcher(NumericType.AttackAdd)]
    public class Map_ChangeAttackAdd: INumericWatcher
    {
        public void Run(long a, float b)
        {
            Log.Error("收到了额外攻击力改变事件");
            if (a != UnitComponent.Instance.MyUnit.Id) return;
            FUI5V5Map fui5V5Map = Game.Scene.GetComponent<FUIComponent>().Get(FUI5V5Map.UIPackageName) as FUI5V5Map;
            fui5V5Map.ExtraAttackInfo.text = ((int) b).ToString();
        }
    }
}