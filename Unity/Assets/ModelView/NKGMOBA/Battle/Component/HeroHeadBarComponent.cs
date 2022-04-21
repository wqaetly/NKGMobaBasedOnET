//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年5月31日 14:21:00
//------------------------------------------------------------

using FairyGUI;
using UnityEngine;

namespace ET
{
    /// <summary>
    /// 头部血条组件，负责血条的密度以及血条与人物的同步
    /// </summary>
    public class HeroHeadBarComponent: Entity
    {
        public Unit Hero;
        public FUI_HeadBar m_HeadBar;
        public Vector2 m_Hero2Screen;
        public Vector2 m_HeadBarScreenPos;
        public Renderer m_HeadBarGapRender;
        public static readonly int UVStart = Shader.PropertyToID("UVStart");
        public static readonly int UVFactor = Shader.PropertyToID("UVFactor");
        public static readonly int PerSplitWidth = Shader.PropertyToID("PerSplitWidth");

        public override void Dispose()
        {
            if (this.IsDisposed) return;
            base.Dispose();
            this.Hero = null;
            m_HeadBar = null;
            m_Hero2Screen = Vector2.zero;
            this.m_HeadBarScreenPos = Vector2.zero;
            this.m_HeadBarGapRender = null;
        }
    }
}