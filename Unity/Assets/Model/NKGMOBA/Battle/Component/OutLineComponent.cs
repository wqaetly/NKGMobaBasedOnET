//此文件格式由工具自动生成

using UnityEngine;

namespace ETModel
{
    #region System

    [ObjectSystem]
    public class OutLineComponentAwakeSystem: AwakeSystem<OutLineComponent>
    {
        public override void Awake(OutLineComponent self)
        {
            self.Awake();
        }
    }

    [ObjectSystem]
    public class OutLineComponentUpdateSystem: UpdateSystem<OutLineComponent>
    {
        public override void Update(OutLineComponent self)
        {
            self.Update();
        }
    }

    [ObjectSystem]
    public class OutLineComponentFixedUpdateSystem: FixedUpdateSystem<OutLineComponent>
    {
        public override void FixedUpdate(OutLineComponent self)
        {
            self.FixedUpdate();
        }
    }

    [ObjectSystem]
    public class OutLineComponentDestroySystem: DestroySystem<OutLineComponent>
    {
        public override void Destroy(OutLineComponent self)
        {
            self.Destroy();
        }
    }

    #endregion

    /// <summary>
    /// 描边组件，用于处理Unit描边
    /// </summary>
    public class OutLineComponent: Component
    {
        #region 私有成员

        private MouseTargetSelectorComponent m_MouseTargetSelectorComponent;

        /// <summary>
        /// 用于替换材质属性的Block
        /// </summary>
        private static MaterialPropertyBlock s_MaterialPropertyBlock = new MaterialPropertyBlock();

        /// <summary>
        /// 缓存的上一帧的目标Unit
        /// </summary>
        private Unit m_CachedUnit;

        /// <summary>
        /// 重置Unit描边信息
        /// </summary>
        private void ResetUnitOutLineInfo(Unit targetUnit)
        {
            if (targetUnit != null)
            {
                UnityEngine.GameObject selfUnitGo = targetUnit.GameObject;
                selfUnitGo.GetRCInternalComponent<Renderer>("Materials").GetPropertyBlock(s_MaterialPropertyBlock);
                s_MaterialPropertyBlock.SetInt("OutLineWidth", 0);
                selfUnitGo.GetRCInternalComponent<Renderer>("Materials").SetPropertyBlock(s_MaterialPropertyBlock);
            }
        }

        #endregion

        #region 公有成员

        #endregion

        #region 生命周期函数

        public void Awake()
        {
            //此处填写Awake逻辑
            m_MouseTargetSelectorComponent = Game.Scene.GetComponent<MouseTargetSelectorComponent>();
        }

        public void Update()
        {
            //此处填写Update逻辑
            if (m_MouseTargetSelectorComponent.TargetUnit != null)
            {
                B2S_RoleCastComponent roleCastComponent = m_MouseTargetSelectorComponent.TargetUnit.GetComponent<B2S_RoleCastComponent>();
                if (roleCastComponent == null)
                {
                    ResetUnitOutLineInfo(m_CachedUnit);
                    m_CachedUnit = null;
                    return;
                }

                if (m_CachedUnit != m_MouseTargetSelectorComponent.TargetUnit)
                {
                    ResetUnitOutLineInfo(m_CachedUnit);
                    m_CachedUnit = m_MouseTargetSelectorComponent.TargetUnit;
                    GameObject selfUnitGo = m_CachedUnit.GameObject;
                    selfUnitGo.GetRCInternalComponent<Renderer>("Materials").GetPropertyBlock(s_MaterialPropertyBlock);
                    s_MaterialPropertyBlock.SetInt("OutLineWidth", 5);
                    if (roleCastComponent.GetRoleCastToTarget(this.GetParent<Unit>()) == RoleCast.Friendly)
                    {
                        s_MaterialPropertyBlock.SetColor("OutLineColor", Color.blue);
                    }
                    else
                    {
                        s_MaterialPropertyBlock.SetColor("OutLineColor", Color.red);
                    }

                    selfUnitGo.GetRCInternalComponent<Renderer>("Materials").SetPropertyBlock(s_MaterialPropertyBlock);
                }
            }
            else
            {
                ResetUnitOutLineInfo(m_CachedUnit);
                m_CachedUnit = null;
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
        }

        #endregion
    }
}