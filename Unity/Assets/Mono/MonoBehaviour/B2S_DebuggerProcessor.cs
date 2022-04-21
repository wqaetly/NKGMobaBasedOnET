//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年8月8日 20:40:28
//------------------------------------------------------------

using System.Collections.Generic;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace ET
{
    public class B2S_DebuggerProcessor : MonoBehaviour
    {
        //线段渲染器  
        public LineRenderer lineRenderer;
        public Vector3[] Vexs;

        private void Awake()
        {
            lineRenderer = this.gameObject.AddComponent<LineRenderer>();
            lineRenderer.material = new Material(Shader.Find("Sprites/Default"));

            this.lineRenderer.SetWidth(0.05f, 0.05f);
            this.lineRenderer.SetColors(Color.red, Color.red);
        }

        public void Init(Vector3[] vexs)
        {
            // 通过引用的方式索引外部的顶点列表，这样在外部的定点列表刷新时，LineRender可以自动刷新绘制
            Vexs = vexs;
            //设置线段长度，这个数值须要和绘制线3D点的数量想等  
            //否则会抛异常～～  
            lineRenderer.positionCount = vexs.Length;
        }

        private void Update()
        {
            this.lineRenderer.SetPositions(this.Vexs);
        }
    }
}