//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年7月11日 18:22:46
//------------------------------------------------------------

#if UNITY_EDITOR


using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MonKey;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace ET
{
    /// <summary>
    /// 需要注意的是，每个带有UnityCollider2D的UnityGo本身Transform的X,Z不能有偏移, 因为对于Offset我们读取的是UnityCollider2D Offset，而不是Go的
    /// </summary>
    public class B2S_ColliderEditor : OdinEditorWindow
    {
        [LabelText("画线管理者")] [TabGroup("Special", "主持人")]
        public B2S_DebuggerHandler MB2SDebuggerHandler;

        [TabGroup("Test/矩形", "编辑数据")] [HideLabel]
        public B2S_BoxColliderVisualHelper MB2SBoxColliderVisualHelper;

        [TabGroup("Test/圆形", "编辑数据")] [HideLabel]
        public B2S_CircleColliderVisualHelper MB2SCircleColliderVisualHelper;

        [TabGroup("Test/多边形", "编辑数据")] [HideLabel]
        public B2S_PolygonColliderVisualHelper MB2SPolygonColliderVisualHelper;

        [TabGroup("Test/矩形/☆导出数据(Debug用)", "名称与ID映射")]
        public ColliderNameAndIdInflectSupporter BoxColliderNameAndIdInflectSupporter =
            new ColliderNameAndIdInflectSupporter();

        [TabGroup("Test/圆形/☆导出数据(Debug用)", "名称与ID映射")]
        public ColliderNameAndIdInflectSupporter CircleColliderNameAndIdInflectSupporter =
            new ColliderNameAndIdInflectSupporter();

        [TabGroup("Test/多边形/☆导出数据(Debug用)", "名称与ID映射")]
        public ColliderNameAndIdInflectSupporter PolygonColliderNameAndIdInflectSupporter =
            new ColliderNameAndIdInflectSupporter();

        [TabGroup("Test/矩形/☆导出数据(Debug用)", "碰撞数据")]
        public ColliderDataSupporter BoxColliderDataSupporter = new ColliderDataSupporter();

        [TabGroup("Test/圆形/☆导出数据(Debug用)", "碰撞数据")]
        public ColliderDataSupporter CircleColliderDataSupporter = new ColliderDataSupporter();

        [TabGroup("Test/多边形/☆导出数据(Debug用)", "碰撞数据")]
        public ColliderDataSupporter PolygonColliderDataSupporter = new ColliderDataSupporter();

        [TabGroup("Test", "使用视频教程地址")] public string Teach = "https://www.bilibili.com/video/av61062760";

        private List<string> colliderNameAndIdInflectName = new List<string>()
        {
            "BoxColliderNameAndIdInflect", "CircleColliderNameAndIdInflect", "PolygonColliderNameAndIdInflect"
        };

        private List<string> colliderDataName = new List<string>()
            {"BoxColliderData", "CircleColliderData", "PolygonColliderData"};

        [Command("ETEditor_B2SBattleColliderExport", "战斗Box2D碰撞数据可视化编辑器", Category = "ETEditor")]
        private static void OpenWindowCCC()
        {
            var window = GetWindow<B2S_ColliderEditor>();

            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(600, 600);
            window.titleContent = new GUIContent("Box2D可视化编辑器");
        }

        private void OnEnable()
        {
            this.ReadcolliderNameAndIdInflect();
            this.ReadcolliderData();
            this.MB2SDebuggerHandler = new GameObject("Box2DDebuggerHandler").AddComponent<B2S_DebuggerHandler>();

            this.MB2SBoxColliderVisualHelper =
                new B2S_BoxColliderVisualHelper(this.BoxColliderNameAndIdInflectSupporter,
                    this.BoxColliderDataSupporter);
            this.MB2SCircleColliderVisualHelper =
                new B2S_CircleColliderVisualHelper(this.CircleColliderNameAndIdInflectSupporter,
                    this.CircleColliderDataSupporter);
            this.MB2SPolygonColliderVisualHelper =
                new B2S_PolygonColliderVisualHelper(this.PolygonColliderNameAndIdInflectSupporter,
                    this.PolygonColliderDataSupporter);

            this.MB2SBoxColliderVisualHelper.InitColliderBaseInfo();
            this.MB2SCircleColliderVisualHelper.InitColliderBaseInfo();
            this.MB2SPolygonColliderVisualHelper.InitColliderBaseInfo();

            this.MB2SDebuggerHandler.MB2SColliderVisualHelpers.Add(this.MB2SBoxColliderVisualHelper);
            this.MB2SDebuggerHandler.MB2SColliderVisualHelpers.Add(this.MB2SCircleColliderVisualHelper);
            this.MB2SDebuggerHandler.MB2SColliderVisualHelpers.Add(this.MB2SPolygonColliderVisualHelper);
            EditorApplication.update += this.MB2SDebuggerHandler.OnUpdate;
        }

        private void OnDisable()
        {
            EditorApplication.update -= this.MB2SDebuggerHandler.OnUpdate;
            MB2SDebuggerHandler.CleanCollider();
            if (MB2SDebuggerHandler != null)
            {
                UnityEngine.Object.DestroyImmediate(MB2SDebuggerHandler.gameObject);
            }

            this.MB2SDebuggerHandler = null;
            this.MB2SBoxColliderVisualHelper = null;
            this.MB2SCircleColliderVisualHelper = null;
            this.MB2SPolygonColliderVisualHelper = null;
        }

        /// <summary>
        /// 读取碰撞名称和ID映射表
        /// </summary>
        private void ReadcolliderNameAndIdInflect()
        {
            if (File.Exists(
                $"{B2S_BattleColliderExportPathDefine.ColliderNameAndIdInflectSavePath}/{this.colliderNameAndIdInflectName[0]}.bytes"))
            {
                byte[] mfile0 =
                    File.ReadAllBytes(
                        $"{B2S_BattleColliderExportPathDefine.ColliderNameAndIdInflectSavePath}/{this.colliderNameAndIdInflectName[0]}.bytes");
                //这里不进行长度判断会报错，正在试图访问一个已经关闭的流，咱也不懂，咱也不敢问
                if (mfile0.Length > 0)
                    this.BoxColliderNameAndIdInflectSupporter =
                        BsonSerializer.Deserialize<ColliderNameAndIdInflectSupporter>(mfile0);
            }

            if (File.Exists(
                $"{B2S_BattleColliderExportPathDefine.ColliderNameAndIdInflectSavePath}/{this.colliderNameAndIdInflectName[1]}.bytes"))
            {
                byte[] mfile1 =
                    File.ReadAllBytes(
                        $"{B2S_BattleColliderExportPathDefine.ColliderNameAndIdInflectSavePath}/{this.colliderNameAndIdInflectName[1]}.bytes");
                if (mfile1.Length > 0)
                    this.CircleColliderNameAndIdInflectSupporter =
                        BsonSerializer.Deserialize<ColliderNameAndIdInflectSupporter>(mfile1);
            }

            if (File.Exists(
                $"{B2S_BattleColliderExportPathDefine.ColliderNameAndIdInflectSavePath}/{this.colliderNameAndIdInflectName[2]}.bytes"))
            {
                byte[] mfile2 =
                    File.ReadAllBytes(
                        $"{B2S_BattleColliderExportPathDefine.ColliderNameAndIdInflectSavePath}/{this.colliderNameAndIdInflectName[2]}.bytes");
                if (mfile2.Length > 0)
                    this.PolygonColliderNameAndIdInflectSupporter =
                        BsonSerializer.Deserialize<ColliderNameAndIdInflectSupporter>(mfile2);
            }
        }

        /// <summary>
        /// 读取所有碰撞数据
        /// </summary>
        private void ReadcolliderData()
        {
            Type[] types = typeof(ColliderDataSupporter).Assembly.GetTypes();
            foreach (Type type in types)
            {
                if (!type.IsSubclassOf(typeof(B2S_ColliderDataStructureBase)))
                {
                    continue;
                }

                BsonClassMap.LookupClassMap(type);
            }

            if (File.Exists(
                $"{B2S_BattleColliderExportPathDefine.ServerColliderDataSavePath}/{this.colliderDataName[0]}.bytes"))
            {
                byte[] mfile0 =
                    File.ReadAllBytes(
                        $"{B2S_BattleColliderExportPathDefine.ServerColliderDataSavePath}/{this.colliderDataName[0]}.bytes");
                //这里不进行长度判断会报错，正在试图访问一个已经关闭的流，咱也不懂，咱也不敢问
                if (mfile0.Length > 0)
                    this.BoxColliderDataSupporter =
                        BsonSerializer.Deserialize<ColliderDataSupporter>(mfile0);
            }

            if (File.Exists(
                $"{B2S_BattleColliderExportPathDefine.ServerColliderDataSavePath}/{this.colliderDataName[1]}.bytes"))
            {
                byte[] mfile1 =
                    File.ReadAllBytes(
                        $"{B2S_BattleColliderExportPathDefine.ServerColliderDataSavePath}/{this.colliderDataName[1]}.bytes");
                if (mfile1.Length > 0)
                    this.CircleColliderDataSupporter =
                        BsonSerializer.Deserialize<ColliderDataSupporter>(mfile1);
            }

            if (File.Exists(
                $"{B2S_BattleColliderExportPathDefine.ServerColliderDataSavePath}/{this.colliderDataName[2]}.bytes"))
            {
                byte[] mfile2 =
                    File.ReadAllBytes(
                        $"{B2S_BattleColliderExportPathDefine.ServerColliderDataSavePath}/{this.colliderDataName[2]}.bytes");
                if (mfile2.Length > 0)
                {
                    this.PolygonColliderDataSupporter =
                        BsonSerializer.Deserialize<ColliderDataSupporter>(mfile2);
                }
            }
        }

        [Button("将导出碰撞配置同步到客户端", 30), GUIColor(0.5f, 0.5f, 1.0f)]
        public void SyncServerConfigToClientConfig()
        {
            DirectoryInfo directory = new DirectoryInfo(B2S_BattleColliderExportPathDefine.ServerColliderDataSavePath);
            if (Directory.Exists(B2S_BattleColliderExportPathDefine.ClientColliderDataSavePath))
            {
                DirectoryInfo clientDirectoryInfo =
                    new DirectoryInfo(B2S_BattleColliderExportPathDefine.ClientColliderDataSavePath);

                foreach (var fileInfo in clientDirectoryInfo.GetFiles())
                {
                    File.Delete(fileInfo.FullName);
                }
            }
            else
            {
                Directory.CreateDirectory(B2S_BattleColliderExportPathDefine.ClientColliderDataSavePath);
            }

            foreach (var file in directory.GetFiles())
            {
                File.Copy(file.FullName,
                    $"{B2S_BattleColliderExportPathDefine.ClientColliderDataSavePath}/{file.Name}");
            }
            
            AssetDatabase.Refresh();
        }
    }
}


#endif