//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2020年1月24日 16:42:12
//------------------------------------------------------------

using System.Collections.Generic;
using ETModel;

namespace ETHotfix
{
    [MessageHandler]
    public class M2C_SyncNPBehaveBoolDataHandler: AMHandler<M2C_SyncNPBehaveBoolData>
    {
        protected override ETTask Run(ETModel.Session session, M2C_SyncNPBehaveBoolData message)
        {
            Unit unit = UnitComponent.Instance.Get(message.UnitId);
            foreach (var skillCanvaList  in unit.GetComponent<SkillCanvasManagerComponent>().GetAllSkillCanvas())
            {
                foreach (var skillNpRuntimeTree in skillCanvaList.Value)
                {
                    skillNpRuntimeTree.GetBlackboard().Set(message.BBKey,message.Value);
                }
            }
            return ETTask.CompletedTask;
        }
    }
}