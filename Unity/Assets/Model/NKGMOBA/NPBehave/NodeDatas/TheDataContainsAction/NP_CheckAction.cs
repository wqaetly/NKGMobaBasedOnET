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
    public class NP_CheckAction: NP_ClassForStoreAction
    {
        [LabelText("技能花费")]
        public SkillCostTypes SkillCostTypes;

        public override Func<bool> GetFunc1ToBeDone()
        {
            this.m_Func1 = this.CheckCostToSpanSkill;
            return this.m_Func1;
        }

        private bool CheckCostToSpanSkill()
        {
            HeroDataComponent heroDataComponent = Game.Scene.GetComponent<UnitComponent>().Get(this.Unitid).GetComponent<HeroDataComponent>();
            switch (SkillCostTypes)
            {
                case SkillCostTypes.MagicValue:
                    if (heroDataComponent.CurrentMagicValue > 0)
                        return true;
                    else
                    {
                        return false;
                    }
                case SkillCostTypes.Other:
                    return true;
                case SkillCostTypes.HPValue:
                    if (heroDataComponent.CurrentLifeValue > 0)
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