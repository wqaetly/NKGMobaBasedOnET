using System;
using System.Diagnostics;
using System.Numerics;
using Box2DSharp.Common;

namespace Box2DSharp.Collision
{
    public struct Simplex
    {
        public FixedArray3<SimplexVertex> Vertices;

        public int Count;

        public void ReadCache(
            ref SimplexCache cache,
            in DistanceProxy proxyA,
            in Transform transformA,
            in DistanceProxy proxyB,
            in Transform transformB)
        {
            Debug.Assert(cache.Count <= 3);

            // Copy data from cache.
            Count = cache.Count;

            //ref b2SimplexVertex vertices = ref m_v1;
            for (var i = 0; i < Count; ++i)
            {
                ref var v = ref Vertices[i];
                v.IndexA = cache.IndexA[i];
                v.IndexB = cache.IndexB[i];
                var wALocal = proxyA.GetVertex(v.IndexA);
                var wBLocal = proxyB.GetVertex(v.IndexB);
                v.Wa = MathUtils.Mul(transformA, wALocal);
                v.Wb = MathUtils.Mul(transformB, wBLocal);
                v.W = v.Wb - v.Wa;
                v.A = 0.0f;
            }

            // Compute the new simplex metric, if it is substantially different than
            // old metric then flush the simplex.
            if (Count > 1)
            {
                var metric1 = cache.Metric;
                var metric2 = GetMetric();
                if (metric2 < 0.5f * metric1 || 2.0f * metric1 < metric2 || metric2 < Settings.Epsilon)
                {
                    // Reset the simplex.
                    Count = 0;
                }
            }

            // If the cache is empty or invalid ...
            if (Count == 0)
            {
                ref var v = ref Vertices[0];
                v.IndexA = 0;
                v.IndexB = 0;
                var wALocal = proxyA.GetVertex(0);
                var wBLocal = proxyB.GetVertex(0);
                v.Wa = MathUtils.Mul(transformA, wALocal);
                v.Wb = MathUtils.Mul(transformB, wBLocal);
                v.W = v.Wb - v.Wa;
                v.A = 1.0f;
                Count = 1;
            }
        }

        public void WriteCache(ref SimplexCache cache)
        {
            cache.Metric = GetMetric();
            cache.Count = (ushort) Count;
            for (var i = 0; i < Count; ++i)
            {
                cache.IndexA[i] = (byte) Vertices[i].IndexA;
                cache.IndexB[i] = (byte) Vertices[i].IndexB;
            }
        }

        public Vector2 GetSearchDirection()
        {
            switch (Count)
            {
            case 1:
                return -Vertices.Value0.W;

            case 2:
            {
                var e12 = Vertices.Value1.W - Vertices.Value0.W;
                var sgn = MathUtils.Cross(e12, -Vertices.Value0.W);
                if (sgn > 0.0f)
                {
                    // Origin is left of e12.
                    return MathUtils.Cross(1.0f, e12);
                }

                // Origin is right of e12.
                return MathUtils.Cross(e12, 1.0f);
            }

            default:
                throw new ArgumentOutOfRangeException(nameof(Count));
            }
        }

        public Vector2 GetClosestPoint()
        {
            switch (Count)
            {
            case 1:
                return Vertices.Value0.W;

            case 2:
                return Vertices.Value0.A * Vertices.Value0.W + Vertices.Value1.A * Vertices.Value1.W;

            case 3:
                return Vector2.Zero;

            default:
                throw new ArgumentOutOfRangeException(nameof(Count));
            }
        }

        public void GetWitnessPoints(out Vector2 pA, out Vector2 pB)
        {
            switch (Count)
            {
            case 1:
                pA = Vertices.Value0.Wa;
                pB = Vertices.Value0.Wb;
                break;

            case 2:
                pA = Vertices.Value0.A * Vertices.Value0.Wa + Vertices.Value1.A * Vertices.Value1.Wa;
                pB = Vertices.Value0.A * Vertices.Value0.Wb + Vertices.Value1.A * Vertices.Value1.Wb;
                break;

            case 3:
                pA = Vertices.Value0.A * Vertices.Value0.Wa
                   + Vertices.Value1.A * Vertices.Value1.Wa
                   + Vertices.Value2.A * Vertices.Value2.Wa;
                pB = pA;
                break;

            default:
                throw new ArgumentOutOfRangeException(nameof(Count));
            }
        }

        public float GetMetric()
        {
            switch (Count)
            {
            case 0:
                Debug.Assert(false);
                return 0.0f;

            case 1:
                return 0.0f;

            case 2:
                return Vector2.Distance(Vertices.Value0.W, Vertices.Value1.W);

            case 3:
                return MathUtils.Cross(Vertices.Value1.W - Vertices.Value0.W, Vertices.Value2.W - Vertices.Value0.W);

            default:
                throw new ArgumentOutOfRangeException(nameof(Count));
            }
        }

