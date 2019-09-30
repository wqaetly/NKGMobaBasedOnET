// Animancer // Copyright 2019 Kybernetik //

using UnityEngine;

namespace Animancer.FSM
{
    /// <summary>
    /// A state that can be used in a <see cref="StateMachine{TState}"/>.
    /// </summary>
    public interface IState<TState> where TState : class, IState<TState>
    {
        /// <summary>Determines whether this state can be entered.</summary>
        bool CanEnterState(TState previousState);

        /// <summary>Determines whether this state can be exited.</summary>
        bool CanExitState(TState nextState);

        /// <summary>Called when this state is entered.</summary>
        void OnEnterState();

        /// <summary>Called when this state is exited.</summary>
        void OnExitState();
    }

    /************************************************************************************************************************/

    /// <summary>
    /// A type of <see cref="IState{TState}"/> that knows which <see cref="StateMachine{TState}"/> it is used in so it
    /// can be used with various extension methods in <see cref="StateExtensions"/>.
    /// </summary>
    public interface IOwnedState<TState> : IState<TState> where TState : class, IState<TState>
    {
        /// <summary>
        /// The <see cref="StateMachine{TState}"/> that this state is used in.
        /// </summary>
        StateMachine<TState> OwnerStateMachine { get; }
    }

    /************************************************************************************************************************/

    /// <summary>
    /// Various extension methods for <see cref="IOwnedState{TState}"/>.
    /// </summary>
    [HelpURL(AnimancerPlayable.APIDocumentationURL + ".FSM/StateExtensions")]
    public static class StateExtensions
    {
        /************************************************************************************************************************/

        /// <summary>
        /// Checks if the specified 'state' is the <see cref="StateMachine{TState}.CurrentState"/> in its
        /// <see cref="IOwnedState{TState}.OwnerStateMachine"/>.
        /// </summary>
        public static bool IsCurrentState<TState>(this TState state) where TState : class, IOwnedState<TState>
        {
            return state.OwnerStateMachine.CurrentState == state;
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Checks if it is currently possible to enter the specified 'state'. This requires
        /// <see cref="IState{TState}.CanExitState"/> on the <see cref="StateMachine{TState}.CurrentState"/> and
        /// <see cref="IState{TState}.CanEnterState"/> on the specified 'state' to both return true.
        /// </summary>
        public static bool CanEnterState<TState>(this TState state) where TState : class, IOwnedState<TState>
        {
            return state.OwnerStateMachine.CanSetState(state);
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Attempts to enter the specified 'state' and returns true if successful.
        /// <para></para>
        /// This method returns true immediately if the specified 'state' is already the
        /// <see cref="StateMachine{TState}.CurrentState"/>. To allow directly re-entering the same state, use
        /// <see cref="TryReEnterState"/> instead.
        /// </summary>
        public static bool TryEnterState<TState>(this TState state) where TState : class, IOwnedState<TState>
        {
            return state.OwnerStateMachine.TrySetState(state);
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Attempts to enter the specified 'state' and returns true if successful.
        /// <para></para>
        /// This method does not check if the 'state' is already the <see cref="StateMachine{TState}.CurrentState"/>.
        /// To do so, use <see cref="TryEnterState"/> instead.
        /// </summary>
        public static bool TryReEnterState<TState>(this TState state) where TState : class, IOwnedState<TState>
        {
            return state.OwnerStateMachine.TryResetState(state);
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Calls <see cref="IState{TState}.OnExitState"/> on the <see cref="StateMachine{TState}.CurrentState"/> then
        /// changes to the specified 'state' and calls <see cref="IState{TState}.OnEnterState"/> on it.
        /// <para></para>
        /// This method does not check <see cref="IState{TState}.CanExitState"/> or
        /// <see cref="IState{TState}.CanEnterState"/>. To do that, you should use <see cref="TrySetState"/> instead.
        /// </summary>
        public static void ForceEnterState<TState>(this TState state) where TState : class, IOwnedState<TState>
        {
            state.OwnerStateMachine.ForceSetState(state);
        }

        /************************************************************************************************************************/
#pragma warning disable CS1587 // XML comment is not placed on a valid language element.
        // Copy this #region into a class which implements IOwnedState to give it the state extension methods as regular members.
        ///************************************************************************************************************************/
        //#region State Extensions
        ///************************************************************************************************************************/

        ///// <summary>
        ///// Checks if this state is the <see cref="StateMachine{TState}.CurrentState"/> in its
        ///// <see cref="IOwnedState{TState}.OwnerStateMachine"/>.
        ///// </summary>
        //public bool IsCurrentState()
        //{
        //    return OwnerStateMachine.CurrentState == this;
        //}

        ///************************************************************************************************************************/

        ///// <summary>
        ///// Checks if it is currently possible to enter this state. This requires
        ///// <see cref="IState{TState}.CanExitState"/> on the <see cref="StateMachine{TState}.CurrentState"/> and
        ///// <see cref="IState{TState}.CanEnterState"/> on this state to both return true.
        ///// </summary>
        //public bool CanEnterState()
        //{
        //    return OwnerStateMachine.CanSetState(this);
        //}

        ///************************************************************************************************************************/

        ///// <summary>
        ///// Attempts to enter this state and returns true if successful.
        ///// <para></para>
        ///// This method returns true immediately if this state is already the
        ///// <see cref="StateMachine{TState}.CurrentState"/>. To allow directly re-entering the same state, use
        ///// <see cref="TryReEnterState"/> instead.
        ///// </summary>
        //public bool TryEnterState()
        //{
        //    return OwnerStateMachine.TrySetState(this);
        //}

        ///************************************************************************************************************************/

        ///// <summary>
        ///// Attempts to enter this state and returns true if successful.
        ///// <para></para>
        ///// This method does not check if this state is already the <see cref="StateMachine{TState}.CurrentState"/>.
        ///// To do so, use <see cref="TryEnterState"/> instead.
        ///// </summary>
        //public static bool TryReEnterState()
        //{
        //    return OwnerStateMachine.TryResetState(this);
        //}

        ///************************************************************************************************************************/

        ///// <summary>
        ///// Calls <see cref="IState{TState}.OnExitState"/> on the <see cref="StateMachine{TState}.CurrentState"/> then
        ///// changes to the this state and calls <see cref="IState{TState}.OnEnterState"/> on it.
        ///// <para></para>
        ///// This method does not check <see cref="IState{TState}.CanExitState"/> or
        ///// <see cref="IState{TState}.CanEnterState"/>. To do that, you should use <see cref="TrySetState"/> instead.
        ///// </summary>
        //public void ForceEnterState()
        //{
        //    OwnerStateMachine.ForceSetState(this);
        //}

        ///************************************************************************************************************************/
        //#endregion
        ///************************************************************************************************************************/
    }
}
