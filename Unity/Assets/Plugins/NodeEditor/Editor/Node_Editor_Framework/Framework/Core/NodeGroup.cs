using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;
using ETModel;
using NodeEditorFramework.Utilities;
using Sirenix.OdinInspector;
using UnityEditor;

namespace NodeEditorFramework
{
    /// <summary>
    /// A NodeGroup on the canvas that handles node and subgroup pinning and syncing along with functionality to manipulate and customize the group
    /// </summary>
    [Serializable]
    public class NodeGroup
    {
        /// <summary>
        /// Represents selected borders as a flag in order to support corners
        /// </summary>
        [Flags]
        enum BorderSelection: byte
        {
            None = 0,
            Left = 1,
            Right = 2,
            Top = 4,
            Bottom = 8
        };

        public Rect rect;
        public string title;

        public Color color
        {
            get
            {
                return _color;
            }
            set
            {
                _color = value;
                GenerateStyles();
            }
        }

        [SerializeField]
        private Color _color;

        internal bool isClipped;

        private static int s_HeaderHeight = 30;

        [LabelText("字体大小")]
        public int TextSize = 25;

        // Resizing and dragging state for active node group
        private static BorderSelection resizeDir;

        [NonSerialized]
        public List<Node> pinnedNodes = new List<Node>();

        [NonSerialized]
        private List<NodeGroup> pinnedGroups = new List<NodeGroup>();

        // Settings
        /// <summary>
        /// 对Group进行缩放时高亮条宽度
        /// </summary>
        private const int borderWidth = 15;

        private const int minGroupSize = 150;

        // Accessors
        private Rect headerRect
        {
            get
            {
                return new Rect(rect.x, rect.y, rect.width, s_HeaderHeight);
            }
        }

        private Rect bodyRect
        {
            get
            {
                return new Rect(rect.x, rect.y + s_HeaderHeight, rect.width, rect.height);
            }
        }

        internal Rect fullAABBRect
        {
            get
            {
                return new Rect(rect.x, rect.y, rect.width, rect.height);
            }
        }

        /// <summary>
        /// Silently creates a NodeGroup
        /// </summary>
        internal NodeGroup()
        {
        }

        /// <summary>
        /// Creates a new NodeGroup with the specified title at pos and adds it to the current canvas
        /// </summary>
        public NodeGroup(string groupTitle, Vector2 pos)
        {
            title = groupTitle;
            rect = new Rect(pos.x, pos.y, 400, 400);
            GenerateStyles();
            _color = RandomHelper.RandColor();
            NodeEditor.curNodeCanvas.groups.Add(this);
            UpdateGroupOrder();
        }

        /// <summary>
        /// Deletes this NodeGroup
        /// </summary>
        public void Delete()
        {
            NodeEditor.curNodeCanvas.groups.Remove(this);
        }

        #region Group Functionality

        /// <summary>
        /// Update pinned nodes and subGroups of this NodeGroup
        /// </summary>
        private void UpdatePins()
        {
            pinnedGroups = new List<NodeGroup>();
            foreach (NodeGroup group in NodeEditor.curNodeCanvas.groups)
            {
                // Get all pinned groups -> all groups atleast half in the group
                if (group != this && rect.Contains(group.headerRect.center))
                    pinnedGroups.Add(group);
            }

            pinnedNodes = new List<Node>();
            foreach (Node node in NodeEditor.curNodeCanvas.nodes)
            {
                // Get all pinned nodes -> all nodes atleast half in the group
                if (rect.Contains(node.rect.center))
                    pinnedNodes.Add(node);
            }
        }

        /// <summary>
        /// Updates the group order by their sizes for better input handling
        /// </summary>
        private static void UpdateGroupOrder()
        {
            foreach (NodeGroup group in NodeEditor.curNodeCanvas.groups)
                group.UpdatePins();
            //NodeEditor.curNodeCanvas.groups.Sort((x, y) => -x.pinnedGroups.Count.CompareTo(y.pinnedGroups.Count));
            NodeEditor.curNodeCanvas.groups = NodeEditor.curNodeCanvas.groups
                    .OrderByDescending((x) => x.pinnedGroups.Count) // Order by pin hierarchy level
                    .ThenByDescending((x) => x.rect.size.x * x.rect.size.y) // Incase of equal level, prefer smaller groups
                    .ToList();
        }

        #endregion

        #region GUI

        /// <summary>
        /// GroupBody风格
        /// </summary>
        [NonSerialized]
        private GUIStyle backgroundStyle;

        /// <summary>
        /// GroupHeader风格
        /// </summary>
        [NonSerialized]
        private GUIStyle altBackgroundStyle;

