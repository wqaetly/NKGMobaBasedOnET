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
        /// 是否正在移向目标
        /// </summary>
        public bool IsMoveToTarget;

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

            CDInfo attackCDInfo = ReferencePool.Acquire<CDInfo>();
            attackCDInfo.Name = "CommonAttack";
            attackCDInfo.Interval = 750;

            CDInfo moveCDInfo = ReferencePool.Acquire<CDInfo>();
            moveCDInfo.Name = "MoveToAttack";
            moveCDInfo.Interval = 300;

            CDComponent.Instance.AddCDData(this.Entity.Id, attackCDInfo);
            CDComponent.Instance.AddCDData(this.Entity.Id, moveCDInfo);
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