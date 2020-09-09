//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2020年1月19日 11:06:39
//------------------------------------------------------------

using System;
using Sirenix.OdinInspector;

namespace ETModel
{
    [Title("添加一个Buff", TitleAlignment = TitleAlignments.Centered)]
    public class NP_AddBuffAction: NP_ClassForStoreAction
    {
        [LabelText("要添加的Buff的Id")]
        public VTD_Id BuffDataID;

        public override Action GetActionToBeDone()
        {
            this.Action = this.AddBuff;
            return this.Action;
        }

        public void AddBuff()
        {
            Unit unit = Game.Scene.GetComponent<UnitComponent>().Get(this.Unitid);
            Game.Scene.GetComponent<BuffPoolComponent>().AcquireBuff((this.BelongtoRuntimeTree
                    .BelongNP_DataSupportor
                    .BuffDataDic[this.BuffDataID.Value] as NormalBuffNodeData).BuffData, unit, unit);
        }
    }
}