// Animancer // Copyright 2019 Kybernetik //

#pragma warning disable IDE0019 // Use pattern matching.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Playables;
using Object = UnityEngine.Object;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Animancer
{
    /// <summary>
    /// Base class for all states in an <see cref="AnimancerPlayable"/> graph.
    /// Each state is a wrapper for a <see cref="Playable"/> in the <see cref="PlayableGraph"/>.
    /// <para></para>
    /// This class can be used as a custom yield instruction to wait until the animation either stops playing or reaches its end.
    /// </summary>
    /// <remarks>
    /// There are various different ways of getting a state:
    /// <list type="bullet">
    ///   <item>
    ///   Use one of the state's constructors. Generally the first parameter is a layer or mixer which will be used as
    ///   the state's parent. If not specified, you will need to call SetParent manually. Also note than an
    ///   AnimancerComponent can be implicitly cast to its first layer.
    ///   </item>
    ///   <item>
    ///   AnimancerController.CreateState creates a new ClipState. You can optionally specify a custom 'key' to
    ///   register it in the dictionary instead of the default (the 'clip' itself).
    ///   </item>
    ///   <item>
    ///   AnimancerController.GetOrCreateState looks for an existing state registered with the specified 'key' and only
    ///   creates a new one if it doesn’t already exist.
    ///   </item>
    ///   <item>
    ///   AnimancerController.GetState returns an existing state registered with the specified 'key' if there is one.
    ///   </item>
    ///   <item>
    ///   AnimancerController.TryGetState is similar but returns a bool to indicate success and returns the 'state'
    ///   as an out parameter.
    ///   </item>
    ///   <item>
    ///   AnimancerController.Play and CrossFade also return the state they play.
    ///   </item>
    /// </list>
    /// <para></para>
    /// Note that when inheriting from this class, the <see cref="_Playable"/> field must be assigned in the
    /// constructor to avoid throwing <see cref="ArgumentException"/>s throughout the system.
    /// </remarks>
    public abstract class AnimancerState : AnimancerNode, IDisposable, IAnimationClipSource
    {
        /************************************************************************************************************************/
        #region Fields and Properties
        /************************************************************************************************************************/
        #region Hierarchy
        /************************************************************************************************************************/

        /// <summary>
        /// The node which receives the output of this node.
        /// </summary>
        public override AnimancerNode Parent { get { return _Parent; } }
        private AnimancerNode _Parent;

        /// <summary>
        /// Connects this state to the 'parent' mixer at the specified 'portIndex'.
        /// <para></para>
        /// See also <see cref="AnimancerLayer.AddChild(AnimancerState)"/> to connect a state to an available port on a
        /// layer.
        /// </summary>
        public void SetParent(AnimancerNode parent, int portIndex)
        {
            if (_Parent != null)
                _Parent.OnRemoveChild(this);

            PortIndex = portIndex;
            _Parent = parent;
            SetWeightDirty();

            if (parent != null)
                parent.OnAddChild(this);
        }

        /************************************************************************************************************************/

        /// <summary>The parent's <see cref="Playable"/>.</summary>
        protected override Playable ParentPlayable { get { return _Parent._Playable; } }

        /// <summary>The <see cref="Parent"/>'s <see cref="AnimancerNode.KeepChildrenConnected"/>.</summary>
        public override bool StayConnectedWhenWeightless { get { return _Parent.KeepChildrenConnected; } }

        /************************************************************************************************************************/

        /// <summary>
        /// The <see cref="AnimancerNode.Weight"/> of this state multiplied by the <see cref="AnimancerNode.Weight"/> of each of
        /// its parents down the hierarchy to determine how much this state affects the final output.
        /// </summary>
        public float EffectiveWeight
        {
            get
            {
                float weight = Weight;

                var parent = _Parent;
                while (parent != null)
                {
                    weight *= parent.Weight;
                    parent = parent.Parent;
                }

                return weight;
            }
        }

        /************************************************************************************************************************/
        // Layer.
        /************************************************************************************************************************/

        /// <summary>The root <see cref="AnimancerLayer"/> which this state is connected to.</summary>
        public override AnimancerLayer Layer { get { return _Parent.Layer; } }

        /// <summary>
        /// The index of the <see cref="AnimancerLayer"/> this state is connected to (determined by the
        /// <see cref="Parent"/>).
        /// </summary>
        public int LayerIndex
        {
            get { return _Parent.Layer.PortIndex; }
            set
            {
                if (_Parent.Layer.PortIndex == value)
                    return;

                Root.GetLayer(value).AddChild(this);
            }
        }

        /************************************************************************************************************************/
        #endregion
        /************************************************************************************************************************/
        #region Assets
        /************************************************************************************************************************/

        /// <summary>The <see cref="AnimationClip"/> which this state plays (if any).</summary>
        /// <exception cref="NotSupportedException">
        /// Thrown if this state type doesn't have a clip and you try to set it.
        /// </exception>
        public virtual AnimationClip Clip
        {
            get { return null; }
            set { throw new NotSupportedException(GetType() + " does not support setting the Clip."); }
        }

        /// <summary>The main object to show in the Inspector for this state (if any).</summary>
        /// <exception cref="NotSupportedException">
        /// Thrown if this state type doesn't have a main object and you try to set it.
        /// </exception>
        /// <exception cref="InvalidCastException">
        /// Thrown if you try to assign something this state can't use.
        /// </exception>
        public virtual Object MainObject
        {
            get { return null; }
            set { throw new NotSupportedException(GetType() + " does not support setting the MainObject."); }
        }

        /************************************************************************************************************************/

        /// <summary>The average velocity of the root motion caused by this state.</summary>
        public abstract Vector3 AverageVelocity { get; }

        /************************************************************************************************************************/
        // Key.
        /************************************************************************************************************************/

        internal object _Key;

        /// <summary>
        /// The object used to identify this state in the root <see cref="AnimancerPlayable"/> dictionary.
        /// </summary>
        public object Key
        {
            get { return _Key; }
            set
            {
                Root.UnregisterState(this);
                Root.RegisterState(value, this);
            }
        }

        /************************************************************************************************************************/
        #endregion
        /************************************************************************************************************************/
        #region Playing Flags
        /************************************************************************************************************************/

        /// <summary>Is the <see cref="Time"/> automatically advancing?</summary>
        private bool _IsPlaying;

        /// <summary>
        /// Has <see cref="_IsPlaying"/> changed since it was last applied to the <see cref="Playable"/>.
        /// </summary>
        /// <remarks>
        /// Playables start playing by default so we start dirty to pause it during the first update (unless
        /// <see cref="IsPlaying"/> is set to true before that).
        /// </remarks>
        private bool _IsPlayingDirty = true;

        /// <summary>Is the <see cref="Time"/> automatically advancing?</summary>
        /// 
        /// <example>
        /// <code>
        /// void IsPlayingExample(AnimancerComponent animancer, AnimationClip clip)
        /// {
        ///     var state = animancer.GetOrCreateState(clip);
        ///     
        ///     if (state.IsPlaying)
        ///         Debug.Log(clip + " is playing");
        ///     else
        ///         Debug.Log(clip + " is paused");
        ///         
        ///     state.IsPlaying = false;// Pause the animation.
        ///         
        ///     state.IsPlaying = true;// Unpause the animation.
        /// }
        /// </code>
        /// </example>
        public virtual bool IsPlaying
        {
            get { return _IsPlaying; }
            set
            {
                if (_IsPlaying == value)
                    return;

                _IsPlaying = value;

                // If it was already dirty then we just returned to the previous state so it is no longer dirty.
                if (_IsPlayingDirty)
                {
                    _IsPlayingDirty = false;
                }
                else
                {
                    _IsPlayingDirty = true;
                    RequireUpdate();
                }
            }
        }

        /// <summary>
        /// Returns true if this state is playing and is at or fading towards a non-zero
        /// <see cref="AnimancerNode.Weight"/>.
        /// </summary>
        public bool IsActive
        {
            get
            {
                return
                    _IsPlaying &&
                    TargetWeight > 0;
            }
        }

        /// <summary>
        /// Returns true if this state is not playing and is at 0 <see cref="AnimancerNode.Weight"/>.
        /// </summary>
        public bool IsStopped
        {
            get
            {
                return
                    !_IsPlaying &&
                    Weight == 0;
            }
        }

        /************************************************************************************************************************/
        #endregion
        /************************************************************************************************************************/
        #region Timing
        /************************************************************************************************************************/
        // Time.
        /************************************************************************************************************************/

        /// <summary>
        /// The number of seconds that have passed since the start of this animation.
        /// <para></para>
        /// This value will continue increasing after the animation passes the end of its <see cref="Length"/> and it
        /// will either freeze in place or start again from the beginning according to whether it is looping or not.
        /// <para></para>
        /// Animancer Lite does not allow this value to be changed in a runtime build (except setting it to 0).
        /// </summary>
        /// <remarks>
        /// Setting this value actually calls <see cref="PlayableExtensions.SetTime"/> twice to ensure that animation
        /// events aren't triggered incorrectly. Calling it only once would trigger any animation events between the
        /// previous time and the new time. So if an animation plays to the end and you set the time back to 0 (such as
        /// by calling <see cref="Stop"/> or playing a different animation), the next time this animation played it
        /// would immediately trigger all of its events, then play through and trigger them normally as well.
        /// </remarks>
        /// 
        /// <example>
        /// <code>
        /// void PlayAnimation(AnimancerComponent animancer, AnimationClip clip)
        /// {
        ///     var state = animancer.Play(clip);
        ///     
        ///     // Start 0.5 seconds into the animation:
        ///     state.Time = 0.5f;
        ///     
        ///     // Start 50% of the way through the animation (0.5 in a range of 0 to 1):
        ///     state.NormalizedTime = 0.5f;
        /// }
        /// </code>
        /// </example>
        public virtual float Time
        {
            get { return (float)_Playable.GetTime(); }
            set
            {

                _Playable.SetTime(value);
                _Playable.SetTime(value);
            }
        }

        /// <summary>
        /// The <see cref="Time"/> of this state as a portion of the animation's <see cref="Length"/>, meaning the
        /// value goes from 0 to 1 as it plays from start to end, regardless of how long that actually takes.
        /// <para></para>
        /// This value will continue increasing after the animation passes the end of its length and it will either
        /// freeze in place or start again from the beginning according to whether it is looping or not.
        /// <para></para>
        /// The fractional part of the value (<c>NormalizedTime % 1</c>) is the percentage (0-1) of progress in the
        /// current loop while the integer part (<c>(int)NormalizedTime</c>) is the number of times the animation has
        /// been looped.
        /// <para></para>
        /// Animancer Lite does not allow this value to be changed to a value other than 0 in a runtime build.
        /// </summary>
        /// <exception cref="NotSupportedException">Thrown if this state doesn't have a length, as indicated by
        /// <see cref="HasLength"/>.</exception>
        /// 
        /// <example>
        /// <code>
        /// void PlayAnimation(AnimancerComponent animancer, AnimationClip clip)
        /// {
        ///     var state = animancer.Play(clip);
        ///     
        ///     // Start 0.5 seconds into the animation:
        ///     state.Time = 0.5f;
        ///     
        ///     // Start 50% of the way through the animation (0.5 in a range of 0 to 1):
        ///     state.NormalizedTime = 0.5f;
        /// }
        /// </code>
        /// </example>
        public float NormalizedTime
        {
            get { return Time / Length; }
            set { Time = value * Length; }
        }

        /************************************************************************************************************************/
        // Speed.
        /************************************************************************************************************************/

        /// <summary>
        /// How fast the <see cref="Time"/> is advancing every frame.
        /// <para></para>
        /// 1 is the normal speed.
        /// <para></para>
        /// A negative value will play the animation backwards.
        /// <para></para>
        /// Animancer Lite does not allow this value to be changed in a runtime build.
        /// </summary>
        /// 
        /// <example>
        /// <code>
        /// void PlayAnimation(AnimancerComponent animancer, AnimationClip clip)
        /// {
        ///     var state = animancer.Play(clip);
        ///     
        ///     state.Speed = 1;// Normal speed.
        ///     state.Speed = 2;// Double speed.
        ///     state.Speed = 0.5f;// Half speed.
        ///     state.Speed = -1;// Normal speed playing backwards.
        /// }
        /// </code>
        /// </example>
        public float Speed
        {
            get { return (float)_Playable.GetSpeed(); }
            set { _Playable.SetSpeed(value); }
        }

        /// <summary>
        /// The number of times per second that the animation will play its complete <see cref="Length"/>.
        /// <para></para>
        /// A negative value will play the animation backwards.
        /// <para></para>
        /// Animancer Lite does not allow this value to be changed in a runtime build.
        /// </summary>
        /// <exception cref="NotSupportedException">Thrown if this state doesn't have a length, as indicated by
        /// <see cref="HasLength"/>.</exception>
        /// 
        /// <example>
        /// <code>
        /// void PlayAnimation(AnimancerComponent animancer, AnimationClip clip)
        /// {
        ///     var state = animancer.Play(clip);
        ///     
        ///     state.NormalizedSpeed = 1;// Play fully in 1 second.
        ///     state.NormalizedSpeed = 2;// Play through twice per second.
        ///     state.NormalizedSpeed = 0.5f;// Play half the animation per second.
        ///     state.NormalizedSpeed = -1;// Play backwards fully in 1 second.
        /// }
        /// </code>
        /// </example>
        public float NormalizedSpeed
        {
            get { return Speed / Length; }
            set { Speed = value * Length; }
        }

        /************************************************************************************************************************/
        // Duration.
        /************************************************************************************************************************/

        /// <summary>
        /// The number of seconds the animation will take to play fully at its current <see cref="Speed"/>.
        /// <para></para>
        /// Setting this value modifies the <see cref="Speed"/>, not the <see cref="Length"/>.
        /// Animancer Lite does not allow this value to be changed in a runtime build.
        /// <para></para>
        /// For the time remaining from now until it reaches the end, use <see cref="RemainingDuration"/> instead.
        /// </summary>
        /// <exception cref="NotSupportedException">Thrown if this state doesn't have a length, as indicated by
        /// <see cref="HasLength"/>.</exception>
        /// 
        /// <example>
        /// <code>
        /// void PlayAnimation(AnimancerComponent animancer, AnimationClip clip)
        /// {
        ///     var state = animancer.Play(clip);
        ///     
        ///     state.Duration = 1;// Play fully in 1 second.
        ///     state.Duration = 2;// Play fully in 2 seconds.
        ///     state.Duration = 0.5f;// Play fully in half a second.
        ///     state.Duration = -1;// Play backwards fully in 1 second.
        /// }
        /// </code>
        /// </example>
        public float Duration
        {
            get { return Length / Mathf.Abs(Speed); }
            set { Speed = Length / value; }
        }

        /// <summary>
        /// The number of seconds the animation will take to reach the end at its current <see cref="Speed"/>.
        /// <para></para>
        /// Setting this value modifies the <see cref="Speed"/>, not the <see cref="Length"/>.
        /// Animancer Lite does not allow this value to be changed in a runtime build.
        /// <para></para>
        /// For the time it would take to play fully from the start, use <see cref="Duration"/> instead.
        /// </summary>
        /// <exception cref="NotSupportedException">Thrown if this state doesn't have a length, as indicated by
        /// <see cref="HasLength"/>.</exception>
        /// 
        /// <example>
        /// <code>
        /// void PlayAnimation(AnimancerComponent animancer, AnimationClip clip)
        /// {
        ///     var state = animancer.Play(clip);
        ///     
        ///     state.RemainingDuration = 1;// Play from the current time to the end in 1 second.
        ///     state.RemainingDuration = 2;// Play from the current time to the end in 2 seconds.
        ///     state.RemainingDuration = 0.5f;// Play from the current time to the end in half a second.
        ///     state.RemainingDuration = -1;// Play backwards from the current time to the end in 1 second.
        /// }
        /// </code>
        /// </example>
        public float RemainingDuration
        {
            get
            {
                var length = Length;
                return (length - (Time % length)) / Mathf.Abs(Speed);
            }
            set
            {
                var length = Length;
                Speed = (length - (Time % length)) / value;
            }
        }

        /************************************************************************************************************************/
        // Length.
        /************************************************************************************************************************/

        /// <summary>
        /// The total time this state takes to play in seconds (when <c>Speed = 1</c>).
        /// </summary>
        /// <exception cref="NotSupportedException">Thrown if this state doesn't have a length, as indicated by
        /// <see cref="HasLength"/>.</exception>
        public virtual float Length
        {
            get { throw new NotSupportedException("Tried to access the Length of a state that doesn't have one: " + this); }
        }

        /// <summary>
        /// Does this state have a valid <see cref="Length"/> value?
        /// <para></para>
        /// For example, a state that plays an <see cref="AnimationClip"/> will be able to use its
        /// <see cref="AnimationClip.length"/>, however a state that mixes several other states will not have a length
        /// of its own.
        /// </summary>
        public virtual bool HasLength { get { return false; } }

        /// <summary>
        /// Indicates whether this state will loop back to the start when it reaches the end.
        /// </summary>
        public virtual bool IsLooping { get { return false; } }

        /// <summary>
        /// A callback which is triggered when the animation reaches its end (not when the state is exited for
        /// whatever reason).
        /// <para></para>
        /// If the animation is looping, this callback will be triggered every time it passes the end. Otherwise it
        /// will be triggered every frame while it is past the end so if you want to ensure that your callback only
        /// occurs once, you will need to clear it as part of that callback.
        /// <para></para>
        /// This callback is automatically cleared by <see cref="Play"/>, <see cref="OnStartFade"/>, and
        /// <see cref="Stop"/>.
        /// </summary>
        /// 
        /// <example>
        /// <code>
        /// void PlayAnimation(AnimancerComponent animancer, AnimationClip clip)
        /// {
        ///     Debug.Log("Playing " + clip.name);
        ///     var state = animancer.Play(clip);
        ///     
        ///     // Lambda expression:
        ///     state.OnEnd = () =>
        ///     {
        ///         Debug.Log(clip.name + " ended");
        ///     };
        ///     
        ///     // One-line Lambda expression:
        ///     state.OnEnd = () => Debug.Log(clip.name + " ended");
        ///     
        ///     // Anonymous method (functionally identical to the Lambda expression):
        ///     state.OnEnd = delegate()
        ///     {
        ///         Debug.Log(clip.name + " ended");
        ///     };
        ///     
        ///     // Regular Method:
        ///     state.OnEnd = OnAnimationEnd;
        /// }
        /// 
        /// void OnAnimationEnd()
        /// {
        ///     Debug.Log("Animation ended");
        /// }
        /// </code>
        /// <h2>Caching Delegates</h2>
        /// The above examples will all allocate some garbage every time they are used which can cause performance
        /// issues. This can be avoided by caching the delegate:
        /// <para></para><code>
        /// private System.Action _OnAnimationEnd;
        /// 
        /// void Awake()
        /// {
        ///     _OnAnimationEnd = () => Debug.Log("Animation ended");
        /// }
        /// 
        /// void PlayAnimation(AnimancerComponent animancer, AnimationClip clip)
        /// {
        ///     animancer.Play(clip).OnEnd = _OnAnimationEnd;
        /// }
        /// </code>
        /// <h2>Serializables</h2>
        /// You can also use the <see cref="ClipState.Serializable.OnEnd"/> callback which does not get cleared, so you
        /// can assign the delegate once on startup and it will use that for the state every time it is played:
        /// <para></para><code>
        /// [SerializeField]
        /// private ClipState.Serializable _Animation;
        /// 
        /// void Awake()
        /// {
        ///     _Animation.OnEnd = () => Debug.Log("Animation ended");
        /// }
        /// 
        /// void PlayAnimation(AnimancerComponent animancer)
        /// {
        ///     animancer.Transition(_Animation);
        /// }
        /// </code>
        /// Note that we aren't passing an <see cref="AnimationClip"/> into <c>PlayAnimation</c> like the previous examples. Usually you would
        /// already have a serialized field for the <see cref="AnimationClip"/> so you would simply replace it with a
        /// <see cref="ClipState.Serializable"/>.
        /// </example>
        /// 
        /// <remarks>
        /// The reason for this callback being automatically cleared all the time is so that you don't have to worry
        /// about other scripts that might have used the same animation previously.
        /// <para></para>
        /// For example, if a character has an <em>Attack</em> animation which wants to return to <em>Idle</em> when it
        /// finishes but the character gets hit by an enemy in the middle of the <em>Attack</em>, the character will
        /// now want to play the <em>Flinch</em> animation and return to <em>Idle</em> after that instead. At that
        /// point, we no longer care about the end of the <em>Attack</em> animation.
        /// <para></para>
        /// If we want to attack again, we just play the animation and register the callback again. But if the
        /// character has a special skill that lets them perform an attack combo which includes the same
        /// <em>Attack</em> animation followed by several others in sequence, that skill won't want it to still have
        /// the <see cref="OnEnd"/> callback that returns to idle.
        /// <para></para>
        /// This way, each script that plays an animation takes over the responsibility for managing what it expects to
        /// happen without worrying about the expectations of other scripts.
        /// <para></para>
        /// That said, enforcing rules for which animations/actions are allowed to interrupt each other is often very
        /// important so it is covered in the
        /// <see href="https://kybernetikgames.github.io/animancer/docs/examples/state-machines/interrupt-management">
        /// State Machines/Interrupt Management</see> example.
        /// </remarks>
        public abstract Action OnEnd { get; set; }

        /************************************************************************************************************************/
        #endregion
        /************************************************************************************************************************/
        #region Inverse Kinematics
        /************************************************************************************************************************/

        /// <summary>
        /// Determines whether <c>OnAnimatorIK(int layerIndex)</c> will be called on the animated object.
        /// The initial value is determined by <see cref="AnimancerLayer.DefaultApplyAnimatorIK"/>.
        /// <para></para>
        /// This is equivalent to the "IK Pass" toggle in Animator Controller layers.
        /// <para></para>
        /// It requires Unity 2018.1 or newer, however 2018.3 or newer is recommended because a bug in earlier versions
        /// of the Playables API caused this value to only take effect while a state was at
        /// <see cref="AnimancerNode.Weight"/> == 1 which meant that IK would not work while fading between animations.
        /// </summary>
        public abstract bool ApplyAnimatorIK { get; set; }

        /// <summary>
        /// Indicates whether this state is applying IK to the character's feet.
        /// The initial value is determined by <see cref="AnimancerLayer.DefaultApplyFootIK"/>.
        /// <para></para>
        /// This is equivalent to the "Foot IK" toggle in Animator Controller states.
        /// </summary>
        public abstract bool ApplyFootIK { get; set; }

        /************************************************************************************************************************/
        #endregion
        /************************************************************************************************************************/
        #endregion
        /************************************************************************************************************************/
        #region Public Methods
        /************************************************************************************************************************/

        /// <summary>Constructs a new <see cref="AnimancerState"/>.</summary>
        public AnimancerState(AnimancerPlayable root) : base(root) { }

        /************************************************************************************************************************/

        /// <summary>
        /// Plays this animation immediately, without any blending.
        /// Sets <see cref="IsPlaying"/> = true, <see cref="Weight"/> = 1, and <see cref="OnEnd"/> = null.
        /// <para></para>
        /// This method does not change the <see cref="Time"/> so it will continue from its current value.
        /// </summary>
        public void Play()
        {
            IsPlaying = true;
            Weight = 1;
            OnEnd = null;
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Stops the animation and makes it inactive immediately so it no longer affects the output.
        /// Sets <see cref="IsPlaying"/> = false, <see cref="Time"/> = 0, <see cref="Weight"/> = 0, and
        /// <see cref="OnEnd"/> = null.
        /// <para></para>
        /// If you only want to freeze the animation in place, you can set <see cref="IsPlaying"/> = false instead.
        /// Or to freeze all animations, you can call <see cref="AnimancerPlayable.PauseGraph"/>.
        /// </summary>
        public sealed override void Stop()
        {
            IsPlaying = false;
            Time = 0;
            Weight = 0;
            OnEnd = null;
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Called by <see cref="AnimancerNode.StartFade"/>. Clears the <see cref="OnEnd"/> event.
        /// </summary>
        protected internal override void OnStartFade()
        {
            OnEnd = null;
        }

        /************************************************************************************************************************/

        /// <summary>Destroys the <see cref="Playable"/>.</summary>
        public virtual void Dispose()
        {
            if (_Parent != null)
                _Parent.OnRemoveChild(this);

            PortIndex = -1;
            _IsPlaying = false;
            OnEnd = null;

            Root.UnregisterState(this);

            // For some reason this is slightly more efficient than _Playable.Destroy().
            if (_Playable.IsValid())
                Root._Graph.DestroyPlayable(_Playable);
        }

        /************************************************************************************************************************/

        /// <summary>Checks if this state has an animation event with the specified 'functionName'.</summary>
        public abstract bool HasEvent(string functionName);

        /************************************************************************************************************************/

        /// <summary>[<see cref="IAnimationClipSource"/>]
        /// Gathers all the animations associated with this state.
        /// </summary>
        public virtual void GetAnimationClips(List<AnimationClip> clips)
        {
            var clip = Clip;
            if (clip != null)
                clips.Add(clip);
        }

        /************************************************************************************************************************/
        #region Descriptions
        /************************************************************************************************************************/

#if UNITY_EDITOR
        /// <summary>[Editor-Only] Returns a custom drawer for this state.</summary>
        protected internal virtual Editor.IAnimancerStateDrawer GetDrawer()
        {
            return new Editor.AnimancerStateDrawer<AnimancerState>(this);
        }
#endif

        /************************************************************************************************************************/

        /// <summary>Returns a detailed descrption of the current details of this state.</summary>
        public string GetDescription(bool includeClip = true, bool includeChildStates = true, string delimiter = "\n")
        {
            var description = new StringBuilder();
            AppendDescription(description, includeClip, includeChildStates, delimiter);
            return description.ToString();
        }

        /// <summary>Appends a detailed descrption of the current details of this state.</summary>
        public virtual void AppendDescription(StringBuilder description, bool includeClip, bool includeChildStates = true, string delimiter = "\n")
        {
            if (_Key != null)
                description.Append("Key: ").Append(_Key).Append(delimiter);

            var clip = Clip;
            if (clip != null)
            {
                if (includeClip)
                    description.Append("Clip: ").Append(clip.name).Append(delimiter);

#if UNITY_EDITOR
                description.Append("AssetPath: ").Append(AssetDatabase.GetAssetPath(clip)).Append(delimiter);
#endif
            }

            if (HasLength)
                description.Append("Length: ").Append(Length).Append(delimiter);

            if (clip != null)
            {
                description.Append("IsLooping: ").Append(clip.isLooping).Append(delimiter);
            }

            description.Append("PortIndex: ").Append(PortIndex).Append(delimiter);
            description.Append("IsPlaying: ").Append(IsPlaying).Append(delimiter);
            description.Append("Time: ").Append(Time).Append(delimiter);
            if (clip != null)
                description.Append("NormalizedTime: ").Append(NormalizedTime).Append(delimiter);
            description.Append("Speed: ").Append(Speed).Append(delimiter);
            description.Append("Weight: ").Append(Weight);

            if (Weight != TargetWeight)
            {
                description.Append(delimiter).Append("TargetWeight: ").Append(TargetWeight);
                description.Append(delimiter).Append("FadeSpeed: ").Append(FadeSpeed);
            }

            if (includeChildStates)
            {
                string indentedDelimiter = null;

                foreach (var childState in this)
                {
                    if (indentedDelimiter == null)
                        indentedDelimiter = delimiter + "    ";

                    description.Append(delimiter);
                    description.Append("Port ").Append(childState.PortIndex).Append(": Weight ").Append(childState.Weight);
                    description.Append(indentedDelimiter);
                    childState.AppendDescription(description, true, true, indentedDelimiter);
                }
            }
        }

        /************************************************************************************************************************/

        /// <summary>Returns the hierarchy path of this state through its <see cref="Parent"/>s.</summary>
        public string GetPath()
        {
            if (_Parent == null)
                return null;

            var path = new StringBuilder();

            AppendPath(path, _Parent);
            AppendPortAndType(path);

            return path.ToString();
        }

        /// <summary>Appends the hierarchy path of this state through its <see cref="Parent"/>s.</summary>
        private static void AppendPath(StringBuilder path, AnimancerNode parent)
        {
            var parentState = parent as AnimancerState;
            if (parentState != null && parentState._Parent != null)
            {
                AppendPath(path, parentState._Parent);
            }
            else
            {
                path.Append("Layers[")
                    .Append(parent.Layer.PortIndex)
                    .Append("].States");
                return;
            }

            var state = parent as AnimancerState;
            if (state != null)
            {
                state.AppendPortAndType(path);
            }
            else
            {
                path.Append(" -> ")
                    .Append(parent.GetType());
            }
        }

        /// <summary>Appends "[PortIndex] -> GetType().Name".</summary>
        private void AppendPortAndType(StringBuilder path)
        {
            path.Append('[')
                .Append(PortIndex)
                .Append("] -> ")
                .Append(GetType().Name);
        }

        /************************************************************************************************************************/
        #endregion
        /************************************************************************************************************************/
        #endregion
        /************************************************************************************************************************/
        #region Update
        /************************************************************************************************************************/

        /// <summary>[Internal]
        /// Updates the <see cref="Weight"/> for fading, applies it to this state's port on the parent mixer, and plays
        /// or pauses the <see cref="Playable"/> if its state is dirty.
        /// <para></para>
        /// If the root <see cref="AnimancerPlayable.DisconnectWeightlessPlayables"/> is set to true, this method will
        /// also connect/disconnect its state from the animation mixer in the playable graph.
        /// </summary>
        protected override void Update(ref bool needsMoreUpdates)
        {
            base.Update(ref needsMoreUpdates);

            if (_IsPlayingDirty)
            {
                _IsPlayingDirty = false;

                if (_IsPlaying)
                {
#if UNITY_2017_3_OR_NEWER
                    _Playable.Play();
#else
                    _Playable.SetPlayState(PlayState.Playing);
#endif
                }
                else
                {
#if UNITY_2017_3_OR_NEWER
                    _Playable.Pause();
#else
                    _Playable.SetPlayState(PlayState.Paused);
#endif
                }

            }
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Returns true if the animation is playing and hasn't yet reached its end.
        /// <para></para>
        /// This method is called by <see cref="IEnumerator.MoveNext"/> so this object can be used as a custom yield
        /// instruction to wait until it finishes.
        /// </summary>
        /// <exception cref="NotSupportedException">Thrown if this state doesn't have a length, as indicated by
        /// <see cref="HasLength"/>.</exception>
        protected internal override bool IsPlayingAndNotEnding()
        {
            if (!_IsPlaying)
                return false;

            // We check the time one frame ahead so that the coroutine can continue and start the next animation.
            // Otherwise the animation would end and stop playing, then when the coroutine continues and starts another
            // animation, the new one won't actually take effect until the animation update for the following frame.
            // This would mean that a clip with WrapMode.Once stops and the model reverts to its default pose for a
            // frame before the next animation plays. Similarly, WrapMode.Loop would freeze in its last pose for an
            // extra frame.

            // Unfortunately this delta time value is from the last frame so it can not accurately predict the delta
            // time of the next frame.

            var speed = Speed;
            if (speed > 0)
                return Time + speed * UnityEngine.Time.deltaTime < Length;
            else
                return Time + speed * UnityEngine.Time.deltaTime > 0;
        }

        /************************************************************************************************************************/
        #endregion
        /************************************************************************************************************************/
        #region Serializable
        /************************************************************************************************************************/

        /// <summary>
        /// Base class for serializable objects which can create a particular type of <see cref="AnimancerState"/> when
        /// passed into <see cref="AnimancerPlayable.Transition"/>.
        /// </summary>
        /// <remarks>
        /// Even though it has the <see cref="SerializableAttribute"/>, this class won't actually get serialized
        /// by Unity because it's generic and abstract. Each child class still needs to include the attribute.
        /// </remarks>
        [Serializable]
        public abstract class Serializable<TState> : IAnimancerTransition where TState : AnimancerState
        {
            /************************************************************************************************************************/

            [Tooltip("The amount of time the transition should take (in seconds)")]
            [SerializeField]
            private float _FadeDuration = AnimancerPlayable.DefaultFadeDuration;

            /// <summary>The amount of time the transition should take (in seconds).</summary>
            /// <exception cref="ArgumentOutOfRangeException">Thrown when setting the value to a negative number.</exception>
            public float FadeDuration
            {
                get { return _FadeDuration; }
                set
                {
                    if (value < 0)
                        throw new ArgumentOutOfRangeException("value", "must not be negative");

                    _FadeDuration = value;
                }
            }

            /************************************************************************************************************************/

            /// <summary>
            /// The state that was most recently created by this object.
            /// </summary>
            /// <remarks>
            /// Often a <see cref="Serializable{TState}"/> will only be used on a single object so it will only ever
            /// create one state, however it is possible to use them on multiple different objects, in which case it
            /// will create a new state for each object. Each state must be retrieved by using the <see cref="Key"/> in
            /// <see cref="AnimancerPlayable.GetState(object)"/> since this property will only hold the last one.
            /// </remarks>
            public TState State { get; protected set; }

            /************************************************************************************************************************/

            /// <summary>
            /// The <see cref="AnimancerState.Key"/> which the created state will be registered with.
            /// <para></para>
            /// By default, a serializable is used as its own <see cref="Key"/>, but this property can be overridden.
            /// </summary>
            public virtual object Key { get { return this; } }

            /// <summary>
            /// When a serializable is passed into <see cref="AnimancerPlayable.Transition"/>, this property
            /// determines whether it needs to fade in from the start of the animation.
            /// </summary>
            public virtual bool CrossFadeFromStart { get { return false; } }

            /// <summary>
            /// Creates and returns a new <typeparamref name="TState"/> connected to the 'layer'.
            /// <para></para>
            /// This method also assigns it as the <see cref="State"/> (which must be done manually when overriding
            /// this method).
            /// </summary>
            public abstract TState CreateState(AnimancerLayer layer);

            /// <summary>Creates and returns a new <typeparamref name="TState"/> connected to the 'layer'.</summary>
            AnimancerState IAnimancerTransition.CreateState(AnimancerLayer layer) { return CreateState(layer); }

            /************************************************************************************************************************/

            /// <summary>[<see cref="IAnimancerTransition"/>]
            /// Called by <see cref="AnimancerPlayable.Transition"/> to apply any modifications to the 'state'.
            /// </summary>
            public virtual void Apply(AnimancerState state) { }

            /************************************************************************************************************************/

#if UNITY_EDITOR
            /// <summary>Don't use Inspector Gadgets Nested Object Drawers.</summary>
            private const bool NestedObjectDrawers = false;
#endif

            /************************************************************************************************************************/
        }

        /************************************************************************************************************************/
        #endregion
        /************************************************************************************************************************/
    }
}

// Wyam doesn't currently support <see> tags in lists.
// There are various different ways of getting a state:
// <list type="bullet">
//   <item>
//   Use one of the state's constructors. Generally the first parameter is an <see cref="AnimancerLayer"/> or
//   <see cref="IAnimationMixer"/> which will be used as the state's parent. If not specified, you will need to call
//   <see cref="SetParent"/> manually. Also note than an <see cref="AnimancerComponent"/> can be implicitly cast to
//   its first layer.
//   </item>
//   <item>
//   <see cref="AnimancerComponent.CreateState(AnimationClip, int)"/> creates a new <see cref="ClipState"/>.
//   You can optionally specify a custom 'key' to register it in the dictionary instead of the default (the 'clip'
//   itself).
//   </item>
//   <item>
//   <see cref="AnimancerComponent.GetOrCreateState(AnimationClip, int)"/> looks for an existing state registered
//   with the specified 'key' and only create a new one if it doesn’t already exist.
//   </item>
//   <item>
//   <see cref="AnimancerComponent.GetState(object)"/> returns an existing state registered with the specified
//   'key' if there is one.
//   </item>
//   <item>
//   <see cref="AnimancerComponent.TryGetState(object, out AnimancerState)"/> is similar but returns a bool to
//   indicate success and returns the 'state' as an out parameter.
//   </item>
//   <item>
//   <see cref="AnimancerComponent.Play(AnimationClip, int)"/> and
//   <see cref="AnimancerComponent.CrossFade(AnimationClip, float, int)"/> also return the state they play.
//   </item>
// </list>
