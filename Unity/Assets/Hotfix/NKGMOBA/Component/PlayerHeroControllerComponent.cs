//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年7月3日 17:44:51
//------------------------------------------------------------

using System;
using ETModel;

namespace ETHotfix
{
    [ObjectSystem]
    public class PlayerHeroControllerAwakeSystem: AwakeSystem<PlayerHeroControllerComponent>
    {
        public override void Awake(PlayerHeroControllerComponent self)
        {
            self.Awake();
        }
    }

    [ObjectSystem]
    public class PlayerHeroControllerUpdateSystem: UpdateSystem<PlayerHeroControllerComponent>
    {
        public override void Update(PlayerHeroControllerComponent self)
        {
            self.Update();
        }
    }

    /// <summary>
    /// 玩家自己操控英雄组件
    /// </summary>
    public class PlayerHeroControllerComponent: Component
    {
        private UserInputComponent userInputComponent;

        private HeroTransformComponent heroTransformComponent;

        private CDInfo m_QCDInfo;
        private CDInfo m_WCDInfo;
        private CDInfo m_ECDInfo;
        private FUI5V5Map fui5V5Map;

        private CDComponent m_CDComponent;

        public void Awake()
        {
            this.heroTransformComponent = this.GetParent<HotfixUnit>().m_ModelUnit.GetComponent<HeroTransformComponent>();
            this.userInputComponent = ETModel.Game.Scene.GetComponent<UserInputComponent>();
            this.m_CDComponent = ETModel.Game.Scene.GetComponent<CDComponent>();
            m_QCDInfo = ReferencePool.Acquire<CDInfo>();
            m_QCDInfo.Interval = 5000;
            m_QCDInfo.Name = "QCD";
            m_WCDInfo = ReferencePool.Acquire<CDInfo>();
            m_WCDInfo.Interval = 7000;
            m_WCDInfo.Name = "WCD";
            m_ECDInfo = ReferencePool.Acquire<CDInfo>();
            m_ECDInfo.Interval = 10000;
            m_ECDInfo.Name = "ECD";
            m_CDComponent.AddCDData(this.Entity.Id, m_QCDInfo);
            m_CDComponent.AddCDData(this.Entity.Id, m_WCDInfo);
            m_CDComponent.AddCDData(this.Entity.Id, m_ECDInfo);
        }

        public void Update()
        {
            //TODO 先硬编码一波，这一块要放到行为树去处理的
            fui5V5Map = Game.Scene.GetComponent<FUIComponent>().Get(FUI5V5Map.UIPackageName) as FUI5V5Map;
            if (fui5V5Map == null)
            {
                return;
            }

            if (this.userInputComponent.QDown)
            {
                SessionComponent.Instance.Session.Send(new UserInput_SkillCmd() { Message = "Q" });
                if (fui5V5Map.SkillQ_CDInfo.visible) return;
                fui5V5Map.SkillQ_CDInfo.text = "5";
                fui5V5Map.SkillQ_CDInfo.visible = true;
                fui5V5Map.SkillQ_Bar.self.value = 100;
                fui5V5Map.SkillQ_Bar.Visible = true;
                fui5V5Map.SkillQ_Bar.self.TweenValue(0, 5).OnComplete(() =>
                {
                    fui5V5Map.SkillQ_CDInfo.visible = false;
                    fui5V5Map.SkillQ_Bar.Visible = false;
                });
                m_CDComponent.TriggerCD(this.Entity.Id, "QCD");
            }

            if (this.userInputComponent.WDown)
            {
                SessionComponent.Instance.Session.Send(new UserInput_SkillCmd() { Message = "W" });
                if (fui5V5Map.SkillW_CDInfo.visible) return;
                fui5V5Map.SkillW_CDInfo.text = "7";
                fui5V5Map.SkillW_CDInfo.visible = true;
                fui5V5Map.SkillW_Bar.self.value = 100;
                fui5V5Map.SkillW_Bar.self.visible = true;
                fui5V5Map.SkillW_Bar.self.TweenValue(0, 7).OnComplete(() =>
                {
                    fui5V5Map.SkillW_CDInfo.visible = false;
                    fui5V5Map.SkillW_Bar.self.visible = false;
                });
                m_CDComponent.TriggerCD(this.Entity.Id, "WCD");
            }

            if (this.userInputComponent.EDown)
            {
                SessionComponent.Instance.Session.Send(new UserInput_SkillCmd() { Message = "E" });
                if (fui5V5Map.SkillE_CDInfo.visible) return;
                fui5V5Map.SkillE_CDInfo.text = "10";
                fui5V5Map.SkillE_CDInfo.visible = true;
                fui5V5Map.SkillE_Bar.self.value = 100;
                fui5V5Map.SkillE_Bar.self.visible = true;
                fui5V5Map.SkillE_Bar.self.TweenValue(0, 10).OnComplete(() =>
                {
                    fui5V5Map.SkillE_CDInfo.visible = false;
                    fui5V5Map.SkillE_Bar.self.visible = false;
                });
                m_CDComponent.TriggerCD(this.Entity.Id, "ECD");
            }

            if (this.userInputComponent.RDown)
            {
                SessionComponent.Instance.Session.Send(new UserInput_SkillCmd() { Message = "R" });
            }

            long currentTime = TimeHelper.ClientNow();

            if (fui5V5Map.SkillQ_CDInfo.visible)
            {
                fui5V5Map.SkillQ_CDInfo.text = ((int)Math.Ceiling((double) (this.m_QCDInfo.LastTriggerTimer + this.m_QCDInfo.Interval - currentTime) / 1000))
                        .ToString();
            }

            if (fui5V5Map.SkillW_CDInfo.visible)
            {
                fui5V5Map.SkillW_CDInfo.text = ((int)Math.Ceiling((double) (this.m_WCDInfo.LastTriggerTimer + this.m_WCDInfo.Interval - currentTime) / 1000))
                        .ToString();
            }

            if (fui5V5Map.SkillE_CDInfo.visible)
            {
                fui5V5Map.SkillE_CDInfo.text = ((int)Math.Ceiling((double) (this.m_ECDInfo.LastTriggerTimer + this.m_ECDInfo.Interval - currentTime) / 1000))
                        .ToString();
            }
        }
    }
}