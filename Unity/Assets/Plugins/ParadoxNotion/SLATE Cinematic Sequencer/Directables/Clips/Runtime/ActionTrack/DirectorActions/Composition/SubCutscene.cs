using UnityEngine;
using System.Collections;

namespace Slate.ActionClips
{

    [Category("Composition")]
    [Description("SubCutscenes are used for organization. Notice that the CameraTrack of the SubCutscene is ignored if this Cutscene already has an active CameraTrack.")]
    public class SubCutscene : DirectorActionClip, ISubClipContainable
    {

        [SerializeField]
        [HideInInspector]
        private float _length = 1f;

        [Required]
        [UnityEngine.Serialization.FormerlySerializedAs("cutscene")]
        public Cutscene subCutscene;
        public float subCutsceneTimeOffset;

        private bool wasCamTrackActive;

        public override string info {
            get
            {
                if ( ReferenceEquals(subCutscene, root) ) { return "        SubCutscene can't be same as this cutscene"; }
                return subCutscene != null ? string.Format("        SubCutscene\n        '{0}'", subCutscene.name) : "No Cutscene Selected";
            }
        }

        public override bool isValid {
            get { return subCutscene != null && !ReferenceEquals(subCutscene, root); }
        }

        public override float length {
            get { return _length; }
            set { _length = value; }
        }

        new public GameObject actor { //this is not really needed but makes double clicking the clip, select the target cutscene
            get { return isValid ? subCutscene.gameObject : base.actor; }
        }

        float ISubClipContainable.subClipOffset {
            get { return subCutsceneTimeOffset; }
            set { subCutsceneTimeOffset = value; }
        }

        float ISubClipContainable.subClipLength {
            get { return isValid ? subCutscene.length : 0; }
        }

        float ISubClipContainable.subClipSpeed {
            get { return isValid ? subCutscene.playbackSpeed : 0; }
        }

        ///----------------------------------------------------------------------------------------------

        protected override void OnEnter() {
            subCutscene.Sample(0);
            if ( subCutscene.cameraTrack != null ) {
                wasCamTrackActive = subCutscene.cameraTrack.isActive;
                subCutscene.cameraTrack.isActive = false;
            }
        }

        protected override void OnReverseEnter() {
            subCutscene.Sample(subCutscene.length);
            if ( subCutscene.cameraTrack != null ) {
                wasCamTrackActive = subCutscene.cameraTrack.isActive;
                subCutscene.cameraTrack.isActive = false;
            }
        }

        protected override void OnExit() {
            if ( subCutscene.cameraTrack != null ) {
                subCutscene.cameraTrack.isActive = wasCamTrackActive;
            }
            subCutscene.Sample(subCutscene.length);
        }

        protected override void OnReverse() {
            if ( subCutscene.cameraTrack != null ) {
                subCutscene.cameraTrack.isActive = wasCamTrackActive;
            }
            subCutscene.Sample(0);
        }

        protected override void OnUpdate(float time, float previousTime) {
            time = ( time - subCutsceneTimeOffset ) * subCutscene.playbackSpeed;
            var delta = time - previousTime;
            subCutscene.Sample(Mathf.Repeat(time, subCutscene.length + delta));
        }


        ////////////////////////////////////////
        ///////////GUI AND EDITOR STUFF/////////
        ////////////////////////////////////////
#if UNITY_EDITOR

        protected override void OnClipGUI(Rect rect) {
            if ( isValid ) {

                EditorTools.DrawLoopedLines(rect, subCutscene.length / subCutscene.playbackSpeed, this.length, subCutsceneTimeOffset);

                GUI.color = new Color(1, 1, 1, 0.9f);
                GUI.DrawTexture(new Rect(0, 0, rect.height, rect.height), Slate.Styles.cutsceneIcon);
                GUI.color = Color.white;
            }
        }

#endif
    }
}