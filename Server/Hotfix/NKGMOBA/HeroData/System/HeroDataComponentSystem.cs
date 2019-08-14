//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年8月14日 21:50:19
//------------------------------------------------------------

using ETModel;

namespace ETHotfix
{
    [ObjectSystem]
    public class HeroDataComponentSystem: AwakeSystem<HeroDataComponent, long>
    {
        public override void Awake(HeroDataComponent self, long a)
        {
            self.NodeDataForHero = Game.Scene.GetComponent<HeroBaseDataRepositoryComponent>().GetHeroDataById(a);
        }
    }
}