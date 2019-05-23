//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年5月23日 11:05:41
//------------------------------------------------------------

using UnityEngine;

namespace ETModel
{
    [ObjectSystem]
    public class UserInputComponentUpdateSystem: UpdateSystem<UserInputComponent>
    {
        public override void Update(UserInputComponent self)
        {
            self.Update();
        }
    }

    public class UserInputComponent: Component
    {
        private bool m_RightMouseDown;
        public bool RightMouseDown => m_RightMouseDown;
        private bool m_RightMouseUp;
        public bool RightMouseUp => m_RightMouseUp;

        public void Update()
        {
            ResetAllState();
            
            if (Input.GetMouseButtonDown(1))
            {
                this.m_RightMouseDown = true;
                this.m_RightMouseUp = false;
            }

            if (Input.GetKeyDown(KeyCode.A))
            {
                //TODO
            }
            
            if (Input.GetMouseButtonUp(1))
            {
                this.m_RightMouseUp = true;
                this.m_RightMouseDown = false;
            }
        }

        private void ResetAllState()
        {
            this.m_RightMouseDown = false;
            this.m_RightMouseUp = false;
        }
    }
}