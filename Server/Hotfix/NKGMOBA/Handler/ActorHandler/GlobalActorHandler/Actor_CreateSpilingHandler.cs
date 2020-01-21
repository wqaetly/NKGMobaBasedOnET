//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2020年1月21日 16:03:23
//------------------------------------------------------------

using ETModel;
using UnityEngine;

namespace ETHotfix
{
    [ActorMessageHandler(AppType.Map)]
    public class Actor_CreateSpilingHandler: AMActorLocationHandler<Unit, Actor_CreateSpiling>
    {
        protected override ETTask Run(Unit entity, Actor_CreateSpiling message)
        {
            Unit unit = ComponentFactory.CreateWithId<Unit>(IdGenerater.GenerateId());
            //Log.Info($"服务端响应木桩请求，父id为{message.ParentUnitId}");
            Game.Scene.GetComponent<UnitComponent>().Get(message.ParentUnitId).GetComponent<ChildrenUnitComponent>().AddUnit(unit);

            //Log.Info("确认找到了请求的父实体");
            //Game.EventSystem.Run(EventIdType.CreateCollider, unit.Id, 10001, 10006);
            unit.AddComponent<B2S_HeroColliderDataManagerComponent>().CreateHeroColliderData(unit, 10001, 10006);
            unit.AddComponent<HeroDataComponent, long>(10001);
            unit.AddComponent<BuffManagerComponent>();
            unit.AddComponent<B2S_RoleCastComponent>().RoleCast = RoleCast.Adverse;
            //设置木桩位置
            unit.Position = new Vector3(message.X, 0, message.Z);
            // 广播创建的木桩unit
            M2C_CreateSpilings createSpilings = new M2C_CreateSpilings();

            SpilingInfo spilingInfo = new SpilingInfo();
            spilingInfo.X = unit.Position.x;
            spilingInfo.Y = unit.Position.y;
            spilingInfo.Z = unit.Position.z;
            spilingInfo.UnitId = unit.Id;
            spilingInfo.ParentUnitId = message.ParentUnitId;
            createSpilings.Spilings = spilingInfo;

            //向所有小骷髅广播信息
            MessageHelper.Broadcast(createSpilings);
            return ETTask.CompletedTask;
        }
    }
}