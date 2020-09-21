//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2020年9月21日 15:21:08
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace ETModel
{
    [LabelText("数据获取方式")]
    public enum ValueGetType: byte
    {
        [LabelText("从这个数据配置中获取")]
        FromDataSet,

        [LabelText("从黑板中获取")]
        FromBBValue,
    }

    /// <summary>
    /// 修改目标技能黑板
    /// </summary>
    public class NP_ChangeTargetSkillBBValueAction: NP_ClassForStoreAction
    {
        [LabelText("目标技能标识")]
        public NP_BlackBoardRelationData NPBBBValue_TargetSkillId = new NP_BlackBoardRelationData();

        [OnValueChanged("ApplyDataGetType")]
        public ValueGetType ValueGetType = ValueGetType.FromBBValue;

        [LabelText("将要改变的BBValue")]
        public NP_BlackBoardRelationData NPBBValue_ValueToChange = new NP_BlackBoardRelationData();

        public override Action GetActionToBeDone()
        {
            this.Action = this.ChangeTargetSkillBBValue;
            return this.Action;
        }

        public void ChangeTargetSkillBBValue()
        {
            //Log.Info($"修改黑板键{m_NPBalckBoardRelationData.DicKey} 黑板值类型 {m_NPBalckBoardRelationData.NP_BBValueType}  黑板值:Bool：{m_NPBalckBoardRelationData.BoolValue.GetValue()}\n");
            Unit unit = Game.Scene.GetComponent<UnitComponent>().Get(this.Unitid);
            List<NP_RuntimeTree> skillContent = unit.GetComponent<SkillCanvasManagerComponent>()
                    .GetSkillCanvas(NPBBBValue_TargetSkillId.GetTheBBDataValue<int>());

            foreach (var skillCanvas in skillContent)
            {
                if (this.ValueGetType == ValueGetType.FromDataSet)
                {
                    this.NPBBValue_ValueToChange.SetBlackBoardValue(skillCanvas.GetBlackboard());
                }
                else
                {
                    this.NPBBValue_ValueToChange.SetBlackBoardValue(this.BelongtoRuntimeTree.GetBlackboard(), skillCanvas.GetBlackboard());
                }
            }
        }

#if UNITY_EDITOR
        private void ApplyDataGetType()
        {
            if (this.ValueGetType == ValueGetType.FromDataSet)
            {
                NPBBValue_ValueToChange.WriteOrCompareToBB = true;
            }
            else
            {
                NPBBValue_ValueToChange.WriteOrCompareToBB = false;
            }
        }
#endif
    }
}