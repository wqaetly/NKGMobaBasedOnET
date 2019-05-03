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
}