//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2020年9月13日 20:12:57
//------------------------------------------------------------

using System;
using ETModel.BBValues;
using NPBehave;
using Sirenix.OdinInspector;
using Action = System.Action;

namespace ETModel
{
    [Title("检查指定黑板数据", TitleAlignment = TitleAlignments.Centered)]
    public class NP_CheckBBValueAction: NP_ClassForStoreAction
    {
        [LabelText("运算符号")]
        public Operator Ope = Operator.IS_EQUAL;

        public NP_BlackBoardRelationData NpBlackBoardRelationData = new NP_BlackBoardRelationData() { WriteOrCompareToBB = true };

        public override Func<bool> GetFunc1ToBeDone()
        {
            this.Func1 = this.CheckBBValue;
            return this.Func1;
        }

        private bool CheckBBValue()
        {
            if (Ope == Operator.ALWAYS_TRUE)
            {
                return true;
            }

            if (!this.BelongtoRuntimeTree.GetBlackboard().Isset(NpBlackBoardRelationData.BBKey))
            {
                return Ope == Operator.IS_NOT_SET;
            }

            ANP_BBValue bbValue = this.BelongtoRuntimeTree.GetBlackboard().Get(NpBlackBoardRelationData.BBKey),
                    preSetBBValue = this.NpBlackBoardRelationData.NP_BBValue;

            switch (this.Ope)
            {
                case Operator.IS_SET: return true;
                case Operator.IS_EQUAL:
                {
                    switch (preSetBBValue)
                    {
                        case NP_BBValue_Bool npBbValue:
                            return npBbValue == bbValue as NP_BBValue_Bool;
                        case NP_BBValue_Float npBbValue:
                            return npBbValue == bbValue as NP_BBValue_Float;
                        case NP_BBValue_Int npBbValue:
                            return npBbValue == bbValue as NP_BBValue_Int;
                        case NP_BBValue_String npBbValue:
                            return npBbValue == bbValue as NP_BBValue_String;
                        case NP_BBValue_Vector3 npBbValue:
                            return npBbValue == bbValue as NP_BBValue_Vector3;
                        default:
                            Log.Error($"类型为{preSetBBValue.GetType()}的数未注册为NP_BBValue");
                            return false;
                    }
                }
                case Operator.IS_NOT_EQUAL:
                {
                    switch (preSetBBValue)
                    {
                        case NP_BBValue_Bool npBbValue:
                            return npBbValue != bbValue as NP_BBValue_Bool;
                        case NP_BBValue_Float npBbValue:
                            return npBbValue != bbValue as NP_BBValue_Float;
                        case NP_BBValue_Int npBbValue:
                            return npBbValue != bbValue as NP_BBValue_Int;
                        case NP_BBValue_String npBbValue:
                            return npBbValue != bbValue as NP_BBValue_String;
                        case NP_BBValue_Vector3 npBbValue:
                            return npBbValue != bbValue as NP_BBValue_Vector3;
                        default:
                            Log.Error($"类型为{preSetBBValue.GetType()}的数未注册为NP_BBValue");
                            return false;
                    }
                }

                case Operator.IS_GREATER_OR_EQUAL:
                {
                    switch (preSetBBValue)
                    {
                        case NP_BBValue_Bool npBbValue:
                            return npBbValue >= (bbValue as NP_BBValue_Bool);
                        case NP_BBValue_Float npBbValue:
                            return npBbValue >= (bbValue as NP_BBValue_Float);
                        case NP_BBValue_Int npBbValue:
                            return npBbValue >= (bbValue as NP_BBValue_Int);
                        case NP_BBValue_String npBbValue:
                            return npBbValue >= (bbValue as NP_BBValue_String);
                        case NP_BBValue_Vector3 npBbValue:
                            return npBbValue >= (bbValue as NP_BBValue_Vector3);
                        default:
                            Log.Error($"类型为{preSetBBValue.GetType()}的数未注册为NP_BBValue");
                            return false;
                    }
                }

                case Operator.IS_GREATER:
                {
                    switch (preSetBBValue)
                    {
                        case NP_BBValue_Bool npBbValue:
                            return npBbValue > (bbValue as NP_BBValue_Bool);
                        case NP_BBValue_Float npBbValue:
                            return npBbValue > (bbValue as NP_BBValue_Float);
                        case NP_BBValue_Int npBbValue:
                            return npBbValue > (bbValue as NP_BBValue_Int);
                        case NP_BBValue_String npBbValue:
                            return npBbValue > (bbValue as NP_BBValue_String);
                        case NP_BBValue_Vector3 npBbValue:
                            return npBbValue > (bbValue as NP_BBValue_Vector3);
                        default:
                            Log.Error($"类型为{preSetBBValue.GetType()}的数未注册为NP_BBValue");
                            return false;
                    }
                }

                case Operator.IS_SMALLER_OR_EQUAL:
                    switch (preSetBBValue)
                    {
                        case NP_BBValue_Bool npBbValue:
                            return npBbValue <= (bbValue as NP_BBValue_Bool);
                        case NP_BBValue_Float npBbValue:
                            return npBbValue <= (bbValue as NP_BBValue_Float);
                        case NP_BBValue_Int npBbValue:
                            return npBbValue <= (bbValue as NP_BBValue_Int);
                        case NP_BBValue_String npBbValue:
                            return npBbValue <= (bbValue as NP_BBValue_String);
                        case NP_BBValue_Vector3 npBbValue:
                            return npBbValue <= (bbValue as NP_BBValue_Vector3);
                        default:
                            Log.Error($"类型为{preSetBBValue.GetType()}的数未注册为NP_BBValue");
                            return false;
                    }
                case Operator.IS_SMALLER:
                    switch (preSetBBValue)
                    {
                        case NP_BBValue_Bool npBbValue:
                            return npBbValue < (bbValue as NP_BBValue_Bool);
                        case NP_BBValue_Float npBbValue:
                            return npBbValue < (bbValue as NP_BBValue_Float);
                        case NP_BBValue_Int npBbValue:
                            return npBbValue < (bbValue as NP_BBValue_Int);
                        case NP_BBValue_String npBbValue:
                            return npBbValue < (bbValue as NP_BBValue_String);
                        case NP_BBValue_Vector3 npBbValue:
                            return npBbValue < (bbValue as NP_BBValue_Vector3);
                        default:
                            Log.Error($"类型为{preSetBBValue.GetType()}的数未注册为NP_BBValue");
                            return false;
                    }

                default: return false;
            }
        }
    }
}