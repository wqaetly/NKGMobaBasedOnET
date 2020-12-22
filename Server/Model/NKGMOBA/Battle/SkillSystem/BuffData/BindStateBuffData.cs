//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年5月16日 12:09:21
//------------------------------------------------------------

using System.Collections.Generic;
using ETModel.NKGMOBA.Battle.State;
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
        [BoxGroup("自定义项")]
        [LabelText("此状态自带的状态数据")]
        public CustomState OriState = new CustomState();

        [BoxGroup("自定义项")]
        [InfoBox("注意，是在节点编辑器中的Buff节点Id，而不是Buff自身的Id，别搞错了！")]
        [LabelText("此状态自带的Buff节点Id")]
        public List<VTD_BuffInfo> OriBuff = new List<VTD_BuffInfo>();
    }
}