//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年7月20日 20:42:58
//------------------------------------------------------------

namespace ETModel
{
    [ObjectSystem]
    public class B2S_FixtureUserDataAwake: AwakeSystem<B2S_FixtureUserData>
    {
        public override void Awake(B2S_FixtureUserData self)
        {
            self.UnitId = self.Entity.Id;
        }
    }
    /// <summary>
    /// 碰撞信息的Entity，方便挂载Component来拓展信息
    /// </summary>
    public class B2S_FixtureUserData: Entity
    {
        /// <summary>
        /// 归属Unit的ID
        /// </summary>
        public long UnitId;
    }
}