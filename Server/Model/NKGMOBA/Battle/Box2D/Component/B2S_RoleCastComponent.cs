//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2020年1月21日 17:07:02
//------------------------------------------------------------

namespace ETModel
{
    public enum RoleCast
    {
        /// <summary>
        /// 友善的
        /// </summary>
        Friendly,

        /// <summary>
        /// 敌对的
        /// </summary>
        Adverse,

        /// <summary>
        /// 中立的
        /// </summary>
        Neutral
    }

    [System.Flags]
    public enum RoleCamp
    {
        TianZai = 0b1,
        HuiYue = 0b10,
        JunHeng = 0b100
    }

    public class B2S_RoleCastComponent: Component
    {
        /// <summary>
        /// 归属阵营
        /// </summary>
        public RoleCamp RoleCamp;

        /// <summary>
        /// 获取与目标的关系
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        public RoleCast GetRoleCastToTarget(Unit unit)
        {
            if (unit.GetComponent<B2S_RoleCastComponent>() == null)
            {
                return RoleCast.Friendly;
            }

            RoleCamp roleCamp = unit.GetComponent<B2S_RoleCastComponent>().RoleCamp;
            
            if (roleCamp == this.RoleCamp)
            {
                return RoleCast.Friendly;
            }

            switch (roleCamp | this.RoleCamp)
            {
                case RoleCamp.TianZai | RoleCamp.HuiYue:
                    return RoleCast.Adverse;
                case RoleCamp.TianZai | RoleCamp.JunHeng:
                case RoleCamp.HuiYue | RoleCamp.JunHeng:
                    return RoleCast.Neutral;
            }

            return RoleCast.Friendly;
        }
    }
}