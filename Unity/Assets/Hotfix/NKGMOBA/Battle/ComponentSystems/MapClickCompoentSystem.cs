//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年5月23日 11:09:31
//------------------------------------------------------------

#if !SERVER
using FairyGUI;
using UnityEngine;

namespace ET
{
    public class MapClickComponentAwake : AwakeSystem<MapClickCompoent>
    {
        public override void Awake(MapClickCompoent self)
        {
            self.m_UserInputComponent = Game.Scene.GetComponent<UserInputComponent>();

            self.m_MouseTargetSelectorComponent = self.GetParent<Room>().GetComponent<MouseTargetSelectorComponent>();
        }
    }
    
    public class MapClickComponentUpdate : UpdateSystem<MapClickCompoent>
    {
        public override void Update(MapClickCompoent self)
        {
            if (self.m_UserInputComponent.RightMouseDown)
            {
                // 状态帧系统测试代码

                if (Stage.isTouchOnUI) //点在了UI上
                {
                    //Log.Info("点在UI上");
                }
                else //没有点在UI上
                {
                    if (self.m_MouseTargetSelectorComponent.TargetGameObject?.GetComponent<MonoBridge>().CustomTag ==
                        "Map")
                    {
                        self.MapPathFinder(self.m_MouseTargetSelectorComponent.TargetHitPoint);
                    }
                }
            }
        }
    }

    public static class MapClickSystems
    {
        public static void MapPathFinder(this MapClickCompoent self, Vector3 ClickPoint)
        {
            Room room = self.GetParent<Room>();
            UnitComponent unitComponent = room.GetComponent<UnitComponent>();

            unitComponent.MyUnit.SendPathFindCmd(ClickPoint);
        }
    }
}
#endif