namespace ETModel
{
    [Event(EventIdType.CheckForUpdateFinish)]
    public class CheckForUpdateFinishEvent_RemoveCheckForUpdateUI : AEvent
    {
        public override void Run()
        {
			Game.Scene.GetComponent<FUIComponent>().Remove("CheckForResUpdate");
			Log.Info("检查更新界面关闭");
        }
    }
}
