//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年5月3日 19:20:45
//------------------------------------------------------------

using FairyGUI;

namespace ETModel
{
    public class FUILoadingFactory
    {
        public static FUI Create()
        {
            FUI fui = ComponentFactory.Create<FUI, GObject>(FUILoading.UI_FUILoading.CreateInstance());
            fui.Name = "FUILoading";
            fui.GObject.sortingOrder = 99999;
            fui.MakeFullScreen();
            return fui;
        }
    }
}