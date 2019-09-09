// Animancer // Copyright 2019 Kybernetik //

namespace Animancer
{
    /// <summary>
    /// Interface for objects that need to be updated earlier than others.
    /// </summary>
    public interface IEarlyUpdate
    {
        /************************************************************************************************************************/

        /// <summary>
        /// The index of the port on the parent's <see cref="IAnimationMixer.Playable"/> which this node is connected to.
        /// <para></para>
        /// A negative value indicates that it is not assigned to a port.
        /// </summary>
        int PortIndex { get; }

        /// <summary>
        /// Called by <see cref="AnimancerPlayable"/> before <see cref="AnimancerNode.Update"/>.
        /// </summary>
        void EarlyUpdate(out bool needsMoreUpdates);

        /************************************************************************************************************************/
    }
}
