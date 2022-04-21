using System;
using System.Diagnostics;
using System.Numerics;
using Box2DSharp.Collision.Shapes;
using Box2DSharp.Common;
using Box2DSharp.Dynamics;

namespace Box2DSharp.Collision
{
    /// Input parameters for b2TimeOfImpact
    public struct ToiInput
    {
        public DistanceProxy ProxyA;

        public DistanceProxy ProxyB;

        public Sweep SweepA;

        public Sweep SweepB;

        public float Tmax; // defines sweep interval [0, tMax]
    }

    /// Output parameters for b2TimeOfImpact.
    public struct ToiOutput
    {
        public enum ToiState
        {
            Unknown,

            Failed,

            Overlapped,

            Touching,

            Separated
        }

        public ToiState State;

        public float Time;
    }

    public class ToiProfile
    {
        public float ToiTime;

        public float ToiMaxTime;

        public int ToiCalls;

        public int ToiIters;

        public int ToiMaxIters;

        public int ToiRootIters;

        public int ToiMaxRootIters;

        public World World;
    }

    public static class TimeOfImpact
    {
        /// Compute the upper bound on time before two shapes penetrate. Time is represented as
        /// a fraction between [0,tMax]. This uses a swept separating axis and may miss some intermediate,
        /// non-tunneling collisions. If you change the time interval, you should call this function
        /// again.
        /// Note: use b2Distance to compute the contact point and normal at the time of impact.
        public static void ComputeTimeOfImpact(out ToiOutput output, in ToiInput input, ToiProfile toiProfile = null, GJkProfile gjkProfile = null)
        {
            var beginTime = toiProfile == null ? 0 : Stopwatch.GetTimestamp();
            output = new ToiOutput();

            if (toiProfile != null)
            {
                ++toiProfile.ToiCalls;
            }

            output.State = ToiOutput.ToiState.Unknown;
            output.Time = input.Tmax;

            ref readonly var proxyA = ref input.ProxyA;
            ref readonly var proxyB = ref input.ProxyB;

            var sweepA = input.SweepA;
            var sweepB = input.SweepB;

            // Large rotations can make the root finder fail, so we normalize the
            // sweep angles.
            sweepA.Normalize();
            sweepB.Normalize();

            var tMax = input.Tmax;

            var totalRadius = proxyA.Radius + proxyB.Radius;
            var target = Math.Max(Settings.LinearSlop, totalRadius - 3.0f * Settings.LinearSlop);
            var tolerance = 0.25f * Settings.LinearSlop;
            Debug.Assert(target > tolerance);

            var t1 = 0.0f;
            const int maxIterations = 20; // TODO_ERIN b2Settings
            var iter = 0;

            // Prepare input for distance query.
            var cache = new SimplexCache();
            var distanceInput = new DistanceInput
            {
                ProxyA = input.ProxyA,
                ProxyB = input.ProxyB,
                UseRadii = false
            };

            // The outer loop progressively attempts to compute new separating axes.
            // This loop terminates when an axis is repeated (no progress is made).
            for (;;)
            {
                sweepA.GetTransform(out var xfA, t1);
                sweepB.GetTransform(out var xfB, t1);

                // Get the distance between shapes. We can also use the results
                // to get a separating axis.
                distanceInput.TransformA = xfA;
                distanceInput.TransformB = xfB;

                DistanceAlgorithm.Distance(out var distanceOutput, ref cache, distanceInput, gjkProfile);

                // If the shapes are overlapped, we give up on continuous collision.
                if (distanceOutput.Distance <= 0.0f)
                {
                    // Failure!
                    output.State = ToiOutput.ToiState.Overlapped;
                    output.Time = 0.0f;
                    break;
                }

                if (distanceOutput.Distance < target + tolerance)
                {
                    // Victory!
                    output.State = ToiOutput.ToiState.Touching;
                    output.Time = t1;
                    break;
                }

                // Initialize the separating axis.
                var fcn = new SeparationFunction();
                fcn.Initialize(ref cache, proxyA, sweepA, proxyB, sweepB, t1);

                // Compute the TOI on the separating axis. We do this by successively
                // resolving the deepest point. This loop is bounded by the number of vertices.
                var done = false;
                var t2 = tMax;
                var pushBackIter = 0;
                for (;;)
                {
                    // Find the deepest point at t2. Store the witness point indices.

                    var s2 = fcn.FindMinSeparation(out var indexA, out var indexB, t2);

                    // Is the final configuration separated?
                    if (s2 > target + tolerance)
                    {
                        // Victory!
                        output.State = ToiOutput.ToiState.Separated;
                        output.Time = tMax;
                        done = true;
                        break;
                    }

                    // Has the separation reached tolerance?
                    if (s2 > target - tolerance)
                    {
                        // Advance the sweeps
                        t1 = t2;
                        break;
                    }

                    // Compute the initial separation of the witness points.
                    var s1 = fcn.Evaluate(indexA, indexB, t1);

                    // Check for initial overlap. This might happen if the root finder
                    // runs out of iterations.
                    if (s1 < target - tolerance)
                    {
                        output.State = ToiOutput.ToiState.Failed;
                        output.Time = t1;
                        done = true;
                        break;
                    }

                    // Check for touching
                    if (s1 <= target + tolerance)
                    {
                        // Victory! t1 should hold the TOI (could be 0.0).
                        output.State = ToiOutput.ToiState.Touching;
                        output.Time = t1;
                        done = true;
                        break;
                    }

                    // Compute 1D root of: f(x) - target = 0
                    var rootIterCount = 0;
                    float a1 = t1, a2 = t2;
                    for (;;)
                    {
                        // Use a mix of the secant rule and bisection.
                        float t;
                        if ((rootIterCount & 1) != 0)
                        {
                            // Secant rule to improve convergence.
                            t = a1 + (target - s1) * (a2 - a1) / (s2 - s1);
                        }
                        else
                        {
                            // Bisection to guarantee progress.
                            t = 0.5f * (a1 + a2);
                        }

                        ++rootIterCount;
                        if (toiProfile != null)
                        {
                            ++toiProfile.ToiRootIters;
                        }

                        var s = fcn.Evaluate(indexA, indexB, t);

                        if (Math.Abs(s - target) < tolerance)
                        {
                            // t2 holds a tentative value for t1
                            t2 = t;
                            break;
                        }

                        // Ensure we continue to bracket the root.
                        if (s > target)
                        {
                            a1 = t;
                            s1 = s;
                        }
                        else
                        {
                            a2 = t;
                            s2 = s;
                        }

                        if (rootIterCount == 50)
                        {
                            break;
                        }
                    }

                    if (toiProfile != null)
                    {
                        toiProfile.ToiMaxRootIters = Math.Max(toiProfile.ToiMaxRootIters, rootIterCount);
                    }

                    ++pushBackIter;

                    if (pushBackIter == Settings.MaxPolygonVertices)
                    {
                        break;
                    }
                }

                ++iter;
                if (toiProfile != null)
                {
                    ++toiProfile.ToiIters;
                }

                if (done)
                {
                    break;
                }

                if (iter == maxIterations)
                {
                    // Root finder got stuck. Semi-victory.
                    output.State = ToiOutput.ToiState.Failed;
                    output.Time = t1;
                    break;
                }
            }

            if (toiProfile == null)
            {
                return;
            }

            var endTime = Stopwatch.GetTimestamp();
            var time = (endTime - beginTime) / 10000f;
            toiProfile.ToiMaxIters = Math.Max(toiProfile.ToiMaxIters, iter);
            toiProfile.ToiMaxTime = Math.Max(toiProfile.ToiMaxTime, time);
            toiProfile.ToiTime += time;
        }
    }

