using System.Collections.Generic;
using ET.EventType;

namespace ET
{
    public class LeaveRoomFinish_DestroyAllPlayerCards : AEvent<EventType.LeaveRoom>
    {
        protected override async ETTask Run(LeaveRoom a)
        {
            FUI_RoomComponent fuiRoomComponent =
                a.DomainScene.GetComponent<FUIManagerComponent>().GetFUIComponent<FUI_RoomComponent>(FUI_RoomComponent.FUIRoomListName);

            // 如果离开的人是玩家自己，就清空本地所有玩家卡片
            if (a.PlayerId == Game.Scene.GetComponent<PlayerComponent>().PlayerId)
            {
                fuiRoomComponent.FuiRoom.Visible = false;
                fuiRoomComponent.FuiRoomList.Visible = true;
                FUI_RoomUtilities.RemoveAllPlayerCard(fuiRoomComponent);
            }
            else // 否则就在本地移除离开的玩家卡片
            {
                FUI_RoomUtilities.RemovePlayerCard(fuiRoomComponent, a.PlayerId, a.Camp);
            }

            FUI_RoomUtilities.RefreshRoomListBaseOnRoomData(fuiRoomComponent);
            
            await ETTask.CompletedTask;
        }
    }
}