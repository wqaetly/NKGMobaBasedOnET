// Animancer // Copyright 2019 Kybernetik //

using UnityEngine;

namespace Animancer.Examples
{
    /// <summary>
    /// An object for a character to look at using Inverse Kinematics (IK).
    /// </summary>
    [AddComponentMenu("Animancer/Examples/IK Puppet Look Target")]
    [HelpURL(AnimancerPlayable.APIDocumentationURL + ".Examples/IKPuppetLookTarget")]
    public sealed class IKPuppetLookTarget : MonoBehaviour
    {
        /************************************************************************************************************************/

        [SerializeField, Range(0, 1)]
        private float _Weight = 1;

        [SerializeField, Range(0, 1)]
        private float _BodyWeight = 0.3f;

        [SerializeField, Range(0, 1)]
        private float _HeadWeight = 0.6f;

        [SerializeField, Range(0, 1)]
        private float _EyesWeight = 1;

        [SerializeField, Range(0, 1)]
        private float _ClampWeight = 0.5f;

        /************************************************************************************************************************/

        public void UpdateAnimatorIK(Animator animator)
        {
            animator.SetLookAtWeight(_Weight, _BodyWeight, _HeadWeight, _EyesWeight, _ClampWeight);
            animator.SetLookAtPosition(transform.position);
        }

        /************************************************************************************************************************/
    }
}
