using ET.EventType;

namespace ET
{
    public class View_StopMove: AEvent<EventType.CancelMoveFromFSM>
    {
        protected override async ETTask Run(CancelMoveFromFSM a)
        {
            a.Unit.GetComponent<AnimationComponent>().PlayAnimByStackFsmCurrent();
            await ETTask.CompletedTask;
        }
    }
}