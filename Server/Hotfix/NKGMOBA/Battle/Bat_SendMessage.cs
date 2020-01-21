//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2020年1月21日 18:40:49
//------------------------------------------------------------

using ETModel;

namespace ETHotfix.NKGMOBA.Battle
{
    /// <summary>
    /// long为unit的id，float为改变的具体数值
    /// </summary>
    [Event(EventIdType.ChangeHP)]
    public class ChangeHP: AEvent<long, float>
    {
        public override void Run(long a, float b)
        {
            MessageHelper.Broadcast(new M2C_ChangeHeroHP() { UnitId = a, ChangeHPValue = b });
        }
    }
    
    /// <summary>
    /// long为unit的id，float为改变的具体数值
    /// </summary>
    [Event(EventIdType.ChangeMP)]
    public class ChangeMP: AEvent<long, float>
    {
        public override void Run(long a, float b)
        {
            Log.Info("准备发送蓝量改变事件");
            MessageHelper.Broadcast(new M2C_ChangeHeroMP() { UnitId = a, ChangeMPValue = b });
            Log.Info("发送蓝量改变事件成功");
        }
    }
}