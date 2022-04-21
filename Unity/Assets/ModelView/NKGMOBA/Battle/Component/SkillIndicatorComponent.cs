using System.Collections.Generic;
using Werewolf.StatusIndicators.Components;

namespace ET
{
    public class SkillIndicatorComponent : Entity
    {
        public Dictionary<string, Splat> Splats = new Dictionary<string, Splat>();

        public void AddSplats(string name, Splat splat)
        {
            Splats[name] = splat;
        }

        public T GetSplate<T>(string name) where T : Splat
        {
            if (Splats.TryGetValue(name, out var splat))
            {
                return splat as T;
            }

            return default(T);
        }
        
        public void RemoveSplate(string name)
        {
            Splats.Remove(name);
        }
    }
}