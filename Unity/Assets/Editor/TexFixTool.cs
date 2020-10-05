//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2020年10月5日 18:40:41
//------------------------------------------------------------

using System.IO;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace ETEditor
{
    public class TexFixTool: OdinEditorWindow
    {
        [FolderPath]
        [LabelText("材质目录")]
        public string MatPath;

        [FolderPath]
        [LabelText("贴图目录")]
        public string TexPath;

        [LabelText("目标Shader")]
        public Shader TargetShader;

        [MenuItem("NKGTools/地图修复工具")]
        private static void OpenWindow()
        {
            var window = GetWindow<TexFixTool>();
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(600, 600);
            window.titleContent = new GUIContent("地图修复工具");
        }

        [Button("开始修复", 25), GUIColor(100 / 255f, 149 / 255f, 237 / 255f)]
        public void BeginFix()
        {
            DirectoryInfo matDirectoryInfo = new DirectoryInfo(MatPath);
            FileInfo[] allMatFileInfos = matDirectoryInfo.GetFiles();

            foreach (var matFileInfo in allMatFileInfos)
            {
                Material material = AssetDatabase.LoadAssetAtPath<Material>(Path.Combine(MatPath, matFileInfo.Name));
                if (material == null)
                {
                    continue;
                }

                material.shader = TargetShader;
                string texturePath = TexPath + '/' + matFileInfo.Name.Split('.')[0] + ".png";
                material.SetTexture("_MainTexture", AssetDatabase.LoadAssetAtPath<Texture2D>(texturePath));
            }
        }
    }
}