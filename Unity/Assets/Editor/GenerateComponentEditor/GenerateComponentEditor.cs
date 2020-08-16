//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2020年7月25日 13:09:27
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace ETModel
{
    public class GenerateComponentEditor : OdinEditorWindow
    {
        /// <summary>
        /// 用于生成Component的结构体
        /// </summary>
        public struct StructForGenerateComponent
        {
            [LabelText("组件名，不含Component")] public string ComponentName;
            [LabelText("目标文件夹")] [FolderPath] public string FoldName;
        }

        /// <summary>
        /// 所有需要生成的类
        /// </summary>
        [LabelText("所有需要生成的Component配置")] [ListDrawerSettings(Expanded = true)]
        public List<StructForGenerateComponent> AllComponentsForGenerate = new List<StructForGenerateComponent>();

        [LabelText("目标模板")] public TextAsset TargetTemplate;

        [MenuItem("NKGTools/一键生成类工具/Component生成工具")]
        private static void OpenWindow()
        {
            var window = GetWindow<GenerateComponentEditor>();
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(600, 600);
            window.titleContent = new GUIContent("Component生成工具");
        }

        private void OnEnable()
        {
            this.TargetTemplate =
                AssetDatabase.LoadAssetAtPath<TextAsset>("Assets/Editor/GenerateComponentEditor/Template.txt");
            if (AllComponentsForGenerate.Count == 0)
            {
                AllComponentsForGenerate.Add(new StructForGenerateComponent());
            }
        }

        [Button("开始生成", 25), GUIColor(100 / 255f, 149 / 255f, 237 / 255f)]
        public void StartGenerate()
        {
            if (TargetTemplate == null)
            {
                Log.Error("目标模板为空");
                return;
            }

            string templateContent = TargetTemplate.text;
            foreach (var componentConfig in AllComponentsForGenerate)
            {
                string temp = templateContent;
                string finalFileName = $"{componentConfig.FoldName}/{componentConfig.ComponentName}Component.cs";

                temp = temp.Replace("_ComponentName_", componentConfig.ComponentName);
                if (!Directory.Exists(componentConfig.FoldName))
                {
                    Directory.CreateDirectory(componentConfig.FoldName);
                }

                while (File.Exists(finalFileName))
                {
                    finalFileName = finalFileName.Replace(".cs", "_1.cs");
                }

                //将文件信息读入流中
                //初始化System.IO.FileStream类的新实例与指定路径和创建模式
                using (var fs = new FileStream(finalFileName, FileMode.OpenOrCreate))
                {
                    if (!fs.CanWrite)
                    {
                        throw new System.Security.SecurityException("文件fileName=" + finalFileName + "是只读文件不能写入!");
                    }

                    var sw = new StreamWriter(fs);
                    sw.WriteLine(temp);
                    sw.Dispose();
                    sw.Close();
                }
            }

            AssetDatabase.Refresh();
        }
    }
}