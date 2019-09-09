// Animancer // Copyright 2019 Kybernetik //

#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value.

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Animancer.Examples
{
    /// <summary>
    /// Plays through a series of animations in a sequence using a coroutine.
    /// </summary>
    [AddComponentMenu("Animancer/Examples/Sequence Coroutine")]
    [HelpURL(AnimancerPlayable.APIDocumentationURL + ".Examples/SequenceCoroutine")]
    public sealed class SequenceCoroutine : MonoBehaviour
    {
        /************************************************************************************************************************/

        /// <summary>
        /// A <see cref="ClipState.Serializable"/> with a <see cref="PlayDuration"/> to determine how long it should
        /// be played for during the sequence.
        /// </summary>
        [Serializable]
        public class SequenceClip : ClipState.Serializable
        {
            /************************************************************************************************************************/

            [Tooltip("Determines how long this animation will play for during the sequence" +
                " (zero will play through the full animation once)")]
            [SerializeField]
            private float _PlayDuration;

            /// <summary>
            /// Determines how long this animation will play for during the sequence (zero will play through the full
            /// animation once).
            /// </summary>
            public float PlayDuration
            {
                get { return _PlayDuration; }
                set { _PlayDuration = value; }
            }

            /************************************************************************************************************************/
        }

        /************************************************************************************************************************/

        [SerializeField]
        private AnimancerComponent _Animancer;

        [SerializeField]
        private Text _Text;

        [SerializeField]
        private SequenceClip[] _Animations;

        /************************************************************************************************************************/

        private void OnEnable()
        {
            Play(_Animations[0]);
        }

        /************************************************************************************************************************/

        public void PlaySequence()
        {
            StopAllCoroutines();
            StartCoroutine(CoroutineAnimationSequence());
        }

        private IEnumerator CoroutineAnimationSequence()
        {
            for (int i = 1; i < _Animations.Length; i++)
            {
                var animation = _Animations[i];
                var state = Play(animation);

                if (animation.PlayDuration > 0)
                    yield return new WaitForSeconds(animation.PlayDuration);
                else
                    yield return state;
            }

            Play(_Animations[0]);
        }

        /************************************************************************************************************************/

        private AnimancerState Play(ClipState.Serializable animation)
        {
            _Text.text = animation.Clip.name;
            return _Animancer.Transition(animation);
        }

        /************************************************************************************************************************/
    }
}
