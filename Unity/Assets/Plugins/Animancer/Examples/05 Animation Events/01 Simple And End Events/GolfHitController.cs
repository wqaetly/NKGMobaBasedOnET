// Animancer // Copyright 2019 Kybernetik //

#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value.

using System;
using UnityEngine;

namespace Animancer.Examples
{
    /// <summary>
    /// An example of how to use the <see cref="EventfulAnimancerComponent.onEvent"/> callback.
    /// </summary>
    [AddComponentMenu("Animancer/Examples/Golf Hit Controller")]
    [HelpURL(AnimancerPlayable.APIDocumentationURL + ".Examples/GolfHitController")]
    public sealed class GolfHitController : MonoBehaviour
    {
        /************************************************************************************************************************/

        [SerializeField]
        private EventfulAnimancerComponent _Animancer;

        [SerializeField]
        private AnimationClip _Ready;

        [SerializeField]
        private AnimationClip _Swing;

        [SerializeField]
        private AnimationClip _Idle;

        [SerializeField]
        private Rigidbody _Ball;

        [SerializeField]
        private Vector3 _HitVelocity;

        [SerializeField]
        private AudioSource _HitSound;

        /************************************************************************************************************************/

        public enum State
        {
            Ready,
            Swing,
            Idle,
        }

        private State _State;
        private Vector3 _BallStartPosition;

        /************************************************************************************************************************/

        private void OnEnable()
        {
            _BallStartPosition = _Ball.position;

            // Start in the Ready state.
            // The _State is already set to Ready by default because it's the first value in the enum.
            _Animancer.Play(_Ready);
        }

        /************************************************************************************************************************/

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                switch (_State)
                {
                    case State.Ready: StartSwing(); break;
                    case State.Swing: TryCancelSwing(); break;
                    case State.Idle: ReturnToReady(); break;
                    default: throw new ArgumentException("Unhandled State: " + _State);
                }
            }
        }

        /************************************************************************************************************************/

        private void StartSwing()
        {
            _State = State.Swing;

            // Play the swing animation.
            var state = _Animancer.CrossFadeFromStart(_Swing);
            state.OnEnd = OnSwingEnd;

            // When the Animation Event with the function name "Event" occurs:
            _Animancer.onEvent.Set(state, (animationEvent) =>
            {
                // In a real game you would calculate the hit velocity based on player input.
                _Ball.isKinematic = false;
                _Ball.velocity = _HitVelocity;

                _HitSound.Play();
            });

            // If the swing animation doesn't have an event with that function name,
            // trying to assign a callback to that event will log a warning.
        }

        /************************************************************************************************************************/

        private void TryCancelSwing()
        {
            // If the ball hasn't been hit yet, clicking again cancels the swing.
            if (_Ball.isKinematic)
            {
                _State = State.Ready;
                _Animancer.CrossFade(_Ready);
            }
        }

        /************************************************************************************************************************/

        private void OnSwingEnd()
        {
            _State = State.Idle;

            // Since the swing animation used for this example has an 'End' event, we can allow it to determine how
            // long the fade should take instead of having this script pick a value (or using the default 0.3 seconds).
            var fadeDuration = AnimancerPlayable.GetFadeOutDuration();
            _Animancer.CrossFade(_Idle, fadeDuration);
        }

        /************************************************************************************************************************/

        private void ReturnToReady()
        {
            _State = State.Ready;

            _Ball.isKinematic = true;
            _Ball.position = _BallStartPosition;

            _Animancer.CrossFade(_Ready);
        }

        /************************************************************************************************************************/
    }
}
