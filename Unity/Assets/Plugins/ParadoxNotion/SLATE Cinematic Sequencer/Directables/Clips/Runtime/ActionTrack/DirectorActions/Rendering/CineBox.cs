using UnityEngine;
using System.Collections;

namespace Slate.ActionClips
{

    [Category("Rendering")]
    [Description("Controls the visibility of the Cinematic Letterbox. This is only usefull if you want a Cinematic Letterbox only in some parts of your cutscene. For this to work correctly, you should set the CineBox effect in the Cutscene CameraTrack to 0, so that the effect is not overriden.")]
    public class CineBox : DirectorActionClip
    {

        [SerializeField]
        [HideInInspector]
        private float _length = 2;
        [SerializeField]
        [HideInInspector]
        private float _blendIn = 0.25f;
        [SerializeField]
        [HideInInspector]
        private float _blendOut = 0.25f;

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

        protected override void OnUpdate(float time) {
            DirectorGUI.UpdateLetterbox(GetClipWeight(time));
        }
    }
}
