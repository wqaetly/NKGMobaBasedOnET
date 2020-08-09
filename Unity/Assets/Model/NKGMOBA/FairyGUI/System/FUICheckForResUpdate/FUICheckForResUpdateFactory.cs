//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年4月26日 19:06:23
//------------------------------------------------------------

using FairyGUI;

namespace ETModel
{
    public static class FUICheckForResUpdateFactory
    {
        public static FUI Create()
        {
            FUI fui = ComponentFactory.Create<FUI, GObject>(FUICheckForResUpdate.CreateInstance());
            fui.Name = "FUICheckForResUpdate";
            fui.MakeFullScreen();
            return fui;
        }
    }
}