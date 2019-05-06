//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年5月6日 12:21:19
//------------------------------------------------------------

using System;

namespace ETModel
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

            Console.WriteLine("已经清除该玩家在服务端的残留信息");
        }
    }
}