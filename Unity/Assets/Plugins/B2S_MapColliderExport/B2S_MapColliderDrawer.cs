#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ET
{
    public class B2S_MapColliderDrawer : MonoBehaviour
    {
        public ColliderDataSupporter ColliderDataSupporter = new ColliderDataSupporter();

        private void OnDrawGizmos()
        {
            foreach (var colliderData in ColliderDataSupporter.colliderDataDic)
            {
                if (colliderData.Value.b2SColliderType == B2S_ColliderType.BoxColllider)
                {
                    B2S_BoxColliderDataStructure boxColliderDataStructure =
                        colliderData.Value as B2S_BoxColliderDataStructure;

                    Matrix4x4 moveFromSelfToOriMatrix4X4 = Matrix4x4.Translate(new Vector3(
                        -boxColliderDataStructure.finalOffset.X, 0, -boxColliderDataStructure.finalOffset.Y));
                    Matrix4x4 rotationMatrix4X4 = Matrix4x4.Rotate(Quaternion.Euler(0.0f, boxColliderDataStructure.Angle, 0.0f));
                    Matrix4x4 moveFromOriToSelf = Matrix4x4.Translate(new Vector3(
                        boxColliderDataStructure.finalOffset.X, 0, boxColliderDataStructure.finalOffset.Y));
                    
                    Matrix4x4 finalMaterix4x4 = moveFromOriToSelf * rotationMatrix4X4 * moveFromSelfToOriMatrix4X4;

                    Vector3[] points = new Vector3[4]
                    {
                        finalMaterix4x4.MultiplyPoint(new Vector3(
                            boxColliderDataStructure.finalOffset.X - boxColliderDataStructure.hx, 0,
                            boxColliderDataStructure.finalOffset.Y + boxColliderDataStructure.hy)),
                        finalMaterix4x4.MultiplyPoint(new Vector3(
                            boxColliderDataStructure.finalOffset.X + boxColliderDataStructure.hx, 0,
                            boxColliderDataStructure.finalOffset.Y + boxColliderDataStructure.hy)),
                        finalMaterix4x4.MultiplyPoint(new Vector3(
                            boxColliderDataStructure.finalOffset.X + boxColliderDataStructure.hx, 0,
                            boxColliderDataStructure.finalOffset.Y - boxColliderDataStructure.hy)),
                        finalMaterix4x4.MultiplyPoint(new Vector3(
                            boxColliderDataStructure.finalOffset.X - boxColliderDataStructure.hx, 0,
                            boxColliderDataStructure.finalOffset.Y - boxColliderDataStructure.hy))
                    };

                    for (int i = 0; i < points.Length; i++)
                    {
                        Gizmos.DrawLine(points[i], points[(i + 1) % points.Length]);
                    }
                }

                if (colliderData.Value.b2SColliderType == B2S_ColliderType.CircleCollider)
                {
                    B2S_CircleColliderDataStructure circleColliderDataStructure =
                        colliderData.Value as B2S_CircleColliderDataStructure;

                    Vector3 tempVector3 = Vector3.forward * circleColliderDataStructure.radius;

                    var begin = new Vector2(tempVector3.x + circleColliderDataStructure.finalOffset.X,
                        tempVector3.y + circleColliderDataStructure.finalOffset.Y);

                    var step = Mathf.RoundToInt(360 / 30);
                    for (int i = 0; i <= 360; i += step)
                    {
                        tempVector3 = new Vector3(circleColliderDataStructure.radius * Mathf.Sin(i * Mathf.Deg2Rad),
                            circleColliderDataStructure.radius * Mathf.Cos(i * Mathf.Deg2Rad));

                        var to = new Vector2(tempVector3.x + circleColliderDataStructure.finalOffset.X,
                            tempVector3.y + circleColliderDataStructure.finalOffset.Y);

                        if (i > 0)
                            Gizmos.DrawLine(new Vector3(begin.x, 0, begin.y), new Vector3(to.x, 0, to.y));
                        begin = to;
                    }
                }
            }
        }
    }
}

#endif