// Animancer // Copyright 2019 Kybernetik //

#pragma warning disable IDE0018 // Inline variable declaration.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace Animancer
{
    /// <summary>
    /// A <see cref="PlayableBehaviour"/> which can be used as a substitute for the
    /// <see cref="RuntimeAnimatorController"/> normally used to control an <see cref="Animator"/>.
    /// <para></para>
    /// This class can be used as a custom yield instruction to wait until all animations finish playing.
    /// </summary>
    public sealed class AnimancerPlayable : PlayableBehaviour, IEnumerable<AnimancerState>, IEnumerator, IDisposable, IAnimationClipSource
    {
        /************************************************************************************************************************/
        #region Fields and Properties
        /************************************************************************************************************************/

        /// <summary>The URL of the website where the Animancer documentation is hosted.</summary>
        public const string DocumentationURL = "https://kybernetikgames.github.io/animancer";

        /// <summary>The URL of the website where the Animancer API documentation is hosted.</summary>
        public const string APIDocumentationURL = DocumentationURL + "/api/Animancer";

        /// <summary>
        /// The fade duration for any of the CrossFade methods to use if the caller doesn't specify.
        /// </summary>
        public const float DefaultFadeDuration = 0.3f;

        /************************************************************************************************************************/

        /// <summary>
        /// The underlying <see cref="Playable"/> of this <see cref="PlayableBehaviour"/>.
        /// </summary>
        internal Playable _Playable;

        /// <summary>The <see cref="PlayableGraph"/> containing this <see cref="AnimancerPlayable"/>.</summary>
        internal PlayableGraph _Graph;

        /// <summary>[Internal] The <see cref="AnimationLayerMixerPlayable"/> which manages the animation layers.</summary>
        internal AnimationLayerMixerPlayable _LayerMixer;

        /************************************************************************************************************************/

        /// <summary>
        /// The state of the animation currently being played on layer 0.
        /// <para></para>
        /// Specifically, this is the state that was most recently started using any of the
        /// <see cref="AnimancerLayer"/> Play or CrossFade methods on that layer. States controlled individually via
        /// methods in the <see cref="AnimancerState"/> itself will not register in this property.
        /// </summary>
        public AnimancerState CurrentState { get { return _Layers[0].CurrentState; } }

        /// <summary>
        /// The number of times the <see cref="CurrentState"/> has changed. By storing this value and later comparing
        /// the stored value to the current value, you can determine whether the state has been changed since then,
        /// even if the actual state is the same.
        /// </summary>
        public int CurrentStateID { get { return _Layers[0].CurrentStateID; } }

        /************************************************************************************************************************/

        /// <summary>Indicates whether the <see cref="Graph"/> is currently playing.</summary>
        public bool IsGraphPlaying { get { return _IsGraphPlaying; } }
        private bool _IsGraphPlaying = true;

        /// <summary>The layers which each manage their own set of animations.</summary>
        private List<AnimancerLayer> _Layers;

        /// <summary>All of the animation states in any of the root layers (not inside mixers) currently registered to unique keys.</summary>
        private Dictionary<object, AnimancerState> _RegisteredStates;

        /// <summary>All of the nodes that need to be updated early.</summary>
        private List<IEarlyUpdate> _DirtyEarlyNodes;

        /// <summary>All of the nodes that need to be updated.</summary>
        private List<AnimancerNode> _DirtyNodes;

        /************************************************************************************************************************/

        /// <summary>Determines what time source is used to update the <see cref="Graph"/>.</summary>
        public DirectorUpdateMode UpdateMode
        {
            get { return _Graph.GetTimeUpdateMode(); }
            set { _Graph.SetTimeUpdateMode(value); }
        }

        /************************************************************************************************************************/
        #region KeepPlayablesConnected
        /************************************************************************************************************************/

        private bool _KeepPlayablesConnected;

        /// <summary>
        /// Indicates whether playables should stay connected to the graph at all times.
        /// <para></para>
        /// By default, this value is false so that playables will be disconnected from the graph while they are at 0
        /// weight which stops it from evaluating them every frame and is generally more efficient.
        /// </summary>
        public bool KeepPlayablesConnected
        {
            get { return _KeepPlayablesConnected; }
            set
            {
                _KeepPlayablesConnected = value;
                if (value)
                {
                    var count = _Layers.Count;
                    for (int i = 0; i < count; i++)
                        _Layers[i].ConnectAllChildrenToGraph();
                }
                else
                {
                    var count = _Layers.Count;
                    for (int i = 0; i < count; i++)
                        _Layers[i].DisconnectWeightlessChildrenFromGraph();
                }
            }
        }

        /************************************************************************************************************************/
        #endregion
        /************************************************************************************************************************/
        #endregion
        /************************************************************************************************************************/
        #region Initialisation
        /************************************************************************************************************************/

        /// <summary>
        /// Since <see cref="ScriptPlayable{T}.Create(PlayableGraph, int)"/> needs to clone an existing instance, we
        /// keep a static template to avoid allocating an extra garbage one every time.
        /// This is why the fields are assigned in OnPlayableCreate rather than being readonly with field initialisers.
        /// </summary>
        private static readonly AnimancerPlayable Template = new AnimancerPlayable();

        /************************************************************************************************************************/

        /// <summary>
        /// Creates a new <see cref="PlayableGraph"/> containing an <see cref="AnimancerPlayable"/>.
        /// <para></para>
        /// The caller is responsible for calling <see cref="Dispose"/> on the returned object, except in Edit Mode
        /// where it will be called automatically.
        /// </summary>
        public static AnimancerPlayable CreatePlayable(string graphName)
        {
#if UNITY_EDITOR && UNITY_2018_1_OR_NEWER
            var graph = PlayableGraph.Create(graphName + ".Animancer");
#else
            var graph = PlayableGraph.Create();
#endif

            var scriptPlayable = ScriptPlayable<AnimancerPlayable>.Create(graph, Template, 1);
            return scriptPlayable.GetBehaviour();
        }

        /************************************************************************************************************************/

        /// <summary>[Internal]
        /// Called by Unity as it creates an <see cref="AnimancerPlayable"/>.
        /// </summary>
        public override void OnPlayableCreate(Playable playable)
        {
            _Playable = playable;
            playable.SetInputCount(1);
            playable.SetInputWeight(0, 1);

            _Graph = playable.GetGraph();

            _LayerMixer = AnimationLayerMixerPlayable.Create(_Graph, 1);
            _Graph.Connect(_LayerMixer, 0, playable, 0);

            // 4 layers is probably more than most will ever need, but there's not much point allocating a list smaller than that.
            _Layers = new List<AnimancerLayer>(4);
            AddLayer();

            _RegisteredStates = new Dictionary<object, AnimancerState>(FastComparer.Instance);
            _DirtyEarlyNodes = new List<IEarlyUpdate>();
            _DirtyNodes = new List<AnimancerNode>();

#if UNITY_EDITOR
            RegisterToEvaluateInEditMode();
#endif
        }

        /************************************************************************************************************************/

        /// <summary>
        /// An <see cref="IEqualityComparer{T}"/> which ignores overloaded equality operators so it's faster than
        /// <see cref="EqualityComparer{T}.Default"/> for types derived from <see cref="UnityEngine.Object"/>.
        /// </summary>
        public sealed class FastComparer : IEqualityComparer<object>
        {
            /// <summary>Singleton instance.</summary>
            public static readonly FastComparer Instance = new FastComparer();

            /// <summary>Calls <see cref="object.Equals(object, object)"/>.</summary>
            public new bool Equals(object x, object y) { return object.Equals(x, y); }

            /// <summary>Calls <see cref="object.GetHashCode"/>.</summary>
            public int GetHashCode(object obj) { return obj.GetHashCode(); }
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Plays the 'playable' on the specified 'animator'.
        /// </summary>
        public static void Play(Animator animator, AnimancerPlayable playable)
        {
#if UNITY_EDITOR
            // Don't play if the target is a prefab.
            if (UnityEditor.EditorUtility.IsPersistent(animator))
                return;
#endif

            if (playable != null && animator != null)
                AnimationPlayableUtilities.Play(animator, playable._Playable, playable._Graph);
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Returns true as long as the <see cref="Graph"/> hasn't been destroyed (such as by <see cref="Dispose"/>).
        /// </summary>
        public bool IsValid { get { return _Graph.IsValid(); } }

        /// <summary>
        /// Destroys the <see cref="Graph"/> and all its layers and states. This operation cannot be undone.
        /// </summary>
        public void Dispose()
        {
            _Layers = null;
            _RegisteredStates = null;

            // No need to destroy every layer and state individually because destroying the graph will do so anyway.

            if (_Graph.IsValid())
                _Graph.Destroy();
        }

        /************************************************************************************************************************/

#if UNITY_EDITOR
        /// <summary>[Editor-Only]
        /// Ensures that the <see cref="PlayableGraph"/> is destroyed.
        /// </summary>
        ~AnimancerPlayable()
        {
            if (_Layers != null)
                UnityEditor.EditorApplication.delayCall += Dispose;
        }
#endif

        /************************************************************************************************************************/

        /// <summary>
        /// Destroys all states managed by this playable. This operation cannot be undone.
        /// </summary>
        public void DestroyStates()
        {
            var count = _Layers.Count;
            for (int i = 0; i < count; i++)
                _Layers[i].DestroyStates();

            _RegisteredStates.Clear();
        }

        /************************************************************************************************************************/
        #endregion
        /************************************************************************************************************************/
        #region State Creation and Access
        /************************************************************************************************************************/
        #region Create State
        /************************************************************************************************************************/

        /// <summary>
        /// Creates and returns a new <see cref="ClipState"/> to play the 'clip' with this
        /// <see cref="AnimancerPlayable"/> as its parent.
        /// <para></para>
        /// This method does not register the created state with any key, leaving the caller responsible for keeping
        /// track of it.
        /// </summary>
        public ClipState CreateState(AnimationClip clip, int layerIndex = 0)
        {
            SetMinLayerCount(layerIndex + 1);
            return new ClipState(_Layers[layerIndex], clip);
        }

        /// <summary>
        /// Creates and returns a new <see cref="ClipState"/> to play the 'clip' and registers it with the 'key'.
        /// </summary>
        public ClipState CreateState(object key, AnimationClip clip, int layerIndex = 0)
        {
            var state = CreateState(clip, layerIndex);
            RegisterState(key, state);
            return state;
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> if this is not the <see cref="AnimancerNode.Root"/>.
        /// </summary>
        /// <exception cref="ArgumentException"/>
        public void ValidateNode(AnimancerNode node)
        {
            if (node.Root != this)
                throw new ArgumentException("AnimancerNode.Root mismatch:" +
                    " you are attempting to use a node in an AnimancerPlayable that isn't it's root: " + node);
        }

        /************************************************************************************************************************/

        /// <summary>Returns layer 0.</summary>
        public static implicit operator AnimancerLayer(AnimancerPlayable playable)
        {
            return playable.GetLayer(0);
        }

        /************************************************************************************************************************/
        #endregion
        /************************************************************************************************************************/
        #region Get and Set State
        /************************************************************************************************************************/

        /// <summary>
        /// Passes the <see cref="IHasKey.Key"/> into <see cref="GetState(object)"/> and returns the result.
        /// </summary>
        public AnimancerState GetState(IHasKey hasKey)
        {
            return GetState(hasKey.Key);
        }

        /// <summary>
        /// Returns the state registered with the 'key', or null if none exists.
        /// </summary>
        public AnimancerState GetState(object key)
        {
            AnimancerState state;
            TryGetState(key, out state);
            return state;
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Passes the <see cref="IHasKey.Key"/> into <see cref="TryGetState(object, out AnimancerState)"/>
        /// and returns the result.
        /// </summary>
        public bool TryGetState(IHasKey hasKey, out AnimancerState state)
        {
            if (hasKey != null)
            {
                return TryGetState(hasKey.Key, out state);
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
            return _RegisteredStates.TryGetValue(key, out state);
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Returns the state registered with the 'transition's <see cref="IHasKey.Key"/> if there is one. Otherwise
        /// this method uses <see cref="IAnimancerTransition.CreateState"/> to create a new one and registers it with
        /// that key before returning it.
        /// </summary>
        public AnimancerState GetOrCreateState(IAnimancerTransition transition, int layerIndex = 0)
        {
            AnimancerState state;

            var key = transition.Key;

            state = GetState(key);
            if (state == null)
            {
                state = transition.CreateState(GetLayer(layerIndex));
                RegisterState(key, state);
            }

            return state;
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
            if (key == null)
                throw new ArgumentNullException("key");

            AnimancerState state;
            if (TryGetState(key, out state))
            {
                // Complain if a state exists with the 'key' but has the wrong clip or the wrong layer.
                // If a state exists with the 'key' but has the wrong clip, destroy and replace it.
                if (!ReferenceEquals(state.Clip, clip))
                {
                    if (allowSetClip)
                    {
                        state.Clip = clip;
                    }
                    else
                    {
                        throw new ArgumentException(string.Concat(
                            "A state already exists using the specified 'key', but has a different AnimationClip.",
                            "\n - Key: ", key.ToString(),
                            "\n - Existing Clip: ", state.Clip.ToString(),
                            "\n - New Clip: ", clip.ToString()));
                    }
                }
                // Otherwise make sure it is on the correct layer.
                else
                {
                    state.LayerIndex = layerIndex;
                }
            }
            else
            {
                state = CreateState(key, clip, layerIndex);
            }

            return state;
        }

        /************************************************************************************************************************/

        /// <summary>[Internal]
        /// Registers the 'state' in the internal dictionary so the 'key' can be used to get it later on using
        /// <see cref="GetState(object)"/>.
        /// </summary>
        internal void RegisterState(object key, AnimancerState state)
        {
            if (key != null)
                _RegisteredStates.Add(key, state);

            state._Key = key;
        }

        /// <summary>[Internal]
        /// Removes the 'state' from the internal dictionary.
        /// </summary>
        internal void UnregisterState(AnimancerState state)
        {
            if (state._Key == null)
                return;

            _RegisteredStates.Remove(state._Key);
            state._Key = null;
        }

        /************************************************************************************************************************/
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
            var count = _Layers.Count;
            for (int i = 0; i < count; i++)
            {
                foreach (var state in _Layers[i])
                {
                    yield return state;
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }

        /************************************************************************************************************************/

        /// <summary>
        /// Returns an enumerator that will iterate through all layers.
        /// </summary>
        public IEnumerator<AnimancerLayer> GetLayerEnumerator()
        {
            return _Layers.GetEnumerator();
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Returns an enumerator for the dictionary containing all currently registered keys and the animation states
        /// they are mapped to.
        /// </summary>
        public IEnumerator<KeyValuePair<object, AnimancerState>> GetRegisteredStateEnumerator()
        {
            return _RegisteredStates.GetEnumerator();
        }

        /************************************************************************************************************************/
        // IEnumerator for yielding in a coroutine to wait until animations have stopped.
        /************************************************************************************************************************/

        /// <summary>
        /// Determines if any animations are still playing so this object can be used as a custom yield instruction.
        /// </summary>
        bool IEnumerator.MoveNext()
        {
            var count = _Layers.Count;
            for (int i = 0; i < count; i++)
            {
                if (_Layers[i].IsPlayingAndNotEnding())
                    return true;
            }

            return false;
        }

        /// <summary>Returns null.</summary>
        object IEnumerator.Current { get { return null; } }

        /// <summary>Does nothing.</summary>
        void IEnumerator.Reset() { }

        /************************************************************************************************************************/

        /// <summary>[<see cref="IAnimationClipSource"/>]
        /// Gathers all the animations in all layers.
        /// </summary>
        public void GetAnimationClips(List<AnimationClip> clips)
        {
            var count = _Layers.Count;
            for (int i = 0; i < count; i++)
            {
                _Layers[i].GetAnimationClips(clips);
            }
        }

        /************************************************************************************************************************/

#if UNITY_EDITOR
        /// <summary>
        /// [Editor-Only]
        /// Appends a detailed descrption of all currently playing states and other registered states.
        /// </summary>
        public void AppendDescription(StringBuilder description)
        {
            var editors = new List<Editor.AnimancerLayerDrawer>();
            int count;
            Editor.AnimancerLayerDrawer.GatherLayerEditors(this, editors, out count);

            for (int i = 0; i < count; i++)
            {
                var editor = editors[i];

#if UNITY_EDITOR
                description.AppendLine(editor.Layer.Name);
#endif

                description.Append("    Active States: ").Append(editor.ActiveStates.Count);
                var stateCount = editor.ActiveStates.Count;
                for (int j = 0; j < stateCount; j++)
                {
                    description.AppendLine();
                    description.Append("        ");
                    editor.ActiveStates[j].AppendDescription(description, true, true, ",  ");
                }

                description.AppendLine();

                description.Append("    Inactive States: ").Append(editor.InactiveStates.Count);
                stateCount = editor.InactiveStates.Count;
                for (int j = 0; j < stateCount; j++)
                {
                    description.AppendLine();
                    description.Append("        ");
                    editor.InactiveStates[j].AppendDescription(description, true, true, ",  ");
                }
            }
        }

        /// <summary>Appends a detailed descrption of all currently playing states and other registered states.</summary>
        public string GetDescription()
        {
            var description = new StringBuilder();
            AppendDescription(description);
            return description.ToString();
        }
#endif

        /************************************************************************************************************************/
        #endregion
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
            get { return _Layers.Count; }
            set
            {
                int count = _Layers.Count;
                if (count < value)// Increasing.
                {
#if UNITY_EDITOR
                    if (value > maxLayerCount)
                    {
                        Debug.LogWarning("The specified 'LayerCount' " + count + " is greater than the Max Layer Count (" +
                            maxLayerCount + "). If you actually intend to use that many layers" +
                            " you can simply call AnimancerPlayable.SetMaxLayerCount to increase the limit.");
                    }
#endif

                    while (count++ < value)
                        AddLayer();
                }
                else// Decreasing.
                {
                    while (count-- > value)
                    {
                        var layer = _Layers[count];
                        if (layer._Playable.IsValid())
                            _Graph.DestroySubgraph(layer._Playable);
                        layer.DestroyStates();
                    }

                    _Layers.RemoveRange(value, _Layers.Count - value);
                    _LayerMixer.SetInputCount(value);
                }
            }
        }

        /************************************************************************************************************************/

        /// <summary>[Pro-Only]
        /// If the <see cref="LayerCount"/> is below the specified 'min', this method increases it to that value.
        /// </summary>
        public void SetMinLayerCount(int min)
        {
            if (LayerCount < min)
                LayerCount = min;
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Returns the layer at the specified index. If it didn't already exist, this method creates it.
        /// </summary>
        public AnimancerLayer GetLayer(int layerIndex)
        {
            SetMinLayerCount(layerIndex + 1);
            return _Layers[layerIndex];
        }

        /************************************************************************************************************************/

        /// <summary>
        /// The maximum number of layers that can be created before an <see cref="ArgumentOutOfRangeException"/> will
        /// be thrown (default 4).
        /// <para></para>
        /// Lowering this value will not affect layers that have already been created.
        /// </summary>
        public static int maxLayerCount = 4;

        /// <summary>[Pro-Only]
        /// Creates and returns a new <see cref="AnimancerLayer"/>. New layers will override earlier layers by default.
        /// </summary>
        public AnimancerLayer AddLayer()
        {
            int layerIndex = _Layers.Count;

            if (layerIndex >= maxLayerCount)
                throw new ArgumentOutOfRangeException(
                    "Attempted to increase the layer count above the specified limit." +
                    " Set AnimancerPlayable.maxLayerCount if you need to increase the maximum.");

            _LayerMixer.SetInputCount(layerIndex + 1);

            var layer = new AnimancerLayer(this, layerIndex);
            _Graph.Connect(layer._Playable, 0, _LayerMixer, layerIndex);
            _Layers.Add(layer);

            return layer;
        }

        /************************************************************************************************************************/

        /// <summary>[Pro-Only]
        /// Checks whether the layer at the specified index is set to additive blending. Otherwise it will override any
        /// earlier layers.
        /// </summary>
        public bool IsLayerAdditive(int layerIndex)
        {
            return _LayerMixer.IsLayerAdditive((uint)layerIndex);
        }

        /// <summary>[Pro-Only]
        /// Sets the layer at the specified index to blend additively with earlier layers (if true) or to override them
        /// (if false). Newly created layers will override by default.
        /// </summary>
        public void SetLayerAdditive(int layerIndex, bool value)
        {
            SetMinLayerCount(layerIndex + 1);
            _LayerMixer.SetLayerAdditive((uint)layerIndex, value);
        }

        /************************************************************************************************************************/

        /// <summary>[Pro-Only]
        /// Sets an <see cref="AvatarMask"/> to determine which bones the layer at the specified index will affect.
        /// </summary>
        public void SetLayerMask(int layerIndex, AvatarMask mask)
        {
            SetMinLayerCount(layerIndex + 1);

#if UNITY_EDITOR
            _Layers[layerIndex]._Mask = mask;
#endif

            if (mask == null)
                mask = new AvatarMask();

            _LayerMixer.SetLayerMaskFromAvatarMask((uint)layerIndex, mask);
        }

        /************************************************************************************************************************/

        /// <summary>[Editor-Conditional]
        /// Sets the inspector display name of the layer at the specified index. Note that layer names are Editor-Only
        /// so any calls to this method will automatically be compiled out of a runtime build.
        /// </summary>
        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        public void SetLayerName(int layerIndex, string name)
        {
            GetLayer(layerIndex).SetName(name);
        }

        /************************************************************************************************************************/
        #endregion
        /************************************************************************************************************************/
        #region Play Management
        /************************************************************************************************************************/

        /// <summary>
        /// Stops all other animations, plays the 'state', and returns it.
        /// <para></para>
        /// The animation will continue playing from its current <see cref="AnimancerState.Time"/>.
        /// If you wish to force it back to the start, you can simply set the 'state's time to 0.
        /// </summary>
        public AnimancerState Play(AnimancerState state)
        {
            ValidateNode(state);
            var layer = state.Layer;
            layer.Weight = 1;
            return layer.Play(state);
        }

        /// <summary>
        /// Stops all other animations, plays the animation registered with the 'key', and returns that
        /// state. If no state is registered with the 'key', this method does nothing and returns null.
        /// <para></para>
        /// The animation will continue playing from its current <see cref="AnimancerState.Time"/>.
        /// If you wish to force it back to the start, you can simply set the returned state's time to 0.
        /// on the returned state.
        /// </summary>
        public AnimancerState Play(object key)
        {
            AnimancerState state;
            if (TryGetState(key, out state))
                return Play(state);
            else
                return null;
        }

        /************************************************************************************************************************/

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
        public AnimancerState CrossFade(AnimancerState state, float fadeDuration = DefaultFadeDuration)
        {
            if (fadeDuration <= 0)
                return Play(state);

            ValidateNode(state);

            // The base layer should always be at weight 1, but otherwise we can fade the layer in.
            var layer = state.Layer;
            if (layer.PortIndex == 0)
            {
                if (layer.Weight != 1)
                    return Play(state);
            }
            else
            {
                layer.StartFade(1, fadeDuration);
            }

            return layer.CrossFade(state, fadeDuration);
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
        public AnimancerState CrossFade(object key, float fadeDuration = DefaultFadeDuration)
        {
            AnimancerState state;
            if (TryGetState(key, out state))
                return CrossFade(state, fadeDuration);
            else
                return null;
        }

        /************************************************************************************************************************/

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
        public AnimancerState CrossFadeFromStart(AnimancerState state, float fadeDuration = DefaultFadeDuration)
        {
            if (fadeDuration <= 0)
                return Play(state);

            ValidateNode(state);

            // The base layer should always be at weight 1, but otherwise we can fade the layer in.
            var layer = state.Layer;
            if (layer.PortIndex == 0)
            {
                if (layer.Weight != 1)
                    return Play(state);
            }
            else
            {
                layer.StartFade(1, fadeDuration);
            }

            return layer.CrossFadeFromStart(state, fadeDuration);
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
        public AnimancerState CrossFadeFromStart(object key, float fadeDuration = DefaultFadeDuration)
        {
            AnimancerState state;
            if (TryGetState(key, out state))
                return CrossFadeFromStart(state, fadeDuration);
            else
                return null;
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Calls either <see cref="Play(AnimancerState)"/>, <see cref="CrossFade(AnimancerState, float)"/>, or
        /// <see cref="CrossFadeFromStart(AnimancerState, float)"/> based on the details of the 'transition'.
        /// </summary>
        public AnimancerState Transition(IAnimancerTransition transition, int layerIndex = 0)
        {
            var state = GetOrCreateState(transition, layerIndex);

            var fadeDuration = GetFadeOutDuration(transition.FadeDuration);
            if (fadeDuration <= 0)
            {
                state = Play(state);
            }
            else
            {
                if (transition.CrossFadeFromStart)
                    state = CrossFadeFromStart(state, fadeDuration);
                else
                    state = CrossFade(state, fadeDuration);
            }

            transition.Apply(state);
            return state;
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Gets the state registered with the <see cref="IHasKey.Key"/>, stops and rewinds it to the start, then
        /// returns it.
        /// </summary>
        public AnimancerState Stop(IHasKey hasKey)
        {
            return Stop(hasKey.Key);
        }

        /// <summary>
        /// Calls <see cref="AnimancerState.Stop"/> on the state registered with the 'key' to stop it from playing and
        /// rewind it to the start.
        /// </summary>
        public AnimancerState Stop(object key)
        {
            AnimancerState state;
            if (TryGetState(key, out state))
                state.Stop();

            return state;
        }

        /// <summary>
        /// Calls <see cref="AnimancerState.Stop"/> on all animations to stop them from playing and rewind them to the
        /// start.
        /// </summary>
        public void Stop()
        {
            if (_Layers != null)
            {
                var count = _Layers.Count;
                for (int i = 0; i < count; i++)
                    _Layers[i].Stop();
            }
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Returns true if a state is registered with the <see cref="IHasKey.Key"/> and it is currently playing.
        /// </summary>
        public bool IsPlaying(IHasKey hasKey)
        {
            return IsPlaying(hasKey.Key);
        }

        /// <summary>
        /// Returns true if a state is registered with the 'key' and it is currently playing.
        /// </summary>
        public bool IsPlaying(object key)
        {
            AnimancerState state;

            return
                TryGetState(key, out state) &&
                state.IsPlaying;
        }

        /// <summary>
        /// Returns true if at least one animation is being played.
        /// </summary>
        public bool IsPlaying()
        {
            if (!_IsGraphPlaying)
                return false;

            var count = _Layers.Count;
            for (int i = 0; i < count; i++)
            {
                if (_Layers[i].IsPlaying())
                    return true;
            }

            return false;
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Returns true if the 'clip' is currently being played by at least one state in any layer.
        /// <para></para>
        /// This method is inefficient because it searches through every state to find any that are playing the 'clip',
        /// unlike <see cref="IsPlaying(AnimationClip)"/> which only checks the state registered using the 'clip's key.
        /// </summary>
        public bool IsPlayingClip(AnimationClip clip)
        {
            if (!_IsGraphPlaying)
                return false;

            var count = _Layers.Count;
            for (int i = 0; i < count; i++)
            {
                if (_Layers[i].IsPlayingClip(clip))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Returns true if the 'clip' is currently being played by at least one state in the specified layer.
        /// <para></para>
        /// This method is inefficient because it searches through every state to find any that are playing the 'clip',
        /// unlike <see cref="IsPlaying(AnimationClip)"/> which only checks the state registered using the 'clip's key.
        /// </summary>
        public bool IsPlayingClip(AnimationClip clip, int layerIndex)
        {
            if (!_IsGraphPlaying)
                return false;

            return _Layers[layerIndex].IsPlayingClip(clip);
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Resumes playing the <see cref="Graph"/> if <see cref="PauseGraph"/> was called previously.
        /// </summary>
        public void UnpauseGraph()
        {
            if (!_IsGraphPlaying)
            {
                _Graph.Play();
                _IsGraphPlaying = true;
            }
        }

        /// <summary>
        /// Freezes the <see cref="Graph"/> at its current state.
        /// <para></para>
        /// If you call this method, you are responsible for calling <see cref="UnpauseGraph"/> to resume playing.
        /// </summary>
        public void PauseGraph()
        {
            if (_IsGraphPlaying)
            {
                _Graph.Stop();
                _IsGraphPlaying = false;
            }
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Evaluates all of the currently playing animations to apply their states to the animated objects.
        /// </summary>
        public void Evaluate()
        {
            _Graph.Evaluate();
        }

        /// <summary>
        /// Advances all currently playing animations by the specified amount of time (in seconds) and evaluates the
        /// graph to apply their states to the animated objects.
        /// </summary>
        public void Evaluate(float deltaTime)
        {
            _Graph.Evaluate(deltaTime);
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Calculates the total <see cref="AnimancerState.Weight"/> of all states in this playable.
        /// </summary>
        public float GetTotalWeight()
        {
            float weight = 0;

            var count = _Layers.Count;
            for (int i = 0; i < count; i++)
            {
                weight += _Layers[i].GetTotalWeight();
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

                var count = _Layers.Count;
                for (int i = 0; i < count; i++)
                {
                    var layer = _Layers[i];
                    if (layer == null)
                        continue;

                    velocity += layer.AverageVelocity;
                }

                return velocity;
            }
        }

        /************************************************************************************************************************/
        #endregion
        /************************************************************************************************************************/
        #region End Events
        /************************************************************************************************************************/

        /// <summary>
        /// Invokes the <see cref="AnimancerState.OnEnd"/> event of the state that is playing the animation which
        /// triggered the event. Returns true if such a state exists (even if it doesn't have a callback registered).
        /// </summary>
        public bool OnEndEventReceived(AnimationEvent animationEvent)
        {
            // This method could be changed to invoke all events with the correct clip and weight by collecting all the
            // events into a list and invoking them at the end.

            var count = _Layers.Count;
            for (int i = 0; i < count; i++)
            {
                if (TryInvokeOnEndEvent(animationEvent, _Layers[i].CurrentState))
                    return true;
            }

            for (int i = 0; i < count; i++)
            {
                if (_Layers[i].TryInvokeOnEndEvent(animationEvent))
                    return true;
            }

            return false;
        }

        /************************************************************************************************************************/

        /// <summary>
        /// If the <see cref="AnimancerState.Clip"/> and <see cref="AnimancerNode.Weight"/> match the
        /// <see cref="AnimationEvent"/>, this method invokes the <see cref="AnimancerState.OnEnd"/> event and returns
        /// true.
        /// </summary>
        internal static bool TryInvokeOnEndEvent(AnimationEvent animationEvent, AnimancerState state)
        {
            if (state.Weight != animationEvent.animatorClipInfo.weight ||
                state.Clip != animationEvent.animatorClipInfo.clip)
                return false;

            if (state.OnEnd != null)
            {
#if UNITY_EDITOR
                if (CurrentEndEvent != null)
                    throw new InvalidOperationException("Recursive call to TryInvokeOnEndEvent detected");
#endif

                try
                {
                    CurrentEndEvent = animationEvent;
                    CurrentlyEnding = state;
                    state.OnEnd();
                }
                finally
                {
                    CurrentEndEvent = null;
                    CurrentlyEnding = null;
                }
            }

            return true;
        }

        /************************************************************************************************************************/

        /// <summary>
        /// The <see cref="AnimationEvent"/> called 'End' which is currently being triggered.
        /// </summary>
        public static AnimationEvent CurrentEndEvent { get; private set; }

        /// <summary>
        /// The <see cref="AnimancerState"/> which is currently having its <see cref="AnimancerState.OnEnd"/> event
        /// triggered by an <see cref="AnimationEvent"/> called 'End'.
        /// </summary>
        public static AnimancerState CurrentlyEnding { get; private set; }

        /// <summary>
        /// If the <see cref="CurrentEndEvent"/> has a float parameter above 0, this method returns that value.
        /// Otherwise it returns either the 'minDuration' or the <see cref="AnimancerState.RemainingDuration"/> of the
        /// <see cref="CurrentlyEnding"/> state (whichever is higher).
        /// </summary>
        public static float GetFadeOutDuration(float minDuration = DefaultFadeDuration)
        {
            if (CurrentEndEvent == null)
                return minDuration;

            if (CurrentEndEvent.floatParameter > 0)
                return CurrentEndEvent.floatParameter;

            float remainingDuration;
            if (CurrentEndEvent.animatorClipInfo.clip.isLooping)
            {
                var length = CurrentlyEnding.Length;
                var time = CurrentlyEnding.Time % length;

#if UNITY_2018_1_OR_NEWER
                var previousTime = (float)CurrentlyEnding._Playable.GetPreviousTime() % length;
#else
                var previousTime = time - CurrentlyEnding.Speed * AnimancerPlayable.DeltaTime;
#endif

                // If we just passed the end of the animation, the remaining duration would technically be the full
                // duration of the animation, so we most likely want to use the minimum duration instead.
                if (Mathf.FloorToInt(time) != Mathf.FloorToInt(previousTime))
                    return minDuration;

                remainingDuration = CurrentlyEnding.RemainingDuration;
            }
            else
            {
                var time = CurrentlyEnding.Time;
                var speed = CurrentlyEnding.Speed;
                if (speed > 0)
                {
                    remainingDuration = (CurrentlyEnding.Length - time) * speed;
                }
                else
                {
                    remainingDuration = time * -speed;
                }
            }

            return Mathf.Max(minDuration, remainingDuration);
        }

        /************************************************************************************************************************/
        #endregion
        /************************************************************************************************************************/
        #region Key Error Methods
#if UNITY_EDITOR
        /************************************************************************************************************************/
        // These are overloads of other methods that take a System.Object key to ensure the user doesn't try to use an
        // AnimancerState as a key, since the whole point of a key is to identify a state in the first place.
        /************************************************************************************************************************/
        // The CrossFadeFromStart methods are exceptions to this as they use one state as the key for its new spare.
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

        /// <summary>[Warning] You cannot use an AnimancerState as a key. Just call <see cref="AnimancerState.Dispose"/>.</summary>
        [Obsolete("You cannot use an AnimancerState as a key. Just call AnimancerState.Destroy.", true)]
        public bool DestroyState(AnimancerState key)
        {
            key.Dispose();
            return true;
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

        /// <summary>[Warning] You cannot use an AnimancerState as a key. Just call <see cref="AnimancerState.Stop"/>.</summary>
        [Obsolete("You cannot use an AnimancerState as a key. Just call AnimancerState.Stop.", true)]
        public AnimancerState Stop(AnimancerState key)
        {
            key.Stop();
            return key;
        }

        /// <summary>[Warning] You cannot use an AnimancerState as a key. Just check <see cref="AnimancerState.IsPlaying"/>.</summary>
        [Obsolete("You cannot use an AnimancerState as a key. Just check AnimancerState.IsPlaying.", true)]
        public bool IsPlaying(AnimancerState key)
        {
            return key.IsPlaying;
        }

        /************************************************************************************************************************/
#endif
        #endregion
        /************************************************************************************************************************/
        #region Internal Methods
        /************************************************************************************************************************/

        /// <summary>
        /// Adds the 'node' to the list of nodes that need to be updated.
        /// </summary>
        public void RequireEarlyUpdate<T>(T node) where T : AnimancerNode, IEarlyUpdate
        {
            ValidateNode(node);
            _DirtyEarlyNodes.Add(node);
        }

        /// <summary>
        /// Adds the 'node' to the list of nodes that need to be updated early.
        /// </summary>
        public void RequireUpdate(AnimancerNode node)
        {
            ValidateNode(node);
            _DirtyNodes.Add(node);
        }

        /************************************************************************************************************************/

        /// <summary>
        /// The current <see cref="FrameData.deltaTime"/>.
        /// <para></para>
        /// After <see cref="PrepareFrame"/>, this property will be left at its most recent value.
        /// </summary>
        public static float DeltaTime { get; private set; }

        /// <summary>
        /// The current <see cref="FrameData.frameId"/>.
        /// <para></para>
        /// <see cref="ClipState"/> uses this value to determine whether it has accessed the playable's time since it
        /// was last updated in order to cache the <see cref="ClipState.Time"/>.
        /// </summary>
        public uint FrameID { get; private set; }

        /************************************************************************************************************************/

        /// <summary>[Internal]
        /// Called by Unity before the <see cref="Graph"/> is evaluated. Updates the playing states.
        /// </summary>
        public override void PrepareFrame(Playable playable, FrameData info)
        {
            DeltaTime = info.deltaTime;

            // Trigger OnEnd events then use a separate loop to update fading, apply weights, and apply playing flags.

            // Doing it all in one loop would mean that if state 2 triggers an event that tries to play state 1, the
            // change to state 1 wouldn't take effect until the next update even though 2 would be stopped immediately,
            // leaving the object without any animations playing for a frame.

            var count = _DirtyEarlyNodes.Count;
            while (--count >= 0)
            {
                var node = _DirtyEarlyNodes[count];
                if (node.PortIndex >= 0)
                {
                    bool needsMoreUpdates;
                    node.EarlyUpdate(out needsMoreUpdates);
                    if (needsMoreUpdates)
                        continue;
                }

                _DirtyEarlyNodes.RemoveAt(count);
            }

            count = _DirtyNodes.Count;
            while (--count >= 0)
            {
                var node = _DirtyNodes[count];
                if (node.PortIndex >= 0)
                {
                    bool needsMoreUpdates;
                    node.UpdateNode(out needsMoreUpdates);
                    if (needsMoreUpdates)
                        continue;
                }

                _DirtyNodes.RemoveAt(count);
            }

            // Any time before or during this method will still have all Playables at their time from last frame.
            FrameID = (uint)info.frameId;
        }

        /************************************************************************************************************************/
        #region Animate in Edit Mode
#if UNITY_EDITOR
        /************************************************************************************************************************/

        /// <summary>[Editor-Only]
        /// Determines whether the Inspector GUI needs to be constantly repainted in Edit Mode.
        /// </summary>
        public static readonly Editor.BoolPref RepaintConstantlyInEditMode = new Editor.BoolPref(
            Editor.AnimancerLayerDrawer.PrefPrefix + "Repaint Constantly while playing in Edit Mode", true);

        private static List<WeakReference> _PlayablesToAnimateInEditMode;

        /// <summary>[Editor-Only]
        /// Registers this object in the list of things that need to be updated in edit-mode.
        /// </summary>
        private void RegisterToEvaluateInEditMode()
        {
            if (UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode)
                return;

            if (_PlayablesToAnimateInEditMode == null)
            {
                _PlayablesToAnimateInEditMode = new List<WeakReference>();

                var previousFrameTime = UnityEditor.EditorApplication.timeSinceStartup;

                UnityEditor.EditorApplication.update += () =>
                {
                    var time = UnityEditor.EditorApplication.timeSinceStartup;
                    var deltaTime = time - previousFrameTime;
                    previousFrameTime = time;

                    var anyPlaying = false;

                    for (int i = 0; i < _PlayablesToAnimateInEditMode.Count; i++)
                    {
                        var playable = _PlayablesToAnimateInEditMode[i].Target as AnimancerPlayable;
                        if (playable != null && playable.IsValid)
                        {
                            if (playable._IsGraphPlaying)
                            {
                                playable.Evaluate((float)deltaTime);
                                anyPlaying = true;
                            }
                        }
                        else
                        {
                            _PlayablesToAnimateInEditMode.RemoveAt(i--);
                        }
                    }

                    if (anyPlaying && RepaintConstantlyInEditMode)
                        UnityEditorInternal.InternalEditorUtility.RepaintAllViews();
                };
            }

            _PlayablesToAnimateInEditMode.Add(new WeakReference(this));

        }

        /************************************************************************************************************************/
#endif
        #endregion
        /************************************************************************************************************************/

#if UNITY_EDITOR
        /// <summary>[Editor-Only]
        /// Returns true if the 'initial' mode was <see cref="AnimatorUpdateMode.AnimatePhysics"/> and the 'current'
        /// has changed to another mode or if the 'initial' mode was something else and the 'current' has changed to
        /// <see cref="AnimatorUpdateMode.AnimatePhysics"/>.
        /// </summary>
        public static bool HasChangedToOrFromAnimatePhysics(AnimatorUpdateMode? initial, AnimatorUpdateMode current)
        {
            if (initial == null)
                return false;

            var wasAnimatePhysics = initial.Value == AnimatorUpdateMode.AnimatePhysics;
            var isAnimatePhysics = current == AnimatorUpdateMode.AnimatePhysics;
            return wasAnimatePhysics != isAnimatePhysics;
        }
#endif

        /************************************************************************************************************************/

#if UNITY_EDITOR
        /// <summary>[Editor-Only]
        /// Draws the <see cref="_DirtyEarlyNodes"/> and <see cref="_DirtyNodes"/> lists.
        /// </summary>
        internal void DoUpdateListGUI()
        {
            Editor.AnimancerEditorUtilities.BeginVerticalBox(GUI.skin.box);

            GUILayout.Label("Dirty Early Nodes " + _DirtyEarlyNodes.Count);
            for (int i = 0; i < _DirtyEarlyNodes.Count; i++)
            {
                GUILayout.Label(_DirtyEarlyNodes[i].ToString());
            }

            GUILayout.Label("Dirty Nodes " + _DirtyNodes.Count);
            for (int i = 0; i < _DirtyNodes.Count; i++)
            {
                GUILayout.Label(_DirtyNodes[i].ToString());
            }

            Editor.AnimancerEditorUtilities.EndVerticalBox(GUI.skin.box);

            if (Editor.AnimancerEditorUtilities.TryUseContextClick(GUILayoutUtility.GetLastRect()))
            {
                var menu = new UnityEditor.GenericMenu();
                Editor.AnimancerLayerDrawer.ShowUpdatingNodes.AddToggleFunction(menu);
                menu.ShowAsContext();
            }
        }
#endif

        /************************************************************************************************************************/
        #endregion
        /************************************************************************************************************************/
    }
}
