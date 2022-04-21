#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using NKGSlate;

namespace Slate
{
    public class CutsceneEditor : EditorWindow
    {
        enum EditorPlaybackState
        {
            Stoped,
            PlayingForwards,
            PlayingBackwards
        }

        struct GuideLine
        {
            public float time;
            public Color color;

            public GuideLine(float time, Color color)
            {
                this.time = time;
                this.color = color;
            }
        }

        ///----------------------------------------------------------------------------------------------
        public static CutsceneEditor current;

        public static event System.Action OnStopInEditor;

        private Cutscene _cutscene;
        private int _cutsceneID;

        //Layout variables
        private static float LEFT_MARGIN
        {
            //caps for consistency. margin on the left side. The width of the group/tracks list.
            get { return Prefs.trackListLeftMargin; }
            set { Prefs.trackListLeftMargin = Mathf.Clamp(value, 230, 400); }
        }

        private const float RIGHT_MARGIN = 16; //margin on the right side
        private const float TOOLBAR_HEIGHT = 20; //the height of the toolbar
        private const float TOP_MARGIN = 40; //top margin AFTER the toolbar
        private const float GROUP_HEIGHT = 21; //height of group headers
        private const float TRACK_MARGINS = 4; //margin between tracks of same group (top/bottom)
        private const float GROUP_RIGHT_MARGIN = 4; //margin at the right side of groups
        private const float TRACK_RIGHT_MARGIN = 4; //margin at the right side of tracks
        private const float FIRST_GROUP_TOP_MARGIN = 22; //initial top margin

        private static readonly Color LIST_SELECTION_COLOR = new Color(0.5f, 0.5f, 1, 0.3f);
        private static readonly Color GROUP_COLOR = new Color(0f, 0f, 0f, 0.25f);

        private Color HIGHLIGHT_COLOR
        {
            get { return isProSkin ? new Color(0.65f, 0.65f, 1) : new Color(0.1f, 0.1f, 0.1f); }
        }

        private float MAGNET_SNAP_INTERVAL
        {
            get { return viewTime * 0.01f; }
        }

        //Layout Rects
        private Rect topLeftRect; //for playback controls
        private Rect topMiddleRect; //for time info
        private Rect leftRect; //for group/track list
        private Rect centerRect; //for timeline


        [System.NonSerialized] private Dictionary<int, ActionClipWrapper> clipWrappers;
        [System.NonSerialized] private Dictionary<ActionClip, ActionClipWrapper> clipWrappersMap;
        [System.NonSerialized] private EditorPlaybackState editorPlaybackState = EditorPlaybackState.Stoped;
        [System.NonSerialized] private Cutscene.WrapMode editorPlaybackWrapMode = Cutscene.WrapMode.Loop;
        [System.NonSerialized] private ActionClipWrapper interactingClip;
        [System.NonSerialized] private bool isMovingScrubCarret;
        [System.NonSerialized] private bool isMovingEndCarret;
        [System.NonSerialized] private bool isMouseButton2Down;
        [System.NonSerialized] private Vector2 scrollPos;
        [System.NonSerialized] private float totalHeight;
        [System.NonSerialized] private CutsceneTrack pickedTrack;
        [System.NonSerialized] private CutsceneGroup pickedGroup;
        [System.NonSerialized] private float lastStartPlayTime;
        [System.NonSerialized] private float editorPreviousTime;

        [System.NonSerialized] private Vector2? multiSelectStartPos;
        [System.NonSerialized] private List<ActionClipWrapper> multiSelection;
        [System.NonSerialized] private Rect preMultiSelectionRetimeMinMax;
        [System.NonSerialized] private int multiSelectionScaleDirection;

        [System.NonSerialized] private Vector2 mousePosition;
        [System.NonSerialized] private Section draggedSection;
        [System.NonSerialized] private bool willRepaint;
        [System.NonSerialized] private bool willDirty;
        [System.NonSerialized] private bool willResample;
        [System.NonSerialized] private System.Action onDoPopup;
        [System.NonSerialized] private bool isResizingLeftMargin;
        [System.NonSerialized] private bool isHelpButtonPressed;
        [System.NonSerialized] private bool showDragDropInfo;
        [System.NonSerialized] private string searchString;
        [System.NonSerialized] private float[] magnetSnapTimesCache;
        [System.NonSerialized] private List<GuideLine> pendingGuides;
        [System.NonSerialized] private System.Action postWindowsGUI;

        [System.NonSerialized] private CutsceneTrack copyTrack;


        [System.NonSerialized] private float timeInfoStart;
        [System.NonSerialized] private float timeInfoEnd;
        [System.NonSerialized] private float timeInfoInterval;
        [System.NonSerialized] private float timeInfoHighMod;

        ///----------------------------------------------------------------------------------------------

        //The current cutscene reference
        public Cutscene cutscene
        {
            get
            {
                if (_cutscene == null)
                {
                    _cutscene = EditorUtility.InstanceIDToObject(_cutsceneID) as Cutscene;
                }

                return _cutscene;
            }
            private set
            {
                _cutscene = value;
                if (value != null)
                {
                    _cutsceneID = value.GetInstanceID();
                }
            }
        }

        ///----------------------------------------------------------------------------------------------

        //The length of the cutscene reference
        public float length
        {
            get { return cutscene.length; }
            set { cutscene.length = value; }
        }

        //The min view time
        public float viewTimeMin
        {
            get { return cutscene.viewTimeMin; }
            set { cutscene.viewTimeMin = value; }
        }

        //The max view time
        public float viewTimeMax
        {
            get { return cutscene.viewTimeMax; }
            set { cutscene.viewTimeMax = value; }
        }

        //The max time currently in view
        public float maxTime
        {
            get { return Mathf.Max(viewTimeMax, length); }
        }

        //The "length" of the currently viewing time
        public float viewTime
        {
            get { return viewTimeMax - viewTimeMin; }
        }

        ///----------------------------------------------------------------------------------------------

        //SHORTCUTS//

        //Is Unity editor pro skin?
        private static bool isProSkin
        {
            get { return EditorGUIUtility.isProSkin; }
        }

        //A white texture
        private static Texture2D whiteTexture
        {
            get { return Slate.Styles.whiteTexture; }
        }

        //Is cutscene reference an asset in project?
        private bool isCutsceneAsset
        {
            get { return cutscene != null && UnityEditor.EditorUtility.IsPersistent(cutscene); }
        }

        //Screen Width. Handles retina.
        private static float screenWidth
        {
            get { return Screen.width / EditorGUIUtility.pixelsPerPoint; }
        }

        //Screen Height. Hanldes retina.
        private static float screenHeight
        {
            get { return Screen.height / EditorGUIUtility.pixelsPerPoint; }
        }

        //The color used in scruber
        private Color scruberColor
        {
            get { return cutscene.isActive ? Color.yellow : new Color(1, 0.3f, 0.3f); }
        }

        ///----------------------------------------------------------------------------------------------

        //UTILITIES

        //Convert time to position
        float TimeToPos(float time)
        {
            return (time - viewTimeMin) / viewTime * centerRect.width;
        }

        //Convert position to time
        float PosToTime(float pos)
        {
            return (pos - LEFT_MARGIN) / centerRect.width * viewTime + viewTimeMin;
        }

        //Round time to nearest working snap interval
        float SnapTime(float time)
        {
            //holding control for precision (ignore snap intervals)
            if (Event.current.control)
            {
                return time;
            }

            return (Mathf.Round(time / Prefs.snapInterval) * Prefs.snapInterval);
        }

        //Do action safely (stop cutscene, do, resample)
        void SafeDoAction(System.Action call)
        {
            var time = cutscene.currentTime;
            Stop(true);
            call();
            cutscene.currentTime = time;
        }

        //Is directable filtered out by search string?
        bool IsFilteredOutBySearch(IDirectable directable, string search)
        {
            if (string.IsNullOrEmpty(search))
            {
                return false;
            }

            if (string.IsNullOrEmpty(directable.name))
            {
                return true;
            }

            return !directable.name.ToLower().Contains(search.ToLower());
        }

        //Draw a vertical guide line at time with color
        void DrawGuideLine(float time, Color color)
        {
            if (time >= viewTimeMin && time <= viewTimeMax)
            {
                var xPos = TimeToPos(time);
                var guideRect = new Rect(xPos + centerRect.x - 1, centerRect.y, 2, centerRect.height);
                GUI.color = color;
                GUI.DrawTexture(guideRect, whiteTexture);
                GUI.color = Color.white;
            }
        }

        //Add a cursor type at rect
        void AddCursorRect(Rect rect, MouseCursor type)
        {
            EditorGUIUtility.AddCursorRect(rect, type);
            willRepaint = true;
        }

        //Pop action GUI calls in popup
        void DoPopup(System.Action call)
        {
            onDoPopup = call;
        }

        //Cache an array of snap times for clip (clip times are excluded)
        //Saved in property .magnetSnapTimesCache
        void CacheMagnetSnapTimes(ActionClip clip = null)
        {
            var result = new List<float>();
            result.Add(0);
            result.Add(length);
            result.Add(cutscene.currentTime);
            if (cutscene.directorGroup != null)
            {
                result.AddRange(cutscene.directorGroup.sections.Select(s => s.time));
            }

            foreach (var cw in clipWrappers)
            {
                var action = cw.Value.action;
                //exclude the target clip and only include the same group
                if (clip == null || (action != clip && action.parent.parent == clip.parent.parent))
                {
                    result.Add(action.startTime);
                    result.Add(action.endTime);
                }
            }

            magnetSnapTimesCache = result.Distinct().ToArray();
        }

        //Find best snap time (closest)
        float? MagnetSnapTime(float time, float[] snapTimes)
        {
            if (snapTimes == null)
            {
                return null;
            }

            var bestDistance = float.PositiveInfinity;
            var bestTime = float.PositiveInfinity;
            for (var i = 0; i < snapTimes.Length; i++)
            {
                var snapTime = snapTimes[i];
                var distance = Mathf.Abs(snapTime - time);
                if (distance < bestDistance)
                {
                    bestDistance = distance;
                    bestTime = snapTime;
                }
            }

            if (Mathf.Abs(bestTime - time) <= MAGNET_SNAP_INTERVAL)
            {
                return bestTime;
            }

            return null;
        }

        ///----------------------------------------------------------------------------------------------
        ///Opens the editor :)
        public static void ShowWindow()
        {
            ShowWindow(null);
        }

        public static void ShowWindow(Cutscene newCutscene)
        {
            var window = EditorWindow.GetWindow(typeof(CutsceneEditor)) as CutsceneEditor;
            window.InitializeAll(newCutscene);
            window.Show();
        }

        //...
        void OnEnable()
        {
            Styles.Load();

#if UNITY_2018_3_OR_NEWER
            UnityEditor.Experimental.SceneManagement.PrefabStage.prefabStageClosing += (stage) =>
            {
                if (cutscene != null && stage.IsPartOfPrefabContents(cutscene.gameObject))
                {
                    Stop(true);
                }
            };
#endif
            UnityEditor.SceneManagement.EditorSceneManager.sceneSaving -= OnWillSaveScene;
            UnityEditor.SceneManagement.EditorSceneManager.sceneSaving += OnWillSaveScene;

#pragma warning disable 618
            EditorApplication.playmodeStateChanged -= InitializeAll;
            EditorApplication.playmodeStateChanged += InitializeAll;
#pragma warning restore

            EditorApplication.update -= OnEditorUpdate;
            EditorApplication.update += OnEditorUpdate;
#if UNITY_2019_3_OR_NEWER
            SceneView.duringSceneGui -= OnSceneGUI;
            SceneView.duringSceneGui += OnSceneGUI;
#else
            SceneView.onSceneGUIDelegate -= OnSceneGUI;
            SceneView.onSceneGUIDelegate += OnSceneGUI;
#endif

            Tools.hidden = false;
            titleContent = new GUIContent("SLATE", Styles.cutsceneIconOpen);
            wantsMouseMove = true;
            autoRepaintOnSceneChange = true;
            minSize = new Vector2(500, 250);
            willRepaint = true;
            showDragDropInfo = true;
            pendingGuides = new List<GuideLine>();

            current = this;
            InitializeAll();
        }

        //...
        void OnDisable()
        {
            UnityEditor.SceneManagement.EditorSceneManager.sceneSaving -= OnWillSaveScene;

#pragma warning disable 618
            EditorApplication.playmodeStateChanged -= InitializeAll;
#pragma warning restore

            EditorApplication.update -= OnEditorUpdate;
#if UNITY_2019_3_OR_NEWER
            SceneView.duringSceneGui -= OnSceneGUI;
#else
            SceneView.onSceneGUIDelegate -= OnSceneGUI;
#endif
            Tools.hidden = false;
            if (cutscene != null && !Application.isPlaying)
            {
                Stop(true);
            }

            current = null;
        }


        //Set a new view when a script is selected in Unity's tab
        void OnSelectionChange()
        {
            if (Selection.activeGameObject != null)
            {
                var cut = Selection.activeGameObject.GetComponent<Cutscene>();
                if (cut != null && cutscene != cut)
                {
                    InitializeAll(cut);
                }
            }
        }

        //Before scene is saved we need to stop so that cutscene changes are reverted.
        void OnWillSaveScene(UnityEngine.SceneManagement.Scene scene, string path)
        {
            if (cutscene != null && cutscene.currentTime > 0)
            {
                Stop(true);
                Debug.LogWarning(
                    "Scene Saved while a cutscene was in preview mode. Cutscene was reverted before saving the scene along with changes it affected.");
            }
        }

        ///Initialize everything
        void InitializeAll()
        {
            InitializeAll(cutscene);
        }

        void InitializeAll(Cutscene newCutscene)
        {
            //first stop current cut if any
            if (cutscene != null)
            {
                if (!Application.isPlaying)
                {
                    Stop(true);
                }
            }

            //set the new
            if (newCutscene != null)
            {
                cutscene = newCutscene;
                CutsceneUtility.selectedObject = null;
                multiSelection = null;
                InitClipWrappers();
                if (!Application.isPlaying)
                {
                    Stop(true);
                }
            }

            willRepaint = true;
        }

        //initialize the action clip wrappers
        void InitClipWrappers()
        {
            if (cutscene == null)
            {
                return;
            }

            multiSelection = null;
            var lastTime = cutscene.currentTime;

            if (!Application.isPlaying)
            {
                Stop(true);
            }

            cutscene.Validate();
            clipWrappers = new Dictionary<int, ActionClipWrapper>();
            clipWrappersMap = new Dictionary<ActionClip, ActionClipWrapper>();
            for (int g = 0; g < cutscene.groups.Count; g++)
            {
                for (int t = 0; t < cutscene.groups[g].tracks.Count; t++)
                {
                    for (int a = 0; a < cutscene.groups[g].tracks[t].clips.Count; a++)
                    {
                        var id = UID(g, t, a);
                        if (clipWrappers.ContainsKey(id))
                        {
                            Debug.LogError("Collided UIDs. This should really not happen but it did!");
                            continue;
                        }

                        var clip = cutscene.groups[g].tracks[t].clips[a];
                        var wrapper = new ActionClipWrapper(clip);
                        clipWrappers[id] = wrapper;
                        clipWrappersMap[clip] = wrapper;
                    }
                }
            }

            if (lastTime > 0)
            {
                cutscene.currentTime = lastTime;
            }
        }

        //An integer UID out of list indeces (group, track, action clip)
        int UID(int g, int t, int a)
        {
            var A = g.ToString("D3");
            var B = t.ToString("D3");
            var C = a.ToString("D4");
            return int.Parse(A + B + C);
        }

