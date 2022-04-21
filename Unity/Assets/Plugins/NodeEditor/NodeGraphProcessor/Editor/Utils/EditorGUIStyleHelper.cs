//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2021年6月9日 12:42:03
//------------------------------------------------------------

using System.Collections.Generic;
using UnityEngine;

namespace GraphProcessor
{
    public static class EditorGUIStyleHelper
    {
        private static Dictionary<string, GUIStyle> s_Styles = new Dictionary<string, GUIStyle>();

        static EditorGUIStyleHelper()
        {
            s_Styles.Clear();
        }

        /// <summary>
        /// 通过名称获取指定GUIStyle副本（深拷贝），内部缓存
        /// 例如EditorGUIStyleHelper.GetGUIStyleByName(nameof(EditorStyles.toolbarButton));
        /// </summary>
        /// <param name="styleName"></param>
        /// <returns></returns>
        public static GUIStyle GetGUIStyleByName(string styleName)
        {
            if (s_Styles.TryGetValue(styleName, out var guiStyle))
            {
                return guiStyle;
            }
            else
            {
                GUIStyle finalResult = new GUIStyle(styleName);
                s_Styles[styleName] = finalResult;
                return finalResult;
            }
        }

        /// <summary>
        /// 通过名称指定GUIStyle副本（深拷贝）的padding，内部缓存
        /// 例如EditorGUIStyleHelper.SetGUIStylePadding(nameof(EditorStyles.toolbarButton), new RectOffset(20, 20, 0, 0));
        /// </summary>
        /// <param name="styleName"></param>
        /// <returns></returns>
        public static GUIStyle SetGUIStylePadding(string styleName, RectOffset rectOffset)
        {
            GUIStyle guiStyle = GetGUIStyleByName(styleName);
            guiStyle.padding = rectOffset;
            return guiStyle;
        }
    }
}