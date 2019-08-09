//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年8月5日 20:31:32
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using ETMode;
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
        /// <param name="supportorId">所处碰撞关系数据载体id</param>
        /// <param name="nodeDataId">结点ID</param>
        public static B2S_HeroColliderData CreateHeroColliderData(this B2S_HeroColliderDataManagerComponent self, Unit unit, long supportorId,
        long nodeDataId)
        {
            //用于记录id
            int flag = 0;
            foreach (KeyValuePair<(long, int, B2S_HeroColliderData), bool> VARIABLE in self.AllColliderData)
            {
                if (VARIABLE.Key.Item1 == nodeDataId)
                {
                    if (VARIABLE.Value)
                    {
                        self.AllColliderData[VARIABLE.Key] = false;
                        //Log.Info("复用的碰撞数据");
                        return VARIABLE.Key.Item3;
                    }
                    flag++;
                }
            }

            B2S_CollisionsRelationSupport b2SCollisionsRelationSupport = Game.Scene.GetComponent<B2S_CollisionRelationRepositoryComponent>()
                    .GetB2S_CollisionsRelationSupportById(supportorId);

            if (!b2SCollisionsRelationSupport.B2S_CollisionsRelationDic.ContainsKey(nodeDataId))
            {
                Log.Error($"所请求的碰撞关系数据结点不存在,ID为{nodeDataId}");
                return null;
            }

            //创建数据，并以英雄作为父Entity
            B2S_HeroColliderData b2SHeroColliderData =
                    ComponentFactory.CreateWithParent<B2S_HeroColliderData, B2S_CollisionInstance, long>(unit,
                        b2SCollisionsRelationSupport.B2S_CollisionsRelationDic[nodeDataId],
                        nodeDataId);

            b2SHeroColliderData.AddComponent<B2S_CollisionResponseComponent>();

            self.AllColliderData
                    .Add((b2SHeroColliderData.ID, flag, b2SHeroColliderData), false);
            //Log.Info("新建的碰撞数据");
            return b2SHeroColliderData;
        }
    }
}