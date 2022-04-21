//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年10月2日 12:17:56
//------------------------------------------------------------

using Sirenix.OdinInspector;

namespace ET
{
    public class ChangePropertyBuffData: BuffDataBase
    {
        /// <summary>
        /// 将要被添加的值
        /// </summary>
        [BoxGroup("自定义项")]
        [LabelText("将要被添加的值")]
        public float TheValueWillBeAdded = 0;
    }
}