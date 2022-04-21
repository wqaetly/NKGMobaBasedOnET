using System.Collections.Generic;
using System.Numerics;
using NPBehave;

namespace ET
{
    public static class NP_BBValueHelper
    {
        /// <summary>
        /// 通过ANP_BBValue来设置目标黑板值
        /// </summary>
        /// <param name="self"></param>
        /// <param name="blackboard"></param>
        /// <param name="key"></param>
        public static void SetTargetBlackboardUseANP_BBValue(ANP_BBValue anpBbValue, Blackboard blackboard, string key, bool isLocalPlayer = true)
        {
            // 这里只能用这个ToString()来做判断，直接获取Name的话是简略版本的
            switch (anpBbValue.NP_BBValueType.ToString())
            {
                case "System.String":
                    blackboard.Set(key, (anpBbValue as NP_BBValue_String).GetValue(), isLocalPlayer);
                    break;
                case "System.Single":
                    blackboard.Set(key, (anpBbValue as NP_BBValue_Float).GetValue(), isLocalPlayer);
                    break;
                case "System.Int32":
                    blackboard.Set(key, (anpBbValue as NP_BBValue_Int).GetValue(), isLocalPlayer);
                    break;
                case "System.Int64":
                    blackboard.Set(key, (anpBbValue as NP_BBValue_Long).GetValue(), isLocalPlayer);
                    break;
                case "System.UInt32":
                    blackboard.Set(key, (anpBbValue as NP_BBValue_UInt).GetValue(), isLocalPlayer);
                    break;
                case "System.Boolean":
                    blackboard.Set(key, (anpBbValue as NP_BBValue_Bool).GetValue(), isLocalPlayer);
                    break;
                case "System.Collections.Generic.List`1[System.Int64]":
                    blackboard.Set(key, (anpBbValue as NP_BBValue_List_Long).GetValue(), isLocalPlayer);
                    break;
                case "System.Numerics.Vector3":
                    blackboard.Set(key, (anpBbValue as NP_BBValue_Vector3).GetValue(), isLocalPlayer);
                    break;
            }
        }

        /// <summary>
        /// 自动从T创建一个NP_BBValue
        /// </summary>
        public static ANP_BBValue AutoCreateNPBBValueFromTValue<T>(T value)
        {
            string valueType = typeof(T).ToString();
            object boxedValue = value;
            ANP_BBValue anpBbValue = null;
            switch (valueType)
            {
                case "System.String":
                    anpBbValue = new NP_BBValue_String() {Value = boxedValue as string};
                    break;
                case "System.Single":
                    anpBbValue = new NP_BBValue_Float() {Value = (float) boxedValue};
                    break;
                case "System.Int32":
                    anpBbValue = new NP_BBValue_Int() {Value = (int) boxedValue};
                    break;
                case "System.Int64":
                    anpBbValue = new NP_BBValue_Long() {Value = (long) boxedValue};
                    break;
                case "System.UInt32":
                    anpBbValue = new NP_BBValue_UInt() {Value = (uint) boxedValue};
                    break;
                case "System.Boolean":
                    anpBbValue = new NP_BBValue_Bool() {Value = (bool) boxedValue};
                    break;
                case "System.Collections.Generic.List`1[System.Int64]":
                    //因为List是引用类型，所以这里要做一下特殊处理，如果要设置的值为0元素的List，就Clear一下，而且这个东西也不会用来做为黑板条件，因为它没办法用来对比
                    //否则就拷贝全部元素
                    NP_BBValue_List_Long list = new NP_BBValue_List_Long();
                    list.SetValueFrom((List<long>) boxedValue);
                    anpBbValue = list;
                    break;
                case "System.Numerics.Vector3":
                    anpBbValue = new NP_BBValue_Vector3() {Value = (Vector3) boxedValue};
                    break;
            }

            return anpBbValue;
        }


        /// <summary>
        /// 从anpBbValue中拷贝数据到self
        /// </summary>
        /// <param name="self"></param>
        /// <param name="anpBbValue"></param>
        public static void SetValueFrom(in ANP_BBValue self, ANP_BBValue anpBbValue)
        {
            if (anpBbValue == null)
            {
                Log.Error($"anpBbValue为空");
                return;
            }

            if (self.NP_BBValueType != anpBbValue.NP_BBValueType)
            {
                Log.Error(
                    $"两个类型不一致的NP_BBValue无法进行拷贝，Self：{self.NP_BBValueType.ToString()} anpBbValue: {anpBbValue.NP_BBValueType.ToString()}");
            }

            self.SetValueFrom(anpBbValue);
        }

