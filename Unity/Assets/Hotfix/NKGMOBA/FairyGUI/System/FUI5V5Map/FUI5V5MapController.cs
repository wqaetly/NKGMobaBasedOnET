//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年5月21日 19:41:22
//------------------------------------------------------------

using ETHotfix.FUI5v5Map;
using ETModel;

namespace ETHotfix
{
    [Event(EventIdType.Show5v5MapUI)]
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
            var hotfixui = FUI5v5Map.FUI5V5Map.CreateInstance();
            //默认将会以Id为Name，也可以自定义Name，方便查询和管理
            hotfixui.Name = FUIPackage.FUI5v5Map;
            //设置UI为全屏大小
            hotfixui.MakeFullScreen();
            //将UI注册到FUIComponent中，正式显示
            Game.Scene.GetComponent<FUIComponent>().Add(hotfixui, true);
        }
    }

    [Event(EventIdType.SetSelfHeroDataOnUI)]
    public class SetSelfHeroDataOnUI: AEvent
    {
        public override void Run()
        {
            NodeDataForHero mNodeDataForHero =
                    ETModel.Game.Scene.GetComponent<UnitComponent>().MyUnit.GetComponent<HeroDataComponent>().NodeDataForHero;
            FUI5V5Map fui5v5Map = (FUI5V5Map) Game.Scene.GetComponent<FUIComponent>().Get(FUI5V5Map.UIPackageName);
            fui5v5Map.AttackInfo.text = (mNodeDataForHero.OriAttackValue + mNodeDataForHero.ExtAttackValue).ToString();
        }
    }
}