//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2020年9月7日 15:35:42
//------------------------------------------------------------

using Sirenix.OdinInspector;

namespace ETModel
{
    /// <summary>
    /// 会检查一次是否达到指定层数，然后再决定是否触发事件
    /// </summary>
    public class ListenBuffEvent_CheckOverlay: ListenBuffEvent_Normal
    {
        [LabelText("目标层数")]
        public int TargetOverlay;

        public override void Run(ABuffSystemBase a)
        {
            //Log.Info($"层数判定_通过监听机制添加Buff");
            if (a.CurrentOverlay == this.TargetOverlay)
            {
                //Log.Info($"直接添加_通过监听机制增加Buff");
                foreach (var buffInfo in this.BuffInfoWillBeAdded)
                {
                    buffInfo.AutoAddBuff(a.BuffData.BelongToBuffDataSupportorId, buffInfo.BuffNodeId.Value, a.TheUnitFrom, a.TheUnitBelongto,
                        a.BelongtoRuntimeTree);
                }
            }
        }
    }
}