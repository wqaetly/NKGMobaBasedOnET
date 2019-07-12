//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年7月12日 20:55:57
//------------------------------------------------------------

using System.Collections.Generic;

namespace ETModel
{
    /// <summary>
    /// 名称与id映射字典
    /// </summary>
    public class ColliderNameAndIdInflectSupporter
    {
        public Dictionary<string, long> colliderNameAndIdInflectDic = new Dictionary<string, long>();
    }

    /// <summary>
    /// 碰撞体数据字典
    /// </summary>
    public class ColliderDataSupporter
    {
        public Dictionary<long, B2S_ColliderVisualHelperBase> colliderDataDic = new Dictionary<long, B2S_ColliderVisualHelperBase>();
    }
}