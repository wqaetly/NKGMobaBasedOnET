namespace ETModel
{
    [Event(EventIdType.CheckForUpdateFinish)]
    public class CheckForUpdateFinishEvent_RemoveCheckForUpdateUI : AEvent
    {
        public override void Run()
        {
			Game.Scene.GetComponent<UIComponent>().Remove(UIType.CheckForUpdate);
        }
    }
}
