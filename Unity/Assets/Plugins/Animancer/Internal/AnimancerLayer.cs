// Animancer // Copyright 2019 Kybernetik //

#pragma warning disable IDE0018 // Inline variable declaration.

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace Animancer
{
    /// <summary>
    /// A layer on which animations can play with their states managed independantly of other layers while blending the
    /// output with those layers.
    /// <para></para>
    /// This class can be used as a custom yield instruction to wait until all animations finish playing.
    /// </summary>
    public sealed class AnimancerLayer : AnimancerNode, IAnimationClipSource
    {
        /************************************************************************************************************************/
        #region Fields and Properties
        /************************************************************************************************************************/

        /// <summary>A layer is its own root.</summary>
        public override AnimancerLayer Layer { get { return this; } }

        /// <summary>Returns null because layers always connect to the <see cref="AnimancerNode.Root"/>.</summary>
        public override AnimancerNode Parent { get { return null; } }

        /// <summary>The <see cref="Root"/>'s <see cref="AnimationLayerMixerPlayable"/>.</summary>
        protected override Playable ParentPlayable { get { return Root._LayerMixer; } }

        /// <summary>Indicates whether child playables should stay connected to this mixer at all times.</summary>
        public override bool KeepChildrenConnected { get { return Root.KeepPlayablesConnected; } }

        /// <summary>The <see cref="Root"/>'s <see cref="AnimancerNode.KeepPlayablesConnected"/>.</summary>
        public override bool StayConnectedWhenWeightless { get { return Root.KeepPlayablesConnected; } }

        /************************************************************************************************************************/

        private AnimancerState _CurrentState;

        /// <summary>
        /// The state of the animation currently being played.
        /// <para></para>
        /// Specifically, this is the state that was most recently started using any of the Play or CrossFade methods
        /// on this layer. States controlled individually via methods in the <see cref="AnimancerState"/> itself will
        /// not register in this property.
        /// <para></para>
        /// Each time this property changes, the <see cref="CurrentStateID"/> is incremented.
        /// </summary>
        public AnimancerState CurrentState
        {
            get { return _CurrentState; }
            private set
            {
                _CurrentState = value;
                CurrentStateID++;
            }
        }

        /// <summary>
        /// The number of times the <see cref="CurrentState"/> has changed. By storing this value and later comparing
        /// the stored value to the current value, you can determine whether the state has been changed since then,
        /// even if the actual state is the same.
        /// </summary>
        public int CurrentStateID { get; private set; }

        /************************************************************************************************************************/

        /// <summary>All of the animation states connected to the <see cref="_Mixer"/>.</summary>
        private readonly List<AnimancerState> States = new List<AnimancerState>();

        /************************************************************************************************************************/
        #endregion
        /************************************************************************************************************************/
        #region Initialisation
        /************************************************************************************************************************/

        /// <summary>[Internal]
        /// Constructs a new <see cref="AnimancerLayer"/>.
        /// </summary>
        internal AnimancerLayer(AnimancerPlayable root, int layerIndex)
            : base(root)
        {

            PortIndex = layerIndex;
            _Playable = AnimationMixerPlayable.Create(Root._Graph, 1, true);
        }

        /************************************************************************************************************************/

        /// <summary>[Pro-Only]
        /// Determines whether this layer is set to additive blending. Otherwise it will override any earlier layers.
        /// </summary>
        public bool IsAdditive
        {
            get { return Root.IsLayerAdditive(PortIndex); }
            set { Root.SetLayerAdditive(PortIndex, value); }
        }

        /************************************************************************************************************************/

        /// <summary>[Pro-Only]
        /// Sets an <see cref="AvatarMask"/> to determine which bones this layer will affect.
        /// </summary>
        public void SetMask(AvatarMask mask)
        {
            Root.SetLayerMask(PortIndex, mask);
        }

#if UNITY_EDITOR
        /// <summary>[Editor-Only]
        /// The <see cref="AvatarMask"/> that determines which bones this layer will affect.
        /// </summary>
        internal AvatarMask _Mask;
#endif

        /************************************************************************************************************************/

        /// <summary>
        /// Destroys all states connected to this layer. This operation cannot be undone.
        /// </summary>
        public void DestroyStates()
        {
            var count = States.Count;
            for (int i = 0; i < count; i++)
            {
                States[i].Dispose();
            }

            States.Clear();
        }

        /************************************************************************************************************************/
        #endregion
        /************************************************************************************************************************/
        #region State Creation and Access
        /************************************************************************************************************************/

        /// <summary>The number of states using this layer as their <see cref="AnimancerState.Parent"/>.</summary>
        public override int ChildCount { get { return States.Count; } }

        /// <summary>
        /// Returns the state connected to the specified 'portIndex' as a child of this layer.
        /// </summary>
        public override AnimancerState GetChild(int portIndex)
        {
            return States[portIndex];
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Adds a new port and uses <see cref="AnimancerState.SetParent"/> to connect the 'state' to it.
        /// </summary>
        public void AddChild(AnimancerState state)
        {
            if (state.Parent == this)
                return;

            var portIndex = States.Count;
            States.Add(null);
            _Playable.SetInputCount(portIndex + 1);
            state.SetParent(this, portIndex);
        }

        /************************************************************************************************************************/

        /// <summary>Connects the 'state' to this layer at its <see cref="AnimancerNode.PortIndex"/>.</summary>
        protected internal override void OnAddChild(AnimancerState state)
        {
            OnAddChild(States, state);
        }

        /************************************************************************************************************************/

        /// <summary>Disconnects the 'state' from this layer at its <see cref="AnimancerNode.PortIndex"/>.</summary>
        protected internal override void OnRemoveChild(AnimancerState state)
        {
            var portIndex = state.PortIndex;
            ValidateRemoveChild(States[portIndex], state);

            if (_Playable.GetInput(portIndex).IsValid())
                Root._Graph.Disconnect(_Playable, portIndex);

            // Swap the last state into the place of the one that was just removed.
            var lastPort = States.Count - 1;
            if (portIndex < lastPort)
            {
                state = States[lastPort];
                state.DisconnectFromGraph();

                States[portIndex] = state;
                state.PortIndex = portIndex;

                if (KeepChildrenConnected || state.Weight != 0)
                    state.ConnectToGraph();
            }

            States.RemoveAt(lastPort);
            _Playable.SetInputCount(lastPort);
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> if the <see cref="AnimancerState.Parent"/> is not this
        /// <see cref="AnimancerLayer"/>.
        /// </summary>
        /// <exception cref="ArgumentException"/>
        public void ValidateState(AnimancerState state)
        {
            if (state.Parent != this)
                throw new ArgumentException("AnimancerState.Parent mismatch:" +
                    " you are attempting to use a state in an AnimancerLayer that isn't it's parent.");
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Returns an enumerator that will iterate through all states connected directly to this layer (not inside
        /// <see cref="MixerState"/>s).
        /// </summary>
        public override IEnumerator<AnimancerState> GetEnumerator() { return States.GetEnumerator(); }

        /************************************************************************************************************************/

        /// <summary>
        /// Returns true if the <see cref="CurrentState"/> is playing and hasn't yet reached its end.
        /// <para></para>
        /// This method is called by <see cref="IEnumerator.MoveNext"/> so this object can be used as a custom yield
        /// instruction to wait until it finishes.
        /// </summary>
        /// <exception cref="NotSupportedException">Thrown if this state doesn't have a length, as indicated by
        /// <see cref="HasLength"/>.</exception>
        protected internal override bool IsPlayingAndNotEnding()
        {
            return _CurrentState != null && _CurrentState.IsPlayingAndNotEnding();
        }

        /************************************************************************************************************************/
        #endregion
        /************************************************************************************************************************/
        #region Play Management
        /************************************************************************************************************************/

        /// <summary>
        /// Called by <see cref="AnimancerNode.StartFade"/> (when this layer starts fading, not when one of its states
        /// starts fading). Clears the <see cref="AnimancerState.OnEnd"/> events of all states.
        /// </summary>
        protected internal override void OnStartFade()
        {
            var count = States.Count;
            for (int i = 0; i < count; i++)
            {
                States[i].OnStartFade();
            }
        }

        /************************************************************************************************************************/
        // Starting
        /************************************************************************************************************************/

        /// <summary>
        /// Stops all other animations, plays the 'state', and returns it.
        /// <para></para>
        /// The animation will continue playing from its current <see cref="AnimancerState.Time"/>.
        /// If you wish to force it back to the start, you can simply set the 'state's time to 0.
        /// </summary>
        public AnimancerState Play(AnimancerState state)
        {
            ValidateState(state);

            CurrentState = state;

            state.Play();

            var count = States.Count;
            for (int i = 0; i < count; i++)
            {
                var otherState = States[i];
                if (otherState != state)
                    otherState.Stop();
            }

            return state;
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Starts fading in the 'state' over the course of the 'fadeDuration' while fading out all others in this
        /// layer. Returns the 'state'.
        /// <para></para>
        /// If the 'state' was already playing, it will continue doing so from the current time, unlike
        /// <see cref="CrossFadeFromStart"/>.
        /// <para></para>
        /// If the 'state' was already playing and fading in with less time remaining than the 'fadeDuration', this
        /// method will allow it to complete the existing fade rather than starting a slower one.
        /// <para></para>
        /// If this layer currently has 0 <see cref="Weight"/>, this method will instead start fading in the layer
        /// itself and simply <see cref="Play"/> the 'state'.
        /// <para></para>
        /// Animancer Lite only allows the default 'fadeDuration' (0.3 seconds) in a runtime build.
        /// </summary>
        public AnimancerState CrossFade(AnimancerState state, float fadeDuration = AnimancerPlayable.DefaultFadeDuration)
        {
            ValidateState(state);

            CurrentState = state;

            if (Weight == 0)
                return Play(state);

            // If the state is already playing or will finish fading in faster than this new fade,
            // continue the existing fade but still pretend it was restarted.
            if (state.IsPlaying && state.TargetWeight == 1 &&
                (state.Weight == 1 || state.FadeSpeed * fadeDuration > Mathf.Abs(1 - state.Weight)))
            {
                OnStartFade();
            }
            // Otherwise fade in the target state and fade out all others.
            else
            {
                state.IsPlaying = true;
                state.StartFade(1, fadeDuration);

                var count = States.Count;
                for (int i = 0; i < count; i++)
                {
                    var otherState = States[i];
                    if (otherState != state)
                        otherState.StartFade(0, fadeDuration);
                }
            }

            return state;
        }

        /************************************************************************************************************************/

#if UNITY_EDITOR
        /// <summary>[Editor-Only]
        /// The maximum number of duplicate states that can be created by <see cref="CrossFadeFromStart"/> for a single
        /// clip before it will start giving usage warnings. Default = 5.
        /// </summary>
        public static int maxCrossFadeFromStartDepth;
#endif

        /// <summary>
        /// Starts fading in the 'state' from the start over the course of the 'fadeDuration' while fading out all
        /// others in this layer. Returns the 'state'.
        /// <para></para>
        /// If the 'state' isn't currently at 0 <see cref="AnimancerState.Weight"/>, this method will actually fade it
        /// to 0 along with the others and create and return a new state with the same clip to fade to 1. This ensures
        /// that calling this method will always fade out from all current states and fade in from the start of the
        /// desired animation. States created for this purpose are cached so they can be reused in the future.
        /// <para></para>
        /// Calling this method repeatedly on subsequent frames will probably have undesirable effects; you most likely
        /// want to use <see cref="CrossFade"/> instead.
        /// <para></para>
        /// If this layer currently has 0 <see cref="Weight"/>, this method will instead start fading in the layer
        /// itself and simply <see cref="Play"/> the 'state'.
        /// <para></para>
        /// Animancer Lite only allows the default 'fadeDuration' (0.3 seconds) in a runtime build.
        /// </summary>
        /// <remarks>
        /// This can be useful when you want to repeat an action while the previous animation is still fading out.
        /// For example, if you play an 'Attack' animation, it ends and starts fading back to 'Idle', and while it is
        /// doing so you want to start another 'Attack'. The previous 'Attack' can't simply snap back to the start, so
        /// you can use this method to create a second 'Attack' state to fade in while the old one fades out.
        /// </remarks>
        public AnimancerState CrossFadeFromStart(AnimancerState state, float fadeDuration = AnimancerPlayable.DefaultFadeDuration)
        {

            if (Weight == 0)
                return Play(state);

            if (Weight != 0 && state.Weight != 0)
            {
                var clip = state.Clip;

                if (clip == null)
                {
                    Debug.LogWarning("CrossFadeFromStart was called on a state which has no clip: " + state, state.MainObject);
                }
                else
                {
                    var layerIndex = state.LayerIndex;

                    // Get the default state registered with the clip.
                    state = Root.GetOrCreateState(clip, clip, layerIndex);

#if UNITY_EDITOR
                    int depth = 1;
#endif

                    // If that state isn't at 0 weight, get or create another state registered using the previous state as a key.
                    // Keep going through states in this manner until you find one at 0 weight.
                    while (state.Weight != 0)
                    {
                        // Explicitly cast the state to an object to avoid the overload that warns about using a state as a key.
                        state = Root.GetOrCreateState((object)state, clip, layerIndex);

#if UNITY_EDITOR
                        if (depth++ == maxCrossFadeFromStartDepth)
                        {
                            throw new ArgumentOutOfRangeException("depth", "CrossFadeFromStart has created " +
                                maxCrossFadeFromStartDepth + " or more states for a single clip." +
                                " This is most likely a result of calling the method repeatedly on consecutive frames." +
                                " You probably just want to use CrossFade instead, but you can increase" +
                                " AnimancerLayer.MaxCrossFadeFromStartDepth if necessary.");
                        }
#endif
                    }
                }
            }

            // Reset the time in case it wasn't already reset when the weight was previously set to 0.
            state.Time = 0;

            // Now that we have a state with 0 weight, start fading it in.
            return CrossFade(state, fadeDuration);
        }

        /************************************************************************************************************************/
        // Stopping
        /************************************************************************************************************************/

        /// <summary>
        /// Sets <see cref="Weight"/> = 0 and calls <see cref="AnimancerState.Stop"/> on all animations to stop them
        /// from playing and rewind them to the start.
        /// </summary>
        public override void Stop()
        {
            Weight = 0;
            CurrentState = null;

            var count = States.Count;
            for (int i = 0; i < count; i++)
            {
                States[i].Stop();
            }
        }

        /************************************************************************************************************************/
        // Checking
        /************************************************************************************************************************/

        /// <summary>
        /// Returns true if the 'clip' is currently being played by at least one state.
        /// </summary>
        public bool IsPlayingClip(AnimationClip clip)
        {
            var count = States.Count;
            for (int i = 0; i < count; i++)
            {
                var state = States[i];
                if (state.Clip == clip && state.IsPlaying)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Returns true if at least one animation is being played.
        /// </summary>
        public bool IsPlaying()
        {
            var count = States.Count;
            for (int i = 0; i < count; i++)
            {
                if (States[i].IsPlaying)
                    return true;
            }

            return false;
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Calculates the total <see cref="AnimancerState.Weight"/> of all states in this layer.
        /// </summary>
        public float GetTotalWeight()
        {
            float weight = 0;

            var count = States.Count;
            for (int i = 0; i < count; i++)
            {
                weight += States[i].Weight;
            }

            return weight;
        }

        /************************************************************************************************************************/

        /// <summary>
        /// The average velocity of the root motion of all currently playing animations, taking their current
        /// <see cref="AnimancerState.Weight"/> into account.
        /// </summary>
        public Vector3 AverageVelocity
        {
            get
            {
                var velocity = Vector3.zero;

                var count = States.Count;
                for (int i = 0; i < count; i++)
                {
                    var state = States[i];
                    velocity += state.AverageVelocity * state.Weight;
                }

                return velocity;
            }
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Invokes the <see cref="AnimancerState.OnEnd"/> event of the state that is playing the animation which
        /// triggered the event. Returns true if such a state exists (even if it doesn't have a callback registered).
        /// </summary>
        internal bool TryInvokeOnEndEvent(AnimationEvent animationEvent)
        {
            var count = States.Count;
            for (int i = 0; i < count; i++)
            {
                if (AnimancerPlayable.TryInvokeOnEndEvent(animationEvent, States[i]))
                    return true;
            }

            return false;
        }

        /************************************************************************************************************************/
        #endregion
        /************************************************************************************************************************/
        #region Inverse Kinematics
        /************************************************************************************************************************/

        /// <summary>
        /// Determines the default value of <see cref="AnimancerState.ApplyAnimatorIK"/> for all new states created in
        /// this layer. Default false.
        /// <para></para>
        /// It requires Unity 2018.1 or newer, however 2018.3 or newer is recommended because a bug in earlier versions
        /// of the Playables API caused this value to only take effect while at least one state was at
        /// <see cref="AnimancerState.Weight"/> = 1 which meant that IK would not work while fading between animations.
        /// </summary>
        public bool DefaultApplyAnimatorIK { get; set; }

        /// <summary>
        /// Determines whether <c>OnAnimatorIK(int layerIndex)</c> will be called on the animated object for any <see cref="States"/>.
        /// The initial value is determined by <see cref="DefaultApplyAnimatorIK"/> when a new state is created.
        /// <para></para>
        /// This is equivalent to the "IK Pass" toggle in Animator Controller layers.
        /// <para></para>
        /// It requires Unity 2018.1 or newer, however 2018.3 or newer is recommended because a bug in earlier versions
        /// of the Playables API caused this value to only take effect while at least one state was at
        /// <see cref="AnimancerState.Weight"/> = 1 which meant that IK would not work while fading between animations.
        /// </summary>
        public bool ApplyAnimatorIK
        {
            get
            {
                var count = States.Count;
                for (int i = 0; i < count; i++)
                {
                    if (States[i].ApplyAnimatorIK)
                        return true;
                }

                return false;
            }
            set
            {
                DefaultApplyAnimatorIK = value;

                var count = States.Count;
                for (int i = 0; i < count; i++)
                {
                    States[i].ApplyAnimatorIK = value;
                }
            }
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Determines the default value of <see cref="AnimancerState.ApplyFootIK"/> for all new states created in this
        /// layer. Default false.
        /// </summary>
        public bool DefaultApplyFootIK { get; set; }

        /// <summary>
        /// Determines whether any of the <see cref="States"/> in this layer are applying IK to the character's feet.
        /// The initial value is determined by <see cref="DefaultApplyFootIK"/> when a new state is created.
        /// <para></para>
        /// This is equivalent to the "Foot IK" toggle in Animator Controller states (applied to the whole layer).
        /// </summary>
        public bool ApplyFootIK
        {
            get
            {
                var count = States.Count;
                for (int i = 0; i < count; i++)
                {
                    if (States[i].ApplyFootIK)
                        return true;
                }

                return false;
            }
            set
            {
                DefaultApplyFootIK = value;

                var count = States.Count;
                for (int i = 0; i < count; i++)
                {
                    States[i].ApplyFootIK = value;
                }
            }
        }

        /************************************************************************************************************************/
        #endregion
        /************************************************************************************************************************/
        #region Inspector
        /************************************************************************************************************************/

        /// <summary>Returns a short description of this layer.</summary>
        public override string ToString()
        {
#if UNITY_EDITOR
            return string.Concat(Name, " (Layer ", PortIndex.ToString(), ")");
#else
            return "Layer " + PortIndex;
#endif
        }

        /************************************************************************************************************************/

        /// <summary>[<see cref="IAnimationClipSource"/>]
        /// Gathers all the animations in this layer.
        /// </summary>
        public void GetAnimationClips(List<AnimationClip> clips)
        {
            var count = States.Count;
            for (int i = 0; i < count; i++)
            {
                States[i].GetAnimationClips(clips);
            }
        }

        /************************************************************************************************************************/

        /// <summary>Returns a user-friendly key to identify the 'state' in the inspector.</summary>
        public string GetDisplayKey(AnimancerState state)
        {
            return string.Concat("[", state.PortIndex.ToString(), "]");
        }

        /************************************************************************************************************************/

#if UNITY_EDITOR
        /// <summary>[Editor-Only] [Internal] Indicates whether the inspector details for this layer are expanded.</summary>
        internal bool _IsInspectorExpanded;

        /// <summary>[Editor-Only] [Internal] The inspector display name of this layer.</summary>
        internal string _Name;

        /// <summary>[Editor-Only] The inspector display name of this layer.</summary>
        public string Name
        {
            get
            {
                if (_Name == null)
                {
                    if (_Mask != null)
                        return _Mask.name;

                    _Name = PortIndex == 0 ? "Base Layer" : "Layer " + PortIndex;
                }

                return _Name;
            }
        }
#endif

        /// <summary>[Editor-Conditional]
        /// Sets the inspector display name of this layer. Note that layer names are Editor-Only so any calls to this
        /// method will automatically be compiled out of a runtime build.
        /// </summary>
        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        public void SetName(string name)
        {
#if UNITY_EDITOR
            _Name = name;
#endif
        }

        /************************************************************************************************************************/
        #endregion
        /************************************************************************************************************************/
    }
}
