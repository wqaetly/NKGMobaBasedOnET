using System.Numerics;
using Box2DSharp.Common;

namespace Box2DSharp.Dynamics.Contacts
{
    public struct ContactVelocityConstraint
    {
        /// <summary>
        /// Size <see cref="Settings.MaxManifoldPoints"/>
        /// </summary>
        public FixedArray2<VelocityConstraintPoint> Points;

        public int ContactIndex;

        public float Friction;

        public int IndexA;

        public int IndexB;

        public float InvIa, InvIb;

        public float InvMassA, InvMassB;

        public Matrix2x2 K;

        public Vector2 Normal;

        public Matrix2x2 NormalMass;

        public int PointCount;

        public float Restitution;

        public float TangentSpeed;
    }
}