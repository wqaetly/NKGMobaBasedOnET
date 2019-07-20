//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年7月20日 20:45:46
//------------------------------------------------------------

namespace ETModel
{
    public class B2S_FixtureUserDataFactory
    {
        public B2S_FixtureUserData CreateFixtureUserData()
        {
            return ComponentFactory.Create<B2S_FixtureUserData>();
        }
    }
}