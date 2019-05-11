using System.Numerics;
using Box2DSharp.Collision.Collider;
using Box2DSharp.Common;

namespace Box2DSharp.Collision.Shapes
{
    public class EdgeShape : Shape
    {
        public bool HasVertex0, HasVertex3;

        /// Optional adjacent vertices. These are used for smooth collision.
        public Vector2 Vertex0;

        /// These are the edge vertices
        public Vector2 Vertex1;

        /// These are the edge vertices
        public Vector2 Vertex2;

        /// Optional adjacent vertices. These are used for smooth collision.
        public Vector2 Vertex3;

        public EdgeShape()
        {
            ShapeType = ShapeType.Edge;
            Radius = Settings.PolygonRadius;
            Vertex0.X = 0.0f;
            Vertex0.Y = 0.0f;
            Vertex3.X = 0.0f;
            Vertex3.Y = 0.0f;
            HasVertex0 = false;
            HasVertex3 = false;
        }

        public void Set(in Vector2 v1, in Vector2 v2)
        {
            Vertex1 = v1;
            Vertex2 = v2;
            HasVertex0 = false;
            HasVertex3 = false;
        }

        /// <inheritdoc />
        public override Shape Clone()
        {
            var clone = new EdgeShape
            {
                Vertex1 = Vertex1,
                Vertex2 = Vertex2,
                HasVertex0 = HasVertex0,
                HasVertex3 = HasVertex3
            };
            return clone;
        }

        /// <inheritdoc />
        public override int GetChildCount()
        {
            return 1;
        }

        /// <inheritdoc />
        public override bool TestPoint(in Transform transform, in Vector2 point)
        {
            return false;
        }

        /// <inheritdoc />
        public override bool RayCast(
            out RayCastOutput output,
            in RayCastInput input,
            in Transform transform,
            int childIndex)
        {
            output = default;

            // Put the ray into the edge's frame of reference.
            var p1 = MathUtils.MulT(transform.Rotation, input.P1 - transform.Position);
            var p2 = MathUtils.MulT(transform.Rotation, input.P2 - transform.Position);
            var d = p2 - p1;

            var v1 = Vertex1;
            var v2 = Vertex2;
            var e = v2 - v1;
            var normal = new Vector2(e.Y, -e.X);
            normal = Vector2.Normalize(normal);

            // q = p1 + t * d
            // dot(normal, q - v1) = 0
            // dot(normal, p1 - v1) + t * dot(normal, d) = 0
            var numerator = Vector2.Dot(normal, v1 - p1);
            var denominator = Vector2.Dot(normal, d);

            if (denominator.Equals(0.0f))
            {
                return false;
            }

            var t = numerator / denominator;
            if (t < 0.0f || input.MaxFraction < t)
            {
                return false;
            }

            var q = p1 + t * d;

            // q = v1 + s * r
            // s = dot(q - v1, r) / dot(r, r)
            var r = v2 - v1;
            var rr = Vector2.Dot(r, r);
            if (rr.Equals(0.0f))
            {
                return false;
            }

            var s = Vector2.Dot(q - v1, r) / rr;
            if (s < 0.0f || 1.0f < s)
            {
                return false;
            }

            output = new RayCastOutput
            {
                Fraction = t,
                Normal = numerator > 0.0f
                             ? -MathUtils.Mul(transform.Rotation, normal)
                             : MathUtils.Mul(transform.Rotation, normal)
            };

            return true;
        }

        /// <inheritdoc />
        public override void ComputeAABB(out AABB aabb, in Transform xf, int childIndex)
        {
            var v1 = MathUtils.Mul(xf, Vertex1);
            var v2 = MathUtils.Mul(xf, Vertex2);

            var lower = Vector2.Min(v1, v2);
            var upper = Vector2.Max(v1, v2);

            var r = new Vector2(Radius, Radius);
            aabb = new AABB(lower - r, upper + r);
        }

        /// <inheritdoc />
        public override void ComputeMass(out MassData massData, float density)
        {
            massData = new MassData
            {
                Mass = 0,
                Center = 0.5f * (Vertex1 + Vertex2),
                RotationInertia = 0
            };
        }
    }
}