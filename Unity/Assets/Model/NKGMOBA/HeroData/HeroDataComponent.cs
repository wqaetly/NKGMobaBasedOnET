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

        public int CurrentLevel;

        public float CurrentLifeValue;

        public float MaxLifeValue;

        public float CurrentMagicValue;

        public float MaxMagicValue;

        /// <summary>
        /// 当前攻击力
        /// </summary>
        public float CurrentAttackValue;

        /// <summary>
        /// 当前法强
        /// </summary>
        public float CurrentSpellpower;

        public int Q_SkillLevel;
        public int W_SkillLevel;
        public int E_SkillLevel;
        public int R_SkillLevel;

        /// <summary>
        /// 获取指定技能等级
        /// </summary>
        /// <param name="i">技能序号</param>
        /// <returns></returns>
        public int GetSkillLevel(int i)
        {
            switch (i)
            {
                case 0:
                    return Q_SkillLevel;
                case 1:
                    return W_SkillLevel;
                case 2:
                    return E_SkillLevel;
                case 3:
                    return R_SkillLevel;
            }

            Log.Info($"技能序号获取错误,{i}");
            return -1;
        }
    }
}