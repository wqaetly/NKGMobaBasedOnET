namespace ET
{
    public enum NumericType
    {
        //最小值，小于此值的都被认为是原始属性
        Min = 10000,

        //生命值
        Hp = 1001,

        //最大生命值
        MaxHp = 1002,
        MaxHpBase = MaxHp * 10 + 1,
        MaxHpAdd = MaxHp * 10 + 2,

        //魔法值
        Mp = 1003,

        //最大魔法值
        MaxMp = 1004,
        MaxMpBase = MaxMp * 10 + 1,
        MaxMpAdd = MaxMp * 10 + 2,

        //速度
        Speed = 1005,
        SpeedBase = Speed * 10 + 1,
        SpeedAdd = Speed * 10 + 2,

        //攻击力
        Attack = 1006,
        AttackBase = Attack * 10 + 1,
        AttackAdd = Attack * 10 + 2,

        //法强
        MagicStrength = 1007,
        MagicStrengthBase = MagicStrength * 10 + 1,
        MagicStrengthAdd = MagicStrength * 10 + 2,

        //护甲
        Armor = 1008,
        ArmorBase = Armor * 10 + 1,
        ArmorAdd = Armor * 10 + 2,

        //魔抗
        MagicResistance = 1009,
        MagicResistanceBase = MagicResistance * 10 + 1,
        MagicResistanceAdd = MagicResistance * 10 + 2,

        //护甲穿透
        ArmorPenetration = 1010,
        ArmorPenetrationBase = ArmorPenetration * 10 + 1,
        ArmorPenetrationAdd = ArmorPenetration * 10 + 2,

        //法术穿透
        MagicPenetration = 1011,
        MagicPenetrationBase = MagicPenetration * 10 + 1,
        MagicPenetrationAdd = MagicPenetration * 10 + 2,

        //暴击率
        CriticalStrikeProbability = 1012,

        //技能冷却缩减
        SkillCD = 1013,

        //生命恢复
        HPRec = 1014,
        HPRecBase = HPRec * 10 + 1,
        HPRecAdd = HPRec * 10 + 2,

        //魔法恢复
        MPRec = 1015,
        MPRecBase = MPRec * 10 + 1,
        MPRecAdd = MPRec * 10 + 2,

        //攻击速度
        AttackSpeed = 1016,
        AttackSpeedBase = AttackSpeed * 10 + 1,
        AttackSpeedAdd = AttackSpeed * 10 + 2,

        //攻速收益
        AttackSpeedIncome = 1017,

        //等级
        Level = 1018,

        //最大等级
        MaxLevel = 1019,

        //暴击伤害
        CriticalStrikeHarm = 1020,

        //攻击距离
        AttackRange = 1021,
        AttackRangeBase = AttackRange * 10 + 1,
        AttackRangeAdd = AttackRange * 10 + 2,
    }
}