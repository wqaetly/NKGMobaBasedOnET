using System;
using ETHotfix.NKGMOBA.Factory;
using ETModel;
using PF;
using UnityEngine;

namespace ETHotfix
{
    [MessageHandler(AppType.Map)]
    public class G2M_CreateUnitHandler: AMRpcHandler<G2M_CreateUnit, M2G_CreateUnit>
    {
        protected override async ETTask Run(Session session, G2M_CreateUnit request, M2G_CreateUnit response, Action reply)
        {
            Unit targetUnit = UnitFactory.CreateDarius();

            //给小骷髅添加信箱组件，队列处理收到的消息（赋予了InstanceId）
            await targetUnit.AddComponent<MailBoxComponent>().AddLocation();

            //添加同gate服务器通信基础组件，主要是赋予ID
            targetUnit.AddComponent<UnitGateComponent, long>(request.GateSessionId);

            //设置回复消息的Id
            response.UnitId = targetUnit.Id;

            // 广播创建的unit
            M2C_CreateUnits createUnits = new M2C_CreateUnits();
            Unit[] units = Game.Scene.GetComponent<UnitComponent>().GetAll();
            foreach (Unit u in units)
            {
                UnitInfo unitInfo = new UnitInfo();
                unitInfo.X = u.Position.x;
                unitInfo.Y = u.Position.y;
                unitInfo.Z = u.Position.z;
                unitInfo.UnitId = u.Id;
                createUnits.Units.Add(unitInfo);
            }

            //向所有小骷髅广播信息
            MessageHelper.Broadcast(createUnits);

            //广播完回复客户端，这边搞好了
            reply();
        }
    }
}