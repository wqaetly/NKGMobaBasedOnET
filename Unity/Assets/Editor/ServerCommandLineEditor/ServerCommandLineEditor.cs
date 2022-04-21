using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Security.AccessControl;
using UnityEditor;
using UnityEditor.Compilation;
using UnityEngine;
using AssemblyBuilder = UnityEditor.Compilation.AssemblyBuilder;

namespace ET
{
    public class ServerCommandLineEditor: EditorWindow
    {
        [MenuItem("Tools/打开服务器选项")]
        private static void ShowWindow()
        {
            GetWindow(typeof(ServerCommandLineEditor));
        }
        
        public void OnGUI()
        {
            if (GUILayout.Button("启动本地服务器"))
            {
                string arguments = $"--AppType=Server --Process=1 --Console=1";
                ProcessHelper.Run("Server.exe", arguments, "../Bin/");
            }

            if (GUILayout.Button("刷新资源"))
            {
                AssetDatabase.Refresh();
            }
        }
    }
}