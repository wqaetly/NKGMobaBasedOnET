// Animancer // Copyright 2019 Kybernetik //

using UnityEngine;

namespace Animancer
{
    /// <summary>
    /// Interface for components that manage an <see cref="AnimancerPlayable"/>.
    /// </summary>
    /// <remarks>
    /// This interface allows Animancer Lite to reference an <see cref="AnimancerComponent"/> inside the pre-compiled
    /// DLL while allowing that component to remain outside as a regular script. Otherwise everything would need to be
    /// in the DLL which would cause Unity to lose all the script references when upgrading from Animancer Lite to Pro.
    /// </remarks>
    public interface IAnimancerComponent
    {
        /************************************************************************************************************************/
#pragma warning disable IDE1006 // Naming Styles.
        /************************************************************************************************************************/

        /// <summary>Indicates whether this component will be updated.</summary>
        bool enabled { get; }

        /// <summary>The <see cref="GameObject"/> this component is attached to.</summary>
        GameObject gameObject { get; }

        /************************************************************************************************************************/
#pragma warning restore IDE1006 // Naming Styles.
        /************************************************************************************************************************/

        /// <summary>The <see cref="UnityEngine.Animator"/> component which this script controls.</summary>
        Animator Animator { get; }

        /// <summary>The internal system which manages the playing animations.</summary>
        AnimancerPlayable Playable { get; }

        /// <summary>Indicates whether the <see cref="Playable"/> has been initialised (is not null).</summary>
        bool IsPlayableInitialised { get; }

        /// <summary>
        /// Determines when animations are updated and which time source is used. This property is mainly a wrapper
        /// around the <see cref="Animator.updateMode"/>.
        /// </summary>
        AnimatorUpdateMode UpdateMode { get; set; }

        /************************************************************************************************************************/

        /// <summary>
        /// Gets an existing state that plays the 'clip' if there is one. Otherwise creates a new state to play it.
        /// </summary>
        AnimancerState GetOrCreateState(AnimationClip clip, int layerIndex = 0, bool allowSetClip = false);

        /************************************************************************************************************************/
#if UNITY_EDITOR
        /************************************************************************************************************************/

        /// <summary>[Editor-Only] The name of the serialized backing field for the <see cref="Animator"/> property.</summary>
        string AnimatorFieldName { get; }

        /// <summary>[Editor-Only] The <see cref="UpdateMode"/> what was first used when this script initialised.</summary>
        AnimatorUpdateMode? InitialUpdateMode { get; }

        /************************************************************************************************************************/
#endif
        /************************************************************************************************************************/
    }
}
