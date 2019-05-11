using System;
using System.Buffers;
using System.Collections;
using System.Diagnostics;
using System.Numerics;
using Box2DSharp.Collision.Collider;
using Box2DSharp.Common;
using Box2DSharp.Dynamics.Contacts;
using Box2DSharp.Dynamics.Joints;

namespace Box2DSharp.Dynamics
{
    /// This is an internal class.
    public ref struct Island
    {
        internal Body[] Bodies;

        internal int BodyCount;

        internal int ContactCount;

        internal IContactListener ContactListener;

        internal Contact[] Contacts;

        internal int JointCount;

        internal Joint[] Joints;

        internal Position[] Positions;

        internal Velocity[] Velocities;

        public Island(
            int bodyCapacity,
            int contactCapacity,
            int jointCapacity,
            IContactListener contactListener)
        {
            BodyCount = 0;
            ContactCount = 0;
            JointCount = 0;

            ContactListener = contactListener;

            Bodies = ArrayPool<Body>.Shared.Rent(bodyCapacity);
            Contacts = ArrayPool<Contact>.Shared.Rent(contactCapacity);
            Joints = ArrayPool<Joint>.Shared.Rent(jointCapacity);
            Positions = ArrayPool<Position>.Shared.Rent(bodyCapacity);
            Velocities = ArrayPool<Velocity>.Shared.Rent(bodyCapacity);
        }

        internal void Reset()
        {
            ArrayPool<Body>.Shared.Return(Bodies, true);
            Bodies = default;

            ArrayPool<Contact>.Shared.Return(Contacts, true);
            Contacts = default;

            ArrayPool<Joint>.Shared.Return(Joints, true);
            Joints = default;

            ArrayPool<Position>.Shared.Return(Positions, true);
            Positions = default;

            ArrayPool<Velocity>.Shared.Return(Velocities, true);
            Velocities = default;
        }

        internal void Clear()
        {
            BodyCount = 0;
            ContactCount = 0;
            JointCount = 0;
        }

        internal void Solve(out Profile profile, in TimeStep step, in Vector2 gravity, bool allowSleep)
        {
            profile = new Profile();
            var timer = SimpleObjectPool<Stopwatch>.Shared.Get();
            timer.Restart();

            var h = step.Dt;

            // Integrate velocities and apply damping. Initialize the body state.
            for (var i = 0; i < BodyCount; ++i)
            {
                var b = Bodies[i];

                var c = b.Sweep.C;
                var a = b.Sweep.A;
                var v = b.LinearVelocity;
                var w = b.AngularVelocity;

                // Store positions for continuous collision.
                b.Sweep.C0 = b.Sweep.C;
                b.Sweep.A0 = b.Sweep.A;

                if (b.BodyType == BodyType.DynamicBody)
                {
                    // Integrate velocities.
                    v += h * (b.GravityScale * gravity + b.InvMass * b.Force);
                    w += h * b.InverseInertia * b.Torque;

                    // Apply damping.
                    // ODE: dv/dt + c * v = 0
                    // Solution: v(t) = v0 * exp(-c * t)
                    // Time step: v(t + dt) = v0 * exp(-c * (t + dt)) = v0 * exp(-c * t) * exp(-c * dt) = v * exp(-c * dt)
                    // v2 = exp(-c * dt) * v1
                    // Pade approximation:
                    // v2 = v1 * 1 / (1 + c * dt)
                    v *= 1.0f / (1.0f + h * b.LinearDamping);
                    w *= 1.0f / (1.0f + h * b.AngularDamping);
                }

                Positions[i].Center = c;
                Positions[i].Angle = a;
                Velocities[i].V = v;
                Velocities[i].W = w;
            }

            timer.Restart();

            // Solver data
            var solverData = new SolverData(in step, Positions, Velocities);

            // Initialize velocity constraints.
            var contactSolverDef = new ContactSolverDef(in step, ContactCount, Contacts, Positions, Velocities);

            var contactSolver = new ContactSolver(in contactSolverDef);
            contactSolver.InitializeVelocityConstraints();

            if (step.WarmStarting)
            {
                contactSolver.WarmStart();
            }

            for (var i = 0; i < JointCount; ++i)
            {
                Joints[i].InitVelocityConstraints(in solverData);
            }

            profile.SolveInit = timer.ElapsedMilliseconds;

            // Solve velocity constraints
            timer.Restart();
            for (var i = 0; i < step.VelocityIterations; ++i)
            {
                for (var j = 0; j < JointCount; ++j)
                {
                    Joints[j].SolveVelocityConstraints(in solverData);
                }

                contactSolver.SolveVelocityConstraints();
            }

            // Store impulses for warm starting
            contactSolver.StoreImpulses();
            profile.SolveVelocity = timer.ElapsedMilliseconds;

            // Integrate positions
            for (var i = 0; i < BodyCount; ++i)
            {
                var c = Positions[i].Center;
                var a = Positions[i].Angle;
                var v = Velocities[i].V;
                var w = Velocities[i].W;

                // Check for large velocities
                var translation = h * v;
                if (Vector2.Dot(translation, translation) > Settings.MaxTranslationSquared)
                {
                    var ratio = Settings.MaxTranslation / translation.Length();
                    v *= ratio;
                }

                var rotation = h * w;
                if (rotation * rotation > Settings.MaxRotationSquared)
                {
                    var ratio = Settings.MaxRotation / Math.Abs(rotation);
                    w *= ratio;
                }

                // Integrate
                c += h * v;
                a += h * w;

                Positions[i].Center = c;
                Positions[i].Angle = a;
                Velocities[i].V = v;
                Velocities[i].W = w;
            }

            // Solve position constraints
            timer.Restart();
            var positionSolved = false;
            for (var i = 0; i < step.PositionIterations; ++i)
            {
                var contactsOkay = contactSolver.SolvePositionConstraints();

                var jointsOkay = true;
                for (var j = 0; j < JointCount; ++j)
                {
                    var jointOkay = Joints[j].SolvePositionConstraints(in solverData);
                    jointsOkay = jointsOkay && jointOkay;
                }

                if (contactsOkay && jointsOkay)
                {
                    // Exit early if the position errors are small.
                    positionSolved = true;
                    break;
                }
            }

            // Copy state buffers back to the bodies
            for (var i = 0; i < BodyCount; ++i)
            {
                var body = Bodies[i];
                body.Sweep.C = Positions[i].Center;
                body.Sweep.A = Positions[i].Angle;
                body.LinearVelocity = Velocities[i].V;
                body.AngularVelocity = Velocities[i].W;
                body.SynchronizeTransform();
            }

            profile.SolvePosition = timer.ElapsedMilliseconds;

            Report(contactSolver.VelocityConstraints);

            if (allowSleep)
            {
                var minSleepTime = Settings.MaxFloat;

                // 线速度最小值平方
                const float linTolSqr = Settings.LinearSleepTolerance * Settings.LinearSleepTolerance;

                // 角速度最小值平方
                const float angTolSqr = Settings.AngularSleepTolerance * Settings.AngularSleepTolerance;

                for (var i = 0; i < BodyCount; ++i)
                {
                    var b = Bodies[i];
                    if (b.BodyType == BodyType.StaticBody) // 静态物体没有休眠
                    {
                        continue;
                    }

                    if (!b.HasFlag(BodyFlags.AutoSleep)                              // 不允许休眠
                     || b.AngularVelocity * b.AngularVelocity > angTolSqr            // 或 角速度大于最小值
                     || Vector2.Dot(b.LinearVelocity, b.LinearVelocity) > linTolSqr) // 或 线速度大于最小值
                    {
                        b.SleepTime = 0.0f;
                        minSleepTime = 0.0f;
                    }
                    else
                    {
                        b.SleepTime += h;
                        minSleepTime = Math.Min(minSleepTime, b.SleepTime);
                    }
                }

                if (minSleepTime >= Settings.TimeToSleep && positionSolved)
                {
                    for (var i = 0; i < BodyCount; ++i)
                    {
                        var b = Bodies[i];
                        b.IsAwake = false;
                    }
                }
            }

            contactSolver.Reset();
            SimpleObjectPool<Stopwatch>.Shared.Return(timer);
        }

        internal void SolveTOI(in TimeStep subStep, int toiIndexA, int toiIndexB)
        {
            Debug.Assert(toiIndexA < BodyCount);
            Debug.Assert(toiIndexB < BodyCount);

            // Initialize the body state.
            for (var i = 0; i < BodyCount; ++i)
            {
                var b = Bodies[i];
                Positions[i].Center = b.Sweep.C;
                Positions[i].Angle = b.Sweep.A;
                Velocities[i].V = b.LinearVelocity;
                Velocities[i].W = b.AngularVelocity;
            }

            var contactSolverDef = new ContactSolverDef(in subStep, ContactCount, Contacts, Positions, Velocities);
            var contactSolver = new ContactSolver(contactSolverDef);

            // Solve position constraints.
            for (var i = 0; i < subStep.PositionIterations; ++i)
            {
                var contactsOkay = contactSolver.SolveTOIPositionConstraints(toiIndexA, toiIndexB);
                if (contactsOkay)
                {
                    break;
                }
            }

#if FALSE
// Is the new position really safe?
            for (int i = 0; i < m_contactCount; ++i)
            {
                var c = m_contacts[i];
                var fA = c.FixtureA;
                var fB = c.FixtureB;

                var bA = fA.GetBody();
                var bB = fB.GetBody();

                int indexA = c.GetChildIndexA();
                int indexB = c.GetChildIndexB();

                b2DistanceInput input = new b2DistanceInput();
                input.proxyA.Set(fA.Shape, indexA);
                input.proxyB.Set(fB.Shape, indexB);
                input.transformA = bA.GetTransform();
                input.transformB = bB.GetTransform();
                input.useRadii = false;

                b2DistanceOutput output = new b2DistanceOutput();
                SimplexCache     cache = new SimplexCache {count = 0};
                DistanceAlgorithm.b2Distance(ref output, ref cache, ref input);

                if (output.distance.Equals(0) || cache.count == 3)
                {
                    cache.count += 0;
                }
            }
#endif

            // Leap of faith to new safe state.
            Bodies[toiIndexA].Sweep.C0 = Positions[toiIndexA].Center;
            Bodies[toiIndexA].Sweep.A0 = Positions[toiIndexA].Angle;
            Bodies[toiIndexB].Sweep.C0 = Positions[toiIndexB].Center;
            Bodies[toiIndexB].Sweep.A0 = Positions[toiIndexB].Angle;

            // No warm starting is needed for TOI events because warm
            // starting impulses were applied in the discrete solver.
            contactSolver.InitializeVelocityConstraints();

            // Solve velocity constraints.
            for (var i = 0; i < subStep.VelocityIterations; ++i)
            {
                contactSolver.SolveVelocityConstraints();
            }

            // Don't store the TOI contact forces for warm starting
            // because they can be quite large.

            var h = subStep.Dt;

            // Integrate positions
            for (var i = 0; i < BodyCount; ++i)
            {
                var c = Positions[i].Center;
                var a = Positions[i].Angle;
                var v = Velocities[i].V;
                var w = Velocities[i].W;

                // Check for large velocities
                var translation = h * v;
                if (Vector2.Dot(translation, translation) > Settings.MaxTranslationSquared)
                {
                    var ratio = Settings.MaxTranslation / translation.Length();
                    v *= ratio;
                }

                var rotation = h * w;
                if (rotation * rotation > Settings.MaxRotationSquared)
                {
                    var ratio = Settings.MaxRotation / Math.Abs(rotation);
                    w *= ratio;
                }

                // Integrate
                c += h * v;
                a += h * w;

                Positions[i].Center = c;
                Positions[i].Angle = a;
                Velocities[i].V = v;
                Velocities[i].W = w;

                // Sync bodies
                var body = Bodies[i];
                body.Sweep.C = c;
                body.Sweep.A = a;
                body.LinearVelocity = v;
                body.AngularVelocity = w;
                body.SynchronizeTransform();
            }

            Report(contactSolver.VelocityConstraints);
            contactSolver.Reset();
        }

        internal void Add(Body body)
        {
            Debug.Assert(BodyCount < Bodies.Length);
            body.IslandIndex = BodyCount;
            Bodies[BodyCount] = body;
            ++BodyCount;
        }

        internal void Add(Contact contact)
        {
            Debug.Assert(ContactCount < Contacts.Length);
            Contacts[ContactCount++] = contact;
        }

        internal void Add(Joint joint)
        {
            Debug.Assert(JointCount < Joints.Length);
            Joints[JointCount++] = joint;
        }

        private void Report(ContactVelocityConstraint[] constraints)
        {
            if (ContactListener == null)
            {
                return;
            }

            for (var i = 0; i < ContactCount; ++i)
            {
                var c = Contacts[i];

                var vc = constraints[i];

                var impulse = new ContactImpulse {Count = vc.PointCount};
                unsafe
                {
                    for (var j = 0; j < vc.PointCount; ++j)
                    {
                        impulse.NormalImpulses.Values[j] = vc.Points[j].NormalImpulse;
                        impulse.TangentImpulses.Values[j] = vc.Points[j].TangentImpulse;
                    }
                }

                ContactListener.PostSolve(c, impulse);
            }
        }
    }
}