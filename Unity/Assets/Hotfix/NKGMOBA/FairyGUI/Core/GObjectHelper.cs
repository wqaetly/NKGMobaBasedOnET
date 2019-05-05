using FairyGUI;
using System.Collections.Generic;

namespace ETHotfix
{
    public static class GObjectHelper
    {
        private static Dictionary<GObject, FUI> keyValuePairs = new Dictionary<GObject, FUI>();

        public static T Get<T>(this GObject self) where T : FUI
        {
            if (self != null && keyValuePairs.ContainsKey(self))
            {
                return keyValuePairs[self] as T;
            }

            return default(T);
        }

        public static void Add(this GObject self, FUI fui)
        {
            if (self != null && fui != null)
            {
                keyValuePairs[self] = fui;
            }
        }

        public static FUI Remove(this GObject self)
        {
            if (self != null && keyValuePairs.ContainsKey(self))
            {
                var result = keyValuePairs[self];
                keyValuePairs.Remove(self);
                return result;
            }

            return null;
        }
    }
}