//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年7月10日 21:02:18
//------------------------------------------------------------

using System.IO;
using System.Linq;
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
        public string FileName = "PolygonColliderNameAndIdInflect";
        
        [InlineEditor]
        [Required("需要至少一个Unity2D多边形碰撞器")]
        [HideLabel]
        [BsonIgnore]
        public PolygonCollider2D mCollider2D;


        
        public override void InitColliderBaseInfo()
        {
            this.mColliderShape = B2S_ColliderType.PolygonCollider;
        }

        [Button("重新绘制多边形碰撞体", 25), GUIColor(0.2f, 0.9f, 1.0f)]
        public override void InitPointInfo()
        {
            matrix4X4 = Matrix4x4.TRS(theObjectWillBeEdited.transform.position, theObjectWillBeEdited.transform.rotation,
                theObjectWillBeEdited.transform.parent.localScale);
            points = mCollider2D.points.ToList();
            pointCount = mCollider2D.GetTotalPointCount();
            this.canDraw = true;
        }

        public override void DrawCollider()
        {
            for (int i = 0; i < pointCount; i++)
            {
                if (i < pointCount - 1)
                    Gizmos.DrawLine(
                        matrix4X4.MultiplyPoint(new Vector3(points[i].x + this.mCollider2D.offset.x, 0, points[i].y + this.mCollider2D.offset.y)),
                        matrix4X4.MultiplyPoint(new Vector3(points[i + 1].x + this.mCollider2D.offset.x, 0,
                            points[i + 1].y + this.mCollider2D.offset.y)));
                else
                {
                    Gizmos.DrawLine(
                        matrix4X4.MultiplyPoint(new Vector3(points[i].x + this.mCollider2D.offset.x, 0, points[i].y + this.mCollider2D.offset.y)),
                        matrix4X4.MultiplyPoint(new Vector3(points[0].x + this.mCollider2D.offset.x, 0,
                            points[0].y + this.mCollider2D.offset.y)));
                }
            }
        }

        [Button("保存所有多边形碰撞体名称与ID映射信息", 25), GUIColor(0.2f, 0.9f, 1.0f)]
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
                return;
            }

            if (this.id == 0)
            {
                if (this.McolliderNameAndIdInflectSupporter.colliderNameAndIdInflectDic.TryGetValue(this.theObjectWillBeEdited.transform.parent.name,
                    out this.id))
                {
                    Debug.Log($"自动设置圆形碰撞体ID成功，ID为{id}");
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

        public B2S_PolygonColliderVisualHelper(ColliderNameAndIdInflectSupporter colliderNameAndIdInflectSupporter): base(
            colliderNameAndIdInflectSupporter)
        {
        }
    }
}