        public static bool Compare(ANP_BBValue lhs, ANP_BBValue rhs, Operator op)
        {
            switch (op)
            {
                case Operator.IS_SET: return true;
                case Operator.IS_EQUAL:
                {
                    switch (lhs)
                    {
                        case NP_BBValue_Bool npBbValue:
                            return npBbValue == rhs as NP_BBValue_Bool;
                        case NP_BBValue_Float npBbValue:
                            return npBbValue == rhs as NP_BBValue_Float;
                        case NP_BBValue_Int npBbValue:
                            return npBbValue == rhs as NP_BBValue_Int;
                        case NP_BBValue_String npBbValue:
                            return npBbValue == rhs as NP_BBValue_String;
                        case NP_BBValue_Vector3 npBbValue:
                            return npBbValue == rhs as NP_BBValue_Vector3;
                        case NP_BBValue_Long npBbValue:
                            return npBbValue == rhs as NP_BBValue_Long;
                        case NP_BBValue_List_Long npBbValue:
                            return npBbValue == rhs as NP_BBValue_List_Long;
                        default:
                            Log.Error($"类型为{lhs.GetType()}的数未注册为NP_BBValue");
                            return false;
                    }
                }
                case Operator.IS_NOT_EQUAL:
                {
                    switch (lhs)
                    {
                        case NP_BBValue_Bool npBbValue:
                            return npBbValue != rhs as NP_BBValue_Bool;
                        case NP_BBValue_Float npBbValue:
                            return npBbValue != rhs as NP_BBValue_Float;
                        case NP_BBValue_Int npBbValue:
                            return npBbValue != rhs as NP_BBValue_Int;
                        case NP_BBValue_String npBbValue:
                            return npBbValue != rhs as NP_BBValue_String;
                        case NP_BBValue_Long npBbValue:
                            return npBbValue != rhs as NP_BBValue_Long;
                        case NP_BBValue_Vector3 npBbValue:
                            return npBbValue != rhs as NP_BBValue_Vector3;
                        case NP_BBValue_List_Long npBbValue:
                            return npBbValue != rhs as NP_BBValue_List_Long;
                        default:
                            Log.Error($"类型为{lhs.GetType()}的数未注册为NP_BBValue");
                            return false;
                    }
                }

                case Operator.IS_GREATER_OR_EQUAL:
                {
                    switch (lhs)
                    {
                        case NP_BBValue_Bool npBbValue:
                            return (rhs as NP_BBValue_Bool) >= npBbValue;
                        case NP_BBValue_Float npBbValue:
                            return (rhs as NP_BBValue_Float) >= npBbValue;
                        case NP_BBValue_Int npBbValue:
                            return (rhs as NP_BBValue_Int) >= npBbValue;
                        case NP_BBValue_String npBbValue:
                            return (rhs as NP_BBValue_String) >= npBbValue;
                        case NP_BBValue_Long npBbValue:
                            return (rhs as NP_BBValue_Long) >= npBbValue;
                        case NP_BBValue_Vector3 npBbValue:
                            return (rhs as NP_BBValue_Vector3) >= npBbValue;
                        case NP_BBValue_List_Long npBbValue:
                            return (rhs as NP_BBValue_List_Long) >= npBbValue;
                        default:
                            Log.Error($"类型为{lhs.GetType()}的数未注册为NP_BBValue");
                            return false;
                    }
                }

                case Operator.IS_GREATER:
                {
                    switch (lhs)
                    {
                        case NP_BBValue_Bool npBbValue:
                            return (rhs as NP_BBValue_Bool) > npBbValue;
                        case NP_BBValue_Float npBbValue:
                            return (rhs as NP_BBValue_Float) > npBbValue;
                        case NP_BBValue_Int npBbValue:
                            return (rhs as NP_BBValue_Int) > npBbValue;
                        case NP_BBValue_String npBbValue:
                            return (rhs as NP_BBValue_String) > npBbValue;
                        case NP_BBValue_Long npBbValue:
                            return (rhs as NP_BBValue_Long) > npBbValue;
                        case NP_BBValue_Vector3 npBbValue:
                            return (rhs as NP_BBValue_Vector3) > npBbValue;
                        case NP_BBValue_List_Long npBbValue:
                            return (rhs as NP_BBValue_List_Long) > npBbValue;
                        default:
                            Log.Error($"类型为{lhs.GetType()}的数未注册为NP_BBValue");
                            return false;
                    }
                }

                case Operator.IS_SMALLER_OR_EQUAL:
                    switch (lhs)
                    {
                        case NP_BBValue_Bool npBbValue:
                            return (rhs as NP_BBValue_Bool) <= npBbValue;
                        case NP_BBValue_Float npBbValue:
                            return (rhs as NP_BBValue_Float) <= npBbValue;
                        case NP_BBValue_Int npBbValue:
                            return (rhs as NP_BBValue_Int) <= npBbValue;
                        case NP_BBValue_String npBbValue:
                            return (rhs as NP_BBValue_String) <= npBbValue;
                        case NP_BBValue_Long npBbValue:
                            return (rhs as NP_BBValue_Long) <= npBbValue;
                        case NP_BBValue_Vector3 npBbValue:
                            return (rhs as NP_BBValue_Vector3) <= npBbValue;
                        default:
                            Log.Error($"类型为{lhs.GetType()}的数未注册为NP_BBValue");
                            return false;
                    }
                case Operator.IS_SMALLER:
                    switch (lhs)
                    {
                        case NP_BBValue_Bool npBbValue:
                            return (rhs as NP_BBValue_Bool) < npBbValue;
                        case NP_BBValue_Float npBbValue:
                            return (rhs as NP_BBValue_Float) < npBbValue;
                        case NP_BBValue_Int npBbValue:
                            return (rhs as NP_BBValue_Int) < npBbValue;
                        case NP_BBValue_String npBbValue:
                            return (rhs as NP_BBValue_String) < npBbValue;
                        case NP_BBValue_Long npBbValue:
                            return (rhs as NP_BBValue_Long) < npBbValue;
                        case NP_BBValue_Vector3 npBbValue:
                            return (rhs as NP_BBValue_Vector3) < npBbValue;
                        default:
                            Log.Error($"类型为{lhs.GetType()}的数未注册为NP_BBValue");
                            return false;
                    }

                default: return false;
            }
        }
    }
}