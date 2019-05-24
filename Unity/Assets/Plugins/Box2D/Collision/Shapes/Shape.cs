using System.Numerics;
using Box2DSharp.Collision.Collider;
using Box2DSharp.Common;

namespace Box2DSharp.Collision.Shapes
{
    /// This holds the mass data computed for a shape.
    /// A shape is used for collision detection. You can create a shape however you like.
    /// Shapes used for simulation in b2World are created automatically when a b2Fixture
    /// is created. Shapes may encapsulate a one or more child shapes.
    public abstract class Shape
    {
        /// Radius of a shape. For polygonal shapes this must be b2_polygonRadius. There is no support for
        /// making rounded polygons.
        public float Radius { get; internal set; }

        public ShapeType ShapeType { get; internal set; }

        /// Clone the concrete shape using the provided allocator.
        public abstract Shape Clone();

        /// Get the number of child primitives.
        public abstract int GetChildCount();

        /// Test a point for containment in this shape. This only works for convex shapes.
        /// @param xf the shape world transform.
        /// @param p a point in world coordinates.
        public abstract bool TestPoint(in Transform transform, in Vector2 point);

        /// Cast a ray against a child shape.
        /// @param output the ray-cast results.
        /// @param input the ray-cast input parameters.
        /// @param transform the transform to be applied to the shape.
        /// @param childIndex the child shape index
        public abstract bool RayCast(
            out RayCastOutput output,
            in RayCastInput input,
            in Transform transform,
            int childIndex);

        /// Given a transform, compute the associated axis aligned bounding box for a child shape.
        /// @param aabb returns the axis aligned box.
        /// @param xf the world transform of the shape.
        /// @param childIndex the child shape
        public abstract void ComputeAABB(out AABB aabb, in Transform xf, int childIndex);

        /// Compute the mass properties of this shape using its dimensions and density.
        /// The inertia tensor is computed about the local origin.
        /// @param massData returns the mass data for this shape.
        /// @param density the density in kilograms per meter squared.
        public abstract void ComputeMass(out MassData massData, float density);
    }
}