using UnityEngine;
using NodeEditorFramework.Utilities;
using UnityEditor;

namespace NodeEditorFramework
{
    public enum ConnectionDrawMethod
    {
        Bezier,
        StraightLine
    }

    public static partial class NodeEditorGUI
    {
        internal static bool isEditorWindow;

        private const string c_GUISkinPath = "Assets/Plugins/NodeEditor/Editor/Node_Editor_Framework/Resources/GUISkin/NodeEditorSkin.guiskin";

        // static GUI settings, textures and styles
        public static int knobSize = 16;

        private static ConnectionDrawMethod s_ConnectionDrawMethod = ConnectionDrawMethod.StraightLine;

        public static GUISkin nodeSkin;

        public static GUIStyle nodeTittleCentered;

        public static GUIStyle nodeLeftPort;
        public static GUIStyle nodeRightLabel;

        public static GUIStyle nodeBox;
        public static GUIStyle nodeBox_HighLightOutLine;

        public static GUIStyle toolbar;
        public static GUIStyle toolbarLabel;
        public static GUIStyle toolbarDropdown;
        public static GUIStyle toolbarButton;

        public static bool Init()
        {
            // Skin & Styles
            nodeSkin = AssetDatabase.LoadAssetAtPath<GUISkin>(c_GUISkinPath);
            
            //设置字体大小
            foreach (GUIStyle style in GUI.skin)
            {
                style.fontSize = 12;
                //style.normal.textColor = style.active.textColor = style.focused.textColor = style.hover.textColor = NE_TextColor;
            }
            
            nodeTittleCentered = nodeSkin.GetStyle("Node_TittleCentered");
            nodeLeftPort = nodeSkin.GetStyle("Node_LeftPort");;
            nodeRightLabel = nodeSkin.GetStyle("Node_RightPort");

            nodeBox = nodeSkin.box;
            nodeBox_HighLightOutLine = nodeSkin.GetStyle("Node_HighLightOutLine");

            // Toolbar
            toolbar = GUI.skin.FindStyle("toolbar");
            toolbarButton = GUI.skin.FindStyle("toolbarButton");
            toolbarLabel = GUI.skin.FindStyle("toolbarButton");
            toolbarDropdown = GUI.skin.FindStyle("toolbarDropdown");
            
            return true;
        }

        public static void StartNodeGUI(bool IsEditorWindow)
        {
            NodeEditor.checkInit(true);

            isEditorWindow = IsEditorWindow;
        }

        public static void EndNodeGUI()
        {

        }

        #region Connection Drawing

        // Curve parameters
        public static float curveBaseDirection = 1.5f, curveBaseStart = 2f, curveDirectionScale = 0.004f;

        /// <summary>
        /// Draws a node connection from start to end, horizontally
        /// </summary>
        public static void DrawConnection(Vector2 startPos, Vector2 endPos, Color col)
        {
            Vector2 startVector = startPos.x <= endPos.x? Vector2.right : Vector2.left;
            DrawConnection(startPos, startVector, endPos, -startVector, col);
        }

        /// <summary>
        /// Draws a node connection from start to end, horizontally
        /// </summary>
        public static void DrawConnection(Vector2 startPos, Vector2 endPos, ConnectionDrawMethod drawMethod, Color col)
        {
            Vector2 startVector = startPos.x <= endPos.x? Vector2.right : Vector2.left;
            DrawConnection(startPos, startVector, endPos, -startVector, drawMethod, col);
        }

        /// <summary>
        /// Draws a node connection from start to end with specified vectors
        /// </summary>
        public static void DrawConnection(Vector2 startPos, Vector2 startDir, Vector2 endPos, Vector2 endDir, Color col)
        {
            DrawConnection(startPos, startDir, endPos, endDir, s_ConnectionDrawMethod, col);
        }

