//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年10月3日 9:32:09
//------------------------------------------------------------

namespace ETModel
{
    /// <summary>
    /// Buff添加辅助类
    /// </summary>
    public static class BuffAddHelper
    {
        /// <summary>
        /// 自动添加Buff
        /// </summary>
        /// <param name="self"></param>
        public static void AutoAddBuff(this BuffSystemBase self)
        {
            switch (self.MSkillBuffDataBase.BuffTargetTypes)
            {
                case BuffTargetTypes.Self:
                    self.theUnitFrom.GetComponent<BuffManagerComponent>().AddBuff(self);
                    break;
                case BuffTargetTypes.Others:
                    self.theUnitBelongto.GetComponent<BuffManagerComponent>().AddBuff(self);
                    break;
            }
        }
    }
}