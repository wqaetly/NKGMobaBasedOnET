using UnityEngine;
using System.Collections;
using System.Linq;
using UnityEngine.Audio;

namespace Slate
{

    [UniqueElement]
    [Attachable(typeof(DirectorGroup))]
    [Description("The Camera Track is the track within wich you create your camera shots and moves. Once the Camera Track becomes active, the Director Camera will be enabled. You can control when the Director Camera takes effect by setting the 'Active Time Offset', while the Blend In/Out parameters control the ammount of blending there will be from the game camera to the first and the last shot of the track.\nIf you don't want a cinematic letterbox effect, you can set CineBox Fade Time to 0.\nIf you don't want to use Slate Camera features, you can simply disable or delete this track.")]
    [Icon(typeof(Camera))]
    ///The CameraTrack is responsible to camera direction of the Cutscene
    public class CameraTrack : CutsceneTrack
    {

        //there can only be one active camera track
        private static CameraTrack activeCameraTrack;

        [SerializeField]
        [HideInInspector]
        private float _startTimeOffset;
        [SerializeField]
        [HideInInspector]
        private float _endTimeOffset;

        [HideInInspector]
        [Min(0)]
        public float _blendIn = 0f;
        [HideInInspector]
        [Min(0)]
        public float _blendOut = 0f;
        [HideInInspector]
        public EaseType interpolation = EaseType.QuarticInOut;
        [HideInInspector]
        [Range(0, 1)]
        public float cineBoxFadeTime = 0.5f;
        [HideInInspector]
        public float appliedSmoothing = 0f;
        [HideInInspector]
        public Camera exitCameraOverride;

        private GameCamera entryCamera;

        public CameraShot firstShot { get; private set; }
        public CameraShot lastShot { get; private set; }
        public CameraShot currentShot { get; set; }

        public override string info {
            get { return string.Format("Game Blend In {0} / Out {1}", _blendIn.ToString(), _blendOut.ToString()); }
        }

        public override float startTime {
            get { return _startTimeOffset; }
            set { _startTimeOffset = Mathf.Clamp(value, 0, parent.endTime / 2); }
        }

        public override float endTime {
            get { return parent.endTime - _endTimeOffset; }
            set { _endTimeOffset = Mathf.Clamp(parent.endTime - value, 0, parent.endTime / 2); }
        }

        public override float blendIn {
            get
            {
                if ( _blendIn == 0 || firstShot == null ) return 0;
                return firstShot.startTime - this.startTime + _blendIn;
            }
            set { _blendIn = value; }
        }

        public override float blendOut {
            get
            {
                if ( _blendOut == 0 || lastShot == null ) return 0;
                return this.endTime - lastShot.endTime + _blendOut;
            }
            set { _blendOut = value; }
        }


        protected override void OnEnter() {

            if ( activeCameraTrack != null ) {
                return;
            }

            activeCameraTrack = this;

            firstShot = (CameraShot)clips.FirstOrDefault(s => s.startTime >= this.startTime);
            lastShot = (CameraShot)clips.LastOrDefault(s => s.endTime <= this.endTime);
            currentShot = firstShot;

            if ( exitCameraOverride != null ) {
                exitCameraOverride.gameObject.SetActive(false);
            }

            DirectorCamera.Enable();
        }

