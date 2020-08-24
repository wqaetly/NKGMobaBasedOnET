using Sirenix.OdinInspector;

namespace NPBehave
{
    public enum Operator
    {
        [LabelText("已设置")]
        IS_SET,
        [LabelText("未设置")]
        IS_NOT_SET,
        [LabelText("等于")]
        IS_EQUAL,
        [LabelText("不等于")]
        IS_NOT_EQUAL,
        [LabelText("大于等于")]
        IS_GREATER_OR_EQUAL,
        [LabelText("大于")]
        IS_GREATER,
        [LabelText("小于等于")]
        IS_SMALLER_OR_EQUAL,
        [LabelText("小于")]
        IS_SMALLER,
        [LabelText("总是为真")]
        ALWAYS_TRUE
    }
}