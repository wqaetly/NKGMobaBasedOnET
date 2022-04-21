//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年9月21日 16:04:06
//------------------------------------------------------------

namespace ET
{
    /// <summary>
    /// 监听Buff回调
    /// </summary>
    public class ListenBuffCallBackBuffSystem: ABuffSystemBase<ListenBuffCallBackBuffData>
    {
        public ListenBuffEvent_Normal ListenBuffEventNormal;
        
        public override void OnExecute(uint currentFrame)
        {
            if (GetBuffDataWithTType.HasOverlayerJudge)
            {
                ListenBuffEventNormal = ReferencePool.Acquire<ListenBuffEvent_CheckOverlay>();
                ListenBuffEventNormal.BuffInfoWillBeAdded = GetBuffDataWithTType.BuffInfoWillBeAdded;
                var listenOverLayer = ListenBuffEventNormal as ListenBuffEvent_CheckOverlay;
                listenOverLayer.TargetOverlay = GetBuffDataWithTType.TargetOverLayer;
            }
            else
            {
                ListenBuffEventNormal = ReferencePool.Acquire<ListenBuffEvent_Normal>();
                ListenBuffEventNormal.BuffInfoWillBeAdded = GetBuffDataWithTType.BuffInfoWillBeAdded;
            }
            this.GetBuffTarget().BelongToRoom.GetComponent<BattleEventSystemComponent>().RegisterEvent($"{this.GetBuffDataWithTType.EventId.Value}{this.TheUnitFrom.Id}", ListenBuffEventNormal);
        }

        public override void OnFinished(uint currentFrame)
        {
            this.GetBuffTarget().BelongToRoom.GetComponent<BattleEventSystemComponent>().UnRegisterEvent($"{this.GetBuffDataWithTType.EventId.Value}{this.TheUnitFrom.Id}", ListenBuffEventNormal);
        }
    }
}