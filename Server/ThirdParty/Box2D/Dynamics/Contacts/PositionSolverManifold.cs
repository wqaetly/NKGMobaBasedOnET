using System.ComponentModel;
using System.Diagnostics;
using System.Numerics;
using Box2DSharp.Collision.Collider;
using Box2DSharp.Common;

namespace Box2DSharp.Dynamics.Contacts
{
    public struct PositionSolverManifold
    {
        public Vector2 Normal;

        public Vector2 Point;

        public float Separation;

        public void Initialize(in ContactPositionConstraint pc, in Transform xfA, in Transform xfB, int index)
        {
            Debug.Assert(pc.PointCount > 0);

            switch (pc.Type)
            {
            case ManifoldType.Circles:
            {
                //var pointA = MathUtils.Mul(xfA, pc.LocalPoint); // inline
                var x = xfA.Rotation.Cos * pc.LocalPoint.X - xfA.Rotation.Sin * pc.LocalPoint.Y + xfA.Position.X;
                var y = xfA.Rotation.Sin * pc.LocalPoint.X + xfA.Rotation.Cos * pc.LocalPoint.Y + xfA.Position.Y;
                var pointA = new Vector2(x, y);

                // var pointB = MathUtils.Mul(xfB, pc.LocalPoints.Value0); // inline
                x = xfB.Rotation.Cos * pc.LocalPoints.Value0.X - xfB.Rotation.Sin * pc.LocalPoints.Value0.Y + xfB.Position.X;
                y = xfB.Rotation.Sin * pc.LocalPoints.Value0.X + xfB.Rotation.Cos * pc.LocalPoints.Value0.Y + xfB.Position.Y;
                var pointB = new Vector2(x, y);

                Normal = pointB - pointA;
                Normal.Normalize();
                Point = 0.5f * (pointA + pointB);
                Separation = Vector2.Dot(pointB - pointA, Normal) - pc.RadiusA - pc.RadiusB;
            }
                break;

            case ManifoldType.FaceA:
            {
                // Normal = MathUtils.Mul(xfA.Rotation, pc.LocalNormal); // inline
                Normal = new Vector2(
                    xfA.Rotation.Cos * pc.LocalNormal.X - xfA.Rotation.Sin * pc.LocalNormal.Y,
                    xfA.Rotation.Sin * pc.LocalNormal.X + xfA.Rotation.Cos * pc.LocalNormal.Y);

                // var planePoint = MathUtils.Mul(xfA, pc.LocalPoint); // inline
                var x = xfA.Rotation.Cos * pc.LocalPoint.X - xfA.Rotation.Sin * pc.LocalPoint.Y + xfA.Position.X;
                var y = xfA.Rotation.Sin * pc.LocalPoint.X + xfA.Rotation.Cos * pc.LocalPoint.Y + xfA.Position.Y;
                var planePoint = new Vector2(x, y);

                // var clipPoint = MathUtils.Mul(xfB, pc.LocalPoints[index]); // inline

                if (index == 0)
                {
                    x = xfB.Rotation.Cos * pc.LocalPoints.Value0.X - xfB.Rotation.Sin * pc.LocalPoints.Value0.Y + xfB.Position.X;
                    y = xfB.Rotation.Sin * pc.LocalPoints.Value0.X + xfB.Rotation.Cos * pc.LocalPoints.Value0.Y + xfB.Position.Y;
                }
                else
                {
                    x = xfB.Rotation.Cos * pc.LocalPoints.Value1.X - xfB.Rotation.Sin * pc.LocalPoints.Value1.Y + xfB.Position.X;
                    y = xfB.Rotation.Sin * pc.LocalPoints.Value1.X + xfB.Rotation.Cos * pc.LocalPoints.Value1.Y + xfB.Position.Y;
                }

                var clipPoint = new Vector2(x, y);

                Separation = Vector2.Dot(clipPoint - planePoint, Normal) - pc.RadiusA - pc.RadiusB;
                Point = clipPoint;
            }
                break;

            case ManifoldType.FaceB:
            {
                // Normal = MathUtils.Mul(xfB.Rotation, pc.LocalNormal); // inline
                Normal = new Vector2(
                    xfB.Rotation.Cos * pc.LocalNormal.X - xfB.Rotation.Sin * pc.LocalNormal.Y,
                    xfB.Rotation.Sin * pc.LocalNormal.X + xfB.Rotation.Cos * pc.LocalNormal.Y);

                // var planePoint = MathUtils.Mul(xfB, pc.LocalPoint); // inline
                var x = xfB.Rotation.Cos * pc.LocalPoint.X - xfB.Rotation.Sin * pc.LocalPoint.Y + xfB.Position.X;
                var y = xfB.Rotation.Sin * pc.LocalPoint.X + xfB.Rotation.Cos * pc.LocalPoint.Y + xfB.Position.Y;
                var planePoint = new Vector2(x, y);

                // var clipPoint = MathUtils.Mul(xfA, pc.LocalPoints[index]); // inline
                if (index == 0)
                {
                    x = xfA.Rotation.Cos * pc.LocalPoints.Value0.X - xfA.Rotation.Sin * pc.LocalPoints.Value0.Y + xfA.Position.X;
                    y = xfA.Rotation.Sin * pc.LocalPoints.Value0.X + xfA.Rotation.Cos * pc.LocalPoints.Value0.Y + xfA.Position.Y;
                }
                else
                {
                    x = xfA.Rotation.Cos * pc.LocalPoints.Value1.X - xfA.Rotation.Sin * pc.LocalPoints.Value1.Y + xfA.Position.X;
                    y = xfA.Rotation.Sin * pc.LocalPoints.Value1.X + xfA.Rotation.Cos * pc.LocalPoints.Value1.Y + xfA.Position.Y;
                }

                var clipPoint = new Vector2(x, y);

                Separation = Vector2.Dot(clipPoint - planePoint, Normal) - pc.RadiusA - pc.RadiusB;
                Point = clipPoint;

                // Ensure normal points from A to B
                Normal = -Normal;
            }
                break;
            default:
                throw new InvalidEnumArgumentException($"Invalid ManifoldType: {pc.Type}");
            }
        }
    }
}