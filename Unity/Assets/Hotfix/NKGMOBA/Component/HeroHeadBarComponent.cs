//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年5月31日 14:21:00
//------------------------------------------------------------

using ETHotfix;
using ETModel;
using FairyGUI;
using UnityEngine;

namespace ETHotfix
{
    [ObjectSystem]
    public class HeroHeadBarComponentAwakeSystem: AwakeSystem<HeroHeadBarComponent, Unit, FUI>
    {
        public override void Awake(HeroHeadBarComponent self, Unit m_Hero, FUI m_HeadBar)
        {
            self.Awake(m_Hero, m_HeadBar);
        }
    }

    [ObjectSystem]
    public class HeroHeadBarComponentUpdateSystem: UpdateSystem<HeroHeadBarComponent>
    {
        public override void Update(HeroHeadBarComponent self)
        {
            self.Update();
        }
    }

    /// <summary>
    /// 头部血条组件，负责血条的密度以及血条与人物的同步
    /// </summary>
    public class HeroHeadBarComponent: Component
    {
        private Unit m_Hero;
        private FUIHeadBar m_HeadBar;
        private Vector2 m_Hero2Screen;
        private Vector2 m_HeadBarScreenPos;

        public void Awake(Unit hero, FUI headBar)
        {
            this.m_Hero = hero;
            HeroDataComponent heroDataComponent = hero.GetComponent<HeroDataComponent>();
            this.m_HeadBar = headBar as FUIHeadBar;
            //这个血量最大值有点特殊，还需要设置一下密度用事件比较好一点

            this.SetDensityOfBar(this.m_Hero.GetComponent<HeroDataComponent>().GetAttribute(NumericType.MaxHp));
            this.m_HeadBar.Bar_HP.self.value = heroDataComponent.GetAttribute(NumericType.MaxHp);
            this.m_HeadBar.Bar_MP.self.max = heroDataComponent.GetAttribute(NumericType.MaxMp);
            this.m_HeadBar.Bar_MP.self.value = heroDataComponent.GetAttribute(NumericType.MaxMp);
        }

        public void Update()
        {
            // 游戏物体的世界坐标转屏幕坐标
            this.m_Hero2Screen =
                    Camera.main.WorldToScreenPoint(new Vector3(m_Hero.Position.x, this.m_Hero.Position.y, this.m_Hero.Position.z));

            // 屏幕坐标转FGUI全局坐标
            this.m_HeadBarScreenPos.x = m_Hero2Screen.x;
            this.m_HeadBarScreenPos.y = Screen.height - m_Hero2Screen.y;

            // FGUI全局坐标转头顶血条本地坐标
            this.m_HeadBar.GObject.position = GRoot.inst.GlobalToLocal(m_HeadBarScreenPos);

            // 血条本地坐标修正
            this.m_HeadBar.GObject.x -= GetOffsetX(m_HeadBarScreenPos);
            this.m_HeadBar.GObject.y -= 180;
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

        public void SetDensityOfBar(float maxHP)
        {
            float actual = 0;
            if (maxHP % 100 - 0 <= 0.1f)
            {
                actual = maxHP / 100 + 1;
            }
            else
            {
                actual = maxHP / 100 + 2;
            }

            this.m_HeadBar.Bar_HP.self.max = maxHP;
            this.m_HeadBar.HPGapList.numItems = (int) actual;
            this.m_HeadBar.HPGapList.columnGap = (int) (this.m_HeadBar.HPGapList.actualWidth / (actual - 1));
        }
    }
}