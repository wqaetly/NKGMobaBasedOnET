//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年5月23日 11:09:31
//------------------------------------------------------------

using System;
using ETModel;
using FairyGUI;
using UnityEngine;
using UnityEngine.Assertions.Must;

namespace ETHotfix
{
    [ObjectSystem]
    public class MapClickComponentAwake: AwakeSystem<MapClickCompoent>
    {
        public override void Awake(MapClickCompoent self)
        {
            self.Awake();
        }
    }

    [ObjectSystem]
    public class MapClickComponentUpdate: UpdateSystem<MapClickCompoent>
    {
        public override void Update(MapClickCompoent self)
        {
            self.Update();
        }
    }

    [Event(EventIdType.ClickSmallMap)]
    public class SmallMapPathFinder: AEvent<Vector3>
    {
        public override void Run(Vector3 a)
        {
            ETModel.SessionComponent.Instance.Session.Send(new Frame_ClickMap() { X = a.x, Y = a.y, Z = a.z });
        }
    }
    
    public class MapClickCompoent: Component
    {
        private UserInputComponent m_UserInputComponent;

        private MouseTargetSelectorComponent m_MouseTargetSelectorComponent;
        
        private readonly Frame_ClickMap frameClickMap = new Frame_ClickMap();

        public void Awake()
        {
            this.m_UserInputComponent = ETModel.Game.Scene.GetComponent<UserInputComponent>();
            this.m_MouseTargetSelectorComponent = ETModel.Game.Scene.GetComponent<MouseTargetSelectorComponent>();
        }

        public void Update()
        {
            if (this.m_UserInputComponent.RightMouseDown)
            {
                if (Stage.isTouchOnUI) //点在了UI上
                {
                    //Log.Info("点在UI上");
                }
                else //没有点在UI上
                {
                    if (m_MouseTargetSelectorComponent.TargetGameObject?.GetComponent<MonoBridge>().CustomTag == "Map")
                    {
                        MapPathFinder(m_MouseTargetSelectorComponent.TargetHitPoint);
                    }
                }
            }
        }
        
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

        public override void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }
            base.Dispose();
            m_UserInputComponent = null;
            m_MouseTargetSelectorComponent = null;
        }
    }
}