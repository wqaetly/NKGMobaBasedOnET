//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年5月23日 10:42:59
//------------------------------------------------------------

using System;
using ETHotfix.FUI5v5Map;
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
            HeroDataComponent heroDataComponent = ETModel.Game.Scene.GetComponent<UnitComponent>().MyUnit.GetComponent<HeroDataComponent>();

            NodeDataForHero mNodeDataForHero = heroDataComponent.NodeDataForHero;

            self.SmallMapSprite.onRightClick.Add(this.AnyEventHandler);

            GameObject HeroAvatars =
                    (GameObject) ETModel.Game.Scene.GetComponent<ResourcesComponent>().GetAsset("heroavatars.unity3d", "HeroAvatars");
            GameObject HeroSkillIcons =
                    (GameObject) ETModel.Game.Scene.GetComponent<ResourcesComponent>().GetAsset("heroskillicons.unity3d", "HeroSkillIcons");

            self.HeroAvatarLoader.texture = new NTexture(HeroAvatars.Get<Sprite>(mNodeDataForHero.HeroAvatar).texture);
            self.SkillTalent_Loader.texture = new NTexture(HeroSkillIcons.Get<Sprite>(mNodeDataForHero.Talent_SkillSprite).texture);
            self.SkillQ_Loader.texture = new NTexture(HeroSkillIcons.Get<Sprite>(mNodeDataForHero.Q_SkillSprite).texture);
            self.SkillW_Loader.texture = new NTexture(HeroSkillIcons.Get<Sprite>(mNodeDataForHero.W_SkillSprite).texture);
            self.SkillE_Loader.texture = new NTexture(HeroSkillIcons.Get<Sprite>(mNodeDataForHero.E_SkillSprite).texture);
            self.SkillR_Loader.texture = new NTexture(HeroSkillIcons.Get<Sprite>(mNodeDataForHero.R_SkillSprite).texture);

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

            self.RedText.text = String.Concat(heroDataComponent.CurrentLifeValue.ToString(), "/", heroDataComponent.MaxLifeValue.ToString());
            self.BlueText.text = String.Concat(heroDataComponent.CurrentMagicValue.ToString(), "/", heroDataComponent.MaxMagicValue.ToString());
            
            self.RedProBar.self.value = heroDataComponent.CurrentLifeValue;
            self.RedProBar.self.max = heroDataComponent.MaxLifeValue;

            self.BlueProBar.self.value = heroDataComponent.CurrentMagicValue;
            self.BlueProBar.self.max = heroDataComponent.MaxMagicValue;

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