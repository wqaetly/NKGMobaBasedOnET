// Animancer // Copyright 2019 Kybernetik //

#pragma warning disable IDE0018 // Inline variable declaration.

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace Animancer
{
    /// <summary>
    /// Base class for <see cref="Animancer"/> objects with a <see cref="Weight"/> the can fade over time.
    /// </summary>
    public abstract class AnimancerNode : IEnumerable<AnimancerState>, IEnumerator
    {
        /************************************************************************************************************************/
        #region Graph
        /************************************************************************************************************************/

        /// <summary>
        /// The internal struct this state manages in the <see cref="PlayableGraph"/>.
        /// <para></para>
        /// Should be set in the child class constructor. Failure to do so will throw the following exception
        /// throughout the system when using this node: "<see cref="ArgumentException"/>: The playable passed as an
        /// argument is invalid. To create a valid playable, please use the appropriate Create method".
        /// </summary>
        protected internal Playable _Playable;

        /************************************************************************************************************************/

        /// <summary>The <see cref="AnimancerPlayable"/> at the root of the graph.</summary>
        public readonly AnimancerPlayable Root;

        /// <summary>The root <see cref="AnimancerLayer"/> which this node is connected to.</summary>
        public abstract AnimancerLayer Layer { get; }

        /// <summary>
        /// The node which receives the output of this node.
        /// <para></para>
        /// If this is an <see cref="AnimancerLayer"/> connected to the root <see cref="AnimancerPlayable"/> it will
        /// not have a <see cref="Parent"/>.
        /// </summary>
        public abstract AnimancerNode Parent { get; }

        /// <summary>The parent's <see cref="Playable"/>.</summary>
        protected abstract Playable ParentPlayable { get; }

        /// <summary>The parent's <see cref="KeepChildrenConnected"/>.</summary>
        public abstract bool StayConnectedWhenWeightless { get; }

        /************************************************************************************************************************/

        /// <summary>
        /// The index of the port this node is connected to on the parent's <see cref="IAnimationMixer.Playable"/>.
        /// <para></para>
        /// A negative value indicates that it is not assigned to a port.
        /// </summary>
        /// <remarks>
        /// The setter is internal so user defined states can't set it incorrectly. Ideally,
        /// <see cref="AnimancerLayer"/> should be able to set the port in its constructor and
        /// <see cref="AnimancerState.SetParent"/> should also be able to set it, but classes that further inherit from
        /// there should not be able to change it without properly calling that method.
        /// </remarks>
        public int PortIndex { get; internal set; }

        /************************************************************************************************************************/

        /// <summary>Constructs a new <see cref="AnimancerNode"/>.</summary>
        public AnimancerNode(AnimancerPlayable root)
        {

            if (root == null)
                throw new ArgumentNullException("root");

            PortIndex = -1;
            Root = root;
        }

        /************************************************************************************************************************/

        /// <summary>The number of states using this node as their <see cref="AnimancerState.Parent"/>.</summary>
        public virtual int ChildCount { get { return 0; } }

        /// <summary>
        /// Returns the state connected to the specified 'portIndex' as a child of this node.
        /// </summary>
        /// <exception cref="NotSupportedException">Thrown if this node can't have children.</exception>
        public virtual AnimancerState GetChild(int portIndex)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Called when a child is connected with this node as its <see cref="AnimancerState.Parent"/>.
        /// </summary>
        /// <exception cref="NotSupportedException">Thrown if this node can't have children.</exception>
        protected internal virtual void OnAddChild(AnimancerState state)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Called when a child's <see cref="AnimancerState.Parent"/> is changed from this node to something else.
        /// </summary>
        /// <exception cref="NotSupportedException">Thrown if this node can't have children.</exception>
        protected internal virtual void OnRemoveChild(AnimancerState state)
        {
            throw new NotSupportedException();
        }

        /************************************************************************************************************************/

        /// <summary>Connects the 'state' to the 'mixer' at its <see cref="AnimancerNode.PortIndex"/>.</summary>
        /// <exception cref="InvalidOperationException">Thrown if the <see cref="PortIndex"/> was already occupied.</exception>
        protected void OnAddChild(IList<AnimancerState> states, AnimancerState state)
        {
            var portIndex = state.PortIndex;

            if (states[portIndex] != null)
            {
                state.SetParent(null, -1);
                throw new InvalidOperationException(
                    "Tried to add a state to an already occupied port on " + this + ":" +
                    "\n    Port: " + portIndex +
                    "\n    Old State: " + states[portIndex] +
                    "\n    New State: " + state);
            }

            states[portIndex] = state;

            if (KeepChildrenConnected)
            {
                state.ConnectToGraph();
            }
            else
            {
                state.SetWeightDirty();
            }
        }

        /// <summary>[Internal]
        /// Throws an <see cref="InvalidOperationException"/> if the provided states are different.
        /// <para></para>
        /// Use this method to verify that a state being removed was actually assigned to its specified
        /// <see cref="PortIndex"/>.
        /// </summary>
        protected void ValidateRemoveChild(AnimancerState connected, AnimancerState removing)
        {
            if (connected == removing)
                return;

            throw new InvalidOperationException(
                "Tried to remove a state that wasn't actually connected to its port on " + this + ":" +
                "\n    Port: " + removing.PortIndex +
                "\n    Connected Child: " + connected +
                "\n    Disconnecting Child: " + removing);
        }

        /************************************************************************************************************************/

        /// <summary>[Internal]
        /// Called by <see cref="AnimancerState.Dispose"/> for any states connected to this mixer.
        /// Adds the 'state's port to a list of spares to be reused by another state and notifies the root
        /// <see cref="AnimancerPlayable"/>.
        /// </summary>
        protected internal virtual void OnChildDestroyed(AnimancerState state) { }

        /************************************************************************************************************************/

        /// <summary>Indicates whether child playables should stay connected to this mixer at all times.</summary>
        public virtual bool KeepChildrenConnected { get { return false; } }

        /// <summary>
        /// Ensures that all children of this node are connected to the <see cref="_Playable"/>.
        /// </summary>
        internal void ConnectAllChildrenToGraph()
        {
            foreach (var state in this)
            {
                if (!_Playable.GetInput(state.PortIndex).IsValid())
                {
                    state.ConnectToGraph();
                    state.SetWeightDirty();
                }

                state.ConnectAllChildrenToGraph();
            }
        }

        /// <summary>
        /// Ensures that all children of this node which have zero weight are disconnected from the
        /// <see cref="_Playable"/>.
        /// </summary>
        internal void DisconnectWeightlessChildrenFromGraph()
        {
            foreach (var state in this)
            {
                if (state.Weight == 0)
                    state.DisconnectFromGraph();

                state.DisconnectWeightlessChildrenFromGraph();
            }
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Connects the <see cref="_Playable"/> to the parent <see cref="IAnimationMixer.Playable"/>.
        /// </summary>
        public void ConnectToGraph()
        {
            Root._Graph.Connect(_Playable, 0, ParentPlayable, PortIndex);
            RequireUpdate();
        }

        /// <summary>
        /// Disconnects the <see cref="_Playable"/> from the parent <see cref="IAnimationMixer.Playable"/>.
        /// </summary>
        public void DisconnectFromGraph()
        {
            var parentMixer = ParentPlayable;
            if (parentMixer.GetInput(PortIndex).IsValid())
                Root._Graph.Disconnect(parentMixer, PortIndex);
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Indicates whether the <see cref="_Playable"/> is usable (properly initialised and not destroyed).
        /// </summary>
        public bool IsValid { get { return _Playable.IsValid(); } }

        /************************************************************************************************************************/
        // IEnumerable for 'foreach' statements.
        /************************************************************************************************************************/

        /// <summary>Gets an enumerator for all of this node's child states.</summary>
        public virtual IEnumerator<AnimancerState> GetEnumerator()
        {
            yield break;
        }

        IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }

        /************************************************************************************************************************/
        // IEnumerator for yielding in a coroutine to wait until animations have stopped.
        /************************************************************************************************************************/

        /// <summary>
        /// Returns true if the animation is playing and hasn't yet reached its end.
        /// <para></para>
        /// This method is called by <see cref="IEnumerator.MoveNext"/> so this object can be used as a custom yield
        /// instruction to wait until it finishes.
        /// </summary>
        protected internal abstract bool IsPlayingAndNotEnding();

        /// <summary>Calls <see cref="IsPlayingAndNotEnding"/>.</summary>
        bool IEnumerator.MoveNext() { return IsPlayingAndNotEnding(); }

        /// <summary>Returns null.</summary>
        object IEnumerator.Current { get { return null; } }

        /// <summary>Does nothing.</summary>
        void IEnumerator.Reset() { }

        /************************************************************************************************************************/
        #endregion
        /************************************************************************************************************************/
        #region Weight
        /************************************************************************************************************************/

        /// <summary>The current blend weight of this layer. Accessed via <see cref="Weight"/>.</summary>
        private float _Weight;

        /// <summary>Indicates whether the weight has changed and should be applied to the parent mixer.</summary>
        private bool _IsWeightDirty = true;

        /************************************************************************************************************************/

        /// <summary>
        /// The current blend weight of this node which determines how much it affects the final output.
        /// 0 has no effect while 1 applies the full effect of this node.
        /// <para></para>
        /// Setting this property cancels any fade currently in progress. If you don't wish to do that, you can use
        /// <see cref="SetWeight"/> instead.
        /// <para></para>
        /// Animancer Lite only allows this value to be set to 0 or 1 in a runtime build.
        /// </summary>
        public float Weight
        {
            get { return _Weight; }
            set
            {
                SetWeight(value);
                TargetWeight = value;
                FadeSpeed = 0;
            }
        }

        /// <summary>
        /// Sets the current blend weight of this node which determines how much it affects the final output.
        /// 0 has no effect while 1 applies the full effect of this node.
        /// <para></para>
        /// This method allows any fade currently in progress to continue. If you don't wish to do that, you can set
        /// the <see cref="Weight"/> property instead.
        /// <para></para>
        /// Animancer Lite only allows this value to be set to 0 or 1 in a runtime build.
        /// </summary>
        public void SetWeight(float value)
        {
            if (_Weight == value)
                return;

            _Weight = value;
            _IsWeightDirty = true;
            RequireUpdate();
        }

        /// <summary>
        /// Flags this node as having a dirty weight that needs to be applied next update.
        /// </summary>
        protected internal void SetWeightDirty()
        {
            _IsWeightDirty = true;
            RequireUpdate();
        }

        /************************************************************************************************************************/

        /// <summary>[Internal]
        /// Applies the <see cref="Weight"/> to the connection between this node and its <see cref="Parent"/>.
        /// </summary>
        internal void ApplyWeight()
        {
            if (!_IsWeightDirty)
                return;

            _IsWeightDirty = false;

            var parentMixer = ParentPlayable;

            if (!StayConnectedWhenWeightless)
            {
                if (_Weight != 0)
                {
                    if (!parentMixer.GetInput(PortIndex).IsValid())
                        ConnectToGraph();
                }
                else
                {
                    DisconnectFromGraph();
                    return;
                }
            }

            parentMixer.SetInputWeight(PortIndex, _Weight);
        }

        /************************************************************************************************************************/
        #endregion
        /************************************************************************************************************************/
        #region Fading
        /************************************************************************************************************************/

        /// <summary>The target blend weight which this layer is fading towards.</summary>
        public float TargetWeight { get; set; }

        /// <summary>The speed at which this layer is fading towards the <see cref="TargetWeight"/>.</summary>
        public float FadeSpeed { get; set; }

        /************************************************************************************************************************/

        /// <summary>
        /// Calls <see cref="OnStartFade"/> and starts fading the <see cref="Weight"/> over the course
        /// of the 'fadeDuration' (in seconds).
        /// <para></para>
        /// If the 'targetWeight' is 0 then <see cref="Stop"/> will be called when the fade is complete.
        /// <para></para>
        /// If the <see cref="Weight"/> is already equal to the 'targetWeight' then the fade will end
        /// immediately.
        /// <para></para>
        /// Animancer Lite only allows a 'targetWeight' of 0 or 1 and the default 'fadeDuration' in a runtime build.
        /// </summary>
        public void StartFade(float targetWeight, float fadeDuration = AnimancerPlayable.DefaultFadeDuration)
        {

            TargetWeight = targetWeight;

            if (targetWeight == Weight)
            {
                if (targetWeight == 0)
                {
                    Stop();
                }
                else
                {
                    FadeSpeed = 0;
                    OnStartFade();
                }

                return;
            }

            // Duration 0 = Instant.
            if (fadeDuration <= 0)
            {
                FadeSpeed = float.PositiveInfinity;
            }
            else// Otherwise determine how fast we need to go to cover the distance in the specified time.
            {
                FadeSpeed = Mathf.Abs(Weight - targetWeight) / fadeDuration;
            }

            OnStartFade();
            RequireUpdate();
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Called by <see cref="FadeExtensions.StartFade"/>.
        /// </summary>
        protected internal abstract void OnStartFade();

        /// <summary>
        /// Stops this node and makes it inactive so it no longer affects the output.
        /// </summary>
        public abstract void Stop();

        /************************************************************************************************************************/

#if UNITY_EDITOR
        /// <summary>[Editor-Only]
        /// Draws controls for <see cref="AnimancerState.FadeSpeed"/> and <see cref="AnimancerState.TargetWeight"/>.
        /// </summary>
        internal void DoFadeDetailsGUI(Rect area)
        {
            var labelWidth = UnityEditor.EditorGUIUtility.labelWidth;

            area = UnityEditor.EditorGUI.IndentedRect(area);

            var speedLabel = Editor.AnimancerEditorUtilities.GetNarrowText("Fade Speed");
            var targetLabel = Editor.AnimancerEditorUtilities.GetNarrowText("Target Weight");

            float speedWidth, weightWidth;
            Rect speedRect, weightRect;
            Editor.AnimancerEditorUtilities.SplitHorizontally(area, speedLabel, targetLabel, out speedWidth, out weightWidth, out speedRect, out weightRect);

            var indentLevel = UnityEditor.EditorGUI.indentLevel;
            UnityEditor.EditorGUI.indentLevel = 0;

            // Fade Speed.
            UnityEditor.EditorGUIUtility.labelWidth = speedWidth;
            FadeSpeed = UnityEditor.EditorGUI.FloatField(speedRect, speedLabel, FadeSpeed);

            // Target Weight.
            UnityEditor.EditorGUIUtility.labelWidth = weightWidth;
            TargetWeight = UnityEditor.EditorGUI.FloatField(weightRect, targetLabel, TargetWeight);

            UnityEditor.EditorGUI.indentLevel = indentLevel;
            UnityEditor.EditorGUIUtility.labelWidth = labelWidth;
        }
#endif

        /************************************************************************************************************************/
        #endregion
        /************************************************************************************************************************/
        #region Updating
        /************************************************************************************************************************/

        /// <summary>
        /// Indicates whether this has been added to the list of nodes that need to be updated.
        /// </summary>
        private bool _IsUpdating;

        /************************************************************************************************************************/

        /// <summary>
        /// Adds this to the list of nodes that need to be updated if it wasn't there already.
        /// </summary>
        protected internal void RequireUpdate()
        {
            if (_IsUpdating)
                return;

            Root.RequireUpdate(this);
            _IsUpdating = true;
        }

        /************************************************************************************************************************/

        /// <summary>[Internal]
        /// Calls <see cref="Update"/> and assumes that if 'needsMoreUpdates' returns false then this node will be
        /// removed from the updating list.
        /// </summary>
        internal void UpdateNode(out bool needsMoreUpdates)
        {
            needsMoreUpdates = false;
            Update(ref needsMoreUpdates);
            _IsUpdating = needsMoreUpdates;
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Updates the <see cref="Weight"/> for fading, applies it to this state's port on the parent mixer, and plays
        /// or pauses the <see cref="_Playable"/> if its state is dirty.
        /// <para></para>
        /// If the root <see cref="AnimancerPlayable.DisconnectWeightlessPlayables"/> is set to true, this method will
        /// also connect/disconnect its state from the animation mixer in the playable graph.
        /// </summary>
        protected virtual void Update(ref bool needsMoreUpdates)
        {
            // Update Fade.
            var fadeSpeed = FadeSpeed;
            if (fadeSpeed != 0)
            {
                var target = TargetWeight;

                var current = _Weight = MoveTowards(_Weight, target, fadeSpeed * AnimancerPlayable.DeltaTime);
                _IsWeightDirty = true;

                if (current == target)
                {
                    if (current == 0)
                    {
                        Stop();
                    }
                    else
                    {
                        FadeSpeed = 0;
                    }
                }
                else needsMoreUpdates = true;
            }

            ApplyWeight();
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Moves the 'current' value towards the 'target' without moving more than the 'maxDelta'.
        /// <para></para>
        /// This implementation seems to be about twice as fast as <see cref="Mathf.MoveTowards"/>
        /// </summary>
        public static float MoveTowards(float current, float target, float maxDelta)
        {
            var delta = target - current;
            if (delta > 0)
            {
                if (delta <= maxDelta)
                    return target;
                else
                    return current + maxDelta;
            }
            else
            {
                if (-delta <= maxDelta)
                    return target;
                else
                    return current - maxDelta;
            }
        }

        /************************************************************************************************************************/
        #endregion
        /************************************************************************************************************************/
    }
}