        protected override void OnUpdate(float time, float previousTime) {

            if ( activeCameraTrack != this ) {
                return;
            }

            if ( cineBoxFadeTime > 0 ) {
                //use cinebox time as blendInOut override parameter for GetTrackWeight.
                DirectorGUI.UpdateLetterbox(GetTrackWeight(time, cineBoxFadeTime));
            }

            if ( exitCameraOverride != null ) {
                if ( time > blendIn && entryCamera == null ) {
                    entryCamera = DirectorCamera.gameCamera;
                    DirectorCamera.gameCamera = exitCameraOverride.GetAddComponent<GameCamera>(); ;
                }

                if ( time <= blendIn && entryCamera != null ) {
                    DirectorCamera.gameCamera = entryCamera;
                    entryCamera = null;
                }
            }


            var weight = GetTrackWeight(time);

            IDirectableCamera source = null;
            IDirectableCamera target = null;

            if ( currentShot != null && currentShot.targetShot != null ) {
                target = currentShot.targetShot;
                if ( currentShot.blendInEffect == CameraShot.BlendInEffectType.EaseIn ) {
                    if ( currentShot != firstShot && time <= currentShot.startTime + currentShot.blendIn ) {
                        source = currentShot.previousShot.targetShot;
                        weight *= currentShot.GetClipWeight(( time + this.startTime ) - currentShot.startTime);
                    }
                }
            }

            //passing null source = game camera, null target = the director camera itself.
            DirectorCamera.Update(source, target, interpolation, weight, appliedSmoothing);
        }

        protected override void OnExit() {
            if ( activeCameraTrack == this ) {
                activeCameraTrack = null;
                DirectorCamera.Disable();
            }
        }

        protected override void OnReverseEnter() {
            if ( activeCameraTrack == null ) {
                activeCameraTrack = this;
                DirectorCamera.Enable();
            }
        }

        protected override void OnReverse() {
            if ( activeCameraTrack == this ) {
                activeCameraTrack = null;
                DirectorCamera.Disable();
            }
        }

        ////////////////////////////////////////
        ///////////GUI AND EDITOR STUFF/////////
        ////////////////////////////////////////
#if UNITY_EDITOR

        public override float defaultHeight {
            get { return Prefs.showShotThumbnails ? 60f : base.defaultHeight; }
        }

        public override void OnTrackTimelineGUI(Rect posRect, Rect timeRect, float cursorTime, System.Func<float, float> TimeToPos) {

            base.OnTrackTimelineGUI(posRect, timeRect, cursorTime, TimeToPos);

            UnityEditor.Handles.color = UnityEditor.EditorGUIUtility.isProSkin ? Color.white : Color.black;
            if ( blendIn > 0 ) {
                var first = clips.FirstOrDefault(s => s.startTime >= this.startTime);
                if ( first != null ) {
                    var a = new Vector2(TimeToPos(this.startTime), posRect.y + defaultHeight / 2);
                    var b = new Vector2(TimeToPos(first.startTime), a.y);
                    b.x -= 1;
                    UnityEditor.Handles.DrawLine(a, b);
                    UnityEditor.Handles.DrawLine(b, new Vector2(b.x - 5, b.y - 5));
                    UnityEditor.Handles.DrawLine(b, new Vector2(b.x - 5, b.y + 5));
                }
            }

            if ( blendOut > 0 ) {
                var last = clips.LastOrDefault(s => s.endTime <= this.endTime);
                if ( last != null ) {
                    var a = new Vector2(TimeToPos(this.endTime), posRect.y + defaultHeight / 2);
                    var b = new Vector2(TimeToPos(last.endTime), a.y);
                    UnityEditor.Handles.DrawLine(a, b);
                    UnityEditor.Handles.DrawLine(a, new Vector2(a.x - 5, a.y - 5));
                    UnityEditor.Handles.DrawLine(a, new Vector2(a.x - 5, a.y + 5));
                }
            }
            UnityEditor.Handles.color = Color.white;


            if ( exitCameraOverride != null ) {
                var text = string.Format("<size=12><b>ExitCamera: '{0}'</b></size>", exitCameraOverride.name);
                var size = GUI.skin.GetStyle("Label").CalcSize(new GUIContent(text));
                var r = new Rect(TimeToPos(endTime) + 5, 0, size.x, size.y);
                r.center = new Vector2(r.center.x, posRect.y + defaultHeight / 2);
                GUI.color = new Color(0, 0, 0, 0.3f);
                GUI.DrawTexture(r, Styles.whiteTexture);
                GUI.color = Color.white;
                GUI.Label(r, text);
            }
        }

#endif
    }
}