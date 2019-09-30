// Animancer // Copyright 2019 Kybernetik //

using UnityEngine;

namespace Animancer.Examples
{
    /// <summary>An object that can be interacted with.</summary>
    public interface IInteractable
    {
        /************************************************************************************************************************/

        void Interact();

        /************************************************************************************************************************/
    }

    /// <summary>
    /// Attempts to interact with whatever <see cref="IInteractable"/> the cursor is pointing at when the user clicks
    /// the mouse.
    /// </summary>
    [AddComponentMenu("Animancer/Examples/Click To Interact")]
    [HelpURL(AnimancerPlayable.APIDocumentationURL + ".Examples/ClickToInteract")]
    public sealed class ClickToInteract : MonoBehaviour
    {
        /************************************************************************************************************************/

        private void Update()
        {
            if (!Input.GetMouseButtonDown(0))
                return;

            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit raycastHit;
            if (Physics.Raycast(ray, out raycastHit))
            {
                var interactable = raycastHit.collider.GetComponentInParent<IInteractable>();
                if (interactable != null)
                    interactable.Interact();
            }
        }

        /************************************************************************************************************************/
    }
}