        //Play button pressed or otherwise started
        public void Play(Cutscene.WrapMode wrapMode = Cutscene.WrapMode.Loop, System.Action callback = null)
        {
            titleContent = new GUIContent("SLATE", Styles.cutsceneIconClose);

            if (Application.isPlaying)
            {
                var temp = cutscene.currentTime == length ? 0 : cutscene.currentTime;
                cutscene.Play(0, length, cutscene.defaultWrapMode, callback, Cutscene.PlayingDirection.Forwards);
                cutscene.currentTime = temp;
                return;
            }

            editorPlaybackWrapMode = wrapMode;
            editorPlaybackState = EditorPlaybackState.PlayingForwards;
            editorPreviousTime = Time.realtimeSinceStartup;
            lastStartPlayTime = cutscene.currentTime;
            OnStopInEditor = callback != null ? callback : OnStopInEditor;
        }

        //Play reverse button pressed
        public void PlayReverse()
        {
            titleContent = new GUIContent("SLATE", Styles.cutsceneIconClose);

            if (Application.isPlaying)
            {
                var temp = cutscene.currentTime == 0 ? length : cutscene.currentTime;
                cutscene.Play(0, length, cutscene.defaultWrapMode, null, Cutscene.PlayingDirection.Backwards);
                cutscene.currentTime = temp;
                return;
            }

            editorPlaybackState = EditorPlaybackState.PlayingBackwards;
            editorPreviousTime = Time.realtimeSinceStartup;
            if (cutscene.currentTime == 0)
            {
                cutscene.currentTime = length;
                lastStartPlayTime = 0;
            }
            else
            {
                lastStartPlayTime = cutscene.currentTime;
            }
        }

        //Pause button pressed
        public void Pause()
        {
            titleContent = new GUIContent("SLATE", Styles.cutsceneIconOpen);

            if (Application.isPlaying)
            {
                if (cutscene.isActive)
                {
                    cutscene.Pause();
                    return;
                }
            }

            editorPlaybackState = EditorPlaybackState.Stoped;
            if (OnStopInEditor != null)
            {
                OnStopInEditor();
                OnStopInEditor = null;
            }
        }

        //Stop button pressed or otherwise reset the scrubbing/previewing
        public void Stop(bool forceRewind)
        {
            titleContent = new GUIContent("SLATE", Styles.cutsceneIconOpen);

            if (Application.isPlaying)
            {
                if (cutscene.isActive)
                {
                    cutscene.Stop();
                    return;
                }
            }

            if (OnStopInEditor != null)
            {
                OnStopInEditor();
                OnStopInEditor = null;
            }

            //Super important to Sample instead of setting time here, so that we rewind correct if need be. 0 rewinds.
            cutscene.Sample(editorPlaybackState != EditorPlaybackState.Stoped && !forceRewind ? lastStartPlayTime : 0);
            editorPlaybackState = EditorPlaybackState.Stoped;
            willRepaint = true;
        }

        ///Steps time forward to the next key time
        void StepForward()
        {
            var keyable = CutsceneUtility.selectedObject as IKeyable;
            if (keyable != null)
            {
                var time = keyable.animationData.GetKeyNext(keyable.RootTimeToLocalTimeUnclamped());
                cutscene.currentTime = time + keyable.startTime;
                return;
            }

            if (cutscene.currentTime == cutscene.length)
            {
                cutscene.currentTime = 0;
                return;
            }

            cutscene.currentTime = cutscene.GetPointerTimes().FirstOrDefault(t => t > cutscene.currentTime + 0.01f);
        }

        ///Steps time backwards to the previous key time
        void StepBackward()
        {
            var keyable = CutsceneUtility.selectedObject as IKeyable;
            if (keyable != null)
            {
                var time = keyable.animationData.GetKeyPrevious(keyable.RootTimeToLocalTimeUnclamped());
                cutscene.currentTime = time + keyable.startTime;
                return;
            }

            if (cutscene.currentTime == 0)
            {
                cutscene.currentTime = cutscene.length;
                return;
            }

            cutscene.currentTime = cutscene.GetPointerTimes().LastOrDefault(t => t < cutscene.currentTime - 0.01f);
        }

        //Sample the cutscene
        void OnEditorUpdate()
        {
            //if cutscene playmode active, it will sample and update itself.
            if (cutscene == null || cutscene.isActive)
            {
                return;
            }

            if (EditorApplication.isCompiling)
            {
                Stop(true);
                return;
            }

            var delta = (Time.realtimeSinceStartup - editorPreviousTime) * Time.timeScale;
            delta *= cutscene.playbackSpeed;
            editorPreviousTime = Time.realtimeSinceStartup;

            //Sample at it's current time.
            cutscene.Sample();

            //Nothing.
            if (editorPlaybackState == EditorPlaybackState.Stoped)
            {
                return;
            }

            //Playback.
            if (cutscene.currentTime >= length && editorPlaybackState == EditorPlaybackState.PlayingForwards)
            {
                if (editorPlaybackWrapMode == Cutscene.WrapMode.Once)
                {
                    Stop(true);
                    return;
                }

                if (editorPlaybackWrapMode == Cutscene.WrapMode.Loop)
                {
                    cutscene.Sample(0);
                    cutscene.Sample(delta);
                    return;
                }
            }

            if (cutscene.currentTime <= 0 && editorPlaybackState == EditorPlaybackState.PlayingBackwards)
            {
                Stop(true);
                return;
            }

            cutscene.currentTime += editorPlaybackState == EditorPlaybackState.PlayingForwards ? delta : -delta;
        }


        //...
        void OnSceneGUI(SceneView sceneView)
        {
            if (cutscene == null)
            {
                return;
            }

            //Shortcuts for scene gui only
            var e = Event.current;
            if (e.type == EventType.KeyDown)
            {
                if (e.keyCode == KeyCode.Space && !e.shift)
                {
                    GUIUtility.keyboardControl = 0;
                    if (editorPlaybackState != EditorPlaybackState.Stoped)
                    {
                        Stop(false);
                    }
                    else
                    {
                        Play();
                    }

                    e.Use();
                }

                if (e.keyCode == KeyCode.Comma)
                {
                    GUIUtility.keyboardControl = 0;
                    StepBackward();
                    e.Use();
                }

                if (e.keyCode == KeyCode.Period)
                {
                    GUIUtility.keyboardControl = 0;
                    StepForward();
                    e.Use();
                }
            }


            ///Forward OnSceneGUI
            if (cutscene.directables != null)
            {
                for (var i = 0; i < cutscene.directables.Count; i++)
                {
                    var directable = cutscene.directables[i];
                    directable.SceneGUI(CutsceneUtility.selectedObject == directable);
                }
            }
            ///

            ///No need to show tools of cutscene object, plus handles are shown per clip when required
            Tools.hidden = (Selection.activeObject == cutscene || Selection.activeGameObject == cutscene.gameObject) &&
                           CutsceneUtility.selectedObject != null;

            ///Cutscene Root info and gizmos
            Handles.color = Prefs.gizmosColor;
            Handles.Label(cutscene.transform.position + new Vector3(0, 0.4f, 0), "Cutscene Root");
            Handles.DrawLine(cutscene.transform.position + cutscene.transform.forward,
                cutscene.transform.position + cutscene.transform.forward * -1);
            Handles.DrawLine(cutscene.transform.position + cutscene.transform.right,
                cutscene.transform.position + cutscene.transform.right * -1);
            Handles.color = Color.white;

            Handles.BeginGUI();

            if (cutscene.currentTime > 0 && (cutscene.currentTime < cutscene.length || !Application.isPlaying))
            {
                ///view frame. Red = scrubbing, yellow = active in playmode
                var cam = sceneView.camera;
                var lineWidth = 3f;
                var top = new Rect(0, 0, cam.pixelWidth, lineWidth);
                var bottom = new Rect(0, cam.pixelHeight - lineWidth - 10, cam.pixelWidth, lineWidth + 10);
                var left = new Rect(0, 0, lineWidth, cam.pixelHeight);
                var right = new Rect(cam.pixelWidth - lineWidth, 0, lineWidth, cam.pixelHeight);
                var texture = whiteTexture;
                GUI.color = cutscene.isActive ? Color.green : Color.red;
                GUI.DrawTexture(top, texture);
                GUI.DrawTexture(bottom, texture);
                GUI.DrawTexture(left, texture);
                GUI.DrawTexture(right, texture);
                //

                //Info
                GUI.color = Color.black.WithAlpha(0.7f);
                if (cutscene.isActive)
                {
                    GUI.Label(bottom, string.Format(" Active '{0}'", cutscene.name), GUIStyle.none);
                }
                else
                {
                    GUI.Label(bottom,
                        string.Format(
                            " Previewing '{0}'. Non animatable changes made to actor components will be reverted.",
                            cutscene.name), GUIStyle.none);
                }
            }

            GUI.color = Color.white;
            Handles.EndGUI();
        }

        //...
        void Update()
        {
            if (willRepaint)
            {
                willRepaint = false;
                Repaint();
            }
        }

        //...
        void OnGUI()
        {
            GUI.skin.label.richText = true;
            GUI.skin.label.alignment = TextAnchor.UpperLeft;
            EditorStyles.label.richText = true;
            EditorStyles.textField.wordWrap = true;
            EditorStyles.foldout.richText = true;
            var e = Event.current;
            mousePosition = e.mousePosition;
            current = this;

            if (cutscene == null || isHelpButtonPressed)
            {
                ShowWelcome();
                return;
            }

            //avoid edit when compiling
            if (EditorApplication.isCompiling)
            {
                Stop(true);
                ShowNotification(new GUIContent("Compiling\n...Please wait..."));
                return;
            }

            //handle undo/redo shortcuts
            if (e.type == EventType.ValidateCommand && e.commandName == "UndoRedoPerformed")
            {
                GUIUtility.hotControl = 0;
                GUIUtility.keyboardControl = 0;
                multiSelection = null;
                cutscene.Validate();
                InitClipWrappers();
                e.Use();
                return;
            }

            //prefab editing is not allowed
            if (isCutsceneAsset)
            {
                ShowNotification(new GUIContent(
                    "Editing Prefab Assets is not allowed\nPlease add an instance in the scene, or open the prefab for editing"));
                return;
                // if ( e.isMouse || e.isKey ) {
                //     e.Use();
                // }
            }

            //remove notifications quickly
            if (e.type == EventType.MouseDown)
            {
                RemoveNotification();
            }

            //button 2 seems buggy
            if (e.button == 2 && e.type == EventType.MouseDown)
            {
                isMouseButton2Down = true;
            }

            if (e.button == 2 && e.rawType == EventType.MouseUp)
            {
                isMouseButton2Down = false;
            }

            //Record Undo and dirty? This is an overal fallback. Certain actions register undo as well.
            var doRecordUndo = e.rawType == EventType.MouseDown && (e.button == 0 || e.button == 1);
            doRecordUndo |= e.type == EventType.DragPerform;
            if (doRecordUndo)
            {
                Undo.RegisterFullObjectHierarchyUndo(cutscene.groupsRoot.gameObject, "Cutscene Change");
                Undo.RecordObject(cutscene, "Cutscene Change");
                willDirty = true;
            }

            //reorder clips lists for better UI. This is strictly a UI thing.
            if (interactingClip == null && e.type == EventType.Layout)
            {
                foreach (var group in cutscene.groups)
                {
                    foreach (var track in group.tracks)
                    {
                        track.clips = track.clips.OrderBy(a => a.startTime).ToList();
                    }
                }
            }

            //make the layout rects
            topLeftRect = new Rect(0, TOOLBAR_HEIGHT, LEFT_MARGIN, TOP_MARGIN);
            topMiddleRect = new Rect(LEFT_MARGIN, TOOLBAR_HEIGHT, screenWidth - LEFT_MARGIN - RIGHT_MARGIN, TOP_MARGIN);
            leftRect = new Rect(0, TOOLBAR_HEIGHT + TOP_MARGIN, LEFT_MARGIN,
                screenHeight - TOOLBAR_HEIGHT - TOP_MARGIN + scrollPos.y);
            centerRect = new Rect(LEFT_MARGIN, TOP_MARGIN + TOOLBAR_HEIGHT, screenWidth - LEFT_MARGIN - RIGHT_MARGIN,
                screenHeight - TOOLBAR_HEIGHT - TOP_MARGIN + scrollPos.y);

            //...
            DoKeyboardShortcuts();
            ShowPlaybackControls(topLeftRect);
            ShowTimeInfo(topMiddleRect);
            ShowToolbar();
            DoScrubControls();
            DoZoomAndPan();


            //Dirty and Resample flags?
            if (e.rawType == EventType.MouseUp && e.button == 0)
            {
                willDirty = true;
                willResample = true;
            }


            //Timelines
            var scrollRect1 = Rect.MinMaxRect(0, centerRect.yMin, screenWidth, screenHeight - 5);
            var scrollRect2 = Rect.MinMaxRect(0, centerRect.yMin, screenWidth, totalHeight + 150);
            scrollPos = GUI.BeginScrollView(scrollRect1, scrollPos, scrollRect2);
            ShowGroupsAndTracksList(leftRect);
            ShowTimeLines(centerRect);
            GUI.EndScrollView();
            ///---

            DrawGuides();
            AcceptDrops();


            ///Final stuff...

            //clean selection and hotcontrols
            if (e.type == EventType.MouseDown && e.button == 0 && GUIUtility.hotControl == 0)
            {
                if (centerRect.Contains(mousePosition))
                {
                    CutsceneUtility.selectedObject = null;
                    multiSelection = null;
                }

                GUIUtility.keyboardControl = 0;
                showDragDropInfo = false;
            }

            //just some info for the user to drag/drop gameobject in editor
            if (showDragDropInfo && cutscene.groups.Find(g => g.GetType() == typeof(ActorGroup)) == null)
            {
                var label = "Drag & Drop GameObjects or Prefabs in this window to create Actor Groups";
                var size = new GUIStyle("label").CalcSize(new GUIContent(label));
                var notificationRect = new Rect(0, 0, size.x, size.y);
                notificationRect.center =
                    new Vector2((screenWidth / 2) + (LEFT_MARGIN / 2), (screenHeight / 2) + TOP_MARGIN);
                GUI.Label(notificationRect, label);
            }

            //repaint?
            if (e.type == EventType.MouseDrag || e.type == EventType.MouseUp || GUI.changed)
            {
                willRepaint = true;
            }

            //dirty?
            if (willDirty)
            {
                willDirty = false;
                EditorUtility.SetDirty(cutscene);
                foreach (var o in cutscene.GetComponentsInChildren(typeof(IDirectable), true).Cast<Object>())
                {
                    EditorUtility.SetDirty(o);
                }
            }

            //resample?
            if (willResample)
            {
                willResample = false;
                //delaycall so that other gui controls are finalized before resample.
                EditorApplication.delayCall += () =>
                {
                    if (cutscene != null) cutscene.ReSample();
                };
            }

            //hack to show modal popup windows
            if (onDoPopup != null)
            {
                var temp = onDoPopup;
                onDoPopup = null;
                QuickPopup.Show(temp);
            }

            //if a prefab darken whole UI
            if (isCutsceneAsset)
            {
                GUI.color = Color.black.WithAlpha(0.5f);
                GUI.DrawTexture(new Rect(0, 0, screenWidth, screenHeight), whiteTexture);
                GUI.color = Color.white;
            }

            //cheap ver/hor seperators
            Handles.color = Color.black;
            Handles.DrawLine(new Vector2(0, centerRect.y + 1), new Vector2(centerRect.xMax, centerRect.y + 1));
            Handles.DrawLine(new Vector2(centerRect.x, centerRect.y + 1), new Vector2(centerRect.x, centerRect.yMax));
            Handles.color = Color.white;

            //cleanup
            GUI.color = Color.white;
            GUI.backgroundColor = Color.white;
            GUI.skin = null;
        }

        ///----------------------------------------------------------------------------------------------

