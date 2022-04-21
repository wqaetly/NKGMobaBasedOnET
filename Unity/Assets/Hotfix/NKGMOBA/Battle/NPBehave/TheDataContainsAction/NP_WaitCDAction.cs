//此文件格式由工具自动生成

using System;
using Sirenix.OdinInspector;

namespace ET
{
    [Title("等待CD", TitleAlignment = TitleAlignments.Centered)]
    public class NP_WaitCDAction: NP_ClassForStoreAction
    {
        [LabelText("CD名")]
        public NP_BlackBoardRelationData CDName = new NP_BlackBoardRelationData();

        public override Func<bool, NPBehave.Action.Result> GetFunc2ToBeDone()
        {
            this.Func2 = WaitCDAction;
            return this.Func2;
        }

        public NPBehave.Action.Result WaitCDAction(bool hasDown)
        {
            Unit unit = BelongToUnit;
            CDComponent cdComponent = unit.BelongToRoom.GetComponent<CDComponent>();
            
            if (!cdComponent.GetCDResult(BelongToUnit.Id, CDName.GetBlackBoardValue<string>(this.BelongtoRuntimeTree.GetBlackboard())))
            {
                return NPBehave.Action.Result.PROGRESS;
            }
            else
            {
                return NPBehave.Action.Result.SUCCESS;
            }
        }
    }
}