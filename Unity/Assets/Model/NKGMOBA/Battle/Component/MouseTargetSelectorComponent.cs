//此文件格式由工具自动生成

using UnityEngine;

namespace ETModel
{
    #region System

    [ObjectSystem]
    public class MouseTargetSelectorComponentComponentAwakeSystem: AwakeSystem<MouseTargetSelectorComponent>
    {
        public override void Awake(MouseTargetSelectorComponent self)
        {
            self.Awake();
        }
    }

    [ObjectSystem]
    public class MouseTargetSelectorComponentComponentUpdateSystem: UpdateSystem<MouseTargetSelectorComponent>
    {
        public override void Update(MouseTargetSelectorComponent self)
        {
            self.Update();
        }
    }

    [ObjectSystem]
    public class MouseTargetSelectorComponentComponentFixedUpdateSystem: FixedUpdateSystem<MouseTargetSelectorComponent>
    {
        public override void FixedUpdate(MouseTargetSelectorComponent self)
        {
            self.FixedUpdate();
        }
    }

    [ObjectSystem]
    public class MouseTargetSelectorComponentComponentDestroySystem: DestroySystem<MouseTargetSelectorComponent>
    {
        public override void Destroy(MouseTargetSelectorComponent self)
        {
            self.Destroy();
        }
    }

    #endregion

    /// <summary>
    /// 用于鼠标选择目标的组件，功能类似于UserInputComponent，需要指定目标的其余组件可以从这个组件来获取目标对象
    /// </summary>
    public class MouseTargetSelectorComponent: Component
    {
        #region 私有成员

        private Camera m_MainCamera;

        private int m_TargetLayerInfo;

        /// <summary>
        /// 射线击中Gameobject
        /// </summary>
        public GameObject TargetGameObject;

        /// <summary>
        /// 射线击中Unit
        /// </summary>
        public Unit TargetUnit;

        /// <summary>
        /// 射线击中的点
        /// </summary>
        public Vector3 TargetHitPoint;

        #endregion
        
        #region 公有成员

        /// <summary>
        /// 重置目标对象数据
        /// </summary>
        public void ResetTargetInfo()
        {
            this.TargetGameObject = null;
            this.TargetUnit = null;
            this.TargetHitPoint = Vector3.zero;
        }
        
        #endregion

        #region 生命周期函数

        public void Awake()
        {
            //此处填写Awake逻辑
            m_MainCamera = Camera.main;
            m_TargetLayerInfo = LayerMask.GetMask("Map", "Unit");
        }

        public void Update()
        {
            this.ResetTargetInfo();
            //此处填写Update逻辑
            if (Physics.Raycast(m_MainCamera.ScreenPointToRay(Input.mousePosition), out RaycastHit hitInfo, 1000, m_TargetLayerInfo))
            {
                this.TargetHitPoint = hitInfo.point;
                this.TargetGameObject = hitInfo.transform.gameObject;
                Unit unit = hitInfo.transform.GetComponent<MonoBridge>().BelongToUnit;
                if (unit != null)
                {
                    this.TargetUnit = unit;
                }
            }
        }

        public void FixedUpdate()
        {
            //此处填写FixedUpdate逻辑
        }

        public void Destroy()
        {
            //此处填写Destroy逻辑
        }

        public override void Dispose()
        {
            if (IsDisposed)
                return;
            base.Dispose();
            //此处填写释放逻辑,但涉及Entity的操作，请放在Destroy中
            m_MainCamera = null;
            this.ResetTargetInfo();
        }

        #endregion
    }
}