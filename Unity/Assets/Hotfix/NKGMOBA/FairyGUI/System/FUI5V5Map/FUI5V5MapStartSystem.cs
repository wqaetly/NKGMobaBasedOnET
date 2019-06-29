//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年5月23日 10:42:59
//------------------------------------------------------------

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
            self.SmallMapSprite.onRightClick.Add(this.AnyEventHandler);
            GameObject HeroAvatars =
                    (GameObject) ETModel.Game.Scene.GetComponent<ResourcesComponent>().GetAsset("heroavatars.unity3d", "HeroAvatars");
            GameObject HeroSkillIcons =
                    (GameObject) ETModel.Game.Scene.GetComponent<ResourcesComponent>().GetAsset("heroskillicons.unity3d", "HeroSkillIcons");

            self.HeroAvatarLoader.texture = new NTexture(HeroAvatars.Get<Sprite>("Darius_Circle_8").texture);
            self.SkillTalent_Loader.texture = new NTexture(HeroSkillIcons.Get<Sprite>("Darius_PassiveBuff").texture);
            self.SkillQ_Loader.texture = new NTexture(HeroSkillIcons.Get<Sprite>("Darius_Icon_Decimate").texture);
            self.SkillW_Loader.texture = new NTexture(HeroSkillIcons.Get<Sprite>("Darius_Icon_Hamstring").texture);
            self.SkillE_Loader.texture = new NTexture(HeroSkillIcons.Get<Sprite>("Darius_Icon_Axe_Grab").texture);
            self.SkillR_Loader.texture = new NTexture(HeroSkillIcons.Get<Sprite>("Darius_Icon_Sudden_Death").texture);

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