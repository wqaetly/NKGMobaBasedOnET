//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年7月10日 21:01:36
//------------------------------------------------------------

using System.IO;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ETModel
{
    /// <summary>
    /// 圆形可视化辅助
    /// </summary>
    public class B2S_CircleColliderVisualHelper: B2S_ColliderVisualHelperBase
    {
        [DisableInEditorMode]
        [LabelText("映射文件名称")]
        public string FileName = "CircleColliderNameAndIdInflect";
        
        [InlineEditor]
        [Required("需要至少一个Unity2D圆形碰撞器")]
        [HideLabel]
        [BsonIgnore]
        public CircleCollider2D mCollider2D;

        [BsonIgnore]
        [LabelText("圆线段数")]
        public int Segments;

        private CircleCollider2D mCircleCollider2D;


        public override void InitColliderBaseInfo()
        {
            this.mColliderShape = B2S_ColliderType.CircleCollider;
        }

        [Button("重新绘制圆形碰撞体", 25), GUIColor(0.2f, 0.9f, 1.0f)]
        public override void InitPointInfo()
        {
            mCircleCollider2D = mCollider2D;
            matrix4X4 = Matrix4x4.TRS(theObjectWillBeEdited.transform.position, theObjectWillBeEdited.transform.rotation,
                theObjectWillBeEdited.transform.parent.localScale);
            this.canDraw = true;
        }

        public override void DrawCollider()
        {
            Vector3 tempVector3 = Vector3.forward * mCircleCollider2D.radius;

            var originFrom = new Vector2(tempVector3.x + mCircleCollider2D.offset.x,
                tempVector3.y + mCircleCollider2D.offset.y);
            Vector3 finalFrom = matrix4X4.MultiplyPoint(new Vector3(originFrom.x, 0, originFrom.y));

            var step = Mathf.RoundToInt(360 / Segments);
            for (int i = 0; i <= 360; i += step)
            {
                tempVector3 = new Vector3(mCircleCollider2D.radius * Mathf.Sin(i * Mathf.Deg2Rad),
                    mCircleCollider2D.radius * Mathf.Cos(i * Mathf.Deg2Rad));

                var originTo = new Vector2(tempVector3.x + mCircleCollider2D.offset.x,
                    tempVector3.y + mCircleCollider2D.offset.y);
                var finalTo =
                        matrix4X4.MultiplyPoint(new Vector3(originTo.x, 0, originTo.y));

                Gizmos.DrawLine(finalFrom, finalTo);
                finalFrom = finalTo;
            }
        }


        [Button("保存所有圆形碰撞体名称与ID映射信息", 25), GUIColor(0.2f, 0.9f, 1.0f)]
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
                mCollider2D = theObjectWillBeEdited.GetComponent<CircleCollider2D>();
                if (mCollider2D == null)
                {
                    this.canDraw = false;
                }
            }
        }

        public B2S_CircleColliderVisualHelper(ColliderNameAndIdInflectSupporter colliderNameAndIdInflectSupporter): base(colliderNameAndIdInflectSupporter)
        {
        }
    }
}