    //
    public struct SeparationFunction
    {
        public enum FunctionType
        {
            Points,

            FaceA,

            FaceB
        }

        public Vector2 Axis;

        public Vector2 LocalPoint;

        public DistanceProxy ProxyA;

        public DistanceProxy ProxyB;

        public Sweep SweepA;

        public Sweep SweepB;

        public FunctionType Type;

        // TODO_ERIN might not need to return the separation

        public float Initialize(
            ref SimplexCache cache,
            DistanceProxy proxyA,
            in Sweep sweepA,
            DistanceProxy proxyB,
            in Sweep sweepB,
            float t1)
        {
            ProxyA = proxyA;
            ProxyB = proxyB;
            int count = cache.Count;
            Debug.Assert(0 < count && count < 3);
            var av0 = cache.IndexA.Value0;
            var av1 = cache.IndexA.Value1;
            var bv0 = cache.IndexB.Value0;
            var bv1 = cache.IndexB.Value1;
            SweepA = sweepA;
            SweepB = sweepB;

            SweepA.GetTransform(out var xfA, t1);
            SweepB.GetTransform(out var xfB, t1);

            if (count == 1)
            {
                Type = FunctionType.Points;
                var localPointA = ProxyA.GetVertex(av0);
                var localPointB = ProxyB.GetVertex(bv0);
                var pointA = MathUtils.Mul(xfA, localPointA);
                var pointB = MathUtils.Mul(xfB, localPointB);
                Axis = pointB - pointA;
                var s = Axis.Normalize();
                return s;
            }

            if (av0 == av1)
            {
                // Two points on B and one on A.
                Type = FunctionType.FaceB;
                var localPointB1 = proxyB.GetVertex(bv0);
                var localPointB2 = proxyB.GetVertex(bv1);

                Axis = MathUtils.Cross(localPointB2 - localPointB1, 1.0f);
                Axis.Normalize();
                var normal = MathUtils.Mul(xfB.Rotation, Axis);

                LocalPoint = 0.5f * (localPointB1 + localPointB2);
                var pointB = MathUtils.Mul(xfB, LocalPoint);

                var localPointA = proxyA.GetVertex(av0);
                var pointA = MathUtils.Mul(xfA, localPointA);

                var s = Vector2.Dot(pointA - pointB, normal);
                if (s < 0.0f)
                {
                    Axis = -Axis;
                    s = -s;
                }

                return s;
            }
            else
            {
                // Two points on A and one or two points on B.
                Type = FunctionType.FaceA;
                var localPointA1 = ProxyA.GetVertex(av0);
                var localPointA2 = ProxyA.GetVertex(av1);

                Axis = MathUtils.Cross(localPointA2 - localPointA1, 1.0f);
                Axis.Normalize();
                var normal = MathUtils.Mul(xfA.Rotation, Axis);

                LocalPoint = 0.5f * (localPointA1 + localPointA2);
                var pointA = MathUtils.Mul(xfA, LocalPoint);

                var localPointB = ProxyB.GetVertex(bv0);
                var pointB = MathUtils.Mul(xfB, localPointB);

                var s = Vector2.Dot(pointB - pointA, normal);
                if (s < 0.0f)
                {
                    Axis = -Axis;
                    s = -s;
                }

                return s;
            }
        }