        //...		
        void DoKeyboardShortcuts()
        {
            var e = Event.current;
            if (e.type == EventType.KeyDown && GUIUtility.keyboardControl == 0)
            {
                //play
                if (e.keyCode == KeyCode.Space && !e.shift)
                {
                    if (editorPlaybackState != EditorPlaybackState.Stoped)
                    {
                        Stop(false);
                    }
                    else
                    {
                        Play();
                    }

                    e.Use();
                }

                //step forw
                if (e.keyCode == KeyCode.Period)
                {
                    StepForward();
                    e.Use();
                }

                //step back
                if (e.keyCode == KeyCode.Comma)
                {
                    StepBackward();
                    e.Use();
                }

                //key at scrubber
                if (e.keyCode == KeyCode.K)
                {
                    var keyable = CutsceneUtility.selectedObject as IKeyable;
                    if (keyable != null)
                    {
                        keyable.TryAddIdentityKey(keyable.RootTimeToLocalTime());
                    }

                    e.Use();
                }

                //split at scrubber
                if (e.keyCode == KeyCode.S)
                {
                    var clip = CutsceneUtility.selectedObject as ActionClip;
                    if (clip != null)
                    {
                        var wrapper = clipWrappersMap[clip];
                        if (wrapper != null)
                        {
                            wrapper.Split(cutscene.currentTime);
                        }
                    }

                    e.Use();
                }

                //strech fit
                if (e.keyCode == KeyCode.F)
                {
                    var clip = CutsceneUtility.selectedObject as ActionClip;
                    if (clip != null)
                    {
                        var wrapper = clipWrappersMap[clip];
                        if (wrapper != null)
                        {
                            wrapper.StretchFit();
                        }
                    }

                    e.Use();
                }

                //strech fit
                if (e.keyCode == KeyCode.C)
                {
                    var clip = CutsceneUtility.selectedObject as ActionClip;
                    if (clip != null)
                    {
                        var wrapper = clipWrappersMap[clip];
                        if (wrapper != null)
                        {
                            wrapper.CleanKeysOffRange();
                        }
                    }

                    e.Use();
                }


                if (e.keyCode == KeyCode.Delete || e.keyCode == KeyCode.Backspace)
                {
                    if (multiSelection != null)
                    {
                        SafeDoAction(() =>
                        {
                            foreach (var act in multiSelection.Select(b => b.action).ToArray())
                            {
                                (act.parent as CutsceneTrack).DeleteAction(act);
                            }

                            InitClipWrappers();
                            multiSelection = null;
                        });
                        e.Use();
                    }
                    else
                    {
                        var clip = CutsceneUtility.selectedObject as ActionClip;
                        if (clip != null)
                        {
                            SafeDoAction(() =>
                            {
                                (clip.parent as CutsceneTrack).DeleteAction(clip);
                                InitClipWrappers();
                            });
                            e.Use();
                        }
                    }
                }
            }
        }

        //...
        void DrawGuides()
        {
            //draw a vertical line at 0 time
            DrawGuideLine(0, isProSkin ? Color.white : Color.black);

            //draw a vertical line at length time
            DrawGuideLine(length, isProSkin ? Color.white : Color.black);

            //draw a vertical line at current time
            if (cutscene.currentTime > 0)
            {
                DrawGuideLine(cutscene.currentTime, scruberColor);
            }

            //draw a vertical line at dragging clip start/end time
            if (interactingClip != null)
            {
                if (interactingClip.isDragging || interactingClip.isScalingStart)
                {
                    DrawGuideLine(interactingClip.action.startTime, Color.white.WithAlpha(0.05f));
                }

                if (interactingClip.isDragging || interactingClip.isScalingEnd)
                {
                    DrawGuideLine(interactingClip.action.endTime, Color.white.WithAlpha(0.05f));
                }
            }

            //draw a vertical line at dragging section
            if (draggedSection != null)
            {
                DrawGuideLine(draggedSection.time, draggedSection.color);
            }

            if (cutscene.isActive)
            {
                if (cutscene.playTimeMin > 0)
                {
                    DrawGuideLine(cutscene.playTimeMin, Color.red);
                }

                if (cutscene.playTimeMax < length)
                {
                    DrawGuideLine(cutscene.playTimeMax, Color.red);
                }
            }

            //draw other "subscribed" guidelines
            for (var i = 0; i < pendingGuides.Count; i++)
            {
                DrawGuideLine(pendingGuides[i].time, pendingGuides[i].color);
            }

            pendingGuides.Clear();
        }


        //...
        void ShowWelcome()
        {
            if (cutscene == null)
            {
                isHelpButtonPressed = false;
            }

            var label = string.Format("<size=30><b>{0}</b></size>",
                isHelpButtonPressed ? "Important and Helpful Links" : "Welcome to SLATE!");
            var size = new GUIStyle("label").CalcSize(new GUIContent(label));
            var titleRect = new Rect(0, 0, size.x, size.y);
            titleRect.center = new Vector2(screenWidth / 2, (screenHeight / 2) - size.y);
            GUI.Label(titleRect, label);

            var iconRect = new Rect(0, 0, 128, 128);
            iconRect.center = new Vector2(screenWidth / 2, titleRect.yMin - 60);
            GUI.DrawTexture(iconRect, Styles.slateIcon);

            var buttonRect = new Rect(0, 0, size.x, size.y);
            var next = 0;

            if (!isHelpButtonPressed)
            {
                GUI.backgroundColor = new Color(0.8f, 0.8f, 1, 1f);
                buttonRect.center = new Vector2(screenWidth / 2, (screenHeight / 2) + (size.y + 2) * next);
                next++;
                if (GUI.Button(buttonRect, "Create New Cutscene"))
                {
                    InitializeAll(Commands.CreateCutscene());
                }

                GUI.backgroundColor = Color.white;
            }

            buttonRect.center = new Vector2(screenWidth / 2, (screenHeight / 2) + (size.y + 2) * next);
            next++;
            if (GUI.Button(buttonRect, "Visit The Website"))
            {
                Help.BrowseURL("http://slate.paradoxnotion.com");
            }

            buttonRect.center = new Vector2(screenWidth / 2, (screenHeight / 2) + (size.y + 2) * next);
            next++;
            if (GUI.Button(buttonRect, "Read The Documentation"))
            {
                Help.BrowseURL("http://slate.paradoxnotion.com/documentation");
            }

            buttonRect.center = new Vector2(screenWidth / 2, (screenHeight / 2) + (size.y + 2) * next);
            next++;
            if (GUI.Button(buttonRect, "Download Extensions"))
            {
                Help.BrowseURL("http://slate.paradoxnotion.com/downloads");
            }

            buttonRect.center = new Vector2(screenWidth / 2, (screenHeight / 2) + (size.y + 2) * next);
            next++;
            if (GUI.Button(buttonRect, "Join The Forums"))
            {
                Help.BrowseURL("http://slate.paradoxnotion.com/forums-page");
            }

            if (!isHelpButtonPressed)
            {
                buttonRect.center = new Vector2(screenWidth / 2, (screenHeight / 2) + (size.y + 2) * next);
                next++;
                if (GUI.Button(buttonRect, "Leave a Review"))
                {
                    Help.BrowseURL("http://u3d.as/ozt");
                }
            }

            if (isHelpButtonPressed && cutscene != null)
            {
                var backRect = new Rect(0, 0, 400, 20);
                backRect.center = new Vector2(screenWidth / 2, 20);
                GUI.backgroundColor = new Color(0.8f, 0.8f, 1, 1f);
                if (GUI.Button(backRect, "Close Help Panel"))
                {
                    isHelpButtonPressed = false;
                }

                GUI.backgroundColor = Color.white;
            }
        }

        //...
        void AcceptDrops()
        {
            if (cutscene.currentTime > 0)
            {
                return;
            }

            var e = Event.current;
            if (e.type == EventType.DragUpdated)
            {
                DragAndDrop.visualMode = DragAndDropVisualMode.Link;
            }

            if (e.type == EventType.DragPerform)
            {
                for (int i = 0; i < DragAndDrop.objectReferences.Length; i++)
                {
                    var o = DragAndDrop.objectReferences[i];
                    if (o is GameObject)
                    {
                        var go = (GameObject) o;

                        if (go.GetComponent<DirectorCamera>() != null)
                        {
                            ShowNotification(
                                new GUIContent("The 'DIRECTOR' group is already used for the 'DirectorCamera' object"));
                            continue;
                        }

                        if (cutscene.GetAffectedActors().Contains(go))
                        {
                            ShowNotification(new GUIContent(string.Format("GameObject '{0}' is already in the cutscene",
                                o.name)));
                            continue;
                        }

                        DragAndDrop.AcceptDrag();
                        var newGroup = cutscene.AddGroup<ActorGroup>(go);
                        newGroup.AddTrack<ActorActionTrack>("Action Track");
                        CutsceneUtility.selectedObject = newGroup;
                    }
                }
            }
        }

        //The toolbar...
        void ShowToolbar()
        {
            if (!isProSkin)
            {
                GUI.contentColor = Color.black.WithAlpha(0.7f);
            }

            GUI.enabled = cutscene.currentTime <= 0;

            var e = Event.current;

            GUI.backgroundColor = Color.white;
            GUI.color = Color.white;
            GUILayout.BeginHorizontal(EditorStyles.toolbar);

            if (GUILayout.Button(string.Format("[{0}]", cutscene.name), EditorStyles.toolbarDropDown,
                GUILayout.Width(100)))
            {
                GenericMenu.MenuFunction2 SelectCutscene = (object cut) =>
                {
                    Selection.activeObject = (Cutscene) cut;
                    EditorGUIUtility.PingObject((Cutscene) cut);
                };

                var cutscenes = FindObjectsOfType<Cutscene>();
                var menu = new GenericMenu();
                foreach (Cutscene cut in cutscenes)
                {
                    menu.AddItem(new GUIContent(string.Format("[{0}]", cut.name)), cut == cutscene, SelectCutscene,
                        cut);
                }

                menu.ShowAsContext();
            }

            if (GUILayout.Button("Select", EditorStyles.toolbarButton, GUILayout.Width(60)))
            {
                Selection.activeObject = cutscene;
                EditorGUIUtility.PingObject(cutscene);
            }

            if (GUILayout.Button("Render", EditorStyles.toolbarButton, GUILayout.Width(60)))
            {
                RenderWindow.Open();
            }

            if (GUILayout.Button("Snap: " + Prefs.snapInterval.ToString(), EditorStyles.toolbarDropDown,
                GUILayout.Width(90)))
            {
                var menu = new GenericMenu();
                menu.AddItem(new GUIContent("0.001"), false, () =>
                {
                    Prefs.timeStepMode = Prefs.TimeStepMode.Seconds;
                    Prefs.frameRate = 1000;
                });
                menu.AddItem(new GUIContent("0.01"), false, () =>
                {
                    Prefs.timeStepMode = Prefs.TimeStepMode.Seconds;
                    Prefs.frameRate = 100;
                });
                menu.AddItem(new GUIContent("0.1"), false, () =>
                {
                    Prefs.timeStepMode = Prefs.TimeStepMode.Seconds;
                    Prefs.frameRate = 10;
                });
                menu.AddItem(new GUIContent("30 FPS"), false, () =>
                {
                    Prefs.timeStepMode = Prefs.TimeStepMode.Frames;
                    Prefs.frameRate = 30;
                });
                menu.AddItem(new GUIContent("60 FPS"), false, () =>
                {
                    Prefs.timeStepMode = Prefs.TimeStepMode.Frames;
                    Prefs.frameRate = 60;
                });
                menu.ShowAsContext();
            }

            GUILayout.Space(10);

            Prefs.magnetSnapping = GUILayout.Toggle(Prefs.magnetSnapping, new GUIContent(Styles.magnetIcon),
                EditorStyles.toolbarButton);


            GUILayout.FlexibleSpace();
            if (!Prefs.autoKey)
            {
                var wasEnabled = GUI.enabled;
                GUI.enabled = true;
                var changedParams = CutsceneUtility.changedParameterCallbacks;
                var hasChangedParams = changedParams != null && changedParams.Count > 0;
                GUI.color = hasChangedParams ? Color.white : Color.clear;
                GUILayout.BeginHorizontal(EditorStyles.toolbarButton);
                if (hasChangedParams)
                {
                    GUI.backgroundColor = Color.clear;
                    GUI.color = Color.green;
                    var b1 = GUILayout.Button(Styles.keyIcon, EditorStyles.toolbarButton);
                    GUI.color = Color.white;
                    var b2 = GUILayout.Button(string.Format("Key ({0}) Changed Parameters", changedParams.Count),
                        EditorStyles.toolbarButton);
                    GUI.backgroundColor = Color.white;
                    if (b1 || b2)
                    {
                        foreach (var pair in changedParams)
                        {
                            pair.Value.Commit();
                        }
                    }
                }

                GUI.color = Color.white;
                GUILayout.EndHorizontal();
                GUI.enabled = wasEnabled;
            }

            GUILayout.FlexibleSpace();


            GUI.color = Color.white.WithAlpha(0.3f);
            GUILayout.Label(string.Format("<size=9>SLATE Version {0}</size>",
                Cutscene.VERSION_NUMBER.ToString("0.00")));
            GUI.color = Color.white;

            if (GUILayout.Button(Slate.Styles.gearIcon, EditorStyles.toolbarButton, GUILayout.Width(26)))
            {
                PreferencesWindow.Show(new Rect(screenWidth - 5 - 400, TOOLBAR_HEIGHT + 5, 400,
                    screenHeight - TOOLBAR_HEIGHT - 50));
            }

            isHelpButtonPressed = GUILayout.Toggle(isHelpButtonPressed, "Help", EditorStyles.toolbarButton);

            GUILayout.EndHorizontal();
            GUI.backgroundColor = Color.white;

            GUI.enabled = true;
            GUI.contentColor = Color.white;
        }

        //Scrubing....
        void DoScrubControls()
        {
            if (cutscene.isActive)
            {
                //no scrubbing if playing in runtime
                return;
            }

            ///
            var e = Event.current;
            if (e.type == EventType.MouseDown && topMiddleRect.Contains(mousePosition))
            {
                var carretPos = TimeToPos(length) + leftRect.width;
                var isEndCarret = Mathf.Abs(mousePosition.x - carretPos) < 10 || e.control;
                if (isEndCarret)
                {
                    CacheMagnetSnapTimes();
                }

                if (e.button == 0)
                {
                    isMovingEndCarret = isEndCarret;
                    isMovingScrubCarret = !isMovingEndCarret;
                    Pause();
                }

                if (e.button == 1 && isEndCarret && cutscene.directables != null)
                {
                    var menu = new GenericMenu();
                    menu.AddItem(new GUIContent("Set To Last Clip Time"), false, () =>
                    {
                        var lastClip = cutscene.directables.Where(d => d is ActionClip).OrderBy(d => d.endTime)
                            .LastOrDefault();
                        if (lastClip != null)
                        {
                            length = lastClip.endTime;
                        }
                    });
                    menu.ShowAsContext();
                }

                e.Use();
            }

            if (e.button == 0 && e.rawType == EventType.MouseUp)
            {
                isMovingScrubCarret = false;
                isMovingEndCarret = false;
            }

            var pointerTime = PosToTime(mousePosition.x);
            if (isMovingScrubCarret)
            {
                cutscene.currentTime = SnapTime(pointerTime);
                cutscene.currentTime = Mathf.Clamp(cutscene.currentTime, Mathf.Max(viewTimeMin, 0) + float.Epsilon,
                    viewTimeMax - float.Epsilon);
            }

            if (isMovingEndCarret)
            {
                length = SnapTime(pointerTime);
                var magnetSnap = MagnetSnapTime(length, magnetSnapTimesCache);
                length = magnetSnap != null ? magnetSnap.Value : length;
                length = Mathf.Clamp(length, viewTimeMin + float.Epsilon, viewTimeMax - float.Epsilon);
            }
        }

