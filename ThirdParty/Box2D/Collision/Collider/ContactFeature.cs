namespace Box2DSharp.Collision.Collider
{
    /// The features that intersect to form the contact point
    /// This must be 4 bytes or less.
    public struct ContactFeature
    {
        public enum FeatureType: byte
        {
            Vertex = 0,

            Face = 1
        }

        /// Feature index on shapeA
        public byte IndexA;

        /// Feature index on shapeB
        public byte IndexB;

        /// The feature type on shapeA
        public byte TypeA;

        /// The feature type on shapeB
        public byte TypeB;
    }
}