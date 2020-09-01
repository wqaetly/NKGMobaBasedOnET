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
    public class ListenBuffEventBase: AEvent<BuffSystemBase>
    {
        /// <summary>
        /// Buff回调条件达成时会添加的Buff
        /// </summary>
        [LabelText("Buff回调条件达成时会添加的Buff")]
        public List<BuffDataBase> m_BuffsWillBeAdded = new List<BuffDataBase>();

        public override void Run(BuffSystemBase a)
        {
            //Log.Info($"直接添加_通过监听机制增加Buff");
            foreach (var VARIABLE in m_BuffsWillBeAdded)
            {
                //Log.Info($"直接添加_通过监听机制增加id为{VARIABLE.FlagId}的Buff");
                Game.Scene.GetComponent<BuffPoolComponent>().AcquireBuff(VARIABLE, a.theUnitFrom, a.theUnitBelongto);
            }
        }
    }
    

    /// <summary>
    /// 监听Buff事件数据基类，用以监听指定事件
    /// </summary>
    public class ListenBuffEvent_CheckOverlay: ListenBuffEventBase
    {
        [LabelText("目标层数")]
        public int targetOverlay;

        public override void Run(BuffSystemBase a)
        {
            //Log.Info($"层数判定_通过监听机制添加Buff");
            if (a.CurrentOverlay == this.targetOverlay)
            {
                foreach (var VARIABLE in m_BuffsWillBeAdded)
                {
                    //Log.Info($"层数判定_通过监听机制添加id为{VARIABLE.FlagId}的Buff");
                    Game.Scene.GetComponent<BuffPoolComponent>().AcquireBuff(VARIABLE, a.theUnitFrom, a.theUnitBelongto);
                }
            }
        }
    }

    /// <summary>
    /// 监听Buff事件数据基类，用以监听指定事件
    /// </summary>
    public class ListenBuffDataBase: BuffDataBase
    {
        [BoxGroup("自定义项")]
        [LabelText("要监听的事件ID标识")]
        [ValueDropdown("GetEventIds")]
        public string EventId;

        /// <summary>
        /// Buff事件
        /// </summary>
        [BoxGroup("自定义项")]
        [HideLabel]
        public ListenBuffEventBase ListenBuffEventBase;

#if UNITY_EDITOR
        private IEnumerable<string> GetEventIds()
        {
            UnityEngine.Object[] subAssets = AssetDatabase.LoadAllAssetsAtPath(UnityEngine.PlayerPrefs.GetString("LastCanvasPath"));
            if (subAssets != null)
            {
                foreach (var subAsset in subAssets)
                {
                    if (subAsset is NPBehaveCanvasDataManager npBehaveCanvasDataManager)
                    {
                        return npBehaveCanvasDataManager.EventValues.Keys;
                    }
                }
            }

            return null;
        }
#endif
    }
    
}