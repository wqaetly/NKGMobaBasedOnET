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

            // Barycentric coordinates
            // 质心坐标
            var u = Vector2.Dot(e, B - Q);
            var v = Vector2.Dot(e, Q - A);

            var radius = edgeA.Radius + circleB.Radius;

            var cf = new ContactFeature {IndexB = 0, TypeB = (byte) ContactFeature.FeatureType.Vertex};

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
                if (edgeA.HasVertex0)
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
                cf.TypeA = (byte) ContactFeature.FeatureType.Vertex;
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
                if (edgeA.HasVertex3)
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
                cf.TypeA = (byte) ContactFeature.FeatureType.Vertex;
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

                var n = new Vector2(-e.Y, e.X);
                if (Vector2.Dot(n, Q - A) < 0.0f)
                {
                    n.Set(-n.X, -n.Y);
                }

                n.Normalize();

                cf.IndexA = 0;
                cf.TypeA = (byte) ContactFeature.FeatureType.Face;
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

        /// Compute the collision manifold between an edge and a circle.
        public static void CollideEdgeAndPolygon(
            ref Manifold manifold,
            EdgeShape edgeA,
            in Transform xfA,
            PolygonShape polygonB,
            in Transform xfB)
        {
            new EPCollider().Collide(
                ref manifold,
                ref edgeA,
                xfA,
                ref polygonB,
                xfB);
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

        // This class collides and edge and a polygon, taking into account edge adjacency.
        public struct EPCollider
        {
            public enum VertexType
            {
                Isolated,

                Concave,

                Convex
            }

            public Vector2 CentroidB;

            public bool Front;

            public Vector2 LowerLimit, UpperLimit;

            public Vector2 Normal;

            public Vector2 Normal0, Normal1, Normal2;

            public TempPolygon PolygonB;

            public float Radius;

            public Transform Transform;

            public VertexType Type1, Type2;

            public Vector2 V0, V1, V2, V3;

            // Algorithm:
            // 1. Classify v1 and v2
            // 2. Classify polygon centroid as front or back
            // 3. Flip normal if necessary
            // 4. Initialize normal range to [-pi, pi] about face normal
            // 5. Adjust normal range according to adjacent edges
            // 6. Visit each separating axes, only accept axes within the range
            // 7. Return if _any_ axis indicates separation
            // 8. Clip
            public void Collide(
                ref Manifold manifold,
                ref EdgeShape edgeA,
                in Transform xfA,
                ref PolygonShape polygonB,
                in Transform xfB)
            {
                Transform = MathUtils.MulT(xfA, xfB);

                CentroidB = MathUtils.Mul(Transform, polygonB.Centroid);

                V0 = edgeA.Vertex0;
                V1 = edgeA.Vertex1;
                V2 = edgeA.Vertex2;
                V3 = edgeA.Vertex3;

                var hasVertex0 = edgeA.HasVertex0;
                var hasVertex3 = edgeA.HasVertex3;

                var edge1 = V2 - V1;
                edge1.Normalize();
                Normal1.Set(edge1.Y, -edge1.X);
                var offset1 = Vector2.Dot(Normal1, CentroidB - V1);
                float offset0 = 0.0f, offset2 = 0.0f;
                bool convex1 = false, convex2 = false;

                // Is there a preceding edge?
                if (hasVertex0)
                {
                    var edge0 = V1 - V0;
                    edge0.Normalize();
                    Normal0.Set(edge0.Y, -edge0.X);
                    convex1 = MathUtils.Cross(edge0, edge1) >= 0.0f;
                    offset0 = Vector2.Dot(Normal0, CentroidB - V0);
                }

                // Is there a following edge?
                if (hasVertex3)
                {
                    var edge2 = V3 - V2;
                    edge2.Normalize();
                    Normal2.Set(edge2.Y, -edge2.X);
                    convex2 = MathUtils.Cross(edge1, edge2) > 0.0f;
                    offset2 = Vector2.Dot(Normal2, CentroidB - V2);
                }

                // Determine front or back  Determine collision normal limits.
                if (hasVertex0 && hasVertex3)
                {
                    if (convex1 && convex2)
                    {
                        Front = offset0 >= 0.0f || offset1 >= 0.0f || offset2 >= 0.0f;
                        if (Front)
                        {
                            Normal = Normal1;
                            LowerLimit = Normal0;
                            UpperLimit = Normal2;
                        }
                        else
                        {
                            Normal = -Normal1;
                            LowerLimit = -Normal1;
                            UpperLimit = -Normal1;
                        }
                    }
                    else if (convex1)
                    {
                        Front = offset0 >= 0.0f || (offset1 >= 0.0f && offset2 >= 0.0f);
                        if (Front)
                        {
                            Normal = Normal1;
                            LowerLimit = Normal0;
                            UpperLimit = Normal1;
                        }
                        else
                        {
                            Normal = -Normal1;
                            LowerLimit = -Normal2;
                            UpperLimit = -Normal1;
                        }
                    }
                    else if (convex2)
                    {
                        Front = offset2 >= 0.0f || (offset0 >= 0.0f && offset1 >= 0.0f);
                        if (Front)
                        {
                            Normal = Normal1;
                            LowerLimit = Normal1;
                            UpperLimit = Normal2;
                        }
                        else
                        {
                            Normal = -Normal1;
                            LowerLimit = -Normal1;
                            UpperLimit = -Normal0;
                        }
                    }
                    else
                    {
                        Front = offset0 >= 0.0f && offset1 >= 0.0f && offset2 >= 0.0f;
                        if (Front)
                        {
                            Normal = Normal1;
                            LowerLimit = Normal1;
                            UpperLimit = Normal1;
                        }
                        else
                        {
                            Normal = -Normal1;
                            LowerLimit = -Normal2;
                            UpperLimit = -Normal0;
                        }
                    }
                }
                else if (hasVertex0)
                {
                    if (convex1)
                    {
                        Front = offset0 >= 0.0f || offset1 >= 0.0f;
                        if (Front)
                        {
                            Normal = Normal1;
                            LowerLimit = Normal0;
                            UpperLimit = -Normal1;
                        }
                        else
                        {
                            Normal = -Normal1;
                            LowerLimit = Normal1;
                            UpperLimit = -Normal1;
                        }
                    }
                    else
                    {
                        Front = offset0 >= 0.0f && offset1 >= 0.0f;
                        if (Front)
                        {
                            Normal = Normal1;
                            LowerLimit = Normal1;
                            UpperLimit = -Normal1;
                        }
                        else
                        {
                            Normal = -Normal1;
                            LowerLimit = Normal1;
                            UpperLimit = -Normal0;
                        }
                    }
                }
                else if (hasVertex3)
                {
                    if (convex2)
                    {
                        Front = offset1 >= 0.0f || offset2 >= 0.0f;
                        if (Front)
                        {
                            Normal = Normal1;
                            LowerLimit = -Normal1;
                            UpperLimit = Normal2;
                        }
                        else
                        {
                            Normal = -Normal1;
                            LowerLimit = -Normal1;
                            UpperLimit = Normal1;
                        }
                    }
                    else
                    {
                        Front = offset1 >= 0.0f && offset2 >= 0.0f;
                        if (Front)
                        {
                            Normal = Normal1;
                            LowerLimit = -Normal1;
                            UpperLimit = Normal1;
                        }
                        else
                        {
                            Normal = -Normal1;
                            LowerLimit = -Normal2;
                            UpperLimit = Normal1;
                        }
                    }
                }
                else
                {
                    Front = offset1 >= 0.0f;
                    if (Front)
                    {
                        Normal = Normal1;
                        LowerLimit = -Normal1;
                        UpperLimit = -Normal1;
                    }
                    else
                    {
                        Normal = -Normal1;
                        LowerLimit = Normal1;
                        UpperLimit = Normal1;
                    }
                }

                // Get polygonB in frameA
                PolygonB = new TempPolygon {Count = polygonB.Count};
                for (var i = 0; i < polygonB.Count; ++i)
                {
                    PolygonB.Vertices[i] = MathUtils.Mul(Transform, polygonB.Vertices[i]);
                    PolygonB.Normals[i] = MathUtils.Mul(Transform.Rotation, polygonB.Normals[i]);
                }

                Radius = polygonB.Radius + edgeA.Radius;

                manifold.PointCount = 0;

                var edgeAxis = ComputeEdgeSeparation();

                // If no valid normal can be found than this edge should not collide.
                if (edgeAxis.Type == EPAxis.EPAxisType.Unknown)
                {
                    return;
                }

                if (edgeAxis.Separation > Radius)
                {
                    return;
                }

                var polygonAxis = ComputePolygonSeparation();
                if (polygonAxis.Type != EPAxis.EPAxisType.Unknown && polygonAxis.Separation > Radius)
                {
                    return;
                }

                // Use hysteresis for jitter reduction.
                const float k_relativeTol = 0.98f;
                const float k_absoluteTol = 0.001f;

                EPAxis primaryAxis;
                if (polygonAxis.Type == EPAxis.EPAxisType.Unknown)
                {
                    primaryAxis = edgeAxis;
                }
                else if (polygonAxis.Separation > k_relativeTol * edgeAxis.Separation + k_absoluteTol)
                {
                    primaryAxis = polygonAxis;
                }
                else
                {
                    primaryAxis = edgeAxis;
                }

                Span<ClipVertex> ie = stackalloc ClipVertex[2];
                var rf = new ReferenceFace();
                if (primaryAxis.Type == EPAxis.EPAxisType.EdgeA)
                {
                    manifold.Type = ManifoldType.FaceA;

                    // Search for the polygon normal that is most anti-parallel to the edge normal.
                    var bestIndex = 0;
                    var bestValue = Vector2.Dot(Normal, PolygonB.Normals[0]);
                    for (var i = 1; i < PolygonB.Count; ++i)
                    {
                        var value = Vector2.Dot(Normal, PolygonB.Normals[i]);
                        if (value < bestValue)
                        {
                            bestValue = value;
                            bestIndex = i;
                        }
                    }

                    var i1 = bestIndex;
                    var i2 = i1 + 1 < PolygonB.Count ? i1 + 1 : 0;

                    ie[0].Vector = PolygonB.Vertices[i1];
                    ie[0].Id.ContactFeature.IndexA = 0;
                    ie[0].Id.ContactFeature.IndexB = (byte) i1;
                    ie[0].Id.ContactFeature.TypeA = (byte) ContactFeature.FeatureType.Face;
                    ie[0].Id.ContactFeature.TypeB = (byte) ContactFeature.FeatureType.Vertex;

                    ie[1].Vector = PolygonB.Vertices[i2];
                    ie[1].Id.ContactFeature.IndexA = 0;
                    ie[1].Id.ContactFeature.IndexB = (byte) i2;
                    ie[1].Id.ContactFeature.TypeA = (byte) ContactFeature.FeatureType.Face;
                    ie[1].Id.ContactFeature.TypeB = (byte) ContactFeature.FeatureType.Vertex;

                    if (Front)
                    {
                        rf.I1 = 0;
                        rf.I2 = 1;
                        rf.V1 = V1;
                        rf.V2 = V2;
                        rf.Normal = Normal1;
                    }
                    else
                    {
                        rf.I1 = 1;
                        rf.I2 = 0;
                        rf.V1 = V2;
                        rf.V2 = V1;
                        rf.Normal = -Normal1;
                    }
                }
                else
                {
                    manifold.Type = ManifoldType.FaceB;

                    ie[0].Vector = V1;
                    ie[0].Id.ContactFeature.IndexA = 0;
                    ie[0].Id.ContactFeature.IndexB = (byte) primaryAxis.Index;
                    ie[0].Id.ContactFeature.TypeA = (byte) ContactFeature.FeatureType.Vertex;
                    ie[0].Id.ContactFeature.TypeB = (byte) ContactFeature.FeatureType.Face;

                    ie[1].Vector = V2;
                    ie[1].Id.ContactFeature.IndexA = 0;
                    ie[1].Id.ContactFeature.IndexB = (byte) primaryAxis.Index;
                    ie[1].Id.ContactFeature.TypeA = (byte) ContactFeature.FeatureType.Vertex;
                    ie[1].Id.ContactFeature.TypeB = (byte) ContactFeature.FeatureType.Face;

                    rf.I1 = primaryAxis.Index;
                    rf.I2 = rf.I1 + 1 < PolygonB.Count ? rf.I1 + 1 : 0;
                    rf.V1 = PolygonB.Vertices[rf.I1];
                    rf.V2 = PolygonB.Vertices[rf.I2];
                    rf.Normal = PolygonB.Normals[rf.I1];
                }

                rf.SideNormal1.Set(rf.Normal.Y, -rf.Normal.X);
                rf.SideNormal2 = -rf.SideNormal1;
                rf.SideOffset1 = Vector2.Dot(rf.SideNormal1, rf.V1);
                rf.SideOffset2 = Vector2.Dot(rf.SideNormal2, rf.V2);

                // Clip incident edge against extruded edge1 side edges.
                Span<ClipVertex> clipPoints1 = stackalloc ClipVertex[2];
                Span<ClipVertex> clipPoints2 = stackalloc ClipVertex[2];
                int np;

                // Clip to box side 1
                np = ClipSegmentToLine(
                    clipPoints1,
                    ie,
                    rf.SideNormal1,
                    rf.SideOffset1,
                    rf.I1);

                if (np < Settings.MaxManifoldPoints)
                {
                    return;
                }

                // Clip to negative box side 1
                np = ClipSegmentToLine(
                    clipPoints2,
                    clipPoints1,
                    rf.SideNormal2,
                    rf.SideOffset2,
                    rf.I2);

                if (np < Settings.MaxManifoldPoints)
                {
                    return;
                }

                // Now clipPoints2 contains the clipped points.
                if (primaryAxis.Type == EPAxis.EPAxisType.EdgeA)
                {
                    manifold.LocalNormal = rf.Normal;
                    manifold.LocalPoint = rf.V1;
                }
                else
                {
                    manifold.LocalNormal = polygonB.Normals[rf.I1];
                    manifold.LocalPoint = polygonB.Vertices[rf.I1];
                }

                var pointCount = 0;
                for (var i = 0; i < Settings.MaxManifoldPoints; ++i)
                {
                    var separation = Vector2.Dot(rf.Normal, clipPoints2[i].Vector - rf.V1);

                    if (separation <= Radius)
                    {
                        ref var cp = ref manifold.Points[pointCount];

                        if (primaryAxis.Type == EPAxis.EPAxisType.EdgeA)
                        {
                            cp.LocalPoint = MathUtils.MulT(Transform, clipPoints2[i].Vector);
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

            public EPAxis ComputeEdgeSeparation()
            {
                EPAxis axis;
                axis.Type = EPAxis.EPAxisType.EdgeA;
                axis.Index = Front ? 0 : 1;
                axis.Separation = float.MaxValue;

                for (var i = 0; i < PolygonB.Count; ++i)
                {
                    var s = Vector2.Dot(Normal, PolygonB.Vertices[i] - V1);
                    if (s < axis.Separation)
                    {
                        axis.Separation = s;
                    }
                }

                return axis;
            }

            public EPAxis ComputePolygonSeparation()
            {
                EPAxis axis;
                axis.Type = EPAxis.EPAxisType.Unknown;
                axis.Index = -1;
                axis.Separation = -float.MaxValue;

                var perp = new Vector2(-Normal.Y, Normal.X);

                for (var i = 0; i < PolygonB.Count; ++i)
                {
                    var n = -PolygonB.Normals[i];

                    var s1 = Vector2.Dot(n, PolygonB.Vertices[i] - V1);
                    var s2 = Vector2.Dot(n, PolygonB.Vertices[i] - V2);
                    var s = Math.Min(s1, s2);

                    if (s > Radius)
                    {
                        // No collision
                        axis.Type = EPAxis.EPAxisType.EdgeB;
                        axis.Index = i;
                        axis.Separation = s;
                        return axis;
                    }

                    // Adjacency
                    if (Vector2.Dot(n, perp) >= 0.0f)
                    {
                        if (Vector2.Dot(n - UpperLimit, Normal) < -Settings.AngularSlop)
                        {
                            continue;
                        }
                    }
                    else
                    {
                        if (Vector2.Dot(n - LowerLimit, Normal) < -Settings.AngularSlop)
                        {
                            continue;
                        }
                    }

                    if (s > axis.Separation)
                    {
                        axis.Type = EPAxis.EPAxisType.EdgeB;
                        axis.Index = i;
                        axis.Separation = s;
                    }
                }

                return axis;
            }
        }
    }
}