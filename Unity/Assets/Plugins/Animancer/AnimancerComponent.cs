// Animancer // Copyright 2019 Kybernetik //

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace Animancer
{
    /// <summary>
    /// A substitute for a <see cref="RuntimeAnimatorController"/> which allows you to freely play animations on an
    /// <see cref="UnityEngine.Animator"/> without using the standard Mecanim state machine system.
    /// <para></para>
    /// This class can be used as a custom yield instruction to wait until all animations finish playing.
    /// </summary>
    /// <remarks>
    /// This class is mostly just a wrapper that connects an <see cref="AnimancerPlayable"/> to an
    /// <see cref="UnityEngine.Animator"/>.
    /// </remarks>
    [AddComponentMenu("Animancer/Animancer Component")]
    [HelpURL(AnimancerPlayable.APIDocumentationURL + "/AnimancerComponent")]
    [DefaultExecutionOrder(-5000)]// Initialise before anything else tries to use this component.
    public class AnimancerComponent : MonoBehaviour, IAnimancerComponent, IEnumerable<AnimancerState>, IEnumerator, IAnimationClipSource
    {
        /************************************************************************************************************************/
        #region Fields and Properties
        /************************************************************************************************************************/

        /// <summary>Group "Assets/Create/..." menu items just under "Avatar Mask".</summary>
        public const int AssetMenuOrder = 410;

        /************************************************************************************************************************/

        [SerializeField]
        [Tooltip("The Animator component which this script controls")]
        private Animator _Animator;

        /// <summary>The <see cref="UnityEngine.Animator"/> component which this script controls.</summary>
        public Animator Animator
        {
            get { return _Animator; }
            set
            {
#if UNITY_EDITOR
                Editor.AnimancerEditorUtilities.SetIsInspectorExpanded(_Animator, true);
                Editor.AnimancerEditorUtilities.SetIsInspectorExpanded(value, false);

                if (!UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode)
                {
                    _Animator = value;
                    return;
                }
#endif

                // Disable the previous Animator so it stops playing the graph.
                if (_Playable != null && _Animator != null)
                    _Animator.enabled = false;

                _Animator = value;
                AnimancerPlayable.Play(_Animator, _Playable);
            }
        }

#if UNITY_EDITOR
        /// <summary>[Editor-Only] The name of the serialized backing field for the <see cref="Animator"/> property.</summary>
        string IAnimancerComponent.AnimatorFieldName { get { return "_Animator"; } }
#endif

        /************************************************************************************************************************/

        private AnimancerPlayable _Playable;

        /// <summary>
        /// The internal system which manages the playing animations.
        /// Accessing this property will automatically initialise it.
        /// </summary>
        public AnimancerPlayable Playable
        {
            get
            {
                InitialisePlayable();
                return _Playable;
            }
        }

        /// <summary>Indicates whether the <see cref="Playable"/> has been initialised.</summary>
        public bool IsPlayableInitialised { get { return _Playable != null && _Playable.IsValid; } }

        /************************************************************************************************************************/

#if UNITY_2018_1_OR_NEWER
        /// <summary>
        /// If true, disabling this object will stop and rewind all animations. Otherwise they will simply be paused
        /// and will resume from their current states when it is re-enabled.
        /// <para></para>
        /// The default value is true.
        /// <para></para>
        /// This property wraps <see cref="Animator.keepAnimatorControllerStateOnDisable"/> and inverts its value.
        /// The value is serialized by the <see cref="UnityEngine.Animator"/>.
        /// <para></para>
        /// It requires Unity 2018.1 or newer.
        /// </summary>
        public bool StopOnDisable
        {
            get { return !_Animator.keepAnimatorControllerStateOnDisable; }
            set { _Animator.keepAnimatorControllerStateOnDisable = !value; }
        }
#endif

        /************************************************************************************************************************/
        #region Update Mode
        /************************************************************************************************************************/

        /// <summary>
        /// Determines when animations are updated and which time source is used. This property is mainly a wrapper
        /// around the <see cref="Animator.updateMode"/>.
        /// <para></para>
        /// Note that changing to or from <see cref="AnimatorUpdateMode.AnimatePhysics"/> at runtime has no effect.
        /// </summary>
        /// <exception cref="NullReferenceException">Thrown if no <see cref="Animator"/> is assigned.</exception>
        public AnimatorUpdateMode UpdateMode
        {
            get { return _Animator.updateMode; }
            set
            {
                _Animator.updateMode = value;

                if (!IsPlayableInitialised)
                    return;

                // UnscaledTime on the Animator is actually identical to Normal when using the Playables API so we need
                // to set the graph's DirectorUpdateMode to determine how it gets its delta time.
                _Playable.UpdateMode = value == AnimatorUpdateMode.UnscaledTime ?
                    DirectorUpdateMode.UnscaledGameTime :
                    DirectorUpdateMode.GameTime;

#if UNITY_EDITOR
                if (InitialUpdateMode == null)
                {
                    InitialUpdateMode = value;
                }
                else if (UnityEditor.EditorApplication.isPlaying)
                {
                    if (AnimancerPlayable.HasChangedToOrFromAnimatePhysics(InitialUpdateMode, value))
                        Debug.LogWarning("Changing the Animator.updateMode to or from AnimatePhysics at runtime will have no effect." +
                            " You must set it in the Unity Editor or on startup.");
                }
#endif
            }
        }

        /************************************************************************************************************************/
#if UNITY_EDITOR
        /************************************************************************************************************************/

        /// <summary>[Editor-Only]
        /// The <see cref="UpdateMode"/> what was first used when this script initialised.
        /// This is used to give a warning when changing to or from <see cref="AnimatorUpdateMode.AnimatePhysics"/> at
        /// runtime since it won't work correctly.
        /// </summary>
        public AnimatorUpdateMode? InitialUpdateMode { get; private set; }

        /************************************************************************************************************************/
#endif
        /************************************************************************************************************************/
        #endregion
        /************************************************************************************************************************/
        #endregion
        /************************************************************************************************************************/
        #region Animation Events
        /************************************************************************************************************************/
        // These methods are above their regular overloads so Animation Events find them first (because the others can't be used).
        /************************************************************************************************************************/

        /// <summary>Called by Animation Events. Calls <see cref="Play(AnimationClip, int)"/>.</summary>
        private void Play(AnimationEvent animationEvent)
        {
            Play((AnimationClip)animationEvent.objectReferenceParameter, animationEvent.intParameter);
        }

        /// <summary>
        /// Called by Animation Events.
        /// Calls <see cref="Play(AnimationClip, int)"/> and sets the <see cref="AnimancerState.Time"/> = 0.
        /// </summary>
        private void PlayFromStart(AnimationEvent animationEvent)
        {
            Play((AnimationClip)animationEvent.objectReferenceParameter, animationEvent.intParameter)
                .Time = 0;
        }

        /// <summary>Called by Animation Events. Calls <see cref="CrossFade(AnimationClip, float, int)"/>.</summary>
        private void CrossFade(AnimationEvent animationEvent)
        {
            CrossFade((AnimationClip)animationEvent.objectReferenceParameter, animationEvent.intParameter);
        }

        /// <summary>Called by Animation Events. Calls <see cref="CrossFadeFromStart(AnimationClip, float, int)"/>.</summary>
        private void CrossFadeFromStart(AnimationEvent animationEvent)
        {
            CrossFadeFromStart((AnimationClip)animationEvent.objectReferenceParameter, animationEvent.intParameter);
        }

        /// <summary>Called by Animation Events. Calls <see cref="Transition(IAnimancerTransition, int)"/>.</summary>
        private void Transition(AnimationEvent animationEvent)
        {
            Transition((IAnimancerTransition)animationEvent.objectReferenceParameter, animationEvent.intParameter);
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Called by Animation Events. Invokes the <see cref="AnimancerState.OnEnd"/> event of the
        /// <see cref="CurrentState"/> if it is playing the <see cref="AnimationClip"/> which triggered the event.
        /// <para></para>
        /// Logs a warning if no state is registered for that animation.
        /// </summary>
        private void End(AnimationEvent animationEvent)
        {
            if (_Playable == null)
            {
                // This could only happen if another Animator triggers the event on this object somehow.
                Debug.LogWarning("AnimationEvent 'End' was triggered by " + animationEvent.animatorClipInfo.clip +
                    ", but the AnimancerComponent.Playable hasn't been initialised.",
                    this);
                return;
            }

            if (_Playable.OnEndEventReceived(animationEvent))
                return;

            if (animationEvent.messageOptions == SendMessageOptions.RequireReceiver)
            {
                Debug.LogWarning("AnimationEvent 'End' was triggered by " + animationEvent.animatorClipInfo.clip +
                    ", but no state was found with that key.",
                    this);
            }
        }

        /************************************************************************************************************************/
        #endregion
        /************************************************************************************************************************/
        #region Initialisation
        /************************************************************************************************************************/

#if UNITY_EDITOR
        /// <summary>[Editor-Only]
        /// Called by the Unity Editor when this component is first added (in edit mode) and whenever the Reset command
        /// is executed from its context menu.
        /// <para></para>
        /// Destroys the playable if one has been initialised.
        /// Searches for an <see cref="UnityEngine.Animator"/> on this object, or it's children or parents.
        /// Removes the <see cref="Animator.runtimeAnimatorController"/> if it finds one.
        /// <para></para>
        /// This method also prevents you from adding multiple copies of this component to a single object. Doing so
        /// will destroy the new one immediately and change the old one's type to match the new one, allowing you to
        /// change the type without losing the values of any serialized fields they share.
        /// </summary>
        protected virtual void Reset()
        {
            OnDestroy();

            _Animator = Editor.AnimancerEditorUtilities.GetComponentInHierarchy<Animator>(gameObject);

            if (_Animator != null)
            {
                _Animator.runtimeAnimatorController = null;
                Editor.AnimancerEditorUtilities.SetIsInspectorExpanded(_Animator, false);

                // Collapse the Animator property because the custom inspector uses that to control whether the
                // Animator's inspector is expanded.
                using (var serializedObject = new UnityEditor.SerializedObject(this))
                {
                    var property = serializedObject.FindProperty("_Animator");
                    property.isExpanded = false;
                    serializedObject.ApplyModifiedProperties();
                }
            }

            AnimancerUtilities.IfMultiComponentThenChangeType(this);
        }
#endif

        /************************************************************************************************************************/

        /// <summary>
        /// Called by Unity when this component becomes enabled and active.
        /// <para></para>
        /// Ensures that the <see cref="PlayableGraph"/> is playing.
        /// </summary>
        protected virtual void OnEnable()
        {
            if (IsPlayableInitialised)
                _Playable.UnpauseGraph();
        }

        /// <summary>
        /// Called by Unity when this component becomes disabled or inactive.
        /// <para></para>
        /// Stops all currently playing animations and the <see cref="PlayableGraph"/> if it was playing.
        /// </summary>
        protected virtual void OnDisable()
        {
            if (IsPlayableInitialised)
            {
#if UNITY_2018_1_OR_NEWER
                if (StopOnDisable)
#endif
                {
                    Stop();
                }

                _Playable.PauseGraph();
            }
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Creates a new playable if it doesn't already exist.
        /// </summary>
        private void InitialisePlayable()
        {
            if (IsPlayableInitialised)
                return;

            _Playable = AnimancerPlayable.CreatePlayable(name);

            if (_Animator != null)
            {
#if UNITY_EDITOR
                InitialUpdateMode = UpdateMode;
#endif

                AnimancerPlayable.Play(_Animator, _Playable);
            }
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Called by Unity when this component is destroyed.
        /// Ensures that the <see cref="Playable"/> is properly cleaned up.
        /// </summary>
        protected virtual void OnDestroy()
        {
            if (IsPlayableInitialised)
            {
                _Playable.Dispose();
                _Playable = null;
            }
        }

        /************************************************************************************************************************/

#if UNITY_EDITOR
        /// <summary>[Editor-Only]
        /// Ensures that the <see cref="PlayableGraph"/> is destroyed.
        /// </summary>
        ~AnimancerComponent()
        {
            if (_Playable != null)
                UnityEditor.EditorApplication.delayCall += OnDestroy;
        }
#endif

        /************************************************************************************************************************/
        #endregion
        /************************************************************************************************************************/
        #region State Creation and Access
        /************************************************************************************************************************/

        /// <summary>
        /// The state of the animation currently being played on layer 0.
        /// <para></para>
        /// Specifically, this is the state that was most recently started using any of the Play or CrossFade methods
        /// on that layer. States controlled individually via methods in the <see cref="AnimancerState"/> itself will
        /// not register in this property.
        /// </summary>
        public AnimancerState CurrentState
        {
            get
            {
                if (_Playable != null)
                    return _Playable.CurrentState;
                else
                    return null;
            }
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Returns the 'clip' itself. This method is used to determine the dictionary key to use for an animation
        /// when none is specified by the user, such as in <see cref="Play(AnimationClip)"/>. It can be overridden by
        /// child classes to use something else as the key.
        /// </summary>
        public virtual object GetKey(AnimationClip clip)
        {
            return clip;
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Creates and returns a new <see cref="ClipState"/> to play the 'clip'.
        /// </summary>
        public ClipState CreateState(AnimationClip clip, int layerIndex = 0)
        {
            return CreateState(GetKey(clip), clip, layerIndex);
        }

        /// <summary>
        /// Creates and returns a new <see cref="ClipState"/> to play the 'clip' and registers it with the 'key'.
        /// </summary>
        public ClipState CreateState(object key, AnimationClip clip, int layerIndex = 0)
        {
            return Playable.CreateState(key, clip, layerIndex);
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Calls <see cref="GetOrCreateState(AnimationClip)"/> for each of the specified clips.
        /// </summary>
        public void CreateStates(AnimationClip clip0, AnimationClip clip1, int layerIndex = 0)
        {
            GetOrCreateState(clip0, layerIndex);
            GetOrCreateState(clip1, layerIndex);
        }

        /// <summary>
        /// Calls <see cref="GetOrCreateState(AnimationClip)"/> for each of the specified clips.
        /// </summary>
        public void CreateStates(AnimationClip clip0, AnimationClip clip1, AnimationClip clip2, int layerIndex = 0)
        {
            GetOrCreateState(clip0, layerIndex);
            GetOrCreateState(clip1, layerIndex);
            GetOrCreateState(clip2, layerIndex);
        }

        /// <summary>
        /// Calls <see cref="GetOrCreateState(AnimationClip)"/> for each of the specified clips.
        /// </summary>
        public void CreateStates(AnimationClip clip0, AnimationClip clip1, AnimationClip clip2, AnimationClip clip3, int layerIndex = 0)
        {
            GetOrCreateState(clip0, layerIndex);
            GetOrCreateState(clip1, layerIndex);
            GetOrCreateState(clip2, layerIndex);
            GetOrCreateState(clip3, layerIndex);
        }

        /// <summary>
        /// Calls <see cref="GetOrCreateState(AnimationClip)"/> for each of the specified clips.
        /// </summary>
        public void CreateStates(params AnimationClip[] clips)
        {
            CreateStates(0, clips);
        }

        /// <summary>
        /// Calls <see cref="GetOrCreateState(AnimationClip)"/> for each of the specified clips.
        /// </summary>
        public void CreateStates(int layerIndex, params AnimationClip[] clips)
        {
            if (clips == null)
                return;

            var count = clips.Length;
            for (int i = 0; i < count; i++)
            {
                var clip = clips[i];
                if (clip != null)
                    GetOrCreateState(clip, layerIndex);
            }
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Calls <see cref="GetKey"/> then passes the key to <see cref="GetState(object)"/> and returns the result.
        /// </summary>
        public AnimancerState GetState(AnimationClip clip)
        {
            if (clip != null)
                return GetState(GetKey(clip));
            else
                return null;
        }

        /// <summary>
        /// Passes the <see cref="IHasKey.Key"/> into <see cref="GetState(object)"/> and returns the result.
        /// </summary>
        public AnimancerState GetState(IHasKey hasKey)
        {
            if (_Playable != null)
                return _Playable.GetState(hasKey);
            else
                return null;
        }

        /// <summary>
        /// Returns the state associated with the 'key', or null if none exists.
        /// </summary>
        public AnimancerState GetState(object key)
        {
            if (_Playable != null)
                return _Playable.GetState(key);
            else
                return null;
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Calls <see cref="GetKey"/> then passes the key to <see cref="TryGetState(object, AnimancerState)"/> and
        /// returns the result.
        /// </summary>
        public bool TryGetState(AnimationClip clip, out AnimancerState state)
        {
            if (clip != null)
            {
                return TryGetState(GetKey(clip), out state);
            }
            else
            {
                state = null;
                return false;
            }
        }

        /// <summary>
        /// Passes the <see cref="IHasKey.Key"/> into <see cref="TryGetState(object, out AnimancerState)"/>
        /// and returns the result.
        /// </summary>
        public bool TryGetState(IHasKey hasKey, out AnimancerState state)
        {
            if (_Playable != null)
            {
                return _Playable.TryGetState(hasKey, out state);
            }
            else
            {
                state = null;
                return false;
            }
        }

        /// <summary>
        /// If a state is registered with the 'key', this method outputs it as the 'state' and returns true. Otherwise
        /// 'state' is set to null and this method returns false.
        /// </summary>
        public bool TryGetState(object key, out AnimancerState state)
        {
            if (_Playable != null)
            {
                return _Playable.TryGetState(key, out state);
            }
            else
            {
                state = null;
                return false;
            }
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Determines a key for the clip using the <see cref="GetKey"/> method (which uses the 'clip' itself unless
        /// that method is overridden).
        /// Returns the state which registered with that key or creates one if it doesn't exist.
        /// <para></para>
        /// If the state already exists but has the wrong <see cref="AnimancerState.Clip"/>, the 'allowSetClip'
        /// parameter determines what will happen. False causes it to throw an <see cref="ArgumentException"/> while
        /// true allows it to change the <see cref="AnimancerState.Clip"/>. Note that the change is somewhat costly to
        /// performance to use with caution.
        /// </summary>
        /// <exception cref="ArgumentException"/>
        public AnimancerState GetOrCreateState(AnimationClip clip, int layerIndex = 0, bool allowSetClip = false)
        {
            if (clip != null)
                return GetOrCreateState(GetKey(clip), clip, layerIndex, allowSetClip);
            else
                return null;
        }

        /// <summary>
        /// Returns the state registered with the <see cref="IHasKey.Key"/> if there is one. Otherwise
        /// this method uses <see cref="IAnimancerTransition.CreateState"/> to create a new one and registers it with
        /// that key before returning it.
        /// </summary>
        public AnimancerState GetOrCreateState(IAnimancerTransition transition, int layerIndex = 0)
        {
            return Playable.GetOrCreateState(transition, layerIndex);
        }

        /// <summary>
        /// Returns the state which registered with the 'key' or creates one if it doesn't exist.
        /// <para></para>
        /// If the state already exists but has the wrong <see cref="AnimancerState.Clip"/>, the 'allowSetClip'
        /// parameter determines what will happen. False causes it to throw an <see cref="ArgumentException"/> while
        /// true allows it to change the <see cref="AnimancerState.Clip"/>. Note that the change is somewhat costly to
        /// performance to use with caution.
        /// </summary>
        /// <exception cref="ArgumentException"/>
        public AnimancerState GetOrCreateState(object key, AnimationClip clip, int layerIndex = 0, bool allowSetClip = false)
        {
            return Playable.GetOrCreateState(key, clip, layerIndex, allowSetClip);
        }

        /************************************************************************************************************************/

        /// <summary>[<see cref="IAnimationClipSource"/>]
        /// Gathers all the animations in the <see cref="Playable"/>.
        /// </summary>
        public virtual void GetAnimationClips(List<AnimationClip> clips)
        {
            if (IsPlayableInitialised)
                _Playable.GetAnimationClips(clips);

#if UNITY_EDITOR
            var sources = transform.root.GetComponentsInChildren<IAnimancerClipSource>();
            Editor.AnimancerEditorUtilities.GatherAnimationClips(this, clips, sources, (i) => sources[i].Animancer);
#endif
        }

        /************************************************************************************************************************/
        #endregion
        /************************************************************************************************************************/
        #region Layers
        /************************************************************************************************************************/

        /// <summary>[Pro-Only]
        /// The number of animation layers in the graph.
        /// </summary>
        public int LayerCount
        {
            get
            {
                if (IsPlayableInitialised)
                    return _Playable.LayerCount;
                else
                    return 1;
            }
            set { Playable.LayerCount = value; }
        }

        /************************************************************************************************************************/

        /// <summary>[Pro-Only]
        /// If the <see cref="LayerCount"/> is below the specified 'min', this method sets it to that value.
        /// </summary>
        public void SetMinLayerCount(int min)
        {
            Playable.SetMinLayerCount(min);
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Returns the layer at the specified index. If it didn't already exist, this method creates it.
        /// </summary>
        public AnimancerLayer GetLayer(int layerIndex)
        {
            return Playable.GetLayer(layerIndex);
        }

        /************************************************************************************************************************/

        /// <summary>[Pro-Only]
        /// Creates and returns a new <see cref="AnimancerLayer"/>. New layers are set to override earlier layers by
        /// default.
        /// </summary>
        public AnimancerLayer AddLayer()
        {
            return Playable.AddLayer();
        }

        /************************************************************************************************************************/

        /// <summary>[Pro-Only]
        /// Checks whether the layer at the specified index is set to additive blending. Otherwise it will override any
        /// earlier layers.
        /// </summary>
        public bool IsLayerAdditive(int layerIndex)
        {
            return Playable.IsLayerAdditive(layerIndex);
        }

        /// <summary>[Pro-Only]
        /// Sets the layer at the specified index to blend additively with earlier layers (if true) or to override them
        /// (if false). Newly created layers will override by default.
        /// </summary>
        public void SetLayerAdditive(int layerIndex, bool value)
        {
            Playable.SetLayerAdditive(layerIndex, value);
        }

        /************************************************************************************************************************/

        /// <summary>[Pro-Only]
        /// Sets an <see cref="AvatarMask"/> to determine which bones the layer at the specified index will affect.
        /// </summary>
        public void SetLayerMask(int layerIndex, AvatarMask mask)
        {
            Playable.SetLayerMask(layerIndex, mask);
        }

        /************************************************************************************************************************/

        /// <summary>[Editor-Conditional]
        /// Sets the inspector display name of the layer at the specified index. Note that layer names are Editor-Only
        /// so any calls to this method will automatically be compiled out of a runtime build.
        /// </summary>
        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        public void SetLayerName(int layerIndex, string name)
        {
            Playable.SetLayerName(layerIndex, name);
        }

        /************************************************************************************************************************/

        /// <summary>Returns layer 0 of the <see cref="Playable"/>.</summary>
        public static implicit operator AnimancerLayer(AnimancerComponent animancer)
        {
            return animancer.Playable;
        }

        /************************************************************************************************************************/
        #endregion
        /************************************************************************************************************************/
        #region Play Management
        /************************************************************************************************************************/

        /// <summary>
        /// Stops all other animations, plays the 'clip', and returns its state.
        /// <para></para>
        /// The animation will continue playing from its current <see cref="AnimancerState.Time"/>.
        /// To restart it from the beginning you can use <c>...Play(clip, layerIndex).Time = 0;</c>.
        /// </summary>
        public AnimancerState Play(AnimationClip clip, int layerIndex = 0)
        {
            var state = GetOrCreateState(GetKey(clip), clip, layerIndex);
            return Play(state);
        }

        /// <summary>
        /// Stops all other animations, plays the 'state', and returns it.
        /// <para></para>
        /// The animation will continue playing from its current <see cref="AnimancerState.Time"/>.
        /// To restart it from the beginning you can use <c>...Play(state).Time = 0;</c>.
        /// </summary>
        public AnimancerState Play(AnimancerState state)
        {
            _Animator.enabled = true;
            return Playable.Play(state);
        }

        /// <summary>
        /// Stops all other animations, plays the animation registered with the 'key', and returns that
        /// state. If no state is registered with the 'key', this method does nothing and returns null.
        /// <para></para>
        /// The animation will continue playing from its current <see cref="AnimancerState.Time"/>.
        /// To restart it from the beginning you can use <c>...Play(key).Time = 0;</c>.
        /// </summary>
        public AnimancerState Play(object key)
        {
            _Animator.enabled = true;
            return Playable.Play(key);
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Starts fading in the 'clip' over the course of the 'fadeDuration' while fading out all others in the same
        /// layer. Returns its state.
        /// <para></para>
        /// If the animation was already playing, it will continue doing so from the current time, unlike
        /// <see cref="CrossFadeFromStart(AnimationClip, float, int)"/>.
        /// <para></para>
        /// If the animation was already playing and fading in with less time remaining than the 'fadeDuration', this
        /// method will allow it to complete the existing fade rather than starting a slower one.
        /// <para></para>
        /// If the layer currently has 0 <see cref="Weight"/>, this method will instead start fading in the layer
        /// itself and simply <see cref="Play"/> the 'state'.
        /// <para></para>
        /// Animancer Lite only allows the default 'fadeDuration' (0.3 seconds) in a runtime build.
        /// </summary>
        public AnimancerState CrossFade(AnimationClip clip, float fadeDuration = AnimancerPlayable.DefaultFadeDuration, int layerIndex = 0)
        {
            var state = GetOrCreateState(GetKey(clip), clip, layerIndex);
            return CrossFade(state, fadeDuration);
        }

        /// <summary>
        /// Starts fading in the 'state' over the course of the 'fadeDuration' while fading out all others in the same
        /// layer. Returns the 'state'.
        /// <para></para>
        /// If the 'state' was already playing, it will continue doing so from the current time, unlike
        /// <see cref="CrossFadeFromStart(AnimancerState, float)"/>.
        /// <para></para>
        /// If the 'state' was already playing and fading in with less time remaining than the 'fadeDuration', this
        /// method will allow it to complete the existing fade rather than starting a slower one.
        /// <para></para>
        /// If the layer currently has 0 <see cref="Weight"/>, this method will instead start fading in the layer
        /// itself and simply <see cref="Play"/> the 'state'.
        /// <para></para>
        /// Animancer Lite only allows the default 'fadeDuration' (0.3 seconds) in a runtime build.
        /// </summary>
        public AnimancerState CrossFade(AnimancerState state, float fadeDuration = AnimancerPlayable.DefaultFadeDuration)
        {
            _Animator.enabled = true;
            return Playable.CrossFade(state, fadeDuration);
        }

        /// <summary>
        /// Starts fading in the animation registered with the 'key' over the course of the 'fadeDuration' while fading
        /// out all others in the same layer. Returns the animation's state (or null if none was registered).
        /// <para></para>
        /// If the animation was already playing, it will continue doing so from the current time, unlike
        /// <see cref="CrossFadeFromStart(object, float)"/>.
        /// <para></para>
        /// If the animation was already playing and fading in with less time remaining than the 'fadeDuration', this
        /// method will allow it to complete the existing fade rather than starting a slower one.
        /// <para></para>
        /// If the layer currently has 0 <see cref="Weight"/>, this method will instead start fading in the layer
        /// itself and simply <see cref="Play"/> the 'state'.
        /// <para></para>
        /// Animancer Lite only allows the default 'fadeDuration' (0.3 seconds) in a runtime build.
        /// </summary>
        public AnimancerState CrossFade(object key, float fadeDuration = AnimancerPlayable.DefaultFadeDuration)
        {
            _Animator.enabled = true;
            return Playable.CrossFade(key, fadeDuration);
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Starts fading in the 'clip' from the start over the course of the 'fadeDuration' while fading out all
        /// others in the same layer. Returns its state.
        /// <para></para>
        /// If the animation isn't currently at 0 <see cref="AnimancerState.Weight"/>, this method will actually fade
        /// it to 0 along with the others and create and return a new state with the same clip to fade to 1. This
        /// ensures that calling this method will always fade out from all current states and fade in from the start of
        /// the desired animation. States created for this purpose are cached so they can be reused in the future.
        /// <para></para>
        /// Calling this method repeatedly on subsequent frames will probably have undesirable effects; you most likely
        /// want to use <see cref="CrossFade(AnimationClip, float, int)"/> instead.
        /// <para></para>
        /// If the layer currently has 0 <see cref="Weight"/>, this method will instead start fading in the layer
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
        public AnimancerState CrossFadeFromStart(AnimationClip clip, float fadeDuration = AnimancerPlayable.DefaultFadeDuration,
            int layerIndex = 0)
        {
            var state = GetOrCreateState(GetKey(clip), clip, layerIndex);
            return CrossFadeFromStart(state, fadeDuration);
        }

        /// <summary>
        /// Starts fading in the 'state' from the start over the course of the 'fadeDuration' while fading out all
        /// others in the same layer. Returns the 'state'.
        /// <para></para>
        /// If the 'state' isn't currently at 0 <see cref="AnimancerState.Weight"/>, this method will actually fade it
        /// to 0 along with the others and create and return a new state with the same clip to fade to 1. This ensures
        /// that calling this method will always fade out from all current states and fade in from the start of the
        /// desired animation. States created for this purpose are cached so they can be reused in the future.
        /// <para></para>
        /// Calling this method repeatedly on subsequent frames will probably have undesirable effects; you most likely
        /// want to use <see cref="CrossFade(AnimancerState, float)"/> instead.
        /// <para></para>
        /// If the layer currently has 0 <see cref="Weight"/>, this method will instead start fading in the layer
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
            _Animator.enabled = true;
            return Playable.CrossFadeFromStart(state, fadeDuration);
        }

        /// <summary>
        /// Starts fading in the animation registered with the 'key' from the start over the course of the
        /// 'fadeDuration' while fading out all others in the same layer. Returns its state.
        /// <para></para>
        /// If the animation isn't currently at 0 <see cref="AnimancerState.Weight"/>, this method will actually fade
        /// it to 0 along with the others and create and return a new state with the same clip to fade to 1. This
        /// ensures that calling this method will always fade out from all current states and fade in from the start of
        /// the desired animation. States created for this purpose are cached so they can be reused in the future.
        /// <para></para>
        /// Calling this method repeatedly on subsequent frames will probably have undesirable effects; you most likely
        /// want to use <see cref="CrossFade(object, float)"/> instead.
        /// <para></para>
        /// If the layer currently has 0 <see cref="Weight"/>, this method will instead start fading in the layer
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
        public AnimancerState CrossFadeFromStart(object key, float fadeDuration = AnimancerPlayable.DefaultFadeDuration)
        {
            _Animator.enabled = true;
            return Playable.CrossFadeFromStart(key, fadeDuration);
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Calls either <see cref="Play(AnimancerState)"/>, <see cref="CrossFade(AnimancerState, float)"/>, or
        /// <see cref="CrossFadeFromStart(AnimancerState, float)"/> based on the details of the 'transition'.
        /// </summary>
        public AnimancerState Transition(IAnimancerTransition transition, int layerIndex = 0)
        {
            _Animator.enabled = true;
            return Playable.Transition(transition, layerIndex);
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Gets the state associated with the 'clip', stops and rewinds it to the start, then returns it.
        /// </summary>
        public AnimancerState Stop(AnimationClip clip)
        {
            return Stop(GetKey(clip));
        }

        /// <summary>
        /// Gets the state registered with the <see cref="IHasKey.Key"/>, stops and rewinds it to the start, then
        /// returns it.
        /// </summary>
        public AnimancerState Stop(IHasKey hasKey)
        {
            if (_Playable != null)
                return _Playable.Stop(hasKey);
            else
                return null;
        }

        /// <summary>
        /// Gets the state associated with the 'key', stops and rewinds it to the start, then returns it.
        /// </summary>
        public AnimancerState Stop(object key)
        {
            if (_Playable != null)
                return _Playable.Stop(key);
            else
                return null;
        }

        /// <summary>
        /// Stops all animations and rewinds them to the start.
        /// </summary>
        public void Stop()
        {
            if (_Playable != null)
                _Playable.Stop();
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Returns true if a state is registered for the 'clip' and it is currently playing.
        /// <para></para>
        /// The actual dictionary key is determined using <see cref="GetKey"/>.
        /// </summary>
        public bool IsPlaying(AnimationClip clip)
        {
            return IsPlaying(GetKey(clip));
        }

        /// <summary>
        /// Returns true if a state is registered with the <see cref="IHasKey.Key"/> and it is currently playing.
        /// </summary>
        public bool IsPlaying(IHasKey hasKey)
        {
            if (_Playable != null)
                return _Playable.IsPlaying(hasKey);
            else
                return false;
        }

        /// <summary>
        /// Returns true if a state is registered with the 'key' and it is currently playing.
        /// </summary>
        public bool IsPlaying(object key)
        {
            if (_Playable != null)
                return _Playable.IsPlaying(key);
            else
                return false;
        }

        /// <summary>
        /// Returns true if at least one animation is being played.
        /// </summary>
        public bool IsPlaying()
        {
            if (_Playable != null)
                return _Playable.IsPlaying();
            else
                return false;
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Returns true if the 'clip' is currently being played by at least one state.
        /// <para></para>
        /// This method is inefficient because it searches through every state to find any that are playing the 'clip',
        /// unlike <see cref="IsPlaying(AnimationClip)"/> which only checks the state registered using the 'clip's key.
        /// </summary>
        public bool IsPlayingClip(AnimationClip clip)
        {
            if (_Playable != null)
                return _Playable.IsPlayingClip(clip);
            else
                return false;
        }

        /// <summary>
        /// Returns true if the 'clip' is currently being played by at least one state.
        /// <para></para>
        /// This method is inefficient because it searches through every state to find any that are playing the 'clip',
        /// unlike <see cref="IsPlaying(AnimationClip)"/> which only checks the state registered using the 'clip's key.
        /// </summary>
        public bool IsPlayingClip(AnimationClip clip, int layerIndex)
        {
            if (_Playable != null)
                return _Playable.IsPlayingClip(clip, layerIndex);
            else
                return false;
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Evaluates all of the currently playing animations to apply their states to the animated objects.
        /// </summary>
        public void Evaluate()
        {
            if (_Playable != null)
                _Playable.Evaluate();
        }

        /// <summary>
        /// Advances all currently playing animations by the specified amount of time (in seconds) and evaluates the
        /// graph to apply their states to the animated objects.
        /// </summary>
        public void Evaluate(float deltaTime)
        {
            if (_Playable != null)
                _Playable.Evaluate(deltaTime);
        }

        /************************************************************************************************************************/
        #region Key Error Methods
#if UNITY_EDITOR
        /************************************************************************************************************************/
        // These are overloads of other methods that take a System.Object key to ensure the user doesn't try to use an
        // AnimancerState as a key, since the whole point of a key is to identify a state in the first place.
        /************************************************************************************************************************/
        // State Creation and Access.
        /************************************************************************************************************************/

        /// <summary>[Warning] You cannot use an AnimancerState as a key. The whole point of a key is to identify a state in the first place.</summary>
        [Obsolete("You cannot use an AnimancerState as a key. The whole point of a key is to identify a state in the first place.", true)]
        public AnimancerState GetState(AnimancerState key)
        {
            return key;
        }

        /// <summary>[Warning] You cannot use an AnimancerState as a key. The whole point of a key is to identify a state in the first place.</summary>
        [Obsolete("You cannot use an AnimancerState as a key. The whole point of a key is to identify a state in the first place.", true)]
        public bool TryGetState(AnimancerState key, out AnimancerState state)
        {
            state = key;
            return true;
        }

        /// <summary>[Warning] You cannot use an AnimancerState as a key. The whole point of a key is to identify a state in the first place.</summary>
        [Obsolete("You cannot use an AnimancerState as a key. The whole point of a key is to identify a state in the first place.", true)]
        public AnimancerState GetOrCreateState(AnimancerState key, AnimationClip clip, int layerIndex = 0)
        {
            return key;
        }

        /************************************************************************************************************************/
        // Play Management.
        /************************************************************************************************************************/

        /// <summary>[Warning]
        /// Transitions should be started using <see cref="Transition"/> so they can choose between
        /// <see cref="Play(AnimancerState)"/>, <see cref="CrossFade(AnimancerState, float)"/>, and
        /// <see cref="CrossFadeFromStart(AnimancerState, float)"/> on their own.
        /// <para></para>
        /// If you want to forcibly use this method, you can call
        /// <code>
        /// var state = GetOrCreateState(transition, layerIndex);
        /// Play(state, fadeDuration);
        /// </code>.
        /// </summary>
        [Obsolete("Transitions should be started using Transition so they can choose between" +
            " Play, CrossFade, and CrossFadeFromStart on their own.", true)]
        public AnimancerState Play(IAnimancerTransition transition)
        {
            return Transition(transition);
        }

        /// <summary>[Warning]
        /// Transitions should be started using <see cref="Transition"/> so they can choose between
        /// <see cref="Play(AnimancerState)"/>, <see cref="CrossFade(AnimancerState, float)"/>, and
        /// <see cref="CrossFadeFromStart(AnimancerState, float)"/> on their own.
        /// <para></para>
        /// If you want to forcibly use this method, you can call
        /// <code>
        /// var state = GetOrCreateState(transition, layerIndex);
        /// CrossFade(state, fadeDuration);
        /// </code>.
        /// </summary>
        [Obsolete("Transitions should be started using Transition so they can choose between" +
            " Play, CrossFade, and CrossFadeFromStart on their own.", true)]
        public AnimancerState CrossFade(IAnimancerTransition transition, float fadeDuration = AnimancerPlayable.DefaultFadeDuration)
        {
            return Transition(transition);
        }

        /// <summary>[Warning]
        /// Transitions should be started using <see cref="Transition"/> so they can choose between
        /// <see cref="Play(AnimancerState)"/>, <see cref="CrossFade(AnimancerState, float)"/>, and
        /// <see cref="CrossFadeFromStart(AnimancerState, float)"/> on their own.
        /// <para></para>
        /// If you want to forcibly use this method, you can call
        /// <code>
        /// var state = GetOrCreateState(transition, layerIndex);
        /// CrossFadeFromStart(state, fadeDuration);
        /// </code>.
        /// </summary>
        [Obsolete("Transitions should be started using Transition so they can choose between" +
            " Play, CrossFade, and CrossFadeFromStart on their own.", true)]
        public AnimancerState CrossFadeFromStart(IAnimancerTransition transition, float fadeDuration = AnimancerPlayable.DefaultFadeDuration)
        {
            return Transition(transition);
        }

        /************************************************************************************************************************/

        /// <summary>[Warning] You cannot use an AnimancerState as a key. Just call Stop() on the state itself.</summary>
        [Obsolete("You cannot use an AnimancerState as a key. Just call Stop() on the state itself.", true)]
        public AnimancerState Stop(AnimancerState key)
        {
            key.Stop();
            return key;
        }

        /// <summary>[Warning] You cannot use an AnimancerState as a key. Just check IsPlaying on the state itself.</summary>
        [Obsolete("You cannot use an AnimancerState as a key. Just check IsPlaying on the state itself.", true)]
        public bool IsPlaying(AnimancerState key)
        {
            return key.IsPlaying;
        }

        /************************************************************************************************************************/
#endif
        #endregion
        /************************************************************************************************************************/
        #region Enumeration
        /************************************************************************************************************************/
        // IEnumerable for 'foreach' statements.
        /************************************************************************************************************************/

        /// <summary>
        /// Returns an enumerator that will iterate through all states in each layer (not states inside mixers).
        /// </summary>
        public IEnumerator<AnimancerState> GetEnumerator()
        {
            if (!IsPlayableInitialised)
                yield break;

            foreach (var state in _Playable)
                yield return state;
        }

        IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }

        /************************************************************************************************************************/
        // IEnumerator for yielding in a coroutine to wait until all animations have stopped.
        /************************************************************************************************************************/

        /// <summary>
        /// Determines if any animations are still playing so this object can be used as a custom yield instruction.
        /// </summary>
        bool IEnumerator.MoveNext()
        {
            if (!IsPlayableInitialised)
                return false;

            return ((IEnumerator)_Playable).MoveNext();
        }

        /// <summary>Returns null.</summary>
        object IEnumerator.Current { get { return null; } }

        /// <summary>Does nothing.</summary>
        void IEnumerator.Reset() { }

        /************************************************************************************************************************/
        #endregion
        /************************************************************************************************************************/
        #endregion
        /************************************************************************************************************************/
    }
}
