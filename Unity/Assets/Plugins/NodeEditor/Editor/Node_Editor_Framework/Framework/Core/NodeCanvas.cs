using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using Plugins.NodeEditor.Node_Editor.Default;
using Sirenix.OdinInspector;

namespace NodeEditorFramework
{
    /// <summary>
    /// Base class for all canvas types
    /// </summary>
    public abstract class NodeCanvas: SerializedScriptableObject
    {
        public virtual string canvasName
        {
            get
            {
                return "DEFAULT";
            }
        }

        public virtual bool allowRecursion
        {
            get
            {
                return false;
            }
        }

        [HideInEditorMode]
        public NodeEditorState[] editorStates = new NodeEditorState[0];

        public string saveName;
        public string savePath;

        [LabelText("所有Nodes")]
        public List<Node> nodes = new List<Node>();

        /// <summary>
        /// 待删除的结点，用于Undo
        /// </summary>
        [HideInInspector]
        public List<Node> nodesForDelete = new List<Node>();

        [LabelText("右击Group的顶部标题即可弹出删除选项")]
        public List<NodeGroup> groups = new List<NodeGroup>();

        #region 生成一个Canvas

        /// <summary>
        /// Creates a canvas of the specified canvasType as long as it is a subclass of NodeCanvas
        /// </summary>
        public static NodeCanvas CreateCanvas(Type canvasType)
        {
            NodeCanvas canvas;
            if (canvasType != null && canvasType.IsSubclassOf(typeof (NodeCanvas)))
                canvas = CreateInstance(canvasType) as NodeCanvas;
            else
            {
                canvas = CreateInstance<DefaultCanvas>();
            }

            canvas.name = canvas.saveName = "New " + canvas.canvasName;

            if (canvas is DefaultCanvas)
            {
                canvas.savePath = NodeEditor.editorPath + "Resources/Saves/DefaultCanvas.asset";
                NodeEditorSaveManager.SaveNodeCanvas(canvas.savePath, ref canvas);
            }
            else
            {
                string panelPath = NodeEditor.editorPath + "Resources/Saves/";
                string panelFileName = "Node Canvas";
                string path = UnityEditor.EditorUtility.SaveFilePanelInProject("Save Node Canvas", panelFileName, "asset", "", panelPath);
                canvas.savePath = path;
                NodeEditorSaveManager.SaveNodeCanvas(canvas.savePath, ref canvas);
            }
            
            canvas.OnCreate();
            return canvas;
        }

        #endregion

        #region Extension Methods

        // GENERAL

        protected virtual void OnCreate()
        {
        }

        protected virtual void ValidateSelf()
        {
        }

        public virtual void OnBeforeSavingCanvas()
        {
        }

        public virtual bool CanAddNode(string nodeID)
        {
            return true;
        }

        // GUI

        public virtual void DrawCanvasPropertyEditor()
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Validates this canvas, checking for any broken nodes or references and cleans them.
        /// </summary>
        public void Validate()
        {
            NodeEditor.checkInit(false);

            // Check Groups
            CheckNodeCanvasList(ref groups, "groups");

            // Check Nodes and their connection ports
            CheckNodeCanvasList(ref nodes, "nodes");
            foreach (Node node in nodes)
            {
                ConnectionPortManager.UpdateConnectionPorts(node);
                node.canvas = this;
                foreach (ConnectionPort port in node.connectionPorts)
                    port.Validate(node);
            }

            // Check EditorStates
            if (editorStates == null)
                editorStates = new NodeEditorState[0];
            editorStates = editorStates.Where((NodeEditorState state) => state != null).ToArray();
            foreach (NodeEditorState state in editorStates)
            {
                bool isValidate = true;
                foreach (var node in state.selectedNodes)
                {
                    if (!nodes.Contains(node))
                    {
                        isValidate = false;
                    }
                }

                if (!isValidate)
                {
                    state.selectedNodes.Clear();
                }
            }

            // Validate CanvasType-specific stuff
            ValidateSelf();
        }

        /// <summary>
        /// Checks the specified list and assures it is initialized, contains no null nodes and it it does, removes them and outputs an error.
        /// </summary>
        private void CheckNodeCanvasList<T>(ref List<T> list, string listName)
        {
            if (list == null)
            {
                Debug.LogWarning("NodeCanvas '" + name + "' " + listName +
                    " were erased and set to null! Automatically fixed!");
                list = new List<T>();
            }

            int originalCount = list.Count;
            list = list.Where((T o) => o != null).ToList();
            if (originalCount != list.Count)
                Debug.LogWarning("NodeCanvas '" + name + "' contained " + (originalCount - list.Count) +
                    " broken (null) " + listName + "! Automatically fixed!");
        }

        /// <summary>
        /// Updates the source of this canvas to the specified path, updating saveName and savePath aswell as livesInScene when prefixed with "SCENE/"
        /// </summary>
        public void UpdateSource(string path)
        {
            if (path == savePath)
                return;
            string newName;

            int nameStart = path.LastIndexOf('/') + 1;
            newName = path.Substring(nameStart, path.Length - nameStart - 6);

            savePath = path;
            saveName = newName;

            return;
        }

        #endregion
    }
}