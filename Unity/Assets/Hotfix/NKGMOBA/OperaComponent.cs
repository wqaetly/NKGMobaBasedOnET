using System;
using ETModel;
using UnityEngine;

namespace ETHotfix
{
    [Event(EventIdType.ClickSmallMap)]
    public class SmallMapPathFinder: AEvent<Vector3>
    {
        public override void Run(Vector3 a)
        {
            ETModel.SessionComponent.Instance.Session.Send(new Frame_ClickMap() { X = a.x, Y = a.y, Z = a.z });
        }
    }

    [Event(EventIdType.ClickMap)]
    public class MapPathFinder: AEvent<Vector3>
    {
        public override void Run(Vector3 a)
        {
            Game.Scene.GetComponent<OperaComponent>().MapPathFinder(a);
        }
    }

    public class OperaComponent: Component
    {
        private readonly Frame_ClickMap frameClickMap = new Frame_ClickMap();

        public void MapPathFinder(Vector3 ClickPoint)
        {
            frameClickMap.X = ClickPoint.x;
            frameClickMap.Y = ClickPoint.y;
            frameClickMap.Z = ClickPoint.z;
            ETModel.SessionComponent.Instance.Session.Send(frameClickMap);
            // 测试actor rpc消息
            this.TestActor().Coroutine();
        }

        public async ETVoid TestActor()
        {
            try
            {
                M2C_TestActorResponse response = (M2C_TestActorResponse) await SessionComponent.Instance.Session.Call(
                    new C2M_TestActorRequest() { Info = "actor rpc request" });
                Log.Info(response.Info);
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }
    }
}