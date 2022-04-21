using ET.EventType;

namespace ET
{
    public class StackFSMComponentSystem_StateChanged_View_PlayAnim: AEvent<EventType.FSMStateChanged_PlayAnim>
    {
        protected override async ETTask Run(FSMStateChanged_PlayAnim a)
        {
            a.Unit.GetComponent<AnimationComponent>().PlayAnimByStackFsmCurrent();
            await ETTask.CompletedTask;
        }
    }
    
    public class StackFSMComponentSystem_View
    {
        
    }
}