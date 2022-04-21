using UnityEngine;
using System.Collections;
using System.Linq;

namespace Slate
{

    [Description("The Animation Track works with the legacy 'Animation' Component. Each Animation Track represents a different layer of the animation system. The zero layered track (bottom) will blend in/out with the default animation clip set on the Animation Component of the actor if any, while all other Animation Tracks will play above.")]
    [Attachable(typeof(ActorGroup))]
    [Category("Legacy")]
    [Icon(typeof(UnityEngine.AI.NavMeshAgent))]
    public class AnimationTrack : CutsceneTrack
    {

        [SerializeField]
        [Range(0, 1)]
        private float _weight = 1f;
        [SerializeField]
        [Range(0, 1)]
        private float _blendIn = 0.5f;
        [SerializeField]
        [Range(0, 1)]
        private float _blendOut = 0.5f;
        [SerializeField]
        private AnimationBlendMode _animationBlendMode = AnimationBlendMode.Blend;
        [SerializeField]
        private string _mixTransformName = string.Empty;

        private Animation anim;
        private AnimationState state;

        public override string info {
            get { return string.Format("Layer: {0}, {1} {2}", layerOrder, animationBlendMode, ( string.IsNullOrEmpty(mixTransformName) ? "" : ", " + mixTransformName )); }
        }

        public override float blendIn {
            get { return _blendIn; }
        }

        public override float blendOut {
            get { return _blendOut; }
        }

        public float weight {
            get { return _weight; }
        }

        public AnimationBlendMode animationBlendMode {
            get { return _animationBlendMode; }
            private set { _animationBlendMode = value; }
        }

        public string mixTransformName {
            get { return _mixTransformName; }
            private set { _mixTransformName = value; }
        }

        protected override bool OnInitialize() {
            anim = actor.GetComponent<Animation>();
            if ( anim == null ) {
                Debug.LogError("The Animation Track requires the actor to have the 'Animation' Component attached", actor);
                return false;
            }

            return true;
        }

        //The track callbacks here are responsible ONLY for playing the base default clip if any.
        //The Play Animation Action Clip is responsible for playing itself.
        protected override void OnEnter() {
            anim = actor.GetComponent<Animation>();
            if ( anim == null || anim.clip == null || anim.IsPlaying(anim.clip.name) ) {
                state = null;
                return;
            }

            if ( anim.playAutomatically ) {
                state = anim[anim.clip.name];
                state.layer = 10; //set the base state to 10. Everything else is playing above it
                state.wrapMode = WrapMode.Loop;
                state.blendMode = AnimationBlendMode.Blend;
                state.enabled = true;
            }
        }

        protected override void OnUpdate(float time, float previousTime) {
            if ( state != null ) {
                state.time = Mathf.Repeat(time, state.length);
                state.weight = GetTrackWeight(time);
                anim.Sample();
            }
        }

        protected override void OnExit() {
            if ( state != null ) {
                state.enabled = false;
            }
        }

        protected override void OnReverseEnter() {
            if ( state != null ) {
                state.enabled = true;
            }
        }

        protected override void OnReverse() {
            if ( state != null ) {
                state.enabled = false;
            }
        }

        public Transform GetMixTransform() {
            if ( string.IsNullOrEmpty(mixTransformName) ) {
                return null;
            }
            var o = anim.transform.GetComponentsInChildren<Transform>().FirstOrDefault(t => t.name == mixTransformName);
            if ( o == null ) {
                Debug.LogWarning("Cant find transform with name '" + mixTransformName + "' for PlayAnimation Action", anim);
            }
            return o;
        }
    }
}