//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2020年8月23日 14:22:01
//------------------------------------------------------------

using System;
using Animancer.FSM;
using Sirenix.OdinInspector;

namespace ETModel.BBValues
{
    public abstract class NP_BBValueBase<T>: ANP_BBValue, INP_BBValue<T>
    {
        [LabelText("值")]
        public T Value;

        public T GetValue()
        {
            return Value;
        }

        public void SetValue(INP_BBValue<T> bbValue)
        {
            Value = bbValue.GetValue();
        }

        public void SetValue(T bbValue)
        {
            Value = bbValue;
        }
    }
}