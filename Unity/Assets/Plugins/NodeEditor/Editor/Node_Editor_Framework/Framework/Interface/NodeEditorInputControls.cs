using UnityEngine;
using System;
using System.Collections.Generic;
using ETModel;
using NodeEditorFramework.Utilities.CreateNodesWindow;
using NPOI.SS.Formula.Functions;
using UnityEditor;

namespace NodeEditorFramework
{
    /// <summary>
    /// Collection of default Node Editor controls for the NodeEditorInputSystem
    /// </summary>
    public static class NodeEditorInputControls
    {
        #region Node Context Entries

        [ContextEntryAttribute(ContextType.Node, "Delete Node")]
        private static void DeleteNode(NodeEditorInputInfo inputInfo)
        {
            if (inputInfo.editorState.focusedNode != null)
            {
                List<Node> temp = new List<Node>();
                Undo.RecordObject(inputInfo.editorState.canvas, "NodeEditor_删除保存");

                foreach (var node in inputInfo.editorState.selectedNodes)
                {
                    temp.Add(node);
                }

                foreach (var node in temp)
                {
                    node.Delete();
                }

                inputInfo.editorState.selectedNodes.Clear();
                inputInfo.inputEvent.Use();
            }
        }

        [ContextEntryAttribute(ContextType.Node, "Duplicate Node")]
        private static void DuplicateNode(NodeEditorInputInfo inputInfo)
        {
            NodeEditorState state = inputInfo.editorState;
            if (state.focusedNode != null && state.selectedNodes.Count > 0 && state.canvas.CanAddNode(state.focusedNode.GetID))
            {
                //TODO 支持多个Node复制操作
                // Create new node of same type
                Node.Create(state.focusedNode.GetID, NodeEditor.ScreenToCanvasSpace(inputInfo.inputPos), state.canvas,
                    state.connectKnob);
                state.connectKnob = null;
                inputInfo.inputEvent.Use();
            }
        }

        [HotkeyAttribute(KeyCode.Delete, EventType.KeyUp)]
        private static void DeleteNodeKey(NodeEditorInputInfo inputInfo)
        {
            if (GUIUtility.keyboardControl > 0)
                return;
            if (inputInfo.editorState.focusedNode != null)
            {
                inputInfo.editorState.focusedNode.Delete();
                inputInfo.inputEvent.Use();
            }
        }

        #endregion

        #region 框选多个Node

        public static Vector2 startSelectionPos;

        [EventHandlerAttribute(EventType.MouseDown, 20)] // Priority over hundred to make it call after the GUI
        private static void HandleWindowSelectionStart(NodeEditorInputInfo inputInfo)
        {
            if (GUIUtility.hotControl > 0)
                return; // GUI has control

            NodeEditorState state = inputInfo.editorState;
            if (inputInfo.inputEvent.button == 0 && state.focusedNode == null &&
                NodeGroup.HeaderAtPosition(state, NodeEditor.ScreenToCanvasSpace(inputInfo.inputPos)) == null)
            {
                // Left clicked on the empty canvas -> Start the selection process
                state.boxSelecting = true;
                startSelectionPos = inputInfo.inputPos;
            }
        }

        [EventHandlerAttribute(EventType.MouseDrag,20)]
        private static void HandleWindowSelection(NodeEditorInputInfo inputInfo)
        {
            NodeEditorState state = inputInfo.editorState;
            if (state.boxSelecting)
            {
                NodeEditor.RepaintClients();
            }
        }
        
        [EventHandlerAttribute(EventType.MouseUp,20)]
        private static void HandleWindowSelectionEnd(NodeEditorInputInfo inputInfo)
        {
            if (inputInfo.editorState.boxSelecting)
            {
                inputInfo.editorState.boxSelecting = false;
                NodeEditor.RepaintClients();
            }
        }

        #endregion

        #region Node Dragging

        [EventHandlerAttribute(EventType.MouseDown, 30)] // Priority over hundred to make it call after the GUI
        private static void HandleNodeDraggingStart(NodeEditorInputInfo inputInfo)
        {
            NodeEditorState state = inputInfo.editorState;
            if (inputInfo.inputEvent.button == 0 && state.focusedNode != null)
            {
                // Clicked inside the selected Node, so start dragging it
                state.dragNode = true;
                state.StartDrag("node", inputInfo.inputPos, state.focusedNode.rect.position);
            }
        }

        [EventHandlerAttribute(EventType.MouseDrag,30)]
        private static void HandleNodeDragging(NodeEditorInputInfo inputInfo)
        {
            NodeEditorState state = inputInfo.editorState;
            if (state.dragNode)
            {
                // If conditions apply, drag the selected node, else disable dragging
                if (state.selectedNodes.Count > 0 && inputInfo.editorState.dragUserID == "node")
                {
                    // Apply new position for the dragged node
                    Vector2 newOffset = state.UpdateDrag("node", inputInfo.inputPos);
                    foreach (var node in state.selectedNodes)
                    {
                        node.position += newOffset;
                    }

                    NodeEditor.RepaintClients();
                }
                else
                    state.dragNode = false;
                Event.current.Use();
            }
        }
        
