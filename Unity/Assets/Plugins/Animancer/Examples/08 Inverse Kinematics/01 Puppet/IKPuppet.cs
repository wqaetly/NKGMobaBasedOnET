// Animancer // Copyright 2019 Kybernetik //

#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value.

using UnityEngine;

namespace Animancer.Examples
{
    /// <summary>
    /// Demonstrates how to use Unity's Inverse Kinematics (IK) system to move a character's limbs.
    /// </summary>
    [AddComponentMenu("Animancer/Examples/IK Puppet")]
    [HelpURL(AnimancerPlayable.APIDocumentationURL + ".Examples/IKPuppet")]
    public sealed class IKPuppet : MonoBehaviour
    {
        /************************************************************************************************************************/

        [SerializeField]
        private AnimancerComponent _Animancer;

        [SerializeField]
        private Transform _BodyTarget;

        [SerializeField]
        private IKPuppetLookTarget _LookTarget;

        [SerializeField]
        private IKPuppetTarget[] _IKTargets;

        /************************************************************************************************************************/

        private void Awake()
        {
            // Enable the OnAnimatorIK message.
            _Animancer.GetLayer(0).ApplyAnimatorIK = true;
        }

        /************************************************************************************************************************/

        private void OnAnimatorIK(int layerIndex)
        {
            _Animancer.Animator.bodyPosition = _BodyTarget.position;
            _Animancer.Animator.bodyRotation = _BodyTarget.rotation;

            _LookTarget.UpdateAnimatorIK(_Animancer.Animator);

            for (int i = 0; i < _IKTargets.Length; i++)
            {
                _IKTargets[i].UpdateAnimatorIK(_Animancer.Animator);
            }
        }

        /************************************************************************************************************************/
    }
}
