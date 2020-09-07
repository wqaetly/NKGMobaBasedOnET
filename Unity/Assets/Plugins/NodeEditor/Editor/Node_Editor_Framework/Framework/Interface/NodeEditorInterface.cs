using System;
using System.IO;
using UnityEngine;
using Plugins.NodeEditor.Editor.Canvas;
using Plugins.NodeEditor.Node_Editor.Default;
using GenericMenu = NodeEditorFramework.Utilities.GenericMenu;

namespace NodeEditorFramework.Standard
{
    public class NodeEditorInterface
    {
        public NodeEditorUserCache canvasCache;
        public Action<GUIContent> ShowNotificationAction;

        // GUI
        public string sceneCanvasName = "";
        public float toolbarHeight = 17;

        // Modal Panel
        public bool showModalPanel;
        public Rect modalPanelRect = new Rect(20, 50, 250, 70);
        public Action modalPanelContent;
        
        public void ShowNotification(GUIContent message)
        {
            if (ShowNotificationAction != null)
                ShowNotificationAction(message);
        }

        #region GUI

        public void DrawToolbarGUI(Rect rect)
        {
            rect.height = toolbarHeight;
            GUILayout.BeginArea(rect, NodeEditorGUI.toolbar);
            GUILayout.BeginHorizontal();
            float curToolbarHeight = 0;

            if (GUILayout.Button("File", NodeEditorGUI.toolbarDropdown, GUILayout.Width(50)))
            {
                GenericMenu menu = new GenericMenu();

                // New Canvas filled with canvas types
                NodeCanvasManager.FillCanvasTypeMenu(ref menu, NewNodeCanvas, "New Canvas/");
                menu.AddSeparator("");

                // Load / Save
#if UNITY_EDITOR
                menu.AddItem(new GUIContent("Load Canvas"), false, LoadCanvas);
                menu.AddItem(new GUIContent("Reload Canvas"), false, ReloadCanvas);
                menu.AddSeparator("");

                menu.AddItem(new GUIContent("Save Canvas"), false, SaveCanvas);
                menu.AddItem(new GUIContent("Save Canvas As"), false, SaveCanvasAs);

                // menu.AddSeparator("");
#endif
                menu.Show(new Vector2(5, toolbarHeight));
            }

            curToolbarHeight = Mathf.Max(curToolbarHeight, GUILayoutUtility.GetLastRect().yMax);

            GUILayout.Space(10);
            GUILayout.FlexibleSpace();

            GUILayout.Label(new GUIContent(this.canvasCache.openedCanvasPath), NodeEditorGUI.toolbarLabel);
            GUILayout.Label(this.canvasCache.typeData.DisplayString, NodeEditorGUI.toolbarLabel);
            curToolbarHeight = Mathf.Max(curToolbarHeight, GUILayoutUtility.GetLastRect().yMax);

            GUI.backgroundColor = new Color(1, 0.3f, 0.3f, 1);
            if (NodeEditor.curNodeCanvas is NPBehaveCanvas)
            {
                if (GUILayout.Button("DataBase", NodeEditorGUI.toolbarButton, GUILayout.Width(100)))
                {
                    NPBehaveCanvas npBehaveCanvas = this.canvasCache.nodeCanvas as NPBehaveCanvas;
                    UnityEditor.Selection.activeObject = npBehaveCanvas.GetCurrentCanvasDatas();
                }
            }

#if !UNITY_EDITOR
			GUILayout.Space(5);
			if (GUILayout.Button("Quit", NodeEditorGUI.toolbarButton, GUILayout.Width(100)))
				Application.Quit ();
#endif
            curToolbarHeight = Mathf.Max(curToolbarHeight, GUILayoutUtility.GetLastRect().yMax);
            GUI.backgroundColor = Color.white;

            GUILayout.EndHorizontal();
            GUILayout.EndArea();
            if (Event.current.type == EventType.Repaint)
                toolbarHeight = curToolbarHeight;
        }

