using System;
using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using Box2DSharp.Collision;
using Box2DSharp.Collision.Collider;
using Box2DSharp.Collision.Shapes;
using Box2DSharp.Common;

namespace Box2DSharp.Dynamics
{
    /// A fixture is used to attach a shape to a body for collision detection. A fixture
    /// inherits its transform from its parent. Fixtures hold additional non-geometric data
    /// such as friction, collision filters, etc.
    /// Fixtures are created via b2Body::CreateFixture.
    /// @warning you cannot reuse fixtures.
    public class Fixture : IDisposable
    {
        /// <summary>
        /// the coefficient of restitution. This will _not_ change the restitution of existing contacts.
        /// </summary>
        public float Restitution;

        /// Get the parent body of this fixture. This is null if the fixture is not attached.
        /// @return the parent body.
        public Body Body { get; set; }

        /// <summary>
        /// the density of this fixture. This will _not_ automatically adjust the mass of the body. You must call b2Body::ResetMassData to update the body's mass.
        /// </summary>
        public float Density
        {
            get => _density;
            set
            {
                Debug.Assert(value.IsValid() && value >= 0.0f);
                _density = value;
            }
        }

        private float _density;

        /// Get the child shape. You can modify the child shape, however you should not change the
        /// number of vertices because this will crash some collision caching mechanisms.
        /// Manipulating the shape may lead to non-physical behavior.
        /// Set the contact filtering data. This will not update contacts until the next time
        /// step when either parent body is active and awake.
        /// This automatically calls Refilter.
        public Filter Filter
        {
            get => _filter;
            set
            {
                _filter = value;
                Refilter();
            }
        }

        private Filter _filter;

        /// <summary>
        /// the coefficient of friction. This will _not_ change the friction of existing contacts.
        /// </summary>
        public float Friction { get; set; }

        public bool IsSensor
        {
            get => _isSensor;
            set
            {
                if (_isSensor != value)
                {
                    Body.IsAwake = true;
                    _isSensor = value;
                }
            }
        }

        private bool _isSensor;

        public FixtureProxy[] Proxies { get; private set; }

        public int ProxyCount { get; internal set; }

        public Shape Shape { get; private set; }

        /// <summary>
        /// the user data that was assigned in the fixture definition. Use this to store your application specific data.
        /// </summary>
        public object UserData { get; set; }

        /// Get the type of the child shape. You can use this to down cast to the concrete shape.
        /// @return the shape type.
        public ShapeType ShapeType => Shape.ShapeType;

        /// We need separation create/destroy functions from the constructor/destructor because
        /// the destructor cannot access the allocator (no destructor arguments allowed by C++).
        internal static Fixture Create(Body body, in FixtureDef def)
        {
            var childCount = def.Shape.GetChildCount();

            var fixture = new Fixture
            {
                UserData = def.UserData,
                Friction = def.Friction,
                Restitution = def.Restitution,
                Body = body,
                Filter = def.Filter,
                IsSensor = def.IsSensor,
                Shape = def.Shape.Clone(),
                ProxyCount = 0,
                Density = def.Density,
                Proxies = new FixtureProxy[childCount]
            };

            // Reserve proxy space
            for (var i = 0; i < childCount; ++i)
            {
                fixture.Proxies[i] = new FixtureProxy();
            }

            return fixture;
        }

        internal static void Destroy(Fixture fixture)
        {
            // The proxies must be destroyed before calling this.
            Debug.Assert(fixture.ProxyCount == 0);
            fixture.Dispose();
        }

        // These support body activation/deactivation.
        internal void CreateProxies(in BroadPhase broadPhase, in Transform xf)
        {
            Debug.Assert(ProxyCount == 0);

            // Create proxies in the broad-phase.
            ProxyCount = Shape.GetChildCount();

            for (var i = 0; i < ProxyCount; ++i)
            {
                var proxy = Proxies[i];
                Shape.ComputeAABB(out proxy.AABB, xf, i);
                proxy.Fixture = this;
                proxy.ChildIndex = i;
                proxy.ProxyId = broadPhase.CreateProxy(proxy.AABB, proxy);
            }
        }

        internal void DestroyProxies(in BroadPhase broadPhase)
        {
            // Destroy proxies in the broad-phase.
            for (var i = 0; i < ProxyCount; ++i)
            {
                var proxy = Proxies[i];
                broadPhase.DestroyProxy(proxy.ProxyId);
                proxy.ProxyId = BroadPhase.NullProxy;
            }

            ProxyCount = 0;
        }