        //
        public float FindMinSeparation(out int indexA, out int indexB, float t)
        {
            SweepA.GetTransform(out var xfA, t);
            SweepB.GetTransform(out var xfB, t);

            switch (Type)
            {
            case FunctionType.Points:
            {
                var axisA = MathUtils.MulT(xfA.Rotation, Axis);
                var axisB = MathUtils.MulT(xfB.Rotation, -Axis);

                indexA = ProxyA.GetSupport(axisA);
                indexB = ProxyB.GetSupport(axisB);

                var localPointA = ProxyA.GetVertex(indexA);
                var localPointB = ProxyB.GetVertex(indexB);

                var pointA = MathUtils.Mul(xfA, localPointA);
                var pointB = MathUtils.Mul(xfB, localPointB);

                var separation = Vector2.Dot(pointB - pointA, Axis);
                return separation;
            }

            case FunctionType.FaceA:
            {
                var normal = MathUtils.Mul(xfA.Rotation, Axis);
                var pointA = MathUtils.Mul(xfA, LocalPoint);

                var axisB = MathUtils.MulT(xfB.Rotation, -normal);

                indexA = -1;
                indexB = ProxyB.GetSupport(axisB);

                var localPointB = ProxyB.GetVertex(indexB);
                var pointB = MathUtils.Mul(xfB, localPointB);

                var separation = Vector2.Dot(pointB - pointA, normal);
                return separation;
            }

            case FunctionType.FaceB:
            {
                var normal = MathUtils.Mul(xfB.Rotation, Axis);
                var pointB = MathUtils.Mul(xfB, LocalPoint);

                var axisA = MathUtils.MulT(xfA.Rotation, -normal);

                indexB = -1;
                indexA = ProxyA.GetSupport(axisA);

                var localPointA = ProxyA.GetVertex(indexA);
                var pointA = MathUtils.Mul(xfA, localPointA);

                var separation = Vector2.Dot(pointA - pointB, normal);
                return separation;
            }

            default:
                Debug.Assert(false);
                indexA = -1;
                indexB = -1;
                return 0.0f;
            }
        }

        //
        public float Evaluate(int indexA, int indexB, float t)
        {
            SweepA.GetTransform(out var xfA, t);
            SweepB.GetTransform(out var xfB, t);

            switch (Type)
            {
            case FunctionType.Points:
            {
                var localPointA = ProxyA.GetVertex(indexA);
                var localPointB = ProxyB.GetVertex(indexB);

                var pointA = MathUtils.Mul(xfA, localPointA);
                var pointB = MathUtils.Mul(xfB, localPointB);
                var separation = Vector2.Dot(pointB - pointA, Axis);

                return separation;
            }

            case FunctionType.FaceA:
            {
                var normal = MathUtils.Mul(xfA.Rotation, Axis);
                var pointA = MathUtils.Mul(xfA, LocalPoint);

                var localPointB = ProxyB.GetVertex(indexB);
                var pointB = MathUtils.Mul(xfB, localPointB);

                var separation = Vector2.Dot(pointB - pointA, normal);
                return separation;
            }

            case FunctionType.FaceB:
            {
                var normal = MathUtils.Mul(xfB.Rotation, Axis);
                var pointB = MathUtils.Mul(xfB, LocalPoint);

                var localPointA = ProxyA.GetVertex(indexA);
                var pointA = MathUtils.Mul(xfA, localPointA);

                var separation = Vector2.Dot(pointA - pointB, normal);
                return separation;
            }

            default:
                Debug.Assert(false);
                return 0.0f;
            }
        }
    }
}