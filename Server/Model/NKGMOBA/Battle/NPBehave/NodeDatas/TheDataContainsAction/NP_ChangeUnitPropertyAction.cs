//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年9月25日 15:28:53
//------------------------------------------------------------

using System;
using Sirenix.OdinInspector;

namespace ETModel
{
    /// <summary>
    /// 改变Unit属性
    /// </summary>
    [Title("改变Unit属性", TitleAlignment = TitleAlignments.Centered)]
    public class NP_ChangeUnitPropertyAction: NP_ClassForStoreAction
    {
        public NP_BlackBoardRelationData NPBalckBoardRelationData = new NP_BlackBoardRelationData();

        [LabelText("要更改的Unit属性为")]
        public BuffWorkTypes BuffWorkTypes;

        public override Action GetActionToBeDone()
        {
            this.Action = this.ChangeUnitProperty;
            return this.Action;
        }

        public void ChangeUnitProperty()
        {
            HeroDataComponent heroDataComponent = Game.Scene.GetComponent<UnitComponent>().Get(this.Unitid).GetComponent<HeroDataComponent>();
            switch (BuffWorkTypes)
            {
                case BuffWorkTypes.ChangeMagic:
                    float tobeReMagicValue = this.BelongtoRuntimeTree.GetBlackboard().Get<float>(this.NPBalckBoardRelationData.BBKey);
                    heroDataComponent.CurrentMagicValue -= tobeReMagicValue;
                    try
                    {
                        Game.EventSystem.Run(EventIdType.ChangeMP, this.Unitid, -tobeReMagicValue);
                    }
                    catch (Exception e)
                    {
                        Log.Error(e);
                        throw;
                    }

                    // Log.Info(
                    //     $"减少了蓝：{((float) Game.Scene.GetComponent<UnitComponent>().Get(this.Unitid).GetComponent<NP_RuntimeTreeManager>().GetTreeByRuntimeID(this.RuntimeTreeID).GetBlackboard()[m_NPBalckBoardRelationData.DicKey]).ToString()}");
                    break;
                case BuffWorkTypes.ChangeHP:
                    heroDataComponent.CurrentLifeValue -= this.BelongtoRuntimeTree.GetBlackboard().Get<float>(this.NPBalckBoardRelationData.BBKey);
                    break;
            }
        }
    }
}