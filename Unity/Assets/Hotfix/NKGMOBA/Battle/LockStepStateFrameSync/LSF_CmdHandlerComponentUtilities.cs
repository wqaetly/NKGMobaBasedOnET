using System;
using System.Collections.Generic;

namespace ET
{
    public static class LSF_CmdHandlerComponentUtilities
    {
        public static void Load(this LSF_CmdDispatcherComponent self)
        {
            self.Handlers.Clear();
            HashSet<Type> types = Game.EventSystem.GetTypes(typeof(LSF_MessageHandlerAttribute));

            foreach (Type type in types)
            {
                ILockStepStateFrameSyncMessageHandler IlockStepStateFrameSyncMessageHandler =
                    Activator.CreateInstance(type) as ILockStepStateFrameSyncMessageHandler;
                if (IlockStepStateFrameSyncMessageHandler == null)
                {
                    Log.Error(
                        $"IlockStepStateFrameSyncMessageHandler handle {type.Name} 需要继承 ILockStepStateFrameSyncMessageHandler");
                    continue;
                }

                LSF_MessageHandlerAttribute lsfMessageHandlerAttribute =
                    Game.EventSystem.GetAttribute<LSF_MessageHandlerAttribute>(type);
                
                if (lsfMessageHandlerAttribute.LSF_CmdHandlerType == 0)
                {
                    Log.Error($"帧同步CmdType为0: {type.Name}");
                    continue;
                }

                self.RegisterHandler(lsfMessageHandlerAttribute.LSF_CmdHandlerType, IlockStepStateFrameSyncMessageHandler);
            }
        }

        public static void RegisterHandler(this LSF_CmdDispatcherComponent self, uint lsfCmdType,
            ILockStepStateFrameSyncMessageHandler handler)
        {
            if (!self.Handlers.ContainsKey(lsfCmdType))
            {
                self.Handlers.Add(lsfCmdType, new List<ILockStepStateFrameSyncMessageHandler>());
            }

            self.Handlers[lsfCmdType].Add(handler);
        }

        public static void Handle(this LSF_CmdDispatcherComponent self, Room room,
            ALSF_Cmd lAlsfCmd)
        {
            List<ILockStepStateFrameSyncMessageHandler> actions;
            if (!self.Handlers.TryGetValue(lAlsfCmd.LockStepStateFrameSyncDataType, out actions))
            {
                Log.Error($"帧同步指令没有处理: {lAlsfCmd.LockStepStateFrameSyncDataType} {lAlsfCmd.GetType()} {lAlsfCmd}");
                return;
            }

            foreach (ILockStepStateFrameSyncMessageHandler ev in actions)
            {
                try
                {
                    ev.Handle(room, lAlsfCmd);
                }
                catch (Exception e)
                {
                    Log.Error(e);
                }
            }
        }
    }
}