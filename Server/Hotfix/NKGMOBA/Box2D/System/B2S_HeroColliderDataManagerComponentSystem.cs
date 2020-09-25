//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年8月5日 20:31:32
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using ETModel;

namespace ETHotfix
{
    public static class B2S_HeroColliderDataComponentFactory
    {
        /// <summary>
        /// 创建碰撞数据
        /// </summary>
        /// <param name="self"></param>
        /// <param name="unit">所归属Unit</param>
        /// <param name="collisionRelationId">所处碰撞关系数据载体id</param>
        /// <param name="nodeDataId">碰撞体数据ID</param>
        public static B2S_ColliderEntity CreateHeroColliderData(this B2S_ColliderDataManagerComponent self, Unit unit, long collisionRelationId,
        long nodeDataId)
        {
            //用于记录id
            int flag = 0;
            foreach (KeyValuePair<(long, int, B2S_ColliderEntity), bool> VARIABLE in self.AllColliderData)
            {
                if (VARIABLE.Key.Item1 == nodeDataId)
                {
                    if (VARIABLE.Value)
                    {
                        self.AllColliderData[VARIABLE.Key] = false;
                        Log.Info($"复用的碰撞数据,ID为{nodeDataId}");
                        return VARIABLE.Key.Item3;
                    }

                    flag++;
                }
            }

            B2S_CollisionsRelationSupport b2SCollisionsRelationSupport = Game.Scene.GetComponent<B2S_CollisionRelationRepositoryComponent>()
                    .GetB2S_CollisionsRelationSupportById(collisionRelationId);

            if (!b2SCollisionsRelationSupport.B2S_CollisionsRelationDic.ContainsKey(nodeDataId))
            {
                Log.Error($"所请求的碰撞关系数据结点不存在,ID为{nodeDataId}");
                return null;
            }

            //创建数据，并以英雄作为父Entity
            B2S_ColliderEntity b2SColliderEntity =
                    ComponentFactory.CreateWithParent<B2S_ColliderEntity, B2S_CollisionInstance, long, int>(unit,
                        b2SCollisionsRelationSupport.B2S_CollisionsRelationDic[nodeDataId],
                        nodeDataId, flag);

            //把这个碰撞实体增加到管理者维护 TODO 待优化，目的同B2S_ColliderEntityManagerComponent
            Game.Scene.GetComponent<B2S_ColliderEntityManagerComponent>().AddColliderEntity(b2SColliderEntity);

            //添加到碰撞数据管理者
            self.AllColliderData
                    .Add((b2SColliderEntity.ID, flag, b2SColliderEntity), false);
            Log.Info($"新建的碰撞数据.ID为{nodeDataId}");
            return b2SColliderEntity;
        }

        /// <summary>
        /// 回收碰撞数据
        /// </summary>
        /// <param name="self"></param>
        /// <param name="supportorId">所处碰撞关系数据载体id</param>
        /// <param name="nodeDataId">结点ID</param>
        public static void RecycleColliderData(this B2S_ColliderEntity self)
        {
            self.Entity.GetComponent<B2S_ColliderDataManagerComponent>().AllColliderData[(self.ID, self.flagID, self)] = false;
        }
    }
}