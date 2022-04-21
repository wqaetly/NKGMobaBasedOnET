namespace ET
{
    public class UnitAttributesDataComponentAwakeSystem : AwakeSystem<UnitAttributesDataComponent, long>
    {
        public override void Awake(UnitAttributesDataComponent self, long a)
        {
            self.UnitAttributesNodeDataBase = self.GetParent<Unit>().DomainScene()
                .GetComponent<UnitAttributesDataRepositoryComponent>()
                .GetUnitAttributesDataById_DeepCopy<HeroAttributesNodeData>(10001, a);

            self.NumericComponent = self.Parent.GetComponent<NumericComponent>();

            self.NumericComponent.SetValueWithoutBroadCast(NumericType.Level, 1);
            self.NumericComponent.SetValueWithoutBroadCast(NumericType.MaxLevel, 18);
            self.NumericComponent.SetValueWithoutBroadCast(NumericType.MaxHpBase, self.UnitAttributesNodeDataBase
                .OriHP);
            self.NumericComponent.SetValueWithoutBroadCast(NumericType.MaxHpAdd, self.UnitAttributesNodeDataBase
                .GroHP);
            self.NumericComponent.SetValueWithoutBroadCast(NumericType.MaxHp,
                self.UnitAttributesNodeDataBase.OriHP + self.UnitAttributesNodeDataBase.GroHP);
            self.NumericComponent.SetValueWithoutBroadCast(NumericType.Hp,
                self.UnitAttributesNodeDataBase.OriHP + self.UnitAttributesNodeDataBase.GroHP);

            self.NumericComponent.SetValueWithoutBroadCast(NumericType.MaxMpBase,
                self.UnitAttributesNodeDataBase.OriMagicValue);
            self.NumericComponent.SetValueWithoutBroadCast(NumericType.MaxMpAdd,
                self.UnitAttributesNodeDataBase.GroMagicValue);
            self.NumericComponent.SetValueWithoutBroadCast(NumericType.MaxMp,
                self.UnitAttributesNodeDataBase.OriMagicValue + self.UnitAttributesNodeDataBase.GroMagicValue);
            self.NumericComponent.SetValueWithoutBroadCast(NumericType.Mp,
                self.UnitAttributesNodeDataBase.OriMagicValue + self.UnitAttributesNodeDataBase.GroMagicValue);

            self.NumericComponent.SetValueWithoutBroadCast(NumericType.AttackBase,
                self.UnitAttributesNodeDataBase.OriAttackValue);
            self.NumericComponent.SetValueWithoutBroadCast(NumericType.AttackAdd,
                self.UnitAttributesNodeDataBase.GroAttackValue);
            self.NumericComponent.SetValueWithoutBroadCast(NumericType.Attack,
                self.UnitAttributesNodeDataBase.OriAttackValue + self.UnitAttributesNodeDataBase.GroAttackValue);

            self.NumericComponent.SetValueWithoutBroadCast(NumericType.SpeedBase,
                self.UnitAttributesNodeDataBase.OriMoveSpeed);
            self.NumericComponent.SetValueWithoutBroadCast(NumericType.SpeedAdd,
                self.UnitAttributesNodeDataBase.GroMoveSpeed);
            self.NumericComponent.SetValueWithoutBroadCast(NumericType.Speed,
                self.UnitAttributesNodeDataBase.OriMoveSpeed + self.UnitAttributesNodeDataBase.GroMoveSpeed);

            self.NumericComponent.SetValueWithoutBroadCast(NumericType.ArmorBase,
                self.UnitAttributesNodeDataBase.OriArmor);
            self.NumericComponent.SetValueWithoutBroadCast(NumericType.ArmorAdd,
                self.UnitAttributesNodeDataBase.GroArmor);
            self.NumericComponent.SetValueWithoutBroadCast(NumericType.Armor,
                self.UnitAttributesNodeDataBase.OriArmor + self.UnitAttributesNodeDataBase.GroArmor);

            self.NumericComponent.SetValueWithoutBroadCast(NumericType.MagicResistanceBase,
                self.UnitAttributesNodeDataBase.OriMagicResistance);
            self.NumericComponent.SetValueWithoutBroadCast(NumericType.MagicResistanceAdd,
                self.UnitAttributesNodeDataBase.GroMagicResistance);
            self.NumericComponent.SetValueWithoutBroadCast(NumericType.MagicResistance,
                self.UnitAttributesNodeDataBase.OriMagicResistance +
                self.UnitAttributesNodeDataBase.GroMagicResistance);

            self.NumericComponent.SetValueWithoutBroadCast(NumericType.HPRecBase,
                self.UnitAttributesNodeDataBase.OriHPRec);
            self.NumericComponent.SetValueWithoutBroadCast(NumericType.HPRecAdd,
                self.UnitAttributesNodeDataBase.GroHPRec);
            self.NumericComponent.SetValueWithoutBroadCast(NumericType.HPRec,
                self.UnitAttributesNodeDataBase.OriHPRec + self.UnitAttributesNodeDataBase.GroHPRec);

            self.NumericComponent.SetValueWithoutBroadCast(NumericType.MPRecBase,
                self.UnitAttributesNodeDataBase.OriMagicRec);
            self.NumericComponent.SetValueWithoutBroadCast(NumericType.MPRecAdd,
                self.UnitAttributesNodeDataBase.GroMagicRec);
            self.NumericComponent.SetValueWithoutBroadCast(NumericType.MPRec,
                self.UnitAttributesNodeDataBase.OriMagicRec + self.UnitAttributesNodeDataBase.GroMagicRec);

            self.NumericComponent.SetValueWithoutBroadCast(NumericType.AttackSpeedBase,
                self.UnitAttributesNodeDataBase.OriAttackSpeed);
            self.NumericComponent.SetValueWithoutBroadCast(NumericType.AttackSpeedAdd,
                self.UnitAttributesNodeDataBase.GroAttackSpeed);
            self.NumericComponent.SetValueWithoutBroadCast(NumericType.AttackSpeed,
                self.UnitAttributesNodeDataBase.OriAttackSpeed + self.UnitAttributesNodeDataBase.GroAttackSpeed);

            self.NumericComponent.SetValueWithoutBroadCast(NumericType.AttackSpeedIncome,
                self.UnitAttributesNodeDataBase.OriAttackIncome);

            self.NumericComponent.SetValueWithoutBroadCast(NumericType.CriticalStrikeProbability,
                self.UnitAttributesNodeDataBase.OriCriticalStrikeProbability);

            self.NumericComponent.SetValueWithoutBroadCast(NumericType.CriticalStrikeHarm,
                self.UnitAttributesNodeDataBase.OriCriticalStrikeHarm);

            //法术穿透
            self.NumericComponent.SetValueWithoutBroadCast(NumericType.MagicPenetrationBase, 0);
            self.NumericComponent.SetValueWithoutBroadCast(NumericType.MagicPenetrationAdd, 0);

            self.NumericComponent.SetValueWithoutBroadCast(NumericType.AttackRangeBase,
                self.UnitAttributesNodeDataBase.OriAttackRange);
            self.NumericComponent.SetValueWithoutBroadCast(NumericType.AttackRangeAdd, 0);
            self.NumericComponent.SetValueWithoutBroadCast(NumericType.AttackRange,
                self.UnitAttributesNodeDataBase.OriAttackRange);

            self.NumericComponent.InitOriNumerDic();
        }
    }
}