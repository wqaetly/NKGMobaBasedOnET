//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年5月31日 10:51:11
//------------------------------------------------------------

using ETHotfix.FUIHeadBar;
using ETModel;

namespace ETHotfix
{
    [Event(EventIdType.CreateHeadBar)]
    public class LoginSuccess_CreateHeadBar: AEvent<long>
    {
        public override void Run(long fuiId)
        {
            ETModel.Game.Scene.GetComponent<FUIPackageComponent>().AddPackage(FUIPackage.FUIHeadBar);
            var hotfixui = FUIHeadBar.HeadBar.CreateInstance();
            //默认将会以Id为Name，也可以自定义Name，方便查询和管理
            hotfixui.Name = fuiId.ToString();
            //Log.Info($"这个英雄血条id为{hotfixui.Name}");
            hotfixui.MakeFullScreen();
            Game.Scene.GetComponent<FUIComponent>().Add(hotfixui, true);
        }
    }

    [Event(EventIdType.ChangeHPMax)]
    public class ChangeHPBar_Density: AEvent<long, float>
    {
        public override void Run(long fuiId, float maxHP)
        {
            //Log.Info($"事件收到的血条ID为{fuiId}");
            HeadBar headBar = Game.Scene.GetComponent<FUIComponent>().Get(fuiId) as HeadBar;
            float actual = 0;
            if (maxHP % 100 - 0 <= 0.1f)
            {
                actual = maxHP / 100 + 1;
            }
            else
            {
                actual = maxHP / 100 + 2;
            }

            headBar.HPGapList.numItems = (int) actual;
            headBar.HPGapList.columnGap = (int) (headBar.HPGapList.actualWidth / (actual - 1));
            //ETModel.Log.Info($"此次更新间距：{headBar.HPGapList.columnGap}");
        }
    }
}