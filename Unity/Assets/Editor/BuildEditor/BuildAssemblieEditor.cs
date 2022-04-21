using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEditor;
using UnityEditor.Compilation;

namespace ET
{
    [InitializeOnLoad]
    public static class BuildAssemblieEditor
    {
        /// <summary>
        /// 最原始的4个程序集路径
        /// </summary>
        private static string[] s_OriginDllDirs = new[]
        {
            "Library/ScriptAssemblies/Unity.Model",
            "Library/ScriptAssemblies/Unity.ModelView",
            "Library/ScriptAssemblies/Unity.Hotfix",
            "Library/ScriptAssemblies/Unity.HotfixView"
        };

        /// <summary>
        /// 最原始的4个程序集对应名称
        /// </summary>
        private static string[] s_OriginDllName = new[]
        {
            "Model",
            "ModelView",
            "Hotfix",
            "HotfixView"
        };

        /// <summary>
        /// 最终的Hotfix dll路径
        /// </summary>
        private static string s_FinalHotfixDllDir = "Assets/Res/Code/";

        static BuildAssemblieEditor()
        {
            for (int i = 0; i < s_OriginDllDirs.Length; i++)
            {
                string dllOriPath = s_OriginDllDirs[i] + ".dll";
                string dllDesPath = Path.Combine(s_FinalHotfixDllDir, s_OriginDllName[i] + ".dll.bytes");

                string pdbOriPath = s_OriginDllDirs[i] + ".pdb";
                string pdbDesPath = Path.Combine(s_FinalHotfixDllDir, s_OriginDllName[i] + ".pdb.bytes");

                File.Copy(dllOriPath, dllDesPath, true);
                File.Copy(pdbOriPath, pdbDesPath, true);
            }

            AssetDatabase.Refresh();
        }
    }
}