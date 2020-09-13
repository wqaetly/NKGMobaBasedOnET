using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using ETModel;
using NodeEditorFramework.Utilities;
using Plugins.NodeEditor.Editor.Canvas;
using Plugins.NodeEditor.Node_Editor.Default;
using UnityEditor;
using Object = UnityEngine.Object;
#if UNITY_5_3_OR_NEWER || UNITY_5_3
using UnityEngine.SceneManagement;

#endif

namespace NodeEditorFramework
{
    /// <summary>
    /// Manager handling all save and load operations on NodeCanvases and NodeEditorStates of the Node Editor, both as assets and in the scene
    /// </summary>
    public static class NodeEditorSaveManager
    {
        #region Asset Saving

        /// <summary>
        /// Saves the the specified NodeCanvas as a new asset at path, optionally as a working copy and overwriting any existing save at path
        /// </summary>
        public static void SaveNodeCanvas(string path, ref NodeCanvas nodeCanvas)
        {
            if (string.IsNullOrEmpty(path)) throw new System.ArgumentNullException("Cannot save NodeCanvas: No path specified!");
            if (nodeCanvas == null)
                throw new System.ArgumentNullException("Cannot save NodeCanvas: The specified NodeCanvas that should be saved to path '" + path +
                    "' is null!");
            if (nodeCanvas.GetType() == typeof (NodeCanvas))
                throw new System.ArgumentException(
                    "Cannot save NodeCanvas: The NodeCanvas has no explicit type! Please convert it to a valid sub-type of NodeCanvas!");

            nodeCanvas.Validate();

            if (!UnityEditor.AssetDatabase.Contains(nodeCanvas))
            {
                UnityEditor.AssetDatabase.CreateAsset(nodeCanvas, path);
            }
            UnityEditor.AssetDatabase.SaveAssets();
            UnityEditor.AssetDatabase.Refresh();

            NodeEditorCallbacks.IssueOnSaveCanvas(nodeCanvas);
        }

        /// <summary>
        /// 净化Canvas中的数据（一般不用，这是在开发期去除一些残余数据）
        /// </summary>
        /// <param name="path"></param>
        private static void PurifyCanvas(string path)
        {
            UnityEngine.Object[] subAssets = AssetDatabase.LoadAllAssetsAtPath(path);
            bool hasInitOneCanvasDataManager = false, hasInitCanvasEditorState = false;
            if (subAssets != null)
            {
                foreach (var subAsset in subAssets)
                {
                    if (subAsset is ConnectionPort connection && connection.body == null)
                    {
                        UnityEngine.Object.DestroyImmediate(subAsset, true);
                    }
                    else if (subAsset is NPBehaveCanvasDataManager)
                    {
                        if (hasInitOneCanvasDataManager)
                        {
                            UnityEngine.Object.DestroyImmediate(subAsset, true);
                        }
                        else
                        {
                            hasInitOneCanvasDataManager = true;
                        }
                    }
                    else if (subAsset is NodeEditorState)
                    {
                        if (hasInitCanvasEditorState)
                        {
                            UnityEngine.Object.DestroyImmediate(subAsset, true);
                        }
                        else
                        {
                            hasInitCanvasEditorState = true;
                        }
                    }
                    else if (subAsset == null)
                    {
                        UnityEngine.Object.DestroyImmediate(subAsset, true);
                    }
                }
            }
        }

        
        /// <summary>
        /// Loads the NodeCanvas from the asset file at path and optionally creates a working copy of it before returning
        /// </summary>
        public static NodeCanvas LoadNodeCanvas(string path)
        {
#if !UNITY_EDITOR
			throw new System.NotImplementedException ();
#else
            if (string.IsNullOrEmpty(path))
                throw new System.ArgumentNullException("Cannot load Canvas: No path specified!");
            path = ResourceManager.PreparePath(path);

            // Load only the NodeCanvas from the save file
            NodeCanvas nodeCanvas = ResourceManager.LoadResource<NodeCanvas>(path);
            if (nodeCanvas == null)
                throw new UnityException("Cannot load NodeCanvas: The file at the specified path '" + path +
                    "' is no valid save file as it does not contain a NodeCanvas!");

            // Set the path as the new source of the canvas
            nodeCanvas.UpdateSource(path);

            // Postprocess the loaded canvas
            nodeCanvas.Validate();

            NodeEditorCallbacks.IssueOnLoadCanvas(nodeCanvas);
            return nodeCanvas;
#endif
        }

        #region Utility

#if UNITY_EDITOR

        /// <summary>
        /// Adds the specified hidden subAssets to the mainAsset
        /// </summary>
        public static void AddSubAssets(ScriptableObject[] subAssets, ScriptableObject mainAsset)
        {
            foreach (ScriptableObject subAsset in subAssets)
                AddSubAsset(subAsset, mainAsset);
        }

        /// <summary>
        /// Adds the specified hidden subAsset to the mainAsset
        /// </summary>
        public static void AddSubAsset(ScriptableObject subAsset, ScriptableObject mainAsset)
        {
            if (subAsset != null && mainAsset != null)
            {
                Object[] subAssets = AssetDatabase.LoadAllAssetRepresentationsAtPath(AssetDatabase.GetAssetPath(mainAsset));
                if (subAssets.Contains(subAsset))
                {
                    return;
                }

                UnityEditor.AssetDatabase.AddObjectToAsset(subAsset, mainAsset);
                subAsset.hideFlags = HideFlags.HideInHierarchy;
            }
        }

        /// <summary>
        /// Adds the specified hidden subAsset to the mainAsset at path
        /// </summary>
        public static void AddSubAsset(ScriptableObject subAsset, string path)
        {
            if (subAsset != null && !string.IsNullOrEmpty(path))
            {
                UnityEditor.AssetDatabase.AddObjectToAsset(subAsset, path);
                subAsset.hideFlags = HideFlags.HideInHierarchy;
            }
        }

#endif

        #endregion

        #endregion

        #region Utility

        /// <summary>
        /// Returns the editorState with the specified name in canvas.
        /// If not found but others and forceFind is false, a different one is chosen randomly, else a new one is created.
        /// </summary>
        public static NodeEditorState ExtractEditorState(NodeCanvas canvas, string stateName, bool forceFind = false)
        {
            NodeEditorState state = null;
            if (canvas.editorStates.Length > 0) // Search for the editorState
                state = canvas.editorStates.FirstOrDefault((NodeEditorState s) => s.name == stateName);
            if (state == null && !forceFind) // Take any other if not found
                state = canvas.editorStates.FirstOrDefault();
            if (state == null)
            {
                // Create editorState
                state = ScriptableObject.CreateInstance<NodeEditorState>();
                state.canvas = canvas;
                // Insert into list
                int index = canvas.editorStates.Length;
                System.Array.Resize(ref canvas.editorStates, index + 1);
                canvas.editorStates[index] = state;
                NodeEditorSaveManager.AddSubAsset(state, canvas);
#if UNITY_EDITOR
                UnityEditor.EditorUtility.SetDirty(canvas);
#endif
            }

            state.canvas = canvas;
            state.name = stateName;
            return state;
        }

        public static void SetLastCanvasPath(string path)
        {
            UnityEngine.PlayerPrefs.SetString("LastCanvasPath", path);
        }

        public static string GetLastCanvasPath()
        {
            string result = UnityEngine.PlayerPrefs.GetString("LastCanvasPath", "");
            return result;
        }

        #endregion
    }
}