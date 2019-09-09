// Animancer // Copyright 2019 Kybernetik //

using System.Collections.Generic;
using UnityEngine;

namespace Animancer
{
    /// <summary>
    /// Proxy for an identical interface introduced in Unity 2018.3 to have a class provide its own list of
    /// <see cref="AnimationClip"/>s to the Animation Window without an Animator Controller.
    /// </summary>
    public interface IAnimationClipSource
#if UNITY_2018_3_OR_NEWER
        : UnityEngine.IAnimationClipSource
    { }
#else
    {
        /// <summary>
        /// Gathers all the animations associated with this object.
        /// </summary>
        void GetAnimationClips(List<AnimationClip> clips);
    }
#endif
}
