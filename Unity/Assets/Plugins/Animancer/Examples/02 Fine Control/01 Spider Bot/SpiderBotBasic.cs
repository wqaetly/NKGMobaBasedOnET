// Animancer // Copyright 2019 Kybernetik //

#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value.

using System;
using UnityEngine;

namespace Animancer.Examples
{
    /// <summary>
    /// Demonstrates how to play a single "Wake Up" animation forwards to wake up and backwards to go back to sleep.
    /// </summary>
    [AddComponentMenu("Animancer/Examples/Spider Bot Basic")]
    [HelpURL(AnimancerPlayable.APIDocumentationURL + ".Examples/SpiderBotBasic")]
    public sealed class SpiderBotBasic : MonoBehaviour
    {
        /************************************************************************************************************************/

        [SerializeField]
        private AnimancerComponent _Animancer;

        [SerializeField]
        private AnimationClip _WakeUp;

        [SerializeField]
        private AnimationClip _Move;

        private Action _PauseGraph;
        private Action _FadeToMovement;

        /************************************************************************************************************************/

        private void Awake()
        {
            // Start paused at the beginning of the animation.
            _Animancer.Play(_WakeUp);
            _Animancer.Playable.Evaluate();
            _Animancer.Playable.PauseGraph();

            // Cache the delegates we will use with the OnEnd event so we don't allocate garbage every time.
            _PauseGraph = _Animancer.Playable.PauseGraph;
            _FadeToMovement = () => _Animancer.CrossFade(_Move);
        }

        /************************************************************************************************************************/

        private void Update()
        {
            // Wake up and start moving when Space is pressed.
            if (Input.GetKeyDown(KeyCode.Space))
            {
                // Make sure the graph is unpaused (because we pause it when going back to sleep).
                _Animancer.Playable.UnpauseGraph();

                // Play the wake up animation forwards.
                var state = _Animancer.CrossFade(_WakeUp);
                state.Speed = 1;

                // When finished waking up, start moving.
                state.OnEnd = _FadeToMovement;
            }

            // Go back to sleep when Space is released.
            if (Input.GetKeyUp(KeyCode.Space))
            {
                // Play the wake up animation backwards.
                var state = _Animancer.CrossFade(_WakeUp);
                state.Speed = -1;

                // If it was not already playing, start at the end of the animation.
                // Otherwise just play backwards from the current time.
                if (state.Weight == 0 || state.NormalizedTime > 1)
                    state.NormalizedTime = 1;

                // When finished going to sleep, pause the graph.
                state.OnEnd = _PauseGraph;
            }
        }

        /************************************************************************************************************************/
    }
}
