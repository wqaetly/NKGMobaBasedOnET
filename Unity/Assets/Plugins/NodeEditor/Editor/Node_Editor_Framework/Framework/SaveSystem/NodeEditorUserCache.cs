#if UNITY_EDITOR
#define CACHE
#endif

using System;
using System.IO;
using NodeEditorFramework.Standard;
using UnityEngine;
using NodeEditorFramework.Utilities;
using UnityEditor;

namespace NodeEditorFramework
{
    /// <summary>
    /// 用户缓存，包含nodeCanvas和editorState
    /// </summary>
    public class NodeEditorUserCache
    {
        public NodeCanvas nodeCanvas;
        public NodeEditorState editorState;
        public string openedCanvasPath = "";

        public Type defaultNodeCanvasType;
        public NodeCanvasTypeData typeData;

        private const string MainEditorStateIdentifier = "MainEditorState";

        #region Setup

        public NodeEditorUserCache()
        {
        }

        /// <summary>
        /// Assures a canvas is loaded, either from the cache or new
        /// </summary>
        public void AssureCanvas()
        {
            if (nodeCanvas == null)
            {
                LoadNodeCanvas(NodeEditorSaveManager.GetLastCanvasPath());
            }

            if (editorState == null)
                NewEditorState();
        }

        #endregion

        #region Save/Load

        /// <summary>
        /// Saves the mainNodeCanvas and it's associated mainEditorState as an asset at path
        /// </summary>
        public void SaveNodeCanvas(string path)
        {
            nodeCanvas.editorStates = new NodeEditorState[] { editorState };
            NodeEditorSaveManager.SaveNodeCanvas(path, ref nodeCanvas);
        }

        /// <summary>
        /// Loads the mainNodeCanvas and it's associated mainEditorState from an asset at path
        /// 加载一个Canvas
        /// </summary>
        public void LoadNodeCanvas(string path)
        {
            //如果路径一致，说明是同一个canvas
            if (NodeEditor.curEditorState != null && NodeEditor.curEditorState.canvas != null && (NodeEditor.curEditorState.canvas.savePath == path ||
                path.Contains(NodeEditor.curEditorState.canvas.savePath)) && (
                NodeEditorSaveManager.GetLastCanvasPath() == path || path.Contains(NodeEditorSaveManager.GetLastCanvasPath())))
            {
                this.nodeCanvas = NodeEditor.curEditorState.canvas;
                return;
            }

            //如果不存在路径，则新建一个DefaultCanvas
            if (!File.Exists(path) || (nodeCanvas = NodeEditorSaveManager.LoadNodeCanvas(path)) == null)
            {
                NewNodeCanvas();
                return;
            }

            editorState = NodeEditorSaveManager.ExtractEditorState(nodeCanvas, MainEditorStateIdentifier);
            nodeCanvas.Validate();
            UpdateCanvasInfo();
            NodeEditor.RepaintClients();
            Debug.Log($"加载{path}成功");
        }

        /// <summary>
        /// Creates and loads a new NodeCanvas
        /// </summary>
        public void NewNodeCanvas(Type canvasType = null)
        {
            canvasType = canvasType ?? defaultNodeCanvasType;
            nodeCanvas = NodeCanvas.CreateCanvas(canvasType);
            NewEditorState();
            UpdateCanvasInfo();
            LoadNodeCanvas(nodeCanvas.savePath);
        }

        /// <summary>
        /// Creates a new EditorState for the current NodeCanvas
        /// </summary>
        public void NewEditorState()
        {
            editorState = ScriptableObject.CreateInstance<NodeEditorState>();
            editorState.canvas = nodeCanvas;
            editorState.name = MainEditorStateIdentifier;
            nodeCanvas.editorStates = new NodeEditorState[] { editorState };

            UnityEditor.EditorUtility.SetDirty(nodeCanvas);
            NodeEditorSaveManager.AddSubAsset(editorState, nodeCanvas);
        }

        #endregion

        #region Utility

        private void UpdateCanvasInfo()
        {
            typeData = NodeCanvasManager.GetCanvasTypeData(nodeCanvas);
            openedCanvasPath = nodeCanvas.savePath;
            this.editorState.canvas = nodeCanvas;
            NodeEditor.curEditorState = this.editorState;
            NodeEditorSaveManager.SetLastCanvasPath(openedCanvasPath);
        }

        #endregion
    }
}