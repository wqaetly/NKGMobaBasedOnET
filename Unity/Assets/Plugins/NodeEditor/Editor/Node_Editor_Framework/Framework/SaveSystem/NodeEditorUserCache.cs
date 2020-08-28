#if UNITY_EDITOR
#define CACHE
#endif

using System;
using System.IO;
using UnityEngine;
using NodeEditorFramework.Utilities;
using UnityEditor;

namespace NodeEditorFramework
{
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
                NewNodeCanvas();
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
            NodeEditor.RepaintClients();
        }

        /// <summary>
        /// Loads the mainNodeCanvas and it's associated mainEditorState from an asset at path
        /// </summary>
        public void LoadNodeCanvas(string path)
        {
            // Try to load the NodeCanvas
            if (!File.Exists(path) || (nodeCanvas = NodeEditorSaveManager.LoadNodeCanvas(path)) == null)
            {
                NewNodeCanvas();
                return;
            }

            editorState = NodeEditorSaveManager.ExtractEditorState(nodeCanvas, MainEditorStateIdentifier);

            openedCanvasPath = path;
            nodeCanvas.Validate();
            UpdateCanvasInfo();
            nodeCanvas.TraverseAll();
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

            openedCanvasPath = "";
            UpdateCanvasInfo();
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
        }

        #endregion
    }
}