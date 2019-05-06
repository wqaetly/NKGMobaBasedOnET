//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年5月6日 20:28:45
//------------------------------------------------------------

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
            Game.Scene.RemoveComponent<SessionComponent>();
            ETModel.Game.Scene.RemoveComponent<ETModel.SessionComponent>();
        }
    }
}