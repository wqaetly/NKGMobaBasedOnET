//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2020年12月11日 16:25:26
//------------------------------------------------------------

namespace ETModel
{
    public enum ModifierType
    {
        /// <summary>
        /// 常数修改器类型
        /// </summary>
        Constant,

        /// <summary>
        /// 百分比修改器类型
        /// </summary>
        Percentage
    }

    public abstract class ADataModifier: IReference
    {
        /// <summary>
        /// 修改器类型
        /// </summary>
        public abstract ModifierType ModifierType { get; }

        /// <summary>
        /// 目标属性名称
        /// </summary>
        public string TargetAttributeName;

        /// <summary>
        /// 获取修改值
        /// </summary>
        /// <returns></returns>
        public abstract float GetModifierValue();

        public abstract void Clear();
    }
}