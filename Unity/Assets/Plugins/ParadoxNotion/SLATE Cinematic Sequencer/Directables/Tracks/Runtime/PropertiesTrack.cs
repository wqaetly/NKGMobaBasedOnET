using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Slate
{

    [UniqueElement]
    [Name("Properties Track")]
    [Category("Legacy")]
    [Description("With the Properties Track, you can select to animate any supported type property or field on any component on the actor, or within it's whole transform hierarchy.\n\nNote, that you can do exactly the same, by using the 'Animate Properties' ActionClip added in an 'Action Track'.")]
    ///Properties Tracks are able to animate any supported property type with AnimationCurves
    abstract public class PropertiesTrack : CutsceneTrack, IKeyable
    {

        [SerializeField]
        [HideInInspector]
        private AnimationDataCollection _animationData = new AnimationDataCollection();
        public AnimationDataCollection animationData {
            get { return _animationData; }
        }

        public object animatedParametersTarget {
            get { return actor; }
        }

        protected override void OnAfterValidate() {
            animationData.Validate(this);
        }

        protected override void OnEnter() {
            animationData.SetVirtualTransformParent(this.GetSpaceTransform(TransformSpace.CutsceneSpace));
            animationData.SetSnapshot();
        }

        protected override void OnUpdate(float time, float previousTime) {
            animationData.Evaluate(time, previousTime, this.GetWeight(time));
        }

        protected override void OnReverse() {
            animationData.RestoreSnapshot();
        }


        ///----------------------------------------------------------------------------------------------
        ///---------------------------------------UNITY EDITOR-------------------------------------------
#if UNITY_EDITOR

        [SerializeField]
        [HideInInspector]
        private bool _showCurves;

        public override bool showCurves {
            get { return _showCurves; }
            set { _showCurves = value; }
        }

        public override float defaultHeight {
            get { return 16f; }
        }

        public override void OnTrackInfoGUI(Rect trackRect) {
            var e = Event.current;
            var wasEnable = GUI.enabled;
            GUI.enabled = true;

            var foldRect = new Rect(4, 0, 14, defaultHeight);
            var labelRect = new Rect(foldRect.xMax, 0, trackRect.width, defaultHeight);
            showCurves = UnityEditor.EditorGUI.Foldout(foldRect, showCurves, string.Empty);
            GUI.Label(labelRect, this.name, Styles.leftLabel);

            if ( showCurves ) {
                var paramsRect = new Rect(0, defaultHeight, trackRect.width, trackRect.height);
                DoParamsInfoGUI(e, paramsRect, this, true);
            }
            GUI.enabled = wasEnable;
        }

        public override void OnTrackTimelineGUI(Rect posRect, Rect timeRect, float cursorTime, System.Func<float, float> TimeToPos) {

            var e = Event.current;
            var baseDopeRect = new Rect(posRect.xMin, posRect.yMin, posRect.width, defaultHeight);

            GUI.color = new Color(0.5f, 0.5f, 0.5f, 0.3f);
            GUI.Box(baseDopeRect, "", Slate.Styles.clipBoxHorizontalStyle);
            GUI.color = Color.white;

            DopeSheetEditor.DrawDopeSheet(this.animationData, this, baseDopeRect, timeRect.x, timeRect.width);
            if ( showCurves ) {
                var subDopeRect = Rect.MinMaxRect(posRect.xMin, posRect.yMin + defaultHeight, posRect.xMax, posRect.yMax);
                DoClipCurves(e, subDopeRect, timeRect, TimeToPos, this);
            }
        }

        protected override void OnSceneGUI() {

            if ( actor == null || actor.Equals(null) || !showCurves || !isActive || animationData == null || !animationData.isValid ) {
                return;
            }

            for ( var i = 0; i < animationData.animatedParameters.Count; i++ ) {
                var animParam = animationData.animatedParameters[i];
                if ( animParam.isValid && animParam.parameterName == "localPosition" ) {
                    var transform = animParam.ResolvedMemberObject() as Transform;
                    if ( transform != null ) {
                        var context = transform.parent != null ? transform.parent : this.GetSpaceTransform(TransformSpace.CutsceneSpace);
                        CurveEditor3D.Draw3DCurve(animParam, this, context, root.currentTime);
                    }
                }
            }
        }

#endif
    }
}