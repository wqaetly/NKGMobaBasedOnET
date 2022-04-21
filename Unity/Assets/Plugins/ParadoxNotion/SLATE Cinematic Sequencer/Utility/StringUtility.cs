using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Slate
{

    public static class StringUtility
    {

        ///Convert camelCase to words as the name implies.
        public static string SplitCamelCase(this string s) {
            if ( string.IsNullOrEmpty(s) ) return s;
            s = char.ToUpper(s[0]) + s.Substring(1);
            return System.Text.RegularExpressions.Regex.Replace(s, "(?<=[a-z])([A-Z])", " $1").Trim();
        }

        ///Absolute path to relative project path
        public static string AbsToRelativePath(string absolutepath) {
            if ( absolutepath.StartsWith(Application.dataPath) ) {
                return "Assets" + absolutepath.Substring(Application.dataPath.Length);
            }
            return null;
        }
    }
}