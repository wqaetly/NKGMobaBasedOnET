//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年7月10日 21:02:18
//------------------------------------------------------------

using System.IO;
using System.Linq;
using DefaultNamespace;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEngine;
using UnityEditor;

namespace ETModel
{
    /// <summary>
    /// 多边形可视化辅助
    /// </summary>
    public class B2S_PolygonColliderVisualHelper: B2S_ColliderVisualHelperBase
    {
        [DisableInEditorMode]
        [LabelText("映射文件名称")]
        public string NameAndIdInflectFileName = "PolygonColliderNameAndIdInflect";

        [DisableInEditorMode]
        [LabelText("碰撞数据文件名称")]
        public string ColliderDataFileName = "PolygonColliderData";

        [InlineEditor]
        [Required("需要至少一个Unity2D多边形碰撞器")]
        [HideLabel]
        [BsonIgnore]
        public PolygonCollider2D mCollider2D;

        [LabelText("多边形碰撞体数据")]
        public B2S_PolygonColliderDataStructure MB2S_PolygonColliderDataStructure = new B2S_PolygonColliderDataStructure();

        public override void InitColliderBaseInfo()
        {
            this.MB2S_PolygonColliderDataStructure.b2SColliderType = B2S_ColliderType.PolygonCollider;
        }

        [Button("重新绘制多边形碰撞体", 25), GUIColor(0.2f, 0.9f, 1.0f)]
        public override void InitPointInfo()
        {
            this.MB2S_PolygonColliderDataStructure.points.Clear();
            
            matrix4X4 = Matrix4x4.TRS(theObjectWillBeEdited.transform.position, theObjectWillBeEdited.transform.rotation,
                theObjectWillBeEdited.transform.parent.localScale);

            for (int i = 0; i < mCollider2D.points.Length; i++)
            {
                this.MB2S_PolygonColliderDataStructure.points.Add(mCollider2D.points[i].UnityVector2ToCoustumVector2());
            }

            MB2S_PolygonColliderDataStructure.pointCount = mCollider2D.GetTotalPointCount();
            MB2S_PolygonColliderDataStructure.offset.Fill(this.mCollider2D.offset);
            this.canDraw = true;
        }

        public override void DrawCollider()
        {
            for (int i = 0; i < MB2S_PolygonColliderDataStructure.pointCount; i++)
            {
                if (i < MB2S_PolygonColliderDataStructure.pointCount - 1)
                    Gizmos.DrawLine(matrix4X4.MultiplyPoint(new Vector3(MB2S_PolygonColliderDataStructure.points[i].x + this.mCollider2D.offset.x, 0,
                            MB2S_PolygonColliderDataStructure.points[i].y + this.mCollider2D.offset.y)),
                        matrix4X4.MultiplyPoint(new Vector3(MB2S_PolygonColliderDataStructure.points[i + 1].x + this.mCollider2D.offset.x, 0,
                            MB2S_PolygonColliderDataStructure.points[i + 1].y + this.mCollider2D.offset.y)));
                else
                {
                    Gizmos.DrawLine(matrix4X4.MultiplyPoint(new Vector3(MB2S_PolygonColliderDataStructure.points[i].x + this.mCollider2D.offset.x, 0,
                            MB2S_PolygonColliderDataStructure.points[i].y + this.mCollider2D.offset.y)),
                        matrix4X4.MultiplyPoint(new Vector3(MB2S_PolygonColliderDataStructure.points[0].x + this.mCollider2D.offset.x, 0,
                            MB2S_PolygonColliderDataStructure.points[0].y + this.mCollider2D.offset.y)));
                }
            }
        }

        [Button("保存所有多边形碰撞体名称与ID映射信息", 25), GUIColor(0.2f, 0.9f, 1.0f)]
        public override void SavecolliderNameAndIdInflect()
        {
            if (!this.MColliderNameAndIdInflectSupporter.colliderNameAndIdInflectDic.ContainsKey(this.theObjectWillBeEdited.transform.parent.name))
            {
                MColliderNameAndIdInflectSupporter.colliderNameAndIdInflectDic.Add(this.theObjectWillBeEdited.transform.parent.name,
                    this.MB2S_PolygonColliderDataStructure.id);
            }
            else
            {
                MColliderNameAndIdInflectSupporter.colliderNameAndIdInflectDic[this.theObjectWillBeEdited.transform.parent.name] =
                        this.MB2S_PolygonColliderDataStructure.id;
            }

            using (FileStream file = File.Create($"{this.NameAndIdInflectSavePath}/{this.NameAndIdInflectFileName}.bytes"))
            {
                BsonSerializer.Serialize(new BsonBinaryWriter(file), this.MColliderNameAndIdInflectSupporter);
            }
        }

        [Button("保存所有多边形碰撞体信息", 25), GUIColor(0.2f, 0.9f, 1.0f)]
        public override void SavecolliderData()
        {
            if (this.theObjectWillBeEdited != null && this.mCollider2D != null)
            {
                if (!this.MColliderDataSupporter.colliderDataDic.ContainsKey(this.MB2S_PolygonColliderDataStructure.id))
                {
                    this.MColliderDataSupporter.colliderDataDic.Add(this.MB2S_PolygonColliderDataStructure.id,
                        this.MB2S_PolygonColliderDataStructure);
                }
                else
                {
                    this.MColliderDataSupporter.colliderDataDic[this.MB2S_PolygonColliderDataStructure.id] =
                            this.MB2S_PolygonColliderDataStructure;
                }

                using (FileStream file = File.Create($"{this.ColliderDataSavePath}/{this.ColliderDataFileName}.bytes"))
                {
                    BsonSerializer.Serialize(new BsonBinaryWriter(file), this.MColliderDataSupporter);
                }
            }
        }

        public override void OnUpdate()
        {
            if (theObjectWillBeEdited == null)
            {
                mCollider2D = null;
                this.canDraw = false;
                this.MB2S_PolygonColliderDataStructure.id = 0;
                this.MB2S_PolygonColliderDataStructure.points.Clear();
                this.MB2S_PolygonColliderDataStructure.pointCount = 0;
                this.MB2S_PolygonColliderDataStructure.isSensor = false;
                this.MB2S_PolygonColliderDataStructure.offset.Clean();
                return;
            }

            if (this.MB2S_PolygonColliderDataStructure.id == 0)
            {
                if (this.MColliderNameAndIdInflectSupporter.colliderNameAndIdInflectDic.TryGetValue(this.theObjectWillBeEdited.transform.parent.name,
                    out this.MB2S_PolygonColliderDataStructure.id))
                {
                    Debug.Log($"自动设置圆形碰撞体ID成功，ID为{MB2S_PolygonColliderDataStructure.id}");
                }
                
                if (this.MColliderDataSupporter.colliderDataDic.ContainsKey(this.MB2S_PolygonColliderDataStructure.id))
                {
                    this.MB2S_PolygonColliderDataStructure =
                            (B2S_PolygonColliderDataStructure) this.MColliderDataSupporter.colliderDataDic[this.MB2S_PolygonColliderDataStructure.id];
                }
            }

            if (mCollider2D == null)
            {
                mCollider2D = theObjectWillBeEdited.GetComponent<PolygonCollider2D>();
                if (mCollider2D == null)
                {
                    this.canDraw = false;
                }
            }
        }

        public B2S_PolygonColliderVisualHelper(ColliderNameAndIdInflectSupporter colliderNameAndIdInflectSupporter,
        ColliderDataSupporter colliderDataSupporter): base(colliderNameAndIdInflectSupporter, colliderDataSupporter)
        {
        }
    }
}