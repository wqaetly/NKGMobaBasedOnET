//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年7月10日 21:01:06
//------------------------------------------------------------

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
        public string FileName = "BoxColliderNameAndIdInflect";
        
        [InlineEditor]
        [Required("需要至少一个Unity2D矩形碰撞器")]
        [BsonIgnore]
        public BoxCollider2D mCollider2D;


        public override void InitColliderBaseInfo()
        {
            this.mColliderShape = B2S_ColliderType.BoxColllider;
        }

        [Button("重新绘制矩形碰撞体", 25), GUIColor(0.2f, 0.9f, 1.0f)]
        public override void InitPointInfo()
        {
            this.points.Clear();
            BoxCollider2D tempBox2D = this.mCollider2D;
            Vector2 box2DSize = new Vector2(1 / this.theObjectWillBeEdited.transform.parent.localScale.x,
                1 / this.theObjectWillBeEdited.transform.parent.localScale.y);
            this.points.Add(new Vector2(-tempBox2D.bounds.size.x * box2DSize.x / 2 + tempBox2D.offset.x,
                -tempBox2D.bounds.size.y * box2DSize.y / 2 + tempBox2D.offset.y));
            this.points.Add(new Vector2(-tempBox2D.bounds.size.x * box2DSize.x / 2 + tempBox2D.offset.x,
                tempBox2D.bounds.size.y * box2DSize.y / 2 + tempBox2D.offset.y));
            this.points.Add(new Vector2(tempBox2D.bounds.size.x * box2DSize.x / 2 + tempBox2D.offset.x,
                tempBox2D.bounds.size.y * box2DSize.y / 2 + tempBox2D.offset.y));
            this.points.Add(new Vector2(tempBox2D.bounds.size.x * box2DSize.x / 2 + tempBox2D.offset.x,
                -tempBox2D.bounds.size.y * box2DSize.y / 2 + tempBox2D.offset.y));

            matrix4X4 = Matrix4x4.TRS(theObjectWillBeEdited.transform.position, theObjectWillBeEdited.transform.rotation,
                theObjectWillBeEdited.transform.parent.localScale);
            this.pointCount = this.points.Count;
            this.canDraw = true;
        }

        public override void DrawCollider()
        {
            if (this.mCollider2D is BoxCollider2D)

                for (int i = 0; i < pointCount; i++)
                {
                    if (i < pointCount - 1)
                        Gizmos.DrawLine(matrix4X4.MultiplyPoint(new Vector3(points[i].x, 0, points[i].y)),
                            matrix4X4.MultiplyPoint(new Vector3(points[i + 1].x, 0, points[i + 1].y)));
                    else
                    {
                        Gizmos.DrawLine(matrix4X4.MultiplyPoint(new Vector3(points[i].x, 0, points[i].y)),
                            matrix4X4.MultiplyPoint(new Vector3(points[0].x, 0, points[0].y)));
                    }
                }
        }

        [Button("保存所有矩形碰撞体名称与ID映射信息", 25), GUIColor(0.2f, 0.9f, 1.0f)]
        public override void SavecolliderNameAndIdInflect()
        {
            if (!this.McolliderNameAndIdInflectSupporter.colliderNameAndIdInflectDic.ContainsKey(this.theObjectWillBeEdited.transform.parent.name))
            {
                McolliderNameAndIdInflectSupporter.colliderNameAndIdInflectDic.Add(this.theObjectWillBeEdited.transform.parent.name, this.id);
            }
            else
            {
                McolliderNameAndIdInflectSupporter.colliderNameAndIdInflectDic[this.theObjectWillBeEdited.transform.parent.name] = this.id;
            }

            using (FileStream file = File.Create($"{SavePath}/{this.FileName}.bytes"))
            {
                BsonSerializer.Serialize(new BsonBinaryWriter(file), this.McolliderNameAndIdInflectSupporter);
            }
        }

        public override void OnUpdate()
        {
            if (theObjectWillBeEdited == null)
            {
                mCollider2D = null;
                this.canDraw = false;
                this.id = 0;
                return;
            }

            if (this.id == 0)
            {
                if (this.McolliderNameAndIdInflectSupporter.colliderNameAndIdInflectDic.TryGetValue(this.theObjectWillBeEdited.transform.parent.name,
                    out this.id))
                {
                    Debug.Log($"自动设置矩形碰撞体ID成功，ID为{id}");
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

        public B2S_BoxColliderVisualHelper(ColliderNameAndIdInflectSupporter colliderNameAndIdInflectSupporter): base(
            colliderNameAndIdInflectSupporter)
        {
        }
    }
}