        //...
        void DoZoomAndPan()
        {
            if (!centerRect.Contains(mousePosition))
            {
                return;
            }

            var e = Event.current;
            //Zoom or scroll down/up if prefs is set to scrollwheel
            if ((e.type == EventType.ScrollWheel && Prefs.scrollWheelZooms) || (e.alt && !e.shift && e.button == 1))
            {
                this.AddCursorRect(centerRect, MouseCursor.Zoom);
                if (e.type == EventType.MouseDrag || e.type == EventType.MouseDown || e.type == EventType.MouseUp ||
                    e.type == EventType.ScrollWheel)
                {
                    var pointerTimeA = PosToTime(mousePosition.x);
                    var delta = e.alt ? -e.delta.x * 0.1f : e.delta.y;
                    var t = (Mathf.Abs(delta * 25) / centerRect.width) * viewTime;
                    viewTimeMin += delta > 0 ? -t : t;
                    viewTimeMax += delta > 0 ? t : -t;
                    var pointerTimeB = PosToTime(mousePosition.x + e.delta.x);
                    var diff = pointerTimeA - pointerTimeB;
                    viewTimeMin += diff;
                    viewTimeMax += diff;
                    e.Use();
                }
            }

            //pan left/right, up/down
            if (isMouseButton2Down || (e.alt && !e.shift && e.button == 0))
            {
                this.AddCursorRect(centerRect, MouseCursor.Pan);
                if (e.type == EventType.MouseDrag || e.type == EventType.MouseDown || e.type == EventType.MouseUp)
                {
                    var t = (Mathf.Abs(e.delta.x) / centerRect.width) * viewTime;
                    viewTimeMin += e.delta.x > 0 ? -t : t;
                    viewTimeMax += e.delta.x > 0 ? -t : t;
                    scrollPos.y -= e.delta.y;
                    e.Use();
                }
            }
        }

        //top left controls
        void ShowPlaybackControls(Rect topLeftRect)
        {
            var autoKeyRect = new Rect(topLeftRect.xMin + 10, topLeftRect.yMin + 4, 32, 32);
            AddCursorRect(autoKeyRect, MouseCursor.Link);
            GUI.backgroundColor = Prefs.autoKey ? Color.black.WithAlpha(0.5f) : Color.grey.WithAlpha(0.5f);
            GUI.Box(autoKeyRect, string.Empty, Styles.clipBoxStyle);
            GUI.color = Prefs.autoKey ? new Color(1, 0.4f, 0.4f) : Color.white;
            GUI.backgroundColor = Color.clear;
            if (GUI.Button(autoKeyRect, Styles.keyIcon, (GUIStyle) "box"))
            {
                Prefs.autoKey = !Prefs.autoKey;
                ShowNotification(new GUIContent(string.Format("AutoKey {0}", Prefs.autoKey ? "Enabled" : "Disabled"),
                    Styles.keyIcon));
            }

            var autoKeyLabelRect = autoKeyRect;
            autoKeyLabelRect.yMin += 16;
            GUI.backgroundColor = Color.white;
            GUI.Label(autoKeyLabelRect, "<color=#AAAAAA>Auto</color>", Styles.centerLabel);
            GUI.color = Color.white;


            if (!isProSkin)
            {
                GUI.contentColor = Color.black.WithAlpha(0.7f);
            }

            //Cutscene shows the gui
            GUILayout.BeginArea(topLeftRect);

            GUILayout.BeginVertical();
            GUILayout.FlexibleSpace();

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            Rect lastRect;
            if (GUILayout.Button(Styles.stepReverseIcon, (GUIStyle) "box", GUILayout.Width(20), GUILayout.Height(20)))
            {
                StepBackward();
                Event.current.Use();
            }

            lastRect = GUILayoutUtility.GetLastRect();
            if (lastRect.Contains(Event.current.mousePosition))
            {
                AddCursorRect(lastRect, MouseCursor.Link);
            }


            var isStoped = Application.isPlaying
                ? (cutscene.isPaused || !cutscene.isActive)
                : editorPlaybackState == EditorPlaybackState.Stoped;
            if (isStoped)
            {
                if (GUILayout.Button(Styles.playReverseIcon, (GUIStyle) "box", GUILayout.Width(20),
                    GUILayout.Height(20)))
                {
                    PlayReverse();
                    Event.current.Use();
                }

                lastRect = GUILayoutUtility.GetLastRect();
                if (lastRect.Contains(Event.current.mousePosition))
                {
                    AddCursorRect(lastRect, MouseCursor.Link);
                }

                if (GUILayout.Button(Styles.playIcon, (GUIStyle) "box", GUILayout.Width(20), GUILayout.Height(20)))
                {
                    Play();
                    Event.current.Use();
                }

                lastRect = GUILayoutUtility.GetLastRect();
                if (lastRect.Contains(Event.current.mousePosition))
                {
                    AddCursorRect(lastRect, MouseCursor.Link);
                }
            }
            else
            {
                if (GUILayout.Button(Styles.pauseIcon, (GUIStyle) "box", GUILayout.Width(44), GUILayout.Height(20)))
                {
                    Pause();
                    Event.current.Use();
                }

                lastRect = GUILayoutUtility.GetLastRect();
                if (lastRect.Contains(Event.current.mousePosition))
                {
                    AddCursorRect(lastRect, MouseCursor.Link);
                }
            }


            if (GUILayout.Button(Styles.stopIcon, (GUIStyle) "box", GUILayout.Width(20), GUILayout.Height(20)))
            {
                Stop(false);
                Event.current.Use();
            }

            lastRect = GUILayoutUtility.GetLastRect();
            if (lastRect.Contains(Event.current.mousePosition))
            {
                AddCursorRect(lastRect, MouseCursor.Link);
            }

            if (GUILayout.Button(Styles.stepIcon, (GUIStyle) "box", GUILayout.Width(20), GUILayout.Height(20)))
            {
                StepForward();
                Event.current.Use();
            }

            lastRect = GUILayoutUtility.GetLastRect();
            if (lastRect.Contains(Event.current.mousePosition))
            {
                AddCursorRect(lastRect, MouseCursor.Link);
            }

            GUI.backgroundColor = Color.white;

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.FlexibleSpace();
            GUILayout.EndVertical();

            GUILayout.EndArea();

            GUI.contentColor = Color.white;
        }


        //top mid - viewTime selection and time info
        void ShowTimeInfo(Rect topMiddleRect)
        {
            GUI.color = Color.white.WithAlpha(0.2f);
            GUI.Box(topMiddleRect, string.Empty, EditorStyles.toolbarButton);
            GUI.color = Color.black.WithAlpha(0.2f);
            GUI.Box(topMiddleRect, string.Empty, Styles.timeBoxStyle);
            GUI.color = Color.white;

            timeInfoInterval = 1000000f;
            timeInfoHighMod = timeInfoInterval;
            var lowMod = 0.01f;
            var modulos = new float[]
                {0.1f, 0.5f, 1, 5, 10, 50, 100, 500, 1000, 5000, 10000, 50000, 100000, 250000, 500000}; //... O.o
            for (var i = 0; i < modulos.Length; i++)
            {
                var count = viewTime / modulos[i];
                if (centerRect.width / count > 50)
                {
                    //50 is approx width of label
                    timeInfoInterval = modulos[i];
                    lowMod = i > 0 ? modulos[i - 1] : lowMod;
                    timeInfoHighMod = i < modulos.Length - 1 ? modulos[i + 1] : timeInfoHighMod;
                    break;
                }
            }

            var doFrames = Prefs.timeStepMode == Prefs.TimeStepMode.Frames;
            var timeStep = doFrames ? (1f / Prefs.frameRate) : lowMod;

            timeInfoStart = (float) Mathf.FloorToInt(viewTimeMin / timeInfoInterval) * timeInfoInterval;
            timeInfoEnd = (float) Mathf.CeilToInt(viewTimeMax / timeInfoInterval) * timeInfoInterval;
            timeInfoStart = Mathf.Round(timeInfoStart * 10) / 10;
            timeInfoEnd = Mathf.Round(timeInfoEnd * 10) / 10;

            GUI.BeginGroup(topMiddleRect);
            {
                //the minMax slider
                var _timeMin = viewTimeMin;
                var _timeMax = viewTimeMax;
                var sliderRect = new Rect(5, 0, topMiddleRect.width - 10, 18);
                EditorGUI.MinMaxSlider(sliderRect, ref _timeMin, ref _timeMax, 0, maxTime);
                viewTimeMin = _timeMin;
                viewTimeMax = _timeMax;
                if (sliderRect.Contains(Event.current.mousePosition) && Event.current.clickCount == 2)
                {
                    viewTimeMin = 0;
                    viewTimeMax = length;
                }

                GUI.color = Color.white.WithAlpha(0.1f);
                GUI.DrawTexture(Rect.MinMaxRect(0, TOP_MARGIN - 1, topMiddleRect.xMax, TOP_MARGIN),
                    Styles.whiteTexture);
                GUI.color = Color.white;

                //the step interval
                if (centerRect.width / (viewTime / timeStep) > 6)
                {
                    for (var i = timeInfoStart; i <= timeInfoEnd; i += timeStep)
                    {
                        var posX = TimeToPos(i);
                        var frameRect = Rect.MinMaxRect(posX - 1, TOP_MARGIN - 2, posX + 1, TOP_MARGIN - 1);
                        GUI.color = isProSkin ? Color.white : Color.black;
                        GUI.DrawTexture(frameRect, whiteTexture);
                        GUI.color = Color.white;
                    }
                }

                //the time interval
                for (var i = timeInfoStart; i <= timeInfoEnd; i += timeInfoInterval)
                {
                    var posX = TimeToPos(i);
                    var rounded = Mathf.Round(i * 10) / 10;

                    GUI.color = isProSkin ? Color.white : Color.black;
                    var markRect = Rect.MinMaxRect(posX - 2, TOP_MARGIN - 3, posX + 2, TOP_MARGIN - 1);
                    GUI.DrawTexture(markRect, whiteTexture);
                    GUI.color = Color.white;

                    var text = doFrames ? (rounded * Prefs.frameRate).ToString("0") : rounded.ToString("0.00");
                    var size = GUI.skin.GetStyle("label").CalcSize(new GUIContent(text));
                    var stampRect = new Rect(0, 0, size.x, size.y);
                    stampRect.center = new Vector2(posX, TOP_MARGIN - size.y + 4);
                    GUI.color = rounded % timeInfoHighMod == 0 ? Color.white : Color.white.WithAlpha(0.5f);
                    GUI.Box(stampRect, text, (GUIStyle) "label");
                    GUI.color = Color.white;
                }

                //the number showing current time when scubing
                if (cutscene.currentTime > 0)
                {
                    var label = doFrames
                        ? (cutscene.currentTime * Prefs.frameRate).ToString("0")
                        : cutscene.currentTime.ToString("0.00");
                    var text = "<b><size=17>" + label + "</size></b>";
                    var size = Styles.headerBoxStyle.CalcSize(new GUIContent(text));
                    var posX = TimeToPos(cutscene.currentTime);
                    var stampRect = new Rect(0, 0, size.x, size.y);
                    stampRect.center = new Vector2(posX, TOP_MARGIN - size.y / 2);

                    GUI.backgroundColor = isProSkin ? Color.black.WithAlpha(0.4f) : Color.black.WithAlpha(0.7f);
                    GUI.color = scruberColor;
                    GUI.Box(stampRect, text, Styles.headerBoxStyle);
                }

                //the length position carret texture and pre-exit length indication
                var lengthPos = TimeToPos(length);
                var lengthRect = new Rect(0, 0, 16, 16);
                lengthRect.center = new Vector2(lengthPos, TOP_MARGIN - 2);
                GUI.color = isProSkin ? Color.white : Color.black;
                GUI.DrawTexture(lengthRect, Styles.carretIcon);
                GUI.color = Color.white;
            }
            GUI.EndGroup();
        }


        //left - the groups and tracks info and option per group/track
        void ShowGroupsAndTracksList(Rect leftRect)
        {
            var e = Event.current;
            //allow resize list width
            var scaleRect = new Rect(leftRect.xMax - 4, leftRect.yMin, 4, leftRect.height);
            AddCursorRect(scaleRect, MouseCursor.ResizeHorizontal);
            if (e.type == EventType.MouseDown && e.button == 0 && scaleRect.Contains(e.mousePosition))
            {
                isResizingLeftMargin = true;
                e.Use();
            }

            if (isResizingLeftMargin)
            {
                LEFT_MARGIN = e.mousePosition.x + 2;
            }

            if (e.rawType == EventType.MouseUp)
            {
                isResizingLeftMargin = false;
            }

            GUI.enabled = cutscene.currentTime <= 0;
            //starting height && search.
            var nextYPos = FIRST_GROUP_TOP_MARGIN;
            var wasEnabled = GUI.enabled;
            GUI.enabled = true;
            var collapseAllRect = Rect.MinMaxRect(leftRect.x + 2, leftRect.y + 2, 18, leftRect.y + 16);
            var searchRect = Rect.MinMaxRect(collapseAllRect.x + 18, leftRect.y + 4, leftRect.xMax - 32,
                leftRect.y + 20 - 1);
            var searchCancelRect = Rect.MinMaxRect(searchRect.xMax, searchRect.y, leftRect.xMax - 16, searchRect.yMax);
            var createGroupRect = new Rect(searchCancelRect.xMax, searchCancelRect.yMin + 2, 10, 10);
            var anyExpanded = cutscene.groups.Any(g => !g.isCollapsed);
            AddCursorRect(collapseAllRect, MouseCursor.Link);
            GUI.color = Color.white.WithAlpha(0.5f);
            if (GUI.Button(collapseAllRect, anyExpanded ? "▼" : "►", (GUIStyle) "label"))
            {
                foreach (var group in cutscene.groups)
                {
                    group.isCollapsed = anyExpanded;
                }
            }

            GUI.color = Color.white;
            searchString = EditorGUI.TextField(searchRect, searchString, (GUIStyle) "ToolbarSeachTextField");
            if (GUI.Button(searchCancelRect, string.Empty, (GUIStyle) "ToolbarSeachCancelButton"))
            {
                searchString = string.Empty;
                GUIUtility.keyboardControl = 0;
            }

            if (GUI.Button(createGroupRect, Slate.Styles.plusIcon, GUIStyle.none))
            {
                GenericMenu genericMenu = new GenericMenu();

                genericMenu.AddItem(new GUIContent("Add Actor Group"), false, data =>
                {
                    var newGroup = cutscene.AddGroup<ActorGroup>(null).AddTrack<ActorActionTrack>();


                    CutsceneUtility.selectedObject = newGroup;
                }, null);
                genericMenu.AddItem(new GUIContent("Add Skill Group"), false, data =>
                {
                    var newGroup = cutscene.AddGroup<ST_ParadoxNotionGroup>(null).AddTrack<ST_ParadoxNotionTrack>();
                    CutsceneUtility.selectedObject = newGroup;
                }, null);
                genericMenu.ShowAsContext();
            }

            GUI.enabled = wasEnabled;
            //begin area for left Rect

            GUI.BeginGroup(leftRect);
            ShowListGroups(e, ref nextYPos);
            GUI.EndGroup();
            //store total height required
            totalHeight = nextYPos;

            //Simple button to add empty group for convenience
            var addButtonY = totalHeight + TOP_MARGIN + TOOLBAR_HEIGHT + 20;
            var addRect = Rect.MinMaxRect(leftRect.xMin + 10, addButtonY, leftRect.xMax - 10, addButtonY + 20);
        }

