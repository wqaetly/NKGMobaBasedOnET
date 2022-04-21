using UnityEngine;

namespace ET
{
    [ObjectSystem]
    public class CameraComponentAwakeSystem : AwakeSystem<CameraComponent>
    {
        public override void Awake(CameraComponent self)
        {
            self.Awake();
        }
    }

    [ObjectSystem]
    public class CameraComponentLateUpdateSystem : LateUpdateSystem<CameraComponent>
    {
        public override void LateUpdate(CameraComponent self)
        {
            self.LateUpdate();
        }
    }

    [ObjectSystem]
    public class CameraComponentDestroySystem : DestroySystem<CameraComponent>
    {
        public override void Destroy(CameraComponent self)
        {
            self.m_MainCamera = null;
            self.Unit = null;
        }
    }


    public class CameraComponent : Entity
    {
        // 战斗摄像机
        public Camera m_MainCamera;

        public Unit Unit;

        private Vector3 offenPosition;

        public Camera MainCamera
        {
            get { return this.m_MainCamera; }
        }

        public void Awake()
        {
            this.m_MainCamera = Camera.main;
        }

        public void LateUpdate()
        {
            // 摄像机每帧更新位置
            UpdatePosition();
        }

        private void UpdatePosition()
        {
            if (this.Unit == null)
            {
                return;
            }

            this.m_MainCamera.transform.position = this.Unit.ViewPosition + offenPosition;
        }

        public void SetTargetUnit(Unit unit)
        {
            this.Unit = unit;
            offenPosition = m_MainCamera.transform.position - this.Unit.Position;
        }
    }
}