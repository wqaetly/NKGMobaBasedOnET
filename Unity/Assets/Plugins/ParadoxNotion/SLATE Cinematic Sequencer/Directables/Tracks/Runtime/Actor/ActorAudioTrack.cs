namespace Slate
{

    [Attachable(typeof(ActorGroup))]
    public class ActorAudioTrack : AudioTrack
    {

        [UnityEngine.SerializeField]
        protected bool _useAudioSourceOnActor;

        public override bool useAudioSourceOnActor {
            get { return _useAudioSourceOnActor; }
        }
    }
}