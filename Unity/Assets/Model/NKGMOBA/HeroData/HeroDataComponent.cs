//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年8月14日 21:37:53
//------------------------------------------------------------

namespace ETModel
{
    [ObjectSystem]
    public class HeroDataComponentSystem: AwakeSystem<HeroDataComponent, long>
    {
        public override void Awake(HeroDataComponent self, long a)
        {
            self.NodeDataForHero = Game.Scene.GetComponent<HeroBaseDataRepositoryComponent>().GetHeroDataById(a);
            self.MaxLifeValue = self.NodeDataForHero.OriHP + self.NodeDataForHero.ExtHP + self.NodeDataForHero.GroHP;
            self.CurrentLifeValue = self.MaxLifeValue;
            self.MaxMagicValue = self.NodeDataForHero.OriMagicValue + self.NodeDataForHero.ExtMagicValue + self.NodeDataForHero.GroMagicValue;
            self.CurrentMagicValue = self.MaxMagicValue;
        }
    }
    
    /// <summary>
    /// 英雄数据组件，负责管理英雄数据
    /// </summary>
    public class HeroDataComponent: Component
    {
        public NodeDataForHero NodeDataForHero;

        public float CurrentLifeValue;

        public float MaxLifeValue;

        public float CurrentMagicValue;

        public float MaxMagicValue;
    }
}