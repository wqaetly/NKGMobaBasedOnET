namespace Slate
{

    [Attachable(typeof(DirectorGroup))]
    [Description("Use the Director Properties Track to animate any component property of the Director Camera. This is particularly useful to animate image effects rather than transforms, since transforms are handled by the CameraTrack and it's Shot Clips. You can still animate transforms if you are after a one-take, by having a CameraTrack without shot clips.")]
    public class DirectorPropertiesTrack : PropertiesTrack { }
}