        //...
        void ShowListGroups(Event e, ref float nextYPos)
        {
            //GROUPS
            for (int g = 0; g < cutscene.groups.Count; g++)
            {
                var group = cutscene.groups[g];

                if (IsFilteredOutBySearch(group, searchString))
                {
                    group.isCollapsed = true;
                    continue;
                }

                var groupRect = new Rect(4, nextYPos, leftRect.width - GROUP_RIGHT_MARGIN - 4, GROUP_HEIGHT - 3);
                this.AddCursorRect(groupRect, pickedGroup == null ? MouseCursor.Link : MouseCursor.MoveArrow);
                nextYPos += GROUP_HEIGHT;

                ///highligh?
                var groupSelected = (ReferenceEquals(group, CutsceneUtility.selectedObject) || group == pickedGroup);
                GUI.color = groupSelected ? LIST_SELECTION_COLOR : GROUP_COLOR;
                GUI.Box(groupRect, string.Empty, Styles.headerBoxStyle);
                GUI.color = Color.white;


                //GROUP CONTROLS
                var plusClicked = false;
                GUI.color = isProSkin ? Color.white.WithAlpha(0.5f) : new Color(0.2f, 0.2f, 0.2f);
                var plusRect = new Rect(groupRect.xMax - 14, groupRect.y + 5, 8, 8);
                if (GUI.Button(plusRect, Slate.Styles.plusIcon, GUIStyle.none))
                {
                    plusClicked = true;
                }

                if (!group.isActive)
                {
                    var disableIconRect = new Rect(plusRect.xMin - 20, groupRect.y + 1, 16, 16);
                    if (GUI.Button(disableIconRect, Styles.hiddenIcon, GUIStyle.none))
                    {
                        group.isActive = true;
                    }
                }

                if (group.isLocked)
                {
                    var lockIconRect = new Rect(plusRect.xMin - (group.isActive ? 20 : 36), groupRect.y + 1, 16, 16);
                    if (GUI.Button(lockIconRect, Styles.lockIcon, GUIStyle.none))
                    {
                        group.isLocked = false;
                    }
                }

                GUI.color = isProSkin ? Color.yellow : Color.white;
                GUI.color = group.isActive ? GUI.color : Color.grey;
                var foldRect = new Rect(groupRect.x + 2, groupRect.y + 1, 20, groupRect.height);
                var isVirtual = group.referenceMode == CutsceneGroup.ActorReferenceMode.UseInstanceHideOriginal;
                group.isCollapsed = !EditorGUI.Foldout(foldRect, !group.isCollapsed,
                    string.Format("<b>{0} {1}</b>", group.name, isVirtual ? "(Ref)" : string.Empty));
                GUI.color = Color.white;
                //Actor Object Field
                if (group.actor == null)
                {
                    var oRect = Rect.MinMaxRect(groupRect.xMin + 20, groupRect.yMin + 1, groupRect.xMax - 20,
                        groupRect.yMax - 1);
                    group.actor =
                        (GameObject) UnityEditor.EditorGUI.ObjectField(oRect, group.actor, typeof(GameObject), true);
                }
                ///---

                ///CONTEXT
                if ((e.type == EventType.ContextClick && groupRect.Contains(e.mousePosition)) || plusClicked)
                {
                    var menu = new GenericMenu();
                    foreach (var _info in EditorTools.GetTypeMetaDerivedFrom(typeof(CutsceneTrack)))
                    {
                        var info = _info;
                        if (info.attachableTypes == null || !info.attachableTypes.Contains(group.GetType()))
                        {
                            continue;
                        }

                        var canAdd = !info.isUnique ||
                                     (group.tracks.Find(track => track.GetType() == info.type) == null);
                        var finalPath = string.IsNullOrEmpty(info.category)
                            ? info.name
                            : info.category + "/" + info.name;
                        if (canAdd)
                        {
                            menu.AddItem(new GUIContent("Add Track/" + finalPath), false,
                                () => { group.AddTrack(info.type); });
                        }
                        else
                        {
                            menu.AddDisabledItem(new GUIContent("Add Track/" + finalPath));
                        }
                    }

                    if (group.CanAddTrack(copyTrack))
                    {
                        menu.AddItem(new GUIContent("Paste Track"), false, () => { group.DuplicateTrack(copyTrack); });
                    }
                    else
                    {
                        menu.AddDisabledItem(new GUIContent("Paste Track"));
                    }

                    menu.AddItem(new GUIContent("Disable Group"), !group.isActive,
                        () => { group.isActive = !group.isActive; });
                    menu.AddItem(new GUIContent("Lock Group"), group.isLocked,
                        () => { group.isLocked = !group.isLocked; });

                    if (!(group is DirectorGroup))
                    {
                        menu.AddItem(new GUIContent("Select Actor (Double Click)"), false,
                            () => { Selection.activeObject = group.actor; });
                        menu.AddItem(new GUIContent("Replace Actor"), false, () => { group.actor = null; });
                        menu.AddItem(new GUIContent("Duplicate"), false, () =>
                        {
                            cutscene.DuplicateGroup(group);
                            InitClipWrappers();
                        });
                        menu.AddSeparator("/");
                        menu.AddItem(new GUIContent("Delete Group"), false, () =>
                        {
                            if (EditorUtility.DisplayDialog("Delete Group", "Are you sure?", "YES", "NO!"))
                            {
                                cutscene.DeleteGroup(group);
                                InitClipWrappers();
                            }
                        });
                    }

                    menu.ShowAsContext();
                    e.Use();
                }


                ///REORDERING
                if (e.type == EventType.MouseDown && e.button == 0 && groupRect.Contains(e.mousePosition))
                {
                    CutsceneUtility.selectedObject = group;
                    if (!(group is DirectorGroup))
                    {
                        pickedGroup = group;
                    }

                    if (e.clickCount == 2)
                    {
                        Selection.activeGameObject = group.actor;
                    }

                    e.Use();
                }

                if (pickedGroup != null && pickedGroup != group && !(group is DirectorGroup))
                {
                    if (groupRect.Contains(e.mousePosition))
                    {
                        var markRect = new Rect(groupRect.x,
                            (cutscene.groups.IndexOf(pickedGroup) < g) ? groupRect.yMax - 2 : groupRect.y,
                            groupRect.width, 2);
                        GUI.color = Color.grey;
                        GUI.DrawTexture(markRect, Styles.whiteTexture);
                        GUI.color = Color.white;
                    }

                    if (e.rawType == EventType.MouseUp && e.button == 0 && groupRect.Contains(e.mousePosition))
                    {
                        cutscene.groups.Remove(pickedGroup);
                        cutscene.groups.Insert(g, pickedGroup);
                        cutscene.Validate();
                        pickedGroup = null;
                        e.Use();
                    }
                }

                ///SHOW TRACKS (?)
                if (!group.isCollapsed)
                {
                    ShowListTracks(e, group, ref nextYPos);
                    //draw vertical graphic on left side of nested track rects
                    GUI.color = groupSelected ? LIST_SELECTION_COLOR : GROUP_COLOR;
                    var verticalRect = Rect.MinMaxRect(groupRect.x, groupRect.yMax, groupRect.x + 3, nextYPos - 2);
                    GUI.DrawTexture(verticalRect, Styles.whiteTexture);
                    GUI.color = Color.white;
                }
            }
        }

        //...
        void ShowListTracks(Event e, CutsceneGroup group, ref float nextYPos)
        {
            //TRACKS
            for (int t = 0; t < group.tracks.Count; t++)
            {
                var track = group.tracks[t];
                var yPos = nextYPos;

                var trackRect = new Rect(10, yPos, leftRect.width - TRACK_RIGHT_MARGIN - 10, track.finalHeight);
                nextYPos += track.finalHeight + TRACK_MARGINS;

                //GRAPHICS
                GUI.color = ColorUtility.Grey(isProSkin
                    ? (track.isActive ? 0.25f : 0.2f)
                    : (track.isActive ? 0.9f : 0.8f));
                GUI.DrawTexture(trackRect, whiteTexture);
                GUI.color = Color.white.WithAlpha(0.25f);
                GUI.Box(trackRect, string.Empty, (GUIStyle) "flow node 0");
                if (ReferenceEquals(track, CutsceneUtility.selectedObject) || track == pickedTrack)
                {
                    GUI.color = LIST_SELECTION_COLOR;
                    GUI.DrawTexture(trackRect, whiteTexture);
                }

                //custom color indicator
                if (track.isActive && track.color != Color.white && track.color.a > 0.2f)
                {
                    GUI.color = track.color;
                    var colorRect = new Rect(trackRect.xMax + 1, trackRect.yMin, 2, track.finalHeight);
                    GUI.DrawTexture(colorRect, whiteTexture);
                }

                GUI.color = Color.white;
                //

                ///
                GUI.BeginGroup(trackRect);
                track.OnTrackInfoGUI(trackRect);
                GUI.EndGroup();
                ///

                AddCursorRect(trackRect, pickedTrack == null ? MouseCursor.Link : MouseCursor.MoveArrow);

                //CONTEXT
                if (e.type == EventType.ContextClick && trackRect.Contains(e.mousePosition))
                {
                    var menu = new GenericMenu();
                    menu.AddItem(new GUIContent("Disable Track"), !track.isActive,
                        () => { track.isActive = !track.isActive; });
                    menu.AddItem(new GUIContent("Lock Track"), track.isLocked,
                        () => { track.isLocked = !track.isLocked; });
                    menu.AddItem(new GUIContent("Copy"), false, () => { copyTrack = track; });
                    if (track.GetType().RTGetAttribute<UniqueElementAttribute>(true) == null)
                    {
                        menu.AddItem(new GUIContent("Duplicate"), false, () =>
                        {
                            group.DuplicateTrack(track);
                            InitClipWrappers();
                        });
                    }
                    else
                    {
                        menu.AddDisabledItem(new GUIContent("Duplicate"));
                    }

                    menu.AddSeparator("/");
                    menu.AddItem(new GUIContent("Delete Track"), false, () =>
                    {
                        if (EditorUtility.DisplayDialog("Delete Track", "Are you sure?", "YES", "NO!"))
                        {
                            group.DeleteTrack(track);
                            InitClipWrappers();
                        }
                    });
                    menu.ShowAsContext();
                    e.Use();
                }

                //REORDERING
                if (e.type == EventType.MouseDown && e.button == 0 && trackRect.Contains(e.mousePosition))
                {
                    CutsceneUtility.selectedObject = track;
                    pickedTrack = track;
                    e.Use();
                }

                if (pickedTrack != null && pickedTrack != track && ReferenceEquals(pickedTrack.parent, group))
                {
                    if (trackRect.Contains(e.mousePosition))
                    {
                        var markRect = new Rect(trackRect.x,
                            (group.tracks.IndexOf(pickedTrack) < t) ? trackRect.yMax - 2 : trackRect.y, trackRect.width,
                            2);
                        GUI.color = Color.grey;
                        GUI.DrawTexture(markRect, Styles.whiteTexture);
                        GUI.color = Color.white;
                    }

                    if (e.rawType == EventType.MouseUp && e.button == 0 && trackRect.Contains(e.mousePosition))
                    {
                        group.tracks.Remove(pickedTrack);
                        group.tracks.Insert(t, pickedTrack);
                        cutscene.Validate();
                        pickedTrack = null;
                        e.Use();
                    }
                }
            }
        }


        ///----------------------------------------------------------------------------------------------

