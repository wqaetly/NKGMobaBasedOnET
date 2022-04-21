//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2020年7月25日 13:09:27
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MonKey;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace ET
{
    public class GenerateComponentEditor: OdinEditorWindow
    {
        /// <summary>
        /// 所有需要生成的类
        /// </summary>
        [LabelText("所有需要生成Code配置")]
        [ListDrawerSettings(Expanded = true)]
        public List<AParams_GenerateBase> TargetsForGenerate = new List<AParams_GenerateBase>();

        [Command("ETEditor_CodeGenerator","从模板生成代码",Category = "ETEditor")]
        private static void OpenWindow()
        {
            var window = GetWindow<GenerateComponentEditor>();
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(600, 600);
            window.titleContent = new GUIContent("代码生成工具");
        }

        [Button("开始生成", 25), GUIColor(100 / 255f, 149 / 255f, 237 / 255f)]
        public void StartGenerate()
        {
            if (this.TargetsForGenerate.Count == 0)
            {
                return;
            }

            foreach (var mParams in TargetsForGenerate)
            {
                string templateContent = AssetDatabase.LoadAssetAtPath<TextAsset>(mParams.TemplateAssetPath).text;

                string temp = templateContent;
                string finalFileName = $"{mParams.FoldName}/{mParams.FileName}.cs";

                foreach (var kParam in mParams.GetAllParams())
                {
                    temp = temp.Replace(kParam.Key, kParam.Value);
                }

                if (!Directory.Exists(mParams.FoldName))
                {
                    Directory.CreateDirectory(mParams.FoldName);
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