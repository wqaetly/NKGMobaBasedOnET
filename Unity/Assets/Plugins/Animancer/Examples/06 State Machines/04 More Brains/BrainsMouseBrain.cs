// Animancer // Copyright 2019 Kybernetik //

#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value.

using Animancer.FSM;
using UnityEngine;

namespace Animancer.Examples
{
    /// <summary>
    /// A <see cref="BrainsCreatureBrain"/> which controls the creature using mouse input.
    /// </summary>
    /// <remarks>
    /// This class would normally just be called "MouseBrain", but this name was chosen to avoid conflict with the
    /// other examples.
    /// </remarks>
    [AddComponentMenu("Animancer/Examples/Brains Mouse Brain")]
    [HelpURL(AnimancerPlayable.APIDocumentationURL + ".Examples/BrainsMouseBrain")]
    public sealed class BrainsMouseBrain : BrainsCreatureBrain
    {
        /************************************************************************************************************************/

        [SerializeField]
        private BrainsCreatureState _Locomotion;

        [SerializeField]
        private float _StopDistance = 0.2f;

        [SerializeField]
        private float _MinRunDistance = 1;

        private Vector3? _Destination;

        /************************************************************************************************************************/

        private void OnEnable()
        {
            _Destination = null;
        }

        /************************************************************************************************************************/

        private void Update()
        {
            UpdateInput();
            UpdateMovement();
        }

        /************************************************************************************************************************/

        private void UpdateInput()
        {
            if (Input.GetMouseButton(0))
            {
                var creaturePosition = Creature.Rigidbody.position;

                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                RaycastHit raycastHit;
                if (Physics.Raycast(ray, out raycastHit))
                {
                    _Destination = raycastHit.point;
                }
                else
                {
                    // If the ray doesn't hit anything, just use the point where it intersects
                    // the XZ plane at the height the creature is currently at.
                    _Destination = CalculateRayTargetXZ(ray, creaturePosition.y);
                }

                IsRunning =
                    _Destination != null &&
                    Vector3.Distance(creaturePosition, _Destination.Value) >= _MinRunDistance;
            }
            else
            {
                IsRunning = false;
            }
        }

        /************************************************************************************************************************/

        public static Vector3? CalculateRayTargetXZ(Ray ray, float y = 0)
        {
            y = ray.origin.y - y;

            // If the ray starts above the target and is pointing up then it will never intersect.
            // Same if it is below and pointing down or if it is perfectly horizontal.
            if (ray.direction.y == 0 || SameSign(y, ray.direction.y))
                return null;

            return ray.origin - ray.direction * (y / ray.direction.y);
        }

        public static bool SameSign(float x, float y)
        {
            return
                (x > 0 && y > 0) ||
                (x < 0 && y < 0) ||
                (x == 0 && y == 0);
        }

        /************************************************************************************************************************/

        private void UpdateMovement()
        {
            if (_Destination != null)
            {
                var fromCurrentToDestination = _Destination.Value - Creature.Rigidbody.position;

                // Vector magnitudes are calculated using Pythagoras' Theorem which involves a square root.
                // Square roots are a much slower operation than simple arithmetic operations.
                // Since we only need to see which is greater, we can compare the squared magnitude and stop distance.
                if (fromCurrentToDestination.sqrMagnitude > _StopDistance * _StopDistance)
                {
                    MovementDirection = fromCurrentToDestination;
                    _Locomotion.TryEnterState();
                    Debug.DrawLine(Creature.Rigidbody.position, _Destination.Value, Color.cyan);
                    return;
                }
            }

            _Destination = null;
            MovementDirection = Vector3.zero;
            Creature.Idle.TryEnterState();
        }

        /************************************************************************************************************************/
    }
}
