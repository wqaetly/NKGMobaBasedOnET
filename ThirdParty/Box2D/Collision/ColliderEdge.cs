using System;
using System.Buffers;
using System.Diagnostics;
using System.Numerics;
using Box2DSharp.Collision.Collider;
using Box2DSharp.Collision.Shapes;
using Box2DSharp.Common;

namespace Box2DSharp.Collision
{
    public static partial class CollisionUtils
    {
        /// <summary>
        ///     Compute contact points for edge versus circle.
        ///     This accounts for edge connectivity.
        ///     计算边缘和圆的碰撞点
        /// </summary>
        /// <param name="manifold"></param>
        /// <param name="edgeA"></param>
        /// <param name="xfA"></param>
        /// <param name="circleB"></param>
        /// <param name="xfB"></param>
        public static void CollideEdgeAndCircle(
            ref Manifold manifold,
            EdgeShape edgeA,
            in Transform xfA,
            CircleShape circleB,
            in Transform xfB)
        {
            manifold.PointCount = 0;

            // Compute circle in frame of edge
            // 在边缘形状的外框处理圆形
            var Q = MathUtils.MulT(xfA, MathUtils.Mul(xfB, circleB.Position));

            Vector2 A = edgeA.Vertex1, B = edgeA.Vertex2;
            var e = B - A;

            // Normal points to the right for a CCW winding
            var n = new Vector2(e.Y, -e.X);
            var offset = Vector2.Dot(n, Q - A);

            var oneSided = edgeA.OneSided;
            if (oneSided && offset < 0.0f)
            {
                return;
            }

            // Barycentric coordinates
            // 质心坐标
            var u = Vector2.Dot(e, B - Q);
            var v = Vector2.Dot(e, Q - A);

            var radius = edgeA.Radius + circleB.Radius;

            var cf = new ContactFeature {IndexB = 0, TypeB = (byte)ContactFeature.FeatureType.Vertex};

            // Region A
            if (v <= 0.0f)
            {
                var P = A;
                var d = Q - P;
                var dd = Vector2.Dot(d, d);
                if (dd > radius * radius)
                {
                    return;
                }

                // Is there an edge connected to A?
                if (edgeA.OneSided)
                {
                    var A1 = edgeA.Vertex0;
                    var B1 = A;
                    var e1 = B1 - A1;
                    var u1 = Vector2.Dot(e1, B1 - Q);

                    // Is the circle in Region AB of the previous edge?
                    if (u1 > 0.0f)
                    {
                        return;
                    }
                }

                cf.IndexA = 0;
                cf.TypeA = (byte)ContactFeature.FeatureType.Vertex;
                manifold.PointCount = 1;
                manifold.Type = ManifoldType.Circles;
                manifold.LocalNormal.SetZero();
                manifold.LocalPoint = P;
                ref var point = ref manifold.Points.Value0;
                point.Id.Key = 0;
                point.Id.ContactFeature = cf;
                point.LocalPoint = circleB.Position;
                return;
            }

            // Region B
            if (u <= 0.0f)
            {
                var P = B;
                var d = Q - P;
                var dd = Vector2.Dot(d, d);
                if (dd > radius * radius)
                {
                    return;
                }

                // Is there an edge connected to B?
                if (edgeA.OneSided)
                {
                    var B2 = edgeA.Vertex3;
                    var A2 = B;
                    var e2 = B2 - A2;
                    var v2 = Vector2.Dot(e2, Q - A2);

                    // Is the circle in Region AB of the next edge?
                    if (v2 > 0.0f)
                    {
                        return;
                    }
                }

                cf.IndexA = 1;
                cf.TypeA = (byte)ContactFeature.FeatureType.Vertex;
                manifold.PointCount = 1;
                manifold.Type = ManifoldType.Circles;
                manifold.LocalNormal.SetZero();
                manifold.LocalPoint = P;
                ref var point = ref manifold.Points.Value0;
                point.Id.Key = 0;
                point.Id.ContactFeature = cf;
                point.LocalPoint = circleB.Position;
                return;
            }

            {
                // Region AB
                var den = Vector2.Dot(e, e);
                Debug.Assert(den > 0.0f);
                var P = 1.0f / den * (u * A + v * B);
                var d = Q - P;
                var dd = Vector2.Dot(d, d);
                if (dd > radius * radius)
                {
                    return;
                }

                if (offset < 0.0f)
                {
                    n.Set(-n.X, -n.Y);
                }

                n.Normalize();

                cf.IndexA = 0;
                cf.TypeA = (byte)ContactFeature.FeatureType.Face;
                manifold.PointCount = 1;
                manifold.Type = ManifoldType.FaceA;
                manifold.LocalNormal = n;
                manifold.LocalPoint = A;
                ref var point = ref manifold.Points.Value0;
                point.Id.Key = 0;
                point.Id.ContactFeature = cf;
                point.LocalPoint = circleB.Position;
            }
        }

