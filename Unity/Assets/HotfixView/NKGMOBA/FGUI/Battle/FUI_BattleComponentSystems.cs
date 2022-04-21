//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2021年8月29日 9:39:13
//------------------------------------------------------------

using System;
using FairyGUI;
using UnityEngine;

namespace ET
{
    public class FUI_BattleComponentAwakeSystem : AwakeSystem<FUI_BattleComponent, FUI_Battle_Main>
    {
        public override void Awake(FUI_BattleComponent self, FUI_Battle_Main fuiUIPanelBattle)
        {
            self.FuiUIPanelBattle = fuiUIPanelBattle;
            Scene scene = self.DomainScene();

            UnitComponent unitComponent = scene.GetComponent<RoomManagerComponent>().GetBattleRoom()
                .GetComponent<UnitComponent>();

            long playerUnitId = unitComponent.MyUnit.Id;
            self.m_QCDInfo = CDComponent.Instance.AddCDData(playerUnitId, "Q", 0, info =>
            {
                if (info.Result)
                {
                    self.FuiUIPanelBattle.m_SkillQ_CDInfo.visible = false;
                    self.FuiUIPanelBattle.m_SkillQ_Bar.Visible = false;
                    return;
                }

                self.FuiUIPanelBattle.m_SkillQ_CDInfo.text =
                    ((int) Math.Ceiling((double) (info.RemainCDLength) / 1000))
                    .ToString();
                self.FuiUIPanelBattle.m_SkillQ_CDInfo.visible = true;
                self.FuiUIPanelBattle.m_SkillQ_Bar.self.value = 100 * (info.RemainCDLength / info.Interval);
                self.FuiUIPanelBattle.m_SkillQ_Bar.Visible = true;
            });
            self.m_WCDInfo = CDComponent.Instance.AddCDData(playerUnitId, "W", 0, info =>
            {
                if (info.Result)
                {
                    self.FuiUIPanelBattle.m_SkillW_CDInfo.visible = false;
                    self.FuiUIPanelBattle.m_SkillW_Bar.Visible = false;
                    return;
                }

                self.FuiUIPanelBattle.m_SkillW_CDInfo.text =
                    ((int) Math.Ceiling((double) (info.RemainCDLength) / 1000))
                    .ToString();
                self.FuiUIPanelBattle.m_SkillW_CDInfo.visible = true;
                self.FuiUIPanelBattle.m_SkillW_Bar.self.value = 100 * (info.RemainCDLength / info.Interval);
                self.FuiUIPanelBattle.m_SkillW_Bar.Visible = true;
            });
            self.m_ECDInfo = CDComponent.Instance.AddCDData(playerUnitId, "E", 0, info =>
            {
                if (info.Result)
                {
                    self.FuiUIPanelBattle.m_SkillE_CDInfo.visible = false;
                    self.FuiUIPanelBattle.m_SkillE_Bar.Visible = false;
                    return;
                }

                self.FuiUIPanelBattle.m_SkillE_CDInfo.text =
                    ((int) Math.Ceiling((double) (info.RemainCDLength) / 1000))
                    .ToString();
                self.FuiUIPanelBattle.m_SkillE_CDInfo.visible = true;
                self.FuiUIPanelBattle.m_SkillE_Bar.self.value = 100 * (info.RemainCDLength / info.Interval);
                self.FuiUIPanelBattle.m_SkillE_Bar.Visible = true;
            });

            Unit unit = unitComponent.MyUnit;
            UnitAttributesDataComponent unitAttributesDataComponent = unit.GetComponent<UnitAttributesDataComponent>();

            HeroAttributesNodeData heroAttributesNodeData =
                unitAttributesDataComponent.GetAttributeDataAs<HeroAttributesNodeData>();

            self.FuiUIPanelBattle.m_SmallMapSprite.onRightClick.Add(this.AnyEventHandler);

            self.FuiUIPanelBattle.m_Btn_GMController_Enable.self.visible = false;
            self.FuiUIPanelBattle.m_Btn_GMController_Disable.self.onClick.Add(() =>
            {
                self.FuiUIPanelBattle.m_Btn_GMController_Disable.Visible = false;
                self.FuiUIPanelBattle.m_Btn_GMController_Enable.Visible = true;
                self.FuiUIPanelBattle.m_Par_GMControllerDis.Play();
            });
            self.FuiUIPanelBattle.m_Btn_GMController_Enable.self.onClick.Add(() =>
            {
                self.FuiUIPanelBattle.m_Btn_GMController_Disable.Visible = true;
                self.FuiUIPanelBattle.m_Btn_GMController_Enable.Visible = false;
                self.FuiUIPanelBattle.m_Part_GMControllerEnable.Play();
            });

            self.FuiUIPanelBattle.m_Btn_CreateSpiling.self.onClick.Add(() =>
            {
                LSF_CreateSpilingCmd lsfCreateSpilingCmd =
                    ReferencePool.Acquire<LSF_CreateSpilingCmd>().Init(unit.Id) as LSF_CreateSpilingCmd;

                lsfCreateSpilingCmd.UnitInfo = new UnitInfo()
                {
                    X = unit.Position.x, Y = unit.Position.y, Z = unit.Position.z, RoleCamp =
                        (int) RoleCamp.TianZai,
                    ConfigId = 10001, BelongToPlayerId = Game.Scene.GetComponent<PlayerComponent>().PlayerId,
                    UnitId = IdGenerater.Instance.GenerateUnitId(scene.Zone), RoomId = unit.BelongToRoom.Id
                };

                unit.BelongToRoom.GetComponent<LSF_Component>().AddCmdToSendQueue(lsfCreateSpilingCmd, false);
            });

            self.FuiUIPanelBattle.m_HeroAvatarLoader.url =
                XAssetPathUtilities.GetUnitAvatatIcon("Darius", heroAttributesNodeData.UnitAvatar);
            self.FuiUIPanelBattle.m_SkillTalent_Loader.url =
                XAssetPathUtilities.GetSkillIcon("Darius", heroAttributesNodeData.Talent_SkillSprite);
            self.FuiUIPanelBattle.m_SkillQ_Loader.url =
                XAssetPathUtilities.GetSkillIcon("Darius", heroAttributesNodeData.Q_SkillSprite);
            self.FuiUIPanelBattle.m_SkillW_Loader.url =
                XAssetPathUtilities.GetSkillIcon("Darius", heroAttributesNodeData.W_SkillSprite);
            self.FuiUIPanelBattle.m_SkillE_Loader.url =
                XAssetPathUtilities.GetSkillIcon("Darius", heroAttributesNodeData.E_SkillSprite);
            self.FuiUIPanelBattle.m_SkillR_Loader.url =
                XAssetPathUtilities.GetSkillIcon("Darius", heroAttributesNodeData.R_SkillSprite);

            self.FuiUIPanelBattle.m_AttackInfo.text =
                unitAttributesDataComponent.GetAttribute(NumericType.Attack).ToString();
            self.FuiUIPanelBattle.m_ExtraAttackInfo.text =
                unitAttributesDataComponent.GetAttribute(NumericType.AttackAdd).ToString();
            self.FuiUIPanelBattle.m_MagicInfo.text =
                unitAttributesDataComponent.GetAttribute(NumericType.MagicStrength).ToString();
            self.FuiUIPanelBattle.m_ExtraMagicInfo.text =
                unitAttributesDataComponent.GetAttribute(NumericType.MagicStrengthAdd).ToString();
            self.FuiUIPanelBattle.m_ArmorInfo.text =
                unitAttributesDataComponent.GetAttribute(NumericType.Armor).ToString();
            self.FuiUIPanelBattle.m_ArmorpenetrationInfo.text =
                unitAttributesDataComponent.GetAttribute(NumericType.ArmorPenetration).ToString();
            self.FuiUIPanelBattle.m_SpellResistanceInfo.text =
                unitAttributesDataComponent.GetAttribute(NumericType.MagicResistance).ToString();
            self.FuiUIPanelBattle.m_MagicpenetrationInfo.text =
                unitAttributesDataComponent.GetAttribute(NumericType.MagicPenetration).ToString();
            self.FuiUIPanelBattle.m_AttackSpeedInfo.text =
                unitAttributesDataComponent.GetAttribute(NumericType.AttackSpeed).ToString();
            self.FuiUIPanelBattle.m_SkillCDInfo.text =
                unitAttributesDataComponent.GetAttribute(NumericType.SkillCD).ToString();
            self.FuiUIPanelBattle.m_CriticalstrikeInfo.text = unitAttributesDataComponent
                .GetAttribute(NumericType.CriticalStrikeProbability).ToString();
            self.FuiUIPanelBattle.m_MoveSpeedInfo.text =
                unitAttributesDataComponent.GetAttribute(NumericType.Speed).ToString();

            self.FuiUIPanelBattle.m_RedText.text =
                $"{unitAttributesDataComponent.GetAttribute(NumericType.Hp)}/{unitAttributesDataComponent.GetAttribute(NumericType.MaxHp)}";
            self.FuiUIPanelBattle.m_BlueText.text =
                $"{unitAttributesDataComponent.GetAttribute(NumericType.Mp)}/{unitAttributesDataComponent.GetAttribute(NumericType.MaxMp)}";

            self.FuiUIPanelBattle.m_RedProBar.self.max = unitAttributesDataComponent.GetAttribute(NumericType.MaxHp);
            self.FuiUIPanelBattle.m_RedProBar.self.value = unitAttributesDataComponent.GetAttribute(NumericType.Hp);

            self.FuiUIPanelBattle.m_BlueProBar.self.max = unitAttributesDataComponent.GetAttribute(NumericType.MaxMp);
            self.FuiUIPanelBattle.m_BlueProBar.self.value = unitAttributesDataComponent.GetAttribute(NumericType.Mp);

            self.FuiUIPanelBattle.m_SkillTalent_CDInfo.visible = false;
            self.FuiUIPanelBattle.m_SkillTalent_Bar.Visible = false;

            self.FuiUIPanelBattle.m_SkillQ_CDInfo.visible = false;
            self.FuiUIPanelBattle.m_SkillQ_Bar.Visible = false;

            self.FuiUIPanelBattle.m_SkillW_CDInfo.visible = false;
            self.FuiUIPanelBattle.m_SkillW_Bar.Visible = false;

            self.FuiUIPanelBattle.m_SkillE_CDInfo.visible = false;
            self.FuiUIPanelBattle.m_SkillE_Bar.Visible = false;

            self.FuiUIPanelBattle.m_SkillR_CDInfo.visible = false;
            self.FuiUIPanelBattle.m_SkillR_Bar.Visible = false;

            self.FuiUIPanelBattle.m_SkillD_CDInfo.visible = false;
            self.FuiUIPanelBattle.m_SkillD_Bar.Visible = false;

            self.FuiUIPanelBattle.m_SkillF_CDInfo.visible = false;
            self.FuiUIPanelBattle.m_SkillF_Bar.Visible = false;
        }

