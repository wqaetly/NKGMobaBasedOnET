//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2020年12月11日 16:24:58
//------------------------------------------------------------

namespace ET
{
    public class PercentageModifier: ADataModifier
    {
        /// <summary>
        /// 百分比
        /// </summary>
        public float Percentage;
        
        public override ModifierType ModifierType
        {
            get
            {
                return ModifierType.Percentage;
            }
        }

        public override float GetModifierValue()
        {
            return Percentage;
        }

        public override void Clear()
        {
            Percentage = 0;
        }
    }
}