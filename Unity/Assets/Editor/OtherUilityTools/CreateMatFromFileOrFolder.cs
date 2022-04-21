//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年7月2日 14:03:34
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using MongoDB.Bson;
using MonKey;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace ET
{
    public class CreateMatFromFileOrFolder: OdinEditorWindow
    {
        [InfoBox("此工具所生成的Material文件以dds文件名为基础，生成位置处于dds同一目录")]
        [InfoBox("这里的文件夹中的所有dds文件都将生成一个Material")]
        [LabelText("这里是dds文件夹列表")]
        [FolderPath]
        public List<string> ddsFolderList = new List<string>();

        [InfoBox("下面列表中所有dds文件都将生成一个Material")]
        [LabelText("这里是dds文件列表")]
        public List<Texture> ddsFileList = new List<Texture>();

        [InfoBox("下面列表中所有dds文件都将生成一个material")]
        [LabelText("这里是去掉文件夹与文件中重合文件之后的最终文件列表,请不要编辑它！")]
        public HashSet<Texture> FinalFiles = new HashSet<Texture>();

        [InfoBox("这里的Shader选择将决定你材质球最终效果")]
        [HideLabel]
        public Shader ShaderSetting;

        [Command("ETEditor_AutoGenerateMaterialFromDDS","依据dds文件自动生成Material",Category = "ETEditor")]
        private static void OpenWindow()
        {
            var window = GetWindow<CreateMatFromFileOrFolder>();
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(600, 600);
            window.titleContent = new GUIContent("自动生成Material工具");
        }

        [Button("开始一键生成Material文件", 25), GUIColor(0.4f, 0.8f, 1)]
        public void StartGenerateMatFile()
        {
            List<string> innerRes = new List<string>();
            //先遍历文件夹中所有dds文件
            foreach (var VARIABLE in this.ddsFolderList)
            {
                innerRes.AddRange(EditorResHelper.GetAllDDSFilePath(VARIABLE));
            }

            //根据文件路径加载文件
            foreach (var VARIABLE in innerRes)
            {
                this.ddsFileList.Add(AssetDatabase.LoadAssetAtPath<Texture>(VARIABLE));
            }

            //使用HashSet去重
            FinalFiles = new HashSet<Texture>(this.ddsFileList);
            
            //正式生成
            foreach (var VARIABLE in FinalFiles)
            {
                Material materialData = new Material(this.ShaderSetting);
                materialData.mainTexture = VARIABLE;

                string fileFullPath = AssetDatabase.GetAssetPath(VARIABLE);
                string[] pathSplit = fileFullPath.Split('/');

                AssetDatabase.CreateAsset(materialData,
                    string.Concat(fileFullPath.Substring(0, fileFullPath.Length - pathSplit[pathSplit.Length - 1].Length), VARIABLE.name, ".mat"));
            }
        }
    }
}