//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2021年9月1日 21:53:23
//------------------------------------------------------------

using MonKey;
using UnityEditor;

namespace ET
{
    public class Proto2CS
    {
        [Command("ETEditor_Proto2CS", "PB协议导出", Category = "ETEditor")]
        public static void DoProto2CS()
        {
            ProcessHelper.Run("Proto2CS.exe", "", "../Tools/Proto2CS/Bin/");
        }
    }
}