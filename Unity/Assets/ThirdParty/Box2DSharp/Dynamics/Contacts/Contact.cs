using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Box2DSharp.Collision;
using Box2DSharp.Collision.Collider;
using Box2DSharp.Collision.Shapes;
using Box2DSharp.Common;

namespace Box2DSharp.Dynamics.Contacts
{
    public abstract class Contact
    {
        /// <summary>
        /// Get fixture A in this contact.
        /// </summary>
        public Fixture FixtureA;

        /// <summary>
        /// Get fixture B in this contact.
        /// </summary>
        public Fixture FixtureB;

        internal ContactFlag Flags;

        internal float Friction;

        /// <summary>
        /// Get the child primitive index for fixture A.
        /// </summary>
        public int ChildIndexA;

        /// <summary>
        /// Get the child primitive index for fixture B.
        /// </summary>
        public int ChildIndexB;

        /// <summary>
        /// Get the contact manifold. Do not modify the manifold unless you understand the
        /// internals of Box2D.
        /// </summary>
        public Manifold Manifold;

        /// World pool and list pointers.
        /// Nodes for connecting bodies.
        internal LinkedListNode<Contact> Node;

        internal ContactEdge NodeA;

        internal ContactEdge NodeB;

        internal float Restitution;

        internal float TangentSpeed;

        internal float Toi;

        internal int ToiCount;

        internal void Initialize(Fixture fixtureA, int indexA, Fixture fixtureB, int indexB)
        {
            Flags = ContactFlag.EnabledFlag;

            FixtureA = fixtureA;
            FixtureB = fixtureB;

            NodeA = SimpleObjectPool<ContactEdge>.Shared.Get();
            NodeB = SimpleObjectPool<ContactEdge>.Shared.Get();

            ChildIndexA = indexA;
            ChildIndexB = indexB;

            ToiCount = 0;

            Friction = MixFriction(FixtureA.Friction, FixtureB.Friction);
            Restitution = MixRestitution(FixtureA.Restitution, FixtureB.Restitution);

            TangentSpeed = 0.0f;
        }

        internal virtual void Reset()
        {
            FixtureA = default;
            FixtureB = default;
            Flags = default;
            Friction = default;
            ChildIndexA = default;
            ChildIndexB = default;
            Manifold = default;
            Node = default;
            SimpleObjectPool<ContactEdge>.Shared.Return(NodeA);
            SimpleObjectPool<ContactEdge>.Shared.Return(NodeB);
            NodeA = default;
            NodeB = default;
            Restitution = default;
            TangentSpeed = default;
            Toi = default;
            ToiCount = default;
        }

        private static float MixFriction(float friction1, float friction2)
        {
            return (float) Math.Sqrt(friction1 * friction2);
        }

        private static float MixRestitution(float restitution1, float restitution2)
        {
            return restitution1 > restitution2 ? restitution1 : restitution2;
        }

        /// Get the world manifold.
        public void GetWorldManifold(out WorldManifold worldManifold)
        {
            var bodyA = FixtureA.Body;
            var bodyB = FixtureB.Body;
            var shapeA = FixtureA.Shape;
            var shapeB = FixtureB.Shape;
            worldManifold = new WorldManifold();
            worldManifold.Initialize(
                Manifold,
                bodyA.GetTransform(),
                shapeA.Radius,
                bodyB.GetTransform(),
                shapeB.Radius);
        }

        /// Is this contact touching?

        public bool IsTouching
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => HasFlag(ContactFlag.TouchingFlag);
        }

        /// Enable/disable this contact. This can be used inside the pre-solve
        /// contact listener. The contact is only disabled for the current
        /// time step (or sub-step in continuous collisions).
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetEnabled(bool flag)
        {
            if (flag)
            {
                Flags |= ContactFlag.EnabledFlag;
            }
            else
            {
                Flags &= ~ContactFlag.EnabledFlag;
            }
        }

