//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年8月5日 20:31:32
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using ETHotfix.NKGMOBA.Factory;
using ETModel;

namespace ETHotfix
{
    /// <summary>
    /// B2S碰撞体工厂
    /// </summary>
    public static class B2S_ColliderFactory
    {
        /// <summary>
        /// 创建碰撞体Unit
        /// </summary>
        /// <param name="self"></param>
        /// <param name="unit">所归属Unit（施法者Unit）</param>
        /// <param name="collisionRelationId">所处碰撞关系数据载体id</param>
        /// <param name="nodeDataId">碰撞体数据ID（在碰撞关系数据载体中的节点Id）</param>
        public static Unit CreateCollider(this B2S_UnitColliderManagerComponent self, Unit unit, long collisionRelationId,
        long nodeDataId)
        {
            //用于记录id
            int flag = 0;
            foreach (KeyValuePair<(long, int, Unit), bool> VARIABLE in self.AllColliderData)
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

            Unit b2sColliderEntity = UnitFactory.CreateCollider(unit, collisionRelationId, nodeDataId, flag);

            //添加到碰撞数据管理者
            self.AllColliderData
                    .Add((b2sColliderEntity.GetComponent<B2S_ColliderComponent>().NodeDataId, flag, b2sColliderEntity), false);
            
            //Log.Info($"新建的碰撞数据.ID为{nodeDataId}");
            return b2sColliderEntity;
        }

        /// <summary>
        /// 回收碰撞数据
        /// </summary>
        /// <param name="self"></param>
        public static void RecycleColliderData(this Unit self)
        {
            self.Entity.GetComponent<B2S_UnitColliderManagerComponent>()
                            .AllColliderData[
                                (self.GetComponent<B2S_ColliderComponent>().NodeDataId, self.GetComponent<B2S_ColliderComponent>().FlagId, self)]
                    = false;
        }
    }
}