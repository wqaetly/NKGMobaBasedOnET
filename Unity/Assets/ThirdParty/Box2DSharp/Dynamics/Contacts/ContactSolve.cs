using System;
using System.Buffers;
using System.Diagnostics;
using System.Numerics;
using Box2DSharp.Collision.Collider;
using Box2DSharp.Common;

namespace Box2DSharp.Dynamics.Contacts
{
    public ref struct ContactSolver
    {
        private Contact[] _contacts;

        private int _contactCount;

        private ContactPositionConstraint[] _positionConstraints;

        internal ContactVelocityConstraint[] VelocityConstraints;

        private Position[] _positions;

        private readonly Velocity[] _velocities;

        public unsafe ContactSolver(in ContactSolverDef def)
        {
            var step = def.Step;
            _contactCount = def.ContactCount;
            _positionConstraints = ArrayPool<ContactPositionConstraint>.Shared.Rent(_contactCount);
            VelocityConstraints = ArrayPool<ContactVelocityConstraint>.Shared.Rent(_contactCount);

            _positions = def.Positions;
            _velocities = def.Velocities;
            _contacts = def.Contacts;

            // Initialize position independent portions of the constraints.
            for (var i = 0; i < _contactCount; ++i)
            {
                var contact = _contacts[i];

                var fixtureA = contact.FixtureA;
                var fixtureB = contact.FixtureB;
                var shapeA = fixtureA.Shape;
                var shapeB = fixtureB.Shape;
                var radiusA = shapeA.Radius;
                var radiusB = shapeB.Radius;
                var bodyA = fixtureA.Body;
                var bodyB = fixtureB.Body;
                ref readonly var manifold = ref contact.Manifold;

                var pointCount = manifold.PointCount;
                Debug.Assert(pointCount > 0);

                ref var vc = ref VelocityConstraints[i];
                vc.Friction = contact.Friction;
                vc.Restitution = contact.Restitution;
                vc.TangentSpeed = contact.TangentSpeed;
                vc.IndexA = bodyA.IslandIndex;
                vc.IndexB = bodyB.IslandIndex;
                vc.InvMassA = bodyA.InvMass;
                vc.InvMassB = bodyB.InvMass;
                vc.InvIa = bodyA.InverseInertia;
                vc.InvIb = bodyB.InverseInertia;
                vc.ContactIndex = i;
                vc.PointCount = pointCount;
                vc.K.SetZero();
                vc.NormalMass.SetZero();

                ref var pc = ref _positionConstraints[i];
                pc.IndexA = bodyA.IslandIndex;
                pc.IndexB = bodyB.IslandIndex;
                pc.InvMassA = bodyA.InvMass;
                pc.InvMassB = bodyB.InvMass;
                pc.LocalCenterA = bodyA.Sweep.LocalCenter;
                pc.LocalCenterB = bodyB.Sweep.LocalCenter;
                pc.InvIa = bodyA.InverseInertia;
                pc.InvIb = bodyB.InverseInertia;
                pc.LocalNormal = manifold.LocalNormal;
                pc.LocalPoint = manifold.LocalPoint;
                pc.PointCount = pointCount;
                pc.RadiusA = radiusA;
                pc.RadiusB = radiusB;
                pc.Type = manifold.Type;

                for (var j = 0; j < pointCount; ++j)
                {
                    var cp = manifold.Points.Values[j];
                    ref var vcp = ref vc.Points.Values[j];

                    if (step.WarmStarting)
                    {
                        vcp.NormalImpulse = step.DtRatio * cp.NormalImpulse;
                        vcp.TangentImpulse = step.DtRatio * cp.TangentImpulse;
                    }
                    else
                    {
                        vcp.NormalImpulse = 0.0f;
                        vcp.TangentImpulse = 0.0f;
                    }

                    vcp.Ra.SetZero();
                    vcp.Rb.SetZero();
                    vcp.NormalMass = 0.0f;
                    vcp.TangentMass = 0.0f;
                    vcp.VelocityBias = 0.0f;

                    pc.LocalPoints[j] = cp.LocalPoint;
                }
            }
        }

        public void Reset()
        {
            ArrayPool<ContactPositionConstraint>.Shared.Return(_positionConstraints, true);
            _positionConstraints = null;
            ArrayPool<ContactVelocityConstraint>.Shared.Return(VelocityConstraints, true);
            VelocityConstraints = null;
        }

        public void InitializeVelocityConstraints()
        {
            for (var i = 0; i < _contactCount; ++i)
            {
                ref var vc = ref VelocityConstraints[i];
                ref var pc = ref _positionConstraints[i];

                var radiusA = pc.RadiusA;
                var radiusB = pc.RadiusB;
                ref readonly var manifold = ref _contacts[vc.ContactIndex].Manifold;

                var indexA = vc.IndexA;
                var indexB = vc.IndexB;

                var mA = vc.InvMassA;
                var mB = vc.InvMassB;
                var iA = vc.InvIa;
                var iB = vc.InvIb;
                var localCenterA = pc.LocalCenterA;
                var localCenterB = pc.LocalCenterB;

                var cA = _positions[indexA].Center;
                var aA = _positions[indexA].Angle;
                var vA = _velocities[indexA].V;
                var wA = _velocities[indexA].W;

                var cB = _positions[indexB].Center;
                var aB = _positions[indexB].Angle;
                var vB = _velocities[indexB].V;
                var wB = _velocities[indexB].W;

                Debug.Assert(manifold.PointCount > 0);

                var xfA = new Transform();
                var xfB = new Transform();
                xfA.Rotation.Set(aA);
                xfB.Rotation.Set(aB);
                xfA.Position = cA - MathUtils.Mul(xfA.Rotation, localCenterA);
                xfB.Position = cB - MathUtils.Mul(xfB.Rotation, localCenterB);

                var worldManifold = new WorldManifold();
                worldManifold.Initialize(
                    manifold,
                    xfA,
                    radiusA,
                    xfB,
                    radiusB);

                vc.Normal = worldManifold.Normal;

                var pointCount = vc.PointCount;

                unsafe
                {
                    for (var j = 0; j < pointCount; ++j)
                    {
                        ref var vcp = ref vc.Points.Values[j];

                        vcp.Ra = worldManifold.Points.Values[j] - cA;
                        vcp.Rb = worldManifold.Points.Values[j] - cB;

                        var rnA = MathUtils.Cross(vcp.Ra, vc.Normal);
                        var rnB = MathUtils.Cross(vcp.Rb, vc.Normal);

                        var kNormal = mA + mB + iA * rnA * rnA + iB * rnB * rnB;

                        vcp.NormalMass = kNormal > 0.0f ? 1.0f / kNormal : 0.0f;

                        var tangent = MathUtils.Cross(vc.Normal, 1.0f);

                        var rtA = MathUtils.Cross(vcp.Ra, tangent);
                        var rtB = MathUtils.Cross(vcp.Rb, tangent);

                        var kTangent = mA + mB + iA * rtA * rtA + iB * rtB * rtB;

                        vcp.TangentMass = kTangent > 0.0f ? 1.0f / kTangent : 0.0f;

                        // Setup a velocity bias for restitution.
                        vcp.VelocityBias = 0.0f;
                        var vRel = Vector2.Dot(
                            vc.Normal,
                            vB + MathUtils.Cross(wB, vcp.Rb) - vA - MathUtils.Cross(wA, vcp.Ra));
                        if (vRel < -Settings.VelocityThreshold)
                        {
                            vcp.VelocityBias = -vc.Restitution * vRel;
                        }
                    }
                }

                // If we have two points, then prepare the block solver.
                if (vc.PointCount == 2)
                {
                    ref var vcp1 = ref vc.Points.Value0;
                    ref var vcp2 = ref vc.Points.Value1;

                    var rn1A = MathUtils.Cross(vcp1.Ra, vc.Normal);
                    var rn1B = MathUtils.Cross(vcp1.Rb, vc.Normal);
                    var rn2A = MathUtils.Cross(vcp2.Ra, vc.Normal);
                    var rn2B = MathUtils.Cross(vcp2.Rb, vc.Normal);

                    var k11 = mA + mB + iA * rn1A * rn1A + iB * rn1B * rn1B;
                    var k22 = mA + mB + iA * rn2A * rn2A + iB * rn2B * rn2B;
                    var k12 = mA + mB + iA * rn1A * rn2A + iB * rn1B * rn2B;

                    // Ensure a reasonable condition number.
                    const float maxConditionNumber = 1000.0f;
                    if (k11 * k11 < maxConditionNumber * (k11 * k22 - k12 * k12))
                    {
                        // K is safe to invert.
                        vc.K.Ex.Set(k11, k12);
                        vc.K.Ey.Set(k12, k22);
                        vc.NormalMass = vc.K.GetInverse();
                    }
                    else
                    {
                        // The constraints are redundant, just use one.
                        // TODO_ERIN use deepest?
                        vc.PointCount = 1;
                    }
                }
            }
        }

        public unsafe void WarmStart()
        {
            // Warm start.
            for (var i = 0; i < _contactCount; ++i)
            {
                ref var vc = ref VelocityConstraints[i];

                var indexA = vc.IndexA;
                var indexB = vc.IndexB;
                var mA = vc.InvMassA;
                var iA = vc.InvIa;
                var mB = vc.InvMassB;
                var iB = vc.InvIb;
                var pointCount = vc.PointCount;

                var vA = _velocities[indexA].V;
                var wA = _velocities[indexA].W;
                var vB = _velocities[indexB].V;
                var wB = _velocities[indexB].W;

                var normal = vc.Normal;
                var tangent = MathUtils.Cross(normal, 1.0f);

                for (var j = 0; j < pointCount; ++j)
                {
                    ref var vcp = ref vc.Points.Values[j];
                    var P = vcp.NormalImpulse * normal + vcp.TangentImpulse * tangent;
                    wA -= iA * MathUtils.Cross(vcp.Ra, P);
                    vA -= mA * P;
                    wB += iB * MathUtils.Cross(vcp.Rb, P);
                    vB += mB * P;
                }

                _velocities[indexA].V = vA;
                _velocities[indexA].W = wA;
                _velocities[indexB].V = vB;
                _velocities[indexB].W = wB;
            }
        }

        public unsafe void SolveVelocityConstraints()
        {
            for (var i = 0; i < _contactCount; ++i)
            {
                ref var vc = ref VelocityConstraints[i];

                var indexA = vc.IndexA;
                var indexB = vc.IndexB;
                var mA = vc.InvMassA;
                var iA = vc.InvIa;
                var mB = vc.InvMassB;
                var iB = vc.InvIb;
                var pointCount = vc.PointCount;

                var vA = _velocities[indexA].V;
                var wA = _velocities[indexA].W;
                var vB = _velocities[indexB].V;
                var wB = _velocities[indexB].W;

                var normal = vc.Normal;
                var tangent = MathUtils.Cross(normal, 1.0f);
                var friction = vc.Friction;

                Debug.Assert(pointCount == 1 || pointCount == 2);

                // Solve tangent constraints first because non-penetration is more important
                // than friction.
                for (var j = 0; j < pointCount; ++j)
                {
                    ref var vcp = ref vc.Points.Values[j];

                    // Relative velocity at contact
                    var dv = vB + MathUtils.Cross(wB, vcp.Rb) - vA - MathUtils.Cross(wA, vcp.Ra);

                    // Compute tangent force
                    var vt = Vector2.Dot(dv, tangent) - vc.TangentSpeed;
                    var lambda = vcp.TangentMass * -vt;

                    // MathUtils.b2Clamp the accumulated force
                    var maxFriction = friction * vcp.NormalImpulse;
                    var newImpulse = MathUtils.Clamp(vcp.TangentImpulse + lambda, -maxFriction, maxFriction);
                    lambda = newImpulse - vcp.TangentImpulse;
                    vcp.TangentImpulse = newImpulse;

                    // Apply contact impulse
                    var P = lambda * tangent;

                    vA -= mA * P;
                    wA -= iA * MathUtils.Cross(vcp.Ra, P);

                    vB += mB * P;
                    wB += iB * MathUtils.Cross(vcp.Rb, P);
                }

                // Solve normal constraints
                if (pointCount == 1)
                {
                    for (var j = 0; j < pointCount; ++j)
                    {
                        ref var vcp = ref vc.Points.Values[j];

                        // Relative velocity at contact
                        var dv = vB + MathUtils.Cross(wB, vcp.Rb) - vA - MathUtils.Cross(wA, vcp.Ra);

                        // Compute normal impulse
                        var vn = Vector2.Dot(dv, normal);
                        var lambda = -vcp.NormalMass * (vn - vcp.VelocityBias);

                        // MathUtils.b2Clamp the accumulated impulse
                        var newImpulse = Math.Max(vcp.NormalImpulse + lambda, 0.0f);
                        lambda = newImpulse - vcp.NormalImpulse;
                        vcp.NormalImpulse = newImpulse;

                        // Apply contact impulse
                        var P = lambda * normal;
                        vA -= mA * P;
                        wA -= iA * MathUtils.Cross(vcp.Ra, P);

                        vB += mB * P;
                        wB += iB * MathUtils.Cross(vcp.Rb, P);
                    }
                }
                else
                {
                    // Block solver developed in collaboration with Dirk Gregorius (back in 01/07 on Box2D_Lite).
                    // Build the mini LCP for this contact patch
                    //
                    // vn = A * x + b, vn >= 0, x >= 0 and vn_i * x_i = 0 with i = 1..2
                    //
                    // A = J * W * JT and J = ( -n, -r1 x n, n, r2 x n )
                    // b = vn0 - velocityBias
                    //
                    // The system is solved using the "Total enumeration method" (s. Murty). The complementary constraint vn_i * x_i
                    // implies that we must have in any solution either vn_i = 0 or x_i = 0. So for the 2D contact problem the cases
                    // vn1 = 0 and vn2 = 0, x1 = 0 and x2 = 0, x1 = 0 and vn2 = 0, x2 = 0 and vn1 = 0 need to be tested. The first valid
                    // solution that satisfies the problem is chosen.
                    // 
                    // In order to account of the accumulated impulse 'a' (because of the iterative nature of the solver which only requires
                    // that the accumulated impulse is clamped and not the incremental impulse) we change the impulse variable (x_i).
                    //
                    // Substitute:
                    // 
                    // x = a + d
                    // 
                    // a := old total impulse
                    // x := new total impulse
                    // d := incremental impulse 
                    //
                    // For the current iteration we extend the formula for the incremental impulse
                    // to compute the new total impulse:
                    //
                    // vn = A * d + b
                    //    = A * (x - a) + b
                    //    = A * x + b - A * a
                    //    = A * x + b'
                    // b' = b - A * a;

                    ref var cp1 = ref vc.Points.Value0;
                    ref var cp2 = ref vc.Points.Value1;

                    var a = new Vector2(cp1.NormalImpulse, cp2.NormalImpulse);
                    Debug.Assert(a.X >= 0.0f && a.Y >= 0.0f);

                    // Relative velocity at contact
                    var dv1 = vB + MathUtils.Cross(wB, cp1.Rb) - vA - MathUtils.Cross(wA, cp1.Ra);
                    var dv2 = vB + MathUtils.Cross(wB, cp2.Rb) - vA - MathUtils.Cross(wA, cp2.Ra);

                    // Compute normal velocity
                    var vn1 = Vector2.Dot(dv1, normal);
                    var vn2 = Vector2.Dot(dv2, normal);

                    var b = new Vector2 {X = vn1 - cp1.VelocityBias, Y = vn2 - cp2.VelocityBias};

                    // Compute b'
                    b -= MathUtils.Mul(vc.K, a);

                    for (;;)
                    {
                        //
                        // Case 1: vn = 0
                        //
                        // 0 = A * x + b'
                        //
                        // Solve for x:
                        //
                        // x = - inv(A) * b'
                        //
                        var x = -MathUtils.Mul(vc.NormalMass, b);

                        if (x.X >= 0.0f && x.Y >= 0.0f)
                        {
                            // Get the incremental impulse
                            var d = x - a;

                            // Apply incremental impulse
                            var P1 = d.X * normal;
                            var P2 = d.Y * normal;
                            vA -= mA * (P1 + P2);
                            wA -= iA * (MathUtils.Cross(cp1.Ra, P1) + MathUtils.Cross(cp2.Ra, P2));

                            vB += mB * (P1 + P2);
                            wB += iB * (MathUtils.Cross(cp1.Rb, P1) + MathUtils.Cross(cp2.Rb, P2));

                            // Accumulate
                            cp1.NormalImpulse = x.X;
                            cp2.NormalImpulse = x.Y;

#if B2_DEBUG_SOLVER
// Postconditions
                            const float k_errorTol = 1e-3f;
                            dv1 = vB + MathUtils.Cross(wB, cp1.rB) - vA - MathUtils.Cross(wA, cp1.rA);
                            dv2 = vB + MathUtils.Cross(wB, cp2.rB) - vA - MathUtils.Cross(wA, cp2.rA);

                            // Compute normal velocity
                            vn1 = Vector2.Dot(dv1, normal);
                            vn2 = Vector2.Dot(dv2, normal);

                            Debug.Assert(Math.Abs(vn1 - cp1.velocityBias) < k_errorTol);
                            Debug.Assert(Math.Abs(vn2 - cp2.velocityBias) < k_errorTol);
#endif
                            break;
                        }

                        //
                        // Case 2: vn1 = 0 and x2 = 0
                        //
                        //   0 = a11 * x1 + a12 * 0 + b1' 
                        // vn2 = a21 * x1 + a22 * 0 + b2'
                        //
                        x.X = -cp1.NormalMass * b.X;
                        x.Y = 0.0f;
                        vn1 = 0.0f;
                        vn2 = vc.K.Ex.Y * x.X + b.Y;
                        if (x.X >= 0.0f && vn2 >= 0.0f)
                        {
                            // Get the incremental impulse
                            var d = x - a;

                            // Apply incremental impulse
                            var P1 = d.X * normal;
                            var P2 = d.Y * normal;
                            vA -= mA * (P1 + P2);
                            wA -= iA * (MathUtils.Cross(cp1.Ra, P1) + MathUtils.Cross(cp2.Ra, P2));

                            vB += mB * (P1 + P2);
                            wB += iB * (MathUtils.Cross(cp1.Rb, P1) + MathUtils.Cross(cp2.Rb, P2));

                            // Accumulate
                            cp1.NormalImpulse = x.X;
                            cp2.NormalImpulse = x.Y;

#if B2_DEBUG_SOLVER
// Postconditions
                            dv1 = vB + MathUtils.Cross(wB, cp1.rB) - vA - MathUtils.Cross(wA, cp1.rA);

                            // Compute normal velocity
                            vn1 = Vector2.Dot(dv1, normal);

                            Debug.Assert(Math.Abs(vn1 - cp1.velocityBias) < k_errorTol);
#endif
                            break;
                        }

                        //
                        // Case 3: vn2 = 0 and x1 = 0
                        //
                        // vn1 = a11 * 0 + a12 * x2 + b1' 
                        //   0 = a21 * 0 + a22 * x2 + b2'
                        //
                        x.X = 0.0f;
                        x.Y = -cp2.NormalMass * b.Y;
                        vn1 = vc.K.Ey.X * x.Y + b.X;
                        vn2 = 0.0f;

                        if (x.Y >= 0.0f && vn1 >= 0.0f)
                        {
                            // Resubstitute for the incremental impulse
                            var d = x - a;

                            // Apply incremental impulse
                            var P1 = d.X * normal;
                            var P2 = d.Y * normal;
                            vA -= mA * (P1 + P2);
                            wA -= iA * (MathUtils.Cross(cp1.Ra, P1) + MathUtils.Cross(cp2.Ra, P2));

                            vB += mB * (P1 + P2);
                            wB += iB * (MathUtils.Cross(cp1.Rb, P1) + MathUtils.Cross(cp2.Rb, P2));

                            // Accumulate
                            cp1.NormalImpulse = x.X;
                            cp2.NormalImpulse = x.Y;

#if B2_DEBUG_SOLVER
// Postconditions
                            dv2 = vB + MathUtils.Cross(wB, cp2.rB) - vA - MathUtils.Cross(wA, cp2.rA);

                            // Compute normal velocity
                            vn2 = Vector2.Dot(dv2, normal);

                            Debug.Assert(Math.Abs(vn2 - cp2.velocityBias) < k_errorTol);
#endif
                            break;
                        }

                        //
                        // Case 4: x1 = 0 and x2 = 0
                        // 
                        // vn1 = b1
                        // vn2 = b2;
                        x.X = 0.0f;
                        x.Y = 0.0f;
                        vn1 = b.X;
                        vn2 = b.Y;

                        if (vn1 >= 0.0f && vn2 >= 0.0f)
                        {
                            // Resubstitute for the incremental impulse
                            var d = x - a;

                            // Apply incremental impulse
                            var P1 = d.X * normal;
                            var P2 = d.Y * normal;
                            vA -= mA * (P1 + P2);
                            wA -= iA * (MathUtils.Cross(cp1.Ra, P1) + MathUtils.Cross(cp2.Ra, P2));

                            vB += mB * (P1 + P2);
                            wB += iB * (MathUtils.Cross(cp1.Rb, P1) + MathUtils.Cross(cp2.Rb, P2));

                            // Accumulate
                            cp1.NormalImpulse = x.X;
                            cp2.NormalImpulse = x.Y;
                        }

                        // No solution, give up. This is hit sometimes, but it doesn't seem to matter.
                        break;
                    }
                }

                _velocities[indexA].V = vA;
                _velocities[indexA].W = wA;
                _velocities[indexB].V = vB;
                _velocities[indexB].W = wB;
            }
        }

        public void StoreImpulses()
        {
            unsafe
            {
                for (var i = 0; i < _contactCount; ++i)
                {
                    ref var vc = ref VelocityConstraints[i];
                    ref var manifold = ref _contacts[vc.ContactIndex].Manifold;

                    for (var j = 0; j < vc.PointCount; ++j)
                    {
                        ref var point = ref manifold.Points.Values[j];
                        point.NormalImpulse = vc.Points[j].NormalImpulse;
                        point.TangentImpulse = vc.Points[j].TangentImpulse;
                    }
                }
            }
        }

        public bool SolvePositionConstraints()
        {
            var minSeparation = 0.0f;

            for (var i = 0; i < _contactCount; ++i)
            {
                ref var pc = ref _positionConstraints[i];

                var indexA = pc.IndexA;
                var indexB = pc.IndexB;
                var localCenterA = pc.LocalCenterA;
                var mA = pc.InvMassA;
                var iA = pc.InvIa;
                var localCenterB = pc.LocalCenterB;
                var mB = pc.InvMassB;
                var iB = pc.InvIb;
                var pointCount = pc.PointCount;

                var cA = _positions[indexA].Center;
                var aA = _positions[indexA].Angle;

                var cB = _positions[indexB].Center;
                var aB = _positions[indexB].Angle;

                // Solve normal constraints
                for (var j = 0; j < pointCount; ++j)
                {
                    var xfA = new Transform();
                    var xfB = xfA;
                    xfA.Rotation.Set(aA);
                    xfB.Rotation.Set(aB);
                    xfA.Position = cA - MathUtils.Mul(xfA.Rotation, localCenterA);
                    xfB.Position = cB - MathUtils.Mul(xfB.Rotation, localCenterB);

                    var psm = new PositionSolverManifold();
                    psm.Initialize(pc, xfA, xfB, j);
                    var normal = psm.Normal;

                    var point = psm.Point;
                    var separation = psm.Separation;

                    var rA = point - cA;
                    var rB = point - cB;

                    // Track max constraint error.
                    minSeparation = Math.Min(minSeparation, separation);

                    // Prevent large corrections and allow slop.
                    var C = MathUtils.Clamp(
                        Settings.Baumgarte * (separation + Settings.LinearSlop),
                        -Settings.MaxLinearCorrection,
                        0.0f);

                    // Compute the effective mass.
                    var rnA = MathUtils.Cross(rA, normal);
                    var rnB = MathUtils.Cross(rB, normal);
                    var K = mA + mB + iA * rnA * rnA + iB * rnB * rnB;

                    // Compute normal impulse
                    var impulse = K > 0.0f ? -C / K : 0.0f;

                    var P = impulse * normal;

                    cA -= mA * P;
                    aA -= iA * MathUtils.Cross(rA, P);

                    cB += mB * P;
                    aB += iB * MathUtils.Cross(rB, P);
                }

                _positions[indexA].Center = cA;
                _positions[indexA].Angle = aA;

                _positions[indexB].Center = cB;
                _positions[indexB].Angle = aB;
            }

            // We can't expect minSpeparation >= -b2_linearSlop because we don't
            // push the separation above -b2_linearSlop.
            return minSeparation >= -3.0f * Settings.LinearSlop;
        }

        public bool SolveTOIPositionConstraints(int toiIndexA, int toiIndexB)
        {
            var minSeparation = 0.0f;

            for (var i = 0; i < _contactCount; ++i)
            {
                ref var pc = ref _positionConstraints[i];

                var indexA = pc.IndexA;
                var indexB = pc.IndexB;
                var localCenterA = pc.LocalCenterA;
                var localCenterB = pc.LocalCenterB;
                var pointCount = pc.PointCount;

                var mA = 0.0f;
                var iA = 0.0f;
                if (indexA == toiIndexA || indexA == toiIndexB)
                {
                    mA = pc.InvMassA;
                    iA = pc.InvIa;
                }

                var mB = 0.0f;
                var iB = 0.0f;
                if (indexB == toiIndexA || indexB == toiIndexB)
                {
                    mB = pc.InvMassB;
                    iB = pc.InvIb;
                }

                var cA = _positions[indexA].Center;
                var aA = _positions[indexA].Angle;

                var cB = _positions[indexB].Center;
                var aB = _positions[indexB].Angle;

                // Solve normal constraints
                for (var j = 0; j < pointCount; ++j)
                {
                    var xfA = new Transform();
                    var xfB = new Transform();
                    xfA.Rotation.Set(aA);
                    xfB.Rotation.Set(aB);
                    xfA.Position = cA - MathUtils.Mul(xfA.Rotation, localCenterA);
                    xfB.Position = cB - MathUtils.Mul(xfB.Rotation, localCenterB);

                    var psm = new PositionSolverManifold();
                    psm.Initialize(pc, xfA, xfB, j);
                    var normal = psm.Normal;

                    var point = psm.Point;
                    var separation = psm.Separation;

                    var rA = point - cA;
                    var rB = point - cB;

                    // Track max constraint error.
                    minSeparation = Math.Min(minSeparation, separation);

                    // Prevent large corrections and allow slop.
                    var C = MathUtils.Clamp(
                        Settings.ToiBaugarte * (separation + Settings.LinearSlop),
                        -Settings.MaxLinearCorrection,
                        0.0f);

                    // Compute the effective mass.
                    var rnA = MathUtils.Cross(rA, normal);
                    var rnB = MathUtils.Cross(rB, normal);
                    var K = mA + mB + iA * rnA * rnA + iB * rnB * rnB;

                    // Compute normal impulse
                    var impulse = K > 0.0f ? -C / K : 0.0f;

                    var P = impulse * normal;

                    cA -= mA * P;
                    aA -= iA * MathUtils.Cross(rA, P);

                    cB += mB * P;
                    aB += iB * MathUtils.Cross(rB, P);
                }

                _positions[indexA].Center = cA;
                _positions[indexA].Angle = aA;

                _positions[indexB].Center = cB;
                _positions[indexB].Angle = aB;
            }

            // We can't expect minSpeparation >= -b2_linearSlop because we don't
            // push the separation above -b2_linearSlop.
            return minSeparation >= -1.5f * Settings.LinearSlop;
        }
    }
}