        /// Has this contact been disabled?
        public bool IsEnabled
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => HasFlag(ContactFlag.EnabledFlag);
        }

        /// Override the default friction mixture. You can call this in b2ContactListener::PreSolve.
        /// This value persists until set or reset.
        public void SetFriction(float friction)
        {
            Friction = friction;
        }

        /// Get the friction.
        public float GetFriction()
        {
            return Friction;
        }

        /// Reset the friction mixture to the default value.
        public void ResetFriction()
        {
            Friction = MixFriction(FixtureA.Friction, FixtureB.Friction);
        }

        /// Override the default restitution mixture. You can call this in b2ContactListener::PreSolve.
        /// The value persists until you set or reset.
        public void SetRestitution(float restitution)
        {
            Restitution = restitution;
        }

        /// Get the restitution.
        public float GetRestitution()
        {
            return Restitution;
        }

        /// Reset the restitution to the default value.
        public void ResetRestitution()
        {
            Restitution = MixRestitution(FixtureA.Restitution, FixtureB.Restitution);
        }

        /// Set the desired tangent speed for a conveyor belt behavior. In meters per second.
        public void SetTangentSpeed(float speed)
        {
            TangentSpeed = speed;
        }

        /// Get the desired tangent speed. In meters per second.
        public float GetTangentSpeed()
        {
            return TangentSpeed;
        }

        /// Evaluate this contact with your own manifold and transforms.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal abstract void Evaluate(ref Manifold manifold, in Transform xfA, Transform xfB);

        /// Flag this contact for filtering. Filtering will occur the next time step.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void FlagForFiltering()
        {
            Flags |= ContactFlag.FilterFlag;
        }

        internal void Update(IContactListener listener)
        {
            var oldManifold = Manifold;

            // Re-enable this contact.
            Flags |= ContactFlag.EnabledFlag;

            var touching = false;
            var wasTouching = HasFlag(ContactFlag.TouchingFlag);

            var sensorA = FixtureA.IsSensor;
            var sensorB = FixtureB.IsSensor;
            var sensor = sensorA || sensorB;

            var bodyA = FixtureA.Body;
            var bodyB = FixtureB.Body;
            var xfA = bodyA.GetTransform();
            var xfB = bodyB.GetTransform();

            // Is this contact a sensor?
            if (sensor)
            {
                var shapeA = FixtureA.Shape;
                var shapeB = FixtureB.Shape;
                touching = CollisionUtils.TestOverlap(
                    shapeA,
                    ChildIndexA,
                    shapeB,
                    ChildIndexB,
                    xfA,
                    xfB,
                    bodyA.World.GJkProfile);

                // Sensors don't generate manifolds.
                Manifold.PointCount = 0;
            }
            else
            {
                Evaluate(ref Manifold, xfA, xfB);
                touching = Manifold.PointCount > 0;

                // Match old contact ids to new contact ids and copy the
                // stored impulses to warm start the solver.

                unsafe
                {
                    for (var i = 0; i < Manifold.PointCount; ++i)
                    {
                        var mp2 = Manifold.Points.Values[i];
                        mp2.NormalImpulse = 0.0f;
                        mp2.TangentImpulse = 0.0f;
                        var id2 = mp2.Id;

                        for (var j = 0; j < oldManifold.PointCount; ++j)
                        {
                            var mp1 = oldManifold.Points.Values[j];

                            if (mp1.Id.Key == id2.Key)
                            {
                                mp2.NormalImpulse = mp1.NormalImpulse;
                                mp2.TangentImpulse = mp1.TangentImpulse;
                                break;
                            }
                        }
                    }
                }

                if (touching != wasTouching)
                {
                    bodyA.IsAwake = true;
                    bodyB.IsAwake = true;
                }
            }

            if (touching)
            {
                Flags |= ContactFlag.TouchingFlag;
            }
            else
            {
                Flags &= ~ContactFlag.TouchingFlag;
            }

            if (listener != default)
            {
                if (wasTouching == false && touching)
                {
                    listener.BeginContact(this);
                }

                if (wasTouching && touching == false)
                {
                    listener.EndContact(this);
                }

                if (sensor == false && touching)
                {
                    listener.PreSolve(this, oldManifold);
                }
            }
        }

        // Flags stored in m_flags
        [Flags]
        public enum ContactFlag
        {
            // Used when crawling contact graph when forming islands.
            IslandFlag = 0x0001,

            // Set when the shapes are touching.
            TouchingFlag = 0x0002,

            // This contact can be disabled (by user)
            EnabledFlag = 0x0004,

            // This contact needs filtering because a fixture filter was changed.
            FilterFlag = 0x0008,

            // This bullet contact had a TOI event
            BulletHitFlag = 0x0010,

            // This contact has a valid TOI in m_toi
            ToiFlag = 0x0020
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool HasFlag(ContactFlag flag)
        {
            return (Flags & flag) != 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetFlag(ContactFlag flag)
        {
            Flags |= flag;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void UnsetFlag(ContactFlag flag)
        {
            Flags &= ~flag;
        }
    }
}