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
    public class BundleInfo
    {
        public List<string> ParentPaths = new List<string>();
    }

    public enum PlatformType
    {
        None,
        Android,
        IOS,
        PC,
        MacOS,
    }

    public enum BuildType
    {
        Development,
        Release,
    }

    public class BuildEditor: OdinEditorWindow
    {
        private readonly Dictionary<string, BundleInfo> dictionary = new Dictionary<string, BundleInfo>();
        List<string> FUIRes = new List<string>();

        [LabelText("平台选择")]
        public PlatformType platformType;

        [LabelText("出包模式")]
        public BuildType buildType;

        [LabelText("一把梭打包,自动分析依赖")]
        [FolderPath]
        public List<string> IndependentABFolder;

        [LabelText("一把梭打包,不分析依赖")]
        [FolderPath]
        public List<string> WithoutShareABFolder;

        [LabelText("一把梭打包,自动分析依赖和共享资源")]
        [FolderPath]
        public List<string> ShareABFolder;

        [LabelText("上一版本号")]
        public int lastVersion;

        [LabelText("当前版本号")]
        public int currentVersion;

        [LabelText("AB包配置文件")]
        public BuildData m_BuildData = new BuildData();

        [LabelText("是否打Exe包")]
        public bool isBuildExe;

        [LabelText("Exe是否包含AB包")]
        public bool isContainAB;

        private BuildAssetBundleOptions buildAssetBundleOptions = BuildAssetBundleOptions.None;
        private BuildOptions buildOptions = BuildOptions.AllowDebugging | BuildOptions.Development;

        private void OnEnable()
        {
            this.InitABEditorConfig();
        }

        [MenuItem("NKGTools/打包工具")]
        private static void OpenWindow()
        {
            var window = GetWindow<BuildEditor>();
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(600, 600);
            window.titleContent = new GUIContent("打包工具");
        }

        [Button("将所有AB配置保存至文件", 25), GUIColor(0.4f, 0.8f, 1)]
        public void BuildAB()
        {
            this.m_BuildData.VersionInfo = this.currentVersion;
            this.m_BuildData.IndependentBundleAndAtlas = this.IndependentABFolder;
            this.m_BuildData.BundleAndAtlasWithoutShare = this.WithoutShareABFolder;
            using (FileStream fileStream = new FileStream("Assets/Res/EditorExtensionInfoSave/MyABInfo.bytes", FileMode.Create))
            {
                BsonSerializer.Serialize(new BsonBinaryWriter(fileStream), this.m_BuildData);
            }
        }

        [Button("清空StreamingAssets目录，并生成新的Version.txt文件", 25), GUIColor(0.4f, 0.8f, 1)]
        public void CleanStreamingAssets()
        {
            FileHelper.CleanDirectory("Assets/StreamingAssets");
            //创建版本信息类，并将版本号与资源总大小赋值
            VersionConfig versionProto = new VersionConfig();
            versionProto.Version = 0;
            versionProto.TotalSize = 0;
            //如果不将AB打入EXE文件，则需要额外生成一个Version.txt文件
            if (!isContainAB)
            {
                using (FileStream fileStream = new FileStream("Assets/StreamingAssets/Version.txt", FileMode.Create))
                {
                    byte[] bytes = JsonHelper.ToJson(versionProto).ToByteArray();
                    fileStream.Write(bytes, 0, bytes.Length);
                }
            }
        }

        [Button("一键配置AB包标签（自动清空所有标签,自动配置FUI包标签）", 25), GUIColor(0.4f, 0.8f, 1)]
        public void MarkAB()
        {
            ClearPackingTagAndAssetBundle();

            foreach (var VARIABLE in this.IndependentABFolder)
            {
                //自动过滤FUI
                if (VARIABLE.EndsWith("FUI")) continue;
                SetIndependentBundleAndAtlas(VARIABLE);
            }

            foreach (var VARIABLE in this.WithoutShareABFolder)
            {
                if (VARIABLE.EndsWith("FUI")) continue;
                SetBundleAndAtlasWithoutShare(VARIABLE);
            }

            foreach (var VARIABLE in this.ShareABFolder)
            {
                if (VARIABLE.EndsWith("FUI")) continue;
                this.SetShareBundleAndAtlas(VARIABLE);
            }

            this.FUIRes = EditorResHelper.GetFUIResourcePath();

            foreach (var VARIABLE in this.FUIRes)
            {
                Object go = AssetDatabase.LoadAssetAtPath<Object>(VARIABLE);
                if (VARIABLE.EndsWith("png"))
                {
                    SetBundle(VARIABLE, go.name.Split('_')[0]);
                    continue;
                }

                SetBundle(VARIABLE, go.name);
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport | ImportAssetOptions.ForceUpdate);
        }

        [Button("开始打包", 35), GUIColor(0.4f, 0.5f, 1)]
        public void StartBuild()
        {
            switch (buildType)
            {
                case BuildType.Development:
                    buildOptions = BuildOptions.Development | BuildOptions.AutoRunPlayer | BuildOptions.ConnectWithProfiler |
                            BuildOptions.AllowDebugging;
                    break;
                case BuildType.Release:
                    buildOptions = BuildOptions.None;
                    break;
            }

            if (this.platformType == PlatformType.None)
            {
                this.ShowNotification(new GUIContent("请选择打包平台!"));
                return;
            }

            //开始打包
            BuildHelper.Build(this.platformType, this.buildAssetBundleOptions, this.buildOptions, this.isBuildExe, this.isContainAB,
                this.currentVersion);
        }

        #region 相关函数

        /// <summary>
        /// 初始化AB工具配置
        /// </summary>
        private void InitABEditorConfig()
        {
            if (!Directory.Exists("Assets/Res/EditorExtensionInfoSave/"))
            {
                Directory.CreateDirectory("Assets/Res/EditorExtensionInfoSave/");
            }

            if (!File.Exists("Assets/Res/EditorExtensionInfoSave/MyABInfo.bytes"))
            {
                using (FileStream fileStream = new FileStream("Assets/Res/EditorExtensionInfoSave/MyABInfo.bytes", FileMode.Create))
                {
                    BsonSerializer.Serialize(new BsonBinaryWriter(fileStream), this.m_BuildData);
                }
            }

            byte[] m_ABConfig = File.ReadAllBytes("Assets/Res/EditorExtensionInfoSave/MyABInfo.bytes");
            this.m_BuildData = BsonSerializer.Deserialize<BuildData>(m_ABConfig);
            this.IndependentABFolder = this.m_BuildData.IndependentBundleAndAtlas;
            this.WithoutShareABFolder = this.m_BuildData.BundleAndAtlasWithoutShare;
            this.ShareABFolder = this.m_BuildData.BundleAndAtlasShare;
            this.lastVersion = this.m_BuildData.VersionInfo;
        }

        /// <summary>
        /// 打没有atlas的包，分析共享资源
        /// </summary>
        /// <param name="dir"></param>
        private static void SetNoAtlas(string dir)
        {
            List<string> paths = EditorResHelper.GetPrefabsAndScenes(dir);

            foreach (string path in paths)
            {
                List<string> pathes = CollectDependencies(path);

                foreach (string pt in pathes)
                {
                    if (pt == path)
                    {
                        continue;
                    }

                    SetAtlas(pt, "", true);
                }
            }
        }

        /// <summary>
        /// 会将目录下的每个prefab引用的资源强制打成一个包，不分析共享资源
        /// </summary>
        /// <param name="dir"></param>
        private static void SetBundles(string dir)
        {
            List<string> paths = EditorResHelper.GetPrefabsAndScenes(dir);
            foreach (string path in paths)
            {
                string path1 = path.Replace('\\', '/');
                Object go = AssetDatabase.LoadAssetAtPath<Object>(path1);

                SetBundle(path1, go.name);
            }
        }

        /// <summary>
        /// 会将目录下的每个prefab引用的资源打成一个包,只给顶层prefab打包
        /// </summary>
        /// <param name="dir"></param>
        private static void SetRootBundleOnly(string dir)
        {
            List<string> paths = EditorResHelper.GetPrefabsAndScenes(dir);
            foreach (string path in paths)
            {
                string path1 = path.Replace('\\', '/');
                Object go = AssetDatabase.LoadAssetAtPath<Object>(path1);

                SetBundle(path1, go.name);
            }
        }

        /// <summary>
        /// 会将目录下的每个prefab引用的资源强制打成一个包，不分析共享资源
        /// </summary>
        /// <param name="dir"></param>
        private static void SetIndependentBundleAndAtlas(string dir)
        {
            List<string> paths = EditorResHelper.GetPrefabsAndScenes(dir);

            foreach (string path in paths)
            {
                string path1 = path.Replace('\\', '/');
                Object go = AssetDatabase.LoadAssetAtPath<Object>(path1);

                AssetImporter importer = AssetImporter.GetAtPath(path1);
                if (importer == null || go == null)
                {
                    Log.Error("error: " + path1);
                    continue;
                }

                importer.assetBundleName = $"{go.name}.unity3d";

                List<string> pathes = CollectDependencies(path1);

                foreach (string pt in pathes)
                {
                    if (pt == path1)
                    {
                        continue;
                    }

                    SetBundleAndAtlas(pt, go.name, true);
                }
            }
        }

        /// <summary>
        /// 配置ab包，不分析共享资源
        /// </summary>
        /// <param name="dir"></param>
        private static void SetBundleAndAtlasWithoutShare(string dir)
        {
            List<string> paths = EditorResHelper.GetPrefabsAndScenes(dir);
            foreach (string path in paths)
            {
                string path1 = path.Replace('\\', '/');
                Object go = AssetDatabase.LoadAssetAtPath<Object>(path1);

                SetBundle(path1, go.name);
            }
        }

        /// <summary>
        /// 收集依赖信息
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        private static List<string> CollectDependencies(string o)
        {
            string[] paths = AssetDatabase.GetDependencies(o);
            return paths.ToList();
        }

        /// <summary>
        /// 设置共享资源的Tag
        /// </summary>
        /// <param name="dir"></param>
        private void SetShareBundleAndAtlas(string dir)
        {
            this.dictionary.Clear();
            List<string> paths = EditorResHelper.GetPrefabsAndScenes(dir);

            foreach (string path in paths)
            {
                string path1 = path.Replace('\\', '/');
                Object go = AssetDatabase.LoadAssetAtPath<Object>(path1);

                SetBundle(path1, go.name);

                List<string> pathes = CollectDependencies(path1);
                foreach (string pt in pathes)
                {
                    if (pt == path1)
                    {
                        continue;
                    }

                    // 不存在则记录下来
                    if (!this.dictionary.ContainsKey(pt))
                    {
                        // 如果已经设置了包
                        if (GetBundleName(pt) != "")
                        {
                            continue;
                        }

                        Log.Info($"{path1}----{pt}");
                        BundleInfo bundleInfo = new BundleInfo();
                        bundleInfo.ParentPaths.Add(path1);
                        this.dictionary.Add(pt, bundleInfo);

                        SetAtlas(pt, go.name);

                        continue;
                    }

                    // 依赖的父亲不一样
                    BundleInfo info = this.dictionary[pt];
                    if (info.ParentPaths.Contains(path1))
                    {
                        continue;
                    }

                    info.ParentPaths.Add(path1);

                    DirectoryInfo dirInfo = new DirectoryInfo(dir);
                    string dirName = dirInfo.Name;

                    SetBundleAndAtlas(pt, $"{dirName}-share", true);
                }
            }
        }

        /// <summary>
        /// 刷新重置所有ab标签配置
        /// </summary>
        private static void ClearPackingTagAndAssetBundle()
        {
            List<string> bundlePaths = EditorResHelper.GetAllResourcePath("Assets/Bundles/", true);
            foreach (string bundlePath in bundlePaths)
            {
                SetBundle(bundlePath, "", true);
            }
        }

        /// <summary>
        /// 得到ab名称
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private static string GetBundleName(string path)
        {
            string extension = Path.GetExtension(path);
            if (extension == ".cs" || extension == ".dll" || extension == ".js")
            {
                return "";
            }

            if (path.Contains("Resources"))
            {
                return "";
            }

            AssetImporter importer = AssetImporter.GetAtPath(path);
            if (importer == null)
            {
                return "";
            }

            return importer.assetBundleName;
        }

        /// <summary>
        /// 配置ab包，主要是为了增加标签（如果没有配置，则默认为untiy3d）
        /// </summary>
        /// <param name="path"></param>
        /// <param name="name"></param>
        /// <param name="overwrite"></param>
        private static void SetBundle(string path, string name, bool overwrite = false)
        {
            string extension = Path.GetExtension(path);
            if (extension == ".cs" || extension == ".dll" || extension == ".js")
            {
                return;
            }

            if (path.Contains("Resources"))
            {
                return;
            }

            AssetImporter importer = AssetImporter.GetAtPath(path);
            if (importer == null)
            {
                return;
            }

            if (importer.assetBundleName != "" && overwrite == false)
            {
                return;
            }

            string bundleName = "";
            if (name != "")
            {
                bundleName = $"{name}.unity3d";
            }

            importer.assetBundleName = bundleName;
        }

        /// <summary>
        /// 设置纹理包，主要是为了增加标签
        /// </summary>
        /// <param name="path"></param>
        /// <param name="name"></param>
        /// <param name="overwrite"></param>
        private static void SetAtlas(string path, string name, bool overwrite = false)
        {
            string extension = Path.GetExtension(path);
            if (extension == ".cs" || extension == ".dll" || extension == ".js")
            {
                return;
            }

            if (path.Contains("Resources"))
            {
                return;
            }

            TextureImporter textureImporter = AssetImporter.GetAtPath(path) as TextureImporter;
            if (textureImporter == null)
            {
                return;
            }

            if (textureImporter.spritePackingTag != "" && overwrite == false)
            {
                return;
            }

            textureImporter.spritePackingTag = name;
            AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceSynchronousImport | ImportAssetOptions.ForceUpdate);
        }

        /// <summary>
        /// 配置ab包与纹理包（如果存在的话）
        /// </summary>
        /// <param name="path"></param>
        /// <param name="name"></param>
        /// <param name="overwrite"></param>
        private static void SetBundleAndAtlas(string path, string name, bool overwrite = false)
        {
            string extension = Path.GetExtension(path);
            if (extension == ".cs" || extension == ".dll" || extension == ".js" || extension == ".mat")
            {
                return;
            }

            if (path.Contains("Resources"))
            {
                return;
            }

            AssetImporter importer = AssetImporter.GetAtPath(path);
            if (importer == null)
            {
                return;
            }

            if (importer.assetBundleName == "" || overwrite)
            {
                string bundleName = "";
                if (name != "")
                {
                    bundleName = $"{name}.unity3d";
                }

                importer.assetBundleName = bundleName;
            }

            TextureImporter textureImporter = importer as TextureImporter;
            if (textureImporter == null)
            {
                return;
            }

            if (textureImporter.spritePackingTag == "" || overwrite)
            {
                textureImporter.spritePackingTag = name;
                AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceSynchronousImport | ImportAssetOptions.ForceUpdate);
            }
        }

        #endregion
    }
}