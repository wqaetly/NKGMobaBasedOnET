using UnityEngine;

namespace ETModel
{
    [ObjectSystem]
    public class CameraComponentAwakeSystem: AwakeSystem<CameraComponent, Unit>
    {
        public override void Awake(CameraComponent self, Unit unit)
        {
            self.Awake(unit);
        }
    }

    [ObjectSystem]
    public class CameraComponentLateUpdateSystem: LateUpdateSystem<CameraComponent>
    {
        public override void LateUpdate(CameraComponent self)
        {
            self.LateUpdate();
        }
    }

    public class CameraComponent: Component
    {
        // 战斗摄像机
        private Camera m_MainCamera;

        public Unit Unit;

        private Vector3 offenPosition;

        public Camera MainCamera
        {
            get
            {
                return this.m_MainCamera;
            }
        }

        public void Awake(Unit unit)
        {
            this.m_MainCamera = Camera.main;
            this.Unit = unit;
            offenPosition = m_MainCamera.transform.position - this.Unit.Position;
        }

        public void LateUpdate()
        {
            // 摄像机每帧更新位置
            UpdatePosition();
        }

        private void UpdatePosition()
        {
            this.m_MainCamera.transform.position = this.Unit.Position + offenPosition;
        }

        public override void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }
			
            base.Dispose();
            m_MainCamera = null;
            Unit = null;
        }
    }
}