// Animancer // Copyright 2019 Kybernetik //

#if UNITY_EDITOR

#pragma warning disable IDE0018 // Inline variable declaration.

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Animancer.Editor
{
    /// <summary>[Editor-Only]
    /// The type of object an <see cref="AnimationClip"/> can animate.
    /// </summary>
    public enum AnimationType
    {
        /// <summary>Unable to determine a type.</summary>
        None,

        /// <summary>A Humanoid rig.</summary>
        Humanoid,

        /// <summary>A Generic rig.</summary>
        Generic,

        /// <summary>
        /// An animation that modifies a <see cref="SpriteRenderer.sprite"/>.
        /// <para></para>
        /// This is technically a <see cref="Generic"/> animation.
        /// </summary>
        Sprite,
    }

    public static partial class AnimancerEditorUtilities
    {
        /************************************************************************************************************************/

        private static Dictionary<AnimationClip, bool> _ClipToIsSprite;

        /// <summary>
        /// Attempts to determine the <see cref="AnimationType"/> of the specified 'clip'.
        /// </summary>
        public static AnimationType GetAnimationType(AnimationClip clip)
        {
            if (clip == null)
                return AnimationType.None;

            if (clip.isHumanMotion)
                return AnimationType.Humanoid;

            if (_ClipToIsSprite == null)
                _ClipToIsSprite = new Dictionary<AnimationClip, bool>();

            bool isSprite;
            if (!_ClipToIsSprite.TryGetValue(clip, out isSprite))
            {
                var bindings = AnimationUtility.GetObjectReferenceCurveBindings(clip);
                for (int i = 0; i < bindings.Length; i++)
                {
                    if (bindings[i].type == typeof(SpriteRenderer))
                    {
                        isSprite = true;
                        break;
                    }
                }

                _ClipToIsSprite.Add(clip, isSprite);
            }

            return isSprite ? AnimationType.Sprite : AnimationType.Generic;
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Attempts to determine the <see cref="AnimationType"/> of the specified 'animator'.
        /// </summary>
        public static AnimationType GetAnimationType(Animator animator)
        {
            if (animator == null)
                return AnimationType.None;

            if (animator.isHuman)
                return AnimationType.Humanoid;

            return animator.GetComponent<SpriteRenderer>() != null ?
                AnimationType.Sprite :
                AnimationType.Generic;
        }

        /************************************************************************************************************************/
    }
}

#endif
