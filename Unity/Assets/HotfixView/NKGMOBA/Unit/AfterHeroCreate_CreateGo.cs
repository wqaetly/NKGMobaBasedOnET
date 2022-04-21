using UnityEngine;
using UnityEngine.UI;

namespace ET
{
    public class AfterHeroCreate_CreateGo : AEvent<EventType.AfterHeroCreate_CreateGo>
    {
        protected override async ETTask Run(EventType.AfterHeroCreate_CreateGo args)
        {
            Client_UnitConfig clientUnitConfig = Client_UnitConfigCategory.Instance.Get(args.HeroConfigId);
            GameObjectComponent gameObjectComponent =
                args.Unit.AddComponent<GameObjectComponent, string>(clientUnitConfig.UnitName);

            gameObjectComponent.GameObject.transform.position =
                args.Unit.Position;

            args.Unit.AddComponent<AnimationComponent>();
            args.Unit.AddComponent<UnitTransformComponent>();
            args.Unit.AddComponent<TurnComponent>();
            args.Unit.AddComponent<EffectComponent>();
            args.Unit.AddComponent<CommonAttackComponent_View>();
            args.Unit.AddComponent<FallingFontComponent>();
            args.Unit.AddComponent<B2S_DebuggerComponent>();

            // 只有本地玩家才会显示技能指示器
            if (args.IsLocalPlayer)
            {
                args.Unit.AddComponent<SkillIndicatorComponent>();
                args.Unit.AddComponent<PlayerHeroControllerComponent>();
            }
            
            gameObjectComponent.GameObject.GetComponent<MonoBridge>().BelongToUnitId = args.Unit.Id;

            SkillCanvasConfig unitPassiveSkillConfig =
                SkillCanvasConfigCategory.Instance.Get(clientUnitConfig.UnitPassiveSkillId);
            SkillCanvasConfig unitQSkillConfig =
                SkillCanvasConfigCategory.Instance.Get(clientUnitConfig.UnitQSkillId);
            SkillCanvasConfig unitWSkillConfig =
                SkillCanvasConfigCategory.Instance.Get(clientUnitConfig.UnitWSkillId);
            SkillCanvasConfig unitESkillConfig =
                SkillCanvasConfigCategory.Instance.Get(clientUnitConfig.UnitESkillId);
            SkillCanvasConfig Test =
                SkillCanvasConfigCategory.Instance.Get(clientUnitConfig.UnitRSkillId);

            //英雄属性组件
            args.Unit.AddComponent<UnitAttributesDataComponent, long>(clientUnitConfig.UnitAttributesDataId);

            //Log.Info("开始装载技能");
            NP_RuntimeTreeFactory.CreateSkillNpRuntimeTree(args.Unit, unitPassiveSkillConfig.NPBehaveId,
                unitPassiveSkillConfig.BelongToSkillId).Start();
            NP_RuntimeTreeFactory
                .CreateSkillNpRuntimeTree(args.Unit, unitQSkillConfig.NPBehaveId, unitQSkillConfig.BelongToSkillId)
                .Start();
            NP_RuntimeTreeFactory
                .CreateSkillNpRuntimeTree(args.Unit, unitWSkillConfig.NPBehaveId, unitWSkillConfig.BelongToSkillId)
                .Start();
            NP_RuntimeTreeFactory
                .CreateSkillNpRuntimeTree(args.Unit, unitESkillConfig.NPBehaveId, unitESkillConfig.BelongToSkillId)
                .Start();
            // NP_RuntimeTreeFactory
            //     .CreateSkillNpRuntimeTree(args.Unit, Test.NPBehaveId, unitESkillConfig.BelongToSkillId)
            //     .Start();

            await ETTask.CompletedTask;
        }
    }
}