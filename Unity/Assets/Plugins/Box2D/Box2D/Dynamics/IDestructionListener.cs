using Box2DSharp.Dynamics.Joints;

namespace Box2DSharp.Dynamics
{
    public interface IDestructionListener
    {
        /// Called when any joint is about to be destroyed due
        /// to the destruction of one of its attached bodies.
        void SayGoodbye(Joint joint);

        /// Called when any fixture is about to be destroyed due
        /// to the destruction of its parent body.
        void SayGoodbye(Fixture fixture);
    }
}