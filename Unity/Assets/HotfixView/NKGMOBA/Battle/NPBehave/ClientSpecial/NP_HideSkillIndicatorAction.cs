//此文件格式由工具自动生成

using System;
using Sirenix.OdinInspector;

#if !SERVER
using Werewolf.StatusIndicators.Components;
#endif


namespace ET
{
    [Title("隐藏技能指示器", TitleAlignment = TitleAlignments.Centered)]
    public class NP_HideSkillIndicatorAction : NP_ClassForStoreAction
    {
        [BoxGroup("引用数据的Id")] [LabelText("技能数据结点Id")]
        public VTD_Id DataId = new VTD_Id();

        [LabelText("隐藏的技能指示器名称")] public NP_BlackBoardRelationData IndicatorName = new NP_BlackBoardRelationData();

        public override Action GetActionToBeDone()
        {
            this.Action = this.HideSkillIndicatorAction;
            return this.Action;
        }

        public void HideSkillIndicatorAction()
        {
#if !SERVER
            Unit unit = BelongToUnit;
            if (unit.GetComponent<SkillIndicatorComponent>() == null)
            {
                return;
            }
            SkillDesNodeData skillDesNodeData =
                (SkillDesNodeData) this.BelongtoRuntimeTree.BelongNP_DataSupportor.BuffNodeDataDic[this.DataId.Value];
            var skillIndicatorName = IndicatorName.GetTheBBDataValue<string>();

            switch (@skillDesNodeData.SkillReleaseMode)
            {
                case SkillReleaseMode.Sector:
                    GameObjectPoolComponent.Instance.RecycleGameObject("Cone/Fire Blast",
                        unit.GetComponent<SkillIndicatorComponent>().GetSplate<Cone>(skillIndicatorName).gameObject);
                    unit.GetComponent<SkillIndicatorComponent>().RemoveSplate(skillIndicatorName);
                    break;
            }
#endif
        }
    }
}