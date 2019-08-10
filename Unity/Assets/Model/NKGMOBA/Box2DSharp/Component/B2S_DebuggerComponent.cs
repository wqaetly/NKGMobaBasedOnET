//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年8月8日 21:11:46
//------------------------------------------------------------

using System.Collections.Generic;
using UnityEngine;

namespace ETModel
{
    /// <summary>
    /// Box2D的碰撞体可视化Debugger组件
    /// </summary>
    public class B2S_DebuggerComponent: Component
    {
        /// <summary>
        /// 直线渲染者,int为ID，bool为是否空闲
        /// </summary>
        public Dictionary<(int, GameObject), bool> m_LinerRenderersDic = new Dictionary<(int, GameObject), bool>();

        private GameObject targetGo; 

        /// <summary>
        /// 设置图形信息
        /// </summary>
        /// <param name="vector2s">顶点数组</param>
        /// <param name="sustainTime"></param>
        public void SetColliderInfo(Vector2[] vector2s, long sustainTime)
        {
            (int, GameObject) keyPair = this.SelectTargetGO();
            SetDeadLineTime(sustainTime, keyPair.Item1, keyPair.Item2).Coroutine();
            keyPair.Item2.GetComponent<B2S_Debugger>().Draw(vector2s.Length, vector2s);
        }

        private (int, GameObject) SelectTargetGO()
        {
            bool hasHandle = false;
            int m_id = 0;
            foreach (KeyValuePair<(int, GameObject), bool> myKeyValuePair in this.m_LinerRenderersDic)
            {
                if (myKeyValuePair.Value)
                {
                    hasHandle = true;
                    this.m_LinerRenderersDic[myKeyValuePair.Key] = false;
                    targetGo = myKeyValuePair.Key.Item2;
                    m_id = myKeyValuePair.Key.Item1;
                    break;
                }
            }

            
            if (!hasHandle)
            {
                int id = this.m_LinerRenderersDic.Count;
                GameObject newLineRenderer = GameObject.Instantiate(Resources.Load<GameObject>("Box2DDebuggerHandler"));
                this.m_LinerRenderersDic.Add((id, newLineRenderer), false);
                targetGo = newLineRenderer;
                m_id = id;
            }

            return (m_id, targetGo);
        }

        /// <summary>
        /// 设置绘制碰撞体持续时间
        /// </summary>
        /// <param name="time"></param>
        /// <param name="id"></param>
        /// <param name="targetGo"></param>
        /// <returns></returns>
        private async ETVoid SetDeadLineTime(long time, int id, GameObject targetGo)
        {
            await Game.Scene.GetComponent<TimerComponent>().WaitAsync(time);
            targetGo.GetComponent<B2S_Debugger>().StopDraw();
            m_LinerRenderersDic[(id, targetGo)] = true;
        }
    }
}