        //middle - the actual timeline tracks
        void ShowTimeLines(Rect centerRect)
        {
            var e = Event.current;

            //bg graphic
            var bgRect = Rect.MinMaxRect(centerRect.xMin, TOP_MARGIN + TOOLBAR_HEIGHT + scrollPos.y, centerRect.xMax,
                screenHeight - TOOLBAR_HEIGHT + scrollPos.y);
            GUI.color = Color.black.WithAlpha(0.1f);
            GUI.DrawTexture(bgRect, whiteTexture);
            GUI.color = Color.black.WithAlpha(0.03f);
            GUI.DrawTextureWithTexCoords(bgRect, Styles.stripes, new Rect(0, 0, bgRect.width / -7, bgRect.height / -7));
            GUI.color = Color.white;

            // draw guides based on time info stored
            for (var _i = timeInfoStart; _i <= timeInfoEnd; _i += timeInfoInterval)
            {
                var i = Mathf.Round(_i * 10) / 10;
                DrawGuideLine(i, Color.black.WithAlpha(0.05f));
                if (i % timeInfoHighMod == 0)
                {
                    DrawGuideLine(i, Color.black.WithAlpha(0.05f));
                }
            }


            //Begin Group
            GUI.BeginGroup(centerRect);

            //starting height
            var nextYPos = FIRST_GROUP_TOP_MARGIN;

            //master sections
            var sectionsRect = Rect.MinMaxRect(Mathf.Max(TimeToPos(viewTimeMin), TimeToPos(0)), 3,
                TimeToPos(viewTimeMax), 18);
            if (cutscene.directorGroup != null)
            {
                //it never should
                ShowGroupSections(cutscene.directorGroup, sectionsRect);
            }

            //Begin Windows
            BeginWindows();

            //GROUPS
            for (int g = 0; g < cutscene.groups.Count; g++)
            {
                var group = cutscene.groups[g];

                if (IsFilteredOutBySearch(group, searchString))
                {
                    group.isCollapsed = true;
                    continue;
                }

                var groupRect = Rect.MinMaxRect(Mathf.Max(TimeToPos(viewTimeMin), TimeToPos(0)), nextYPos,
                    TimeToPos(viewTimeMax), nextYPos + GROUP_HEIGHT);
                nextYPos += GROUP_HEIGHT;

                //if collapsed, just show a heat minimap of clips.
                if (group.isCollapsed)
                {
                    GUI.color = Color.black.WithAlpha(0.15f);
                    var collapseRect = Rect.MinMaxRect(groupRect.xMin + 2, groupRect.yMin + 2, groupRect.xMax,
                        groupRect.yMax - 4);
                    GUI.DrawTexture(collapseRect, Styles.whiteTexture);
                    GUI.color = Color.grey.WithAlpha(0.5f);
                    foreach (var track in group.tracks)
                    {
                        foreach (var clip in track.clips)
                        {
                            var start = TimeToPos(clip.startTime);
                            var end = TimeToPos(clip.endTime);
                            GUI.DrawTexture(
                                Rect.MinMaxRect(start + 0.5f, collapseRect.y + 2, end - 0.5f, collapseRect.yMax - 2),
                                Styles.whiteTexture);
                        }
                    }

                    GUI.color = Color.white;
                    continue;
                }


                //TRACKS
                for (int t = 0; t < group.tracks.Count; t++)
                {
                    var track = group.tracks[t];
                    var yPos = nextYPos;
                    var trackPosRect = Rect.MinMaxRect(Mathf.Max(TimeToPos(viewTimeMin), TimeToPos(track.startTime)),
                        yPos, TimeToPos(viewTimeMax), yPos + track.finalHeight);
                    var trackTimeRect = Rect.MinMaxRect(Mathf.Max(viewTimeMin, track.startTime), 0, viewTimeMax, 0);
                    nextYPos += track.finalHeight + TRACK_MARGINS;

                    //GRAPHICS
                    GUI.color = Color.black.WithAlpha(isProSkin ? 0.06f : 0.1f);
                    GUI.DrawTexture(trackPosRect, whiteTexture);
                    Handles.color = ColorUtility.Grey(isProSkin ? 0.15f : 0.4f);
                    Handles.DrawLine(new Vector2(TimeToPos(viewTimeMin), trackPosRect.y + 1),
                        new Vector2(trackPosRect.xMax, trackPosRect.y + 1));
                    Handles.DrawLine(new Vector2(TimeToPos(viewTimeMin), trackPosRect.yMax),
                        new Vector2(trackPosRect.xMax, trackPosRect.yMax));
                    if (track.showCurves)
                    {
                        Handles.DrawLine(new Vector2(trackPosRect.x, trackPosRect.y + track.defaultHeight),
                            new Vector2(trackPosRect.xMax, trackPosRect.y + track.defaultHeight));
                    }

                    Handles.color = Color.white;
                    if (viewTimeMin < 0)
                    {
                        //just visual clarity
                        GUI.Box(
                            Rect.MinMaxRect(TimeToPos(viewTimeMin), trackPosRect.yMin, TimeToPos(0), trackPosRect.yMax),
                            string.Empty);
                    }

                    if (track.startTime > track.parent.startTime || track.endTime < track.parent.endTime)
                    {
                        Handles.color = Color.white;
                        GUI.color = Color.black.WithAlpha(0.2f);
                        if (track.startTime > track.parent.startTime)
                        {
                            var tStart = TimeToPos(track.startTime);
                            var r = Rect.MinMaxRect(TimeToPos(0), yPos, tStart, yPos + track.finalHeight);
                            GUI.DrawTexture(r, whiteTexture);
                            GUI.DrawTextureWithTexCoords(r, Styles.stripes, new Rect(0, 0, r.width / 7, r.height / 7));
                            var a = new Vector2(tStart, trackPosRect.yMin);
                            var b = new Vector2(a.x, trackPosRect.yMax);
                            Handles.DrawLine(a, b);
                        }

                        if (track.endTime < track.parent.endTime)
                        {
                            var tEnd = TimeToPos(track.endTime);
                            var r = Rect.MinMaxRect(tEnd, yPos, TimeToPos(length), yPos + track.finalHeight);
                            GUI.DrawTexture(r, whiteTexture);
                            GUI.DrawTextureWithTexCoords(r, Styles.stripes, new Rect(0, 0, r.width / 7, r.height / 7));
                            var a = new Vector2(tEnd, trackPosRect.yMin);
                            var b = new Vector2(a.x, trackPosRect.yMax);
                            Handles.DrawLine(a, b);
                        }

                        GUI.color = Color.white;
                        Handles.color = Color.white;
                    }

                    GUI.backgroundColor = Color.white;

                    //highlight selected track
                    if (ReferenceEquals(CutsceneUtility.selectedObject, track))
                    {
                        GUI.color = Color.grey;
                        GUI.Box(trackPosRect.ExpandBy(0, 2), string.Empty, Styles.hollowFrameHorizontalStyle);
                        GUI.color = Color.white;
                    }
                    ///

                    if (track.isLocked)
                    {
                        if (e.isMouse && trackPosRect.Contains(e.mousePosition))
                        {
                            e.Use();
                        }
                    }

                    //...
                    var cursorTime = SnapTime(PosToTime(mousePosition.x));
                    track.OnTrackTimelineGUI(trackPosRect, trackTimeRect, cursorTime, TimeToPos);
                    //...

                    if (!track.isActive || track.isLocked)
                    {
                        postWindowsGUI += () =>
                        {
                            //overlay dark stripes for disabled tracks
                            if (!track.isActive)
                            {
                                GUI.color = Color.black.WithAlpha(0.2f);
                                GUI.DrawTexture(trackPosRect, whiteTexture);
                                GUI.DrawTextureWithTexCoords(trackPosRect, Styles.stripes,
                                    new Rect(0, 0, (trackPosRect.width / 5), (trackPosRect.height / 5)));
                                GUI.color = Color.white;
                            }

                            //overlay light stripes for locked tracks
                            if (track.isLocked)
                            {
                                GUI.color = Color.black.WithAlpha(0.15f);
                                GUI.DrawTextureWithTexCoords(trackPosRect, Styles.stripes,
                                    new Rect(0, 0, trackPosRect.width / 20, trackPosRect.height / 20));
                                GUI.color = Color.white;
                            }

                            if (isProSkin)
                            {
                                string overlayLabel = null;
                                if (!track.isActive && track.isLocked)
                                {
                                    overlayLabel = "DISABLED & LOCKED";
                                }
                                else
                                {
                                    if (!track.isActive)
                                    {
                                        overlayLabel = "DISABLED";
                                    }

                                    if (track.isLocked)
                                    {
                                        overlayLabel = "LOCKED";
                                    }
                                }

                                var size = Styles.centerLabel.CalcSize(new GUIContent(overlayLabel));
                                var bgLabelRect = new Rect(0, 0, size.x, size.y);
                                bgLabelRect.center = trackPosRect.center;
                                GUI.Label(trackPosRect, string.Format("<b>{0}</b>", overlayLabel), Styles.centerLabel);
                                GUI.color = Color.white;
                            }
                        };
                    }


                    //ACTION CLIPS
                    for (int a = 0; a < track.clips.Count; a++)
                    {
                        var action = track.clips[a];
                        var ID = UID(g, t, a);
                        ActionClipWrapper clipWrapper = null;

                        if (!clipWrappers.TryGetValue(ID, out clipWrapper) || clipWrapper.action != action)
                        {
                            InitClipWrappers();
                            clipWrapper = clipWrappers[ID];
                        }

                        //find and store next/previous clips to wrapper
                        var nextClip = a < track.clips.Count - 1 ? track.clips[a + 1] : null;
                        var previousClip = a != 0 ? track.clips[a - 1] : null;
                        clipWrapper.nextClip = nextClip;
                        clipWrapper.previousClip = previousClip;


                        //get the action box rect
                        var clipRect = clipWrapper.rect;

                        //modify it
                        clipRect.y = yPos;
                        clipRect.width = Mathf.Max(action.length / viewTime * centerRect.width, 6);
                        clipRect.height = track.defaultHeight;


                        //get the action time and pos
                        var xTime = action.startTime;
                        var xPos = clipRect.x;

                        if (interactingClip != null && ReferenceEquals(interactingClip.action, action) &&
                            interactingClip.isDragging)
                        {
                            var lastTime = xTime;
                            xTime = PosToTime(xPos + leftRect.width);
                            xTime = SnapTime(xTime);
                            xTime = Mathf.Clamp(xTime, 0, maxTime - 0.1f);

                            //handle multisection. Limit xmin, xmax by their bound rect
                            if (multiSelection != null && multiSelection.Count > 1)
                            {
                                var delta = xTime - lastTime;
                                var boundMin = Mathf.Min(multiSelection.Select(b => b.action.startTime).ToArray());
                                if (boundMin + delta < 0)
                                {
                                    xTime -= delta;
                                    delta = 0;
                                }

                                foreach (var cw in multiSelection)
                                {
                                    if (cw.action != action)
                                    {
                                        cw.action.startTime += delta;
                                    }
                                }
                            }

                            //clamp and cross blend between other nearby clips
                            if (multiSelection == null || multiSelection.Count < 1)
                            {
                                var preCursorClip =
                                    track.clips.LastOrDefault(x => x != action && x.startTime < cursorTime);
                                var postCursorClip =
                                    track.clips.FirstOrDefault(x => x != action && x.endTime > cursorTime);

                                //Shift/Ripple clips
                                //when shifting track clips always clamp to previous clip and no need to clamp to next
                                if (e.shift)
                                {
                                    preCursorClip = previousClip;
                                    postCursorClip = null;
                                }

                                var preTime = preCursorClip != null ? preCursorClip.endTime : 0;
                                var postTime = postCursorClip != null
                                    ? postCursorClip.startTime
                                    : maxTime + action.length;

                                //Magnet snapping when dragging clip
                                if (Prefs.magnetSnapping && !e.control)
                                {
                                    var snapStart = MagnetSnapTime(xTime, magnetSnapTimesCache);
                                    var snapEnd = MagnetSnapTime(xTime + action.length, magnetSnapTimesCache);
                                    if (snapStart != null && snapEnd != null)
                                    {
                                        var distStart = Mathf.Abs(snapStart.Value - xTime);
                                        var distEnd = Mathf.Abs(snapEnd.Value - (xTime + action.length));
                                        var bestTime = distEnd < distStart ? snapEnd.Value : snapStart.Value;
                                        pendingGuides.Add(new GuideLine(bestTime, Color.white));
                                        xTime = distEnd < distStart ? snapEnd.Value - action.length : snapStart.Value;
                                    }
                                    else
                                    {
                                        if (snapEnd != null)
                                        {
                                            pendingGuides.Add(new GuideLine(snapEnd.Value, Color.white));
                                            xTime = snapEnd.Value - action.length;
                                        }

                                        if (snapStart != null)
                                        {
                                            pendingGuides.Add(new GuideLine(snapStart.Value, Color.white));
                                            xTime = snapStart.Value;
                                        }
                                    }
                                }


                                //expand possible time if crossblendable
                                if (action.CanCrossBlend(preCursorClip))
                                {
                                    preTime -= Mathf.Min(action.length / 2, preCursorClip.length / 2);
                                }

                                if (action.CanCrossBlend(postCursorClip))
                                {
                                    postTime += Mathf.Min(action.length / 2, postCursorClip.length / 2);
                                }

                                //does it fit?
                                if (action.length > postTime - preTime)
                                {
                                    xTime = lastTime;
                                }

                                if (xTime != lastTime)
                                {
                                    xTime = Mathf.Clamp(xTime, preTime, postTime - action.length);
                                    //Shift all the next clips along with this one if shift is down
                                    if (e.shift)
                                    {
                                        foreach (var cw in clipWrappers.Values.Where(c =>
                                            c.action.parent == action.parent && c.action != action &&
                                            c.action.startTime > lastTime))
                                        {
                                            cw.action.startTime += xTime - lastTime;
                                        }
                                    }
                                }
                            }

                            //Apply xTime
                            action.startTime = xTime;
                        }

                        //apply xPos
                        clipRect.x = TimeToPos(xTime);


                        //dont draw if outside of view range and not selected
                        var isSelected = ReferenceEquals(CutsceneUtility.selectedObject, action) ||
                                         (multiSelection != null &&
                                          multiSelection.Select(b => b.action).Contains(action));
                        var isVisible = Rect.MinMaxRect(0, scrollPos.y, centerRect.width, centerRect.height)
                            .Overlaps(clipRect);
                        if (!isSelected && !isVisible)
                        {
                            clipWrapper.rect =
                                default(Rect); //we basicaly "nullify" the rect. Too much trouble to work with nullable rect.
                            continue;
                        }

                        //draw selection graphics rect
                        if (isSelected)
                        {
                            var selRect = clipRect.ExpandBy(2);
                            GUI.color = HIGHLIGHT_COLOR;
                            GUI.DrawTexture(selRect, Slate.Styles.whiteTexture);
                            GUI.color = Color.white;
                        }

                        //determine color and draw clip
                        var color = Color.white;
                        color = action.isValid ? color : new Color(1, 0.3f, 0.3f);
                        color = track.isActive ? color : Color.grey;
                        GUI.color = color;
                        GUI.Box(clipRect, string.Empty, Styles.clipBoxHorizontalStyle);
                        clipWrapper.rect = GUI.Window(ID, clipRect, ActionClipWindow, string.Empty, GUIStyle.none);
                        if (!isProSkin)
                        {
                            GUI.color = Color.white.WithAlpha(0.5f);
                            GUI.Box(clipRect, string.Empty);
                            GUI.color = Color.white;
                        }

                        GUI.color = Color.white;

                        //forward external Clip GUI
                        var nextPosX = TimeToPos(nextClip != null ? nextClip.startTime : viewTimeMax);
                        var prevPosX = TimeToPos(previousClip != null ? previousClip.endTime : viewTimeMin);
                        var extRectLeft = Rect.MinMaxRect(prevPosX, clipRect.yMin, clipRect.xMin, clipRect.yMax);
                        var extRectRight = Rect.MinMaxRect(clipRect.xMax, clipRect.yMin, nextPosX, clipRect.yMax);
                        action.ShowClipGUIExternal(extRectLeft, extRectRight);

                        //draw info text outside if clip is too small
                        if (clipRect.width <= 20)
                        {
                            GUI.Label(extRectRight, string.Format("<size=9>{0}</size>", action.info));
                        }
                    }
                }

                //highligh selected group
                if (ReferenceEquals(CutsceneUtility.selectedObject, group))
                {
                    var r = Rect.MinMaxRect(groupRect.xMin, groupRect.yMin, groupRect.xMax, nextYPos);
                    GUI.color = Color.grey;
                    GUI.Box(r, string.Empty, Styles.hollowFrameHorizontalStyle);
                    GUI.color = Color.white;
                }
            }

            EndWindows();

            //call postwindow delegate
            if (postWindowsGUI != null)
            {
                postWindowsGUI();
                postWindowsGUI = null;
            }

            //this is done in the same GUI.Group
            DoMultiSelection();

            GUI.EndGroup();

            //border shadows
            GUI.color = Color.white.WithAlpha(0.2f);
            GUI.Box(bgRect, string.Empty, Styles.shadowBorderStyle);
            GUI.color = Color.white;

            ///darken the time after cutscene length
            if (viewTimeMax > length)
            {
                var endPos = Mathf.Max(TimeToPos(length) + leftRect.width, centerRect.xMin);
                var darkRect = Rect.MinMaxRect(endPos, centerRect.yMin, centerRect.xMax, centerRect.yMax);
                GUI.color = Color.black.WithAlpha(0.3f);
                GUI.Box(darkRect, string.Empty, (GUIStyle) "TextField");
                GUI.color = Color.white;
            }

            ///darken the time before zero
            if (viewTimeMin < 0)
            {
                var startPos = Mathf.Min(TimeToPos(0) + leftRect.width, centerRect.xMax);
                var darkRect = Rect.MinMaxRect(centerRect.xMin, centerRect.yMin, startPos, centerRect.yMax);
                GUI.color = Color.black.WithAlpha(0.3f);
                GUI.Box(darkRect, string.Empty, (GUIStyle) "TextField");
                GUI.color = Color.white;
            }

            //ensure no interactive clip
            if (e.rawType == EventType.MouseUp)
            {
                if (interactingClip != null)
                {
                    interactingClip.ResetInteraction();
                    interactingClip.EndClipAdjust();
                    interactingClip = null;
                }
            }
        }


