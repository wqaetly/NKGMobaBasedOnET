using System;
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
            //创建战斗单位（小骷髅给劲哦）（赋予了Id）
            Unit unit = ComponentFactory.CreateWithId<Unit>(IdGenerater.GenerateId());

            //增加移动组件
            unit.AddComponent<MoveComponent>();
            //增加寻路相关组件
            unit.AddComponent<UnitPathComponent>();

            //增加碰撞体管理组件
            unit.AddComponent<B2S_HeroColliderDataManagerComponent>();

            //Log.Info("创建自身碰撞体");
            unit.GetComponent<B2S_HeroColliderDataManagerComponent>().CreateHeroColliderData(unit, 10001, 10006);

            unit.AddComponent<HeroDataComponent, long>(10001);

            //设置小骷髅位置
            unit.Position = new Vector3(-10, 0, -10);

            //给小骷髅添加信箱组件，队列处理收到的消息（赋予了InstanceId）
            await unit.AddComponent<MailBoxComponent>().AddLocation();

            //添加同gate服务器通信基础组件，主要是赋予ID
            unit.AddComponent<UnitGateComponent, long>(request.GateSessionId);

            //将这个小骷髅维护在Unit组件里
            Game.Scene.GetComponent<UnitComponent>().Add(unit);

            //设置回复消息的Id
            response.UnitId = unit.Id;

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
            
            Log.Info("已经到这一步了");

            //向所有小骷髅广播信息
            MessageHelper.Broadcast(createUnits);

            //广播完回复客户端，这边搞好了
            reply();
        }
    }
}