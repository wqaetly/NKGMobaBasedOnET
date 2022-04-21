//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2020年9月7日 15:35:42
//------------------------------------------------------------

using ET.EventType;
using Sirenix.OdinInspector;

namespace ET
{
    /// <summary>
    /// 会检查一次是否达到指定层数，然后再决定是否触发事件
    /// </summary>
    public class ListenBuffEvent_CheckOverlay : ListenBuffEvent_Normal
    {
        [LabelText("目标层数")] public int TargetOverlay;

        public override void Run(IBuffSystem a)
        {
            IBuffSystem aBuffSystemBase = a;
            if (aBuffSystemBase.CurrentOverlay == this.TargetOverlay)
            {
                foreach (var buffInfo in this.BuffInfoWillBeAdded)
                {
                    buffInfo.AutoAddBuff(aBuffSystemBase.BuffData.BelongToBuffDataSupportorId,
                        buffInfo.BuffNodeId.Value, aBuffSystemBase.TheUnitFrom, aBuffSystemBase.TheUnitBelongto,
                        aBuffSystemBase.BelongtoRuntimeTree);
                }
            }
        }
    }
}