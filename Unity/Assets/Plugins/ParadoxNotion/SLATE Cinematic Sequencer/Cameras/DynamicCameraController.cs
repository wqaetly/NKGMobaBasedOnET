using UnityEngine;

namespace Slate
{

    ///Handles dynamic/procedural camera animation
    [System.Serializable]
    public class DynamicCameraController
    {

        [System.Serializable]
        public class Transposer
        {

            public enum TrackingMode
            {
                None,
                OffsetTracking,
                RailTracking,
            }

            public enum OffsetMode
            {
                LocalSpace,
                WorldSpace
            }

            public TrackingMode trackingMode;

            //OffsetTracking
            [Tooltip("The Target from which an offset will be applied.")]
            public Transform target;
            [Tooltip("The offset from the target to mount the camera to.")]
            public Vector3 targetOffset = new Vector3(0, 0, -1);
            [Tooltip("Is the offset relative to the target's Local or World Space?")]
            public OffsetMode offsetMode;

            //RailTracking
            [Tooltip("The starting point of the rail")]
            public Vector3 railStart = new Vector3(-5, 1, 0);
            [Tooltip("The end point of the rail")]
            public Vector3 railEnd = new Vector3(5, 1, 0);
            [Tooltip("Optional forwards/backwards offset on the rail")]
            public float railOffset;

            [Range(MIN_DAMP, MAX_DAMP)]
            [Tooltip("The smoothness to be applied.")]
            public float smoothDamping = 3f;
        }


        [System.Serializable]
        public class Composer
        {

            public enum TrackingMode
            {
                None,
                FrameComposition
            }

            public TrackingMode trackingMode;
            [Tooltip("The target subject we want to stay within the composition frame.")]
            public Transform target;
            [Tooltip("The point of interest offset from the target in it's local space we actually care about to stay within the composition frame.")]
            public Vector3 targetOffset;
            [Min(0f)]
            [Tooltip("The point of interest area size to stay within the composition frame.")]
            public float targetSize = 0.25f;
            [Tooltip("The center of the view frame")]
            public Vector2 frameCenter;
            [Tooltip("The extends/size of the view frame")]
            public Vector2 frameExtends = new Vector2(0.3f, 0.3f);
            [Range(-80, 80)]
            [Tooltip("Tilt camera angles")]
            public float dutchTilt = 0;
            [Tooltip("Will try zoom to match view frame with target interest frame")]
            public bool zoomAtTargetFrame;
            [Tooltip("The smoothness to be applied.")]
            [Range(MIN_DAMP, MAX_DAMP)]
            public float smoothDamping = 3f;
        }

        private const float MIN_DAMP = 0.5f;
        private const float MAX_DAMP = 10f;

        [SerializeField]
        private Transposer _transposer = new Transposer();
        [SerializeField]
        private Composer _composer = new Composer();

        private int lastUpdateFrame = -1;

        ///The Transposer
        public Transposer transposer {
            get { return _transposer; }
        }

        ///The Composer
        public Composer composer {
            get { return _composer; }
        }

        ///Does controller controls position?
        public bool controlsPosition {
            get { return _transposer != null && _transposer.trackingMode != Transposer.TrackingMode.None; }
        }

        ///Does controller controls rotation?
        public bool controlsRotation {
            get { return _composer != null && _composer.trackingMode != Composer.TrackingMode.None; }
        }

        ///Does the controller controls FOV?
        public bool controlsFieldOfView {
            get { return _composer != null && _composer.trackingMode == Composer.TrackingMode.FrameComposition && _composer.zoomAtTargetFrame; }
        }

