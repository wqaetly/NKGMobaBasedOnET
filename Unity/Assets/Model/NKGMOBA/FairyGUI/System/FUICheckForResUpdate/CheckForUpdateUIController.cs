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

    [Event(EventIdType.CheckForUpdateFinish)]
    public class CheckForUpdateFinishEvent_RemoveCheckForUpdateUI: AEvent
    {
        public override void Run()
        {
            Game.Scene.GetComponent<FUIComponent>().Remove("FUICheckForResUpdate");
        }
    }
}