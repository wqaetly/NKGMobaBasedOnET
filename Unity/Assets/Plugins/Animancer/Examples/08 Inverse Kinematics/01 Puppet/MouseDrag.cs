// Animancer // Copyright 2019 Kybernetik //

#pragma warning disable IDE0018 // Inline variable declaration.

using System;
using UnityEngine;

namespace Animancer.Examples
{
    /// <summary>
    /// Allows the user to drag any object with a collider around on screen with the mouse.
    /// </summary>
    [AddComponentMenu("Animancer/Examples/Mouse Drag")]
    [HelpURL(AnimancerPlayable.APIDocumentationURL + ".Examples/MouseDrag")]
    public sealed class MouseDrag : MonoBehaviour
    {
        /************************************************************************************************************************/

        private Transform _Dragging;

        /************************************************************************************************************************/

        private void Update()
        {
            // On click, do a raycast and grab whatever it hits.
            if (Input.GetMouseButtonDown(0))
            {
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    _Dragging = hit.transform;
                }
            }
            // While holding the button, move the object in line with the mouse ray.
            else if (_Dragging != null && Input.GetMouseButton(0))
            {
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                var distance =
                    Vector3.Dot(_Dragging.position, ray.direction) -
                    Vector3.Dot(ray.origin, ray.direction);
                _Dragging.position = ray.origin + ray.direction * distance;
            }
            else
            {
                _Dragging = null;
            }
        }

        /************************************************************************************************************************/
    }
}
