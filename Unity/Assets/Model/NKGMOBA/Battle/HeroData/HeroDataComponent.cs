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
            self.NodeDataForHero = Game.Scene.GetComponent<HeroBaseDataRepositoryComponent>().GetHeroDataById_DeepCopy(a);

            self.NumericComponent = self.Entity.GetComponent<NumericComponent>();

            self.NumericComponent.NumericDic[(int) NumericType.Level] = 1;
            self.NumericComponent.NumericDic[(int) NumericType.MaxLevel] = 18;
            
            self.NumericComponent.NumericDic[(int) NumericType.MaxHpBase] = self.NodeDataForHero.OriHP;
            self.NumericComponent.NumericDic[(int) NumericType.MaxHpAdd] = self.NodeDataForHero.GroHP;
            self.NumericComponent.NumericDic[(int) NumericType.MaxHp] = self.NodeDataForHero.OriHP + self.NodeDataForHero.GroHP;

            self.NumericComponent.NumericDic[(int) NumericType.Hp] = self.NodeDataForHero.OriHP + self.NodeDataForHero.GroHP;

            self.NumericComponent.NumericDic[(int) NumericType.MaxMpBase] = self.NodeDataForHero.OriMagicValue;
            self.NumericComponent.NumericDic[(int) NumericType.MaxMpAdd] = self.NodeDataForHero.GroMagicValue;
            self.NumericComponent.NumericDic[(int) NumericType.MaxMp] = self.NodeDataForHero.OriMagicValue + self.NodeDataForHero.GroMagicValue;

            self.NumericComponent.NumericDic[(int) NumericType.AttackBase] = self.NodeDataForHero.OriAttackValue;
            self.NumericComponent.NumericDic[(int) NumericType.AttackAdd] = self.NodeDataForHero.GroAttackValue;
            self.NumericComponent.NumericDic[(int) NumericType.Attack] = self.NodeDataForHero.OriAttackValue + self.NodeDataForHero.GroAttackValue;

            self.NumericComponent.NumericDic[(int) NumericType.SpeedBase] = self.NodeDataForHero.OriAttackSpeed;
            self.NumericComponent.NumericDic[(int) NumericType.SpeedAdd] = self.NodeDataForHero.GroAttackSpeed;
            self.NumericComponent.NumericDic[(int) NumericType.Speed] = self.NodeDataForHero.OriAttackSpeed + self.NodeDataForHero.GroAttackSpeed;

            self.NumericComponent.NumericDic[(int) NumericType.ArmorBase] = self.NodeDataForHero.OriArmor;
            self.NumericComponent.NumericDic[(int) NumericType.ArmorAdd] = self.NodeDataForHero.GroArmor;
            self.NumericComponent.NumericDic[(int) NumericType.Armor] = self.NodeDataForHero.OriArmor + self.NodeDataForHero.GroArmor;

            self.NumericComponent.NumericDic[(int) NumericType.MagicResistanceBase] = self.NodeDataForHero.OriMagicResistance;
            self.NumericComponent.NumericDic[(int) NumericType.MagicResistanceAdd] = self.NodeDataForHero.GroMagicResistance;
            self.NumericComponent.NumericDic[(int) NumericType.MagicResistance] =
                    self.NodeDataForHero.OriMagicResistance + self.NodeDataForHero.GroMagicResistance;

            self.NumericComponent.NumericDic[(int) NumericType.HPRecBase] = self.NodeDataForHero.OriHPRec;
            self.NumericComponent.NumericDic[(int) NumericType.HPRecAdd] = self.NodeDataForHero.GroHPRec;
            self.NumericComponent.NumericDic[(int) NumericType.HPRec] = self.NodeDataForHero.OriHPRec + self.NodeDataForHero.GroHPRec;

            self.NumericComponent.NumericDic[(int) NumericType.MPRecBase] = self.NodeDataForHero.OriMagicRec;
            self.NumericComponent.NumericDic[(int) NumericType.MPRecAdd] = self.NodeDataForHero.GroMagicRec;
            self.NumericComponent.NumericDic[(int) NumericType.MPRec] = self.NodeDataForHero.OriMagicRec + self.NodeDataForHero.GroMagicRec;

            self.NumericComponent.NumericDic[(int) NumericType.AttackSpeedBase] = self.NodeDataForHero.OriAttackSpeed;
            self.NumericComponent.NumericDic[(int) NumericType.AttackSpeedAdd] = self.NodeDataForHero.GroAttackSpeed;
            self.NumericComponent.NumericDic[(int) NumericType.AttackSpeed] =
                    self.NodeDataForHero.OriAttackSpeed + self.NodeDataForHero.GroAttackSpeed;

            self.NumericComponent.NumericDic[(int) NumericType.AttackSpeedIncome] = self.NodeDataForHero.OriAttackIncome;

            self.NumericComponent.NumericDic[(int) NumericType.CriticalStrikeProbability] = self.NodeDataForHero.OriCriticalStrikeProbability;

            self.NumericComponent.NumericDic[(int) NumericType.CriticalStrikeHarm] = self.NodeDataForHero.OriCriticalStrikeHarm;

            //法术穿透
            self.NumericComponent.NumericDic[(int) NumericType.MagicPenetrationBase] = 0;
            self.NumericComponent.NumericDic[(int) NumericType.MagicPenetrationAdd] = 0;

            self.NumericComponent.NumericDic[(int) NumericType.AttackRangeBase] = self.NodeDataForHero.OriAttackRange;
            self.NumericComponent.NumericDic[(int) NumericType.AttackRangeAdd] = 0;
            self.NumericComponent.NumericDic[(int) NumericType.AttackRange] = self.NodeDataForHero.OriAttackRange;
            
            self.NumericComponent.InitOriNumerDic();
        }
    }

    /// <summary>
    /// 英雄数据组件，负责管理英雄数据
    /// </summary>
    public class HeroDataComponent: Component
    {
        public NodeDataForHero NodeDataForHero;

        public NumericComponent NumericComponent;

        public float GetAttribute(NumericType numericType)
        {
            return NumericComponent[numericType];
        }

        public override void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }

            base.Dispose();
            NodeDataForHero = null;
            NumericComponent = null;
        }
    }
}