//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年5月21日 19:41:22
//------------------------------------------------------------

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
            ETModel.Game.Scene.GetComponent<FUIPackageComponent>().AddPackage(FUIPackage.FUI5v5Map);
            var hotfixui = FUI5v5Map.FUI5V5Map.CreateInstance();
            //默认将会以Id为Name，也可以自定义Name，方便查询和管理
            hotfixui.Name = FUIPackage.FUI5v5Map;
            hotfixui.MakeFullScreen();
            Game.Scene.GetComponent<FUIComponent>().Add(hotfixui, true);
        }
    }
}