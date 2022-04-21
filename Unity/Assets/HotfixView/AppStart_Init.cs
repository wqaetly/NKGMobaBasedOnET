using System.Collections.Generic;
using System.Security.Policy;

namespace ET
{
    public class AppStart_Init : AEvent<EventType.AppStart>
    {
        protected override async ETTask Run(EventType.AppStart args)
        {
            Game.Scene.AddComponent<TimerComponent>();
            Game.Scene.AddComponent<CoroutineLockComponent>();

            Game.Scene.AddComponent<ConfigComponent>();
            await ConfigComponent.Instance.LoadAsync();

            Game.Scene.AddComponent<OpcodeTypeComponent>();
            Game.Scene.AddComponent<MessageDispatcherComponent>();

            Game.Scene.AddComponent<NetThreadComponent>();

            Game.Scene.AddComponent<ZoneSceneManagerComponent>();
            Game.Scene.AddComponent<GlobalComponent>();

            Game.Scene.AddComponent<NumericWatcherComponent>();

            Game.Scene.AddComponent<GameObjectPoolComponent>();

            Game.Scene.AddComponent<SoundComponent>();

            Game.Scene.AddComponent<CameraComponent>();

            Game.Scene.AddComponent<PlayerComponent>();

            Game.Scene.AddComponent<UserInputComponent>();

            Game.Scene.AddComponent<LSF_CmdDispatcherComponent>();
            Game.Scene.AddComponent<LSF_TickDispatcherComponent>();

            Scene zoneScene = await SceneFactory.CreateZoneScene(1, "Game", Game.Scene);

            //显示登陆界面
            await Game.EventSystem.Publish(new EventType.AppStartInitFinish() {ZoneScene = zoneScene});

            ProtoTest.Do();
        }
    }
}