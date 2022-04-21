using UnityEngine;
using System.Collections;

namespace Slate
{

    ///An interface for all cameras handled by slate
    public interface IDirectableCamera
    {
        GameObject gameObject { get; }
        Camera cam { get; }
        Vector3 position { get; set; }
        Quaternion rotation { get; set; }
        float fieldOfView { get; set; }
        float focalDistance { get; set; }
        float focalLength { get; set; }
        float focalAperture { get; set; }
    }
}