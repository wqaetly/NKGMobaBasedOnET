using System;
using System.Diagnostics;
using System.Numerics;
using System.Runtime.CompilerServices;
using Box2DSharp.Collision.Collider;
using Box2DSharp.Collision.Shapes;
using Box2DSharp.Common;

namespace Box2DSharp.Collision
{
    public static partial class CollisionUtils
    {
        /// Compute the point states given two manifolds. The states pertain to the transition from manifold1
        /// to manifold2. So state1 is either persist or remove while state2 is either add or persist.
        public static void GetPointStates(
            in PointState[] state1,
            in PointState[] state2,
            in Manifold manifold1,
            in Manifold manifold2)
        {
            for (var i = 0; i < Settings.MaxManifoldPoints; ++i)
            {
                state1[i] = PointState.NullState;
                state2[i] = PointState.NullState;
            }

            // Detect persists and removes.
            for (var i = 0; i < manifold1.PointCount; ++i)
            {
                var id = manifold1.Points[i].Id;

                state1[i] = PointState.RemoveState;

                for (var j = 0; j < manifold2.PointCount; ++j)
                {
                    if (manifold2.Points[j].Id.Key == id.Key)
                    {
                        state1[i] = PointState.PersistState;
                        break;
                    }
                }
            }

            // Detect persists and adds.
            for (var i = 0; i < manifold2.PointCount; ++i)
            {
                var id = manifold2.Points[i].Id;

                state2[i] = PointState.AddState;

                for (var j = 0; j < manifold1.PointCount; ++j)
                {
                    if (manifold1.Points[j].Id.Key == id.Key)
                    {
                        state2[i] = PointState.PersistState;
                        break;
                    }
                }
            }
        }

        /// Clipping for contact manifolds.
        public static int ClipSegmentToLine(
            in Span<ClipVertex> vOut,
            in Span<ClipVertex> vIn,
            in Vector2 normal,
            float offset,
            int vertexIndexA)
        {
            // Start with no output points
            var count = 0;

            // Calculate the distance of end points to the line
            var distance0 = Vector2.Dot(normal, vIn[0].Vector) - offset;
            var distance1 = Vector2.Dot(normal, vIn[1].Vector) - offset;

            // If the points are behind the plane
            if (distance0 <= 0.0f)
            {
                vOut[count++] = vIn[0];
            }

            if (distance1 <= 0.0f)
            {
                vOut[count++] = vIn[1];
            }

            // If the points are on different sides of the plane
            if (distance0 * distance1 < 0.0f)
            {
                // Find intersection point of edge and plane
                var interp = distance0 / (distance0 - distance1);
                vOut[count].Vector = vIn[0].Vector + interp * (vIn[1].Vector - vIn[0].Vector);

                // VertexA is hitting edgeB.
                vOut[count].Id.ContactFeature.IndexA = (byte) vertexIndexA;
                vOut[count].Id.ContactFeature.IndexB = vIn[0].Id.ContactFeature.IndexB;
                vOut[count].Id.ContactFeature.TypeA = (byte) ContactFeature.FeatureType.Vertex;
                vOut[count].Id.ContactFeature.TypeB = (byte) ContactFeature.FeatureType.Face;
                ++count;
                Debug.Assert(count == 2);
            }

            return count;
        }

        /// Determine if two generic shapes overlap.
        public static bool TestOverlap(
            Shape shapeA,
            int indexA,
            Shape shapeB,
            int indexB,
            in Transform xfA,
            in Transform xfB,
            GJkProfile gJkProfile)
        {
            var input = new DistanceInput();
            input.ProxyA.Set(shapeA, indexA);
            input.ProxyB.Set(shapeB, indexB);
            input.TransformA = xfA;
            input.TransformB = xfB;
            input.UseRadii = true;

            var cache = new SimplexCache();
            DistanceAlgorithm.Distance(out var output, ref cache, input, gJkProfile);
            return output.Distance < 10.0f * Settings.Epsilon;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TestOverlap(in AABB a, AABB b)
        {
            //var d1 = b.LowerBound - a.UpperBound;

            if (b.LowerBound.X - a.UpperBound.X > 0.0f || b.LowerBound.Y - a.UpperBound.Y > 0.0f)
            {
                return false;
            }

            //var d2 = a.LowerBound - b.UpperBound;
            if (a.LowerBound.X - b.UpperBound.X > 0.0f || a.LowerBound.Y - b.UpperBound.Y > 0.0f)
            {
                return false;
            }

            return true;
        }
    }
}