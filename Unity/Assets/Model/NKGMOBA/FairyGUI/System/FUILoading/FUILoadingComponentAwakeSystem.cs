//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年5月3日 19:21:59
//------------------------------------------------------------

namespace ETModel
{
    [ObjectSystem]
    public class FUILoadingComponentAwakeSystem: AwakeSystem<FUILoadingComponent>
    {
        public override void Awake(FUILoadingComponent self)
        {
            self.FuiLoading = (FUILoading.UI_FUILoading) Game.Scene.GetComponent<FUIComponent>().Get("FUILoading").GObject;
        }
    }
}