// Animancer // Copyright 2019 Kybernetik //

#pragma warning disable CS0618 // Type or member is obsolete (for MixerStates in Animancer Lite).
#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value.

using System;
using UnityEngine;

namespace Animancer.Examples
{
    /// <summary>
    /// Expands the <see cref="SpiderBotBasic"/> example with a <see cref="MixerState.Serializable2D"/> and
    /// <see cref="Rigidbody"/> to allow the bot to move in any direction.
    /// </summary>
    [AddComponentMenu("Animancer/Examples/Spider Bot Advanced")]
    [HelpURL(AnimancerPlayable.APIDocumentationURL + ".Examples/SpiderBotAdvanced")]
    public sealed class SpiderBotAdvanced : MonoBehaviour, IAnimancerClipSource
    {
        /************************************************************************************************************************/

        [Header("Physics")]

        [SerializeField]
        private Rigidbody _Body;

        [SerializeField]
        private float _MovementSpeed = 1.5f;

        [SerializeField]
        private float _SprintMultiplier = 2;

        /************************************************************************************************************************/

        [Header("Animation")]

        [SerializeField]
        private AnimancerComponent _Animancer;
        public AnimancerComponent Animancer { get { return _Animancer; } }

        [SerializeField]
        private AnimationClip _WakeUp;

        [SerializeField]
        private MixerState.Serializable2D _Locomotion;

        [SerializeField]
        private float _AnimationAcceleration = 3;

        private bool _IsAwake;
        private Action _PauseGraph;
        private Action _FadeToMovement;

        /************************************************************************************************************************/

        private void Awake()
        {
            // Start paused at the beginning of the animation.
            _Animancer.Play(_WakeUp);
            _Animancer.Playable.Evaluate();
            _Animancer.Playable.PauseGraph();

            // Create the locomotion state but don't do anything with it yet.
            _Animancer.GetOrCreateState(_Locomotion);

            // Cache the delegates we will use with the OnEnd event so we don't allocate garbage every time.
            _PauseGraph = _Animancer.Playable.PauseGraph;
            _FadeToMovement = () => _Animancer.Transition(_Locomotion);
        }

        /************************************************************************************************************************/

        private void Update()
        {
            // Get the movement and clamp it so we don't move faster diagonally.
            var input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            input = Vector2.ClampMagnitude(input, 1);

            var isMoving = input != Vector2.zero;

            var isSprinting = Input.GetButton("Fire3");// Left Shift by default.

            // Gradually move the movement parameter towards the target value.
            var movement = Vector2.MoveTowards(_Locomotion.State.Parameter, input, _AnimationAcceleration * Time.deltaTime);
            _Locomotion.State.Parameter = movement;
            _Locomotion.State.Speed = (isMoving && isSprinting) ? _SprintMultiplier : 1;

            if (isMoving || isSprinting)
            {
                if (!_IsAwake)
                {
                    _IsAwake = true;

                    // Make sure the graph is unpaused (because we pause it when going back to sleep).
                    _Animancer.Playable.UnpauseGraph();

                    // Play the wake up animation forwards.
                    var state = _Animancer.CrossFade(_WakeUp);
                    state.Speed = 1;

                    // When finished waking up, start moving.
                    state.OnEnd = _FadeToMovement;
                }
            }
            else
            {
                if (_IsAwake)
                {
                    _IsAwake = false;

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
        }

        /************************************************************************************************************************/

        private void FixedUpdate()
        {
            var movement = _Locomotion.State.Parameter * _Locomotion.State.Speed * _MovementSpeed;
            _Body.velocity = new Vector3(movement.x, 0, movement.y);
        }

        /************************************************************************************************************************/
    }
}
