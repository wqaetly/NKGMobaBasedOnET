// Animancer // Copyright 2019 Kybernetik //

#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value.

using Animancer.FSM;
using UnityEngine;

namespace Animancer.Examples
{
    /// <summary>
    /// A state for a <see cref="BasicCreature"/> which simply plays an animation.
    /// </summary>
    /// <remarks>
    /// This class would normally just be called "CreatureState", but this name was chosen to avoid conflict with the
    /// other examples.
    /// </remarks>
    [AddComponentMenu("Animancer/Examples/Basic Creature State")]
    [HelpURL(AnimancerPlayable.APIDocumentationURL + ".Examples/BasicCreatureState")]
    public sealed class BasicCreatureState : StateBehaviour<BasicCreatureState>, IAnimancerClipSource
    {
        /************************************************************************************************************************/

        [SerializeField]
        private BasicCreature _Creature;

        [SerializeField]
        private AnimationClip _Animation;

        /************************************************************************************************************************/

        private void OnEnable()
        {
            var state = _Creature.Animancer.CrossFade(_Animation);
            if (!_Animation.isLooping)
                state.OnEnd = _Creature.ForceIdleState;
        }

        /************************************************************************************************************************/

        public AnimancerComponent Animancer { get { return _Creature.Animancer; } }

        /************************************************************************************************************************/
    }
}
