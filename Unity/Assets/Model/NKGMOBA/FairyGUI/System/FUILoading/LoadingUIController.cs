//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年5月3日 19:25:38
//------------------------------------------------------------

namespace ETModel
{
    [Event(EventIdType.CreateLoadingUI)]
    public class CreateLoadingEvent_CreateLoadingUI: AEvent
    {
        public override void Run()
        {
            FUI fui = FUILoadingFactory.Create();
            Game.Scene.GetComponent<FUIComponent>().Add(fui);
            fui.AddComponent<FUILoadingComponent>();
        }
    }
    
    [Event(EventIdType.CloseLoadingUI)]
    public class RequireCloseLoadingUI_CloseLoadingUI: AEvent
    {
        public override void Run()
        {
            Game.Scene.GetComponent<FUIComponent>().Get("FUILoading").Visible = false;
            Log.Info("加载UI关闭");
        }
    }
    
    [Event(EventIdType.ShowLoadingUI)]
    public class RequireLoadingUIEvent_ShowLoadingUI: AEvent
    {
        public override void Run()
        {
            Game.Scene.GetComponent<FUIComponent>().Get("FUILoading").Visible = true;
        }
    }
}