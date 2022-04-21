using UnityEngine;
using System.Collections;

namespace Slate.ActionClips
{

    [Category("Utilities")]
    [Description("Render the cutscene in an image-sequence for the selected duration of the clip. Rendering is done in playmode only.")]
    [System.Obsolete("Use the new 'Render' feature")]
    public class ScreenCapture : DirectorActionClip
    {

#if UNITY_EDITOR

        [SerializeField]
        [HideInInspector]
        private float _length = 1f;

        public string filename = "flipbook";
        private int lastCapturedFrame;

        public override string info {
            get { return string.Format("Record\n'{0}_#0000.png'", filename); }
        }

        public override float length {
            get { return _length; }
            set { _length = value; }
        }

        protected override void OnEnter() {

            if ( !Application.isPlaying ) {
                Debug.Log("Screen Capture will take effect in playmode");
                return;
            }

            if ( !UnityEditor.AssetDatabase.IsValidFolder("Assets/Flipbooks") ) {
                UnityEditor.AssetDatabase.CreateFolder("Assets", "Flipbooks");
            }

            lastCapturedFrame = 0;
        }

        protected override void OnUpdate(float deltaTime, float previousTime) {

            if ( !Application.isPlaying || deltaTime == previousTime || string.IsNullOrEmpty(filename) ) {
                return;
            }

            var frame = (int)Mathf.Round(deltaTime * 30);
            frame = Mathf.Min(lastCapturedFrame, frame);
            if ( deltaTime > previousTime ) { lastCapturedFrame++; }
            if ( deltaTime < previousTime ) { lastCapturedFrame--; }
            var final = string.Format(@"Assets\Flipbooks\{0}_#{1}.png", filename, frame.ToString("0000"));
            Application.CaptureScreenshot(final);
            Debug.Log(final);
        }

        protected override void OnReverseEnter() {
            lastCapturedFrame = (int)Mathf.Round(length * 30);
        }

#endif

    }
}