        void AnyEventHandler(EventContext context)
        {
            Vector2 global2Local = ((GObject) context.sender).GlobalToLocal(context.inputEvent.position);
            Vector2 fgui2Unity = new Vector2(global2Local.x, 200 - global2Local.y);
            Vector3 targetPos = new Vector3(-fgui2Unity.x / (200.0f / 100.0f), 0, -fgui2Unity.y / (200.0f / 100.0f));

            Unit unit = Game.Scene.GetComponent<PlayerComponent>().BelongToRoom.GetComponent<UnitComponent>().MyUnit;
            unit.SendPathFindCmd(targetPos);
        }
    }

    public class FUI_BattleComponentUpdateSystem : UpdateSystem<FUI_BattleComponent>
    {
        public override void Update(FUI_BattleComponent self)
        {
            Unit unit = self.DomainScene().GetComponent<RoomManagerComponent>().GetBattleRoom()
                .GetComponent<UnitComponent>().MyUnit;
            long playerUnitId = unit.Id;
            //此处填写Update逻辑
            if (!CDComponent.Instance.GetCDResult(playerUnitId, "Q"))
            {
                self.FuiUIPanelBattle.m_SkillQ_CDInfo.text =
                    ((int) Math.Ceiling((double) (self.m_QCDInfo.RemainCDLength) / 1000))
                    .ToString();
                self.FuiUIPanelBattle.m_SkillQ_Bar.self.value =
                    100 * (self.m_QCDInfo.RemainCDLength * 1f / self.m_QCDInfo.Interval);
            }

            if (!CDComponent.Instance.GetCDResult(playerUnitId, "W"))
            {
                self.FuiUIPanelBattle.m_SkillW_CDInfo.text =
                    ((int) Math.Ceiling((double) (self.m_WCDInfo.RemainCDLength) / 1000))
                    .ToString();
                self.FuiUIPanelBattle.m_SkillW_Bar.self.value =
                    100 * (self.m_WCDInfo.RemainCDLength * 1f / self.m_WCDInfo.Interval);
            }

            if (!CDComponent.Instance.GetCDResult(playerUnitId, "E"))
            {
                self.FuiUIPanelBattle.m_SkillE_CDInfo.text =
                    ((int) Math.Ceiling((double) (self.m_ECDInfo.RemainCDLength) / 1000))
                    .ToString();
                self.FuiUIPanelBattle.m_SkillE_Bar.self.value =
                    100 * (self.m_ECDInfo.RemainCDLength * 1f / self.m_ECDInfo.Interval);
            }
        }
    }

    public class FUI_BattleComponentDestroySystem : DestroySystem<FUI_BattleComponent>
    {
        public override void Destroy(FUI_BattleComponent self)
        {
            self.FuiUIPanelBattle = null;
        }
    }
}