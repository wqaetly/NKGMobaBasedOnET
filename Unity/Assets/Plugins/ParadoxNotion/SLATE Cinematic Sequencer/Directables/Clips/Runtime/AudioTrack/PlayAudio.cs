using UnityEngine;
using System.Collections;

namespace Slate.ActionClips
{

    [Name("Audio Clip")]
    [Description("The audio clip will be send to the AudioMixer selected in it's track if any. You can trim or loop the audio by scaling the clip and you can optionaly show subtitles as well.")]
    [Attachable(typeof(ActorAudioTrack), typeof(DirectorAudioTrack))]
    public class PlayAudio : ActionClip, ISubClipContainable
    {

        [SerializeField]
        [HideInInspector]
        private float _length = 1f;
        [SerializeField]
        [HideInInspector]
        private float _blendIn = 0.25f;
        [SerializeField]
        [HideInInspector]
        private float _blendOut = 0.25f;

        [Required]
        public AudioClip audioClip;
        [AnimatableParameter(0, 1)]
        public float volume = 1;
        [AnimatableParameter(-3, 3)]
        public float pitch = 1;
        [AnimatableParameter(-1, 1)]
        public float stereoPan = 0;

        public float clipOffset;
        [Multiline(5)]
        public string subtitlesText;
        public Color subtitlesColor = Color.white;

        float ISubClipContainable.subClipOffset {
            get { return clipOffset; }
            set { clipOffset = value; }
        }

        float ISubClipContainable.subClipLength {
            get { return audioClip != null ? audioClip.length : 0; }
        }

        float ISubClipContainable.subClipSpeed {
            get { return 1; }
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

        public override bool isValid {
            get { return audioClip != null; }
        }

        public override string info {
            get { return isValid ? ( string.IsNullOrEmpty(subtitlesText) ? audioClip.name : string.Format("<i>'{0}'</i>", subtitlesText) ) : base.info; }
        }

        private AudioTrack track {
            get { return (AudioTrack)parent; }
        }

        private AudioSource source {
            get { return track.source; }
        }

        protected override void OnEnter() { Do(); }
        protected override void OnReverseEnter() { Do(); }
        protected override void OnExit() { Undo(); }
        protected override void OnReverse() { Undo(); }

        void Do() {
            if ( source != null ) {
                source.clip = audioClip;
            }
        }

        protected override void OnUpdate(float time, float previousTime) {
            if ( source != null ) {
                var weight = Easing.Ease(EaseType.QuadraticInOut, 0, 1, GetClipWeight(time));
                var settings = track.sampleSettings;
                settings.volume *= weight * volume;
                settings.pitch *= pitch;
                settings.pan += stereoPan;

                AudioSampler.Sample(source, audioClip, time - clipOffset, previousTime - clipOffset, settings);

                if ( !string.IsNullOrEmpty(subtitlesText) ) {
                    var lerpColor = subtitlesColor;
                    lerpColor.a = weight;
                    DirectorGUI.UpdateSubtitles(string.Format("{0}{1}", parent is ActorAudioTrack ? ( actor.name.Replace("(Clone)", "") + ": " ) : "", subtitlesText), lerpColor);
                }
            }
        }

        void Undo() {
            if ( source != null ) {
                source.clip = null;
            }
        }

        ////////////////////////////////////////
        ///////////GUI AND EDITOR STUFF/////////
        ////////////////////////////////////////
#if UNITY_EDITOR

        protected override void OnClipGUI(Rect rect) {
            EditorTools.DrawLoopedAudioTexture(rect, audioClip, length, clipOffset);
        }

#endif
    }
}