//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2020年9月18日 21:50:07
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace ET
{
    [Title("给指定Unit添加Buff，支持多个对象", TitleAlignment = TitleAlignments.Centered)]
    public class NP_AddBuffToSpecifiedUnitAction : NP_ClassForStoreAction
    {
        [LabelText("要添加的Buff的信息")] public VTD_BuffInfo BuffDataInfo = new VTD_BuffInfo();

        [LabelText("添加目标Id")]
        public NP_BlackBoardRelationData NPBalckBoardRelationData = new NP_BlackBoardRelationData();

        public override Action GetActionToBeDone()
        {
            this.Action = this.AddBuffToSpecifiedUnit;
            return this.Action;
        }

        public void AddBuffToSpecifiedUnit()
        {
            UnitComponent unitComponent = BelongToUnit.BelongToRoom
                .GetComponent<UnitComponent>();

            foreach (var targetUnitId in NPBalckBoardRelationData.GetBlackBoardValue<List<long>>(
                this.BelongtoRuntimeTree.GetBlackboard()))
            {
                BuffDataInfo.AutoAddBuff(BelongtoRuntimeTree.BelongNP_DataSupportor, BuffDataInfo.BuffNodeId.Value,
                    BelongToUnit, unitComponent.Get(targetUnitId), BelongtoRuntimeTree);
            }
        }
    }
}