        [EventHandlerAttribute(EventType.MouseUp,30)]
        private static void HandleNodeDraggingEnd(NodeEditorInputInfo inputInfo)
        {
            if (inputInfo.editorState.dragUserID == "node")
            {
                Vector2 totalDrag = inputInfo.editorState.EndDrag("node");
                if (inputInfo.editorState.dragNode && inputInfo.editorState.selectedNodes.Count > 0)
                {
                    foreach (var node in inputInfo.editorState.selectedNodes)
                    {
                        node.position += inputInfo.editorState.dragOffset;
                        NodeEditorCallbacks.IssueOnMoveNode(node);
                    }
                }
                Event.current.Use();
            }

            inputInfo.editorState.dragNode = false;
        }

        #endregion

        #region Window Panning

        [EventHandlerAttribute(EventType.MouseDown, 40)] // Priority over hundred to make it call after the GUI
        private static void HandleWindowPanningStart(NodeEditorInputInfo inputInfo)
        {
            if (GUIUtility.hotControl > 0)
                return; // GUI has control

            NodeEditorState state = inputInfo.editorState;
            if (inputInfo.inputEvent.button == 2)
            {
                // Left- or Middle clicked on the empty canvas -> Start panning
                Event.current.Use();
                state.panWindow = true;
                state.StartDrag("window", inputInfo.inputPos, state.panOffset);
            }
        }

        [EventHandlerAttribute(EventType.MouseDrag,40)]
        private static void HandleWindowPanning(NodeEditorInputInfo inputInfo)
        {
            NodeEditorState state = inputInfo.editorState;
            if (state.panWindow)
            {
                // Calculate change in panOffset
                if (inputInfo.editorState.dragUserID == "window")
                    state.panOffset += state.UpdateDrag("window", inputInfo.inputPos);
                else
                    state.panWindow = false;

                NodeEditor.RepaintClients();
                Event.current.Use();
            }
        }
        
        [EventHandlerAttribute(EventType.MouseUp,40)]
        private static void HandleWindowPanningEnd(NodeEditorInputInfo inputInfo)
        {
            if (inputInfo.editorState.dragUserID == "window")
            {
                inputInfo.editorState.panOffset = inputInfo.editorState.EndDrag("window");
                Event.current.Use();
            }

            inputInfo.editorState.panWindow = false;
        }

        #endregion

        #region Connection

        private static bool CreateConnection = false;

        [EventHandlerAttribute(EventType.MouseDown,25)]
        private static void HandleConnectionDrawing(NodeEditorInputInfo inputInfo)
        {
            NodeEditorState state = inputInfo.editorState;
            if (inputInfo.inputEvent.button == 0 && state.focusedConnectionKnob != null)
            {
                // Left-Clicked on a ConnectionKnob, handle editing
                if (state.focusedConnectionKnob.direction == Direction.Out)
                {
                    // Knob with multiple connections clicked -> Draw new connection from it
                    state.connectKnob = state.focusedConnectionKnob;
                    CreateConnection = true;
                    inputInfo.inputEvent.Use();
                }
                else if (state.focusedConnectionKnob.direction == Direction.In)
                {
                    // Knob with single connection clicked
                    if (state.focusedConnectionKnob.connected())
                    {
                        // Loose and edit existing connection from it
                        state.connectKnob = state.focusedConnectionKnob.connection(0);
                        state.focusedConnectionKnob.RemoveConnection(state.connectKnob);
                        CreateConnection = false;
                        inputInfo.inputEvent.Use();
                    }
                    else
                    {
                        // Not connected, draw a new connection from it
                        state.connectKnob = state.focusedConnectionKnob;
                        CreateConnection = true;
                        inputInfo.inputEvent.Use();
                    }
                }
            }
        }

        [EventHandlerAttribute(EventType.MouseUp,25)]
        private static void HandleApplyConnection(NodeEditorInputInfo inputInfo)
        {
            NodeEditorState state = inputInfo.editorState;
            if (inputInfo.inputEvent.button == 0 && state.connectKnob != null)
            {
                if (state.focusedNode != null && state.focusedConnectionKnob != null &&
                    state.focusedConnectionKnob != state.connectKnob)
                {
                    // A connection curve was dragged and released onto a connection knob
                    state.focusedConnectionKnob.TryApplyConnection(state.connectKnob);
                }
                else if (CreateConnection)
                {
                    CreateConnection = false;
                    CreateNodesAdvancedDropdown.ShowDropdown(new Rect(inputInfo.inputPos, Vector2.zero), state.connectKnob);
                }

                inputInfo.inputEvent.Use();
            }

            state.connectKnob = null;
        }

        #endregion

        #region Zoom

        [EventHandlerAttribute(EventType.ScrollWheel,60)]
        private static void HandleZooming(NodeEditorInputInfo inputInfo)
        {
            inputInfo.editorState.zoom =
                    (float) Math.Round(Math.Min(4.0, Math.Max(0.6, inputInfo.editorState.zoom + inputInfo.inputEvent.delta.y / 15)), 2);
            NodeEditor.RepaintClients();
        }

        #endregion
    }
}