using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Slate.ActionClips
{

    [Name("Animation Clip")]
    [Description("All animation clips in the same track, will play at an animation layer equal to their track layer order. Thus, animations in tracks on top will play over animations in tracks bellow. You can trim or loop the animation by scaling the clip.")]
    [Attachable(typeof(AnimationTrack))]
    public class PlayAnimationClip : ActorActionClip<Animation>, ISubClipContainable
    {

        [SerializeField]
        [HideInInspector]
        private float _length = 1f;
        [SerializeField]
        [HideInInspector]
        private float _blendIn = 0f;
        [SerializeField]
        [HideInInspector]
        private float _blendOut = 0f;


        [Required]
        public AnimationClip animationClip;
        public float clipOffset;
        [Range(0.1f, 5)]
        public float playbackSpeed = 1;

        private TransformSnapshot snapShot;
        private Transform mixTransform;
        private AnimationState state;
        private bool isListClip;

        float ISubClipContainable.subClipOffset {
            get { return clipOffset; }
            set { clipOffset = value; }
        }

        float ISubClipContainable.subClipLength {
            get { return animationClip != null ? animationClip.length : 0; }
        }

        float ISubClipContainable.subClipSpeed {
            get { return playbackSpeed; }
        }

        public override string info {
            get { return animationClip ? animationClip.name : base.info; }
        }

        public override bool isValid {
            get { return base.isValid && animationClip != null && animationClip.legacy; }
        }

        public override float length {
            get { return _length; }
            set { _length = value; }
        }

        public override float blendIn {
            get { return _blendIn; }
            set { _blendIn = value; }
        }

        public override float blendOut {
            get { return _blendOut; }
            set { _blendOut = value; }
        }

        public override bool canCrossBlend {
            get { return true; }
        }

        private AnimationTrack track { get { return (AnimationTrack)parent; } }


        protected override void OnEnter() {
            snapShot = new TransformSnapshot(actor.gameObject, TransformSnapshot.StoreMode.ChildrenOnly);

            isListClip = actor[animationClip.name] != null;
            if ( !isListClip ) {
                actor.AddClip(animationClip, animationClip.name);
            }

            mixTransform = track.GetMixTransform();
            if ( mixTransform != null ) {
                actor[animationClip.name].AddMixingTransform(mixTransform, true);
            }
        }

        protected override void OnUpdate(float time) {

            state = actor[animationClip.name];
            state.time = time * playbackSpeed;

            var animLength = animationClip.length / playbackSpeed;
            if ( length <= animLength ) {
                state.wrapMode = WrapMode.Once;
                state.time = Mathf.Min(state.time - clipOffset, animationClip.length);
            }

            if ( length > animLength ) {
                if ( animationClip.wrapMode == WrapMode.PingPong ) {
                    state.wrapMode = WrapMode.PingPong;
                    state.time = Mathf.PingPong(state.time - clipOffset, animationClip.length);
                } else {
                    state.wrapMode = WrapMode.Loop;
                    state.time = Mathf.Repeat(state.time - clipOffset, animationClip.length);
                }
            }

            state.layer = track.layerOrder + 11; //Play animations on layer 11+ for all the play animation action clips
            state.weight = GetClipWeight(time) * track.GetTrackWeight();
            state.blendMode = track.animationBlendMode;
            state.enabled = true;

            actor.Sample();
        }

        protected override void OnReverse() {
            snapShot.Restore();
            state.enabled = false;
            if ( !isListClip ) {
                actor.RemoveClip(animationClip);
            }
        }

        protected override void OnExit() {
            state.enabled = false;
        }

        protected override void OnReverseEnter() {
            state.enabled = true;
        }


        ////////////////////////////////////////
        ///////////GUI AND EDITOR STUFF/////////
        ////////////////////////////////////////
#if UNITY_EDITOR

        protected override void OnClipGUI(Rect rect) {
            if ( animationClip != null ) {
                EditorTools.DrawLoopedLines(rect, animationClip.length / playbackSpeed, this.length, clipOffset);
            }
        }

#endif
    }
}