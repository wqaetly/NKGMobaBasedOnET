//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2021年8月26日 22:15:52
//------------------------------------------------------------

using System.Collections.Generic;
using FairyGUI;

namespace ET
{
    /// <summary>
    /// Mono层的简易FGUI管理器
    /// </summary>
    public class FUIManager_MonoOnly
    {
        public static Dictionary<string, GComponent> AllMonoFUIs = new Dictionary<string, GComponent>();

        public static void AddUI(string name, GComponent gComponent)
        {
            AllMonoFUIs[name] = gComponent;
            GRoot.inst.AddChild(gComponent);
        }

        public static T GetUI<T>(string name) where T : GComponent
        {
            return AllMonoFUIs[name] as T;
        }

        public static void RemoveUI(string name)
        {
            AllMonoFUIs[name].Dispose();
            AllMonoFUIs.Remove(name);
        }
    }
}