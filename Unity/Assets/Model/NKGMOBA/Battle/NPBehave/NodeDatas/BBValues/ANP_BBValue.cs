//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2020年8月23日 13:53:21
//------------------------------------------------------------

using System;

namespace ET
{
    [ProtobufBaseTypeRegister]
    public abstract class ANP_BBValue
    {
        public abstract Type NP_BBValueType { get; }

        /// <summary>
        /// 从另一个anpBbValue设置数据
        /// </summary>
        /// <param name="anpBbValue"></param>
        public abstract void SetValueFrom(ANP_BBValue anpBbValue);
    }
}