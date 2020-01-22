//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2020年1月22日 15:42:02
//------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using ETModel;
using FairyGUI;
using UnityEngine;

namespace ETHotfix
{
    [ObjectSystem]
    public class FallingFontComponentAwakeSystem: AwakeSystem<FallingFontComponent>
    {
        public override void Awake(FallingFontComponent self)
        {
            self.myHero = self.Entity as HotfixUnit;
        }
    }

    [ObjectSystem]
    public class FallingFontComponentUpdateSystem: UpdateSystem<FallingFontComponent>
    {
        public override void Update(FallingFontComponent self)
        {
            self.Update();
        }
    }

    /// <summary>
    /// 飘字组件，用于处理诸如伤害飘字。治疗飘字这种效果
    /// </summary>
    public class FallingFontComponent: Component
    {
        /// <summary>
        /// 预备中的飘字组件队列
        /// </summary>
        private Queue<FUIFallBleed.FUIFallBleed> FuiFallBleedQue = new Queue<FUIFallBleed.FUIFallBleed>();

        /// <summary>
        /// 运行中的飘字组件队列
        /// </summary>
        private Queue<FUIFallBleed.FUIFallBleed> RunnningFuiFallBleedQue = new Queue<FUIFallBleed.FUIFallBleed>();

        /// <summary>
        /// 已完成的飘字组件id队列
        /// </summary>
        private Queue<string> completedIdQueue = new Queue<string>();

        public HotfixUnit myHero;

        public void Add(FUIFallBleed.FUIFallBleed fuiFallBleed)
        {
            this.FuiFallBleedQue.Enqueue(fuiFallBleed);
        }

        /// <summary>
        /// 播放飘字特效
        /// </summary>
        /// <param name="targetValue">目标值</param>
        public void Play(float targetValue)
        {
            if (FuiFallBleedQue.Count == 0)
            {
                ETModel.Game.Scene.GetComponent<FUIPackageComponent>().AddPackage(FUIPackage.FUIFallBleed);
                var hotfixui = FUIFallBleed.FUIFallBleed.CreateInstance();
                hotfixui.Name = hotfixui.Id.ToString();
                hotfixui.MakeFullScreen();
                Game.Scene.GetComponent<FUIComponent>().Add(hotfixui, true);
                this.Add(hotfixui);
            }

            FUIFallBleed.FUIFallBleed fuiFallBleed = this.FuiFallBleedQue.Dequeue();
            fuiFallBleed.Tex_ValueToFall.text = targetValue.ToString();
            fuiFallBleed.self.visible = true;
            fuiFallBleed.FallingBleed.Play(CompleteCallBack);

            this.RunnningFuiFallBleedQue.Enqueue(fuiFallBleed);
            this.completedIdQueue.Enqueue(fuiFallBleed.Name);
            ETModel.Log.Info($"新建了一个飘字特效，id：{fuiFallBleed.Name}");
        }

        private void CompleteCallBack()
        {
            this.Recycle(Game.Scene.GetComponent<FUIComponent>().Get(this.completedIdQueue.Dequeue()) as FUIFallBleed.FUIFallBleed);
        }

        public void Recycle(FUIFallBleed.FUIFallBleed fuiFallBleed)
        {
            fuiFallBleed.self.visible = false;
            this.FuiFallBleedQue.Enqueue(fuiFallBleed);
            this.RunnningFuiFallBleedQue.Dequeue();
            ETModel.Log.Info($"回收了一个飘字特效，id：{fuiFallBleed.Name}");
        }

        /// <summary>
        /// 得到偏移的x
        /// </summary>
        /// <param name="barPos">血条的屏幕坐标</param>
        /// <returns></returns>
        private float GetOffsetX(Vector2 barPos)
        {
            float final = 100 + (Screen.width / 2.0f - barPos.x) * 0.05f;
            return final;
        }

        public void Update()
        {
            foreach (var VARIABLE in this.RunnningFuiFallBleedQue)
            {
                // 游戏物体的世界坐标转屏幕坐标
                Vector2 m_Hero2Screen =
                        Camera.main.WorldToScreenPoint(new Vector3(this.myHero.m_ModelUnit.Position.x, this.myHero.m_ModelUnit.Position.y,
                            this.myHero.m_ModelUnit.Position.z));

                // 屏幕坐标转FGUI全局坐标
                m_Hero2Screen.x = m_Hero2Screen.x;
                m_Hero2Screen.y = Screen.height - m_Hero2Screen.y;

                // FGUI全局坐标转头顶血条本地坐标
                VARIABLE.GObject.position = GRoot.inst.GlobalToLocal(m_Hero2Screen);
                VARIABLE.GObject.x -= GetOffsetX(m_Hero2Screen);
                VARIABLE.GObject.y -= 100;
            }
        }
    }
}