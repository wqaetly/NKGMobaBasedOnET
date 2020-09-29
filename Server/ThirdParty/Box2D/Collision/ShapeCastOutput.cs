using System.Numerics;

namespace Box2DSharp.Collision
{
    /// Output results for b2ShapeCast
    public struct ShapeCastOutput
    {
        public Vector2 Point;

        public Vector2 Normal;

        public float Lambda;

        public int Iterations;
    }
}