        //Update controller for target directable camera from target directable element (eg clip).
        public void UpdateControllerHard(IDirectableCamera directableCamera, IDirectable directable) { UpdateController(directableCamera, directable, true); }
        public void UpdateControllerSoft(IDirectableCamera directableCamera, IDirectable directable) { UpdateController(directableCamera, directable, false); }
        void UpdateController(IDirectableCamera directableCamera, IDirectable directable, bool isHard) {

            //UpdateController is called more than once per frame if same shot is used from multiple clips in a cutscene.
            //Ensure that this is updated only once per frame.
            if ( !isHard && lastUpdateFrame == Time.frameCount ) { return; }
            lastUpdateFrame = Time.frameCount;

            //if rootdelta == 0, use Time.delta
            var rootDelta = Mathf.Abs(directable.root.currentTime - directable.root.previousTime);
            var deltaTime = rootDelta != 0 ? rootDelta : Time.deltaTime;
            var cam = directableCamera.cam;

            if ( transposer.target != null && transposer.trackingMode != Transposer.TrackingMode.None ) {
                var targetPos = transposer.target.position;
                if ( transposer.offsetMode == Transposer.OffsetMode.LocalSpace ) {
                    targetPos = transposer.target.TransformPoint(transposer.targetOffset);
                }
                if ( transposer.offsetMode == Transposer.OffsetMode.WorldSpace ) {
                    targetPos = transposer.target.position + transposer.targetOffset;
                }

                //offset tracking
                if ( transposer.trackingMode == Transposer.TrackingMode.OffsetTracking ) {
                    //...
                }

                //rail tracking
                if ( transposer.trackingMode == Transposer.TrackingMode.RailTracking ) {
                    var aT = targetPos - transposer.railStart;
                    var bT = ( transposer.railEnd - transposer.railStart ).normalized;
                    var projectT = Vector3.Project(aT, bT) + transposer.railStart;

                    //clamp it
                    if ( ( projectT - transposer.railStart ).normalized != ( transposer.railEnd - transposer.railStart ).normalized ) {
                        projectT = transposer.railStart;
                    }

                    var dMax = Vector3.Distance(transposer.railStart, transposer.railEnd);
                    var dCurrent = Vector3.Distance(transposer.railStart, projectT);
                    var normDistance = ( dCurrent / dMax ) + transposer.railOffset;
                    targetPos = Vector3.Lerp(transposer.railStart, transposer.railEnd, normDistance);
                }


                if ( isHard || transposer.smoothDamping == MIN_DAMP ) {
                    directableCamera.position = targetPos;
                }

                directableCamera.position = Vector3.Lerp(directableCamera.position, targetPos, deltaTime * ( MAX_DAMP / transposer.smoothDamping ));
            }

            if ( composer.target != null && composer.trackingMode != Composer.TrackingMode.None ) {
                if ( composer.trackingMode == Composer.TrackingMode.FrameComposition ) {
                    var wasRotation = directableCamera.rotation;
                    var pointWorldPos = composer.target.TransformPoint(composer.targetOffset);
                    var rotationToTarget = Quaternion.LookRotation(pointWorldPos - directableCamera.position);
                    directableCamera.rotation = rotationToTarget;

                    var left = Mathf.Clamp01(( composer.frameCenter.x + 0.5f ) - composer.frameExtends.x / 2);
                    var right = Mathf.Clamp01(( composer.frameCenter.x + 0.5f ) + composer.frameExtends.x / 2);
                    var top = Mathf.Clamp01(( composer.frameCenter.y + 0.5f ) - composer.frameExtends.y / 2);
                    var bottom = Mathf.Clamp01(( composer.frameCenter.y + 0.5f ) + composer.frameExtends.y / 2);
                    var viewFrame = Rect.MinMaxRect(left, top, right, bottom);

                    var worldFrameCenter = cam.ViewportToWorldPoint(new Vector3(1 - viewFrame.center.x, viewFrame.center.y, Vector3.Distance(directableCamera.position, pointWorldPos)));
                    var rotationToFrame = Quaternion.LookRotation(worldFrameCenter - directableCamera.position);
                    directableCamera.rotation = wasRotation;

                    var interestBounds = new Bounds(pointWorldPos, new Vector3(composer.targetSize, composer.targetSize, composer.targetSize) * 2);
                    var interestViewFrame = interestBounds.ToViewRect(cam);

                    if ( isHard || composer.smoothDamping == MIN_DAMP ) {

                        directableCamera.rotation = rotationToFrame;

                    } else {

                        var normxMin = ( viewFrame.xMin - interestViewFrame.xMin );
                        var normxMax = ( interestViewFrame.xMax - viewFrame.xMax );
                        var normyMin = ( viewFrame.yMin - interestViewFrame.yMin );
                        var normyMax = ( interestViewFrame.yMax - viewFrame.yMax );

                        var norm = Mathf.Max(normxMin, normxMax, normyMin, normyMax, 0);
                        var normDamp = ( MAX_DAMP / composer.smoothDamping ) * norm;
                        directableCamera.rotation = Quaternion.Lerp(wasRotation, rotationToFrame, normDamp);
                    }

                    //dutch tilt
                    if ( composer.dutchTilt != 0 ) {
                        var euler = directableCamera.cam.transform.GetLocalEulerAngles();
                        euler.z = composer.dutchTilt;
                        directableCamera.cam.transform.SetLocalEulerAngles(euler);
                    }

                    //zoom at frame
                    if ( composer.zoomAtTargetFrame ) {
                        var rectDiff = interestViewFrame.height - viewFrame.height;
                        var fov = directableCamera.fieldOfView;
                        var sign = Mathf.Sign(rectDiff);
                        fov += ( Mathf.Abs(rectDiff) * 20 ) * sign;
                        directableCamera.fieldOfView = Mathf.Clamp(fov, 5, 70);
                    }

                }
            }
        }


#if UNITY_EDITOR

