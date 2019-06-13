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
        public Camera mainCamera;

        public Unit Unit;

        private Vector3 offenPosition;

        public Camera MainCamera
        {
            get
            {
                return this.mainCamera;
            }
        }

        public void Awake(Unit unit)
        {
            this.mainCamera = Camera.main;
            this.Unit = unit;
            offenPosition = mainCamera.transform.position - this.Unit.Position;
        }

        public void LateUpdate()
        {
            // 摄像机每帧更新位置
            UpdatePosition();
        }

        private void UpdatePosition()
        {
            this.mainCamera.transform.position = this.Unit.Position + offenPosition;
        }
    }
}