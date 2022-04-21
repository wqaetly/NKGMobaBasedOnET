using ET.EventType;

namespace ET
{
    public class Event_PlayRunAnimationByMoveSpeed: AEvent<EventType.MoveStart>
    {
        protected override async ETTask Run(MoveStart a)
        {
            if (a.Unit.GetComponent<StackFsmComponent>().GetCurrentFsmState().StateTypes != StateTypes.Run)
            {
                return;
            }

            UnitAttributesDataComponent unitAttributesDataComponent = a.Unit.GetComponent<UnitAttributesDataComponent>();
            float animSpeed = unitAttributesDataComponent.GetAttribute(NumericType.Speed) / unitAttributesDataComponent.GetAttribute(NumericType.SpeedBase);
            a.Unit.GetComponent<AnimationComponent>().PlayAnimByStackFsmCurrent(0.3f, animSpeed);
            
            await ETTask.CompletedTask;
        }
    }
}