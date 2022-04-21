using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

namespace Slate
{

    [Name("Audio Track")]
    [Description("All audio clips played by this track will be send to the selected AudioMixer if any.")]
    [Icon(typeof(AudioClip))]
    ///AudioTracks are able to play AudioClips through the PlayAudio ActionClip
    abstract public class AudioTrack : CutsceneTrack
    {

        [SerializeField]
        protected AudioMixerGroup _outputMixer;
        [SerializeField]
        [Range(0, 1)]
        protected float _masterVolume = 1f;
        [SerializeField]
        [Range(-3, 3)]
        protected float _masterPitch = 1f;
        [SerializeField]
        [Range(-1, 1)]
        protected float _masterStereoPan;
        [SerializeField]
        [Range(0, 1)]
        protected float _masterSpatialBlend;
        [SerializeField]
        protected bool _ignoreTimeScale;

        public override string info {
            get { return string.Format("Mixer: {0} | Volume: {1}", mixer != null ? mixer.name : "NONE", _masterVolume.ToString("0.0")); }
        }

        public AudioSource source { get; private set; }
        public AudioSampler.SampleSettings sampleSettings { get; private set; }

        public AudioMixerGroup mixer {
            get { return _outputMixer; }
        }

        virtual public bool useAudioSourceOnActor {
            get { return false; }
        }


        protected override void OnEnter() { Enable(); }
        protected override void OnReverseEnter() { Enable(); }

        protected override void OnUpdate(float time, float previousTime) {
            if ( !useAudioSourceOnActor ) {
                if ( source != null && !( parent is DirectorGroup ) ) {
                    source.transform.position = actor.transform.position;
                }
            }
        }

        protected override void OnExit() { Disable(); }
        protected override void OnReverse() { Disable(); }

        void Enable() {
            if ( useAudioSourceOnActor ) {
                source = actor.GetComponent<AudioSource>();
            }
            if ( source == null ) {
                source = AudioSampler.GetSourceForID(this);
            }
            SetAndApplySettings();
        }

        void Disable() {
            if ( !useAudioSourceOnActor ) {
                AudioSampler.ReleaseSourceForID(this);
            }
            source = null;
        }

        void SetAndApplySettings() {
            if ( source != null ) {
                source.outputAudioMixerGroup = mixer;
                var settings = sampleSettings;
                settings.volume = _masterVolume;
                settings.pitch = _masterPitch;
                settings.pan = _masterStereoPan;
                settings.spatialBlend = _masterSpatialBlend;
                settings.ignoreTimescale = _ignoreTimeScale;
                sampleSettings = settings;
            }
        }

        public void SetVolume(float volume) {
            _masterVolume = volume;
            SetAndApplySettings();
        }

    }
}