        // Solve a line segment using barycentric coordinates.
        //
        // p = a1 * w1 + a2 * w2
        // a1 + a2 = 1
        //
        // The vector from the origin to the closest point on the line is
        // perpendicular to the line.
        // e12 = w2 - w1
        // dot(p, e) = 0
        // a1 * dot(w1, e) + a2 * dot(w2, e) = 0
        //
        // 2-by-2 linear system
        // [1      1     ][a1] = [1]
        // [w1.e12 w2.e12][a2] = [0]
        //
        // Define
        // d12_1 =  dot(w2, e12)
        // d12_2 = -dot(w1, e12)
        // d12 = d12_1 + d12_2
        //
        // Solution
        // a1 = d12_1 / d12
        // a2 = d12_2 / d12
        public void Solve2()
        {
            ref var v0 = ref Vertices.Value0;
            ref var v1 = ref Vertices.Value1;
            var w1 = v0.W;
            var w2 = v1.W;
            var e12 = w2 - w1;

            // w1 region
            var d12_2 = -Vector2.Dot(w1, e12);
            if (d12_2 <= 0.0f)
            {
                // a2 <= 0, so we clamp it to 0
                v0.A = 1.0f;
                Count = 1;
                return;
            }

            // w2 region
            var d12_1 = Vector2.Dot(w2, e12);
            if (d12_1 <= 0.0f)
            {
                // a1 <= 0, so we clamp it to 0
                v1.A = 1.0f;
                Count = 1;
                Vertices.Value0 = Vertices.Value1;
                return;
            }

            // Must be in e12 region.
            var inv_d12 = 1.0f / (d12_1 + d12_2);
            v0.A = d12_1 * inv_d12;
            v1.A = d12_2 * inv_d12;
            Count = 2;
        }

        // Possible regions:
        // - points[2]
        // - edge points[0]-points[2]
        // - edge points[1]-points[2]
        // - inside the triangle
        public void Solve3()
        {
            ref var v0 = ref Vertices.Value0;
            ref var v1 = ref Vertices.Value1;
            ref var v2 = ref Vertices.Value2;
            var w1 = v0.W;
            var w2 = v1.W;
            var w3 = v2.W;

            // Edge12
            // [1      1     ][a1] = [1]
            // [w1.e12 w2.e12][a2] = [0]
            // a3 = 0
            var e12 = w2 - w1;
            var w1e12 = Vector2.Dot(w1, e12);
            var w2e12 = Vector2.Dot(w2, e12);
            var d12_1 = w2e12;
            var d12_2 = -w1e12;

            // Edge13
            // [1      1     ][a1] = [1]
            // [w1.e13 w3.e13][a3] = [0]
            // a2 = 0
            var e13 = w3 - w1;
            var w1e13 = Vector2.Dot(w1, e13);
            var w3e13 = Vector2.Dot(w3, e13);
            var d13_1 = w3e13;
            var d13_2 = -w1e13;

            // Edge23
            // [1      1     ][a2] = [1]
            // [w2.e23 w3.e23][a3] = [0]
            // a1 = 0
            var e23 = w3 - w2;
            var w2e23 = Vector2.Dot(w2, e23);
            var w3e23 = Vector2.Dot(w3, e23);
            var d23_1 = w3e23;
            var d23_2 = -w2e23;

            // Triangle123
            var n123 = MathUtils.Cross(e12, e13);

            var d123_1 = n123 * MathUtils.Cross(w2, w3);
            var d123_2 = n123 * MathUtils.Cross(w3, w1);
            var d123_3 = n123 * MathUtils.Cross(w1, w2);

            // w1 region
            if (d12_2 <= 0.0f && d13_2 <= 0.0f)
            {
                v0.A = 1.0f;
                Count = 1;
                return;
            }

            // e12
            if (d12_1 > 0.0f && d12_2 > 0.0f && d123_3 <= 0.0f)
            {
                var inv_d12 = 1.0f / (d12_1 + d12_2);
                v0.A = d12_1 * inv_d12;
                v1.A = d12_2 * inv_d12;
                Count = 2;
                return;
            }

            // e13
            if (d13_1 > 0.0f && d13_2 > 0.0f && d123_2 <= 0.0f)
            {
                var inv_d13 = 1.0f / (d13_1 + d13_2);
                v0.A = d13_1 * inv_d13;
                v2.A = d13_2 * inv_d13;
                Count = 2;
                v1 = v2;
                return;
            }

            // w2 region
            if (d12_1 <= 0.0f && d23_2 <= 0.0f)
            {
                v1.A = 1.0f;
                Count = 1;
                v0 = v1;
                return;
            }

            // w3 region
            if (d13_1 <= 0.0f && d23_1 <= 0.0f)
            {
                v2.A = 1.0f;
                Count = 1;
                v0 = v2;
                return;
            }

            // e23
            if (d23_1 > 0.0f && d23_2 > 0.0f && d123_1 <= 0.0f)
            {
                var inv_d23 = 1.0f / (d23_1 + d23_2);
                v1.A = d23_1 * inv_d23;
                v2.A = d23_2 * inv_d23;
                Count = 2;
                v0 = v2;
                return;
            }

            // Must be in triangle123
            var inv_d123 = 1.0f / (d123_1 + d123_2 + d123_3);
            v0.A = d123_1 * inv_d123;
            v1.A = d123_2 * inv_d123;
            v2.A = d123_3 * inv_d123;
            Count = 3;
        }
    }
}