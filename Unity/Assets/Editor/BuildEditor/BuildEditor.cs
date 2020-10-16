//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年5月22日 13:08:41
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ETModel;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ETEditor
{
    public enum BuildType
    {
        Development,
        Release,
    }

    public class BuildEditor: OdinEditorWindow
    {
        private const string c_BuildDataPath = "Assets/Editor/BuildEditor/BuildData.asset";

        [MenuItem("NKGTools/打包工具")]
        private static void OpenWindow()
        {
            var window = GetWindow<BuildEditor>();
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(600, 600);
            window.titleContent = new GUIContent("打包工具");
        }
        
        
        [Button("Apply Rule", 35), GUIColor(0.4f, 0.5f, 1)]
        public void ApplyRule()
        {
            
        }

        [Button("一键出AB包", 35), GUIColor(0.4f, 0.5f, 1)]
        public void OnlyABOutPut()
        {
            
        }
        
        [Button("一键出运行包", 35), GUIColor(0.4f, 0.5f, 1)]
        public void OnlyExeOutPut()
        {

        }
        
        [Button("一键出AB包和运行包", 35), GUIColor(0.4f, 0.5f, 1)]
        public void BothABAndExeOutPut()
        {

        }
    }
}