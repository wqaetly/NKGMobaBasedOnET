//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年9月16日 21:39:43
//------------------------------------------------------------

namespace ETModel
{
    public abstract class BuffBase
    {
        public BuffTypes MBuffTypes;

        public BuffState MBuffState;

        /// <summary>
        /// Buff初始化
        /// </summary>
        public abstract void OnInit();
        
        /// <summary>
        /// Buff触发
        /// </summary>
        public abstract void OnExecute();

        /// <summary>
        /// Buff持续
        /// </summary>
        public virtual void OnUpdate()
        {
            
        }

        /// <summary>
        /// 重置Buff用
        /// </summary>
        public abstract void OnFinished();
    }
}