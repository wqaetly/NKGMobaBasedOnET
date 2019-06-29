//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年6月29日 17:25:37
//------------------------------------------------------------

using ETModel;

namespace ETHotfix
{
    [Event(EventIdType.LoginFinish)]
    public class LoginFinish_ToTestScene:AEvent
    {
        public override void Run()
        {
            this.EnterMapAsync().Coroutine();
        }
        
        private async ETVoid EnterMapAsync()
        {
            ETModel.Game.EventSystem.Run(ETModel.EventIdType.ShowLoadingUI);
            await MapHelper.EnterMapAsync();
            ETModel.Game.EventSystem.Run(ETModel.EventIdType.CloseLoadingUI);
        }
    }
}