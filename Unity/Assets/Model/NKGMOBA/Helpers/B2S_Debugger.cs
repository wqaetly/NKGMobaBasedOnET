//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年8月8日 20:40:28
//------------------------------------------------------------

using System.Collections.Generic;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ETModel
{
    public class B2S_Debugger: SerializedMonoBehaviour
    {
        //线段渲染器  
        public LineRenderer lineRenderer;

        public List<Vector3> mVexs = new List<Vector3>();

        public bool canDraw;

        private void Awake()
        {
            //通过游戏对象，GetComponent方法 传入LineRenderer  
            //就是之前给line游戏对象添加的渲染器属性  
            //有了这个对象才可以为游戏世界渲染线段  
            lineRenderer = this.transform.GetComponent<LineRenderer>();

            this.canDraw = false;

            this.lineRenderer.SetWidth(0.05f, 0.05f);

            this.lineRenderer.SetColors(Color.red, Color.red);
        }

        private void Update()
        {
            if (this.canDraw)
            {
                this.lineRenderer.SetPositions(this.mVexs.ToArray());
            }
        }

        /// <summary>
        /// 绘制圆形
        /// </summary>
        /// <param name="pointCount"></param>
        /// <param name="vexs"></param>
        public void Draw(int pointCount, UnityEngine.Vector2[] vexs)
        {
            //设置线段长度，这个数值须要和绘制线3D点的数量想等  
            //否则会抛异常～～  
            lineRenderer.SetVertexCount(pointCount);

            this.mVexs.Clear();

            foreach (var vex in vexs)
            {
                mVexs.Add(new Vector3(vex.x, 1.0f, vex.y));
            }

            mVexs.Add(mVexs[0]);

            this.canDraw = true;
        }

        public void StopDraw()
        {
            this.canDraw = false;
        }
    }
}