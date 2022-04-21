//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年9月25日 13:59:03
//------------------------------------------------------------

using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using NPBehave;
using Sirenix.OdinInspector;
using Vector3 = System.Numerics.Vector3;
#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

#endif

namespace ET
{
    /// <summary>
    /// 与黑板节点相关的数据
    /// </summary>
    [BoxGroup("黑板数据配置"), GUIColor(0.961f, 0.902f, 0.788f, 1f)]
    [HideLabel]
    public class NP_BlackBoardRelationData
    {
        [LabelText("字典键")] [ValueDropdown("GetBBKeys")] [OnValueChanged("OnBBKeySelected")]
        public string BBKey;

        [LabelText("指定的值类型")] [ReadOnly] public string NP_BBValueType;

        [LabelText("是否可以把值写入黑板，或者是否与黑板进行值对比")] [BsonIgnore]
        public bool WriteOrCompareToBB;

        [ShowIf("WriteOrCompareToBB")] public ANP_BBValue NP_BBValue;

#if UNITY_EDITOR
        private IEnumerable<string> GetBBKeys()
        {
            if (NP_BlackBoardDataManager.CurrentEditedNP_BlackBoardDataManager != null)
            {
                return NP_BlackBoardDataManager.CurrentEditedNP_BlackBoardDataManager.BBValues.Keys;
            }

            return null;
        }

        private void OnBBKeySelected()
        {
            if (NP_BlackBoardDataManager.CurrentEditedNP_BlackBoardDataManager != null)
            {
                foreach (var bbValues in NP_BlackBoardDataManager.CurrentEditedNP_BlackBoardDataManager.BBValues)
                {
                    if (bbValues.Key == this.BBKey)
                    {
                        NP_BBValue = bbValues.Value.DeepCopy();
                        NP_BBValueType = this.NP_BBValue.NP_BBValueType.ToString();
                    }
                }
            }
        }
#endif

        /// <summary>
        /// 获取目标黑板对应的此处的键的值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetBlackBoardValue<T>(Blackboard blackboard)
        {
            return blackboard.Get<T>(this.BBKey);
        }

        /// <summary>
        /// 获取配置的BB值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetTheBBDataValue<T>()
        {
            return (this.NP_BBValue as NP_BBValueBase<T>).GetValue();
        }

        /// <summary>
        /// 自动根据预先设定的值设置值
        /// </summary>
        /// <param name="blackboard">要修改的黑板</param>
        public void SetBlackBoardValue(Blackboard blackboard)
        {
            NP_BBValueHelper.SetTargetBlackboardUseANP_BBValue(this.NP_BBValue, blackboard, BBKey);
        }

        /// <summary>
        /// 自动根据传来的值设置值
        /// </summary>
        /// <param name="blackboard">将要改变的黑板值</param>
        /// <param name="value">值</param>
        public void SetBlackBoardValue<T>(Blackboard blackboard, T value)
        {
            blackboard.Set(this.BBKey, value);
        }

        /// <summary>
        /// 自动将一个黑板的对应key的value设置到另一个黑板上
        /// </summary>
        /// <param name="oriBB">数据源黑板</param>
        /// <param name="desBB">目标黑板</param>
        public void SetBBValueFromThisBBValue(Blackboard oriBB, Blackboard desBB)
        {
            NP_BBValueHelper.SetTargetBlackboardUseANP_BBValue(oriBB.Get(BBKey), desBB, BBKey);
        }
    }
}