//此文件格式由工具自动生成

using System.Collections.Generic;
using ETModel.NKGMOBA.Battle.Fsm;
using ETModel.NKGMOBA.Battle.State;
using UnityEngine;

namespace ETModel
{
    #region System

    [ObjectSystem]
    public class CommonAttackComponentAwakeSystem: AwakeSystem<CommonAttackComponent>
    {
        public override void Awake(CommonAttackComponent self)
        {
            self.Awake();
        }
    }

    #endregion

    public class CommonAttackComponent: Component
    {
        #region 私有成员

        public StackFsmComponent m_StackFsmComponent;

        /// <summary>
        /// 是否可以进行此次攻击
        /// </summary>
        public bool m_CanAttack;

        /// <summary>
        /// 攻击间隔
        /// </summary>
        public long m_AttackInterval;

        /// <summary>
        /// 上一次的攻击时间点
        /// </summary>
        public long m_LastAttackTime;

        /// <summary>
        /// 上次选中的Unit，用于自动攻击
        /// </summary>
        public Unit m_CachedUnit;

        public ETCancellationTokenSource m_ETCancellationTokenSource;

        #endregion

        #region 公有成员

        #endregion

        #region 生命周期函数

        public void Awake()
        {
            //此处填写Awake逻辑
            m_StackFsmComponent = this.Entity.GetComponent<StackFsmComponent>();
            m_ETCancellationTokenSource = ComponentFactory.Create<ETCancellationTokenSource>();
            m_ETCancellationTokenSource.Cancel();
            this.m_ETCancellationTokenSource = null;
        }

        public override void Dispose()
        {
            if (IsDisposed)
                return;
            base.Dispose();
            //此处填写释放逻辑,但涉及Entity的操作，请放在Destroy中
            m_ETCancellationTokenSource?.Cancel();
            this.m_ETCancellationTokenSource = null;
        }

        public void CancelCommonAttack()
        {
            m_ETCancellationTokenSource?.Cancel();
            m_ETCancellationTokenSource = null;
        }

        #endregion
    }
}