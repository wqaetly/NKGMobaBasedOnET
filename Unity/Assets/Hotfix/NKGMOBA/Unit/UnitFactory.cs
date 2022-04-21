using ET.EventType;
using UnityEngine;

namespace ET
{
    public static class UnitFactory
    {
        public static Unit CreateUnit(Room room, long id, int configId)
        {
            UnitComponent unitComponent = room.GetComponent<UnitComponent>();

            Unit unit = unitComponent.AddChildWithId<Unit, int>(id, configId);
            unit.BelongToRoom = room;

            unitComponent.Add(unit);

            return unit;
        }

        public static Unit CreateHero(Room room, UnitInfo unitInfo)
        {
            Unit unit = CreateUnit(room, unitInfo.UnitId, unitInfo.ConfigId);
            PlayerComponent playerComponent = Game.Scene.GetComponent<PlayerComponent>();

            unit.Position = new Vector3(unitInfo.X, unitInfo.Y, unitInfo.Z);

            unit.AddComponent<DataModifierComponent>();
            unit.AddComponent<NP_SyncComponent>();
            unit.AddComponent<NumericComponent>();
            //增加栈式状态机，辅助动画切换
            unit.AddComponent<StackFsmComponent>();
            unit.AddComponent<RecastNavComponent, string>("solo_navmesh");
            unit.AddComponent<MoveComponent>();

            //增加Buff管理组件
            unit.AddComponent<BuffManagerComponent>();
            unit.AddComponent<SkillCanvasManagerComponent>();
            unit.AddComponent<B2S_RoleCastComponent, RoleCamp, RoleTag>((RoleCamp) unitInfo.RoleCamp, RoleTag.Hero);

            unit.AddComponent<NP_RuntimeTreeManager>();
            //Log.Info("行为树创建完成");
            unit.AddComponent<ObjectWait>();
            unit.AddComponent<LSF_TickComponent>();
            unit.AddComponent<CommonAttackComponent_Logic>();
            unit.AddComponent<CastDamageComponent>();
            unit.AddComponent<ReceiveDamageComponent>();

            EventType.AfterHeroCreate_CreateGo createGo = new AfterHeroCreate_CreateGo()
            {
                Unit = unit, HeroConfigId = unitInfo.ConfigId,
            };

            if (unitInfo.BelongToPlayerId == playerComponent.PlayerId)
            {
                UnitComponent unitComponent = room.GetComponent<UnitComponent>();
                unitComponent.MyUnit = unit;
                createGo.IsLocalPlayer = true;
            }

            Game.EventSystem.Publish(createGo).Coroutine();
            return unit;
        }

        public static Unit CreateHeroSpilingUnit(Room room, UnitInfo unitInfo)
        {
            Unit unit = CreateUnit(room, unitInfo.UnitId, unitInfo.ConfigId);
            unit.Position = new Vector3(unitInfo.X, unitInfo.Y, unitInfo.Z);

            unit.AddComponent<DataModifierComponent>();
            unit.AddComponent<NP_SyncComponent>();
            unit.AddComponent<NumericComponent>();
            //增加栈式状态机，辅助动画切换
            unit.AddComponent<StackFsmComponent>();
            unit.AddComponent<RecastNavComponent, string>("solo_navmesh");
            unit.AddComponent<MoveComponent>();

            //增加Buff管理组件
            unit.AddComponent<BuffManagerComponent>();
            unit.AddComponent<SkillCanvasManagerComponent>();
            unit.AddComponent<B2S_RoleCastComponent, RoleCamp, RoleTag>((RoleCamp) unitInfo.RoleCamp, RoleTag.Hero);

            unit.AddComponent<NP_RuntimeTreeManager>();
            //Log.Info("行为树创建完成");
            unit.AddComponent<ObjectWait>();
            unit.AddComponent<CastDamageComponent>();
            unit.AddComponent<ReceiveDamageComponent>();
            unit.AddComponent<LSF_TickComponent>();

            Game.EventSystem.Publish(new EventType.AfterHeroSpilingCreate_CreateGO()
                {Unit = unit, HeroSpilingConfigId = unitInfo.ConfigId}).Coroutine();
            return unit;
        }

        /// <summary>
        /// 创建碰撞体
        /// </summary>
        /// <param name="room">归属的房间</param>
        /// <param name="belongToUnit">归属的Unit</param>
        /// <param name="colliderDataConfigId">碰撞体数据表Id</param>
        /// <param name="collisionRelationDataConfigId">碰撞关系数据表Id</param>
        /// <param name="colliderNPBehaveTreeIdInExcel">碰撞体的行为树Id</param>
        /// <returns></returns>
        public static Unit CreateSpecialColliderUnit(Room room, long belongToUnitId, long selfId,
            int collisionRelationDataConfigId, int colliderNPBehaveTreeIdInExcel, bool followUnitPos,
            bool followUnitRot, Vector3 offset,
            Vector3 targetPos, float angle)
        {
            //为碰撞体新建一个Unit
            Unit b2sColliderEntity = CreateUnit(room, selfId, 0);
            Unit belongToUnit = room.GetComponent<UnitComponent>().Get(belongToUnitId);

            b2sColliderEntity.AddComponent<NP_SyncComponent>();
            b2sColliderEntity.AddComponent<B2S_ColliderComponent, CreateSkillColliderArgs>(
                new CreateSkillColliderArgs()
                {
                    belontToUnit = belongToUnit, collisionRelationDataConfigId = collisionRelationDataConfigId,
                    FollowUnitPos = followUnitPos, FollowUnitRot = followUnitRot, offset = offset,
                    targetPos = targetPos, angle = angle
                });
            b2sColliderEntity.AddComponent<NP_RuntimeTreeManager>();
            b2sColliderEntity.AddComponent<SkillCanvasManagerComponent>();

            //根据传过来的行为树Id来给这个碰撞Unit加上行为树
            NP_RuntimeTreeFactory.CreateSkillNpRuntimeTree(b2sColliderEntity,
                    SkillCanvasConfigCategory.Instance.Get(colliderNPBehaveTreeIdInExcel).NPBehaveId,
                    SkillCanvasConfigCategory.Instance.Get(colliderNPBehaveTreeIdInExcel).BelongToSkillId)
                .Start();

            b2sColliderEntity.AddComponent<LSF_TickComponent>();

            // DEBUG 将碰撞体可视化出来


            return b2sColliderEntity;
        }

        public class CreateHeroColliderArgs
        {
            public Unit Unit;
            public B2S_ColliderDataStructureBase B2SColliderDataStructureBase;
            public string CollisionHandler;
            public bool FollowUnit;
        }

        public class CreateSkillColliderArgs
        {
            public Unit belontToUnit;
            public int collisionRelationDataConfigId;
            public bool FollowUnitPos;
            public bool FollowUnitRot;
            public Vector3 offset;
            public Vector3 targetPos;
            public float angle;
        }
    }
}