//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2021年8月29日 10:32:54
//------------------------------------------------------------

using System.Collections.Generic;
using ET.Hotfix.Demo.Room;
using FairyGUI;

namespace ET
{
    public static class FUI_RoomUtilities
    {
        /// <summary>
        /// 以Room的数据为基础，刷新房间列表
        /// </summary>
        /// <param name="self"></param>
        public static void RefreshRoomListBaseOnRoomData(FUI_RoomComponent self)
        {
            RoomManagerComponent roomManagerComponent = self.DomainScene().GetComponent<RoomManagerComponent>();
            FUIManagerComponent fuiManagerComponent = self.DomainScene().GetComponent<FUIManagerComponent>();

            List<long> handledRoom = new List<long>();
            // 先更新房间列表，仍旧存在的就刷新，不存在的就移除
            for (int i = self.FuiRoomList.m_RoomList.numChildren - 1; i >= 0; i--)
            {
                if (self.FuiRoomList.m_RoomList.GetChildAt(i).data == null)
                {
                    continue;
                }

                string uiIdString = self.FuiRoomList.m_RoomList.GetChildAt(i).data
                    .ToString();
                long uiIdLong = long.Parse(uiIdString);

                Room room = roomManagerComponent.GetLobbyRoom(uiIdLong);
                if (room == null)
                {
                    // 注意这里只需要这样移除一次即可，这里移除后会主动将m_List_RoomList中的元素也移除
                    fuiManagerComponent.Remove(uiIdString);
                    //self.FuiUIPanelLobby.m_UIPanel_RoomList.m_List_RoomList.RemoveChildAt(i);
                }
                else
                {
                    FUI_RoomData fuiRoomData = fuiManagerComponent.GetFUIComponent<FUI_RoomData>(uiIdString);
                    fuiRoomData.m_PlayerNum.text = $"{room.PlayerCount}/6";
                    fuiRoomData.m_RoomName.text = $"{room.RoomName}";
                    handledRoom.Add(uiIdLong);
                }
            }

            // 再尝试从RoomManager数据新增房间
            foreach (var room in roomManagerComponent.LobbyRooms)
            {
                if (handledRoom.Contains(room.Key))
                {
                    continue;
                }

                FUI_RoomData fuiBtnRoom = FUI_RoomData.CreateInstance(self.Domain);

                fuiBtnRoom.self.data = room.Key.ToString();

                fuiBtnRoom.m_PlayerNum.text = $"{room.Value.PlayerCount}/6";
                fuiBtnRoom.m_RoomName.text = $"{room.Value.RoomName}";

                fuiBtnRoom.m_JoinButton.self.onClick.Add(() =>
                {
                    JoinRoomHelper.JoinRoom(self, long.Parse(fuiBtnRoom.self.data.ToString()));
                });

                self.FuiRoomList.m_RoomList.AddChildAt(fuiBtnRoom.self, 0);
                fuiManagerComponent.Add(room.Key.ToString(), fuiBtnRoom, fuiBtnRoom, false);
            }
        }

        public static void RemovePlayerCard(FUI_RoomComponent self, long playerId, int camp)
        {
            FUIManagerComponent fuiManagerComponent = self.DomainScene().GetComponent<FUIManagerComponent>();
            for (int i = self.GetGListByCamp(camp).numChildren - 1; i >= 0; i--)
            {
                if (self.GetGListByCamp(camp).GetChildAt(i).data == null)
                {
                    continue;
                }

                string uiIdString = self.GetGListByCamp(camp).GetChildAt(i).data.ToString();
                long uiIdLong = long.Parse(uiIdString);

                if (uiIdLong == playerId)
                {
                    // 注意这里只需要这样移除一次即可，这里移除后会主动将m_List_InRoom_OtherPlayers中的元素也移除
                    fuiManagerComponent.Remove(self.GetGListByCamp(camp).GetChildAt(i).data.ToString());
                }
            }
        }

        public static void RemoveAllPlayerCard(FUI_RoomComponent self)
        {
            FUIManagerComponent fuiManagerComponent = self.DomainScene().GetComponent<FUIManagerComponent>();

            GList team1 = self.FuiRoom.m_Team1;
            for (int i = team1.numChildren - 1; i >= 0; i--)
            {
                if (team1.GetChildAt(i).data == null)
                {
                    continue;
                }

                string uiIdString = team1.GetChildAt(i).data.ToString();

                // 注意这里只需要这样移除一次即可，这里移除后会主动将m_List_InRoom_OtherPlayers中的元素也移除
                fuiManagerComponent.Remove(uiIdString);
            }
            
            GList team2 = self.FuiRoom.m_Team2;
            for (int i = team2.numChildren - 1; i >= 0; i--)
            {
                if (team2.GetChildAt(i).data == null)
                {
                    continue;
                }

                string uiIdString = team2.GetChildAt(i).data.ToString();
                // 注意这里只需要这样移除一次即可，这里移除后会主动将m_List_InRoom_OtherPlayers中的元素也移除
                fuiManagerComponent.Remove(uiIdString);
            }
        }

        public static void AddPlayerCard(FUI_RoomComponent self, long playerId, string playerAccount, int camp)
        {
            FUIManagerComponent fuiManagerComponent = self.DomainScene().GetComponent<FUIManagerComponent>();
            FUI_PlayerData fuiOtherPlayerCard = FUI_PlayerData.CreateInstance(self.DomainScene());

            fuiOtherPlayerCard.self.data = playerId.ToString();
            fuiOtherPlayerCard.m_RoomPlayerName.text = playerAccount;

            self.GetGListByCamp(camp).AddChildAt(fuiOtherPlayerCard.self, 0);
            fuiManagerComponent.Add(fuiOtherPlayerCard.self.data.ToString(), fuiOtherPlayerCard, fuiOtherPlayerCard,
                false);
        }
    }
}