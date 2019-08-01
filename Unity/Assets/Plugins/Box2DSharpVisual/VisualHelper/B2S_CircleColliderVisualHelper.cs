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
        public string NameAndIdInflectFileName = "CircleColliderNameAndIdInflect";

        [DisableInEditorMode]
        [LabelText("碰撞数据文件名称")]
        public string ColliderDataFileName = "CircleColliderData";

        [InlineEditor]
        [Required("需要至少一个Unity2D圆形碰撞器")]
        [HideLabel]
        [BsonIgnore]
        public CircleCollider2D mCollider2D;

        [LabelText("圆形碰撞体数据")]
        public B2S_CircleColliderDataStructure MB2S_CircleColliderDataStructure = new B2S_CircleColliderDataStructure();

        [BsonIgnore]
        [LabelText("圆线段数")]
        public int Segments;

        public override void InitColliderBaseInfo()
        {
            this.MB2S_CircleColliderDataStructure.b2SColliderType = B2S_ColliderType.CircleCollider;
        }

        [Button("重新绘制圆形碰撞体", 25), GUIColor(0.2f, 0.9f, 1.0f)]
        public override void InitPointInfo()
        {
            matrix4X4 = Matrix4x4.TRS(theObjectWillBeEdited.transform.position, theObjectWillBeEdited.transform.rotation,
                theObjectWillBeEdited.transform.parent.localScale);
            this.MB2S_CircleColliderDataStructure.radius = this.mCollider2D.radius;
            MB2S_CircleColliderDataStructure.offset.Fill(this.mCollider2D.offset);
            this.canDraw = true;
        }

        public override void DrawCollider()
        {
            Vector3 tempVector3 = Vector3.forward * mCollider2D.radius;

            var originFrom = new Vector2(tempVector3.x + mCollider2D.offset.x,
                tempVector3.y + mCollider2D.offset.y);
            Vector3 finalFrom = matrix4X4.MultiplyPoint(new Vector3(originFrom.x, 0, originFrom.y));

            var step = Mathf.RoundToInt(360 / Segments);
            for (int i = 0; i <= 360; i += step)
            {
                tempVector3 = new Vector3(mCollider2D.radius * Mathf.Sin(i * Mathf.Deg2Rad),
                    mCollider2D.radius * Mathf.Cos(i * Mathf.Deg2Rad));

                var originTo = new Vector2(tempVector3.x + mCollider2D.offset.x,
                    tempVector3.y + mCollider2D.offset.y);
                var finalTo =
                        matrix4X4.MultiplyPoint(new Vector3(originTo.x, 0, originTo.y));

                Gizmos.DrawLine(finalFrom, finalTo);
                finalFrom = finalTo;
            }
        }

        [Button("保存所有圆形碰撞体名称与ID映射信息", 25), GUIColor(0.2f, 0.9f, 1.0f)]
        public override void SavecolliderNameAndIdInflect()
        {
            if (!this.MColliderNameAndIdInflectSupporter.colliderNameAndIdInflectDic.ContainsKey(this.theObjectWillBeEdited.name))
            {
                MColliderNameAndIdInflectSupporter.colliderNameAndIdInflectDic.Add(this.theObjectWillBeEdited.name,
                    this.MB2S_CircleColliderDataStructure.id);
            }
            else
            {
                MColliderNameAndIdInflectSupporter.colliderNameAndIdInflectDic[this.theObjectWillBeEdited.name] =
                        this.MB2S_CircleColliderDataStructure.id;
            }

            using (FileStream file = File.Create($"{this.NameAndIdInflectSavePath}/{this.NameAndIdInflectFileName}.bytes"))
            {
                BsonSerializer.Serialize(new BsonBinaryWriter(file), this.MColliderNameAndIdInflectSupporter);
            }
        }

        [Button("保存所有圆形碰撞体信息", 25), GUIColor(0.2f, 0.9f, 1.0f)]
        public override void SavecolliderData()
        {
            if (this.theObjectWillBeEdited != null && this.mCollider2D != null)
            {
                if (!this.MColliderDataSupporter.colliderDataDic.ContainsKey(this.MB2S_CircleColliderDataStructure.id))
                {
                    B2S_CircleColliderDataStructure b2SCircleColliderDataStructure = new B2S_CircleColliderDataStructure();
                    b2SCircleColliderDataStructure.radius = MB2S_CircleColliderDataStructure.radius;
                    this.MColliderDataSupporter.colliderDataDic.Add(this.MB2S_CircleColliderDataStructure.id,
                        b2SCircleColliderDataStructure);
                }
                else
                {
                    this.MColliderDataSupporter.colliderDataDic[this.MB2S_CircleColliderDataStructure.id] =
                            this.MB2S_CircleColliderDataStructure;
                }

                using (FileStream file = File.Create($"{this.ColliderDataSavePath}/{this.ColliderDataFileName}.bytes"))
                {
                    BsonSerializer.Serialize(new BsonBinaryWriter(file), this.MColliderDataSupporter);
                }
            }
        }

        [Button("清除所有圆形碰撞体信息", 25), GUIColor(1.0f, 20 / 255f, 147 / 255f)]
        public override void DeleteAllcolliderData()
        {
            this.MColliderDataSupporter.colliderDataDic.Clear();
            this.MColliderNameAndIdInflectSupporter.colliderNameAndIdInflectDic.Clear();

            using (FileStream file = File.Create($"{this.NameAndIdInflectSavePath}/{this.NameAndIdInflectFileName}.bytes"))
            {
                BsonSerializer.Serialize(new BsonBinaryWriter(file), this.MColliderNameAndIdInflectSupporter);
            }

            using (FileStream file = File.Create($"{this.ColliderDataSavePath}/{this.ColliderDataFileName}.bytes"))
            {
                BsonSerializer.Serialize(new BsonBinaryWriter(file), this.MColliderDataSupporter);
            }
        }

        [Button("清除此圆形碰撞体信息", 25), GUIColor(1.0f, 20 / 255f, 147 / 255f)]
        public override void DeletecolliderData()
        {
            if (this.theObjectWillBeEdited != null && this.mCollider2D != null)
            {
                if (this.MColliderDataSupporter.colliderDataDic.ContainsKey(this.MB2S_CircleColliderDataStructure.id))
                {
                    this.MColliderDataSupporter.colliderDataDic.Remove(this.MB2S_CircleColliderDataStructure.id);
                }

                if (this.MColliderNameAndIdInflectSupporter.colliderNameAndIdInflectDic.ContainsKey(this.theObjectWillBeEdited.name))
                {
                    this.MColliderNameAndIdInflectSupporter.colliderNameAndIdInflectDic.Remove(this.theObjectWillBeEdited.name);
                }

                using (FileStream file = File.Create($"{this.NameAndIdInflectSavePath}/{this.NameAndIdInflectFileName}.bytes"))
                {
                    BsonSerializer.Serialize(new BsonBinaryWriter(file), this.MColliderNameAndIdInflectSupporter);
                }

                using (FileStream file = File.Create($"{this.ColliderDataSavePath}/{this.ColliderDataFileName}.bytes"))
                {
                    BsonSerializer.Serialize(new BsonBinaryWriter(file), this.MColliderDataSupporter);
                }
            }
        }

        public override void OnUpdate()
        {
            if (CachedGameObject != theObjectWillBeEdited)
            {
                if (theObjectWillBeEdited != null)
                    CachedGameObject = theObjectWillBeEdited;
                this.ResetData();
                return;
            }

            if (theObjectWillBeEdited == null)
            {
                this.ResetData();
                return;
            }

            if (this.MB2S_CircleColliderDataStructure.id == 0)
            {
                if (this.MColliderNameAndIdInflectSupporter.colliderNameAndIdInflectDic.TryGetValue(this.theObjectWillBeEdited.name,
                    out this.MB2S_CircleColliderDataStructure.id))
                {
                    Debug.Log($"自动设置圆形碰撞体ID成功，ID为{MB2S_CircleColliderDataStructure.id}");
                }

                if (this.MColliderDataSupporter.colliderDataDic.ContainsKey(this.MB2S_CircleColliderDataStructure.id))
                {
                    this.MB2S_CircleColliderDataStructure =
                            (B2S_CircleColliderDataStructure) this.MColliderDataSupporter.colliderDataDic[this.MB2S_CircleColliderDataStructure.id];
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

        private void ResetData()
        {
            mCollider2D = null;
            this.canDraw = false;
            this.MB2S_CircleColliderDataStructure.id = 0;
            this.MB2S_CircleColliderDataStructure.radius = 0;
            MB2S_CircleColliderDataStructure.offset.Clean();
            this.MB2S_CircleColliderDataStructure.isSensor = false;
        }

        public B2S_CircleColliderVisualHelper(ColliderNameAndIdInflectSupporter colliderNameAndIdInflectSupporter,
        ColliderDataSupporter colliderDataSupporter): base(colliderNameAndIdInflectSupporter, colliderDataSupporter)
        {
        }
    }
}