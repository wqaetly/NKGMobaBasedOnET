using System.Collections.Generic;
using Box2DSharp.Dynamics;
using UnityEngine;

namespace ET
{
    public static class B2S_ColliderDataLoadHelper
    {
        /// <summary>
        /// 加载依赖数据，并且进行碰撞体的生成
        /// </summary>
        /// <param name="self"></param>
        public static void CreateB2S_Collider(this B2S_ColliderComponent self)
        {
            B2S_ColliderDataRepositoryComponent b2SColliderDataRepositoryComponent =
                self.DomainScene().GetComponent<B2S_ColliderDataRepositoryComponent>();

            self.B2S_ColliderDataStructureBase = b2SColliderDataRepositoryComponent.GetDataByColliderId(
                Server_B2SColliderConfigCategory.Instance.Get(self.B2S_ColliderDataConfigId).B2S_ColliderId);

            self.Body = self.WorldComponent.CreateDynamicBody();

            ApplyFixture(self.B2S_ColliderDataStructureBase, self.Body, self.GetParent<Unit>());
        }

        public static void ApplyFixture(B2S_ColliderDataStructureBase b2SColliderDataStructureBase, Body body,
            Unit unit)
        {
            switch (b2SColliderDataStructureBase.b2SColliderType)
            {
                case B2S_ColliderType.BoxColllider:
                    B2S_BoxColliderDataStructure b2SBoxColliderDataStructure =
                        (B2S_BoxColliderDataStructure) b2SColliderDataStructureBase;
                    body.CreateBoxFixture(b2SBoxColliderDataStructure.hx, b2SBoxColliderDataStructure.hy,
                        b2SBoxColliderDataStructure.finalOffset, 0, b2SBoxColliderDataStructure.isSensor, unit);
                    break;
                case B2S_ColliderType.CircleCollider:
                    B2S_CircleColliderDataStructure b2SCircleColliderDataStructure =
                        (B2S_CircleColliderDataStructure) b2SColliderDataStructureBase;
                    body.CreateCircleFixture(b2SCircleColliderDataStructure.radius,
                        b2SCircleColliderDataStructure.finalOffset,
                        b2SCircleColliderDataStructure.isSensor,
                        unit);
                    break;
                case B2S_ColliderType.PolygonCollider:
                    B2S_PolygonColliderDataStructure b2SPolygonColliderDataStructure =
                        (B2S_PolygonColliderDataStructure) b2SColliderDataStructureBase;
                    foreach (var verxtPoint in b2SPolygonColliderDataStructure.finalPoints)
                    {
                        body.CreatePolygonFixture(verxtPoint, b2SPolygonColliderDataStructure.isSensor,
                            unit);
                    }

                    break;
            }
        }

#if !SERVER
        public static void RenderAB2S_Collider(Unit ownerUnit, int b2S_ColliderDataConfigId, Vector2 worldOffset,
            Quaternion worldRotation, long sustainTime)
        {
            B2S_ColliderDataRepositoryComponent b2SColliderDataRepositoryComponent =
                ownerUnit.DomainScene().GetComponent<B2S_ColliderDataRepositoryComponent>();

            B2S_ColliderDataStructureBase b2SColliderDataStructureBase =
                b2SColliderDataRepositoryComponent.GetDataByColliderId(
                    Server_B2SColliderConfigCategory.Instance.Get(b2S_ColliderDataConfigId).B2S_ColliderId);

            List<Vector2> colliderPoints = new List<Vector2>();
            switch (b2SColliderDataStructureBase)
            {
                case B2S_BoxColliderDataStructure b2SBoxColliderDataStructure:
                    
                    break;
                case B2S_CircleColliderDataStructure b2SCircleColliderDataStructure:
                    
                    break;
                case B2S_PolygonColliderDataStructure b2SPolygonColliderDataStructure:
                    
                    break;
            }
        }
#endif
    }
}