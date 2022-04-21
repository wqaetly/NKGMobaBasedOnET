//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2021年9月1日 21:27:41
//------------------------------------------------------------

using MonKey;
using UnityEditor;

namespace ET
{
    public class ExcelExport
    {
        [Command("ETEditor_DoExcelExport", "Excel配置表导出", Category = "ETEditor")]
        public static void DoExcelExport()
        {
            ProcessHelper.Run("ExcelExporter.exe", "", "../Tools/ExcelExporter/Bin/");
        }
    }
}