using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Numerics;
using System.Runtime.CompilerServices;
using Box2DSharp.Collision.Shapes;

namespace Box2DSharp.Collision
{
    /// A distance proxy is used by the GJK algorithm.
    /// It encapsulates any shape.
    public struct DistanceProxy
    {
        /// Initialize the proxy using the given shape. The shape
        /// must remain in scope while the proxy is in use.
        public void Set(Shape shape, int index)
        {
            switch (shape)
            {
            case CircleShape circle:
            {
                Vertices = new[] {circle.Position};
                Count = 1;
                Radius = circle.Radius;
            }
                break;

            case PolygonShape polygon:
            {
                Vertices = polygon.Vertices;
                Count = polygon.Count;
                Radius = polygon.Radius;
            }
                break;

            case ChainShape chain:
            {
                Debug.Assert(0 <= index && index < chain.Count);
                Count = 2;
                Vertices = new Vector2[Count];
                Vertices[0] = chain.Vertices[index];
                if (index + 1 < chain.Count)
                {
                    Vertices[1] = chain.Vertices[index + 1];
                }
                else
                {
                    Vertices[1] = chain.Vertices[0];
                }

                Radius = chain.Radius;
            }
                break;

            case EdgeShape edge:
            {
                Vertices = new[]
                {
                    edge.Vertex1,
                    edge.Vertex2
                };
                Count = 2;
                Radius = edge.Radius;
            }
                break;

            default:
                throw new NotSupportedException();
            }
        }

        /// Initialize the proxy using a vertex cloud and radius. The vertices
        /// must remain in scope while the proxy is in use.
        public void Set(Vector2[] vertices, int count, float radius)
        {
            Vertices = new Vector2[vertices.Length];
            Array.Copy(vertices, Vertices, vertices.Length);
            Count = count;
            Radius = radius;
        }

        /// Get the supporting vertex index in the given direction.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [Pure]
        public int GetSupport(in Vector2 d)
        {
            var bestIndex = 0;
            var bestValue = Vector2.Dot(Vertices[0], d);
            for (var i = 1; i < Count; ++i)
            {
                var value = Vector2.Dot(Vertices[i], d);
                if (value > bestValue)
                {
                    bestIndex = i;
                    bestValue = value;
                }
            }

            return bestIndex;
        }

        /// Get the supporting vertex in the given direction.
        public ref readonly Vector2 GetSupportVertex(in Vector2 d)
        {
            var bestIndex = 0;
            var bestValue = Vector2.Dot(Vertices[0], d);
            for (var i = 1; i < Count; ++i)
            {
                var value = Vector2.Dot(Vertices[i], d);
                if (value > bestValue)
                {
                    bestIndex = i;
                    bestValue = value;
                }
            }

            return ref Vertices[bestIndex];
        }

        /// Get the vertex count.
        public int GetVertexCount()
        {
            return Count;
        }

        /// Get a vertex by index. Used by b2Distance.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [Pure]
        public ref readonly Vector2 GetVertex(int index)
        {
            Debug.Assert(0 <= index && index < Count);
            return ref Vertices[index];
        }

        public Vector2[] Vertices;

        public int Count;

        public float Radius;
    }

    public class GJkProfile
    {
        // GJK using Voronoi regions (Christer Ericson) and Barycentric coordinates.
        public int GjkCalls;

        public int GjkIters;

        public int GjkMaxIters;
    }
}