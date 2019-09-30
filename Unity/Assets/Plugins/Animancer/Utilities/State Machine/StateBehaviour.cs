// Animancer // Copyright 2019 Kybernetik //

using UnityEngine;

namespace Animancer.FSM
{
    /// <summary>
    /// Base class for <see cref="MonoBehaviour"/> states to be used in a <see cref="StateMachine{TState}"/>.
    /// </summary>
    [HelpURL(AnimancerPlayable.APIDocumentationURL + ".FSM/StateBehaviour_1")]
    public abstract class StateBehaviour<TState> : MonoBehaviour, IState<TState>
        where TState : StateBehaviour<TState>
    {
        /************************************************************************************************************************/

        /// <summary>
        /// Determines whether the <see cref="StateMachine{TState}"/> can enter this state. Always returns true
        /// unless overridden.
        /// </summary>
        public virtual bool CanEnterState(TState previousState) { return true; }

        /// <summary>
        /// Determines whether the <see cref="StateMachine{TState}"/> can exit this state. Always returns true
        /// unless overridden.
        /// </summary>
        public virtual bool CanExitState(TState nextState) { return true; }

        /************************************************************************************************************************/

        /// <summary>
        /// Asserts that this component isn't already enabled, then enable it.
        /// </summary>
        public virtual void OnEnterState()
        {
            Debug.Assert(!enabled, this + " was already enabled when entering its state", this);
            enabled = true;
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Asserts that this component isn't already disabled, then disable it.
        /// </summary>
        public virtual void OnExitState()
        {
            if (this == null) return;

            Debug.Assert(enabled, this + " was already disabled when exiting its state", this);
            enabled = false;
        }

        /************************************************************************************************************************/

#if UNITY_EDITOR
        /// <summary>[Editor-Only]
        /// Called by the Unity Editor when this component is first added (in edit mode) and whenever the Reset command
        /// is executed from its context menu.
        /// <para></para>
        /// States start disabled and only the current state gets enabled at runtime.
        /// </summary>
        protected virtual void Reset()
        {
            if (UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode)
                return;

            enabled = false;
        }
#endif

        /************************************************************************************************************************/
    }
}
