//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年5月31日 10:51:11
//------------------------------------------------------------

using ETModel;

namespace ETHotfix
{
    public class FUIHeadBarController
    {
        [Event(EventIdType.CreateHeadBar)]
        public class LoginSuccess_CreateLobbyUI: AEvent<long>
        {
            public override void Run(long fuiId)
            {
                ETModel.Game.Scene.GetComponent<FUIPackageComponent>().AddPackage(FUIPackage.FUIHeadBar);
                var hotfixui = FUIHeadBar.HeadBar.CreateInstance();
                //默认将会以Id为Name，也可以自定义Name，方便查询和管理
                hotfixui.Name = fuiId.ToString();
                hotfixui.MakeFullScreen();
                Game.Scene.GetComponent<FUIComponent>().Add(hotfixui, true);
            }
        }
    }
}