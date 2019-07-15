//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年7月11日 18:22:46
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using ETModel;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;

namespace ETEditor
{
    public class B2S_ColliderEditor: OdinEditorWindow
    {
        [Required("需要一个画线驱动者，请场景中创建一个空物体，命名为Box2DDebuggerHandler，并挂载B2S_DebuggerHandler.cs脚本")]
        [LabelText("画线管理者")]
        [TabGroup("Special", "主持人")]
        public B2S_DebuggerHandler MB2SDebuggerHandler;

        [TabGroup("Test/矩形", "编辑数据")]
        [HideLabel]
        public B2S_BoxColliderVisualHelper MB2SBoxColliderVisualHelper;

        [TabGroup("Test/圆形", "编辑数据")]
        [HideLabel]
        public B2S_CircleColliderVisualHelper MB2SCircleColliderVisualHelper;

        [TabGroup("Test/多边形", "编辑数据")]
        [HideLabel]
        public B2S_PolygonColliderVisualHelper MB2SPolygonColliderVisualHelper;

        [TabGroup("Test/矩形/☆导出数据(Debug用)", "名称与ID映射")]
        public ColliderNameAndIdInflectSupporter BoxColliderNameAndIdInflectSupporter = new ColliderNameAndIdInflectSupporter();

        [TabGroup("Test/圆形/☆导出数据(Debug用)", "名称与ID映射")]
        public ColliderNameAndIdInflectSupporter CircleColliderNameAndIdInflectSupporter = new ColliderNameAndIdInflectSupporter();

        [TabGroup("Test/多边形/☆导出数据(Debug用)", "名称与ID映射")]
        public ColliderNameAndIdInflectSupporter PolygonColliderNameAndIdInflectSupporter = new ColliderNameAndIdInflectSupporter();

        [TabGroup("Test/矩形/☆导出数据(Debug用)", "碰撞数据")]
        public ColliderDataSupporter BoxColliderDataSupporter = new ColliderDataSupporter();

        [TabGroup("Test/圆形/☆导出数据(Debug用)", "碰撞数据")]
        public ColliderDataSupporter CircleColliderDataSupporter = new ColliderDataSupporter();

        [TabGroup("Test/多边形/☆导出数据(Debug用)", "碰撞数据")]
        public ColliderDataSupporter PolygonColliderDataSupporter = new ColliderDataSupporter();

        [TabGroup("Test", "使用视频教程地址")]
        public string Teach;

        /// <summary>
        /// 名称ID保存地址
        /// </summary>
        private string ColliderNameAndIdInflectSavePath = "Assets/Res/EditorExtensionInfoSave/";

        /// <summary>
        /// 名称ID保存地址
        /// </summary>
        private string ColliderDataSavePath = "../Config/ColliderDatas/";

        private List<string> colliderNameAndIdInflectName = new List<string>()
        {
            "BoxColliderNameAndIdInflect", "CircleColliderNameAndIdInflect", "PolygonColliderNameAndIdInflect"
        };

        private List<string> colliderDataName = new List<string>() { "BoxColliderData", "CircleColliderData", "PolygonColliderData" };

        [MenuItem("Tools/其他实用工具/Box2D可视化编辑器")]
        private static void OpenWindow()
        {
            var window = GetWindow<B2S_ColliderEditor>();
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(600, 600);
            window.titleContent = new GUIContent("Box2D可视化编辑器");
        }

