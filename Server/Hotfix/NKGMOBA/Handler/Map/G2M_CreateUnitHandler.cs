using System;
using ETHotfix.NKGMOBA.Factory;
using ETModel;

namespace ETHotfix
{
    [MessageHandler(AppType.Map)]
    public class G2M_CreateUnitHandler: AMRpcHandler<G2M_CreateUnit, M2G_CreateUnit>
    {
        protected override async ETTask Run(Session session, G2M_CreateUnit request, M2G_CreateUnit response, Action reply)
        {
            Unit targetUnit = UnitFactory.CreateDarius();

            //给小骷髅添加信箱组件，队列处理收到的消息，让这个unit正式注册为actor系统一员
            await targetUnit.AddComponent<MailBoxComponent>().AddLocation();

            //添加同gate服务器通信基础组件，记录GateSeesion的Id为ActorId
            targetUnit.AddComponent<UnitGateComponent, long>(request.GateSessionId);

            // 广播创建的unit
            M2C_CreateUnits createUnits = new M2C_CreateUnits();
            Unit[] units = UnitComponent.Instance.GetAll();
            foreach (Unit u in units)
            {
                UnitInfo unitInfo = new UnitInfo();
                if (u.GetComponent<B2S_RoleCastComponent>() != null)
                {
                    //TODO 诺手UnitTypeId暂定10001
                    unitInfo.UnitTypeId = 10001;
                    unitInfo.RoleCamp = (int) u.GetComponent<B2S_RoleCastComponent>().RoleCamp;
                }

                unitInfo.X = u.Position.x;
                unitInfo.Y = u.Position.y;
                unitInfo.Z = u.Position.z;
                unitInfo.UnitId = u.Id;
                createUnits.Units.Add(unitInfo);
            }

            //向所有unit广播信息
            MessageHelper.Broadcast(createUnits);

            //设置回复消息的Id
            response.UnitId = targetUnit.Id;
            //广播完回复客户端，这边搞好了
            reply();
        }
    }
}