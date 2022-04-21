using System.Collections.Generic;
using UnityEditor;

namespace GraphProcessor
{
    public static class NodeGraphWindowHelper
    {
        public static Dictionary<string, BaseGraphWindow> AllNodeGraphWindows =
            new Dictionary<string, BaseGraphWindow>();

        public static T GetAndShowNodeGraphWindow<T>(string path) where T : BaseGraphWindow
        {
            if (AllNodeGraphWindows.TryGetValue(path, out var universalGraphWindow))
            {
                universalGraphWindow.Focus();
                return universalGraphWindow as T;
            }

            T resultWindow = EditorWindow.CreateWindow<T>(typeof(T));
            AllNodeGraphWindows[path] = resultWindow;
            return resultWindow;
        }

        public static T GetAndShowNodeGraphWindow<T>(BaseGraph owner) where T : BaseGraphWindow
        {
            return GetAndShowNodeGraphWindow<T>(AssetDatabase.GetAssetPath(owner));
        }

        public static void AddNodeGraphWindow(BaseGraph owner, BaseGraphWindow universalGraphWindow)
        {
            AllNodeGraphWindows[AssetDatabase.GetAssetPath(owner)] = universalGraphWindow;
        }
        
        public static void RemoveNodeGraphWindow(string path)
        {
            AllNodeGraphWindows.Remove(path);
        }
        
        public static void RemoveNodeGraphWindow(BaseGraph owner)
        {
            AllNodeGraphWindows.Remove(AssetDatabase.GetAssetPath(owner));
        }
    }
}