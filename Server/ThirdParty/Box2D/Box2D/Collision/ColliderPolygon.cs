using System;
using System.Diagnostics;
using System.Numerics;
using Box2DSharp.Collision.Collider;
using Box2DSharp.Collision.Shapes;
using Box2DSharp.Common;

namespace Box2DSharp.Collision
{
    /// <summary>
    ///     Collision Algorithm
    /// </summary>
    public static partial class CollisionUtils
    {
        // Find the max separation between poly1 and poly2 using edge normals from poly1.
        public static float FindMaxSeparation(
            out int edgeIndex,
            PolygonShape poly1,
            in Transform xf1,
            PolygonShape poly2,
            in Transform xf2)
        {
            var count1 = poly1.Count;
            var count2 = poly2.Count;

            var n1s = poly1.Normals;
            var v1s = poly1.Vertices;
            var v2s = poly2.Vertices;
            var xf = MathUtils.MulT(xf2, xf1);

            var bestIndex = 0;
            var maxSeparation = -Settings.MaxFloat;
            for (var i = 0; i < count1; ++i)
            {
                // Get poly1 normal in frame2.
                var n = MathUtils.Mul(xf.Rotation, n1s[i]);
                var v1 = MathUtils.Mul(xf, v1s[i]);

                // Find deepest point for normal i.
                var si = Settings.MaxFloat;
                for (var j = 0; j < count2; ++j)
                {
                    //var sij = Vector2.Dot(n, v2s[j] - v1);
                    ref readonly var v2sj = ref v2s[j];
                    var sij = n.X * (v2sj.X - v1.X) + n.Y * (v2sj.Y - v1.Y);
                    if (sij < si)
                    {
                        si = sij;
                    }
                }

                if (si > maxSeparation)
                {
                    maxSeparation = si;
                    bestIndex = i;
                }
            }

            edgeIndex = bestIndex;
            return maxSeparation;
        }

        public static void FindIncidentEdge(
            in Span<ClipVertex> c,
            PolygonShape poly1,
            in Transform xf1,
            int edge1,
            PolygonShape poly2,
            in Transform xf2)
        {
            var normals1 = poly1.Normals;
            var count2 = poly2.Count;
            var vertices2 = poly2.Vertices;
            var normals2 = poly2.Normals;

            Debug.Assert(0 <= edge1 && edge1 < poly1.Count);

            // Get the normal of the reference edge in poly2's frame.
            var normal1 = MathUtils.MulT(xf2.Rotation, MathUtils.Mul(xf1.Rotation, normals1[edge1]));

            // Find the incident edge on poly2.
            var index = 0;
            var minDot = Settings.MaxFloat;
            for (var i = 0; i < count2; ++i)
            {
                var dot = Vector2.Dot(normal1, normals2[i]);
                if (dot < minDot)
                {
                    minDot = dot;
                    index = i;
                }
            }

            // Build the clip vertices for the incident edge.
            var i1 = index;
            var i2 = i1 + 1 < count2 ? i1 + 1 : 0;
            c[0].Vector = MathUtils.Mul(xf2, vertices2[i1]);
            c[0].Id.ContactFeature.IndexA = (byte) edge1;
            c[0].Id.ContactFeature.IndexB = (byte) i1;
            c[0].Id.ContactFeature.TypeA = (byte) ContactFeature.FeatureType.Face;
            c[0].Id.ContactFeature.TypeB = (byte) ContactFeature.FeatureType.Vertex;

            c[1].Vector = MathUtils.Mul(xf2, vertices2[i2]);
            c[1].Id.ContactFeature.IndexA = (byte) edge1;
            c[1].Id.ContactFeature.IndexB = (byte) i2;
            c[1].Id.ContactFeature.TypeA = (byte) ContactFeature.FeatureType.Face;
            c[1].Id.ContactFeature.TypeB = (byte) ContactFeature.FeatureType.Vertex;
        }

        // Find edge normal of max separation on A - return if separating axis is found
        // Find edge normal of max separation on B - return if separation axis is found
        // Choose reference edge as min(minA, minB)
        // Find incident edge
        // Clip

