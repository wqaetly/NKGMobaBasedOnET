//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年5月1日 21:23:09
//------------------------------------------------------------

namespace ETModel
{
    [ObjectSystem]
    public class UserAwakeSystem : AwakeSystem<User,long>
    {
        public override void Awake(User self, long id)
        {
            self.Awake(id);
        }
    }

    /// <summary>
    /// 玩家对象
    /// </summary>
    public sealed class User : Entity
    {
        //用户ID（唯一）
        public long UserID { get; private set; }

        //是否正在匹配中
        public bool IsMatching { get; set; }

        //Gate转发ActorID
        public long ActorID { get; set; }

        public void Awake(long id)
        {
            this.UserID = id;
        }

        public override void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }

            base.Dispose();

            this.IsMatching = false;
            this.ActorID = 0;
        }
    }
}