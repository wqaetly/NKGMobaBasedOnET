namespace ET
{
    public class StopMoveFromFSM : AEvent<EventType.CancelMoveFromFSM>
    {
        protected override async ETTask Run(EventType.CancelMoveFromFSM a)
        {
            a.Unit.Stop();

            await ETTask.CompletedTask;
        }
    }
}