        // The normal points from 1 to 2
        /// Compute the collision manifold between two polygons.
        public static void CollidePolygons(
            ref Manifold manifold,
            PolygonShape polyA,
            in Transform xfA,
            PolygonShape polyB,
            in Transform xfB)
        {
            manifold.PointCount = 0;
            var totalRadius = polyA.Radius + polyB.Radius;

            var separationA = FindMaxSeparation(
                out var edgeA,
                polyA,
                xfA,
                polyB,
                xfB);
            if (separationA > totalRadius)
            {
                return;
            }

            var separationB = FindMaxSeparation(
                out var edgeB,
                polyB,
                xfB,
                polyA,
                xfA);
            if (separationB > totalRadius)
            {
                return;
            }

            PolygonShape poly1; // reference polygon 
            PolygonShape poly2; // incident polygon 
            Transform xf1, xf2;
            int edge1; // reference edge
            byte flip;
            const float k_tol = 0.1f * Settings.LinearSlop;

            if (separationB > separationA + k_tol)
            {
                poly1 = polyB;
                poly2 = polyA;
                xf1 = xfB;
                xf2 = xfA;
                edge1 = edgeB;
                manifold.Type = ManifoldType.FaceB;
                flip = 1;
            }
            else
            {
                poly1 = polyA;
                poly2 = polyB;
                xf1 = xfA;
                xf2 = xfB;
                edge1 = edgeA;
                manifold.Type = ManifoldType.FaceA;
                flip = 0;
            }

            Span<ClipVertex> incidentEdge = stackalloc ClipVertex[2];
            FindIncidentEdge(in incidentEdge, poly1, xf1, edge1, poly2, xf2);

            var count1 = poly1.Count;
            var vertices1 = poly1.Vertices;

            var iv1 = edge1;
            var iv2 = edge1 + 1 < count1 ? edge1 + 1 : 0;

            var v11 = vertices1[iv1];
            var v12 = vertices1[iv2];

            var localTangent = v12 - v11;
            localTangent.Normalize();

            var localNormal = MathUtils.Cross(localTangent, 1.0f);
            var planePoint = 0.5f * (v11 + v12);

            var tangent = MathUtils.Mul(xf1.Rotation, localTangent);
            var normal = MathUtils.Cross(tangent, 1.0f);

            v11 = MathUtils.Mul(xf1, v11);
            v12 = MathUtils.Mul(xf1, v12);

            // Face offset.
            var frontOffset = Vector2.Dot(normal, v11);

            // Side offsets, extended by polytope skin thickness.
            var sideOffset1 = -Vector2.Dot(tangent, v11) + totalRadius;
            var sideOffset2 = Vector2.Dot(tangent, v12) + totalRadius;

            // Clip incident edge against extruded edge1 side edges.
            Span<ClipVertex> clipPoints1 = stackalloc ClipVertex[2];
            Span<ClipVertex> clipPoints2 = stackalloc ClipVertex[2];

            // Clip to box side 1
            var np = ClipSegmentToLine(
                clipPoints1,
                incidentEdge,
                -tangent,
                sideOffset1,
                iv1);

            if (np < 2)
            {
                return;
            }

            // Clip to negative box side 1
            np = ClipSegmentToLine(
                clipPoints2,
                clipPoints1,
                tangent,
                sideOffset2,
                iv2);

            if (np < 2)
            {
                return;
            }

            // Now clipPoints2 contains the clipped points.
            manifold.LocalNormal = localNormal;
            manifold.LocalPoint = planePoint;

            var pointCount = 0;
            for (var i = 0; i < Settings.MaxManifoldPoints; ++i)
            {
                var separation = Vector2.Dot(normal, clipPoints2[i].Vector) - frontOffset;

                if (separation <= totalRadius)
                {
                    ref var cp = ref manifold.Points[pointCount];
                    cp.LocalPoint = MathUtils.MulT(xf2, clipPoints2[i].Vector);
                    cp.Id = clipPoints2[i].Id;
                    if (flip != default)
                    {
                        // Swap features
                        var cf = cp.Id.ContactFeature;
                        cp.Id.ContactFeature.IndexA = cf.IndexB;
                        cp.Id.ContactFeature.IndexB = cf.IndexA;
                        cp.Id.ContactFeature.TypeA = cf.TypeB;
                        cp.Id.ContactFeature.TypeB = cf.TypeA;
                    }

                    ++pointCount;
                }
            }

            manifold.PointCount = pointCount;
        }
    }
}