        /// <summary>
        /// 进行Group缩放时的颜色风格
        /// </summary>
        [NonSerialized]
        private GUIStyle opBackgroundStyle;

        //		[NonSerialized]
        //		private GUIStyle dragHandleStyle;
        [NonSerialized]
        private GUIStyle headerTitleStyle;

        /// <summary>
        /// Generates all the styles for this node group based of the color
        /// </summary>
        private void GenerateStyles()
        {
            // Transparent background
            Texture2D background = RTEditorGUI.ColorToTex(8, color * new Color(1, 1, 1, 0.4f));
            // lighter, less transparent background
            Texture2D altBackground = RTEditorGUI.ColorToTex(8, color * new Color(1, 1, 1, 0.6f));
            // nearly opaque background
            Texture2D opBackground = RTEditorGUI.ColorToTex(8, color * new Color(1, 1, 1, 0.9f));

            backgroundStyle = new GUIStyle();
            backgroundStyle.normal.background = background;
            backgroundStyle.padding = new RectOffset(10, 10, 0, 0);

            altBackgroundStyle = new GUIStyle();
            altBackgroundStyle.normal.background = altBackground;
            altBackgroundStyle.padding = new RectOffset(10, 10, 0, 0);

            opBackgroundStyle = new GUIStyle();
            opBackgroundStyle.normal.background = opBackground;
            opBackgroundStyle.padding = new RectOffset(10, 10, 5, 5);

            headerTitleStyle = new GUIStyle();
            headerTitleStyle.fontSize = this.TextSize;
            headerTitleStyle.normal.textColor = Color.white;
        }

        /// <summary>
        /// Draws the NodeGroup
        /// </summary>
        public void DrawGroup()
        {
            if (backgroundStyle == null)
                GenerateStyles();
            NodeEditorState state = NodeEditor.curEditorState;
            // Create a rect that is adjusted to the editor zoom
            Rect groupRect = rect;
            groupRect.position += state.zoomPanAdjust + state.panOffset;

            if (state.activeGroup == this && state.resizeGroup)
            {
                // Highlight the currently resized border
                Rect handleRect = getBorderRect(this.bodyRect, NodeGroup.resizeDir);
                handleRect.position += state.zoomPanAdjust + state.panOffset;
                GUI.Box(handleRect, GUIContent.none, opBackgroundStyle);
            }

            // Header
            Rect groupHeaderRect = headerRect;
            groupHeaderRect.position += state.zoomPanAdjust + state.panOffset;

            // Body
            Rect groupBodyRect = bodyRect;
            groupBodyRect.position += state.zoomPanAdjust + state.panOffset;
            GUI.Box(groupBodyRect, GUIContent.none, backgroundStyle);

            GUILayout.BeginArea(groupHeaderRect, altBackgroundStyle);
            GUILayout.BeginHorizontal();

            GUILayout.Space(8);

            // Header Title
            title = EditorGUILayout.TextField(title, headerTitleStyle, GUILayout.MinWidth(40));

            // Header Color Edit
            color = this._color;

            GUILayout.FlexibleSpace();

            GUILayout.EndHorizontal();
            GUILayout.EndArea();
        }

        #endregion

        #region Helpers and Hit Tests

        /// <summary>
        /// Gets a NodeGroup which has it's header under the mouse. If multiple groups are adressed, the smallest is returned.
        /// </summary>
        public static NodeGroup HeaderAtPosition(NodeEditorState state, Vector2 canvasPos)
        {
            if (NodeEditorInputSystem.shouldIgnoreInput(state))
                return null;
            for (int i = state.canvas.groups.Count - 1; i >= 0; i--)
            {
                NodeGroup group = state.canvas.groups[i];
                if (group.headerRect.Contains(canvasPos))
                    return group;
            }

            return null;
        }

        /// <summary>
        /// Gets a NodeGroup under the mouse. If multiple groups are adressed, the group lowest in the pin hierarchy is returned.
        /// </summary>
        public static NodeGroup GroupAtPosition(NodeEditorState state, Vector2 canvasPos)
        {
            if (NodeEditorInputSystem.shouldIgnoreInput(state))
                return null;
            for (int i = state.canvas.groups.Count - 1; i >= 0; i--)
            {
                NodeGroup group = state.canvas.groups[i];
                if (group.headerRect.Contains(canvasPos) || group.rect.Contains(canvasPos))
                    return group;
            }

            return null;
        }

