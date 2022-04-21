using ET.EventType;

namespace ET
{
    public class View_DebugVisualBox2D
    {
        public class View_ShowDebugVisualBox2D: AEvent<EventType.DebugVisualBox2D>
        {
            protected override async ETTask Run(DebugVisualBox2D a)
            {
                var colliderUnit = a.Unit;
                var belongToUnit = a.Unit.GetComponent<B2S_ColliderComponent>().BelongToUnit;
                belongToUnit.GetComponent<B2S_DebuggerComponent>().AddBox2dCollider(colliderUnit);

                await ETTask.CompletedTask;
            }
        }
    }
}