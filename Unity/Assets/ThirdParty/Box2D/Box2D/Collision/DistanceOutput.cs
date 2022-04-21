using System.Numerics;

namespace Box2DSharp.Collision
{
    /// Output for b2Distance.
    public struct DistanceOutput
    {
        /// closest point on shapeA
        public Vector2 PointA;

        /// closest point on shapeB
        public Vector2 PointB;

        public float Distance;

        /// number of GJK iterations used
        public int Iterations;
    }
}