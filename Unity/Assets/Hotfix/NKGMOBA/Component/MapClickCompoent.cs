//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年5月23日 11:09:31
//------------------------------------------------------------

using ETModel;
using FairyGUI;
using UnityEngine;

namespace ETHotfix
{
    [ObjectSystem]
    public class MapClickComponentAwake: AwakeSystem<MapClickCompoent, UserInputComponent>
    {
        public override void Awake(MapClickCompoent self, UserInputComponent a)
        {
            self.Awake(a);
        }
    }

    [ObjectSystem]
    public class MapClickComponentUpdate: UpdateSystem<MapClickCompoent>
    {
        public override void Update(MapClickCompoent self)
        {
            self.Update();
        }
    }

    public class MapClickCompoent: Component
    {
        private UserInputComponent m_UserInputComponent;

        public void Awake(UserInputComponent userInputComponent)
        {
            this.m_UserInputComponent = userInputComponent;
        }

        public void Update()
        {
            if (this.m_UserInputComponent.RightMouseDown)
            {
                if (Stage.isTouchOnUI) //点在了UI上
                {
                    Log.Info("点在UI上");
                }
                else //没有点在UI上
                {
                    Log.Info("没点在UI上");
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit, 1000, LayerMask.GetMask("Map")))
                    {
                        Game.EventSystem.Run(EventIdType.ClickMap, hit.point);
                    }
                }
            }
        }
    }
}