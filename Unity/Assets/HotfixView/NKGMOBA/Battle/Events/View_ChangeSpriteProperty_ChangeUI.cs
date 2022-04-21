using ET.EventType;

namespace ET
{
    public class View_ChangeSpriteProperty_ChangeUI: AEvent<EventType.UnitChangeProperty>
    {
        protected override async ETTask Run(UnitChangeProperty a)
        {
            await ETTask.CompletedTask;
        }
    }
}