        /// <summary>
        /// Gets a NodeGroup under the mouse that requires input (header or border). If multiple groups are adressed, the group lowest in the pin hierarchy is returned.
        /// 获取鼠标点击的需要执行操作的Group（Header或Border），如果找到了多个group，则只处理第一个group
        /// </summary>
        private static NodeGroup GroupAtPositionInput(NodeEditorState state, Vector2 canvasPos)
        {
            if (NodeEditorInputSystem.shouldIgnoreInput(state))
                return null;
            for (int i = state.canvas.groups.Count - 1; i >= 0; i--)
            {
                NodeGroup group = state.canvas.groups[i];
                BorderSelection border;
                if (group.headerRect.Contains(canvasPos) || CheckBorderSelection(state, group.bodyRect, canvasPos, out border))
                    return group;
            }

            return null;
        }

        /// <summary>
        /// Returns true if the mouse position is on the border of the focused node and outputs the border as a flag in selection
        /// 如果鼠标选中边界Border，就返回true，并且改变BorderSelection的值
        /// 这里的rect是整个Group
        /// </summary>
        private static bool CheckBorderSelection(NodeEditorState state, Rect rect, Vector2 canvasPos, out BorderSelection selection)
        {
            selection = 0;
            if (!rect.Contains(canvasPos))
                return false;

            //这里的min和max是指Top，Right为Max，Bottom，Left为Min（你呀，总是能给我整出点新花样）
            Vector2 min = new Vector2(rect.xMin + borderWidth, rect.yMax - borderWidth);
            Vector2 max = new Vector2(rect.xMax - borderWidth, rect.yMin + borderWidth);

            // Check bordes and mark flags accordingly
            if (canvasPos.x < min.x)
                selection = BorderSelection.Left;
            else if (canvasPos.x > max.x)
                selection = BorderSelection.Right;

            if (canvasPos.y < max.y)
                selection |= BorderSelection.Top;
            else if (canvasPos.y > min.y)
                selection |= BorderSelection.Bottom;

            return selection != BorderSelection.None;
        }

        /// <summary>
        /// Gets the rect that represents the passed border flag in the passed rect
        /// </summary>
        private static Rect getBorderRect(Rect rect, BorderSelection border)
        {
            Rect borderRect = rect;
            if ((border & BorderSelection.Left) != 0)
            {
                borderRect.xMax = borderRect.xMin + borderWidth;
            }

            else if ((border & BorderSelection.Right) != 0)
            {
                borderRect.xMin = borderRect.xMax - borderWidth;
            }

            if ((border & BorderSelection.Top) != 0)
            {
                borderRect.yMax = borderRect.yMin + borderWidth;
            }

            else if ((border & BorderSelection.Bottom) != 0)
            {
                borderRect.yMin = borderRect.yMax - borderWidth;
            }

            return borderRect;
        }

        #endregion

        #region Input

        /// <summary>
        /// Handles creation of the group in the editor through a context menu item
        /// </summary>
        [ContextEntryAttribute(ContextType.Canvas, "Create Group")]
        private static void CreateGroup(NodeEditorInputInfo info)
        {
            info.SetAsCurrentEnvironment();
            new NodeGroup("Group", NodeEditor.ScreenToCanvasSpace(info.inputPos));
        }

        /// <summary>
        /// Handles the group context click (on the header only)
        /// </summary>
        [EventHandlerAttribute(EventType.MouseDown, -1)] // Before the other context clicks because they won't account for groups
        private static void HandleGroupContextClick(NodeEditorInputInfo inputInfo)
        {
            NodeEditorState state = inputInfo.editorState;
            if (inputInfo.inputEvent.button == 1 && state.focusedNode == null)
            {
                // Right-click NOT on a node
                NodeGroup focusedGroup = HeaderAtPosition(state, NodeEditor.ScreenToCanvasSpace(inputInfo.inputPos));
                if (focusedGroup != null)
                {
                    // Context click for the group. This is static, not dynamic, because it would be useless
                    GenericMenu context = new GenericMenu();
                    context.AddItem(new GUIContent("Delete"), false, () =>
                    {
                        NodeEditor.curNodeCanvas = state.canvas;
                        focusedGroup.Delete();
                    });
                    context.ShowAsContext();
                    inputInfo.inputEvent.Use();
                }
            }
        }

