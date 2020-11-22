//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2020年10月21日 15:15:48
//------------------------------------------------------------

using System.Collections.Generic;
using System.Threading;
using ETModel;
using ETModel.NKGMOBA.Battle.Fsm;
using ETModel.NKGMOBA.Battle.State;
using UnityEngine;

namespace ETHotfix
{
    [ObjectSystem]
    public class CommonAttackComponentUpdateSystem: UpdateSystem<CommonAttackComponent>
    {
        public override void Update(CommonAttackComponent self)
        {
            self.Update();
        }
    }

    public static class CommonAttackComponentSystem
    {
        public static void SetAttackTarget(this CommonAttackComponent self, Unit targetUnit)
        {
            if (targetUnit == null)
            {
                Log.Error("普攻组件接受到的targetUnit为null");
                return;
            }

            if (targetUnit.GetComponent<B2S_RoleCastComponent>()?.RoleCast == RoleCast.Adverse)
            {
                if (self.CachedUnitForAttack != targetUnit)
                {
                    self.CancelCommonAttack();
                }

                self.CachedUnitForAttack = targetUnit;
                self.Entity.GetComponent<StackFsmComponent>().ChangeState<CommonAttackState>(StateTypes.CommonAttack, "CommonAttack", 1);
            }
        }

        private static async ETVoid StartCommonAttack(this CommonAttackComponent self)
        {
            self.CancellationTokenSource?.Cancel();
            self.CancellationTokenSource = new CancellationTokenSource();

            MessageHelper.Broadcast(new M2C_CommonAttack()
            {
                AttackCasterId = self.Entity.Id, TargetUnitId = self.CachedUnitForAttack.Id, CanAttack = true
            });
            await self.CommonAttack_Internal();
        }

        private static async ETTask CommonAttack_Internal(this CommonAttackComponent self)
        {
            HeroDataComponent heroDataComponent = self.Entity.GetComponent<HeroDataComponent>();
            float attackPre = heroDataComponent.NodeDataForHero.OriAttackPre / (1 + heroDataComponent.NodeDataForHero.ExtAttackSpeed);
            float attackPos = heroDataComponent.NodeDataForHero.OriAttackPos / (1 + heroDataComponent.NodeDataForHero.ExtAttackSpeed);
            float attackSpeed = heroDataComponent.NodeDataForHero.OriAttackSpeed + heroDataComponent.NodeDataForHero.ExtAttackSpeed;

            //播放动画，如果动画播放完成还不能进行下一次普攻，则播放空闲动画
            await Game.Scene.GetComponent<TimerComponent>().WaitAsync((long) (attackPre * 1000), self.CancellationTokenSource.Token);

            List<NP_RuntimeTree> targetSkillCanvas = self.Entity.GetComponent<SkillCanvasManagerComponent>().GetSkillCanvas(10001);
            foreach (var skillCanva in targetSkillCanvas)
            {
                skillCanva.GetBlackboard().Set("CastNormalAttack", true);
                skillCanva.GetBlackboard().Set("NormalAttackUnitIds", new List<long>() { self.CachedUnitForAttack.Id });
            }

            Game.EventSystem.Run(EventIdType.ChangeHP, self.CachedUnitForAttack.Id, -50.0f);
            self.LastAttackTime = TimeHelper.Now();
            self.CanAttack = false;
            self.AttackInterval = (long) (1 / attackSpeed - attackPre) * 1000;

            await Game.Scene.GetComponent<TimerComponent>().WaitAsync(self.AttackInterval, self.CancellationTokenSource.Token);
            //此次攻击完成
            self.CancellationTokenSource.Dispose();
            self.CancellationTokenSource = null;
        }

        public static void Update(this CommonAttackComponent self)
        {
            if (!self.CanAttack)
            {
                if (TimeHelper.Now() - self.LastAttackTime > self.AttackInterval)
                {
                    self.CanAttack = true;
                }
            }

            if (self.Entity.GetComponent<StackFsmComponent>().GetCurrentFsmState().StateTypes == StateTypes.CommonAttack)
            {
                if (self.CachedUnitForAttack != null && !self.CachedUnitForAttack.IsDisposed)
                {
                    Vector3 selfUnitPos = (self.Entity as Unit).Position;
                    float distance = Vector3.Distance(selfUnitPos, self.CachedUnitForAttack.Position);
                    // Vector3 dir = (self.CachedUnitForAttack.Position - (self.Entity as Unit).Position).normalized;
                    // Vector3 targetPos = self.CachedUnitForAttack.Position + dir * 1.75f;
                    //目标距离大于当前攻击距离会先进行寻路，这里的1.75为175码
                    if (distance >= 1.75)
                    {
                        self.IsMoveToTarget = true;
                        self.Entity.GetComponent<UnitPathComponent>()
                                .MoveTo_InternalWithOutStateChange(self.CachedUnitForAttack.Position)
                                .Coroutine();
                    }
                    else
                    {
                        if (self.IsMoveToTarget)
                        {
                            self.Entity.GetComponent<UnitPathComponent>().CancelMove();
                            M2C_PathfindingResult pathfindingResult = new M2C_PathfindingResult()
                            {
                                X = selfUnitPos.x, Y = selfUnitPos.y, Z = selfUnitPos.z, Id = self.Entity.Id
                            };
                            pathfindingResult.Xs.Clear();
                            pathfindingResult.Ys.Clear();
                            pathfindingResult.Zs.Clear();
                            MessageHelper.Broadcast(pathfindingResult);
                            self.IsMoveToTarget = false;
                        }
                        
                        //目标不为空，且处于攻击状态，且上次攻击已完成或取消
                        if ((self.CancellationTokenSource == null || self.CancellationTokenSource.IsCancellationRequested))
                        {
                            if (distance <= 1.75 && self.CanAttack)
                                self.StartCommonAttack().Coroutine();
                        }
                    }
                }
            }
        }
    }
}