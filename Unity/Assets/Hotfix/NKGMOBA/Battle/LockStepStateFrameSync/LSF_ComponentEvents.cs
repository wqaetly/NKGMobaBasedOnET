using System;
using System.Collections.Generic;
using ET.EventType;


namespace ET
{
#if SERVER
#else
    /// <summary>
    /// 根据Ping值的改变来改变Tick频率
    /// </summary>
    public class RTTChanged_ChangeFixedUpdateFPS : AEvent<EventType.PingChange>
    {
        protected override async ETTask Run(PingChange a)
        {
            LSF_Component lsfComponent = Game.Scene.GetComponent<PlayerComponent>()
                .BelongToRoom.GetComponent<LSF_Component>();

            lsfComponent.HalfRTT = a.C2MPing % 2 == 0 ? a.C2MPing / 2 : a.C2MPing / 2 + 1;
            
            int targetAheadOfFrame =
                (int)TimeAndFrameConverter.Frame_Long2Frame(lsfComponent.HalfRTT) + (int)lsfComponent.BufferFrame;

            lsfComponent.TargetAheadOfFrame =
                targetAheadOfFrame > LSF_Component.AheadOfFrameMax
                    ? LSF_Component.AheadOfFrameMax
                    : targetAheadOfFrame;
            
            lsfComponent.RefreshNetInfo(a.ServerTimeSnap, a.MessageFrame);

            await ETTask.CompletedTask;
        }
    }

    /// <summary>
    /// 服务端告诉进入地图后再开始Tick
    /// </summary>
    public class FinishEnterMap_BeginFrameSync : AEvent<EventType.FinishEnterMap>
    {
        protected override async ETTask Run(FinishEnterMap a)
        {
            LSF_Component lsfComponent = Game.Scene.GetComponent<PlayerComponent>()
                .BelongToRoom.GetComponent<LSF_Component>();
            lsfComponent.StartFrameSync();
            await ETTask.CompletedTask;
        }
    }
#endif
}