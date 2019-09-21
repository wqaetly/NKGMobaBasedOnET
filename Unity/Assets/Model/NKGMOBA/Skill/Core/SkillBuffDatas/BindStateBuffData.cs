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
        [LabelText("是否可以叠加")]
        public bool CanOverlay;
        
        [ShowIf("CanOverlay")]
        [LabelText("最大叠加数")]
        public int MaxOverlay;

        [LabelText("此状态自带的Buff")]
        public List<BuffDataBase> OriBuff = new List<BuffDataBase>();

        [LabelText("叠加完成后要执行的Buff")]
        public List<BuffDataBase> buffAddedWhenOverlayFinish = new List<BuffDataBase>();
    }
}