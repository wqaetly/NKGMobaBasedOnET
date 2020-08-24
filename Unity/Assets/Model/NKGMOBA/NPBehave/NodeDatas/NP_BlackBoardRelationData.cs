//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年9月25日 13:59:03
//------------------------------------------------------------

using System.Collections.Generic;
using ETModel.BBValues;
using NPBehave;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ETModel
{
    /// <summary>
    /// 与黑板节点相关的数据
    /// </summary>
    [BoxGroup("黑板数据配置"), GUIColor(0.961f, 0.902f, 0.788f, 1f)]
    [HideLabel]
    public class NP_BlackBoardRelationData
    {
        [LabelText("字典键")]
        [ValueDropdown("GetBBKeys")]
        public string DicKey;

        [LabelText("指定的值类型")]
        [OnValueChanged("ApplyValueTypeChange")]
        public NP_BBValueType NP_BBValueType;

        [ShowIf("NP_BBValueType", NP_BBValueType._String)]
        public NP_BBValue_String StringValue;

        [ShowIf("NP_BBValueType", NP_BBValueType._Float)]
        public NP_BBValue_Float FloatValue;

        [ShowIf("NP_BBValueType", NP_BBValueType._Int)]
        public NP_BBValue_Int IntValue;

        [ShowIf("NP_BBValueType", NP_BBValueType._Bool)]
        public NP_BBValue_Bool BoolValue;

        [ShowIf("NP_BBValueType", NP_BBValueType._Vector3)]
        public NP_BBValue_Vector3 Vector3Value;

#if !SERVER
        public static IEnumerable<string> BBKeys;

        private static IEnumerable<string> GetBBKeys()
        {
            return BBKeys;
        }

        public void ApplyValueTypeChange()
        {
            StringValue = null;
            FloatValue = null;
            IntValue = null;
            BoolValue = null;
            Vector3Value = null;
            switch (this.NP_BBValueType)
            {
                case NP_BBValueType._String:
                    StringValue = new NP_BBValue_String();
                    break;
                case NP_BBValueType._Float:
                    this.FloatValue = new NP_BBValue_Float();
                    break;
                case NP_BBValueType._Int:
                    this.IntValue = new NP_BBValue_Int();
                    break;
                case NP_BBValueType._Bool:
                    this.BoolValue = new NP_BBValue_Bool();
                    break;
                case NP_BBValueType._Vector3:
                    this.Vector3Value = new NP_BBValue_Vector3();
                    break;
            }
        }
#endif

        /// <summary>
        /// 自动根据预先设定的值设置值
        /// </summary>
        /// <param name="blackboard">要修改的黑板</param>
        public void SetBlackBoardValue(Blackboard blackboard)
        {
            switch (this.NP_BBValueType)
            {
                case NP_BBValueType._String:
                    blackboard.Set(DicKey,this.StringValue.GetValue());
                    break;
                case NP_BBValueType._Float:
                    blackboard.Set(DicKey,this.FloatValue.GetValue());
                    break;
                case NP_BBValueType._Int:
                    blackboard.Set(DicKey,this.IntValue.GetValue());
                    break;
                case NP_BBValueType._Bool:
                    blackboard.Set(DicKey,this.BoolValue.GetValue());
                    break;
                case NP_BBValueType._Vector3:
                    blackboard.Set(DicKey,this.Vector3Value.GetValue());
                    break;
            }
        }

        /// <summary>
        /// 自动根据传来的值设置值
        /// </summary>
        /// <param name="blackboard">将要改变的黑板值</param>
        /// <param name="compareType">值类型</param>
        /// <param name="value">值</param>
        public void SetBlackBoardValue<T>(Blackboard blackboard, T value)
        {
            blackboard.Set(this.DicKey, value);
        }
    }
}