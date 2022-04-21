using System.IO;
using MonKey;
using UnityEditor;

namespace ET
{
    public static class WebFileHelper
    {
        [Command("ETEditor_WebFileServer","本地资源服务器",Category = "ETEditor")]
        public static void OpenFileServer()
        {
            ProcessHelper.Run("dotnet", "FileServer.dll", "../FileServer/");
        }
    }
}
