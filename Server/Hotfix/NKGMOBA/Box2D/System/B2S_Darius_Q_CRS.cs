//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019/8/2 23:29:54
// Description: 此代码switch case部分由工具生成，请勿进行增减操作
//------------------------------------------------------------

using ETModel;

namespace ETHotfix
{
    public class B2S_Darius_Q_CRS : Component
    {
        public void OnCollideStart(B2S_FixtureUserData b2SFixtureUserData)
        {
            switch (b2SFixtureUserData.m_B2S_CollisionInstance.BelongGroup)
            {
                case "生命单位":
                    switch (b2SFixtureUserData.m_B2S_CollisionInstance.nodeDataId)
                    {
                        case 40001: //中立生物
                            break;
                        case 20004: //敌方士兵
                            break;
                        case 20003: //敌方英雄
                            break;
                    }
                    break;
            }
        }

        public void OnCollideSustain(B2S_FixtureUserData b2SFixtureUserData)
        {

        }

        public void OnCollideFinish(B2S_FixtureUserData b2SFixtureUserData)
        {

        }
    }
}
