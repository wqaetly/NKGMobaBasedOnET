//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2020年10月1日 14:11:03
//------------------------------------------------------------

using System;

namespace ETModel
{
    /// <summary>
    /// 设置碰撞体BelongToUnit的Id
    /// 所谓所归属的Unit，也就是产出碰撞体的Unit，
    /// 比如诺克放一个Q，那么BelongUnit就是诺克
    /// </summary>
    public class NP_SetUnitIdTheColliderBelongToAction: NP_ClassForStoreAction
    {
        public NP_BlackBoardRelationData NpBlackBoardRelationData = new NP_BlackBoardRelationData();

        public override Action GetActionToBeDone()
        {
            this.Action = this.SetUnitIdTheColliderBelongTo;
            return this.Action;
        }

        public void SetUnitIdTheColliderBelongTo()
        {
#if SERVER
            //这里默许碰撞体自身带有B2S_ColliderComponent
            Unit selfUnit = Game.Scene.GetComponent<UnitComponent>().Get(this.Unitid);
            this.BelongtoRuntimeTree.GetBlackboard().Set(NpBlackBoardRelationData.BBKey, selfUnit.GetComponent<B2S_ColliderComponent>().BelongToUnit.Id);
#endif
        }
    }
}