using UnityEngine;
using System.Collections;

namespace Slate
{

    ///A stub component to interface with the game MainCamera the same as the rest.
    ///We never 'set' these, we only need the getters.
    public class GameCamera : MonoBehaviour, IDirectableCamera
    {

        private Camera _cam;
        public Camera cam {
            get { return _cam != null ? _cam : _cam = GetComponent<Camera>(); }
        }

        public Vector3 position {
            get { return transform.position; }
            set { }
        }

        public Quaternion rotation {
            get { return transform.rotation; }
            set { }
        }

        public float fieldOfView {
            get { return cam.orthographic ? cam.orthographicSize : cam.fieldOfView; }
            set { }
        }

        public float focalDistance {
            get { return 10f; }
            set { }
        }

        public float focalLength {
            get { return 50f; }
            set { }
        }

        public float focalAperture {
            get { return 5f; }
            set { }
        }
    }
}