        ///OnGUI
        public void DoGUI(IDirectableCamera directableCamera, Rect container) {
            if ( composer.target != null && composer.trackingMode != Composer.TrackingMode.None ) {

                if ( composer.trackingMode == Composer.TrackingMode.FrameComposition ) {
                    var cam = directableCamera.cam;

                    var left = Mathf.Clamp01(( composer.frameCenter.x + 0.5f ) - composer.frameExtends.x / 2);
                    var right = Mathf.Clamp01(( composer.frameCenter.x + 0.5f ) + composer.frameExtends.x / 2);
                    var top = Mathf.Clamp01(( composer.frameCenter.y + 0.5f ) - composer.frameExtends.y / 2);
                    var bottom = Mathf.Clamp01(( composer.frameCenter.y + 0.5f ) + composer.frameExtends.y / 2);
                    var viewFrame = Rect.MinMaxRect(left, top, right, bottom);

                    //view frame
                    var min = new Vector2(viewFrame.xMin * container.width, viewFrame.yMin * container.height);
                    var max = new Vector2(viewFrame.xMax * container.width, viewFrame.yMax * container.height);
                    GUI.Box(Rect.MinMaxRect(min.x, min.y, max.x, max.y), "", Styles.hollowFrameStyle);

                    //dark
                    var leftGuide = Rect.MinMaxRect(0, 0, min.x, container.height);
                    var rightGuide = Rect.MinMaxRect(max.x, 0, container.width, container.height);
                    var topGuide = Rect.MinMaxRect(min.x, 0, max.x, min.y);
                    var bottomGuide = Rect.MinMaxRect(min.x, max.y, max.x, container.height);
                    GUI.color = new Color(0, 0, 0, 0.2f);
                    GUI.DrawTexture(leftGuide, Styles.whiteTexture);
                    GUI.DrawTexture(rightGuide, Styles.whiteTexture);
                    GUI.DrawTexture(topGuide, Styles.whiteTexture);
                    GUI.DrawTexture(bottomGuide, Styles.whiteTexture);

                    //lines
                    GUI.color = new Color(1, 1, 1, 0.2f);
                    var leftLine = Rect.MinMaxRect(min.x, 0, min.x + 2, container.height);
                    var rightLine = Rect.MinMaxRect(max.x - 2, 0, max.x, container.height);
                    var topLine = Rect.MinMaxRect(0, min.y, container.width, min.y + 2);
                    var bottonLine = Rect.MinMaxRect(0, max.y - 2, container.width, max.y);
                    GUI.DrawTexture(leftLine, Styles.whiteTexture);
                    GUI.DrawTexture(rightLine, Styles.whiteTexture);
                    GUI.DrawTexture(topLine, Styles.whiteTexture);
                    GUI.DrawTexture(bottonLine, Styles.whiteTexture);

                    GUI.color = Color.white;

                    var pointPos = composer.target.TransformPoint(composer.targetOffset);
                    var bounds = new Bounds(pointPos, new Vector3(composer.targetSize, composer.targetSize, composer.targetSize) * 2);
                    var rect = bounds.ToViewRect(cam);
                    rect = new Rect(rect.x * container.width, rect.y * container.height, rect.width * container.width, rect.height * container.height);
                    GUI.color = Color.green;
                    GUI.Box(rect, "", Styles.hollowFrameStyle); //subject area
                    var pointRect = new Rect(0, 0, 10, 10);
                    pointRect.center = rect.center;
                    GUI.DrawTexture(pointRect, Styles.plusIcon); //subject center
                    GUI.color = Color.white;

                    //label info
                    var label = string.Format("'{0}' FrameComposition", cam.name);
                    var labelSize = GUI.skin.GetStyle("label").CalcSize(new GUIContent(label));
                    var labelRect = new Rect(4, 4, labelSize.x + 2, labelSize.y);
                    GUI.DrawTexture(labelRect, Styles.whiteTexture);
                    GUI.color = Color.grey;
                    GUI.Label(labelRect, label);
                    GUI.color = Color.white;
                }
            }
        }

