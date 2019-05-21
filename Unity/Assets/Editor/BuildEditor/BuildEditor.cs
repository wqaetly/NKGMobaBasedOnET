using System.Collections.Generic;
using System.IO;
using System.Linq;
using ETModel;
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

    public class BuildEditor: EditorWindow
    {
        private readonly Dictionary<string, BundleInfo> dictionary = new Dictionary<string, BundleInfo>();

        string IndependentBundleAndAtlaspath;
        Rect IndependentBundleAndAtlasrect;

        string BundleAndAtlasWithoutSharepath;
        Rect BundleAndAtlasWithoutSharerect;

        List<string> FUIRes = new List<string>();

        private Vector2 scoller;

        [SerializeField]
        public List<string> IndependentBundleAndAtlas = new List<string>();

        [SerializeField]
        public List<string> BundleAndAtlasWithoutShare = new List<string>();

        //序列化对象
        protected SerializedObject _serializedObject;

        //序列化属性
        protected SerializedProperty IndependentBundleAndAtlasProperty;
        protected SerializedProperty BundleAndAtlasWithoutShareProperty;

        private BuildData m_BuildData;

        private PlatformType platformType;
        private bool isBuildExe;
        private bool isContainAB;
        private BuildType buildType;
        private BuildOptions buildOptions = BuildOptions.AllowDebugging | BuildOptions.Development;
        private BuildAssetBundleOptions buildAssetBundleOptions = BuildAssetBundleOptions.None;

        /// <summary>
        /// 上一个版本号
        /// </summary>
        private int lastVersion;

        /// <summary>
        /// 当前版本号
        /// </summary>
        private int currentVersion;

        [MenuItem("Tools/打包工具")]
        public static void ShowWindow()
        {
            GetWindow(typeof (BuildEditor));
        }

        private void OnEnable()
        {
            this.InitABEditorConfig();
        }

        private void OnGUI()
        {
            scoller = EditorGUILayout.BeginScrollView(scoller);

            EditorGUILayout.LabelField("AB包标签设置： ", EditorStyles.boldLabel);

            this.InitABTagList();

            EditorGUILayout.Space();
            EditorGUILayout.Space();

            EditorGUILayout.LabelField("出包设置： ", EditorStyles.boldLabel);
            EditorGUILayout.LabelField("请选择打包平台： ", EditorStyles.boldLabel);
            this.platformType = (PlatformType) EditorGUILayout.EnumFlagsField("	", platformType);
            EditorGUILayout.LabelField("是否打包EXE: ", EditorStyles.boldLabel);
            this.isBuildExe = EditorGUILayout.Toggle("	", this.isBuildExe);
            EditorGUILayout.LabelField("是否同将资源（ab包）打进EXE: ", EditorStyles.boldLabel);
            this.isContainAB = EditorGUILayout.Toggle("	", this.isContainAB);
            EditorGUILayout.LabelField("版本号: （建议递增版本号打包，上一个版本号是" + this.lastVersion + "）", EditorStyles.boldLabel);
            this.currentVersion = EditorGUILayout.IntField("	", this.currentVersion);
            EditorGUILayout.LabelField("BuildType: ", EditorStyles.boldLabel);
            this.buildType = (BuildType) EditorGUILayout.EnumPopup("	", this.buildType);

            switch (buildType)
            {
                case BuildType.Development:
                    this.buildOptions = BuildOptions.Development | BuildOptions.AutoRunPlayer | BuildOptions.ConnectWithProfiler |
                            BuildOptions.AllowDebugging;
                    break;
                case BuildType.Release:
                    this.buildOptions = BuildOptions.None;
                    break;
            }

            EditorGUILayout.LabelField("BuildAssetBundleOptions(可多选): ", EditorStyles.boldLabel);
            this.buildAssetBundleOptions = (BuildAssetBundleOptions) EditorGUILayout.EnumFlagsField("	", this.buildAssetBundleOptions);
            EditorGUILayout.Space();
            EditorGUILayout.EndScrollView();
            if (GUILayout.Button("开始一键打包", GUILayout.Height(50)))
            {
                if (this.platformType == PlatformType.None)
                {
                    this.ShowNotification(new GUIContent("请选择打包平台!"));
                    return;
                }

                //开始打包
                BuildHelper.Build(this.platformType, this.buildAssetBundleOptions, this.buildOptions, this.isBuildExe, this.isContainAB,
                    this.currentVersion);
            }
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

        /// <summary>
        /// 初始化AB包标签数组模块
        /// </summary>
        private void InitABTagList()
        {
            //更新
            _serializedObject.Update();

            EditorGUI.BeginChangeCheck();

            #region //IndependentBundleAndAtlasProperty

            //第二个参数必须为true，否则无法显示子节点即List内容
            EditorGUILayout.PropertyField(this.IndependentBundleAndAtlasProperty, true);

            EditorGUILayout.LabelField("把需要一把梭打包的目录文件夹（每个prefab以及依赖资源都打包）拖到此处：");

            //获得一个长300的框
            this.IndependentBundleAndAtlasrect = EditorGUILayout.GetControlRect(GUILayout.Width(300));

            //将上面的框作为文本输入框
            this.IndependentBundleAndAtlaspath = EditorGUI.TextField(IndependentBundleAndAtlasrect, IndependentBundleAndAtlaspath);

            //如果鼠标正在拖拽中或拖拽结束时，并且鼠标所在位置在文本输入框内
            if ((Event.current.type == EventType.DragUpdated
                    || Event.current.type == EventType.DragExited)
                && IndependentBundleAndAtlasrect.Contains(Event.current.mousePosition))
            {
                //改变鼠标的外表
                DragAndDrop.visualMode = DragAndDropVisualMode.Generic;
                if (DragAndDrop.paths != null && DragAndDrop.paths.Length > 0)
                {
                    this.IndependentBundleAndAtlas.Add(DragAndDrop.paths[0]);
                }
            }

            #endregion

            #region BundleAndAtlasWithoutShareProperty

            //第二个参数必须为true，否则无法显示子节点即List内容
            EditorGUILayout.PropertyField(this.BundleAndAtlasWithoutShareProperty, true);
            EditorGUILayout.LabelField("把不需要分析依赖的目录文件夹（每个prefab都单独打包）拖到此处：");
            //获得一个长300的框
            this.BundleAndAtlasWithoutSharerect = EditorGUILayout.GetControlRect(GUILayout.Width(300));
            //将上面的框作为文本输入框
            this.BundleAndAtlasWithoutSharepath = EditorGUI.TextField(BundleAndAtlasWithoutSharerect, BundleAndAtlasWithoutSharepath);

            //如果鼠标正在拖拽中或拖拽结束时，并且鼠标所在位置在文本输入框内
            if ((Event.current.type == EventType.DragUpdated
                    || Event.current.type == EventType.DragExited)
                && BundleAndAtlasWithoutSharerect.Contains(Event.current.mousePosition))
            {
                //改变鼠标的外表
                DragAndDrop.visualMode = DragAndDropVisualMode.Generic;
                if (DragAndDrop.paths != null && DragAndDrop.paths.Length > 0)
                {
                    this.BundleAndAtlasWithoutShare.Add(DragAndDrop.paths[0]);
                }
            }

            if (EditorGUI.EndChangeCheck())
            {
                //提交修改
                _serializedObject.ApplyModifiedProperties();
            }

            #endregion BundleAndAtlasWithoutShareProperty

            if (GUILayout.Button("整理所有数组，并将数组和版本号写入文件供下次使用"))
            {
                SortOutList(this.IndependentBundleAndAtlas);
                SortOutList(this.BundleAndAtlasWithoutShare);
                this.m_BuildData.VersionInfo = this.currentVersion;
                this.m_BuildData.IndependentBundleAndAtlas = this.IndependentBundleAndAtlas;
                this.m_BuildData.BundleAndAtlasWithoutShare = this.BundleAndAtlasWithoutShare;
                using (FileStream fileStream = new FileStream("Assets/Res/ABInfoFileSave/List.txt", FileMode.Create))
                {
                    byte[] bytes = JsonHelper.ToJson(this.m_BuildData).ToByteArray();
                    fileStream.Write(bytes, 0, bytes.Length);
                }
            }

            if (GUILayout.Button("一键配置AB包标签（自动清空所有标签,自动配置FUI包标签）"))
            {
                ClearPackingTagAndAssetBundle();

                foreach (var VARIABLE in this.IndependentBundleAndAtlas)
                {
                    //自动过滤FUI
                    if (VARIABLE.EndsWith("FUI")) continue;
                    SetIndependentBundleAndAtlas(VARIABLE);
                }

                foreach (var VARIABLE in this.BundleAndAtlasWithoutShare)
                {
                    if (VARIABLE.EndsWith("FUI")) continue;
                    SetBundleAndAtlasWithoutShare(VARIABLE);
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

            if (GUILayout.Button("清空StreamingAssets目录，并且生成Version.txt文件"))
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
        }

        /// <summary>
        /// 整理数组，排序，去重，去空
        /// </summary>
        /// <param name="listToBeSortOut"></param>
        private void SortOutList(List<string> listToBeSortOut)
        {
            listToBeSortOut.Sort();
            for (int i = listToBeSortOut.Count - 1; i >= 0; i--)
            {
                if (listToBeSortOut[i] == null) continue;

                if (listToBeSortOut[i] == "")
                {
                    listToBeSortOut.Remove(listToBeSortOut[i]);
                    continue;
                }

                if (i - 1 >= 0 && listToBeSortOut[i] == listToBeSortOut[i - 1])
                {
                    listToBeSortOut.Remove(listToBeSortOut[i]);
                }
            }
        }

        /// <summary>
        /// 初始化AB工具配置
        /// </summary>
        private void InitABEditorConfig()
        {
            m_BuildData = new BuildData();
            //使用当前类初始化
            _serializedObject = new SerializedObject(this);
            //获取当前类中可序列化的属性
            this.IndependentBundleAndAtlasProperty = _serializedObject.FindProperty("IndependentBundleAndAtlas");
            this.BundleAndAtlasWithoutShareProperty = _serializedObject.FindProperty("BundleAndAtlasWithoutShare");

            if (!Directory.Exists("Assets/Res/ABInfoFileSave/"))
            {
                Directory.CreateDirectory("Assets/Res/ABInfoFileSave/");
            }

            if (!File.Exists("Assets/Res/ABInfoFileSave/List.txt"))
            {
                using (FileStream fileStream = new FileStream("Assets/Res/ABInfoFileSave/List.txt", FileMode.Create))
                {
                    byte[] bytes = JsonHelper.ToJson(new BuildData()).ToByteArray();
                    fileStream.Write(bytes, 0, bytes.Length);
                }
            }

            BuildData buildData = JsonHelper.FromJson<BuildData>(File.ReadAllText("Assets/Res/ABInfoFileSave/List.txt"));
            this.lastVersion = buildData.VersionInfo;
            this.IndependentBundleAndAtlas = buildData.IndependentBundleAndAtlas;
            this.BundleAndAtlasWithoutShare = buildData.BundleAndAtlasWithoutShare;
        }
    }
}