#if UNITY_2017_2_OR_NEWER

using UnityEngine.Video;

namespace Slate
{

    [UniqueElement]
    [Attachable(typeof(DirectorGroup))]
    [Icon(typeof(VideoPlayer))]
    [Description("Plays video clips on the render camera. Please note that scrubbing might be choppy, but playing is not.")]
    public class VideoTrack : CutsceneTrack
    {

        public VideoSampler.VideoRenderTarget renderTarget;
        public VideoAspectRatio aspectRatio = VideoAspectRatio.FitHorizontally;

        public VideoPlayer source { get; private set; }

        public override string info {
            get { return renderTarget.ToString(); }
        }

        protected override void OnEnter() { Enable(); }
        protected override void OnReverseEnter() { Enable(); }
        protected override void OnReverse() { Disable(); }
        protected override void OnExit() { Disable(); }

        void Enable() { source = VideoSampler.GetSourceForID(this); source.Prepare(); }
        void Disable() { VideoSampler.ReleaseSourceForID(this); }

    }
}

#endif