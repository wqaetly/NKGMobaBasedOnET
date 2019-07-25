//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年7月10日 21:01:06
//------------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ETModel
{
    /// <summary>
    /// 矩形可视化辅助
    /// </summary>
    public class B2S_BoxColliderVisualHelper: B2S_ColliderVisualHelperBase
    {
        [DisableInEditorMode]
        [LabelText("映射文件名称")]
        public string NameAndIdInflectFileName = "BoxColliderNameAndIdInflect";

        [DisableInEditorMode]
        [LabelText("碰撞数据文件名称")]
        public string ColliderDataFileName = "BoxColliderData";

        [InlineEditor]
        [Required("需要至少一个Unity2D矩形碰撞器")]
        [BsonIgnore]
        [TabGroup("绘图相关内容")]
        public BoxCollider2D mCollider2D;

        [LabelText("矩形碰撞体数据")]
        [TabGroup("碰撞体数据")]
        public B2S_BoxColliderDataStructure MB2S_BoxColliderDataStructure = new B2S_BoxColliderDataStructure();

        private List<Vector2> points = new List<Vector2>();

        public override void InitColliderBaseInfo()
        {
            this.MB2S_BoxColliderDataStructure.b2SColliderType = B2S_ColliderType.BoxColllider;
        }

        [Button("重新绘制矩形碰撞体", 25), GUIColor(0.2f, 0.9f, 1.0f)]
        public override void InitPointInfo()
        {
            BoxCollider2D tempBox2D = this.mCollider2D;
            this.MB2S_BoxColliderDataStructure.hx = this.mCollider2D.bounds.size.x / 2;
            this.MB2S_BoxColliderDataStructure.hy = this.mCollider2D.bounds.size.y / 2;
            MB2S_BoxColliderDataStructure.offset.Fill(this.mCollider2D.offset);
            this.points.Clear();

            this.points.Add(new Vector2(-tempBox2D.bounds.size.x + tempBox2D.offset.x,
                -tempBox2D.bounds.size.y + tempBox2D.offset.y));
            this.points.Add(new Vector2(-tempBox2D.bounds.size.x + tempBox2D.offset.x,
                tempBox2D.bounds.size.y + tempBox2D.offset.y));
            this.points.Add(new Vector2(tempBox2D.bounds.size.x + tempBox2D.offset.x,
                tempBox2D.bounds.size.y + tempBox2D.offset.y));
            this.points.Add(new Vector2(tempBox2D.bounds.size.x + tempBox2D.offset.x,
                -tempBox2D.bounds.size.y + tempBox2D.offset.y));

            matrix4X4 = Matrix4x4.TRS(theObjectWillBeEdited.transform.position, theObjectWillBeEdited.transform.rotation,
                theObjectWillBeEdited.transform.parent.localScale);

            this.canDraw = true;
        }

        public override void DrawCollider()
        {
            if (this.mCollider2D is BoxCollider2D)

                for (int i = 0; i < this.points.Count; i++)
                {
                    if (i < this.points.Count - 1)
                        Gizmos.DrawLine(matrix4X4.MultiplyPoint(new Vector3(points[i].x, 0,
                                points[i].y)),
                            matrix4X4.MultiplyPoint(new Vector3(points[i + 1].x, 0,
                                points[i + 1].y)));
                    else
                    {
                        Gizmos.DrawLine(matrix4X4.MultiplyPoint(new Vector3(points[i].x, 0,
                                points[i].y)),
                            matrix4X4.MultiplyPoint(new Vector3(points[0].x, 0,
                                points[0].y)));
                    }
                }
        }

        [Button("保存所有矩形碰撞体名称与ID映射信息", 25), GUIColor(0.2f, 0.9f, 1.0f)]
        public override void SavecolliderNameAndIdInflect()
        {
            if (!this.MColliderNameAndIdInflectSupporter.colliderNameAndIdInflectDic.ContainsKey(this.theObjectWillBeEdited.transform.parent.name))
            {
                MColliderNameAndIdInflectSupporter.colliderNameAndIdInflectDic.Add(this.theObjectWillBeEdited.transform.parent.name,
                    this.MB2S_BoxColliderDataStructure.id);
            }
            else
            {
                MColliderNameAndIdInflectSupporter.colliderNameAndIdInflectDic[this.theObjectWillBeEdited.transform.parent.name] =
                        this.MB2S_BoxColliderDataStructure.id;
            }

            using (FileStream file = File.Create($"{this.NameAndIdInflectSavePath}/{this.NameAndIdInflectFileName}.bytes"))
            {
                BsonSerializer.Serialize(new BsonBinaryWriter(file), this.MColliderNameAndIdInflectSupporter);
            }
        }

        [Button("保存所有矩形碰撞体信息", 25), GUIColor(0.2f, 0.9f, 1.0f)]
        public override void SavecolliderData()
        {
            if (this.theObjectWillBeEdited != null && this.mCollider2D != null)
            {
                if (!this.MColliderDataSupporter.colliderDataDic.ContainsKey(this.MB2S_BoxColliderDataStructure.id))
                {
                    this.MColliderDataSupporter.colliderDataDic.Add(this.MB2S_BoxColliderDataStructure.id,
                        this.MB2S_BoxColliderDataStructure);
                }
                else
                {
                    this.MColliderDataSupporter.colliderDataDic[this.MB2S_BoxColliderDataStructure.id] =
                            this.MB2S_BoxColliderDataStructure;
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
                this.MB2S_BoxColliderDataStructure.id = 0;
                this.points.Clear();
                this.MB2S_BoxColliderDataStructure.isSensor = false;
                MB2S_BoxColliderDataStructure.offset.Clean();
                return;
            }

            if (this.MB2S_BoxColliderDataStructure.id == 0)
            {
                if (this.MColliderNameAndIdInflectSupporter.colliderNameAndIdInflectDic.TryGetValue(this.theObjectWillBeEdited.transform.parent.name,
                    out this.MB2S_BoxColliderDataStructure.id))
                {
                    Debug.Log($"自动设置矩形碰撞体ID成功，ID为{MB2S_BoxColliderDataStructure.id}");
                }

                if (this.MColliderDataSupporter.colliderDataDic.ContainsKey(this.MB2S_BoxColliderDataStructure.id))
                {
                    this.MB2S_BoxColliderDataStructure =
                            (B2S_BoxColliderDataStructure) this.MColliderDataSupporter.colliderDataDic[this.MB2S_BoxColliderDataStructure.id];
                }
            }

            if (mCollider2D == null)
            {
                mCollider2D = theObjectWillBeEdited.GetComponent<BoxCollider2D>();
                if (mCollider2D == null)
                {
                    this.canDraw = false;
                }
            }
        }

        public B2S_BoxColliderVisualHelper(ColliderNameAndIdInflectSupporter colliderNameAndIdInflectSupporter,
        ColliderDataSupporter colliderDataSupporter): base(colliderNameAndIdInflectSupporter, colliderDataSupporter)
        {
        }
    }
}