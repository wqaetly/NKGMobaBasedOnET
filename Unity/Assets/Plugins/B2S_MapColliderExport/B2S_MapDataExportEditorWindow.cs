#if UNITY_EDITOR
using System.IO;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using MonKey;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace ET
{
    public class B2S_MapDataExportEditorWindow : OdinEditorWindow
    {
        [LabelText("将要导出的层级")] public string LayerMaskToExport;

        [LabelText("将要保存的路径")] [FolderPath] public string SavedPath = "../Config/B2S_Battle";

        [LabelText("将要保存的文件名")] public string SavedFileName = "B2S_MapColliderData";

        private B2S_MapColliderDrawer m_B2S_MapColliderDrawer;

        [Command("ETEditor_Box2DMapDataExport","地图碰撞数据导出",Category = "ETEditor")]
        public static void OpenBox2DMapDataExportWindow()
        {
            var window = GetWindow<B2S_MapDataExportEditorWindow>();
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(600, 600);
            window.titleContent = new GUIContent("Box2D地图编辑器");
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            m_B2S_MapColliderDrawer = UnityEngine.Object.FindObjectOfType<B2S_MapColliderDrawer>();
            if (m_B2S_MapColliderDrawer == null)
            {
                m_B2S_MapColliderDrawer = new GameObject("*B2S Debugger (Will Auto Deleted)")
                    .AddComponent<B2S_MapColliderDrawer>();
            }

            RedBox2DData();
        }

        [LabelText("地图碰撞数据")] public ColliderDataSupporter ColliderDataSupporter = new ColliderDataSupporter();

        [Button("导出地图碰撞数据", 30), GUIColor(0.37f, 0.56f, 0.37f)]
        public void ExportBox2DData()
        {
            GlobalDefine.Options = new Options();
            ColliderDataSupporter.colliderDataDic.Clear();

            foreach (var o in Resources.FindObjectsOfTypeAll(typeof(GameObject)))
            {
                var obj = (GameObject) o;
                if (obj.layer != LayerMask.NameToLayer(LayerMaskToExport)) continue;

                BoxCollider boxCollider = obj.GetComponent<BoxCollider>();
                if (boxCollider != null)
                {
                    long id = IdGenerater.Instance.GenerateId();
                    var position1 = obj.transform.position;
                    ColliderDataSupporter.colliderDataDic.Add(id, new B2S_BoxColliderDataStructure()
                    {
                        id = IdGenerater.Instance.GenerateId(), isSensor = true,
                        b2SColliderType = B2S_ColliderType.BoxColllider,
                        finalOffset = new System.Numerics.Vector2(position1.x + boxCollider.center.x,
                            position1.z + boxCollider.center.z),
                        hx = boxCollider.size.x * obj.transform.lossyScale.x / 2,
                        hy = boxCollider.size.z * obj.transform.lossyScale.z / 2,
                        Angle = obj.transform.rotation.eulerAngles.y
                    });
                    // Debug.Log(
                    //     $"----{obj.transform.rotation.eulerAngles.y} ------------{new System.Numerics.Vector2(position1.x + boxCollider.center.x, position1.z + boxCollider.center.z).ToString()}");
                }

                SphereCollider sphereCollider = obj.GetComponent<SphereCollider>();
                if (sphereCollider != null)
                {
                    long id = IdGenerater.Instance.GenerateId();
                    var position1 = obj.transform.position;
                    ColliderDataSupporter.colliderDataDic.Add(id, new B2S_CircleColliderDataStructure()
                    {
                        id = IdGenerater.Instance.GenerateId(), isSensor = true,
                        b2SColliderType = B2S_ColliderType.CircleCollider,
                        finalOffset = new System.Numerics.Vector2(position1.x + sphereCollider.center.x,
                            position1.z + sphereCollider.center.z),
                        radius = sphereCollider.radius,
                    });
                }
            }

            m_B2S_MapColliderDrawer.ColliderDataSupporter = ColliderDataSupporter;

            using (FileStream file = File.Create($"{this.SavedPath}/{SavedFileName}.bytes"))
            {
                BsonSerializer.Serialize(new BsonBinaryWriter(file), this.ColliderDataSupporter);
            }
        }


        [Button("读取地图数据", 30), GUIColor(0.37f, 0.56f, 0.37f)]
        public void RedBox2DData()
        {
            if (File.Exists($"{this.SavedPath}/{SavedFileName}.bytes"))
            {
                byte[] mfile0 = File.ReadAllBytes($"{this.SavedPath}/{SavedFileName}.bytes");
                //这里不进行长度判断会报错，正在试图访问一个已经关闭的流，咱也不懂，咱也不敢问
                if (mfile0.Length > 0)
                {
                    this.ColliderDataSupporter =
                        BsonSerializer.Deserialize<ColliderDataSupporter>(mfile0);
                    m_B2S_MapColliderDrawer.ColliderDataSupporter = ColliderDataSupporter;
                }
            }
        }

        private void OnDisable()
        {
            if (m_B2S_MapColliderDrawer != null)
                DestroyImmediate(m_B2S_MapColliderDrawer.gameObject);
        }
    }
}
#endif