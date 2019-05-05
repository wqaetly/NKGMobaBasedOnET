//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年5月4日 16:41:35
//------------------------------------------------------------

using ETModel;

namespace ETHotfix
{
    /// <summary>
    /// 用于Session断开时触发下线
    /// </summary>
    public class SessionOfflineComponent: Component
    {
        public override void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }

            base.Dispose();

            //移除连接组件
            Game.Scene.RemoveComponent<SessionComponent>();
            ETModel.Game.Scene.RemoveComponent<ETModel.SessionComponent>();

            Game.Scene.GetComponent<FUIComponent>().Get(FUIPackage.FUILobby);
            //关闭当前UI，回到登录注册界面
            //Game.Scene.GetComponent<FUIStackComponent>().Clear();
            Game.EventSystem.Run(EventIdType.ShowLoginUI);
            ETModel.Log.Info("被挤掉了，兄弟");
        }
    }
}