        // This structure is used to keep track of the best separating axis.
        public struct EPAxis
        {
            public enum EPAxisType
            {
                Unknown,

                EdgeA,

                EdgeB
            }

            public Vector2 Normal;

            public EPAxisType Type;

            public int Index;

            public float Separation;
        }

        // This holds polygon B expressed in frame A.
        public struct TempPolygon
        {
            /// <summary>
            /// Size Settings.MaxPolygonVertices
            /// </summary>
            public FixedArray8<Vector2> Vertices;

            /// <summary>
            /// Size Settings.MaxPolygonVertices
            /// </summary>
            public FixedArray8<Vector2> Normals;

            public int Count;
        }

        // Reference face used for clipping
        private struct ReferenceFace
        {
            public int I1, I2;

            public Vector2 Normal;

            public Vector2 SideNormal1;

            public Vector2 SideNormal2;

            public float SideOffset1;

            public float SideOffset2;

            public Vector2 V1, V2;
        }

        static EPAxis ComputeEdgeSeparation(in TempPolygon polygonB, in Vector2 v1, Vector2 normal1)
        {
            EPAxis axis = new EPAxis
            {
                Type = EPAxis.EPAxisType.EdgeA,
                Index = -1,
                Separation = -Settings.MaxFloat,
                Normal = default
            };

            var axes = new[] {normal1, -normal1};

            // Find axis with least overlap (min-max problem)
            for (int j = 0; j < 2; ++j)
            {
                float sj = Settings.MaxFloat;

                // Find deepest polygon vertex along axis j
                for (int i = 0; i < polygonB.Count; ++i)
                {
                    float si = Vector2.Dot(axes[j], polygonB.Vertices[i] - v1);
                    if (si < sj)
                    {
                        sj = si;
                    }
                }

                if (sj > axis.Separation)
                {
                    axis.Index = j;
                    axis.Separation = sj;
                    axis.Normal = axes[j];
                }
            }

            return axis;
        }

        static EPAxis ComputePolygonSeparation(in TempPolygon polygonB, in Vector2 v1, in Vector2 v2)
        {
            var axis = new EPAxis
            {
                Type = EPAxis.EPAxisType.Unknown,
                Index = -1,
                Separation = -Settings.MaxFloat,
                Normal = default
            };

            for (var i = 0; i < polygonB.Count; ++i)
            {
                var n = -polygonB.Normals[i];

                var s1 = Vector2.Dot(n, polygonB.Vertices[i] - v1);
                var s2 = Vector2.Dot(n, polygonB.Vertices[i] - v2);
                var s = Math.Min(s1, s2);

                if (s > axis.Separation)
                {
                    axis.Type = EPAxis.EPAxisType.EdgeB;
                    axis.Index = i;
                    axis.Separation = s;
                    axis.Normal = n;
                }
            }

            return axis;
        }

