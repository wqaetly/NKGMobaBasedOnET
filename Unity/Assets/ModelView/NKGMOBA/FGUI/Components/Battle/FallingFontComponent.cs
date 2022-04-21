using System.Collections;
using System.Collections.Generic;
using ET.EventType;
using FairyGUI;
using UnityEngine;

namespace ET
{
    public class ChangeUnitAttribute_FallFront : AEvent<EventType.NumericApplyChangeValue>
    {
        protected override async ETTask Run(NumericApplyChangeValue a)
        {
            if (a.NumericType == NumericType.Hp)
            {
                a.Unit.GetComponent<FallingFontComponent>().Play((int) a.ChangedValue);
            }

            await ETTask.CompletedTask;
        }
    }

    [ObjectSystem]
    public class FallingFontComponentAwakeSystem : AwakeSystem<FallingFontComponent>
    {
        public override void Awake(FallingFontComponent self)
        {
            self.myHero = self.GetParent<Unit>();
            self.FuiManagerComponent = self.DomainScene().GetComponent<FUIManagerComponent>();
            self.FuiPackageManagerComponent = self.DomainScene().GetComponent<FUIPackageManagerComponent>();
        }
    }

    [ObjectSystem]
    public class FallingFontComponentUpdateSystem : UpdateSystem<FallingFontComponent>
    {
        public override void Update(FallingFontComponent self)
        {
            self.Update();
        }
    }

    /// <summary>
    /// 飘字组件，用于处理诸如伤害飘字。治疗飘字这种效果
    /// </summary>
    public class FallingFontComponent : Entity
    {
        /// <summary>
        /// 预备中的飘字组件队列
        /// </summary>
        private Queue<FUI_FlyFont> FuiFallBleedQue = new Queue<FUI_FlyFont>();

        /// <summary>
        /// 运行中的飘字组件队列
        /// </summary>
        private Queue<FUI_FlyFont> RunnningFuiFallBleedQue = new Queue<FUI_FlyFont>();

        /// <summary>
        /// 已完成的飘字组件id队列
        /// </summary>
        private Queue<string> completedIdQueue = new Queue<string>();

        public Unit myHero;

        public FUIManagerComponent FuiManagerComponent;

        public FUIPackageManagerComponent FuiPackageManagerComponent;

        public void Add(FUI_FlyFont fuiFallBleed)
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
                var hotfixui = FUI_FlyFont.CreateInstance(this.DomainScene());
                hotfixui.Name = hotfixui.Id.ToString();
                hotfixui.MakeFullScreen();
                this.DomainScene().GetComponent<FUIManagerComponent>().Add(hotfixui.Name, hotfixui, hotfixui, true);
                this.Add(hotfixui);
            }

            FUI_FlyFont fuiFallBleed = this.FuiFallBleedQue.Dequeue();
            fuiFallBleed.m_Tex_ValueToFall.text = targetValue.ToString();
            fuiFallBleed.self.visible = true;
            fuiFallBleed.m_FallingBleed.Play(CompleteCallBack);

            this.RunnningFuiFallBleedQue.Enqueue(fuiFallBleed);
            this.completedIdQueue.Enqueue(fuiFallBleed.Name);
            //ET.Log.Info($"新建了一个飘字特效，id：{fuiFallBleed.Name}");
        }

        private void CompleteCallBack()
        {
            this.Recycle(this.FuiManagerComponent.GetFUIComponent<FUI_FlyFont>(completedIdQueue.Dequeue()));
        }

        public void Recycle(FUI_FlyFont fuiFallBleed)
        {
            fuiFallBleed.self.visible = false;
            this.FuiFallBleedQue.Enqueue(fuiFallBleed);
            this.RunnningFuiFallBleedQue.Dequeue();
            //ET.Log.Info($"回收了一个飘字特效，id：{fuiFallBleed.Name}");
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
            foreach (var fuiFallBleed in this.RunnningFuiFallBleedQue)
            {
                // 游戏物体的世界坐标转屏幕坐标
                Vector2 m_Hero2Screen =
                    Camera.main.WorldToScreenPoint(new Vector3(this.myHero.Position.x, this.myHero.Position.y,
                        this.myHero.Position.z));

                // 屏幕坐标转FGUI全局坐标
                m_Hero2Screen.y = Screen.height - m_Hero2Screen.y;

                // FGUI全局坐标转头顶血条本地坐标
                fuiFallBleed.GObject.position = GRoot.inst.GlobalToLocal(m_Hero2Screen);
                fuiFallBleed.GObject.x -= GetOffsetX(m_Hero2Screen);
                fuiFallBleed.GObject.y -= 100;
            }
        }

        public override void Dispose()
        {
            if (this.IsDisposed) return;
            base.Dispose();
            this.completedIdQueue.Clear();
            this.FuiFallBleedQue.Clear();
            this.RunnningFuiFallBleedQue.Clear();
            this.myHero.Dispose();
            this.myHero = null;
        }
    }
}