        /// <summary>
        /// Starts a dragging operation for either dragging or resizing (on the header or borders only)
        /// </summary>
        [EventHandlerAttribute(EventType.MouseDown,
            10)] // Priority over hundred to make it call after the GUI, and before Node dragging (110) and window panning (105)
        private static void HandleGroupDraggingStart(NodeEditorInputInfo inputInfo)
        {
            NodeEditorState state = inputInfo.editorState;
            if (inputInfo.inputEvent.button == 0)
            {
                // Do not interfere with other dragging stuff
                NodeGroup focusedGroup = GroupAtPositionInput(state, NodeEditor.ScreenToCanvasSpace(inputInfo.inputPos));
                if (focusedGroup != null)
                {
                    // Start dragging the focused group
                    UpdateGroupOrder();
                    Vector2 canvasInputPos = NodeEditor.ScreenToCanvasSpace(inputInfo.inputPos);
                    if (CheckBorderSelection(state, focusedGroup.bodyRect, canvasInputPos, out NodeGroup.resizeDir))
                    {
                        // Resizing
                        state.activeGroup = focusedGroup;
                        // Get start drag position
                        Vector2 startSizePos = Vector2.zero;
                        if ((NodeGroup.resizeDir & BorderSelection.Left) != 0)
                            startSizePos.x = focusedGroup.rect.xMin;
                        else if ((NodeGroup.resizeDir & BorderSelection.Right) != 0)
                            startSizePos.x = focusedGroup.rect.xMax;
                        if ((NodeGroup.resizeDir & BorderSelection.Top) != 0)
                            startSizePos.y = focusedGroup.rect.yMin;
                        else if ((NodeGroup.resizeDir & BorderSelection.Bottom) != 0)
                            startSizePos.y = focusedGroup.rect.yMax;
                        // Start the resize drag
                        state.StartDrag("group", inputInfo.inputPos, startSizePos);
                        state.resizeGroup = true;
                    }
                    else if (focusedGroup.headerRect.Contains(canvasInputPos))
                    {
                        // Dragging
                        state.activeGroup = focusedGroup;
                        state.StartDrag("group", inputInfo.inputPos, state.activeGroup.rect.position);
                        state.activeGroup.UpdatePins();
                    }
                }
            }
        }

        /// <summary>
        /// Updates the dragging operation for either dragging or resizing
        /// </summary>
        [EventHandlerAttribute(EventType.MouseDrag, 10)]
        private static void HandleGroupDragging(NodeEditorInputInfo inputInfo)
        {
            NodeEditorState state = inputInfo.editorState;
            NodeGroup active = state.activeGroup;
            if (active != null)
            {
                // Handle dragging and resizing of active group
                if (state.dragUserID != "group")
                {
                    state.activeGroup = null;
                    state.resizeGroup = false;
                    return;
                }

                // Update drag operation
                Vector2 dragChange = state.UpdateDrag("group", inputInfo.inputPos);
                Vector2 newSizePos = state.dragObjectPos;
                if (state.resizeGroup)
                {
                    // Resizing -> Apply drag to selected borders while keeping a minimum size
                    // Note: Binary operator and !=0 checks of the flag is enabled, but not necessarily the only flag (in which case you would use ==)
                    Rect r = active.rect;
                    if ((NodeGroup.resizeDir & BorderSelection.Left) != 0)
                        active.rect.xMin = r.xMax - Math.Max(minGroupSize, r.xMax - newSizePos.x);
                    else if ((NodeGroup.resizeDir & BorderSelection.Right) != 0)
                        active.rect.xMax = r.xMin + Math.Max(minGroupSize, newSizePos.x - r.xMin);

                    if ((NodeGroup.resizeDir & BorderSelection.Top) != 0)
                        active.rect.yMin = r.yMax - Math.Max(minGroupSize, r.yMax - newSizePos.y);
                    else if ((NodeGroup.resizeDir & BorderSelection.Bottom) != 0)
                        active.rect.yMax = r.yMin + Math.Max(minGroupSize, newSizePos.y - r.yMin);
                }
                else
                {
                    // Dragging -> Apply drag to body and pinned nodes
                    active.rect.position = newSizePos;
                    foreach (Node pinnedNode in active.pinnedNodes)
                        pinnedNode.position += dragChange;
                    foreach (NodeGroup pinnedGroup in active.pinnedGroups)
                        pinnedGroup.rect.position += dragChange;
                }

                Event.current.Use();
                NodeEditor.RepaintClients();
            }
        }

        /// <summary>
        /// Ends the dragging operation for either dragging or resizing
        /// </summary>
        [EventHandlerAttribute(EventType.MouseUp, 10)]
        private static void HandleDraggingEnd(NodeEditorInputInfo inputInfo)
        {
            if (inputInfo.editorState.dragUserID == "group")
            {
                //				if (inputInfo.editorState.activeGroup != null )
                //					inputInfo.editorState.activeGroup.UpdatePins ();
                inputInfo.editorState.EndDrag("group");
                NodeEditor.RepaintClients();
            }

            UpdateGroupOrder();
            inputInfo.editorState.activeGroup = null;
            inputInfo.editorState.resizeGroup = false;
        }

        #endregion
    }
}