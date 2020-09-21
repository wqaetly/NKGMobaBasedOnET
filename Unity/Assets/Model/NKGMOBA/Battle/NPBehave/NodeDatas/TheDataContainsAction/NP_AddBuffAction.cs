//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2020年1月19日 11:06:39
//------------------------------------------------------------

using System;
using ETModel.BBValues;
using Sirenix.OdinInspector;

namespace ETModel
{
    [Title("给自己添加一个Buff", TitleAlignment = TitleAlignments.Centered)]
    public class NP_AddBuffAction: NP_ClassForStoreAction
    {
        [LabelText("要添加的Buff的信息")]
        public VTD_BuffInfo BuffDataInfo = new VTD_BuffInfo();

        public override Action GetActionToBeDone()
        {
            this.Action = this.AddBuff;
            return this.Action;
        }

        public void AddBuff()
        {
            Unit unit = Game.Scene.GetComponent<UnitComponent>().Get(this.Unitid);
            BuffPoolComponent buffPoolComponent = Game.Scene.GetComponent<BuffPoolComponent>();
            for (int i = 0; i < BuffDataInfo.Layers; i++)
            {
                buffPoolComponent.AcquireBuff(BelongtoRuntimeTree.BelongNP_DataSupportor, this.BuffDataInfo.BuffId.Value, unit, unit,
                    BelongtoRuntimeTree);
            }
        }
    }
}