//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2020年12月23日 16:05:11
//------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace ETModel.NKGMOBA.Battle.State
{
    /// <summary>
    /// 为动画播放拓展StateType方法
    /// </summary>
    public static class StateTypeForAnimExtension
    {
        private static Dictionary<StateTypes, string> AllMap = new Dictionary<StateTypes, string>();

        static StateTypeForAnimExtension()
        {
            foreach (var stateType in Enum.GetValues(typeof (StateTypes)))
            {
                AllMap.Add((StateTypes) stateType, Enum.GetName(typeof (StateTypes), stateType));
            }
        }

        public static string GetStateTypeMapedString(this StateTypes self)
        {
            return AllMap[self];
        }
    }
}