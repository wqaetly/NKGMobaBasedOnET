//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2021年5月12日 19:20:13
//------------------------------------------------------------

using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace ETModel
{
    public class ChangeMaterialBuffData: BuffDataBase
    {
        /// <summary>
        /// 将要被添加的材质名
        /// </summary>
        [BoxGroup("自定义项")]
        [LabelText("将要被添加的材质名")]
        public List<string> TheMaterialNameWillBeAdded = new List<string>();
    }
}