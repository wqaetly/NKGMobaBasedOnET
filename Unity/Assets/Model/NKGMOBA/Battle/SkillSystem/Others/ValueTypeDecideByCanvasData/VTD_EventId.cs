//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2020年9月7日 20:57:34
//------------------------------------------------------------

using System.Collections.Generic;
using Sirenix.OdinInspector;
#if UNITY_EDITOR
using UnityEditor;

#endif

namespace ET
{
    [HideReferenceObjectPicker]
    public struct VTD_EventId
    {
        [ValueDropdown("GetEventId")]
        public string Value;

#if UNITY_EDITOR
        private IEnumerable<string> GetEventId()
        {
            if (NP_BlackBoardDataManager.CurrentEditedNP_BlackBoardDataManager != null)
            {
                return NP_BlackBoardDataManager.CurrentEditedNP_BlackBoardDataManager.EventValues;
            }

            return null;
        }
#endif
    }
}