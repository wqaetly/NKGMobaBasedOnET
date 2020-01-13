using System;
using ETModel;
using ETModel.NKGMOBA.Battle.State;
using UnityEngine.Experimental.UIElements;
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
                if (unitComponent.Get(unitInfo.UnitId) != null)
                {
                    continue;
                }

                //根据不同ID，创建小骷髅
                Unit unit = UnitFactory.Create(unitInfo.UnitId);
                //因为血条需要，创建热更层unit
                HotfixUnit hotfixUnit = HotfixUnitFactory.CreateHotfixUnit(unit);

                unit.Position = new Vector3(unitInfo.X, unitInfo.Y, unitInfo.Z);

                unit.AddComponent<NP_RuntimeTreeManager>();
                //Log.Info("开始创建行为树");
                NP_RuntimeTree npRuntimeTree = NP_RuntimeTreeFactory.CreateNpRuntimeTree(unit, NP_Client_TreeIds.Darius_Q_Client);
                //Log.Info("行为树创建完成");
                npRuntimeTree.m_NPRuntimeTreeRootNode.Start();

                // 创建血条
                Game.EventSystem.Run(EventIdType.CreateHeadBar, unitInfo.UnitId);

                // 增加头顶Bar
                hotfixUnit.AddComponent<HeroHeadBarComponent, Unit, FUI>(unit,
                    Game.Scene.GetComponent<FUIComponent>().Get(unitInfo.UnitId));

                //添加英雄数据
                M2C_GetHeroDataResponse M2C_GetHeroDataResponse = await Game.Scene.GetComponent<SessionComponent>()
                        .Session.Call(new C2M_GetHeroDataRequest() { UnitID = unitInfo.UnitId }) as M2C_GetHeroDataResponse;

                ETModel.Game.Scene.GetComponent<UnitComponent>().Get(unitInfo.UnitId)
                        .AddComponent<HeroDataComponent, long>(M2C_GetHeroDataResponse.HeroDataID);
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