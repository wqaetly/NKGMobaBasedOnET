using UnityEngine;
using Werewolf.StatusIndicators.Components;

#if !SERVER
namespace ET
{
    public class PlayerHeroControllerUpdateSystem : UpdateSystem<PlayerHeroControllerComponent>
    {
        public override void Update(PlayerHeroControllerComponent self)
        {
            if (self.UserInputComponent.QDown)
            {
                Unit unit = self.GetParent<Unit>();
                LSF_PlaySkillInputCmd lsfPlaySkillInputCmd = ReferencePool.Acquire<LSF_PlaySkillInputCmd>();
                lsfPlaySkillInputCmd.Init(unit.Id);
                lsfPlaySkillInputCmd.InputTag = "PlayerInput";
                lsfPlaySkillInputCmd.InputKey = "Q";
                unit.BelongToRoom.GetComponent<LSF_Component>().AddCmdToSendQueue(lsfPlaySkillInputCmd);
            }

            if (self.UserInputComponent.WDown)
            {
                Unit unit = self.GetParent<Unit>();
                LSF_PlaySkillInputCmd lsfPlaySkillInputCmd = ReferencePool.Acquire<LSF_PlaySkillInputCmd>();
                lsfPlaySkillInputCmd.Init(unit.Id);
                lsfPlaySkillInputCmd.InputTag = "PlayerInput";
                lsfPlaySkillInputCmd.InputKey = "W";
                unit.BelongToRoom.GetComponent<LSF_Component>().AddCmdToSendQueue(lsfPlaySkillInputCmd);
            }

            if (self.UserInputComponent.EDown_long)
            {
                Unit unit = self.GetParent<Unit>();

                //TODO 单独制作一个技能指示器系统
                Cone cachedConeGo = unit.GetComponent<SkillIndicatorComponent>()
                    .GetSplate<Cone>($"{unit.Id}_ESkillIndicator");
                if (cachedConeGo == null)
                {
                    var gameObject = GameObjectPoolComponent.Instance.FetchGameObject("Cone/Fire Blast",
                        GameObjectType.SkillIndictor);
                    gameObject.transform.SetParent(unit.GetComponent<GameObjectComponent>().GameObject.transform);
                    gameObject.transform.localPosition = new Vector3(0, 1, 0);
                    cachedConeGo = gameObject.GetComponent<Cone>();
                    cachedConeGo.Angle = 40;
                    unit.GetComponent<SkillIndicatorComponent>().AddSplats($"{unit.Id}_ESkillIndicator", cachedConeGo);
                }

                if (self.UserInputComponent.LeftMouseDown)
                {
                    LSF_PlaySkillInputCmd lsfPlaySkillInputCmd2 = ReferencePool.Acquire<LSF_PlaySkillInputCmd>();
                    lsfPlaySkillInputCmd2.Init(unit.Id);
                    lsfPlaySkillInputCmd2.InputTag = "PlayerInput";
                    lsfPlaySkillInputCmd2.InputKey = "E";
                    lsfPlaySkillInputCmd2.Angle = cachedConeGo.transform.eulerAngles.y;
                    unit.BelongToRoom.GetComponent<LSF_Component>().AddCmdToSendQueue(lsfPlaySkillInputCmd2);

                    GameObjectPoolComponent.Instance.RecycleGameObject("Cone/Fire Blast",
                        unit.GetComponent<SkillIndicatorComponent>().GetSplate<Cone>($"{unit.Id}_ESkillIndicator")
                            .gameObject);
                    unit.GetComponent<SkillIndicatorComponent>().RemoveSplate($"{unit.Id}_ESkillIndicator");
                }
            }

            if (self.UserInputComponent.EUp)
            {
                Unit unit = self.GetParent<Unit>();
                GameObjectPoolComponent.Instance.RecycleGameObject("Cone/Fire Blast",
                    unit.GetComponent<SkillIndicatorComponent>().GetSplate<Cone>($"{unit.Id}_ESkillIndicator")
                        .gameObject);
                unit.GetComponent<SkillIndicatorComponent>().RemoveSplate($"{unit.Id}_ESkillIndicator");
            }

            if (self.UserInputComponent.RDown)
            {
                Unit unit = self.GetParent<Unit>();
                LSF_PlaySkillInputCmd lsfPlaySkillInputCmd = ReferencePool.Acquire<LSF_PlaySkillInputCmd>();
                lsfPlaySkillInputCmd.Init(unit.Id);
                lsfPlaySkillInputCmd.InputKey = "R";
                unit.BelongToRoom.GetComponent<LSF_Component>().AddCmdToSendQueue(lsfPlaySkillInputCmd);
            }
        }
    }

    public class PlayerHeroControllerAwakeSystem : AwakeSystem<PlayerHeroControllerComponent>
    {
        public override void Awake(PlayerHeroControllerComponent self)
        {
            self.UserInputComponent = Game.Scene.GetComponent<UserInputComponent>();
        }
    }
}
#endif