        private void OnEnable()
        {
            this.ReadcolliderNameAndIdInflect();
            this.ReadcolliderData();
            this.MB2SDebuggerHandler = GameObject.Find("Box2DDebuggerHandler").GetComponent<B2S_DebuggerHandler>();

            this.MB2SBoxColliderVisualHelper =
                    new B2S_BoxColliderVisualHelper(this.BoxColliderNameAndIdInflectSupporter, this.BoxColliderDataSupporter);
            this.MB2SCircleColliderVisualHelper =
                    new B2S_CircleColliderVisualHelper(this.CircleColliderNameAndIdInflectSupporter, this.CircleColliderDataSupporter);
            this.MB2SPolygonColliderVisualHelper =
                    new B2S_PolygonColliderVisualHelper(this.PolygonColliderNameAndIdInflectSupporter, this.PolygonColliderDataSupporter);

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
            if (File.Exists($"{this.ColliderNameAndIdInflectSavePath}/{this.colliderNameAndIdInflectName[0]}.bytes"))
            {
                byte[] mfile0 = File.ReadAllBytes($"{this.ColliderNameAndIdInflectSavePath}/{this.colliderNameAndIdInflectName[0]}.bytes");
                //这里不进行长度判断会报错，正在试图访问一个已经关闭的流，咱也不懂，咱也不敢问
                if (mfile0.Length > 0)
                    this.BoxColliderNameAndIdInflectSupporter =
                            BsonSerializer.Deserialize<ColliderNameAndIdInflectSupporter>(mfile0);
            }

            if (File.Exists($"{this.ColliderNameAndIdInflectSavePath}/{this.colliderNameAndIdInflectName[1]}.bytes"))
            {
                byte[] mfile1 = File.ReadAllBytes($"{this.ColliderNameAndIdInflectSavePath}/{this.colliderNameAndIdInflectName[1]}.bytes");
                if (mfile1.Length > 0)
                    this.CircleColliderNameAndIdInflectSupporter =
                            BsonSerializer.Deserialize<ColliderNameAndIdInflectSupporter>(mfile1);
            }

            if (File.Exists($"{this.ColliderNameAndIdInflectSavePath}/{this.colliderNameAndIdInflectName[2]}.bytes"))
            {
                byte[] mfile2 = File.ReadAllBytes($"{this.ColliderNameAndIdInflectSavePath}/{this.colliderNameAndIdInflectName[2]}.bytes");
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
            Type[] types = typeof (ColliderDataSupporter).Assembly.GetTypes();
            foreach (Type type in types)
            {
                if (!type.IsSubclassOf(typeof (B2S_ColliderDataStructureBase)))
                {
                    continue;
                }

                BsonClassMap.LookupClassMap(type);
            }

            if (File.Exists($"{this.ColliderDataSavePath}/{this.colliderDataName[0]}.bytes"))
            {
                byte[] mfile0 = File.ReadAllBytes($"{this.ColliderDataSavePath}/{this.colliderDataName[0]}.bytes");
                //这里不进行长度判断会报错，正在试图访问一个已经关闭的流，咱也不懂，咱也不敢问
                if (mfile0.Length > 0)
                    this.BoxColliderDataSupporter =
                            BsonSerializer.Deserialize<ColliderDataSupporter>(mfile0);
            }

            if (File.Exists($"{this.ColliderDataSavePath}/{this.colliderDataName[1]}.bytes"))
            {
                byte[] mfile1 = File.ReadAllBytes($"{this.ColliderDataSavePath}/{this.colliderDataName[1]}.bytes");
                if (mfile1.Length > 0)
                    this.CircleColliderDataSupporter =
                            BsonSerializer.Deserialize<ColliderDataSupporter>(mfile1);
            }

            if (File.Exists($"{this.ColliderDataSavePath}/{this.colliderDataName[2]}.bytes"))
            {
                byte[] mfile2 = File.ReadAllBytes($"{this.ColliderDataSavePath}/{this.colliderDataName[2]}.bytes");
                if (mfile2.Length > 0)
                {
                    this.PolygonColliderDataSupporter =
                            BsonSerializer.Deserialize<ColliderDataSupporter>(mfile2);
                }
            }
        }

        [Button("导出所有碰撞数据", 30), GUIColor(0.5f, 0.5f, 1.0f)]
        public void ExportAllColliderData()
        {
        }
    }
}