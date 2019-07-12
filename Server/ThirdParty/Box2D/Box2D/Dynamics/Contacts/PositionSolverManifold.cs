using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Numerics;
using System.Runtime.InteropServices;
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
                var pointA = MathUtils.Mul(xfA, pc.LocalPoint);
                var pointB = MathUtils.Mul(xfB, pc.LocalPoints.Value0);
                Normal = pointB - pointA;
                Normal.Normalize();
                Point = 0.5f * (pointA + pointB);
                Separation = Vector2.Dot(pointB - pointA, Normal) - pc.RadiusA - pc.RadiusB;
            }
                break;

            case ManifoldType.FaceA:
            {
                Normal = MathUtils.Mul(xfA.Rotation, pc.LocalNormal);
                var planePoint = MathUtils.Mul(xfA, pc.LocalPoint);

                var clipPoint = MathUtils.Mul(xfB, pc.LocalPoints[index]);
                Separation = Vector2.Dot(clipPoint - planePoint, Normal) - pc.RadiusA - pc.RadiusB;
                Point = clipPoint;
            }
                break;

            case ManifoldType.FaceB:
            {
                Normal = MathUtils.Mul(xfB.Rotation, pc.LocalNormal);
                var planePoint = MathUtils.Mul(xfB, pc.LocalPoint);

                var clipPoint = MathUtils.Mul(xfA, pc.LocalPoints[index]);
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