namespace ETModel
{
    [Event(EventIdType.CheckForUpdateBegin)]
    public class CheckForUpdateBeginEvent_CreateCheckForUpdateUI: AEvent
    {
        public override void Run()
        {
            FUI fui = FUICheckForResUpdateFactory.Create();
            Game.Scene.GetComponent<FUIComponent>().Add(fui);
            fui.AddComponent<FUICheckForResUpdateComponent>();
        }
    }
}