        //Group sections...
        void ShowGroupSections(CutsceneGroup group, Rect rect)
        {
            var e = Event.current;
            GenericMenu sectionsMenu = null;
            if (e.type == EventType.ContextClick && rect.Contains(e.mousePosition))
            {
                var t = PosToTime(mousePosition.x);
                sectionsMenu = new GenericMenu();
                sectionsMenu.AddItem(new GUIContent("Add Section Here"), false,
                    () => { group.sections.Add(new Section("Section", t)); });
            }

            var sections = new List<Section>(group.sections.OrderBy(s => s.time));
            if (sections.Count == 0)
            {
                sections.Insert(0, new Section("No Sections", 0));
                sections.Add(new Section("Outro", maxTime));
            }
            else
            {
                sections.Insert(0, new Section("Intro", 0));
                sections.Add(new Section("Outro", maxTime));
            }

            for (var i = 0; i < sections.Count - 1; i++)
            {
                var section1 = sections[i];
                var section2 = sections[i + 1];
                var pos1 = TimeToPos(section1.time);
                var pos2 = TimeToPos(section2.time);
                var y = rect.y;

                var sectionRect = Rect.MinMaxRect(pos1, y, pos2 - 2, y + GROUP_HEIGHT - 5);
                var markRect = new Rect(sectionRect.x + 2, sectionRect.y + 2, 2, sectionRect.height - 4);
                var clickRect = new Rect(0, y, 15, sectionRect.height);
                var loopRect = Rect.MinMaxRect(Mathf.Max(sectionRect.xMax - 18, sectionRect.xMin), sectionRect.yMin + 2,
                    sectionRect.xMax - 2, sectionRect.yMax - 2);

                clickRect.center = markRect.center;

                GUI.color = section1.color;
                if (section1.colorizeBackground)
                {
                    GUI.DrawTexture(
                        Rect.MinMaxRect(sectionRect.xMin, sectionRect.yMax + 1, sectionRect.xMax,
                            screenHeight + scrollPos.y), whiteTexture);
                }

                GUI.DrawTexture(sectionRect, whiteTexture);
                GUI.color = Color.white.WithAlpha(0.2f);
                GUI.DrawTexture(markRect, whiteTexture);
                GUI.color = (section1.color.grayscale >= 0.5 ? Color.black : Color.white).WithAlpha(0.5f);
                if (section1.exitMode == Section.ExitMode.Loop)
                {
                    GUI.DrawTexture(loopRect, Styles.loopIcon);
                    if (section1.loopCount > 0)
                    {
                        var text = string.Format("<size=9>x {0}/{1}</size>",
                            Mathf.Min(section1.currentLoopIteration, section1.loopCount), section1.loopCount);
                        var loopCountRect = Rect.MinMaxRect(sectionRect.xMin, sectionRect.yMin, loopRect.xMin - 2,
                            sectionRect.yMax);
                        GUI.Label(loopCountRect, text, Styles.rightLabel);
                    }
                }

                GUI.color = section1.color.grayscale >= 0.5 ? Color.black : Color.white;
                GUI.Label(sectionRect, string.Format(" <i>{0}</i>", section1.name));
                GUI.color = Color.white;


                if (sectionRect.Contains(e.mousePosition))
                {
                    if (e.type == EventType.MouseDown && e.button == 0)
                    {
                        if (e.clickCount == 2)
                        {
                            viewTimeMin = section1.time;
                            viewTimeMax = section2.time;
                            e.Use();
                        }
                    }

                    if (i != 0 && e.type == EventType.ContextClick && sectionsMenu != null)
                    {
                        sectionsMenu.AddItem(new GUIContent("Edit"), false, () =>
                        {
                            DoPopup(() =>
                            {
                                section1.name = EditorGUILayout.TextField("Name", section1.name);
                                var previousSectionTime =
                                    sections.Last(s => s.time < section1.time && s != section1).time;
                                var nextSectionTime = sections.First(s => s.time > section1.time && s != section1).time;
                                section1.time = EditorGUILayout.Slider("Time", section1.time,
                                    previousSectionTime + 0.1f, nextSectionTime - 0.1f);
                                section1.exitMode =
                                    (Section.ExitMode) EditorGUILayout.EnumPopup("Exit Mode", section1.exitMode);
                                if (section1.exitMode == Section.ExitMode.Loop)
                                {
                                    section1.loopCount = EditorGUILayout.IntField("Loops", section1.loopCount);
                                }

                                section1.color = EditorGUILayout.ColorField("Color", section1.color);
                                section1.colorizeBackground = EditorGUILayout.Toggle("Colorize Background",
                                    section1.colorizeBackground);
                            });
                        });
                        sectionsMenu.AddItem(new GUIContent("Focus (Double Click)"), false, () =>
                        {
                            viewTimeMin = section1.time;
                            viewTimeMax = section2.time;
                        });
                        sectionsMenu.AddSeparator("/");
                        sectionsMenu.AddItem(new GUIContent("Delete Section"), false,
                            () => { group.sections.Remove(section1); });
                    }
                }

                if (i != 0 && clickRect.Contains(e.mousePosition))
                {
                    this.AddCursorRect(clickRect, MouseCursor.SlideArrow);
                    if (e.type == EventType.MouseDown && e.button == 0)
                    {
                        draggedSection = section1;
                        e.Use();
                    }
                }
            }

            if (draggedSection != null)
            {
                var lastTime = draggedSection.time;
                var newTime = PosToTime(mousePosition.x);
                var previousSectionTime = sections.Last(s => s.time < lastTime).time;
                var nextSectionTime = sections.First(s => s.time > lastTime).time;
                newTime = SnapTime(newTime);
                newTime = Mathf.Clamp(newTime, previousSectionTime + 0.1f, nextSectionTime - 0.1f);
                newTime = Mathf.Clamp(newTime, 0, maxTime);
                draggedSection.time = newTime;

                //shift clips if shift.
                if (e.shift)
                {
                    foreach (var cw in clipWrappers.Values.Where(c => c.action.startTime >= lastTime))
                    {
                        if (cw.action.isLocked)
                        {
                            continue;
                        }

                        var max = cw.previousClip != null ? cw.previousClip.endTime : 0;
                        if (cw.action.CanCrossBlend(cw.previousClip))
                        {
                            max -= Mathf.Min(cw.previousClip.length / 2, cw.action.length / 2);
                        }

                        cw.action.startTime += newTime - lastTime;
                        cw.action.startTime = Mathf.Max(cw.action.startTime, max);
                    }

                    ///This is very unoptimized but PropertyTrack will be deprecated in the future.
                    foreach (var propTrack in cutscene.directables.OfType<PropertiesTrack>())
                    {
                        if (propTrack.isLocked)
                        {
                            continue;
                        }

                        var curves = propTrack.GetCurvesAll();
                        foreach (var curve in curves)
                        {
                            for (var i = 0; i < curve.length; i++)
                            {
                                var key = curve[i];
                                if (key.time >= lastTime)
                                {
                                    key.time += newTime - lastTime;
                                    curve.MoveKey(i, key);
                                }
                            }

                            curve.UpdateTangentsFromMode();
                        }

                        CutsceneUtility.RefreshAllAnimationEditorsOf(propTrack.animationData);
                    }

                    ///
                }

                //shift sections if shift or control
                if (e.shift || e.control)
                {
                    foreach (var section in group.sections.Where(s => s != draggedSection && s.time > lastTime))
                    {
                        section.time += newTime - lastTime;
                    }
                }

                //reset interaction and order sections
                if (e.rawType == EventType.MouseUp)
                {
                    draggedSection = null;
                    group.sections = group.sections.OrderBy(s => s.time).ToList();
                }
            }

            if (sectionsMenu != null)
            {
                sectionsMenu.ShowAsContext();
            }
        }

        //This is done in a GUILayout.Group, thus must use e.mousePosition instead of this.mousePosition
        void DoMultiSelection()
        {
            var e = Event.current;

            var r = new Rect();
            var bigEnough = false;
            if (multiSelectStartPos != null)
            {
                var start = (Vector2) multiSelectStartPos;
                if ((start - e.mousePosition).magnitude > 10)
                {
                    bigEnough = true;
                    r.xMin = Mathf.Max(Mathf.Min(start.x, e.mousePosition.x), 0);
                    r.xMax = Mathf.Min(Mathf.Max(start.x, e.mousePosition.x), screenWidth);
                    r.yMin = Mathf.Min(start.y, e.mousePosition.y);
                    r.yMax = Mathf.Max(start.y, e.mousePosition.y);
                    GUI.color = isProSkin ? Color.white : Color.white.WithAlpha(0.3f);
                    GUI.Box(r, string.Empty, Styles.hollowFrameStyle);
                    GUI.color = Color.white.WithAlpha(0.05f);
                    GUI.DrawTexture(r, whiteTexture);
                    GUI.color = Color.white;
                    foreach (var wrapper in clipWrappers.Values.Where(b => r.Encapsulates(b.rect) && !b.action.isLocked)
                    )
                    {
                        GUI.color = new Color(0.5f, 0.5f, 1, 0.5f);
                        GUI.Box(wrapper.rect, string.Empty, Slate.Styles.clipBoxStyle);
                        GUI.color = Color.white;
                    }
                }
            }

            if (e.rawType == EventType.MouseUp)
            {
                if (bigEnough)
                {
                    multiSelection = clipWrappers.Values.Where(b => r.Encapsulates(b.rect) && !b.action.isLocked)
                        .ToList();
                    if (multiSelection.Count == 1)
                    {
                        CutsceneUtility.selectedObject = multiSelection[0].action;
                        multiSelection = null;
                    }
                }

                multiSelectStartPos = null;
            }

            if (multiSelection != null)
            {
                var boundRect = RectUtility.GetBoundRect(multiSelection.Select(b => b.rect).ToArray()).ExpandBy(4);

                var leftDragRect = new Rect(boundRect.xMin - 6, boundRect.yMin, 4, boundRect.height);
                var rightDragRect = new Rect(boundRect.xMax + 2, boundRect.yMin, 4, boundRect.height);
                AddCursorRect(leftDragRect, MouseCursor.ResizeHorizontal);
                AddCursorRect(rightDragRect, MouseCursor.ResizeHorizontal);
                GUI.color = isProSkin ? new Color(0.7f, 0.7f, 0.7f) : Color.grey;
                GUI.DrawTexture(leftDragRect, Styles.whiteTexture);
                GUI.DrawTexture(rightDragRect, Styles.whiteTexture);
                GUI.color = Color.white;

                if (e.type == EventType.MouseDown &&
                    (leftDragRect.Contains(e.mousePosition) || rightDragRect.Contains(e.mousePosition)))
                {
                    multiSelectionScaleDirection = leftDragRect.Contains(e.mousePosition) ? -1 : 1;
                    var minTime = Mathf.Min(multiSelection.Select(b => b.action.startTime).ToArray());
                    var maxTime = Mathf.Max(multiSelection.Select(b => b.action.endTime).ToArray());
                    preMultiSelectionRetimeMinMax = Rect.MinMaxRect(minTime, 0, maxTime, 0);
                    foreach (var wrapper in multiSelection)
                    {
                        wrapper.BeginClipAdjust();
                    }

                    e.Use();
                }

                if (e.type == EventType.MouseDrag && multiSelectionScaleDirection != 0)
                {
                    foreach (var clipWrapper in multiSelection)
                    {
                        var preTimeMin = preMultiSelectionRetimeMinMax.xMin;
                        var preTimeMax = preMultiSelectionRetimeMinMax.xMax;
                        var pointerTime = SnapTime(PosToTime(mousePosition.x));

                        var lerpMin = multiSelectionScaleDirection == -1
                            ? Mathf.Clamp(pointerTime, 0, preTimeMax)
                            : preTimeMin;
                        var lerpMax = multiSelectionScaleDirection == 1
                            ? Mathf.Max(pointerTime, preTimeMin)
                            : preTimeMax;

                        var normIn = Mathf.InverseLerp(preTimeMin, preTimeMax, clipWrapper.preScaleStartTime);
                        clipWrapper.action.startTime = Mathf.Lerp(lerpMin, lerpMax, normIn);

                        var normOut = Mathf.InverseLerp(preTimeMin, preTimeMax, clipWrapper.preScaleEndTime);
                        clipWrapper.action.endTime = Mathf.Lerp(lerpMin, lerpMax, normOut);

                        clipWrapper.UpdateClipAdjustContents();
                    }

                    e.Use();
                }

                if (e.rawType == EventType.MouseUp)
                {
                    multiSelectionScaleDirection = 0;
                    foreach (var clipWrapper in multiSelection)
                    {
                        clipWrapper.EndClipAdjust();
                    }
                }
            }

            if (e.type == EventType.MouseDown && e.button == 0 && GUIUtility.hotControl == 0)
            {
                multiSelection = null;
                multiSelectStartPos = e.mousePosition;
            }

            GUI.color = Color.white;
        }


        //ActionClip window callback. Its ID is based on the UID function that is based on the index path to the action.
        //The ID of the window is also the same as the ID to use for for clipWrappers dictionary as key to get the clipWrapper for the action that represents this window
        void ActionClipWindow(int id)
        {
            ActionClipWrapper wrapper = null;
            if (clipWrappers.TryGetValue(id, out wrapper))
            {
                wrapper.OnClipGUI(id);
            }
        }


        ///----------------------------------------------------------------------------------------------

        //A wrapper of an ActionClip placed in cutscene
        class ActionClipWrapper
        {
            const float CLIP_DOPESHEET_HEIGHT = 13f;
            const float SCALE_RECT_WIDTH = 5;

            public ActionClip action;
            public bool isDragging;
            public bool isScalingStart;
            public bool isScalingEnd;
            public bool isControlingBlendIn;
            public bool isControlingBlendOut;

            public Dictionary<AnimationCurve, Keyframe[]> preScaleKeys;
            public float preScaleStartTime;
            public float preScaleEndTime;
            public float preScaleSubclipOffset;
            public float preScaleSubclipSpeed;

            public ActionClip previousClip;
            public ActionClip nextClip;

            private Event e;
            private int windowID;
            private bool isWaitingMouseDrag;
            private float overlapIn;
            private float overlapOut;
            private float blendInPosX;
            private float blendOutPosX;
            private bool hasActiveParameters;
            private bool hasParameters;
            private float pointerTime;
            private float snapedPointerTime;
            private bool allowScale;

            private Rect dragRect;
            private Rect controlRectIn;
            private Rect controlRectOut;

            private CutsceneEditor editor
            {
                get { return CutsceneEditor.current; }
            }

            private List<ActionClipWrapper> multiSelection
            {
                get { return editor.multiSelection; }
                set { editor.multiSelection = value; }
            }

            private Rect _rect;

            public Rect rect
            {
                get { return action.isCollapsed ? default(Rect) : _rect; }
                set { _rect = value; }
            }

            public ActionClipWrapper(ActionClip action)
            {
                this.action = action;
            }

            public void ResetInteraction()
            {
                isWaitingMouseDrag = false;
                isDragging = false;
                isControlingBlendIn = false;
                isControlingBlendOut = false;
                isScalingStart = false;
                isScalingEnd = false;
            }

