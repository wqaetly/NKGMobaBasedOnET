//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年5月3日 19:33:12
//------------------------------------------------------------

namespace ETModel
{
    [Event(EventIdType.CloseLoadingUI)]
    public class RequireCloseLoadingUI_CloseLoadingUI: AEvent
    {
        public override void Run()
        {
            Game.Scene.GetComponent<FUIComponent>().Get("FUILoading").Visible = false;
            Log.Info("加载UI关闭");
        }
    }
}