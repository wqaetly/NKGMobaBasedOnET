//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年7月25日 16:38:32
//------------------------------------------------------------

using Boo.Lang;
using Sirenix.OdinInspector;

namespace ETMode
{
    /// <summary>
    /// 碰撞实例
    /// </summary>
    public class B2S_CollisionInstance
    {
        [InfoBox("在这里需要细分敌我",InfoMessageType.Error)]
        [InfoBox("FriendSoldier:己方小兵\nEnemySoldier:敌方小兵\nSelf:自己\nTeammate:队友\nEnemyHeros:敌方英雄\nMonsters:中立生物\nBuildings:建筑物\nBarrier:地形\nOtherHeroCreateCollision:其他英雄的技能所创造的碰撞体")]
        [LabelText("可碰撞对象")]
        [EnumToggleButtons]
        public B2S_AllCollideableObject MB2SAllCollideableObject;
        
        [InfoBox("为了应对特殊情况，比如杰斯强化Q，亚索风墙，输入碰撞体ID即可\n(需要勾选“可碰撞对象中的”OtherHeroCreateCollision)")]
        [LabelText("可碰撞对象拓展")]
        [EnumToggleButtons]
        public List<long> extensionCollisionRelation = new List<long>();

        [Title("碰撞体ID,需要打开Box2D可视化编辑器查看")]
        [HideLabel]
        public long collisionId;
    }
}