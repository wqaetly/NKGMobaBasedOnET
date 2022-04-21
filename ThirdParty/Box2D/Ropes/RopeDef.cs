using System.Numerics;

namespace Box2DSharp.Ropes
{
    /// 
    public struct RopeDef
    {
        public Vector2 Position;

        public Vector2[] Vertices;

        public int Count;

        public float[] Masses;

        public Vector2 Gravity;

        public RopeTuning Tuning;
    };
}