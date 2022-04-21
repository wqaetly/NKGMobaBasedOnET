//此文件格式由工具自动生成

using System;
using Sirenix.OdinInspector;

#if !SERVER
using UnityEngine;
using Werewolf.StatusIndicators.Components;
#endif


namespace ET
{
    [Title("显示技能指示器", TitleAlignment = TitleAlignments.Centered)]
    public class NP_ShowSkillIndicatorAction : NP_ClassForStoreAction
    {
        [BoxGroup("引用数据的Id")] [LabelText("技能数据结点Id")]
        public VTD_Id DataId = new VTD_Id();

        [LabelText("显示的技能指示器名称")] public NP_BlackBoardRelationData IndicatorName = new NP_BlackBoardRelationData();

        public override Action GetActionToBeDone()
        {
            this.Action = this.ShowSkillIndicatorAction;
            return this.Action;
        }

        public void ShowSkillIndicatorAction()
        {
#if !SERVER
            Unit unit = BelongToUnit;

            if (unit.GetComponent<SkillIndicatorComponent>() == null)
            {
                return;
            }

            SkillDesNodeData skillDesNodeData =
                (SkillDesNodeData) this.BelongtoRuntimeTree.BelongNP_DataSupportor.BuffNodeDataDic[this.DataId.Value];

            switch (@skillDesNodeData.SkillReleaseMode)
            {
                case SkillReleaseMode.Sector:
                    Cone cachedConeGo = unit.GetComponent<SkillIndicatorComponent>()
                        .GetSplate<Cone>(IndicatorName.GetTheBBDataValue<string>());
                    if (cachedConeGo == null)
                    {
                        var gameObject = GameObjectPoolComponent.Instance.FetchGameObject("Cone/Fire Blast",
                            GameObjectType.SkillIndictor);
                        gameObject.transform.SetParent(unit.GetComponent<GameObjectComponent>().GameObject.transform);
                        gameObject.transform.localPosition = new Vector3(0, 1, 0);
                        cachedConeGo = gameObject.GetComponent<Cone>();
                        cachedConeGo.Angle = 40;
                        unit.GetComponent<SkillIndicatorComponent>()
                            .AddSplats(IndicatorName.GetTheBBDataValue<string>(), cachedConeGo);
                    }

                    break;
            }
#endif
        }
    }
}