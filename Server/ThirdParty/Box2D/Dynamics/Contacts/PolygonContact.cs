using System.Diagnostics;
using System.Runtime.CompilerServices;
using Box2DSharp.Collision;
using Box2DSharp.Collision.Collider;
using Box2DSharp.Collision.Shapes;
using Box2DSharp.Common;

namespace Box2DSharp.Dynamics.Contacts
{
    public class PolygonContact : Contact
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal override void Evaluate(ref Manifold manifold, in Transform xfA, Transform xfB)
        {
            CollisionUtils.CollidePolygons(
                ref manifold,
                (PolygonShape)FixtureA.Shape,
                xfA,
                (PolygonShape)FixtureB.Shape,
                xfB);
        }
    }

    internal class PolygonContactFactory : IContactFactory
    {
        private readonly ContactPool<PolygonContact> _pool = new ContactPool<PolygonContact>();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Contact Create(Fixture fixtureA, int indexA, Fixture fixtureB, int indexB)
        {
            Debug.Assert(fixtureA.ShapeType == ShapeType.Polygon);
            Debug.Assert(fixtureB.ShapeType == ShapeType.Polygon);
            var contact = _pool.Get();
            contact.Initialize(fixtureA, 0, fixtureB, 0);
            return contact;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Destroy(Contact contact)
        {
            _pool.Return((PolygonContact)contact);
        }
    }
}