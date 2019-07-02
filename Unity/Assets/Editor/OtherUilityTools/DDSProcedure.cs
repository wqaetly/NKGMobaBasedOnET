//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年7月2日 16:49:05
//------------------------------------------------------------

using System;
using System.Diagnostics;
using Boo.Lang;
using ETModel;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace ETEditor
{
    /// <summary>
    /// 贴图工作流
    /// </summary>
    public class DDSProcedure: OdinEditorWindow
    {
        [LabelText("这里是dds文件夹列表")]
        [FolderPath(AbsolutePath = true)]
        public List<string> ddsFolderList = new List<string>();

        [LabelText("这里是Png文件夹列表")]
        [FolderPath(AbsolutePath = true)]
        public List<string> pngFolderList = new List<string>();

        [MenuItem("Tools/其他实用工具/贴图工作流/Png与DDS互转")]
        private static void OpenWindow()
        {
            var window = GetWindow<DDSProcedure>();
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(600, 600);
            window.titleContent = new GUIContent("贴图工作流");
        }

        [Button("Png转DDS", 25), GUIColor(0.4f, 0.8f, 1)]
        public void Png2dds()
        {
            foreach (var VARIABLE in pngFolderList)
            {
                ProcessHelper.Run("PNG_To_DDS.cmd",
                    VARIABLE,
                    "./Tools/");
            }
        }

        [Button("DDS转Png", 25), GUIColor(0.4f, 0.8f, 1)]
        public void DDS2Png()
        {
            foreach (var VARIABLE in this.ddsFolderList)
            {
                ProcessHelper.Run("DDS_To_PNG.cmd",
                    VARIABLE,
                    "./Tools/");
            }
        }
    }
}