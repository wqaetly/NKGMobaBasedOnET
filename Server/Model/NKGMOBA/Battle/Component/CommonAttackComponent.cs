//此文件格式由工具自动生成

using System.Collections.Generic;
using System.Threading;
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
        public bool CanAttack;

        /// <summary>
        /// 攻击间隔
        /// </summary>
        public long AttackInterval;

        /// <summary>
        /// 上一次的攻击时间点
        /// </summary>
        public long LastAttackTime;

        /// <summary>
        /// 是否可以走向目标
        /// </summary>
        public bool CanMoveToTarget = true;

        /// <summary>
        /// 是否正在移向目标
        /// </summary>
        public bool IsMoveToTarget;

        /// <summary>
        /// 上一次移动时间点
        /// </summary>
        public long LastMoveToTime;

        /// <summary>
        /// 走向目标的间隔，默认为0.1s，防止频繁切换状态
        /// </summary>
        public long MoveToTargetInterval = 300;

        /// <summary>
        /// 上次选中的Unit，用于自动攻击
        /// </summary>
        public Unit CachedUnitForAttack;

        public CancellationTokenSource CancellationTokenSource;

        #endregion

        #region 公有成员

        /// <summary>
        /// 取消攻击并且重置攻击对象
        /// </summary>
        public void CancelCommonAttack()
        {
            Game.EventSystem.Run(EventIdType.CancelAttack, this.Entity.Id);
            this.CachedUnitForAttack = null;
        }

        #endregion

        #region 生命周期函数

        public void Awake()
        {
            //此处填写Awake逻辑
            m_StackFsmComponent = this.Entity.GetComponent<StackFsmComponent>();
            this.CancellationTokenSource = new CancellationTokenSource();
            this.CancellationTokenSource = null;
        }

        public override void Dispose()
        {
            if (IsDisposed)
                return;
            base.Dispose();
            //此处填写释放逻辑,但涉及Entity的操作，请放在Destroy中
            this.CancellationTokenSource?.Cancel();
            this.CancellationTokenSource = null;
        }

        #endregion
    }
}