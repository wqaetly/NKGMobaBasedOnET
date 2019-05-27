//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年5月27日 12:45:01
//------------------------------------------------------------

using ETModel;

namespace ETHotfix
{
    [ObjectSystem]
    public class HeroBaseDataComponentAwakeSystem: AwakeSystem<HeroBaseDataComponent, NodeDataForHero>
    {
        public override void Awake(HeroBaseDataComponent self, NodeDataForHero a)
        {
            self.Awake(a);
        }
    }

    public class HeroBaseDataComponent: Component
    {
        /// <summary>
        /// 英雄基础数据
        /// </summary>
        public NodeDataForHero m_NodeDataForHero;

        public void Awake(NodeDataForHero nodeDataForHero)
        {
            this.m_NodeDataForHero = nodeDataForHero;
        }
    }
}