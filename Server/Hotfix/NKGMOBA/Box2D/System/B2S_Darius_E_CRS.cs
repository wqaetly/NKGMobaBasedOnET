//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019/8/12 20:06:09
// Description: 此代码switch case与System部分由工具生成，请勿进行增减操作
//------------------------------------------------------------

using ETModel;

namespace ETHotfix
{
    [Event(EventIdType_Collision.B2S_Darius_E_CRS)]
    public class AddB2S_Darius_E_CRSSystem: AEvent<Entity>
    {
        public override void Run(Entity a)
        {
            a.AddComponent<B2S_Darius_E_CRS>();
        }
    }
    [ObjectSystem]
    public class B2S_Darius_E_CRSAwakeSystem: AwakeSystem<B2S_Darius_E_CRS>
    {
        public override void Awake(B2S_Darius_E_CRS self)
        {
            self.Entity.GetComponent<B2S_CollisionResponseComponent>().OnCollideStartAction += self.OnCollideStart;
            self.Entity.GetComponent<B2S_CollisionResponseComponent>().OnCollideSustainAction += self.OnCollideSustain;
            self.Entity.GetComponent<B2S_CollisionResponseComponent>().OnCollideFinishAction += self.OnCollideFinish;
        }
    }
    public class B2S_Darius_E_CRS : Component
    {
        public void OnCollideStart(B2S_HeroColliderData b2SHeroColliderData)
        {
            switch (b2SHeroColliderData.m_B2S_CollisionInstance.BelongGroup)
            {
                case "生命单位":
                    switch (b2SHeroColliderData.m_B2S_CollisionInstance.nodeDataId)
                    {
                        case 10006:
                        //敌方英雄
                            break;
                    }
                    break;
            }
        }

        public void OnCollideSustain(B2S_HeroColliderData b2SHeroColliderData)
        {

        }

        public void OnCollideFinish(B2S_HeroColliderData b2SHeroColliderData)
        {

        }
    }
}
