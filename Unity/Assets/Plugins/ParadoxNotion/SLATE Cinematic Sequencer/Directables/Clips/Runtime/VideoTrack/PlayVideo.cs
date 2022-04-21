#if UNITY_2017_2_OR_NEWER

using UnityEngine;
using UnityEngine.Video;

namespace Slate
{

    [Attachable(typeof(VideoTrack))]
    [Name("Video Clip")]
    [Description("Play an imported Video Clip on the render camera.")]
    public class PlayVideo : ActionClip
    {

        [SerializeField, HideInInspector]
        private float _length = 2;
        [SerializeField, HideInInspector]
        private float _blendIn = 0.2f;
        [SerializeField, HideInInspector]
        private float _blendOut = 0.2f;

        [Required]
        public VideoClip videoClip;

        private VideoSampler.SampleSettings settings;

        public override bool isValid {
            get { return videoClip != null; }
        }

        public override string info {
            get { return videoClip != null ? videoClip.name : "No Video"; }
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

        private VideoTrack track {
            get { return (VideoTrack)parent; }
        }

        protected override void OnEnter() { Enable(); }
        protected override void OnReverseEnter() { Enable(); }
        protected override void OnReverse() { Disable(); }
        protected override void OnExit() { Disable(); }

        void Enable() {
            settings = VideoSampler.SampleSettings.Default();
            settings.renderTarget = track.renderTarget;
            settings.aspectRatio = track.aspectRatio;
            track.source.Play();
        }

        protected override void OnUpdate(float time, float previousTime) {
            var weight = GetClipWeight(time);
            settings.playbackSpeed = root.playbackSpeed;
            settings.alpha = weight;
            settings.audioVolume = weight;
            VideoSampler.Sample(track.source, videoClip, time, previousTime, settings);
        }

        void Disable() {
            track.source.Stop();
        }

    }
}

#endif