//此文件格式由工具自动生成

using NPBehave;
using Sirenix.OdinInspector;
using Action = System.Action;

namespace ET
{
    [Title("设置CD信息", TitleAlignment = TitleAlignments.Centered)]
    public class NP_SetCDInfoAction : NP_ClassForStoreAction
    {
        [BoxGroup("引用数据的Id")] [LabelText("技能数据结点Id")]
        public VTD_Id DataId = new VTD_Id();

        [BoxGroup("引用数据的Id")] [LabelText("检查技能的Id")]
        public VTD_Id SkillIdBelongTo = new VTD_Id();

        [LabelText("CD名")] public NP_BlackBoardRelationData TheTimeToWait = new NP_BlackBoardRelationData();

        public override Action GetActionToBeDone()
        {
            this.Action = this.SetCDInfoAction;
            return this.Action;
        }

        public void SetCDInfoAction()
        {
            Unit unit = BelongToUnit;
            CDComponent cdComponent = unit.BelongToRoom.GetComponent<CDComponent>();
            SkillDesNodeData skillDesNodeData =
                (SkillDesNodeData) this.BelongtoRuntimeTree.BelongNP_DataSupportor.BuffNodeDataDic[this.DataId.Value];
            long cd = skillDesNodeData.SkillCD[
                unit.GetComponent<SkillCanvasManagerComponent>().GetSkillLevel(this.SkillIdBelongTo.Value)];
            cdComponent.SetCD(unit.Id, TheTimeToWait.GetBlackBoardValue<string>(this.BelongtoRuntimeTree.GetBlackboard()), cd, cd);
        }
    }
}