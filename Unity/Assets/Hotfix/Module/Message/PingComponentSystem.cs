using System;
using ET.EventType;
using UnityEngine;

namespace ET
{
    public class FinishEnterMap_BeginPing : AEvent<EventType.FinishEnterMap>
    {
        protected override async ETTask Run(FinishEnterMap a)
        {
            Game.Scene.GetComponent<PlayerComponent>().GateSession.GetComponent<PingComponent>().PingAsync()
                .Coroutine();

            await ETTask.CompletedTask;
        }
    }

    [ObjectSystem]
    public class PingComponentDestroySystem : DestroySystem<PingComponent>
    {
        public override void Destroy(PingComponent self)
        {
            self.C2GPingValue = default;
        }
    }

    public static class PingComponentUtilities
    {
        public static async ETVoid PingAsync(this PingComponent self)
        {
            Session session = self.GetParent<Session>();
            long instanceId = self.InstanceId;

            while (true)
            {
                if (self.InstanceId != instanceId)
                {
                    return;
                }

                try
                {
                    long clientNow_C2GSend = TimeHelper.ClientNow();

                    G2C_Ping responseFromGate = await session.Call(self.C2G_Ping) as G2C_Ping;

                    if (self.InstanceId != instanceId)
                    {
                        return;
                    }
                    
                    self.C2GPingValue = TimeHelper.ClientNow() - clientNow_C2GSend;

                    long clientNow_C2MSend = TimeHelper.ClientNow();
                    
                    M2C_Ping responseFromMap = await session.Call(self.C2M_Ping) as M2C_Ping;
                    
                    float oldPing = self.C2MPingValue;
                    self.C2MPingValue = TimeHelper.ClientNow() - clientNow_C2MSend;

                    if (oldPing != self.C2MPingValue)
                    {
                        Game.EventSystem.Publish(new EventType.PingChange()
                            {
                                C2GPing = self.C2GPingValue,
                                C2MPing = self.C2MPingValue,
                                ServerTimeSnap = responseFromMap.TimePoint,
                                MessageFrame = responseFromMap.ServerFrame,
                                ZoneScene = self.DomainScene()
                            })
                            .Coroutine();
                    }
                    
                    Game.TimeInfo.ServerMinusClientTime = responseFromGate.Time + (clientNow_C2MSend - clientNow_C2GSend) / 2 - clientNow_C2MSend;
                }
                catch (RpcException e)
                {
                    // session断开导致ping rpc报错，记录一下即可，不需要打成error
                    Log.Info($"ping error: {self.Id} {e.Error}");
                    return;
                }
                catch (Exception e)
                {
                    Log.Error($"ping error: \n{e}");
                }

                await TimerComponent.Instance.WaitAsync(1000);
            }
        }
    }
}