namespace Slate
{

    [Attachable(typeof(ActorGroup))]
    public class ActorPropertiesTrack : PropertiesTrack
    {

        //just add some defaults for convenience
        protected override void OnCreate() {
            base.OnCreate();
            animationData.TryAddParameter(this, typeof(UnityEngine.Transform), "localPosition", null);
            animationData.TryAddParameter(this, typeof(UnityEngine.Transform), "localEulerAngles", null);
        }
    }
}