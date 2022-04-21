//此文件格式由工具自动生成

using System;
using Sirenix.OdinInspector;

namespace ET
{
    [Title("往客户端发送CD信息", TitleAlignment = TitleAlignments.Centered)]
    public class NP_SendCDInfoToClient : NP_ClassForStoreAction
    {
        [LabelText("CD名")] public NP_BlackBoardRelationData CDName = new NP_BlackBoardRelationData();

        public override Action GetActionToBeDone()
        {
            this.Action = this.SendCDInfoToClient;
            return this.Action;
        }

        public void SendCDInfoToClient()
        {
#if SERVER
            CDInfo cdInfo = this.BelongToUnit.BelongToRoom.GetComponent<CDComponent>()
                .GetCDData(this.BelongToUnit.Id,
                    this.CDName.GetBlackBoardValue<string>(this.BelongtoRuntimeTree.GetBlackboard()));
            MessageHelper.BroadcastToRoom(this.BelongToUnit.BelongToRoom, new M2C_SyncCDData()
            {
                UnitId = this.BelongToUnit.Id, CDName = cdInfo.Name, CDLength = cdInfo.Interval,
                RemainCDLength = cdInfo.RemainCDLength
            });
#endif
        }
    }
}