        ///OnDrawGizmos
        public void DoGizmos(IDirectableCamera directableCamera) {
            Gizmos.color = Prefs.gizmosColor;
            if ( transposer.target != null && transposer.trackingMode != Transposer.TrackingMode.None ) {
                var targetPos = transposer.target.position;
                if ( transposer.offsetMode == Transposer.OffsetMode.LocalSpace ) {
                    targetPos = transposer.target.TransformPoint(transposer.targetOffset);
                }
                if ( transposer.offsetMode == Transposer.OffsetMode.WorldSpace ) {
                    targetPos = transposer.target.position + transposer.targetOffset;
                }

                Gizmos.DrawLine(transposer.target.position, targetPos);
                Gizmos.DrawSphere(targetPos, 0.05f);

                if ( transposer.trackingMode == Transposer.TrackingMode.OffsetTracking ) {
                    //...
                }

                if ( transposer.trackingMode == Transposer.TrackingMode.RailTracking ) {
                    Gizmos.DrawLine(transposer.railStart, transposer.railEnd);
                    Gizmos.DrawLine(directableCamera.position, targetPos);
                    Gizmos.DrawSphere(transposer.railStart, 0.05f);
                    Gizmos.DrawSphere(transposer.railEnd, 0.05f);
                }
            }

            if ( composer.target != null && composer.trackingMode != Composer.TrackingMode.None ) {
                var targetPos = composer.target.TransformPoint(composer.targetOffset);
                if ( composer.trackingMode == Composer.TrackingMode.FrameComposition ) {
                    Gizmos.DrawSphere(targetPos, 0.05f);
                    Gizmos.DrawWireSphere(targetPos, composer.targetSize);
                    Gizmos.DrawLine(directableCamera.position, targetPos);
                }
            }
        }

        ///OnSceneGUI
        public void DoSceneGUI(IDirectableCamera directableCamera) {
            if ( transposer.target != null && transposer.trackingMode != Transposer.TrackingMode.None ) {
                if ( transposer.trackingMode == Transposer.TrackingMode.RailTracking ) {
                    UnityEditor.EditorGUI.BeginChangeCheck();
                    var rStart = UnityEditor.Handles.PositionHandle(transposer.railStart, Quaternion.identity);
                    var rEnd = UnityEditor.Handles.PositionHandle(transposer.railEnd, Quaternion.identity);
                    if ( UnityEditor.EditorGUI.EndChangeCheck() ) {
                        UnityEditor.Undo.RecordObject(directableCamera as Object, "Rail Change");
                        transposer.railStart = rStart;
                        transposer.railEnd = rEnd;
                    }
                }
            }
        }

#endif

    }
}