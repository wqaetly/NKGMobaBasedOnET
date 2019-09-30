// Animancer // Copyright 2019 Kybernetik //

#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value.

using UnityEngine;

namespace Animancer.Examples
{
    /// <summary>
    /// Demonstrates the differences between various ways of playing and fading between animations.
    /// </summary>
    [AddComponentMenu("Animancer/Examples/Playing and Fading")]
    [HelpURL(AnimancerPlayable.APIDocumentationURL + ".Examples/PlayingAndFading")]
    public sealed class PlayingAndFading : MonoBehaviour
    {
        /************************************************************************************************************************/

        [SerializeField]
        private AnimancerComponent _Animancer;

        [SerializeField]
        private AnimationClip _Idle;

        [SerializeField]
        private AnimationClip _Action;

        /************************************************************************************************************************/

        private void OnEnable()
        {
            _Animancer.Play(_Idle);
        }

        /************************************************************************************************************************/

        public void Play()
        {
            _Animancer.Play(_Action);
        }

        public void PlayThenIdle()
        {
            _Animancer.Play(_Action).OnEnd = () => _Animancer.Play(_Idle);
        }

        public void PlayFromStart()
        {
            _Animancer.Play(_Action).Time = 0;
        }

        public void PlayFromStartThenIdle()
        {
            var state = _Animancer.Play(_Action);
            state.Time = 0;
            state.OnEnd = () => _Animancer.Play(_Idle);
        }

        /************************************************************************************************************************/

        public void CrossFade()
        {
            _Animancer.CrossFade(_Action);

            // The optional 'fadeDuration' parameter specifies how long it will take (in seconds).
            // _Animancer.CrossFade(_Action, 0.5f);// This will take 0.5 seconds.
            // If not specified, the default duration is 0.3 seconds.
        }

        public void CrossFadeThenIdle()
        {
            _Animancer.CrossFade(_Action).OnEnd = () => _Animancer.CrossFade(_Idle);
        }

        public void BadCrossFadeFromStart()
        {
            _Animancer.CrossFade(_Action).Time = 0;
        }

        public void GoodCrossFadeFromStart()
        {
            _Animancer.CrossFadeFromStart(_Action);
        }

        public void GoodCrossFadeFromStartThenIdle()
        {
            _Animancer.CrossFadeFromStart(_Action).OnEnd = () => _Animancer.CrossFade(_Idle);
        }

        /************************************************************************************************************************/
    }
}
