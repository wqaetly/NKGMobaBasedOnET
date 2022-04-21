using ET.EventType;

namespace ET
{
    public class AppInitFinish_PlayBGM : AEvent<EventType.AppStartInitFinish>
    {
        protected override async ETTask Run(AppStartInitFinish a)
        {
            Game.Scene.GetComponent<SoundComponent>().PlayMusic("Sound_BGM", 0, 0.3f, true).Coroutine();
            await ETTask.CompletedTask;
        }
    }
}