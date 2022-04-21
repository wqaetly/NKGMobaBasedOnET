namespace Box2DSharp.Collision.Collider
{
    /// This is used for determining the state of contact points.
    public enum PointState
    {
        /// point does not exist
        NullState,

        /// point was added in the update
        AddState,

        /// point persisted across the update
        PersistState,

        /// point was removed in the update
        RemoveState
    }
}