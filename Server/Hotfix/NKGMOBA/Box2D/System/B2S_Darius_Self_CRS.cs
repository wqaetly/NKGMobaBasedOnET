//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019/8/14 16:22:55
// Description: 此代码switch case与System部分由工具生成，请勿进行增减操作
//------------------------------------------------------------

using ETModel;

namespace ETHotfix
{
    [Event(EventIdType_Collision.B2S_Darius_Self_CRS)]
    public class AddB2S_Darius_Self_CRSSystem: AEvent<Entity>
    {
        public override void Run(Entity a)
        {
            a.AddComponent<B2S_Darius_Self_CRS>();
        }
    }
    [ObjectSystem]
    public class B2S_Darius_Self_CRSAwakeSystem: AwakeSystem<B2S_Darius_Self_CRS>
    {
        public override void Awake(B2S_Darius_Self_CRS self)
        {
            self.Entity.GetComponent<B2S_CollisionResponseComponent>().OnCollideStartAction += self.OnCollideStart;
            self.Entity.GetComponent<B2S_CollisionResponseComponent>().OnCollideSustainAction += self.OnCollideSustain;
            self.Entity.GetComponent<B2S_CollisionResponseComponent>().OnCollideFinishAction += self.OnCollideFinish;
        }
    }
    public class B2S_Darius_Self_CRS : Component
    {
        public void OnCollideStart(B2S_ColliderEntity b2SColliderEntity)
        {
        }

        public void OnCollideSustain(B2S_ColliderEntity b2SColliderEntity)
        {

        }

        public void OnCollideFinish(B2S_ColliderEntity b2SColliderEntity)
        {

        }
    }
}
