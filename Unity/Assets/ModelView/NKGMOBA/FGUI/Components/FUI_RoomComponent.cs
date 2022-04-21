using FairyGUI;

namespace ET
{
    public class FUI_RoomComponent : Entity
    {
        #region 私有成员

        public FUI_Room FuiRoom;
        public static string FUIRoomName = $"{FUIPackage.Room}_Room";
        public static string FUIRoomListName = $"{FUIPackage.Room}_RoomList";
        public FUI_RoomList FuiRoomList;

        /// <summary>
        /// 由阵营获取GList
        /// </summary>
        /// <param name="camp"></param>
        /// <returns></returns>
        public GList GetGListByCamp(int camp)
        {
            if (camp == 0)
            {
                return this.FuiRoom.m_Team1;
            }
            else if (camp == 1)
            {
                return this.FuiRoom.m_Team2;
            }

            return null;
        }

        #endregion
    }
}