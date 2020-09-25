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
                foreach (var buffDataVtdId in this.BuffInfoWillBeAdded)
                {
                    //Log.Info($"层数判定_通过监听机制添加id为{VARIABLE.FlagId}的Buff");
                    Game.Scene.GetComponent<BuffPoolComponent>()
                            .AcquireBuff(a.BuffData.BelongToBuffDataSupportorId, buffDataVtdId.BuffId.Value, a.TheUnitFrom, a.TheUnitBelongto,
                                a.BelongtoRuntimeTree);
                }
            }
        }
    }
}