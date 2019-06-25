using Sirenix.OdinInspector;

namespace ETModel
{
    public enum BuffTypes
    {
        [LabelText("百分比Buff")]
        Percentage = 10 << 1,

        [LabelText("值Buff")]
        Value = 10 << 2,

        [LabelText("增益Buff")]
        Buff = 1 << 1,

        [LabelText("减益Buff")]
        DeBuff = 1 << 2,

        [LabelText("百分比Buff")]
        Buff_Percentage = Buff | Percentage,

        [LabelText("值Buff")]
        Buff_Value = Buff | Value,

        [LabelText("百分比DeBuff")]
        DeBuff_Percentage = DeBuff | Percentage,

        [LabelText("值DeBuff")]
        DeBuff_Value = DeBuff | Value,

        [LabelText("Buff加成方式(生命值)")]
        BuffValue_HP = Buff_Value | HP,

        [LabelText("Buff加成方式(生命百分比)")]
        BuffPercentage_HP = Buff_Percentage | HP,

        [LabelText("DeBuff加成方式(生命)")]
        DeBuffValue_HP = DeBuff_Value | HP,

        [LabelText("DeBuff加成方式(生命百分比)")]
        DeBuffPercentage_HP = DeBuff_Percentage | HP,

        [LabelText("Buff加成方式(攻击值)")]
        BuffValue_Physical = Buff_Value | Physical,

        [LabelText("Buff加成方式(攻击值百分比)")]
        BuffPercentage_Physical = Buff_Percentage | Physical,

        [LabelText("DeBuff加成方式(攻击值)")]
        DeBuffValue_Physical = DeBuff_Value | Physical,

        [LabelText("DeBuff加成方式(攻击值百分比)")]
        DeBuffPercentage_Physical = DeBuff_Percentage | Physical,

        [LabelText("Buff加成方式(法强值)")]
        BuffValue_Magic = Buff_Value | Magic,

        [LabelText("Buff加成方式(法强值百分比)")]
        BuffPercentage_Magic = Buff_Percentage | Magic,

        [LabelText("DeBuff加成方式(法强值)")]
        DeBuffValue_Magic = DeBuff_Value | Magic,

        [LabelText("DeBuff加成方式(法强值百分比)")]
        DeBuffPercentage_Magic = DeBuff_Percentage | Magic,

        //TODO 可继续拓展
        [LabelText("其他")]
        Other = 5 << 1,

        [LabelText("生命值")]
        HP = 3 << 1,

        [LabelText("法强")]
        Physical = 3 << 3,

        [LabelText("攻击力")]
        Magic = 3 << 5,
    }
}