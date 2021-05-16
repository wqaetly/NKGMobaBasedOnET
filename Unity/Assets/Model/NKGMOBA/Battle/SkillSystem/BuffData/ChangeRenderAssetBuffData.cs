//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2021年5月12日 17:48:44
//------------------------------------------------------------

using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace ETModel
{
    public class ChangeRenderAssetBuffData: BuffDataBase
    {
        /// <summary>
        /// 将要被激活的RenderFeature名称
        /// </summary>
        [BoxGroup("自定义项")]
        [LabelText("将要被激活的RenderFeature名称")]
        public List<string> RenderFeatureNameToActive = new List<string>();
    }
}