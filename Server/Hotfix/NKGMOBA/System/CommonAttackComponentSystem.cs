//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2020年10月21日 15:15:48
//------------------------------------------------------------

using System.Collections.Generic;
using ETModel;
using ETModel.NKGMOBA.Battle.Fsm;
using ETModel.NKGMOBA.Battle.State;

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
        public static async ETVoid CommonAttackStart(this CommonAttackComponent self, Unit targetUnit)
        {
            self.m_ETCancellationTokenSource?.Cancel();
            self.m_ETCancellationTokenSource = ComponentFactory.Create<ETCancellationTokenSource>();
            self.m_StackFsmComponent.ChangeState<CommonAttackState>(StateTypes.CommonAttack, "Attack", 1);
            await self.CommonAttack_Internal(targetUnit);
        }

        public static async ETTask CommonAttack_Internal(this CommonAttackComponent self, Unit targetUnit)
        {
            HeroDataComponent heroDataComponent = self.Entity.GetComponent<HeroDataComponent>();
            float attackPre = heroDataComponent.NodeDataForHero.OriAttackPre / (1 + heroDataComponent.NodeDataForHero.ExtAttackSpeed);
            float attackPos = heroDataComponent.NodeDataForHero.OriAttackPos / (1 + heroDataComponent.NodeDataForHero.ExtAttackSpeed);
            float attackSpeed = heroDataComponent.NodeDataForHero.OriAttackSpeed + heroDataComponent.NodeDataForHero.ExtAttackSpeed;
            //播放动画，如果动画播放完成还不能进行下一次普攻，则播放空闲动画
            await Game.Scene.GetComponent<TimerComponent>().WaitAsync((long) (attackPre * 1000), self.m_ETCancellationTokenSource.Token);
            
            List<NP_RuntimeTree> targetSkillCanvas = self.Entity.GetComponent<SkillCanvasManagerComponent>().GetSkillCanvas(10001);
            foreach (var skillCanva in targetSkillCanvas)
            {
                skillCanva.GetBlackboard().Set("CastNormalAttack", true);
                skillCanva.GetBlackboard().Set("NormalAttackUnitIds", new List<long>() { targetUnit.Id });
            }
            Game.EventSystem.Run(EventIdType.ChangeHP, targetUnit.Id, -50.0f);
            self.m_LastAttackTime = TimeHelper.Now();
            self.m_CanAttack = false;
            self.m_AttackInterval = (long) (1 / attackSpeed - attackPre) * 1000;
            MessageHelper.Broadcast(new M2C_CommonAttackState() { UnitId = self.Entity.Id, CanAttack = false });

            await Game.Scene.GetComponent<TimerComponent>().WaitAsync(self.m_AttackInterval, self.m_ETCancellationTokenSource.Token);
            //此次攻击完成
            self.m_ETCancellationTokenSource.Dispose();
            self.m_ETCancellationTokenSource = null;
        }

        public static void CancelCommonAttack(this CommonAttackComponent self)
        {
            self.m_ETCancellationTokenSource?.Cancel();
            self.m_ETCancellationTokenSource = null;
        }
        
        public static void Update(this CommonAttackComponent self)
        {
            if (!self.m_CanAttack)
            {
                if (TimeHelper.Now() - self.m_LastAttackTime > self.m_AttackInterval)
                {
                    self.m_CanAttack = true;
                    MessageHelper.Broadcast(new M2C_CommonAttackState() { UnitId = self.Entity.Id, CanAttack = true });
                }
            }
        }
    }
}