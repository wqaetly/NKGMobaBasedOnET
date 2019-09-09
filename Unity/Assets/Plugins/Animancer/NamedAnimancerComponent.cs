// Animancer // Copyright 2019 Kybernetik //

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using Object = UnityEngine.Object;

namespace Animancer
{
    /// <summary>
    /// An <see cref="AnimancerComponent"/> which uses the <see cref="Object.name"/>s of <see cref="AnimationClip"/>s
    /// so they can be referenced using strings as well as the clips themselves.
    /// <para></para>
    /// It also has fields to automatically register animations on startup and play the first one automatically without
    /// needing another script to control it, much like Unity's Legacy <see cref="Animation"/> component.
    /// </summary>
    [AddComponentMenu("Animancer/Named Animancer Component")]
    [HelpURL(AnimancerPlayable.APIDocumentationURL + "/NamedAnimancerComponent")]
    public class NamedAnimancerComponent : AnimancerComponent
    {
        /************************************************************************************************************************/
        #region Fields and Properties
        /************************************************************************************************************************/

        [SerializeField]
        [Tooltip("If true, the 'Default Animation' will be automatically played by OnEnable")]
        private bool _PlayAutomatically = true;

        /// <summary>
        /// If true, the first clip in the <see cref="Animations"/> array will be automatically played by
        /// <see cref="OnEnable"/>.
        /// </summary>
        public bool PlayAutomatically
        {
            get { return _PlayAutomatically; }
            set { _PlayAutomatically = value; }
        }

        /************************************************************************************************************************/

        [SerializeField]
        [Tooltip("Animations in this array will be automatically registered by Awake" +
            " as states that can be retrieved using their name")]
        private AnimationClip[] _Animations;

        /// <summary>
        /// Animations in this array will be automatically registered by <see cref="Awake"/> as states that can be
        /// retrieved using their name and the first element will be played by <see cref="OnEnable"/> if
        /// <see cref="PlayAutomatically"/> is true.
        /// </summary>
        public AnimationClip[] Animations
        {
            get { return _Animations; }
            set
            {
                CreateStates(_Animations);
                _Animations = value;
            }
        }

        /************************************************************************************************************************/

        /// <summary>
        /// The first element in the <see cref="Animations"/> array.
        /// </summary>
        public AnimationClip DefaultAnimation
        {
            get
            {
                if (_Animations == null || _Animations.Length == 0)
                    return null;
                else
                    return _Animations[0];
            }
            set
            {
                if (_Animations == null || _Animations.Length == 0)
                    _Animations = new AnimationClip[] { value };
                else
                    _Animations[0] = value;
            }
        }

        /************************************************************************************************************************/
        #endregion
        /************************************************************************************************************************/
        #region Initialisation
        /************************************************************************************************************************/

#if UNITY_EDITOR
        /// <summary>[Editor-Only]
        /// Called by the Unity Editor in edit mode whenever an instance of this script is loaded or a value is changed
        /// in the inspector.
        /// <para></para>
        /// Uses <see cref="ClipState.ValidateClip"/> to ensure that all of the clips in the <see cref="Animations"/>
        /// array are supported by the <see cref="Animancer"/> system and removes any others.
        /// </summary>
        protected virtual void OnValidate()
        {
            if (_Animations == null)
                return;

            for (int i = 0; i < _Animations.Length; i++)
            {
                var clip = _Animations[i];
                if (clip != null)
                {
                    try
                    {
                        ClipState.ValidateClip(clip);
                        continue;
                    }
                    catch (Exception ex)
                    {
                        Debug.LogException(ex, clip);
                    }
                }

                Array.Copy(_Animations, i + 1, _Animations, i, _Animations.Length - (i + 1));
                Array.Resize(ref _Animations, _Animations.Length - 1);
                i--;
            }
        }
#endif

        /************************************************************************************************************************/

        /// <summary>
        /// Called by Unity when this component is being loaded.
        /// <para></para>
        /// Creates a state for each clip in the <see cref="Animations"/> array.
        /// </summary>
        protected virtual void Awake()
        {
            CreateStates(_Animations);
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Called by Unity when this component becomes enabled and active.
        /// <para></para>
        /// Plays the first clip in the <see cref="Animations"/> array if <see cref="PlayAutomatically"/> is true.
        /// <para></para>
        /// Plays the <see cref="PlayableGraph"/> if it was stopped.
        /// </summary>
        protected override void OnEnable()
        {
            base.OnEnable();

            if (_PlayAutomatically && _Animations != null && _Animations.Length > 0)
            {
                var clip = _Animations[0];
                if (clip != null)
                    Play(clip);
            }
        }

        /************************************************************************************************************************/

        /// <summary>[<see cref="IAnimationClipSource"/>]
        /// Gathers all the animations in the <see cref="Playable"/> and the <see cref="Animations"/> array.
        /// </summary>
        public override void GetAnimationClips(List<AnimationClip> clips)
        {
            base.GetAnimationClips(clips);

            if (_Animations == null)
                return;

            var count = _Animations.Length;
            for (int i = 0; i < count; i++)
            {
                var clip = _Animations[i];
                if (!clips.Contains(clip))
                    clips.Add(clip);
            }
        }

        /************************************************************************************************************************/
        #endregion
        /************************************************************************************************************************/
        #region Play Management
        /************************************************************************************************************************/

        /// <summary>
        /// Returns the clip's name. This method is used to determine the dictionary key to use for an animation when
        /// none is specified by the user, such as in <see cref="AnimancerComponent.Play(AnimationClip)"/>.
        /// </summary>
        public override object GetKey(AnimationClip clip)
        {
            return clip.name;
        }

        /************************************************************************************************************************/

        /// <summary>[Coroutine]
        /// Plays each clip in the <see cref="Animations"/> array one after the other. Mainly useful for testing and
        /// showcasing purposes.
        /// </summary>
        public IEnumerator PlayAnimationsInSequence()
        {
            for (int i = 0; i < _Animations.Length; i++)
            {
                var state = Play(_Animations[i]);

                if (state != null)
                    yield return state;
            }

            Stop();
        }

        /************************************************************************************************************************/

        /// <summary>[Pro-Only] [Coroutine]
        /// Cross fades between each clip in the <see cref="Animations"/> array one after the other. Mainly useful for
        /// testing and showcasing purposes.
        /// </summary>
        public IEnumerator CrossFadeAnimationsInSequence(float fadeDuration = AnimancerPlayable.DefaultFadeDuration)
        {
            for (int i = 0; i < _Animations.Length; i++)
            {
                var state = CrossFade(_Animations[i], fadeDuration);

                if (state != null)
                {
                    state.Time = 0;
                    yield return state;
                }
            }

            Stop();
        }

        /************************************************************************************************************************/
        #endregion
        /************************************************************************************************************************/
    }
}
