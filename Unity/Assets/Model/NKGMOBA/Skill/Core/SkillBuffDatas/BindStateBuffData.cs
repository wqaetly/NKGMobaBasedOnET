//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年5月16日 12:09:21
//------------------------------------------------------------

using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;
using Sirenix.OdinInspector;

namespace ETModel
{
    /// <summary>
    /// 绑定一个状态
    /// </summary>
    public class BindStateBuffData: BuffDataBase
    {
        [LabelText("是否可以叠加(不能叠加就刷新，叠加满也刷新)")]
        public bool CanOverlay;

        [LabelText("持续时间，-1为永久")]
        public float SustainTime = 0;
        
        [ShowIf("CanOverlay")]
        [LabelText("最大叠加数")]
        public int MaxOverlay;

        [LabelText("此状态自带的Buff")]
        public List<BuffDataBase> OriBuff = new List<BuffDataBase>();

    }
}