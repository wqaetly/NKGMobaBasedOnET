// Animancer // Copyright 2019 Kybernetik //

#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value.

using UnityEngine;

namespace Animancer.Examples
{
    /// <summary>
    /// Responds to Animation Events called "Footstep" by playing a sound randomly selected from an array.
    /// </summary>
    [AddComponentMenu("Animancer/Examples/Footstep Events")]
    [HelpURL(AnimancerPlayable.APIDocumentationURL + ".Examples/FootstepEvents")]
    public sealed class FootstepEvents : MonoBehaviour
    {
        /************************************************************************************************************************/

        [SerializeField]
        private AudioSource[] _FootSources;

        [SerializeField]
        private AudioClip[] _Sounds;

        /************************************************************************************************************************/

        private void Footstep(int foot)
        {
            // Pick a random sound and play it on the specified foot.
            var source = _FootSources[foot];
            source.clip = _Sounds[Random.Range(0, _Sounds.Length)];
            source.Play();

            // A more complex system could have different footstep sounds depending on the surface being stepped on.
            // This could be done by raycasting down from the feet and determining which sound to use based on the
            // sharedMaterial of the ground's Renderer or even a simple script that holds an enum indicating the type.
        }

        /************************************************************************************************************************/
    }
}
