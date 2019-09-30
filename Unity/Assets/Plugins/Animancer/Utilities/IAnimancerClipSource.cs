// Animancer // Copyright 2019 Kybernetik //

using UnityEngine;

namespace Animancer
{
    /************************************************************************************************************************/

    /// <summary>
    /// Components that implement this interface can provide additional <see cref="AnimationClip"/>s to the Animation
    /// Window when a particular <see cref="AnimancerComponent"/> is selected.
    /// <para></para>
    /// If <see cref="IAnimationClipSource"/> is also implemented, <see cref="IAnimationClipSource.GetAnimationClips"/>
    /// will be used to gather the clips. Otherwise reflection will be used to automatically gather clips from any
    /// <see cref="AnimationClip"/> fields and any fields which implement that interface.
    /// <para></para>
    /// This interface does nothing before Unity 2018.3.
    /// </summary>
    public interface IAnimancerClipSource
    {
        /// <summary>
        /// The <see cref="AnimancerComponent"/> which this object provides animations for.
        /// </summary>
        AnimancerComponent Animancer { get; }
    }

    /************************************************************************************************************************/
}
