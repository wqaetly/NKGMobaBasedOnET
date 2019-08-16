//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年8月16日 21:39:18
//------------------------------------------------------------

using ETModel;

namespace ETHotfix
{
    [Event(EventIdType.CreateUnitComplete)]
    public class CreateUnitComplete:AEvent
    {
        public override void Run()
        {
            // 给自己的Unit添加引用
            ETModel.Game.Scene.GetComponent<UnitComponent>().MyUnit =
                    ETModel.Game.Scene.GetComponent<UnitComponent>().Get(PlayerComponent.Instance.MyPlayer.UnitId);
            ETModel.Game.Scene.GetComponent<UnitComponent>().MyUnit
                    .AddComponent<CameraComponent, Unit>(ETModel.Game.Scene.GetComponent<UnitComponent>().MyUnit);
        }
    }
}