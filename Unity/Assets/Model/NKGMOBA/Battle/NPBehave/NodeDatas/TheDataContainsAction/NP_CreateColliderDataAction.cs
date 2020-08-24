//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年9月27日 22:26:39
//------------------------------------------------------------

using System;
using ETModel;
using ETModel.TheDataContainsAction;
using Sirenix.OdinInspector;

namespace Model.NKGMOBA.NPBehave.NodeDatas.TheDataContainsAction
{
    [Title("创建一个碰撞体",TitleAlignment = TitleAlignments.Centered)]
    public class NP_CreateColliderData: NP_ClassForStoreAction
    {
        [LabelText("要生成的碰撞数据载体ID")]
        public long supportDataID;

        [LabelText("要生成的碰撞数据ID")]
        public long colliderDataID;

        public override Action GetActionToBeDone()
        {
            this.m_Action = this.CreateColliderData;
            return this.m_Action;
        }

        public void CreateColliderData()
        {
            Game.EventSystem.Run(EventIdType.CreateCollider, this.Unitid, this.supportDataID, this.colliderDataID);
        }
    }
}