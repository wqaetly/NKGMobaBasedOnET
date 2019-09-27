//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年9月7日 10:29:38
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using ETModel.TheDataContainsAction;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;
using Sirenix.OdinInspector;

namespace ETModel
{
    /// <summary>
    /// 检查技能是否能释放
    /// </summary>
    public class NP_CheckAction: NP_ClassForStoreAction
    {
        [LabelText("要引用的的数据结点ID")]
        public long dataId;

        [HideInEditorMode]
        public NodeDataForStartSkill m_NodeDataForStartSkill;

        [LabelText("将要检查的技能ID（QWER：0123）")]
        public int theSkillIDBelongTo;

        [LabelText("黑板相关数据")]
        public NP_BlackBoardRelationData m_NPBalckBoardRelationData;

        public override Func<bool> GetFunc1ToBeDone()
        {
            this.m_Func1 = this.CheckCostToSpanSkill;
            return this.m_Func1;
        }

        private bool CheckCostToSpanSkill()
        {
            this.m_NodeDataForStartSkill = (NodeDataForStartSkill)Game.Scene.GetComponent<UnitComponent>().Get(Unitid).GetComponent<NP_RuntimeTreeManager>()
                    .GetTreeByRuntimeID(this.RuntimeTreeID).m_BelongNP_DataSupportor.mSkillDataDic[this.dataId];
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
            m_NPBalckBoardRelationData.SetBlackBoardValue(Game.Scene.GetComponent<UnitComponent>().Get(this.Unitid)
                        .GetComponent<NP_RuntimeTreeManager>()
                        .GetTreeByRuntimeID(this.RuntimeTreeID)
                        .GetBlackboard(), m_NPBalckBoardRelationData.m_CompareType,
                m_NodeDataForStartSkill.SkillCost[heroDataComponent.GetSkillLevel(theSkillIDBelongTo)]);
            switch (m_NodeDataForStartSkill.SkillCostTypes)
            {
                case SkillCostTypes.MagicValue:
                    //依据技能具体消耗来进行属性改变操作
                    if (heroDataComponent.CurrentMagicValue > m_NPBalckBoardRelationData.theFloatValue)
                        return true;
                    else
                    {
                        return false;
                    }
                case SkillCostTypes.Other:
                    return true;
                case SkillCostTypes.HPValue:
                    if (heroDataComponent.CurrentLifeValue > m_NPBalckBoardRelationData.theFloatValue)
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