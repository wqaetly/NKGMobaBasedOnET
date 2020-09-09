//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年9月7日 10:29:38
//------------------------------------------------------------

using System;
using Sirenix.OdinInspector;

namespace ETModel
{
    /// <summary>
    /// 检查技能是否能释放
    /// </summary>
    [Title("检查技能是否能释放", TitleAlignment = TitleAlignments.Centered)]
    public class NP_CheckSkillCanBeCastAction: NP_ClassForStoreAction
    {
        [LabelText("要引用的的数据结点ID")]
        public VTD_Id DataId;

        [HideInEditorMode]
        public BuffNodeDataDes BuffNodeDataDes;

        [LabelText("将要检查的技能ID（QWER：0123）")]
        public int SkillIDBelongTo;

        public NP_BlackBoardRelationData NPBalckBoardRelationData = new NP_BlackBoardRelationData();

        public override Func<bool> GetFunc1ToBeDone()
        {
            this.Func1 = this.CheckCostToSpanSkill;
            return this.Func1;
        }

        private bool CheckCostToSpanSkill()
        {
            this.BuffNodeDataDes = (BuffNodeDataDes) this.BelongtoRuntimeTree.BelongNP_DataSupportor.SkillDataDic[this.DataId.Value];
            //TODO 相关状态检测，例如沉默，眩晕等,下面是示例代码
            /*
            if (Game.Scene.GetComponent<UnitComponent>().Get(this.Unitid).GetComponent<BuffManagerComponent>()
                    .FindBuffByWorkType(BuffWorkTypes.Silence))
            {
                return false;
            }
            */
            //给要修改的黑板节点进行赋值
            HeroDataComponent heroDataComponent = Game.Scene.GetComponent<UnitComponent>().Get(this.Unitid).GetComponent<HeroDataComponent>();
            this.NPBalckBoardRelationData.SetBlackBoardValue(this.BelongtoRuntimeTree.GetBlackboard(),
                this.BuffNodeDataDes.SkillCost[heroDataComponent.GetSkillLevel(this.SkillIDBelongTo)]);
            switch (this.BuffNodeDataDes.SkillCostTypes)
            {
                case SkillCostTypes.MagicValue:
                    //依据技能具体消耗来进行属性改变操作
                    if (heroDataComponent.CurrentMagicValue > this.NPBalckBoardRelationData.GetBlackBoardValue<float>())
                        return true;
                    else
                    {
                        return false;
                    }
                case SkillCostTypes.Other:
                    return true;
                case SkillCostTypes.HPValue:
                    if (heroDataComponent.CurrentLifeValue > this.NPBalckBoardRelationData.GetBlackBoardValue<float>())
                        return true;
                    else
                    {
                        return false;
                    }
                default:
                    return true;
            }
        }
    }
}