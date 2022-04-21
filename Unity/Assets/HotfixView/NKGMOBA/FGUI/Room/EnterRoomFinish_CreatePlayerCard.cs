using ET.EventType;

namespace ET
{
    public class EnterRoomFinish_CreatePlayerCard : AEvent<EventType.CreatePlayerCard>
    {
        protected override async ETTask Run(CreatePlayerCard a)
        {
            FUI_RoomComponent fuiRoomComponent =
                a.DomainScene.GetComponent<FUIManagerComponent>().GetFUIComponent<FUI_RoomComponent>(FUI_RoomComponent.FUIRoomListName);

            FUI_RoomUtilities.AddPlayerCard(fuiRoomComponent, a.PlayerId, a.PlayerAccount, a.Camp);

            await ETTask.CompletedTask;
        }
    }
}