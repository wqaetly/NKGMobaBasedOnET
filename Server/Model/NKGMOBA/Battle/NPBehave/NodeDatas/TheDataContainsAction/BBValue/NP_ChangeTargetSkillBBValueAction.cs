//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2020年9月21日 15:21:08
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

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
        [Tooltip("比如诺手（Unit A）的Q技能会有一个碰撞体Unit（Unit B），这个Action是在UnitB上的一个行为树中的，如果此处为true，则会索引到UnitB而不是UnitA，否则就根据下面的<目标技能归属的英雄Unit Id>来获取目标")]
        [LabelText("目标技能归属Unit是否为自身的Unit")]
        public bool TargetUnitIsSelf = true;

        [HideIf("TargetUnitIsSelf")]
        [LabelText("目标技能归属的英雄Unit Id")]
        public NP_BlackBoardRelationData TargetUnitId = new NP_BlackBoardRelationData();

        [BoxGroup("目标技能Id配置")]
        [HideLabel]
        public VTD_Id TargetSkillId = new VTD_Id();

        [LabelText("要传递给目标技能的黑板值")]
        public NP_BlackBoardRelationData NPBBValue_ValueToChange = new NP_BlackBoardRelationData();

        [OnValueChanged("ApplyDataGetType")]
        public ValueGetType ValueGetType = ValueGetType.FromBBValue;

        public override Action GetActionToBeDone()
        {
            this.Action = this.ChangeTargetSkillBBValue;
            return this.Action;
        }

        public void ChangeTargetSkillBBValue()
        {
            //Log.Info($"修改黑板键{m_NPBalckBoardRelationData.DicKey} 黑板值类型 {m_NPBalckBoardRelationData.NP_BBValueType}  黑板值:Bool：{m_NPBalckBoardRelationData.BoolValue.GetValue()}\n");
            Unit targetUnit = null;

            if (this.TargetUnitIsSelf)
            {
                targetUnit = Game.Scene.GetComponent<UnitComponent>()
                        .Get(this.Unitid);
            }
            else
            {
#if SERVER
                targetUnit = Game.Scene.GetComponent<UnitComponent>()
                        .Get(TargetUnitId.GetBlackBoardValue<long>(this.BelongtoRuntimeTree.GetBlackboard()));
#endif
            }

            List<NP_RuntimeTree> skillContent = targetUnit.GetComponent<SkillCanvasManagerComponent>()
                    .GetSkillCanvas(this.TargetSkillId.Value);

            foreach (var skillCanvas in skillContent)
            {
                //除自己之外
                if (skillCanvas == this.BelongtoRuntimeTree)
                {
                    return;
                }

                if (this.ValueGetType == ValueGetType.FromDataSet)
                {
                    this.NPBBValue_ValueToChange.SetBlackBoardValue(skillCanvas.GetBlackboard());
                }
                else
                {
                    this.NPBBValue_ValueToChange.SetBBValueFromThisBBValue(this.BelongtoRuntimeTree.GetBlackboard(), skillCanvas.GetBlackboard());
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