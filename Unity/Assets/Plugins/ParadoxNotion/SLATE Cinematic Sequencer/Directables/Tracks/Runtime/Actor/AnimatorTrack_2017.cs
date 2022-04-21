#if UNITY_2017_1_OR_NEWER

using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Slate.ActionClips;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace Slate
{

    [Description("The Animator Track works with an 'Animator' Component attached on the actor, but does not require or use the Controller assigned. Instead animation clips can be played directly.\n\nMultiple Animator Tracks can also be added each representing a different animation layer. Root Motion will only be used in Animator Track of Layer 0 (if enabled), which is always the first (bottom) Track.")]
    [Icon(typeof(Animator))]
    [Attachable(typeof(ActorGroup))]
    partial class AnimatorTrack
    {

        const int ROOTMOTION_FRAMERATE = 30;

        public AvatarMask mask;
        public AnimationBlendMode blendMode;
        [Range(0, 1)]
        public float weight = 1f;
        [ShowIf("isMasterTrack", 1)]
        public bool useRootMotion = true;
        [ShowIf("isMasterTrack", 1)]
        public bool applyFootIK = false;

        private Dictionary<PlayAnimatorClip, int> ports;
        private int activeClips;
        private float activeClipsWeight;
        private List<AnimatorTrack> siblingTracks;

        private PlayableGraph graph;
        private AnimationPlayableOutput animationOutput;
        private AnimationLayerMixerPlayable masterMixer;
        private AnimationMixerPlayable clipsMixer;

        private bool wasRootMotion;
        private AnimatorCullingMode wasCullingMode;

        private bool useBakedRootMotion;

        [SerializeField, HideInInspector]
        public bool isRootMotionPreBaked;
        [SerializeField, HideInInspector]
        private List<Vector3> rmPositions;
        [SerializeField, HideInInspector]
        private List<Quaternion> rmRotations;

        private Animator _animator;
        public Animator animator {
            get
            {
                if ( _animator == null || _animator.gameObject != actor.gameObject ) {
                    _animator = actor.GetComponentInChildren<Animator>();
                }
                return _animator;
            }
        }

        public override string info {
            get
            {
                var info = string.Format("Layer: {0} | Mask: {1} | {2}", layerOrder.ToString(), mask != null ? mask.name : "None", blendMode);
                if ( isMasterTrack ) {
                    info += ( useRootMotion ? " | Use RM" : " | No RM" );
                }
                return info;
            }
        }

        public override bool isLocked {
            get { return base.isLocked || ( isRootMotionPreBaked && isMasterTrack ); }
        }

        public bool isMasterTrack {
            get { return layerOrder == 0; }
        }

        float compoundTrackWeight {
            get { return activeClips > 0 ? activeClipsWeight * weight : 0f; }
        }

        //...
        protected override bool OnInitialize() {
            if ( animator == null ) {
                Debug.LogError("Animator Track requires that the actor has the Animator Component attached.", actor);
                return false;
            }
            return true;
        }

        //...
        protected override void OnEnter() {
            if ( isMasterTrack ) {
                wasCullingMode = animator.cullingMode;
                animator.cullingMode = AnimatorCullingMode.AlwaysAnimate;
                wasRootMotion = animator.applyRootMotion;
                animator.applyRootMotion = false;

                siblingTracks = parent.children.OfType<AnimatorTrack>().Where(t => t.isActive).Reverse().ToList();
                var wasActive = animator.gameObject.activeSelf;
                animator.gameObject.SetActive(true);
                CreateAndPlayTree();
                if ( useRootMotion ) {
                    BakeRootMotion();
                }
                animator.gameObject.SetActive(wasActive);
            }
        }

        //...
        protected override void OnUpdate(float time, float previousTime) {
            if ( isMasterTrack ) {
                if ( animator == null || !animator.gameObject.activeInHierarchy || !graph.IsValid() ) {
                    return;
                }

                var totalSiblingWeight = 0f;
                for ( var i = 0; i < siblingTracks.Count; i++ ) {
                    var siblingWeight = siblingTracks[i].compoundTrackWeight;
                    masterMixer.SetInputWeight(i, siblingWeight);
                    totalSiblingWeight += siblingWeight;
                }


                var animatorWeight = totalSiblingWeight < 1 ? 1 - totalSiblingWeight : 0;
                masterMixer.SetInputWeight(siblingTracks.Count + 1, animatorWeight);

                graph.Evaluate(time - previousTime);
                if ( useRootMotion && useBakedRootMotion ) {
                    ApplyBakedRootMotion(time);
                }
            }
        }

        //...
        protected override void OnReverseEnter() {
            if ( isMasterTrack ) {
                wasCullingMode = animator.cullingMode;
                animator.cullingMode = AnimatorCullingMode.AlwaysAnimate;
                wasRootMotion = animator.applyRootMotion;
                animator.applyRootMotion = false;

                CreateAndPlayTree();
                //DO NOT Re-Bake root motion
            }
        }

        //...
        protected override void OnExit() {
            if ( isMasterTrack ) {
                animator.applyRootMotion = wasRootMotion;
                animator.cullingMode = wasCullingMode;
                if ( graph.IsValid() ) {
                    animationOutput.SetWeight(0);
                    graph.Evaluate();
                    graph.Destroy();
                }

                if ( useRootMotion ) {
                    ApplyBakedRootMotion(endTime - startTime);
                }
            }
        }

        //...
        protected override void OnReverse() {
            if ( isMasterTrack ) {
                animator.applyRootMotion = wasRootMotion;
                animator.cullingMode = wasCullingMode;
                if ( graph.IsValid() ) {
                    animationOutput.SetWeight(0);
                    graph.Evaluate();
                    graph.Destroy();
                }

                if ( useRootMotion ) {
                    ApplyBakedRootMotion(0);
                }
            }
        }

        ///----------------------------------------------------------------------------------------------

        //...
        public void EnableClip(PlayAnimatorClip playAnimClip, float blendWeight, float subClipWeight) {
            activeClips++;
            var index = ports[playAnimClip];
            clipsMixer.SetInputWeight(index, activeClips == 1 ? subClipWeight : blendWeight * subClipWeight);
            activeClipsWeight = activeClips >= 2 ? 1 : blendWeight;
        }

        //...
        public void UpdateClip(PlayAnimatorClip playAnimClip, float clipTime, float clipPrevious, float blendWeight, float subClipWeight) {
            var index = ports[playAnimClip];
            var clipPlayable = clipsMixer.GetInput(index);
            clipPlayable.SetTime(clipTime);
            clipsMixer.SetInputWeight(index, activeClips == 1 ? subClipWeight : blendWeight * subClipWeight);
            activeClipsWeight = activeClips >= 2 ? 1 : blendWeight;
        }

        //...
        public void DisableClip(PlayAnimatorClip playAnimClip, float blendWeight, float subClipWeight) {
            activeClips--;
            var index = ports[playAnimClip];
            clipsMixer.SetInputWeight(index, 0f);
            activeClipsWeight = activeClips == 0 ? 0 : activeClipsWeight;
        }

        ///----------------------------------------------------------------------------------------------

        private AnimatorControllerPlayable animatorPlayable;

        //Create and play the playable graph
        void CreateAndPlayTree() {
            graph = PlayableGraph.Create();
            animationOutput = AnimationPlayableOutput.Create(graph, "Animation", animator);
            masterMixer = AnimationLayerMixerPlayable.Create(graph, siblingTracks.Count + 2);
            for ( var i = 0; i < siblingTracks.Count; i++ ) {
                var animatorTrack = siblingTracks[i];
                var mix = animatorTrack.CreateClipsMixer(graph);
                graph.Connect(mix, 0, masterMixer, i);
                masterMixer.SetInputWeight(i, 1f);
                if ( animatorTrack.mask != null ) {
                    masterMixer.SetLayerMaskFromAvatarMask((uint)i, animatorTrack.mask);
                }
                masterMixer.SetLayerAdditive((uint)i, animatorTrack.blendMode == AnimationBlendMode.Additive);
            }

            animatorPlayable = AnimatorControllerPlayable.Create(graph, animator.runtimeAnimatorController);
            graph.Connect(animatorPlayable, 0, masterMixer, siblingTracks.Count + 1);
            masterMixer.SetInputWeight(siblingTracks.Count + 1, 0f);

            animationOutput.SetSourcePlayable(masterMixer);

            // graph.Play();
            // masterMixer.Pause();

            // GraphVisualizerClient.Show(graph, this.name);
        }

        //Create and return the track mixer playable
        Playable CreateClipsMixer(PlayableGraph graph) {
            var clipActions = clips.OfType<PlayAnimatorClip>().ToList();
            ports = new Dictionary<PlayAnimatorClip, int>();
            clipsMixer = AnimationMixerPlayable.Create(graph, clipActions.Count, true);
            for ( var i = 0; i < clipActions.Count; i++ ) {
                var playAnimClip = clipActions[i];
                var clipPlayable = AnimationClipPlayable.Create(graph, playAnimClip.animationClip);
                clipPlayable.SetApplyFootIK(applyFootIK && isMasterTrack);
                graph.Connect(clipPlayable, 0, clipsMixer, i);
                clipsMixer.SetInputWeight(i, 0f);
                ports[playAnimClip] = i;
            }

            //need pause clips.
            clipsMixer.Pause();
            return clipsMixer;
        }

        ///----------------------------------------------------------------------------------------------

        public void PreBakeRootMotion() {
            root.Sample(float.Epsilon);
            root.Sample(0);
            isRootMotionPreBaked = true;
        }

        public void ClearPreBakeRootMotion() {
            rmPositions = new List<Vector3>();
            rmRotations = new List<Quaternion>();
            isRootMotionPreBaked = false;
        }

        //The root motion must be baked if required.
        void BakeRootMotion() {

            if ( isRootMotionPreBaked ) {
                animator.applyRootMotion = false;
                useBakedRootMotion = true;
                return;
            }

            var rb = animator.GetComponent<Rigidbody>();
            if ( rb != null ) {
                rb.MovePosition(animator.transform.localPosition);
                rb.MoveRotation(animator.transform.localRotation);
            }

            useBakedRootMotion = false;
            animator.applyRootMotion = true;
            rmPositions = new List<Vector3>();
            rmRotations = new List<Quaternion>();
            var tempActiveClips = 0;
            var updateInterval = ( 1f / ROOTMOTION_FRAMERATE );
            for ( var time = startTime - updateInterval; time <= endTime + updateInterval; time += updateInterval ) {

                EvaluateTrackClips(time, time - updateInterval, ref tempActiveClips);

                if ( tempActiveClips > 0 ) {
                    graph.Evaluate(updateInterval);
                }

                //apparently animator automatically sets rigidbody pos/rot if attached on same go when evaluated.
                //thus we read pos/rot from rigidbody in such cases.
                var pos = rb != null ? rb.position : animator.transform.localPosition;
                var rot = rb != null ? rb.rotation : animator.transform.localRotation;
                rmPositions.Add(pos);
                rmRotations.Add(rot);
            }
            animator.applyRootMotion = false;
            useBakedRootMotion = true;
        }

        //Evaluate the track clips
        void EvaluateTrackClips(float time, float previousTime, ref int tempActiveClips) {
            foreach ( var clip in ( this as IDirectable ).children ) {
                if ( time >= clip.startTime && previousTime < clip.startTime ) {
                    tempActiveClips++;
                    clip.Enter();
                }

                if ( time >= clip.startTime && time <= clip.endTime ) {
                    clip.Update(time - clip.startTime, previousTime - clip.startTime);
                }

                if ( ( time > clip.endTime || time >= this.endTime ) && previousTime <= clip.endTime ) {
                    tempActiveClips--;
                    clip.Exit();
                }
            }
        }

        //Apply baked root motion by lerping between stored frames.
        void ApplyBakedRootMotion(float time) {
            var frame = Mathf.FloorToInt(time * ROOTMOTION_FRAMERATE);
            var nextFrame = frame + 1;
            nextFrame = nextFrame < rmPositions.Count ? nextFrame : rmPositions.Count - 1;

            var tNow = frame * ( 1f / ROOTMOTION_FRAMERATE );
            var tNext = nextFrame * ( 1f / ROOTMOTION_FRAMERATE );

            var posNow = rmPositions[frame];
            var posNext = rmPositions[nextFrame];
            var pos = Vector3.Lerp(posNow, posNext, Mathf.InverseLerp(tNow, tNext, time));
            animator.transform.localPosition = pos;

            var rotNow = rmRotations[frame];
            var rotNext = rmRotations[nextFrame];
            var rot = Quaternion.Lerp(rotNow, rotNext, Mathf.InverseLerp(tNow, tNext, time));
            animator.transform.localRotation = rot;
        }
    }
}

#endif
