using Sirenix.OdinInspector;

namespace ETModel
{
    public enum BuffAdditionTypes
    {
        [LabelText("百分比Buff")]
        Percentage = 10 << 1,

        [LabelText("值Buff")]
        Value = 10 << 2,

        [LabelText("生命值")]
        HP = 3 << 1,

        [LabelText("法强")]
        Physical = 3 << 3,

        [LabelText("攻击力")]
        Magic = 3 << 5,

        [LabelText("加成方式(生命值)")]
        Value_HP = Value | HP,

        [LabelText("加成方式(生命百分比)")]
        Percentage_HP = Percentage | HP,

        [LabelText("加成方式(攻击值)")]
        Value_Physical = Value | Physical,

        [LabelText("加成方式(攻击值百分比)")]
        Percentage_Physical = Percentage | Physical,

        [LabelText("加成方式(法强值)")]
        Value_Magic = Value | Magic,

        [LabelText("加成方式(法强值百分比)")]
        Percentage_Magic = Percentage | Magic,

        //TODO 可继续拓展
        [LabelText("其他")]
        Other = 5 << 1,
    }
}