//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年9月21日 10:09:03
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

#endif

namespace ETModel
{
    /// <summary>
    /// 监听Buff事件数据基类，用以监听指定事件
    /// </summary>
    public class ListenBuffEvent_Normal: AEvent<ABuffSystemBase>
    {
        /// <summary>
        /// Buff回调条件达成时会添加的Buff的节点Id
        /// </summary>
        [InfoBox("注意，是在节点编辑器中的Buff节点Id，而不是Buff自身的Id，别搞错了！")]
        [LabelText("Buff回调条件达成时会添加的Buff的节点Id")]
        public List<VTD_BuffInfo> BuffInfoWillBeAdded = new List<VTD_BuffInfo>();

        public override void Run(ABuffSystemBase a)
        {
            //Log.Info($"直接添加_通过监听机制增加Buff");
            foreach (var buffInfo in this.BuffInfoWillBeAdded)
            {
                for (int i = 0; i < buffInfo.Layers; i++)
                {
                    //Log.Info($"直接添加_通过监听机制增加id为{VARIABLE.FlagId}的Buff");
                    Game.Scene.GetComponent<BuffPoolComponent>()
                            .AcquireBuff(a.BuffData.BelongToBuffDataSupportorId, buffInfo.BuffId.Value, a.TheUnitFrom, a.TheUnitBelongto,
                                a.BelongtoRuntimeTree);
                }
            }
        }
    }
}