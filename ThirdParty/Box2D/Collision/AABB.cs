using System;
using System.Diagnostics.Contracts;
using System.Numerics;
using System.Runtime.CompilerServices;
using Box2DSharp.Collision.Collider;
using Box2DSharp.Common;

namespace Box2DSharp.Collision
{
    /// <summary>
    ///     An axis aligned bounding box.
    /// </summary>
    public struct AABB
    {
        /// <summary>
        ///     the lower vertex
        /// </summary>
        public Vector2 LowerBound;

        /// <summary>
        ///     the upper vertex
        /// </summary>
        public Vector2 UpperBound;

        public AABB(in Vector2 lowerBound, in Vector2 upperBound)
        {
            LowerBound = lowerBound;
            UpperBound = upperBound;
        }

        /// <summary>
        ///     Verify that the bounds are sorted.
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [Pure]
        public bool IsValid()
        {
            var d = UpperBound - LowerBound;
            var valid = d.X >= 0.0f && d.Y >= 0.0f;
            valid = valid && LowerBound.IsValid() && UpperBound.IsValid();
            return valid;
        }

        /// <summary>
        ///     Get the center of the AABB.
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [Pure]
        public Vector2 GetCenter()
        {
            return 0.5f * (LowerBound + UpperBound);
        }

        /// <summary>
        ///     Get the extents of the AABB (half-widths).
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [Pure]
        public Vector2 GetExtents()
        {
            return 0.5f * (UpperBound - LowerBound);
        }

        /// <summary>
        ///     Get the perimeter length
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [Pure]
        public float GetPerimeter()
        {
            var wx = UpperBound.X - LowerBound.X;
            var wy = UpperBound.Y - LowerBound.Y;
            return wx + wx + wy + wy;
        }

        public bool RayCast(out RayCastOutput output, in RayCastInput input)
        {
            output = default;
            var tmin = -Settings.MaxFloat;
            var tmax = Settings.MaxFloat;

            var p = input.P1;
            var d = input.P2 - input.P1;
            var absD = Vector2.Abs(d);

            var normal = new Vector2();

            {
                if (absD.X < Settings.Epsilon)
                {
                    // Parallel.
                    if (p.X < LowerBound.X || UpperBound.X < p.X)
                    {
                        return false;
                    }
                }
                else
                {
                    var invD = 1.0f / d.X;
                    var t1 = (LowerBound.X - p.X) * invD;
                    var t2 = (UpperBound.X - p.X) * invD;

                    // Sign of the normal vector.
                    var s = -1.0f;

                    if (t1 > t2)
                    {
                        MathUtils.Swap(ref t1, ref t2);
                        s = 1.0f;
                    }

                    // Push the min up
                    if (t1 > tmin)
                    {
                        normal.SetZero();
                        normal.X = s;
                        tmin = t1;
                    }

                    // Pull the max down
                    tmax = Math.Min(tmax, t2);

                    if (tmin > tmax)
                    {
                        return false;
                    }
                }
            }
            {
                if (absD.Y < Settings.Epsilon)
                {
                    // Parallel.
                    if (p.Y < LowerBound.Y || UpperBound.Y < p.Y)
                    {
                        return false;
                    }
                }
                else
                {
                    var invD = 1.0f / d.Y;
                    var t1 = (LowerBound.Y - p.Y) * invD;
                    var t2 = (UpperBound.Y - p.Y) * invD;

                    // Sign of the normal vector.
                    var s = -1.0f;

                    if (t1 > t2)
                    {
                        MathUtils.Swap(ref t1, ref t2);
                        s = 1.0f;
                    }

                    // Push the min up
                    if (t1 > tmin)
                    {
                        normal.SetZero();
                        normal.Y = s;
                        tmin = t1;
                    }

                    // Pull the max down
                    tmax = Math.Min(tmax, t2);

                    if (tmin > tmax)
                    {
                        return false;
                    }
                }
            }

            // Does the ray start inside the box?
            // Does the ray intersect beyond the max fraction?
            if (tmin < 0.0f || input.MaxFraction < tmin)
            {
                return false;
            }

            // Intersection.
            output = new RayCastOutput {Fraction = tmin, Normal = normal};

            return true;
        }

        public static void Combine(in AABB left, in AABB right, out AABB aabb)
        {
            aabb = new AABB(
                Vector2.Min(left.LowerBound, right.LowerBound),
                Vector2.Max(left.UpperBound, right.UpperBound));
        }

        /// <summary>
        ///     Combine an AABB into this one.
        /// </summary>
        /// <param name="aabb"></param>
        public void Combine(in AABB aabb)
        {
            LowerBound = Vector2.Min(LowerBound, aabb.LowerBound);
            UpperBound = Vector2.Max(UpperBound, aabb.UpperBound);
        }

        /// <summary>
        ///     Combine two AABBs into this one.
        /// </summary>
        /// <param name="aabb1"></param>
        /// <param name="aabb2"></param>
        public void Combine(in AABB aabb1, in AABB aabb2)
        {
            LowerBound = Vector2.Min(aabb1.LowerBound, aabb2.LowerBound);
            UpperBound = Vector2.Max(aabb1.UpperBound, aabb2.UpperBound);
        }

        /// <summary>
        ///     Does this aabb contain the provided AABB.
        /// </summary>
        /// <param name="aabb">the provided AABB</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [Pure]
        public bool Contains(in AABB aabb)
        {
            return LowerBound.X <= aabb.LowerBound.X
                && LowerBound.Y <= aabb.LowerBound.Y
                && aabb.UpperBound.X <= UpperBound.X
                && aabb.UpperBound.Y <= UpperBound.Y;
        }
    }
}