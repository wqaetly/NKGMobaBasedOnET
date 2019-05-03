//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年5月3日 18:29:12
//------------------------------------------------------------

using FairyGUI;

namespace ETModel
{
    [Event(EventIdType.ShowLoadingUI)]
    public class RequireLoadingUIEvent_ShowLoadingUI: AEvent
    {
        public override void Run()
        {
            Game.Scene.GetComponent<FUIComponent>().Get("FUILoading").Visible = true;
        }
    }
}