            public void OnClipGUI(int windowID)
            {
                this.windowID = windowID;
                e = Event.current;

                overlapIn = previousClip != null ? Mathf.Max(previousClip.endTime - action.startTime, 0) : 0;
                overlapOut = nextClip != null ? Mathf.Max(action.endTime - nextClip.startTime, 0) : 0;
                blendInPosX = (action.blendIn / action.length) * rect.width;
                blendOutPosX = ((action.length - action.blendOut) / action.length) * rect.width;
                hasParameters = action.hasParameters;
                hasActiveParameters = action.hasActiveParameters;

                pointerTime = editor.PosToTime(editor.mousePosition.x);
                snapedPointerTime = editor.SnapTime(pointerTime);

                allowScale = action.CanScale() && action.length > 0 && rect.width > SCALE_RECT_WIDTH * 2;
                dragRect = new Rect(0, 0, rect.width, rect.height - (hasActiveParameters ? CLIP_DOPESHEET_HEIGHT : 0))
                    .ExpandBy(allowScale ? -SCALE_RECT_WIDTH : 0, 0);
                controlRectIn = new Rect(0, 0, SCALE_RECT_WIDTH,
                    rect.height - (hasActiveParameters ? CLIP_DOPESHEET_HEIGHT : 0));
                controlRectOut = new Rect(rect.width - SCALE_RECT_WIDTH, 0, SCALE_RECT_WIDTH,
                    rect.height - (hasActiveParameters ? CLIP_DOPESHEET_HEIGHT : 0));

                editor.AddCursorRect(dragRect, MouseCursor.Link);
                if (allowScale)
                {
                    editor.AddCursorRect(controlRectIn, MouseCursor.ResizeHorizontal);
                    editor.AddCursorRect(controlRectOut, MouseCursor.ResizeHorizontal);
                }

                //...
                var wholeRect = new Rect(0, 0, rect.width, rect.height);
                if (action.isLocked && e.isMouse && wholeRect.Contains(e.mousePosition))
                {
                    e.Use();
                }

                action.ShowClipGUI(wholeRect);
                if (hasActiveParameters && action.length > 0)
                {
                    ShowClipDopesheet(wholeRect);
                }
                //...


                //set crossblend overlap properties. Do this when no clip is interacting or no clip is dragging
                //this way avoid issue when moving clip on the other side of another, but keep overlap interactive when scaling a clip at least.
                if (editor.interactingClip == null || !editor.interactingClip.isDragging)
                {
                    var overlap = previousClip != null ? Mathf.Max(previousClip.endTime - action.startTime, 0) : 0;
                    if (overlap > 0)
                    {
                        action.blendIn = overlap;
                        previousClip.blendOut = overlap;
                    }
                }


                if (e.type == EventType.MouseDown)
                {
                    if (e.button == 0)
                    {
                        if (dragRect.Contains(e.mousePosition))
                        {
                            isWaitingMouseDrag = true;
                        }

                        editor.interactingClip = this;
                        editor.CacheMagnetSnapTimes(action);
                    }

                    if (e.control && dragRect.Contains(e.mousePosition))
                    {
                        if (multiSelection == null)
                        {
                            multiSelection = new List<ActionClipWrapper>() {this};
                        }

                        if (multiSelection.Contains(this))
                        {
                            multiSelection.Remove(this);
                        }
                        else
                        {
                            multiSelection.Add(this);
                        }
                    }
                    else
                    {
                        CutsceneUtility.selectedObject = action;
                        if (multiSelection != null && !multiSelection.Select(cw => cw.action).Contains(action))
                        {
                            multiSelection = null;
                        }
                    }

                    if (e.clickCount == 2)
                    {
                        //do this with reflection to get the declaring actor in case action has 'new' declaration. This is only done in Shot right now.
                        Selection.activeObject = action.GetType().GetProperty("actor").GetValue(action, null) as Object;
                    }
                }

                if (e.type == EventType.MouseDrag && isWaitingMouseDrag)
                {
                    isDragging = true;
                }

                if (e.rawType == EventType.ContextClick)
                {
                    DoClipContextMenu();
                }


                DrawBlendGraphics();
                DoEdgeControls();


                if (e.rawType == EventType.MouseUp)
                {
                    if (editor.interactingClip != null)
                    {
                        editor.interactingClip.EndClipAdjust();
                        editor.interactingClip.ResetInteraction();
                        editor.interactingClip = null;
                    }
                }

                if (e.button == 0)
                {
                    GUI.DragWindow(dragRect);
                }

                //Draw info text if big enough
                if (rect.width > 20)
                {
                    var r = new Rect(0, 0, rect.width, rect.height);
                    if (overlapIn > 0)
                    {
                        r.xMin = blendInPosX;
                    }

                    if (overlapOut > 0)
                    {
                        r.xMax = blendOutPosX;
                    }

                    var label = string.Format("<size=10>{0}</size>", action.info);
                    GUI.color = Color.black;
                    GUI.Label(r, label);
                    GUI.color = Color.white;
                }
            }

            //blend graphics
            void DrawBlendGraphics()
            {
                if (action.blendIn > 0)
                {
                    Handles.color = Color.black.WithAlpha(0.5f);
                    Handles.DrawAAPolyLine(2, new Vector2(0, rect.height), new Vector2(blendInPosX, 0));
                    Handles.color = Color.black.WithAlpha(0.3f);
                    Handles.DrawAAConvexPolygon(new Vector3(0, 0), new Vector3(0, rect.height),
                        new Vector3(blendInPosX, 0));
                }

                if (action.blendOut > 0 && overlapOut == 0)
                {
                    Handles.color = Color.black.WithAlpha(0.5f);
                    Handles.DrawAAPolyLine(2, new Vector2(blendOutPosX, 0), new Vector2(rect.width, rect.height));
                    Handles.color = Color.black.WithAlpha(0.3f);
                    Handles.DrawAAConvexPolygon(new Vector3(rect.width, 0), new Vector2(blendOutPosX, 0),
                        new Vector2(rect.width, rect.height));
                }

                if (overlapIn > 0)
                {
                    Handles.color = Color.black;
                    Handles.DrawAAPolyLine(2, new Vector2(blendInPosX, 0), new Vector2(blendInPosX, rect.height));
                }

                Handles.color = Color.white;
            }

            //clip scale/blend in/out controls
            void DoEdgeControls()
            {
                var canBlendIn = action.CanBlendIn() && action.length > 0;
                var canBlendOut = action.CanBlendOut() && action.length > 0;
                if (!isScalingStart && !isScalingEnd && !isControlingBlendIn && !isControlingBlendOut)
                {
                    if (allowScale || canBlendIn)
                    {
                        if (controlRectIn.Contains(e.mousePosition))
                        {
                            GUI.BringWindowToFront(windowID);
                            GUI.DrawTexture(controlRectIn.ExpandBy(0, -2), whiteTexture);
                            if (e.type == EventType.MouseDown && e.button == 0)
                            {
                                if (allowScale && !e.control)
                                {
                                    isScalingStart = true;
                                }

                                if (canBlendIn && e.control)
                                {
                                    isControlingBlendIn = true;
                                }

                                BeginClipAdjust();
                                e.Use();
                            }
                        }
                    }

                    if (allowScale || canBlendOut)
                    {
                        if (controlRectOut.Contains(e.mousePosition))
                        {
                            GUI.BringWindowToFront(windowID);
                            GUI.DrawTexture(controlRectOut.ExpandBy(0, -2), whiteTexture);
                            if (e.type == EventType.MouseDown && e.button == 0)
                            {
                                if (allowScale && !e.control)
                                {
                                    isScalingEnd = true;
                                }

                                if (canBlendOut && e.control)
                                {
                                    isControlingBlendOut = true;
                                }

                                BeginClipAdjust();
                                e.Use();
                            }
                        }
                    }
                }

                if (isControlingBlendIn)
                {
                    action.blendIn = Mathf.Clamp(pointerTime - action.startTime, 0, action.length - action.blendOut);
                }

                if (isControlingBlendOut)
                {
                    action.blendOut = Mathf.Clamp(action.endTime - pointerTime, 0, action.length - action.blendIn);
                }

                if (isScalingStart)
                {
                    var prevTime = previousClip != null ? previousClip.endTime : 0;
                    //magnet snap
                    if (Prefs.magnetSnapping && !e.control)
                    {
                        var snapStart = editor.MagnetSnapTime(snapedPointerTime, editor.magnetSnapTimesCache);
                        if (snapStart != null)
                        {
                            snapedPointerTime = snapStart.Value;
                            editor.pendingGuides.Add(new GuideLine(snapedPointerTime, Color.white));
                        }
                    }

                    if (action.CanCrossBlend(previousClip))
                    {
                        prevTime -= Mathf.Min(action.length / 2, previousClip.length / 2);
                    }

                    action.startTime = snapedPointerTime;
                    action.startTime = Mathf.Clamp(action.startTime, prevTime, preScaleEndTime);
                    action.endTime = preScaleEndTime;

                    UpdateClipAdjustContents();
                }

                if (isScalingEnd)
                {
                    var nextTime = nextClip != null ? nextClip.startTime : editor.maxTime;
                    //magnet snap
                    if (Prefs.magnetSnapping && !e.control)
                    {
                        var snapEnd = editor.MagnetSnapTime(snapedPointerTime, editor.magnetSnapTimesCache);
                        if (snapEnd != null)
                        {
                            snapedPointerTime = snapEnd.Value;
                            editor.pendingGuides.Add(new GuideLine(snapedPointerTime, Color.white));
                        }
                    }

                    if (action.CanCrossBlend(nextClip))
                    {
                        nextTime += Mathf.Min(action.length / 2, nextClip.length / 2);
                    }

                    action.endTime = snapedPointerTime;
                    action.endTime = Mathf.Clamp(action.endTime, 0, nextTime);

                    UpdateClipAdjustContents();
                }
            }


            //store pre adjust values
            public void BeginClipAdjust()
            {
                preScaleStartTime = action.startTime;
                preScaleEndTime = action.endTime;
                preScaleKeys = action.GetCurvesAll().ToDictionary(k => k, k => k.keys);
                if (action is ISubClipContainable)
                {
                    preScaleSubclipOffset = (action as ISubClipContainable).subClipOffset;
                    preScaleSubclipSpeed = (action as ISubClipContainable).subClipSpeed;
                }

                editor.CacheMagnetSnapTimes(action);
            }

            //retime keys lerp between start/end time.
            public void UpdateClipAdjustContents()
            {
                if (preScaleKeys == null)
                {
                    return;
                }

                var retime = Event.current.control;
                var trim = !Event.current.shift && !retime;

                foreach (var curve in action.GetCurvesAll())
                {
                    for (var i = 0; i < curve.keys.Length; i++)
                    {
                        var preKey = preScaleKeys[curve][i];

                        if (retime)
                        {
                            var preLength = preScaleEndTime - preScaleStartTime;
                            var newTime = Mathf.LerpUnclamped(0, action.length, preKey.time / preLength);
                            preKey.time = newTime;
                        }

                        if (trim)
                        {
                            preKey.time -= action.startTime - preScaleStartTime;
                        }

                        curve.MoveKey(i, preKey);
                    }

                    curve.UpdateTangentsFromMode();
                }

                CutsceneUtility.RefreshAllAnimationEditorsOf(action.animationData);

                if (action is ISubClipContainable)
                {
                    if (trim)
                    {
                        var subClip = (ISubClipContainable) action;
                        var delta = preScaleStartTime - action.startTime;
                        var newOffset = preScaleSubclipOffset + delta;
                        subClip.subClipOffset = newOffset;
                    }
                }
            }

            //flush pre adjust values
            public void EndClipAdjust()
            {
                preScaleKeys = null;
                if (Prefs.autoCleanKeysOffRange)
                {
                    CleanKeysOffRange();
                }
            }


            ///Split the clip in two, at specified local time
            public ActionClip Split(float time)
            {
                if (!action.IsTimeWithinClip(time))
                {
                    return null;
                }

                if (hasParameters)
                {
                    foreach (var param in action.animationData.animatedParameters)
                    {
                        if (param.HasAnyKey())
                        {
                            param.TryKeyIdentity(action.ToLocalTime(time));
                        }
                    }
                }

                CutsceneUtility.CopyClip(action);
                var copy = CutsceneUtility.PasteClip((CutsceneTrack) action.parent, time);
                copy.startTime = time;
                copy.endTime = action.endTime;
                action.endTime = time;
                copy.blendIn = 0;
                action.blendOut = 0;
                CutsceneUtility.selectedObject = null;
                CutsceneUtility.FlushCopy();

                var delta = action.length;
                if (hasParameters)
                {
                    foreach (var curve in copy.GetCurvesAll())
                    {
                        curve.OffsetCurveTime(-delta);
                        curve.RemoveNegativeKeys();
                    }

                    CutsceneUtility.RefreshAllAnimationEditorsOf(action.animationData);
                }

                if (copy is ISubClipContainable)
                {
                    (copy as ISubClipContainable).subClipOffset -= delta;
                }

                return copy;
            }

            ///Scale clip to fit previous and next
            public void StretchFit()
            {
                var wasStartTime = action.startTime;
                var wasEndTime = action.endTime;
                var targetStart = previousClip != null ? previousClip.endTime : action.parent.startTime;
                var targetEnd = nextClip != null ? nextClip.startTime : action.parent.endTime;
                if (previousClip == null || previousClip.endTime < action.startTime)
                {
                    action.startTime = targetStart;
                    action.endTime = wasEndTime;
                }

                if (nextClip == null || nextClip.startTime > action.endTime)
                {
                    action.endTime = targetEnd;
                }

                var delta = wasStartTime - action.startTime;
                if (hasParameters)
                {
                    foreach (var curve in action.GetCurvesAll())
                    {
                        curve.OffsetCurveTime(delta);
                    }

                    CutsceneUtility.RefreshAllAnimationEditorsOf(action.animationData);
                }

                if (action is ISubClipContainable)
                {
                    (action as ISubClipContainable).subClipOffset += delta;
                }
            }

            ///Clean keys off clip range after adding a key at 0 and length if there is any key outside that range
            public void CleanKeysOffRange()
            {
                if (hasParameters)
                {
                    foreach (var param in action.animationData.animatedParameters)
                    {
                        if (param.HasAnyKey())
                        {
                            if (param.GetKeyPrevious(0) < 0)
                            {
                                param.TryKeyIdentity(0);
                            }

                            if (param.GetKeyNext(action.length) > action.length)
                            {
                                param.TryKeyIdentity(action.length);
                            }
                        }
                    }

                    foreach (var curve in action.GetCurvesAll())
                    {
                        curve.RemoveKeysOffRange(0, action.length);
                        curve.UpdateTangentsFromMode();
                    }

                    CutsceneUtility.RefreshAllAnimationEditorsOf(action.animationData);
                }
            }

            //Show the clip dopesheet
            void ShowClipDopesheet(Rect rect)
            {
                var dopeRect = new Rect(0, rect.height - CLIP_DOPESHEET_HEIGHT, rect.width, CLIP_DOPESHEET_HEIGHT);
                GUI.color = isProSkin ? new Color(0, 0.2f, 0.2f, 0.5f) : new Color(0, 0.8f, 0.8f, 0.5f);
                GUI.Box(dopeRect, string.Empty, Slate.Styles.clipBoxHorizontalStyle);
                GUI.color = Color.white;
                DopeSheetEditor.DrawDopeSheet(action.animationData, action, dopeRect, 0, action.length, false);
            }

            //CONTEXT
            void DoClipContextMenu()
            {
                var menu = new GenericMenu();
                if (multiSelection != null && multiSelection.Contains(this))
                {
                    menu.AddItem(new GUIContent("Delete Clips"), false, () =>
                    {
                        editor.SafeDoAction(() =>
                        {
                            foreach (var act in multiSelection.Select(b => b.action).ToArray())
                            {
                                (act.parent as CutsceneTrack).DeleteAction(act);
                            }

                            editor.InitClipWrappers();
                            multiSelection = null;
                        });
                    });

                    menu.ShowAsContext();
                    e.Use();
                    return;
                }

                menu.AddItem(new GUIContent("Copy Clip"), false, () => { CutsceneUtility.CopyClip(action); });
                menu.AddItem(new GUIContent("Cut Clip"), false, () => { CutsceneUtility.CutClip(action); });

                if (allowScale)
                {
                    menu.AddItem(new GUIContent("Fit Clip (F)"), false, () => { StretchFit(); });
                    if (action.length > 0)
                    {
                        menu.AddItem(new GUIContent("Split At Cursor"), false, () => { Split(snapedPointerTime); });
                        menu.AddItem(new GUIContent("Split At Scrubber (S)"), false,
                            () => { Split(editor.cutscene.currentTime); });
                    }
                }

                if (hasParameters)
                {
                    menu.AddItem(new GUIContent("Key At Cursor"), false,
                        () => { action.TryAddIdentityKey(action.ToLocalTime(snapedPointerTime)); });
                    menu.AddItem(new GUIContent("Key At Scrubber (K)"), false,
                        () => { action.TryAddIdentityKey(action.RootTimeToLocalTime()); });
                }

                menu.AddSeparator("/");

                if (hasActiveParameters)
                {
                    menu.AddItem(new GUIContent("Clean Keys Off-Range (C)"), false, () => { CleanKeysOffRange(); });
                    menu.AddItem(new GUIContent("Remove Animation"), false, () =>
                    {
                        if (EditorUtility.DisplayDialog("Remove Animation",
                            "All Animation Curve keys of all animated parameters for this clip will be removed.\nAre you sure?",
                            "Yes", "No"))
                        {
                            editor.SafeDoAction(() => { action.ResetAnimatedParameters(); });
                        }
                    });
                }

                menu.AddItem(new GUIContent("Delete Clip"), false, () =>
                {
                    editor.SafeDoAction(() =>
                    {
                        (action.parent as CutsceneTrack).DeleteAction(action);
                        editor.InitClipWrappers();
                    });
                });

                menu.ShowAsContext();
                e.Use();
            }
        }
    }
}

#endif