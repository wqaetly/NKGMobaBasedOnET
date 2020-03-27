//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2020年1月19日 11:06:39
//------------------------------------------------------------

using System;
using Sirenix.OdinInspector;

namespace ETModel.TheDataContainsAction
{
    public class NP_AddBuffAction: NP_ClassForStoreAction
    {
        [LabelText("要执行的Buff数据ID")]
        public long BuffDataID;

        public override Action GetActionToBeDone()
        {
            this.m_Action = this.AddBuff;
            return this.m_Action;
        }

        public void AddBuff()
        {
            //Log.Info("行为树添加Buff");
            Unit unit = Game.Scene.GetComponent<UnitComponent>().Get(this.Unitid);
            Game.Scene.GetComponent<BuffPoolComponent>().AcquireBuff((unit.GetComponent<NP_RuntimeTreeManager>()
                    .GetTreeByRuntimeID(this.RuntimeTreeID)
                    .m_BelongNP_DataSupportor
                    .mSkillDataDic[this.BuffDataID] as NodeDataForSkillBuff).SkillBuffBases, unit, unit);
            //Log.Info("Buff添加完成");
        }
    }
}