        /// <summary>
        /// Draws a node connection from start to end with specified vectors
        /// </summary>
        public static void DrawConnection(Vector2 startPos, Vector2 startDir, Vector2 endPos, Vector2 endDir, ConnectionDrawMethod drawMethod,
        Color col)
        {
            if (drawMethod == ConnectionDrawMethod.Bezier)
            {
                NodeEditorGUI.OptimiseBezierDirections(startPos, ref startDir, endPos, ref endDir);
                float dirFactor = 80; //Mathf.Pow ((startPos-endPos).magnitude, 0.3f) * 20;
                //Debug.Log ("DirFactor is " + dirFactor + "with a bezier lenght of " + (startPos-endPos).magnitude);
                RTEditorGUI.DrawBezier(startPos, endPos, startPos + startDir * dirFactor, endPos + endDir * dirFactor, col, null,
                    NodeEditor.curEditorState.zoom * 3);
            }
            else if (drawMethod == ConnectionDrawMethod.StraightLine)
                RTEditorGUI.DrawLine(startPos, endPos, col, null, NodeEditor.curEditorState.zoom * 3);
        }

        /// <summary>
        /// Optimises the bezier directions scale so that the bezier looks good in the specified position relation.
        /// Only the magnitude of the directions are changed, not their direction!
        /// </summary>
        public static void OptimiseBezierDirections(Vector2 startPos, ref Vector2 startDir, Vector2 endPos, ref Vector2 endDir)
        {
            Vector2 offset = (endPos - startPos) * curveDirectionScale;
            float baseDir = Mathf.Min(offset.magnitude / curveBaseStart, 1) * curveBaseDirection;
            Vector2 scale = new Vector2(Mathf.Abs(offset.x) + baseDir, Mathf.Abs(offset.y) + baseDir);
            // offset.x and offset.y linearly increase at scale of curveDirectionScale
            // For 0 < offset.magnitude < curveBaseStart, baseDir linearly increases from 0 to curveBaseDirection. For offset.magnitude > curveBaseStart, baseDir = curveBaseDirection
            startDir = Vector2.Scale(startDir.normalized, scale);
            endDir = Vector2.Scale(endDir.normalized, scale);
        }

        /// <summary>
        /// Gets the second connection vector that matches best, accounting for positions
        /// </summary>
        internal static Vector2 GetSecondConnectionVector(Vector2 startPos, Vector2 endPos, Vector2 firstVector)
        {
            if (firstVector.x != 0 && firstVector.y == 0)
                return startPos.x <= endPos.x? -firstVector : firstVector;
            else if (firstVector.y != 0 && firstVector.x == 0)
                return startPos.y <= endPos.y? -firstVector : firstVector;
            else
                return -firstVector;
        }

        #endregion

        /// <summary>
        /// Unified method to generate a random HSV color value across versions
        /// </summary>
        public static Color RandomColorHSV(int seed, float hueMin, float hueMax, float saturationMin, float saturationMax, float valueMin,
        float valueMax)
        {
            // Set seed
#if UNITY_5_4_OR_NEWER
            UnityEngine.Random.InitState(seed);
#else
			UnityEngine.Random.seed = seed;
#endif
            // Consistent random H,S,V values
            float hue = UnityEngine.Random.Range(hueMin, hueMax);
            float saturation = UnityEngine.Random.Range(saturationMin, saturationMax);
            float value = UnityEngine.Random.Range(valueMin, valueMax);

            // Convert HSV to RGB
#if UNITY_5_3_OR_NEWER
            return UnityEngine.Color.HSVToRGB(hue, saturation, value, false);
#else
			int hi = Mathf.FloorToInt(hue / 60) % 6;
			float frac = hue / 60 - Mathf.Floor(hue / 60);

			float v = value;
			float p = value * (1 - saturation);
			float q = value * (1 - frac * saturation);
			float t = value * (1 - (1 - frac) * saturation);

			if (hi == 0)
				return new Color(v, t, p);
			else if (hi == 1)
				return new Color(q, v, p);
			else if (hi == 2)
				return new Color(p, v, t);
			else if (hi == 3)
				return new Color(p, q, v);
			else if (hi == 4)
				return new Color(t, p, v);
			else
				return new Color(v, p, q);
#endif
        }
    }
}