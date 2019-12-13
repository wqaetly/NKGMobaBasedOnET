//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年9月25日 15:28:53
//------------------------------------------------------------

using System;
using ETModel;
using ETModel.TheDataContainsAction;
using Sirenix.OdinInspector;

namespace Model.NKGMOBA.NPBehave.NodeDatas.TheDataContainsAction
{
    /// <summary>
    /// 改变Unit属性
    /// </summary>
    public class NP_ChangeUnitPropertyAction: NP_ClassForStoreAction
    {
        [LabelText("黑板节点相关的数据")]
        public NP_BlackBoardRelationData m_NPBalckBoardRelationData;

        [LabelText("要更改的Unit属性为")]
        public BuffWorkTypes BuffWorkTypes;

        public override Action GetActionToBeDone()
        {
            this.m_Action = this.ChangeUnitProperty;
            return this.m_Action;
        }

        public void ChangeUnitProperty()
        {
            HeroDataComponent heroDataComponent = Game.Scene.GetComponent<UnitComponent>().Get(this.Unitid).GetComponent<HeroDataComponent>();
            switch (BuffWorkTypes)
            {
                case BuffWorkTypes.ChangeMagic:
                    heroDataComponent.CurrentMagicValue -= (float) Game.Scene.GetComponent<UnitComponent>().Get(this.Unitid)
                            .GetComponent<NP_RuntimeTreeManager>()
                            .GetTreeByRuntimeID(this.RuntimeTreeID)
                            .GetBlackboard()[m_NPBalckBoardRelationData.DicKey];
                    Log.Info(
                        $"减少了蓝量：{((float) Game.Scene.GetComponent<UnitComponent>().Get(this.Unitid).GetComponent<NP_RuntimeTreeManager>().GetTreeByRuntimeID(this.RuntimeTreeID).GetBlackboard()[m_NPBalckBoardRelationData.DicKey]).ToString()}");
                    break;
                case BuffWorkTypes.ChangeHP:
                    heroDataComponent.CurrentLifeValue -= (float) Game.Scene.GetComponent<UnitComponent>().Get(this.Unitid)
                            .GetComponent<NP_RuntimeTreeManager>()
                            .GetTreeByRuntimeID(this.RuntimeTreeID)
                            .GetBlackboard()[m_NPBalckBoardRelationData.DicKey];
                    break;
            }
        }
    }
}