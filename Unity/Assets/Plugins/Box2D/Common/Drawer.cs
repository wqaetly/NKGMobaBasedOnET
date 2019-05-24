using System.Drawing;
using System.Numerics;

namespace Box2DSharp.Common
{
    public interface IDrawer
    {
        DrawFlag Flags { get; set; }

        /// Draw a closed polygon provided in CCW order.
        void DrawPolygon(Vector2[] vertices, int vertexCount, in Color color);

        /// Draw a solid closed polygon provided in CCW order.
        void DrawSolidPolygon(Vector2[] vertices, int vertexCount, in Color color);

        /// Draw a circle.
        void DrawCircle(in Vector2 center, float radius, in Color color);

        /// Draw a solid circle.
        void DrawSolidCircle(in Vector2 center, float radius, in Vector2 axis, in Color color);

        /// Draw a line segment.
        void DrawSegment(in Vector2 p1, in Vector2 p2, in Color color);

        /// Draw a transform. Choose your own length scale.
        /// @param xf a transform.
        void DrawTransform(in Transform xf);

        /// Draw a point.
        void DrawPoint(in Vector2 p, float size, in Color color);
    }
}