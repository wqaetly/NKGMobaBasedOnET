using UnityEngine;
using UnityEngine.Audio;

namespace Slate.ActionClips
{

    [Category("Control")]
    [Description("Will transit the given audio mixer to the given snapshot")]
    public class TransitAudioMixerSnapshot : DirectorActionClip
    {

        [SerializeField]
        [HideInInspector]
        private float _length = 1f;

        public AudioMixer audioMixer;
        public string snapshotName;

        private AudioMixerSnapshot snapshot;

        public override float length {
            get { return _length; }
            set { _length = value; }
        }

        public override float blendIn {
            get { return length; }
        }

        public override bool isValid {
            get { return audioMixer != null; }
        }

        protected override void OnReverseEnter() { Do(); }
        protected override void OnEnter() { Do(); }
        void Do() {
            var snapshot = audioMixer.FindSnapshot(snapshotName);
            if ( snapshot != null ) {
                snapshot.TransitionTo(length);
            }
        }
    }
}