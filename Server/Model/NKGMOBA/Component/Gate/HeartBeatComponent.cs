//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年5月6日 13:12:37
//------------------------------------------------------------

using System;
using System.Net;

namespace ETModel
{
    [ObjectSystem]
    public class HeartBeatSystem: UpdateSystem<HeartBeatComponent>
    {
        public override void Update(HeartBeatComponent self)
        {
            self.Update();
        }
    }

    /// <summary>
    /// Session心跳组件(需要挂载到Session上)
    /// </summary>
    public class HeartBeatComponent: Component
    {
        /// <summary>
        /// 更新间隔
        /// </summary>
        public long UpdateInterval = 5;

        /// <summary>
        /// 超出时间
        /// </summary>
        /// <remarks>如果跟客户端连接时间间隔大于在服务器上删除该Session</remarks>
        public long OutInterval = 10;

        /// <summary>
        /// 记录时间
        /// </summary>
        private long _recordDeltaTime = 0;

        /// <summary>
        /// 当前Session连接时间
        /// </summary>
        public long CurrentTime = 0;


        public void Update()
        {
            // 如果没有到达发包时间、直接返回
            if ((TimeHelper.ClientNowSeconds() - this._recordDeltaTime) < this.UpdateInterval || this.CurrentTime == 0) return;
            // 记录当前时间
            this._recordDeltaTime = TimeHelper.ClientNowSeconds();

            if (TimeHelper.ClientNowSeconds() - CurrentTime > OutInterval)
            {
                //Console.WriteLine("心跳失败");
                Game.Scene.GetComponent<NetOuterComponent>().Remove(this.Parent.InstanceId);
                Game.Scene.GetComponent<NetInnerComponent>().Remove(this.Parent.InstanceId);
            }
            else
            {
                Console.WriteLine("心跳成功");
            }
        }
    }
}