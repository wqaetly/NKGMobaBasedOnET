using System;
using UnityEngine;
using UnityEditor;
using System.IO;
using ETModel;
using MonKey;
using NodeEditorFramework.Utilities;
using NPOI.SS.Util;
using Sirenix.OdinInspector.Editor;

namespace NodeEditorFramework.Standard
{
    public class NodeEditorWindow: EditorWindow
    {
        // Information about current instance
        private static NodeEditorWindow _editor;

        public static NodeEditorWindow editor
        {
            get
            {
                AssureEditor();
                return _editor;
            }
        }

        public static void AssureEditor()
        {
            if (_editor == null) OpenNodeEditor();
        }

        // Canvas cache
        public NodeEditorUserCache canvasCache;
        public static NodeEditorInterface editorInterface;

        // GUI
        private Rect canvasWindowRect
        {
            get
            {
                return new Rect(0, NodeEditorInterface.toolbarHeight, position.width, position.height - NodeEditorInterface.toolbarHeight);
            }
        }

        #region General
        
        /// <summary>
        /// Opens the Node Editor window and loads the last session
        /// </summary>
        [Command("Open NodeEditor","打开可视化节点编辑器",Category = "ETEditor")]
        public static NodeEditorWindow OpenNodeEditor()
        {
            _editor = GetWindow<NodeEditorWindow>();
            _editor.minSize = new Vector2(400, 200);

            NodeEditor.ReInit(false);
            Texture iconTexture = ResourceManager.LoadTexture("Textures/Icon_Dark.png");
            _editor.titleContent = new GUIContent("Node Editor", iconTexture);

            return _editor;
        }

        /// <summary>
        /// Assures that the canvas is opened when double-clicking a canvas asset
        /// </summary>
        [UnityEditor.Callbacks.OnOpenAsset(10)]
        private static bool AutoOpenCanvas(int instanceID, int line)
        {
            if (Selection.activeObject is NodeCanvas)
            {
                if (Selection.activeObject != null)
                {
                    try
                    {
                        AssureEditor();
                        editorInterface.AssertSavaCanvasSuccessfully();
                        string NodeCanvasPath = AssetDatabase.GetAssetPath(instanceID);
                        NodeCanvasPath = NodeCanvasPath.Replace("/", @"\");
                        _editor.canvasCache.LoadNodeCanvas(NodeCanvasPath);
                        //需要重新为editorInterface绑定canvasCache，所以这里要置空，留给框架底层自行检测然后绑定
                        editorInterface = null;
                        return true;
                    }
                    catch
                    {
                        Debug.LogError($"打开失败？试试从\"Tools/其他实用工具/多功能可视化编辑器\"打开吧！");
                    }
                }
            }

            return false;
        }

        private void OnEnable()
        {
            // Subscribe to events
            NodeEditor.ClientRepaints -= Repaint;
            NodeEditor.ClientRepaints += Repaint;

            _editor = this;
            NormalReInit();
        }

        private void OnDestroy()
        {
            editorInterface.AssertSavaCanvasSuccessfully();

            // Unsubscribe from events
            NodeEditor.ClientRepaints -= Repaint;
            NodeEditor.curEditorState = null;
            NodeEditor.curNodeCanvas = null;
            editorInterface = null;
            _editor = null;
            this.canvasCache = null;
        }

        private void NormalReInit()
        {
            NodeEditor.ReInit(false);
            AssureSetup();
            if (canvasCache.nodeCanvas)
                canvasCache.nodeCanvas.Validate();
        }

        private void AssureSetup()
        {
            if (canvasCache == null)
            {
                // Create cache
                canvasCache = new NodeEditorUserCache();
            }

            canvasCache.AssureCanvas();
            if (editorInterface == null)
            {
                // Setup editor interface
                editorInterface = new NodeEditorInterface();
                editorInterface.canvasCache = canvasCache;
                editorInterface.ShowNotificationAction = ShowNotification;
            }
        }

        #endregion

        #region GUI

        private void OnGUI()
        {
            // Initiation
            NodeEditor.checkInit(true);
            if (NodeEditor.InitiationError)
            {
                GUILayout.Label("Node Editor Initiation failed! Check console for more information!");
                return;
            }

            AssureEditor();
            AssureSetup();

            // Begin Node Editor GUI and set canvas rect
            NodeEditorGUI.StartNodeGUI(true);
            canvasCache.editorState.canvasRect = canvasWindowRect;

            try
            {
                // Perform drawing with error-handling
                NodeEditor.DrawCanvas(canvasCache.nodeCanvas, canvasCache.editorState);
            }
            catch (UnityException e)
            {
                // On exceptions in drawing flush the canvas to avoid locking the UI
                canvasCache.NewNodeCanvas();
                NodeEditor.ReInit(true);
                Debug.LogError("Unloaded Canvas due to an exception during the drawing phase!");
                Debug.LogException(e);
            }

            // Draw Interface
            editorInterface.DrawToolbarGUI(new Rect(0, 0, Screen.width, 0));

            // End Node Editor GUI
            NodeEditorGUI.EndNodeGUI();
        }

        #endregion
    }
}