//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年5月23日 10:42:59
//------------------------------------------------------------

using System;
using ETHotfix;
using ETModel;
using FairyGUI;
using UnityEngine;

namespace ETHotfix
{
    [ObjectSystem]
    public class FUI5V5MapStartSystem: StartSystem<FUI5V5Map>
    {
        public override void Start(FUI5V5Map self)
        {
            Unit unit = UnitComponent.Instance.MyUnit;

            HeroDataComponent heroDataComponent = unit.GetComponent<HeroDataComponent>();

            NodeDataForHero mNodeDataForHero = heroDataComponent.NodeDataForHero;

            self.SmallMapSprite.onRightClick.Add(this.AnyEventHandler);

            self.Btn_GMController_Enable.self.visible = false;
            self.Btn_GMController_Disable.self.onClick.Add(() =>
            {
                self.Btn_GMController_Disable.Visible = false;
                self.Btn_GMController_Enable.Visible = true;
                self.Par_GMControllerDis.Play();
            });
            self.Btn_GMController_Enable.self.onClick.Add(() =>
            {
                self.Btn_GMController_Disable.Visible = true;
                self.Btn_GMController_Enable.Visible = false;
                self.Part_GMControllerEnable.Play();
            });

            self.Btn_CreateSpiling.self.onClick.Add(() =>
            {
                SessionComponent.Instance.Session.Send(new Actor_CreateSpiling()
                {
                    X = unit.Position.x, Y = unit.Position.y, Z = unit.Position.z, ParentUnitId = unit.Id
                });
                ETModel.Log.Info($"发送请求木桩父实体id：{unit.Id}");
            });

            GameObject HeroAvatars =
                    ETModel.Game.Scene.GetComponent<ResourcesComponent>().LoadAsset<GameObject>(ABPathUtilities.GetTexturePath("HeroAvatars"));
            GameObject HeroSkillIcons =
                    ETModel.Game.Scene.GetComponent<ResourcesComponent>().LoadAsset<GameObject>(ABPathUtilities.GetTexturePath("HeroSkillIcons"));

            self.HeroAvatarLoader.texture = new NTexture(HeroAvatars.GetTargetObjectFromRC<Sprite>(mNodeDataForHero.HeroAvatar).texture);
            self.SkillTalent_Loader.texture = new NTexture(HeroSkillIcons.GetTargetObjectFromRC<Sprite>(mNodeDataForHero.Talent_SkillSprite).texture);
            self.SkillQ_Loader.texture = new NTexture(HeroSkillIcons.GetTargetObjectFromRC<Sprite>(mNodeDataForHero.Q_SkillSprite).texture);
            self.SkillW_Loader.texture = new NTexture(HeroSkillIcons.GetTargetObjectFromRC<Sprite>(mNodeDataForHero.W_SkillSprite).texture);
            self.SkillE_Loader.texture = new NTexture(HeroSkillIcons.GetTargetObjectFromRC<Sprite>(mNodeDataForHero.E_SkillSprite).texture);
            self.SkillR_Loader.texture = new NTexture(HeroSkillIcons.GetTargetObjectFromRC<Sprite>(mNodeDataForHero.R_SkillSprite).texture);

            self.AttackInfo.text = (mNodeDataForHero.OriAttackValue + mNodeDataForHero.ExtAttackValue).ToString();
            self.ExtraAttackInfo.text = mNodeDataForHero.ExtAttackValue.ToString();
            self.MagicInfo.text = (mNodeDataForHero.OriMagicStrength + mNodeDataForHero.ExtMagicStrength).ToString();
            self.ExtraMagicInfo.text = mNodeDataForHero.ExtMagicRec.ToString();
            self.ArmorInfo.text = (mNodeDataForHero.OriArmor + mNodeDataForHero.ExtArmor).ToString();
            self.ArmorpenetrationInfo.text = (mNodeDataForHero.OriArmorPenetration + mNodeDataForHero.ExtArmorPenetration).ToString();
            self.SpellResistanceInfo.text = (mNodeDataForHero.OriMagicResistance + mNodeDataForHero.ExtMagicResistance).ToString();
            self.MagicpenetrationInfo.text = (mNodeDataForHero.OriMagicPenetration + mNodeDataForHero.ExtMagicPenetration).ToString();
            self.AttackSpeedInfo.text = (mNodeDataForHero.OriAttackSpeed + mNodeDataForHero.ExtAttackSpeed).ToString();
            self.SkillCDInfo.text = (mNodeDataForHero.OriSkillCD + mNodeDataForHero.ExtSkillCD).ToString();
            self.CriticalstrikeInfo.text =
                    (mNodeDataForHero.OriCriticalStrikeProbability + mNodeDataForHero.ExtCriticalStrikeProbability).ToString();
            self.MoveSpeedInfo.text = (mNodeDataForHero.OriMoveSpeed + mNodeDataForHero.ExtMoveSpeed).ToString();

            self.RedText.text = $"{heroDataComponent.CurrentLifeValue}/{heroDataComponent.MaxLifeValue}";
            self.BlueText.text = $"{heroDataComponent.CurrentMagicValue}/{heroDataComponent.MaxMagicValue}";

            self.RedProBar.self.max = heroDataComponent.MaxLifeValue;
            self.RedProBar.self.value = heroDataComponent.CurrentLifeValue;

            self.BlueProBar.self.max = heroDataComponent.MaxMagicValue;
            self.BlueProBar.self.value = heroDataComponent.CurrentMagicValue;

            self.SkillTalent_CDInfo.visible = false;
            self.SkillTalent_Bar.Visible = false;

            self.SkillQ_CDInfo.visible = false;
            self.SkillQ_Bar.Visible = false;

            self.SkillW_CDInfo.visible = false;
            self.SkillW_Bar.Visible = false;

            self.SkillE_CDInfo.visible = false;
            self.SkillE_Bar.Visible = false;

            self.SkillR_CDInfo.visible = false;
            self.SkillR_Bar.Visible = false;

            self.SkillD_CDInfo.visible = false;
            self.SkillD_Bar.Visible = false;

            self.SkillF_CDInfo.visible = false;
            self.SkillF_Bar.Visible = false;
        }

        void AnyEventHandler(EventContext context)
        {
            Vector2 global2Local = ((GObject) context.sender).GlobalToLocal(context.inputEvent.position);
            Vector2 fgui2Unity = new Vector2(global2Local.x, 200 - global2Local.y);
            Vector3 targetPos = new Vector3(-fgui2Unity.x / (200.0f / 100.0f), 0, -fgui2Unity.y / (200.0f / 100.0f));
            Game.EventSystem.Run(EventIdType.ClickSmallMap, targetPos);
        }
    }
}