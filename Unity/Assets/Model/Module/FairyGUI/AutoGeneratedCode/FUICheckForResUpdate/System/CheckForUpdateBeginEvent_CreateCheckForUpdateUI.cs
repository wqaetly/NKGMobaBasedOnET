
using FairyGUI;

namespace ETModel
{
    [Event(EventIdType.CheckForUpdateBegin)]
    public class CheckForUpdateBeginEvent_CreateCheckForUpdateUI: AEvent
    {
        public override void Run()
        {
            this.RunAsync().Coroutine();
        }

        public async ETVoid RunAsync()
        {
            FUI fui = await FUICheckForResUpdateFactory.Create();
            Game.Scene.GetComponent<FUIComponent>().Add(fui);
            fui.AddComponent<FUICheckForResUpdateComponent>();
        }
    }
}