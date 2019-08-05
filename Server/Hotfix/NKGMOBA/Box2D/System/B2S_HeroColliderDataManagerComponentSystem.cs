//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年8月5日 20:31:32
//------------------------------------------------------------

using ETMode;
using ETModel;

namespace ETHotfix
{
    public static class B2S_HeroColliderDataComponentFactory
    {
        /// <summary>
        /// 创建英雄碰撞数据
        /// </summary>
        /// <param name="supportorId">所处载体数据</param>
        /// <param name="nodeDataId">结点ID</param>
        /// <returns></returns>
        public static void CreateHeroColliderData(this B2S_HeroColliderDataManagerComponent self, long supportorId,
        long nodeDataId)
        {
            //如果有就不需要再创建了
            if (self.AllColliderData.ContainsKey(nodeDataId)) return;

            B2S_CollisionsRelationSupport b2SCollisionsRelationSupport = Game.Scene.GetComponent<B2S_CollisionRelationRepositoryComponent>()
                    .GetB2S_CollisionsRelationSupportById(supportorId);

            if (!b2SCollisionsRelationSupport.B2S_CollisionsRelationDic.ContainsKey(nodeDataId))
            {
                Log.Error($"所请求的碰撞关系数据结点不存在,ID为{nodeDataId}");
                return;
            }

            B2S_HeroColliderDataComponent b2SHeroColliderDataComponent =
                    ComponentFactory.Create<B2S_HeroColliderDataComponent, B2S_CollisionInstance, long>(
                        b2SCollisionsRelationSupport.B2S_CollisionsRelationDic[nodeDataId],
                        nodeDataId);

            self.AllColliderData
                    .Add(b2SHeroColliderDataComponent.ID, b2SHeroColliderDataComponent);
                    
        }
    }
}