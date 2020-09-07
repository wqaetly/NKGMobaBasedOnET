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
    public class ListenBuffEventBase: AEvent<ABuffSystemBase>
    {
        /// <summary>
        /// Buff回调条件达成时会添加的Buff
        /// </summary>
        [LabelText("Buff回调条件达成时会添加的Buff")]
        public List<BuffDataBase> m_BuffsWillBeAdded = new List<BuffDataBase>();

        public override void Run(ABuffSystemBase a)
        {
            //Log.Info($"直接添加_通过监听机制增加Buff");
            foreach (var VARIABLE in m_BuffsWillBeAdded)
            {
                //Log.Info($"直接添加_通过监听机制增加id为{VARIABLE.FlagId}的Buff");
                Game.Scene.GetComponent<BuffPoolComponent>().AcquireBuff(VARIABLE, a.TheUnitFrom, a.TheUnitBelongto);
            }
        }
    }
}