        public void DrawModalPanel()
        {
            if (showModalPanel)
            {
                if (modalPanelContent == null)
                    return;
                GUILayout.BeginArea(modalPanelRect, NodeEditorGUI.nodeBox);
                modalPanelContent.Invoke();
                GUILayout.EndArea();
            }
        }

        #endregion

        private void NewNodeCanvas(Type canvasType)
        {
            this.AssertSavaCanvasSuccessfully();
            canvasCache.NewNodeCanvas(canvasType);
        }

#if UNITY_EDITOR
        private void LoadCanvas()
        {
            string path = UnityEditor.EditorUtility.OpenFilePanel("Load Node Canvas", NodeEditor.editorPath + "Resources/Saves/", "asset");
            if (!path.Contains(Application.dataPath))
            {
                if (!string.IsNullOrEmpty(path))
                    ShowNotification(new GUIContent("You should select an asset inside your project folder!"));
            }
            else
            {
                this.AssertSavaCanvasSuccessfully();
                path = path.Replace("/",@"\");
                canvasCache.LoadNodeCanvas(path);
            }
        }

        private void ReloadCanvas()
        {
            string path = canvasCache.nodeCanvas.savePath;
            if (!string.IsNullOrEmpty(path))
            {
                canvasCache.LoadNodeCanvas(path);
                ShowNotification(new GUIContent("Canvas Reloaded!"));
            }
            else
                ShowNotification(new GUIContent("Cannot reload canvas as it has not been saved yet!"));
        }

        public void SaveCanvas()
        {
            string path = canvasCache.nodeCanvas.savePath;
            if (!string.IsNullOrEmpty(path))
            {
                canvasCache.SaveNodeCanvas(path);
                ShowNotification(new GUIContent("Canvas Saved!"));
                Debug.Log($"{path}已保存成功");
            }
            else
            {
                Debug.LogError($"{path}保存失败，请先确保要覆盖的文件已存在！（尝试使用Save As创建目标文件）");
                ShowNotification(new GUIContent("No save location found. Use 'Save As'!"));
            }
        }

        /// <summary>
        /// 供外部调用的保存当前图的接口
        /// </summary>
        /// <returns></returns>
        public bool AssertSavaCanvasSuccessfully()
        {
            if (canvasCache.nodeCanvas is DefaultCanvas || !File.Exists(NodeEditorSaveManager.GetLastCanvasPath()))
            {
                return true;
            }

            string path = canvasCache.nodeCanvas.savePath;
            //清理要删掉的Node
            foreach (var nodeForDelete in canvasCache.nodeCanvas.nodesForDelete)
            {
                UnityEngine.Object.DestroyImmediate(nodeForDelete, true);
            }

            canvasCache.nodeCanvas.nodesForDelete.Clear();

            if (!string.IsNullOrEmpty(path))
            {
                canvasCache.SaveNodeCanvas(path);
                ShowNotification(new GUIContent("Canvas Saved!"));
                Debug.Log($"{path}已保存成功");
                return true;
            }
            else
            {
                Debug.LogError($"{path}保存失败，请先确保要覆盖的文件已存在！（尝试使用Save As创建目标文件）");
                ShowNotification(new GUIContent("No save location found. Use 'Save As'!"));
                return false;
            }
        }

        public void SaveCanvasAs()
        {
            string panelPath = NodeEditor.editorPath + "Resources/Saves/";
            string panelFileName = "Node Canvas";
            if (canvasCache.nodeCanvas != null && !string.IsNullOrEmpty(canvasCache.nodeCanvas.savePath))
            {
                panelPath = canvasCache.nodeCanvas.savePath;
                string savedFileName = System.IO.Path.GetFileNameWithoutExtension(panelPath);
                if (!string.IsNullOrEmpty(savedFileName))
                {
                    panelPath = panelPath.Substring(0, panelPath.LastIndexOf(savedFileName));
                    panelFileName = savedFileName;
                }
            }

            string path = UnityEditor.EditorUtility.SaveFilePanelInProject("Save Node Canvas", panelFileName, "asset", "", panelPath);
            if (!string.IsNullOrEmpty(path))
                canvasCache.SaveNodeCanvas(path);
        }
#endif
    }
}