        public static void CollideEdgeAndPolygon(
            ref Manifold manifold,
            EdgeShape edgeA,
            Transform xfA,
            PolygonShape polygonB,
            in Transform xfB)
        {
            manifold.PointCount = 0;

            Transform xf = MathUtils.MulT(xfA, xfB);

            Vector2 centroidB = MathUtils.Mul(xf, polygonB.Centroid);

            Vector2 v1 = edgeA.Vertex1;
            Vector2 v2 = edgeA.Vertex2;

            Vector2 edge1 = v2 - v1;
            edge1.Normalize();

            // Normal points to the right for a CCW winding
            Vector2 normal1 = new Vector2(edge1.Y, -edge1.X);
            float offset1 = Vector2.Dot(normal1, centroidB - v1);

            bool oneSided = edgeA.OneSided;
            if (oneSided && offset1 < 0.0f)
            {
                return;
            }

            // Get polygonB in frameA
            TempPolygon tempPolygonB = new TempPolygon();
            tempPolygonB.Count = polygonB.Count;
            for (int i = 0; i < polygonB.Count; ++i)
            {
                tempPolygonB.Vertices[i] = MathUtils.Mul(xf, polygonB.Vertices[i]);
                tempPolygonB.Normals[i] = MathUtils.Mul(xf.Rotation, polygonB.Normals[i]);
            }

            float radius = polygonB.Radius + edgeA.Radius;

            EPAxis edgeAxis = ComputeEdgeSeparation(tempPolygonB, v1, normal1);
            if (edgeAxis.Separation > radius)
            {
                return;
            }

            EPAxis polygonAxis = ComputePolygonSeparation(tempPolygonB, v1, v2);
            if (polygonAxis.Separation > radius)
            {
                return;
            }

            // Use hysteresis for jitter reduction.
            const float k_relativeTol = 0.98f;
            const float k_absoluteTol = 0.001f;

            var primaryAxis = new EPAxis();
            if (primaryAxis.Separation - radius > k_relativeTol * (edgeAxis.Separation - radius) + k_absoluteTol)
            {
                primaryAxis = polygonAxis;
            }
            else
            {
                primaryAxis = edgeAxis;
            }

            if (oneSided)
            {
                // Smooth collision
                // See https://box2d.org/posts/2020/06/ghost-collisions/

                Vector2 edge0 = v1 - edgeA.Vertex0;
                edge0.Normalize();
                Vector2 normal0 = new Vector2(edge0.Y, -edge0.X);
                bool convex1 = MathUtils.Cross(edge0, edge1) >= 0.0f;

                Vector2 edge2 = edgeA.Vertex3 - v2;
                edge2.Normalize();
                Vector2 normal2 = new Vector2(edge2.Y, -edge2.X);
                bool convex2 = MathUtils.Cross(edge1, edge2) >= 0.0f;

                const float sinTol = 0.1f;
                bool side1 = Vector2.Dot(primaryAxis.Normal, edge1) <= 0.0f;

                // Check Gauss Map
                if (side1)
                {
                    if (convex1)
                    {
                        if (MathUtils.Cross(primaryAxis.Normal, normal0) > sinTol)
                        {
                            // Skip region
                            return;
                        }

                        // Admit region
                    }
                    else
                    {
                        // Snap region
                        primaryAxis = edgeAxis;
                    }
                }
                else
                {
                    if (convex2)
                    {
                        if (MathUtils.Cross(normal2, primaryAxis.Normal) > sinTol)
                        {
                            // Skip region
                            return;
                        }

                        // Admit region
                    }
                    else
                    {
                        // Snap region
                        primaryAxis = edgeAxis;
                    }
                }
            }

            ClipVertex[] clipPoints = new ClipVertex[2];
            ReferenceFace refFace = new ReferenceFace();
            if (primaryAxis.Type == EPAxis.EPAxisType.EdgeA)
            {
                manifold.Type = ManifoldType.FaceA;

                // Search for the polygon normal that is most anti-parallel to the edge normal.
                int bestIndex = 0;
                float bestValue = Vector2.Dot(primaryAxis.Normal, tempPolygonB.Normals[0]);
                for (int i = 1; i < tempPolygonB.Count; ++i)
                {
                    float value = Vector2.Dot(primaryAxis.Normal, tempPolygonB.Normals[i]);
                    if (value < bestValue)
                    {
                        bestValue = value;
                        bestIndex = i;
                    }
                }

                int i1 = bestIndex;
                int i2 = i1 + 1 < tempPolygonB.Count ? i1 + 1 : 0;

                clipPoints[0].Vector = tempPolygonB.Vertices[i1];
                clipPoints[0].Id.ContactFeature.IndexA = 0;
                clipPoints[0].Id.ContactFeature.IndexB = (byte)i1;
                clipPoints[0].Id.ContactFeature.TypeA = (byte)ContactFeature.FeatureType.Face;
                clipPoints[0].Id.ContactFeature.TypeB = (byte)ContactFeature.FeatureType.Vertex;

                clipPoints[1].Vector = tempPolygonB.Vertices[i2];
                clipPoints[1].Id.ContactFeature.IndexA = 0;
                clipPoints[1].Id.ContactFeature.IndexB = (byte)i2;
                clipPoints[1].Id.ContactFeature.TypeA = (byte)ContactFeature.FeatureType.Face;
                clipPoints[1].Id.ContactFeature.TypeB = (byte)ContactFeature.FeatureType.Vertex;

                refFace.I1 = 0;
                refFace.I2 = 1;
                refFace.V1 = v1;
                refFace.V2 = v2;
                refFace.Normal = primaryAxis.Normal;
                refFace.SideNormal1 = -edge1;
                refFace.SideNormal2 = edge1;
            }
            else
            {
                manifold.Type = ManifoldType.FaceB;

                clipPoints[0].Vector = v2;
                clipPoints[0].Id.ContactFeature.IndexA = 1;
                clipPoints[0].Id.ContactFeature.IndexB = (byte)primaryAxis.Index;
                clipPoints[0].Id.ContactFeature.TypeA = (byte)ContactFeature.FeatureType.Vertex;
                clipPoints[0].Id.ContactFeature.TypeB = (byte)ContactFeature.FeatureType.Face;

                clipPoints[1].Vector = v1;
                clipPoints[1].Id.ContactFeature.IndexA = 0;
                clipPoints[1].Id.ContactFeature.IndexB = (byte)primaryAxis.Index;
                clipPoints[1].Id.ContactFeature.TypeA = (byte)ContactFeature.FeatureType.Vertex;
                clipPoints[1].Id.ContactFeature.TypeB = (byte)ContactFeature.FeatureType.Face;

                refFace.I1 = primaryAxis.Index;
                refFace.I2 = refFace.I1 + 1 < tempPolygonB.Count ? refFace.I1 + 1 : 0;
                refFace.V1 = tempPolygonB.Vertices[refFace.I1];
                refFace.V2 = tempPolygonB.Vertices[refFace.I2];
                refFace.Normal = tempPolygonB.Normals[refFace.I1];

                // CCW winding
                refFace.SideNormal1.Set(refFace.Normal.Y, -refFace.Normal.X);
                refFace.SideNormal2 = -refFace.SideNormal1;
            }

            refFace.SideOffset1 = Vector2.Dot(refFace.SideNormal1, refFace.V1);
            refFace.SideOffset2 = Vector2.Dot(refFace.SideNormal2, refFace.V2);

            // Clip incident edge against reference face side planes
            Span<ClipVertex> clipPoints1 = stackalloc ClipVertex[2];
            Span<ClipVertex> clipPoints2 = stackalloc ClipVertex[2];
            int np;

            // Clip to side 1
            np = ClipSegmentToLine(clipPoints1, clipPoints, refFace.SideNormal1, refFace.SideOffset1, refFace.I1);

            if (np < Settings.MaxManifoldPoints)
            {
                return;
            }

            // Clip to side 2
            np = ClipSegmentToLine(clipPoints2, clipPoints1, refFace.SideNormal2, refFace.SideOffset2, refFace.I2);

            if (np < Settings.MaxManifoldPoints)
            {
                return;
            }

            // Now clipPoints2 contains the clipped points.
            if (primaryAxis.Type == EPAxis.EPAxisType.EdgeA)
            {
                manifold.LocalNormal = refFace.Normal;
                manifold.LocalPoint = refFace.V1;
            }
            else
            {
                manifold.LocalNormal = polygonB.Normals[refFace.I1];
                manifold.LocalPoint = polygonB.Vertices[refFace.I1];
            }

            var pointCount = 0;
            for (var i = 0; i < Settings.MaxManifoldPoints; ++i)
            {
                var separation = Vector2.Dot(refFace.Normal, clipPoints2[i].Vector - refFace.V1);

                if (separation <= radius)
                {
                    ref var cp = ref manifold.Points[pointCount];

                    if (primaryAxis.Type == EPAxis.EPAxisType.EdgeA)
                    {
                        cp.LocalPoint = MathUtils.MulT(xf, clipPoints2[i].Vector);
                        cp.Id = clipPoints2[i].Id;
                    }
                    else
                    {
                        cp.LocalPoint = clipPoints2[i].Vector;
                        cp.Id.ContactFeature.TypeA = clipPoints2[i].Id.ContactFeature.TypeB;
                        cp.Id.ContactFeature.TypeB = clipPoints2[i].Id.ContactFeature.TypeA;
                        cp.Id.ContactFeature.IndexA = clipPoints2[i].Id.ContactFeature.IndexB;
                        cp.Id.ContactFeature.IndexB = clipPoints2[i].Id.ContactFeature.IndexA;
                    }

                    ++pointCount;
                }
            }

            manifold.PointCount = pointCount;
        }
    }
}