//此文件格式由工具自动生成

using System.Collections.Generic;
using System.Threading;
using ETModel.NKGMOBA.Battle.Fsm;
using ETModel.NKGMOBA.Battle.State;
using Sirenix.OdinInspector;
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

    /// <summary>
    /// 攻击执行或取消时执行替换的类型
    /// </summary>
    public enum AttackCastReplaceType
    {
        [LabelText("替换攻击")]
        Attack,

        [LabelText("替换取消攻击")]
        CancelAttack,
    }

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

        /// <summary>
        /// 替换攻击流程目标行为树Id
        /// </summary>
        public long AttackReplaceNPTreeId;

        /// <summary>
        /// 用于替换攻击流程的黑板键值
        /// </summary>
        public NP_BlackBoardRelationData AttackReplaceBB;

        /// <summary>
        /// 替换取消攻击流程目标行为树Id
        /// </summary>
        public long CancelAttackReplaceNPTreeId;

        /// <summary>
        /// 用于替换取消攻击流程的黑板键值
        /// </summary>
        public NP_BlackBoardRelationData CancelAttackReplaceBB;

        public CancellationTokenSource CancellationTokenSource;

        #endregion

        #region 公有成员

        /// <summary>
        /// 设置攻击替换信息
        /// </summary>
        /// <param name="npTreeId"></param>
        /// <param name="attackReplaceBB"></param>
        public void SetAttackReplaceInfo(long npTreeId, NP_BlackBoardRelationData attackReplaceBB)
        {
            this.AttackReplaceNPTreeId = npTreeId;
            this.AttackReplaceBB = attackReplaceBB;
        }

        public bool HasAttackReplaceInfo()
        {
            return this.AttackReplaceBB != null;
        }

        /// <summary>
        /// 设置取消攻击替换流程
        /// </summary>
        /// <param name="npTreeId"></param>
        /// <param name="attackReplaceBB"></param>
        public void SetCancelAttackReplaceInfo(long npTreeId, NP_BlackBoardRelationData attackReplaceBB)
        {
            this.CancelAttackReplaceNPTreeId = npTreeId;
            this.CancelAttackReplaceBB = attackReplaceBB;
        }

        public bool HasCancelAttackReplaceInfo()
        {
            return this.CancelAttackReplaceBB != null;
        }

        /// <summary>
        /// 重置攻击替换信息
        /// </summary>
        public void ReSetAttackReplaceInfo()
        {
            this.AttackReplaceNPTreeId = 0;
            this.AttackReplaceBB = null;
        }

        /// <summary>
        /// 重置取消攻击替换流程
        /// </summary>
        public void ReSetCancelAttackReplaceInfo()
        {
            this.CancelAttackReplaceNPTreeId = 0;
            this.CancelAttackReplaceBB = null;
        }

        /// <summary>
        /// 取消攻击并且重置攻击对象
        /// </summary>
        public void CancelCommonAttack()
        {
            Game.EventSystem.Run(EventIdType.CancelAttack, this.Entity.Id);
        }

        /// <summary>
        /// 取消攻击但不重置攻击对象，比如我们会因为与目标距离大于攻击距离而先寻路
        /// </summary>
        public void CancelAttackWithOutResetAttackTarget()
        {
            Game.EventSystem.Run(EventIdType.CancelAttackWithOutResetAttackTarget, this.Entity.Id);
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
            this.ReSetAttackReplaceInfo();
            this.ReSetCancelAttackReplaceInfo();
        }

        #endregion
    }
}