//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年5月23日 10:42:59
//------------------------------------------------------------

using ETHotfix.FUI5v5Map;
using ETModel;
using FairyGUI;
using UnityEngine;

namespace ETHotfix
{
    [ObjectSystem]
    public class FUI5V5MapStartSystem: StartSystem<FUI5V5Map>
    {
        public override void Start(FUI5V5Map self)
        {
            self.SmallMapSprite.onRightClick.Add(this.AnyEventHandler);
        }

        void AnyEventHandler(EventContext context)
        {
            Log.Info("点击了小地图");
            Log.Info($"在小地图上的坐标为{((GObject) context.sender).GlobalToLocal(context.inputEvent.position)}");
            Vector2 global2Local = ((GObject) context.sender).GlobalToLocal(context.inputEvent.position);
            Vector2 fgui2Unity = new Vector2(global2Local.x, 200 - global2Local.y);
            Vector3 targetPos = new Vector3(-fgui2Unity.x / (200.0f / 100.0f), 0, -fgui2Unity.y / (200.0f / 100.0f));
            Game.EventSystem.Run(EventIdType.ClickSmallMap, targetPos);
        }
    }
}