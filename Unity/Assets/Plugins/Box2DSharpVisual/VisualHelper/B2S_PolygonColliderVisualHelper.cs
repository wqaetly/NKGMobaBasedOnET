//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年7月10日 21:02:18
//------------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Linq;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using Sirenix.OdinInspector;
using UnityEngine;
using Vector2 = System.Numerics.Vector2;

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

        [LabelText("请设置每个多边形最大顶点数")]
        [Range(3, 8)]
        public int MaxPointLimit;

        [InlineEditor]
        [Required("需要至少一个Unity2D多边形碰撞器")]
        [HideLabel]
        public PolygonCollider2D mCollider2D;

        public B2S_PolygonColliderDataStructure MB2S_PolygonColliderDataStructure = new B2S_PolygonColliderDataStructure();

        public override void InitColliderBaseInfo()
        {
            this.MB2S_PolygonColliderDataStructure.b2SColliderType = B2S_ColliderType.PolygonCollider;
        }

        [Button("重新绘制多边形碰撞体", 25), GUIColor(0.2f, 0.9f, 1.0f)]
        public override void InitPointInfo()
        {
            CheckPolygon();

            matrix4X4 = Matrix4x4.TRS(theObjectWillBeEdited.transform.position, theObjectWillBeEdited.transform.rotation,
                theObjectWillBeEdited.transform.parent.localScale);

            MB2S_PolygonColliderDataStructure.pointCount = mCollider2D.GetTotalPointCount();
            MB2S_PolygonColliderDataStructure.finalOffset.X = this.mCollider2D.offset.x;
            MB2S_PolygonColliderDataStructure.finalOffset.Y = this.mCollider2D.offset.y;
            this.canDraw = true;
        }

        public override void DrawCollider()
        {
            foreach (var VARIABLE in this.MB2S_PolygonColliderDataStructure.finalPoints)
            {
                for (int i = 0; i < VARIABLE.Count; i++)
                {
                    if (i < VARIABLE.Count - 1)
                        Gizmos.DrawLine(matrix4X4.MultiplyPoint(new Vector3(VARIABLE[i].X + this.mCollider2D.offset.x,
                                0,
                                VARIABLE[i].Y + this.mCollider2D.offset.y)),
                            matrix4X4.MultiplyPoint(new Vector3(VARIABLE[i + 1].X + this.mCollider2D.offset.x, 0,
                                VARIABLE[i + 1].Y + this.mCollider2D.offset.y)));
                    else
                    {
                        Gizmos.DrawLine(matrix4X4.MultiplyPoint(new Vector3(VARIABLE[i].X + this.mCollider2D.offset.x,
                                0,
                                VARIABLE[i].Y + this.mCollider2D.offset.y)),
                            matrix4X4.MultiplyPoint(new Vector3(VARIABLE[0].X + this.mCollider2D.offset.x, 0,
                                VARIABLE[0].Y + this.mCollider2D.offset.y)));
                    }
                }
            }
        }

        [Button("保存所有多边形碰撞体名称与ID映射信息", 25), GUIColor(0.2f, 0.9f, 1.0f)]
        public override void SavecolliderNameAndIdInflect()
        {
            if (!this.MColliderNameAndIdInflectSupporter.colliderNameAndIdInflectDic.ContainsKey(this.theObjectWillBeEdited.name))
            {
                MColliderNameAndIdInflectSupporter.colliderNameAndIdInflectDic.Add(this.theObjectWillBeEdited.name,
                    this.MB2S_PolygonColliderDataStructure.id);
            }
            else
            {
                MColliderNameAndIdInflectSupporter.colliderNameAndIdInflectDic[this.theObjectWillBeEdited.name] =
                        this.MB2S_PolygonColliderDataStructure.id;
            }

            using (FileStream file = File.Create($"{this.NameAndIdInflectSavePath}/{this.NameAndIdInflectFileName}.bytes"))
            {
                BsonSerializer.Serialize(new BsonBinaryWriter(file), this.MColliderNameAndIdInflectSupporter);
            }
        }

        public void CheckPolygon()
        {
            MB2S_PolygonColliderDataStructure.pointCount = 0;
            this.MB2S_PolygonColliderDataStructure.finalPoints.Clear();

            //对多边形进行分割操作
            List<Vector2> tempPoints = new List<Vector2>();
            //必须进行放缩操作，不然在很近的时候运算误差会导致出错
            foreach (var VARIABLE in this.mCollider2D.points)
            {
                Vector2 tempVector2 = new Vector2(VARIABLE.x * 30, VARIABLE.y * 30);
                tempPoints.Add(tempVector2);
            }

            List<List<Vector2>> tempFinalPolygons = Separator.CalcShapes(tempPoints);

            foreach (var VARIABLE in tempFinalPolygons)
            {
                for (int i = 0; i < VARIABLE.Count; i++)
                {
                    VARIABLE[i] = new Vector2(VARIABLE[i].X / 30, VARIABLE[i].Y / 30);
                }
            }

            List<List<Vector2>> FinalPolygons = Separator.SplitPolygonUntilLessX(this.MaxPointLimit, tempFinalPolygons);

            for (int i = 0; i < FinalPolygons.Count; i++)
            {
                this.MB2S_PolygonColliderDataStructure.finalPoints.Add(new List<Vector2>());
                for (int j = 0; j < FinalPolygons[i].Count; j++)
                {
                    this.MB2S_PolygonColliderDataStructure.finalPoints[i].Add(new Vector2(FinalPolygons[i][j].X, FinalPolygons[i][j].Y));
                }
            }
        }

        [Button("保存所有多边形碰撞体信息", 25), GUIColor(0.2f, 0.9f, 1.0f)]
        public override void SavecolliderData()
        {
            if (this.theObjectWillBeEdited != null && this.mCollider2D != null)
            {
                if (!this.MColliderDataSupporter.colliderDataDic.ContainsKey(this.MB2S_PolygonColliderDataStructure.id))
                {
                    B2S_PolygonColliderDataStructure temp = new B2S_PolygonColliderDataStructure();
                    temp.id = MB2S_PolygonColliderDataStructure.id;
                    temp.finalOffset.X = MB2S_PolygonColliderDataStructure.finalOffset.X;
                    temp.finalOffset.Y = MB2S_PolygonColliderDataStructure.finalOffset.Y;
                    temp.isSensor = MB2S_PolygonColliderDataStructure.isSensor;
                    temp.b2SColliderType = MB2S_PolygonColliderDataStructure.b2SColliderType;
                    for (int i = 0; i < this.MB2S_PolygonColliderDataStructure.finalPoints.Count; i++)
                    {
                        temp.finalPoints.Add(new List<Vector2>());
                        for (int j = 0; j < this.MB2S_PolygonColliderDataStructure.finalPoints[i].Count; j++)
                        {
                            Vector2 costumVector2 = new Vector2(this.MB2S_PolygonColliderDataStructure.finalPoints[i][j].X,
                                this.MB2S_PolygonColliderDataStructure.finalPoints[i][j].Y);
                            temp.finalPoints[i].Add(costumVector2);
                        }
                    }

                    temp.pointCount = this.MB2S_PolygonColliderDataStructure.pointCount;
                    this.MColliderDataSupporter.colliderDataDic.Add(this.MB2S_PolygonColliderDataStructure.id,
                        temp);
                }
                else
                {
                    this.MColliderDataSupporter.colliderDataDic[this.MB2S_PolygonColliderDataStructure.id] =
                            this.MB2S_PolygonColliderDataStructure;
                }
            }

            using (FileStream file = File.Create($"{this.ColliderDataSavePath}/{this.ColliderDataFileName}.bytes"))
            {
                BsonSerializer.Serialize(new BsonBinaryWriter(file), this.MColliderDataSupporter);
            }
        }

        [Button("清除所有多边形碰撞体信息", 25), GUIColor(1.0f, 20 / 255f, 147 / 255f)]
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

        [Button("清除此多边形碰撞体信息", 25), GUIColor(1.0f, 20 / 255f, 147 / 255f)]
        public override void DeletecolliderData()
        {
            if (this.theObjectWillBeEdited != null && this.mCollider2D != null)
            {
                if (this.MColliderDataSupporter.colliderDataDic.ContainsKey(this.MB2S_PolygonColliderDataStructure.id))
                {
                    this.MColliderDataSupporter.colliderDataDic.Remove(this.MB2S_PolygonColliderDataStructure.id);
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
                ResetData();
                return;
            }

            if (theObjectWillBeEdited == null)
            {
                ResetData();
                CachedGameObject = null;
                return;
            }

            if (mCollider2D == null)
            {
                mCollider2D = theObjectWillBeEdited.GetComponent<PolygonCollider2D>();
                if (mCollider2D == null)
                {
                    this.canDraw = false;
                }
            }

            if (this.MB2S_PolygonColliderDataStructure.id == 0)
            {
                this.MColliderNameAndIdInflectSupporter.colliderNameAndIdInflectDic.TryGetValue(this.theObjectWillBeEdited.name,
                    out this.MB2S_PolygonColliderDataStructure.id);

                if (this.MColliderDataSupporter.colliderDataDic.ContainsKey(this.MB2S_PolygonColliderDataStructure.id))
                {
                    this.MB2S_PolygonColliderDataStructure =
                            (B2S_PolygonColliderDataStructure) this.MColliderDataSupporter.colliderDataDic[this.MB2S_PolygonColliderDataStructure.id];
                }
            }
        }

        private void ResetData()
        {
            mCollider2D = null;
            this.canDraw = false;
            this.MB2S_PolygonColliderDataStructure.id = 0;
            this.MB2S_PolygonColliderDataStructure.isSensor = false;
            this.MB2S_PolygonColliderDataStructure.finalOffset = Vector2.Zero;
        }

        public B2S_PolygonColliderVisualHelper(ColliderNameAndIdInflectSupporter colliderNameAndIdInflectSupporter,
        ColliderDataSupporter colliderDataSupporter): base(colliderNameAndIdInflectSupporter, colliderDataSupporter)
        {
        }
    }
}