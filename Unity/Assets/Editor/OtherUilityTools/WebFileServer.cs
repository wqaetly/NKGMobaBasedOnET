//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2020年10月14日 16:58:05
//------------------------------------------------------------

using ETModel;
using UnityEditor;

namespace ETEditor
{
    public class WebFileServer
    {
        [MenuItem("NKGTools/ET/本地资源服务器")]
        public static void OpenFileServer()
        {
            ProcessHelper.Run("dotnet", "FileServer.dll", "../FileServer/");
        }
    }
}