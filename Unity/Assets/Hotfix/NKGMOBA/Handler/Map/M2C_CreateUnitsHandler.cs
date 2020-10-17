using System;
using ETModel;
using ETModel.NKGMOBA.Battle.State;
using Vector3 = UnityEngine.Vector3;

namespace ETHotfix
{
    [MessageHandler]
    public class M2C_CreateUnitsHandler: AMHandler<M2C_CreateUnits>
    {
        protected override async ETTask Run(ETModel.Session session, M2C_CreateUnits message)
        {
            UnitComponent unitComponent = ETModel.Game.Scene.GetComponent<UnitComponent>();

            foreach (UnitInfo unitInfo in message.Units)
            {
                //TODO 暂时先忽略除英雄之外的Unit（如技能碰撞体），后期需要配表来解决这一块的逻辑，并且需要在协议里指定Unit的类型Id（注意不是运行时的Id,是Excel表中的类型Id）
                //TODO 诺手UnitTypeId暂定10001
                if (unitComponent.Get(unitInfo.UnitId) != null || unitInfo.UnitTypeId != 10001)
                {
                    continue;
                }

                //根据不同名称和ID，创建英雄
                Unit unit = UnitFactory.CreateHero("NuoKe", unitInfo.UnitId);
                //因为血条需要，创建热更层unit
                HotfixUnit hotfixUnit = HotfixUnitFactory.CreateHotfixUnit(unit,true);

                hotfixUnit.AddComponent<FallingFontComponent>();

                unit.Position = new Vector3(unitInfo.X, unitInfo.Y, unitInfo.Z);
                unit.AddComponent<NP_RuntimeTreeManager>();

                //Log.Info("开始创建行为树");
                ConfigComponent configComponent = Game.Scene.GetComponent<ConfigComponent>();
                NP_RuntimeTreeFactory
                        .CreateSkillNpRuntimeTree(unit, configComponent.Get<Client_SkillCanvasConfig>(10001).NPBehaveId,
                            configComponent.Get<Client_SkillCanvasConfig>(10001).BelongToSkillId).Start();
                NP_RuntimeTreeFactory
                        .CreateSkillNpRuntimeTree(unit, configComponent.Get<Client_SkillCanvasConfig>(10002).NPBehaveId,
                            configComponent.Get<Client_SkillCanvasConfig>(10002).BelongToSkillId).Start();
                //Log.Info("行为树创建完成");

                //添加英雄数据
                M2C_GetHeroDataResponse M2C_GetHeroDataResponse = await Game.Scene.GetComponent<SessionComponent>()
                        .Session.Call(new C2M_GetHeroDataRequest() { UnitID = unitInfo.UnitId }) as M2C_GetHeroDataResponse;

                ETModel.Game.Scene.GetComponent<UnitComponent>().Get(unitInfo.UnitId)
                        .AddComponent<HeroDataComponent, long>(M2C_GetHeroDataResponse.HeroDataID);

                // 创建头顶Bar
                Game.EventSystem.Run(EventIdType.CreateHeadBar, unitInfo.UnitId);
                // 挂载头顶Bar
                hotfixUnit.AddComponent<HeroHeadBarComponent, Unit, FUI>(unit,
                    Game.Scene.GetComponent<FUIComponent>().Get(unitInfo.UnitId));
            }
            
            if (ETModel.Game.Scene.GetComponent<UnitComponent>().MyUnit == null)
            {
                // 给自己的Unit添加引用
                ETModel.Game.Scene.GetComponent<UnitComponent>().MyUnit =
                        ETModel.Game.Scene.GetComponent<UnitComponent>().Get(PlayerComponent.Instance.MyPlayer.UnitId);
                ETModel.Game.Scene.GetComponent<UnitComponent>().MyUnit
                        .AddComponent<CameraComponent, Unit>(ETModel.Game.Scene.GetComponent<UnitComponent>().MyUnit);

                Game.EventSystem.Run(EventIdType.EnterMapFinish);
            }

            //ETModel.Log.Info($"{DateTime.UtcNow}完成一次创建Unit");
            await ETTask.CompletedTask;
        }
    }
}