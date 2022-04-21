using System.Diagnostics;
using System.Runtime.CompilerServices;
using Box2DSharp.Collision;
using Box2DSharp.Collision.Collider;
using Box2DSharp.Collision.Shapes;
using Box2DSharp.Common;

namespace Box2DSharp.Dynamics.Contacts
{
    /// <summary>
    ///     多边形与圆接触
    /// </summary>
    public class PolygonAndCircleContact : Contact
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal override void Evaluate(ref Manifold manifold, in Transform xfA, Transform xfB)
        {
            CollisionUtils.CollidePolygonAndCircle(
                ref manifold,
                (PolygonShape)FixtureA.Shape,
                xfA,
                (CircleShape)FixtureB.Shape,
                xfB);
        }
    }

    internal class PolygonAndCircleContactFactory : IContactFactory
    {
        private readonly ContactPool<PolygonAndCircleContact> _pool = new ContactPool<PolygonAndCircleContact>();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Contact Create(Fixture fixtureA, int indexA, Fixture fixtureB, int indexB)
        {
            Debug.Assert(fixtureA.ShapeType == ShapeType.Polygon);
            Debug.Assert(fixtureB.ShapeType == ShapeType.Circle);
            var contact = _pool.Get();
            contact.Initialize(fixtureA, 0, fixtureB, 0);
            return contact;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Destroy(Contact contact)
        {
            _pool.Return((PolygonAndCircleContact)contact);
        }
    }
}