        /// Call this if you want to establish collision that was previously disabled by b2ContactFilter::ShouldCollide.
        public void Refilter()
        {
            if (Body == null)
            {
                return;
            }

            // Flag associated contacts for filtering.
            var node = Body.ContactEdges.First;
            while (node != null)
            {
                var contact = node.Value.Contact;
                node = node.Next;
                if (contact.FixtureA == this || contact.FixtureB == this)
                {
                    contact.FlagForFiltering();
                }
            }

            if (Body._world == null)
            {
                return;
            }

            // Touch each proxy so that new pairs may be created
            var broadPhase = Body._world.ContactManager.BroadPhase;
            for (var i = 0; i < ProxyCount; ++i)
            {
                broadPhase.TouchProxy(Proxies[i].ProxyId);
            }
        }

        /// Test a point for containment in this fixture.
        /// @param p a point in world coordinates.
        public bool TestPoint(in Vector2 p)
        {
            return Shape.TestPoint(Body.GetTransform(), p);
        }

        /// Cast a ray against this shape.
        /// @param output the ray-cast results.
        /// @param input the ray-cast input parameters.
        public bool RayCast(out RayCastOutput output, in RayCastInput input, int childIndex)
        {
            return Shape.RayCast(out output, input, Body.GetTransform(), childIndex);
        }

        /// Get the mass data for this fixture. The mass data is based on the density and
        /// the shape. The rotational inertia is about the shape's origin. This operation
        /// may be expensive.
        public void GetMassData(out MassData massData)
        {
            Shape.ComputeMass(out massData, Density);
        }

        /// Get the fixture's AABB. This AABB may be enlarge and/or stale.
        /// If you need a more accurate AABB, compute it using the shape and
        /// the body transform.
        public AABB GetAABB(int childIndex)
        {
            Debug.Assert(0 <= childIndex && childIndex < ProxyCount);
            return Proxies[childIndex].AABB;
        }

        /// Dump this fixture to the log file.
        public void Dump(int bodyIndex)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 同步粗检测AABB形状
        /// </summary>
        /// <param name="broadPhase"></param>
        /// <param name="transform1"></param>
        /// <param name="transform2"></param>
        internal void Synchronize(in BroadPhase broadPhase, in Transform transform1, in Transform transform2)
        {
            if (ProxyCount == 0)
            {
                return;
            }

            for (var i = 0; i < ProxyCount; ++i)
            {
                var proxy = Proxies[i];

                // Compute an AABB that covers the swept shape (may miss some rotation effect).

                Shape.ComputeAABB(out var aabb1, transform1, proxy.ChildIndex);
                Shape.ComputeAABB(out var aabb2, transform2, proxy.ChildIndex);

                proxy.AABB.Combine(aabb1, aabb2);

                var displacement = aabb2.GetCenter() - aabb1.GetCenter();

                broadPhase.MoveProxy(proxy.ProxyId, proxy.AABB, displacement);
            }
        }

        /// <inheritdoc />
        public void Dispose()
        {
            Array.Clear(Proxies, 0, Proxies.Length);
            Proxies = null;
            Body = null;
        }
    }

    /// This holds contact filtering data.
    public struct Filter
    {
        /// The collision category bits. Normally you would just set one bit.
        public ushort CategoryBits
        {
            get => _categoryBits.GetValueOrDefault(0x0001);
            set => _categoryBits = value;
        }

        /// The collision mask bits. This states the categories that this
        /// shape would accept for collision.
        public ushort MaskBits
        {
            get => _maskBits.GetValueOrDefault(0xFFFF);
            set => _maskBits = value;
        }

        /// Collision groups allow a certain group of objects to never collide (negative)
        /// or always collide (positive). Zero means no collision group. Non-zero group
        /// filtering always wins against the mask bits.
        public short GroupIndex;

        private ushort? _categoryBits;

        private ushort? _maskBits;
    }

    /// A fixture definition is used to create a fixture. This class defines an
    /// abstract fixture definition. You can reuse fixture definitions safely.
    public struct FixtureDef
    {
        /// The density, usually in kg/m^2.
        public float Density;

        /// Contact filtering data.
        public Filter Filter;

        private float? _friction;

        /// The friction coefficient, usually in the range [0,1].
        public float Friction
        {
            get => _friction.GetValueOrDefault(0.2f);
            set => _friction = value;
        }

        /// A sensor shape collects contact information but never generates a collision
        /// response.
        public bool IsSensor;

        /// The restitution (elasticity) usually in the range [0,1].
        public float Restitution;

        /// The shape, this must be set. The shape will be cloned, so you
        /// can create the shape on the stack.
        public Shape Shape;

        /// Use this to store application specific fixture data.
        public object UserData;
    }

    /// <summary>
    /// This proxy is used internally to connect fixtures to the broad-phase.
    /// 夹具代理,用于夹具和粗检测之间关联
    /// </summary>
    public class FixtureProxy
    {
        public AABB AABB;

        public int ChildIndex;

        public Fixture Fixture;

        public int ProxyId = BroadPhase.NullProxy;
    }
}