using System;
using System.Collections.Generic;

namespace ET
{
    public class LSF_TickDispatcherComponent : Entity
    {
        public static LSF_TickDispatcherComponent Instance;

        public Dictionary<Type, ILSF_TickHandler> LSF_TickHandlers =
            new Dictionary<Type, ILSF_TickHandler>();

        public bool HasTicker(Type type)
        {
            return LSF_TickHandlers.ContainsKey(type);
        }
    }
}