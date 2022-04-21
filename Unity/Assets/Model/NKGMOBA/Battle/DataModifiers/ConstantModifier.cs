//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2020年12月11日 19:53:54
//------------------------------------------------------------

namespace ET
{
    public class ConstantModifier: ADataModifier
    {
        /// <summary>
        /// 修改的值
        /// </summary>
        public float ChangeValue;

        public override ModifierType ModifierType
        {
            get
            {
                return ModifierType.Constant;
            }
        }

        public override float GetModifierValue()
        {
            return ChangeValue;
        }

        public override void Clear()
        {
            this.ChangeValue = 0;
        }
    }
}