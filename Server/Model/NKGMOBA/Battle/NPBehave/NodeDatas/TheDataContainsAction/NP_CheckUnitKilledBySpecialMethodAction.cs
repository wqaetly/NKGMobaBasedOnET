//此文件格式由工具自动生成

using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace ETModel
{
    [Title("检查目标Unit是否被指定方式杀死", TitleAlignment = TitleAlignments.Centered)]
    public class NP_CheckUnitKilledBySpecialMethodAction: NP_ClassForStoreAction
    {
        [LabelText("目标Unit Id")]
        public NP_BlackBoardRelationData TargetUnitId;

        [LabelText("特殊击杀信息")]
        public string SpecialInfo;

        public override Func<bool> GetFunc1ToBeDone()
        {
            this.Func1 = this.CheckUnitKilledBySpecialMethodAction;
            return this.Func1;
        }

        public bool CheckUnitKilledBySpecialMethodAction()
        {
            Unit unit = UnitComponent.Instance.Get(this.TargetUnitId.GetBlackBoardValue<long>(this.BelongtoRuntimeTree.GetBlackboard()));
            //伤害栈顶数据为指定数据且攻击者是自己即返回true，否则返回false
            Stack<OperateData> operateDataStack = unit.GetComponent<OperatesComponent>().GetOperateDatas(OperatesComponent.OperateType.BeDamaged);
            if (operateDataStack != null)
            {
                OperateData stackTopItem = operateDataStack.Peek();
                if (stackTopItem.OperateCaster == UnitComponent.Instance.Get(this.Unitid) &&
                    (stackTopItem as DamageData).CustomData == this.SpecialInfo)
                {
                    return true;
                }
            }

            return false;
        }
    }
}