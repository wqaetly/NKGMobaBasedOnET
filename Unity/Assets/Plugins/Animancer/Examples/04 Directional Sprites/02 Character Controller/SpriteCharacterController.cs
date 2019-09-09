// Animancer // Copyright 2019 Kybernetik //

#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value.

using UnityEngine;

namespace Animancer.Examples
{
    /// <summary>
    /// A more complex version of the <see cref="SpriteMovementController"/> which adds running and pushing animations
    /// as well as the ability to actually move around.
    /// </summary>
    [AddComponentMenu("Animancer/Examples/Sprite Character Controller")]
    [HelpURL(AnimancerPlayable.APIDocumentationURL + ".Examples/SpriteCharacterController")]
    public sealed class SpriteCharacterController : MonoBehaviour
    {
        /************************************************************************************************************************/

        [Header("Physics")]

        [SerializeField]
        private CapsuleCollider2D _Collider;

        [SerializeField]
        private Rigidbody2D _Rigidbody;

        [SerializeField]
        private float _WalkSpeed = 1;

        [SerializeField]
        private float _RunSpeed = 2;

        /************************************************************************************************************************/

        [Header("Animations")]

        [SerializeField]
        private AnimancerComponent _Animancer;

        [SerializeField]
        private DirectionalAnimationSet _Idle;

        [SerializeField]
        private DirectionalAnimationSet _Walk;

        [SerializeField]
        private DirectionalAnimationSet _Run;

        [SerializeField]
        private DirectionalAnimationSet _Push;

        /************************************************************************************************************************/

        public enum State
        {
            Idle,
            Walk,
            Run,
            Push,
        }

        /************************************************************************************************************************/

        private Vector2 _Movement;
        private State _State;
        private Vector2 _Facing = Vector2.down;

        /************************************************************************************************************************/

        private void Awake()
        {
            _Animancer.Play(_Idle.GetClip(_Facing));
        }

        /************************************************************************************************************************/

        private void Update()
        {
            _Movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            if (_Movement != Vector2.zero)
            {
                UpdateMovementState();

                _Facing = _Movement;
                var animationSet = GetCurrentAnimationSet();
                _Animancer.Play(animationSet.GetClip(_Facing));

                // Snap the movement to the exact directions we have animations for.
                // When using DirectionalAnimationSets this means the character will only move up/right/down/left.
                // But DirectionalAnimationSet8s will allow diagonal movement as well.
                _Movement = animationSet.Snap(_Movement);
                _Movement = Vector2.ClampMagnitude(_Movement, 1);
            }
            else
            {
                _State = State.Idle;
                _Animancer.Play(_Idle.GetClip(_Facing));
            }
        }

        /************************************************************************************************************************/

        // Pre-allocate an array of contact points so Unity doesn't need to allocate a new one every time we call
        // _Collider.GetContacts. This example won't ever have more than 4 contact points, but we might consider a
        // higher number in a real game.
        private static readonly ContactPoint2D[] Contacts = new ContactPoint2D[4];

        private void UpdateMovementState()
        {
            var contactCount = _Collider.GetContacts(Contacts);
            for (int i = 0; i < contactCount; i++)
            {
                // If we are moving directly towards an object (or within 30 degrees of it), we are pushing it.
                if (Vector2.Angle(Contacts[i].normal, _Movement) > 180 - 30)
                {
                    _State = State.Push;
                    return;
                }
            }

            if (Input.GetButton("Fire3"))// Left Shift by default.
                _State = State.Run;
            else
                _State = State.Walk;
        }

        /************************************************************************************************************************/

        private DirectionalAnimationSet GetCurrentAnimationSet()
        {
            switch (_State)
            {
                case State.Idle: return _Idle;
                case State.Walk: return _Walk;
                case State.Run: return _Run;
                case State.Push: return _Push;
                default: throw new System.ArgumentException("Unhandled State: " + _State);
            }
        }

        /************************************************************************************************************************/

        private void FixedUpdate()
        {
            var speed = _State == State.Run ? _RunSpeed : _WalkSpeed;
            _Rigidbody.velocity = _Movement.normalized * speed;
        }

        /************************************************************************************************************************/
    }
}
