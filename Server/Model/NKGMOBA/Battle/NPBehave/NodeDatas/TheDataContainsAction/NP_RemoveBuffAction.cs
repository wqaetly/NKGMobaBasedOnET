//此文件格式由工具自动生成

using System;
using Sirenix.OdinInspector;

namespace ETModel
{
    [Title("移除一个Buff", TitleAlignment = TitleAlignments.Centered)]
    public class NP_RemoveBuffAction: NP_ClassForStoreAction
    {
        [LabelText("要移除的Buff的Id")]
        public VTD_BuffInfo BuffDataInfo = new VTD_BuffInfo();

        public override Action GetActionToBeDone()
        {
            this.Action = this.RemoveBuffAction;
            return this.Action;
        }

        public void RemoveBuffAction()
        {
            Unit unit = UnitComponent.Instance.Get(this.BelongtoRuntimeTree.BelongToUnitId);
            unit.GetComponent<BuffManagerComponent>()
                    .RemoveBuff((this.BelongtoRuntimeTree.BelongNP_DataSupportor.BuffNodeDataDic[BuffDataInfo.BuffNodeId.Value] as NormalBuffNodeData)
                        .BuffData.BuffId);
        }
    }
}