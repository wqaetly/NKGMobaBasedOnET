using System;
using System.Diagnostics;
using System.Numerics;
using Box2DSharp.Collision.Collider;
using Box2DSharp.Common;

namespace Box2DSharp.Collision.Shapes
{
    /// <summary>
    /// A chain shape is a free form sequence of line segments.
    /// The chain has one-sided collision, with the surface normal pointing to the right of the edge.
    /// This provides a counter-clockwise winding like the polygon shape.
    /// Connectivity information is used to create smooth collisions.
    /// <para>@warning the chain will not collide properly if there are self-intersections.</para>
    /// </summary>
    public class ChainShape : Shape
    {
        /// The vertex count.
        public int Count;

        public Vector2 PrevVertex;

        public Vector2 NextVertex;

        /// The vertices. Owned by this class.
        public Vector2[] Vertices;

        public ChainShape()
        {
            ShapeType = ShapeType.Chain;
            Radius = Settings.PolygonRadius;
            Vertices = null;
            Count = 0;
        }

        /// Implement b2Shape. Vertices are cloned using b2Alloc.
        public override Shape Clone()
        {
            var clone = new ChainShape {Vertices = new Vector2[Vertices.Length]};
            Array.Copy(Vertices, clone.Vertices, Vertices.Length);
            clone.Count = Count;
            clone.PrevVertex = PrevVertex;
            clone.NextVertex = NextVertex;
            return clone;
        }

        /// <summary>
        /// Clear all data.
        /// </summary>
        public void Clear()
        {
            Vertices = null;
            Count = 0;
        }

        /// Create a loop. This automatically adjusts connectivity.
        /// @param vertices an array of vertices, these are copied
        /// @param count the vertex count
        public void CreateLoop(Vector2[] vertices, int count = -1)
        {
            if (count == -1)
            {
                count = vertices.Length;
            }

            Debug.Assert(Vertices == null && Count == 0);
            Debug.Assert(count >= 3);
            if (count < 3)
            {
                return;
            }

            for (var i = 1; i < count; ++i)
            {
                var v1 = vertices[i - 1];
                var v2 = vertices[i];

                // If the code crashes here, it means your vertices are too close together.
                Debug.Assert(Vector2.DistanceSquared(v1, v2) > Settings.LinearSlop * Settings.LinearSlop);
            }

            Count = count + 1;
            Vertices = new Vector2[Count];
            Array.Copy(vertices, Vertices, count);
            Vertices[count] = Vertices[0];
            PrevVertex = Vertices[Count - 2];
            NextVertex = Vertices[1];
        }

        /// <summary>
        /// Create a chain with ghost vertices to connect multiple chains together.
        /// </summary>
        /// <param name="vertices">an array of vertices, these are copied</param>
        /// <param name="count">the vertex count</param>
        /// <param name="prevVertex">previous vertex from chain that connects to the start</param>
        /// <param name="nextVertex">next vertex from chain that connects to the end</param>
        public void CreateChain(Vector2[] vertices, int count, Vector2 prevVertex, Vector2 nextVertex)
        {
            Debug.Assert(Vertices == null && Count == 0);
            Debug.Assert(count >= 2);
            for (var i = 1; i < count; ++i)
            {
                // If the code crashes here, it means your vertices are too close together.
                Debug.Assert(
                    Vector2.DistanceSquared(vertices[i - 1], vertices[i])
                  > Settings.LinearSlop * Settings.LinearSlop);
            }

            Count = count;
            Vertices = new Vector2[count];
            Array.Copy(vertices, Vertices, count);

            PrevVertex = prevVertex;
            NextVertex = nextVertex;
        }

        /// @see b2Shape::GetChildCount
        public override int GetChildCount()
        {
            return Count - 1;
        }

        /// Get a child edge.
        public void GetChildEdge(out EdgeShape edge, int index)
        {
            Debug.Assert(0 <= index && index < Count - 1);
            edge = new EdgeShape
            {
                ShapeType = ShapeType.Edge,
                Radius = Radius,
                Vertex1 = Vertices[index + 0],
                Vertex2 = Vertices[index + 1],
                OneSided = true,
                Vertex0 = index > 0 ? Vertices[index - 1] : PrevVertex,
                Vertex3 = index < Count - 2 ? Vertices[index + 2] : NextVertex
            };
        }

        /// This always return false.
        /// @see b2Shape::TestPoint
        public override bool TestPoint(in Transform transform, in Vector2 p)
        {
            return false;
        }

        /// Implement b2Shape.
        public override bool RayCast(
            out RayCastOutput output,
            in RayCastInput input,
            in Transform transform,
            int childIndex)
        {
            Debug.Assert(childIndex < Count);

            var edgeShape = new EdgeShape();

            var i1 = childIndex;
            var i2 = childIndex + 1;
            if (i2 == Count)
            {
                i2 = 0;
            }

            edgeShape.Vertex1 = Vertices[i1];
            edgeShape.Vertex2 = Vertices[i2];

            return edgeShape.RayCast(out output, input, transform, 0);
        }

        /// @see b2Shape::ComputeAABB
        public override void ComputeAABB(out AABB aabb, in Transform transform, int childIndex)
        {
            Debug.Assert(childIndex < Count);

            var i1 = childIndex;
            var i2 = childIndex + 1;
            if (i2 == Count)
            {
                i2 = 0;
            }

            var v1 = MathUtils.Mul(transform, Vertices[i1]);
            var v2 = MathUtils.Mul(transform, Vertices[i2]);

            var lower = Vector2.Min(v1, v2);
            var upper = Vector2.Max(v1, v2);

            var r = new Vector2(Radius, Radius);
            aabb = new AABB(lower - r, upper + r);
        }

        /// Chains have zero mass.
        /// @see b2Shape::ComputeMass
        public override void ComputeMass(out MassData massData, float density)
        {
            massData = new MassData();
            massData.Mass = 0.0f;
            massData.Center.SetZero();
            massData.RotationInertia = 0.0f;
        }
    }
}