//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2020年9月27日 10:31:04
//------------------------------------------------------------

using ETModel;
using ETModel.NKGMOBA.Battle.State;
using UnityEngine;

namespace ETHotfix.NKGMOBA.Factory
{
    public class UnitFactory
    {
        #region base

        /// <summary>
        /// 随机Id
        /// </summary>
        /// <returns></returns>
        private static Unit CreateUnitBase()
        {
            return CreateUnitWithIdBase(IdGenerater.GenerateId());
        }

        /// <summary>
        /// 指定Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private static Unit CreateUnitWithIdBase(long id)
        {
            Unit result = ComponentFactory.CreateWithId<Unit>(id);
            UnitComponent.Instance.Add(result);
            return result;
        }

        #endregion

        #region 高级Unit相关（英雄，野怪，假人）

        /// <summary>
        /// 创建诺手
        /// TODO 后期需要改，应该是一个通用的创建英雄接口，只需要提供Id，然后自动索引其所需要的数据（比如技能数据，英雄数据）
        /// </summary>
        /// <returns></returns>
        public static Unit CreateDarius()
        {
            //创建战斗单位
            Unit unit = CreateUnitBase();
            unit.AddComponent<ChildrenUnitComponent>();
            //增加移动组件
            unit.AddComponent<MoveComponent>();
            //增加寻路相关组件
            unit.AddComponent<UnitPathComponent>();

            //增加碰撞体管理组件
            unit.AddComponent<B2S_UnitColliderManagerComponent>();

            unit.GetComponent<B2S_UnitColliderManagerComponent>().CreateCollider(unit,
                Game.Scene.GetComponent<ConfigComponent>().Get<Server_B2SCollisionRelationConfig>(10001).B2S_CollisionRelationId, 10006);
            unit.AddComponent<B2S_RoleCastComponent>().RoleCast = RoleCast.Friendly;

            unit.AddComponent<NumericComponent>();
            unit.AddComponent<HeroDataComponent, long>(10001);
            unit.AddComponent<ReceiveDamageComponent>();
            unit.AddComponent<CastDamageComponent>();
            unit.AddComponent<DataModifierComponent>();
            
            unit.AddComponent<BuffManagerComponent>();
            unit.AddComponent<NP_RuntimeTreeManager>();
            unit.AddComponent<SkillCanvasManagerComponent>();

            ConfigComponent configComponent = Game.Scene.GetComponent<ConfigComponent>();
            //Log.Info("开始创建行为树");
            NP_RuntimeTreeFactory.CreateSkillNpRuntimeTree(unit, configComponent.Get<Server_SkillCanvasConfig>(10001).NPBehaveId,
                        configComponent.Get<Server_SkillCanvasConfig>(10001).BelongToSkillId)
                    .Start();

            NP_RuntimeTreeFactory.CreateSkillNpRuntimeTree(unit, configComponent.Get<Server_SkillCanvasConfig>(10002).NPBehaveId,
                        configComponent.Get<Server_SkillCanvasConfig>(10002).BelongToSkillId)
                    .Start();

            //默认升一级技能
            unit.GetComponent<SkillCanvasManagerComponent>().AddSkillLevel(configComponent.Get<Server_SkillCanvasConfig>(10002).BelongToSkillId);
            //Log.Info("行为树创建完成");

            //添加栈式状态机组件
            unit.AddComponent<StackFsmComponent>();
            unit.AddComponent<CommonAttackComponent>();

            //设置英雄位置
            unit.Position = new Vector3(-10, 0, -10);
            return unit;
        }

        /// <summary>
        /// 创建假人,需要传入一个父UnitId
        /// </summary>
        /// <returns></returns>
        public static Unit CreateSpiling(long parentId)
        {
            Unit unit = CreateUnitBase();
            //Log.Info($"服务端响应木桩请求，父id为{message.ParentUnitId}");
            UnitComponent.Instance.Get(parentId).GetComponent<ChildrenUnitComponent>().AddUnit(unit);
            //Log.Info("确认找到了请求的父实体");

            unit.AddComponent<B2S_UnitColliderManagerComponent>().CreateCollider(unit,
                Game.Scene.GetComponent<ConfigComponent>().Get<Server_B2SCollisionRelationConfig>(10001).B2S_CollisionRelationId, 10006);
            unit.AddComponent<NumericComponent>();
            unit.AddComponent<HeroDataComponent, long>(10001);
            unit.AddComponent<ReceiveDamageComponent>();
            unit.AddComponent<CastDamageComponent>();
            unit.AddComponent<DataModifierComponent>();
            
            unit.AddComponent<BuffManagerComponent>();
            unit.AddComponent<B2S_RoleCastComponent>().RoleCast = RoleCast.Adverse;
            //添加栈式状态机组件
            unit.AddComponent<StackFsmComponent>();
            unit.AddComponent<MoveComponent>();
            unit.AddComponent<UnitPathComponent>();

            unit.AddComponent<NP_RuntimeTreeManager>();

            ConfigComponent configComponent = Game.Scene.GetComponent<ConfigComponent>();
            // if (RandomHelper.RandomNumber(0, 2) == 0)
            //         //Log.Info("开始创建行为树");
            //     NP_RuntimeTreeFactory.CreateNpRuntimeTree(unit, configComponent.Get<Server_AICanvasConfig>(10001).NPBehaveId)
            //             .Start();

            return unit;
        }

        #endregion

        #region 碰撞体Unit

        /// <summary>
        /// 创建碰撞体
        /// </summary>
        /// <param name="belongToUnit">归属的Unit（施法者Unit）</param>
        /// <param name="collisionRelationId">所处碰撞关系数据载体id</param>
        /// <param name="nodeDataId">碰撞体数据ID（在碰撞关系数据载体中的节点Id）</param>
        /// <param name="flagId">碰撞体缓存池中的FlagId</param>
        /// <returns></returns>
        public static Unit CreateCollider(Unit belongToUnit, long collisionRelationId, long nodeDataId, int flagId)
        {
            B2S_CollisionsRelationSupport b2SCollisionsRelationSupport = Game.Scene.GetComponent<B2S_CollisionRelationRepositoryComponent>()
                    .GetB2S_CollisionsRelationSupportById(collisionRelationId);

            if (!b2SCollisionsRelationSupport.B2S_CollisionsRelationDic.ContainsKey(nodeDataId))
            {
                Log.Error($"所请求的碰撞关系数据结点不存在,ID为{nodeDataId}");
                return null;
            }

            //为碰撞体新建一个Unit
            Unit b2sColliderEntity = CreateUnitBase();
            b2sColliderEntity.AddComponent<NP_RuntimeTreeManager>();
            b2sColliderEntity.AddComponent<SkillCanvasManagerComponent>();

            b2sColliderEntity.AddComponent<B2S_ColliderComponent, Unit, B2S_CollisionInstance, long, int>(belongToUnit,
                b2SCollisionsRelationSupport.B2S_CollisionsRelationDic[nodeDataId],
                nodeDataId, flagId);

            //把这个碰撞实体增加到管理者维护 TODO 待优化，目的同B2S_ColliderEntityManagerComponent
            Game.Scene.GetComponent<B2S_WorldColliderManagerComponent>().AddColliderEntity(b2sColliderEntity);
            return b2sColliderEntity;
        }

        #endregion
    }
}