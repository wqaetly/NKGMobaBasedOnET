//此文件格式由工具自动生成

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
        /// 是否可以进行此次攻击
        /// </summary>
        public bool CanAttack = true;

        /// <summary>
        /// 上次选中的Unit，用于自动攻击
        /// </summary>
        private Unit m_CachedUnit;

        private ETCancellationTokenSource m_ETCancellationTokenSource;

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
            if (!CanAttack)
            {
                return;
            }
            //此处填写Update逻辑
            if (m_UserInputComponent.RightMouseDown && m_MouseTargetSelectorComponent.TargetUnit != null)
            {
                if (m_MouseTargetSelectorComponent.TargetUnit.GetComponent<B2S_RoleCastComponent>().RoleCast == RoleCast.Adverse)
                {
                    //如果之前选中过目标
                    if (m_CachedUnit != null)
                    {
                        //如果之前的目标与当前选中目标不是同一个，说明更改了攻击目标，重新攻击
                        //TODO 这里差一个将目标传送到服务器的操作，因为有些技能会需要根据目标血量来更改攻速
                        if (m_CachedUnit != m_MouseTargetSelectorComponent.TargetUnit)
                        {
                            m_CachedUnit = m_MouseTargetSelectorComponent.TargetUnit;
                            CommonAttackStart(m_CachedUnit).Coroutine();
                            return;
                        }

                        //如果上一次攻击已经完成就再次进行攻击
                        if (m_ETCancellationTokenSource == null || m_ETCancellationTokenSource.IsDisposed)
                        {
                            CommonAttackStart(m_CachedUnit).Coroutine();
                        }
                    }
                    else //如果这是第一次攻击
                    {
                        m_CachedUnit = m_MouseTargetSelectorComponent.TargetUnit;
                        CommonAttackStart(m_CachedUnit).Coroutine();
                    }
                }
            }
            else
            {
                //如果攻击目标不为空，且为有效目标，且自身处于攻击状态，且上一次的攻击已经完成
                if (m_CachedUnit != null && !m_CachedUnit.IsDisposed &&
                    this.m_StackFsmComponent.GetCurrentFsmState().StateTypes == StateTypes.CommonAttack &&
                    (this.m_ETCancellationTokenSource == null || this.m_ETCancellationTokenSource.IsDisposed))
                {
                    //目标距离大于当前攻击距离会先进行寻路
                    if (Vector3.Distance(this.Entity.GameObject.transform.position, m_CachedUnit.Position) > 5)
                    {
                        SessionComponent.Instance.Session.Send(new Frame_ClickMap()
                        {
                            X = m_CachedUnit.Position.x, Y = this.m_CachedUnit.Position.y, Z = this.m_CachedUnit.Position.z
                        });
                    }
                    else
                    {
                        CommonAttackStart(m_CachedUnit).Coroutine();
                    }
                }
            }
        }

        private async ETVoid CommonAttackStart(Unit targetUnit)
        {
            m_ETCancellationTokenSource?.Cancel();
            m_ETCancellationTokenSource = ComponentFactory.Create<ETCancellationTokenSource>();
            
            //向服务端发送攻击请求信息
            await SessionComponent.Instance.Session.Call(new C2M_CommonAttack() { TargetUnitId = targetUnit.Id });
            
            //转向目标Unit
            this.Entity.GetComponent<TurnComponent>().Turn(targetUnit.Position);
            this.m_StackFsmComponent.ChangeState<CommonAttackState>(StateTypes.CommonAttack, "Attack", 1);
            await CommonAttack_Internal(targetUnit, m_ETCancellationTokenSource);
        }

        private async ETTask CommonAttack_Internal(Unit targetUnit, ETCancellationTokenSource etCancellationTokenSource)
        {
            HeroDataComponent heroDataComponent = this.Entity.GetComponent<HeroDataComponent>();
            float attackPre = heroDataComponent.NodeDataForHero.OriAttackPre / (1 + heroDataComponent.NodeDataForHero.ExtAttackSpeed);
            float attackSpeed = heroDataComponent.NodeDataForHero.OriAttackSpeed + heroDataComponent.NodeDataForHero.ExtAttackSpeed;
            //这里假设诺手原始攻击动画0.32s是动画攻击奏效点
            float animationAttackPoint = 0.32f;
            float animationSpeed = animationAttackPoint / attackPre;
            //播放动画，如果动画播放完成还不能进行下一次普攻，则播放空闲动画
            this.m_AnimationComponent.PlayAnimAndReturnIdelFromStart(StateTypes.CommonAttack, speed: animationSpeed);
            await Game.Scene.GetComponent<TimerComponent>()
                    .WaitAsync((long) (1 / attackSpeed * 1000), etCancellationTokenSource.Token);
            //此次攻击完成
            m_ETCancellationTokenSource.Dispose();
            m_ETCancellationTokenSource = null;
        }

        public void CancelCommonAttack()
        {
            m_ETCancellationTokenSource?.Cancel();
            m_ETCancellationTokenSource = null;
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
            m_ETCancellationTokenSource?.Cancel();
            m_ETCancellationTokenSource = null;
        }

        #endregion
    }
}