//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2021年6月9日 14:08:27
//------------------------------------------------------------

using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace GraphProcessor
{
    public static class GraphCreateAndSaveHelper
    {
        public static BaseGraph CreateGraph(Type graphType)
        {
            BaseGraph baseGraph = ScriptableObject.CreateInstance(graphType) as BaseGraph;
            string panelPath = "Assets/Plugins/NodeEditor/Examples/Saves/";
            Directory.CreateDirectory(panelPath);
            string panelFileName = "Graph";
            string path = EditorUtility.SaveFilePanelInProject("Save Graph Asset", panelFileName, "asset", "", panelPath);
            if (string.IsNullOrEmpty(path))
            {
                Debug.Log("创建graph已取消");
                return null;
            }
            AssetDatabase.CreateAsset(baseGraph, path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            return baseGraph;
        }
        
        public static void SaveGraphToDisk(BaseGraph baseGraphToSave)
        {
            EditorUtility.SetDirty(baseGraphToSave);
            AssetDatabase.SaveAssets();
        }
    }
}