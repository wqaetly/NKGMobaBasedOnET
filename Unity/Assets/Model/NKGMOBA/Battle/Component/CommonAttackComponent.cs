//此文件格式由工具自动生成

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

    [ObjectSystem]
    public class CommonAttackComponentUpdateSystem: UpdateSystem<CommonAttackComponent>
    {
        public override void Update(CommonAttackComponent self)
        {
            self.Update();
        }
    }

    #endregion

    public class CommonAttackComponent: Component
    {
        #region 私有成员

        private AnimationComponent m_AnimationComponent;
        private UserInputComponent m_UserInputComponent;
        private MouseTargetSelectorComponent m_MouseTargetSelectorComponent;
        private StackFsmComponent m_StackFsmComponent;

        /// <summary>
        /// 上次选中的Unit，用于自动攻击
        /// </summary>
        private Unit m_CachedUnit;

        private CancellationTokenSource CancellationTokenSource;

        #endregion

        #region 公有成员

        #endregion

        #region 生命周期函数

        public void Awake()
        {
            //此处填写Awake逻辑
            m_AnimationComponent = this.Entity.GetComponent<AnimationComponent>();
            m_StackFsmComponent = this.Entity.GetComponent<StackFsmComponent>();
            m_MouseTargetSelectorComponent = Game.Scene.GetComponent<MouseTargetSelectorComponent>();
            m_UserInputComponent = Game.Scene.GetComponent<UserInputComponent>();
        }

        public void Update()
        {
            //此处填写Update逻辑
            if (m_UserInputComponent.RightMouseDown && m_MouseTargetSelectorComponent.TargetUnit != null)
            {
                if (m_MouseTargetSelectorComponent.TargetUnit.GetComponent<B2S_RoleCastComponent>().RoleCast == RoleCast.Adverse)
                {
                    m_CachedUnit = m_MouseTargetSelectorComponent.TargetUnit;
                    //向服务端发送攻击请求信息
                    SessionComponent.Instance.Session.Send(new C2M_CommonAttack() { TargetUnitId = this.m_CachedUnit.Id });
                }
            }
        }

        public async ETVoid CommonAttackStart(Unit targetUnit)
        {
            this.CancellationTokenSource?.Cancel();
            this.CancellationTokenSource = new CancellationTokenSource();

            //转向目标Unit
            this.Entity.GetComponent<TurnComponent>().Turn(targetUnit.Position);
            this.m_StackFsmComponent.ChangeState<CommonAttackState>(StateTypes.CommonAttack, "Attack", 1);
            await CommonAttack_Internal(targetUnit, this.CancellationTokenSource);
            //此次攻击完成
            this.CancellationTokenSource.Dispose();
            this.CancellationTokenSource = null;
            this.m_StackFsmComponent.RemoveState("Attack");
        }

        private async ETTask CommonAttack_Internal(Unit targetUnit, CancellationTokenSource cancellationTokenSource)
        {
            HeroDataComponent heroDataComponent = this.Entity.GetComponent<HeroDataComponent>();
            float attackPre = heroDataComponent.NodeDataForHero.OriAttackPre / (1 + heroDataComponent.GetAttribute(NumericType.AttackSpeedAdd));
            float attackSpeed = heroDataComponent.GetAttribute(NumericType.AttackSpeed);

            //这里假设诺手原始攻击动画0.32s是动画攻击奏效点
            float animationAttackPoint = 0.32f;

            float animationSpeed = animationAttackPoint / attackPre;
            //播放动画，如果动画播放完成还不能进行下一次普攻，则播放空闲动画
            this.m_AnimationComponent.PlayAnimAndReturnIdelFromStart(StateTypes.CommonAttack, speed: animationSpeed);

            await TimerComponent.Instance.WaitAsync((long) (1 / attackSpeed * 1000), cancellationTokenSource.Token);
            
        }

        public void CancelCommonAttack()
        {
            this.CancellationTokenSource?.Cancel();
            this.CancellationTokenSource = null;
            this.m_StackFsmComponent.RemoveState("Attack");
        }

        public override void Dispose()
        {
            if (IsDisposed)
                return;
            base.Dispose();

            //此处填写释放逻辑,但涉及Entity的操作，请放在Destroy中
            m_UserInputComponent = null;
            m_StackFsmComponent = null;
            m_AnimationComponent = null;
            this.CancellationTokenSource?.Dispose();
            this.CancellationTokenSource = null;
        }

        #endregion
    }
}