//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年7月13日 21:31:25
//------------------------------------------------------------

using System.Collections.Generic;
using System.ComponentModel;
using ETModel;
using Sirenix.OdinInspector;
using UnityEngine;
using Vector2 = System.Numerics.Vector2;

namespace ETModel
{
#if SERVER
    public class B2S_PolygonColliderDataStructure: B2S_ColliderDataStructureBase, ISupportInitialize
#else
    public class B2S_PolygonColliderDataStructure: B2S_ColliderDataStructureBase
#endif
    {
        [LabelText("碰撞体所包含的顶点信息(顺时针),可能由多个多边形组成")]
        [DisableInEditorMode]
        public List<List<CostumVector2>> points = new List<List<CostumVector2>>();
        
#if SERVER
        /// <summary>
        /// 最终在服务端读取的数据表
        /// </summary>
        public List<List<Vector2>> finalPoints = new List<List<Vector2>>();
#endif
        
        [LabelText("总顶点数")]
        [DisableInEditorMode]
        public int pointCount;

#if SERVER
        public void BeginInit()
        {
        }

        public void EndInit()
        {
            for (int i = 0; i < points.Count; i++)
            {
                this.finalPoints.Add(new List<Vector2>());
                for (int j = 0; j < points[i].Count; j++)
                {
                    this.finalPoints[i].Add(this.points[i][j].ToSystemVector2());
                    //Log.Info($"反序列化完成,值为{this.finalPoints[i][j]}");
                }
            }
            
            this.finalOffset = new Vector2(this.offset.x,this.offset.y);
        }
#endif
    }
}