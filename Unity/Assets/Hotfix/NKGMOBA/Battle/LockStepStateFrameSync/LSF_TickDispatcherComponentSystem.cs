using System;

namespace ET
{
    public class LSF_TickDispatcherComponentAwakeSystem : AwakeSystem<LSF_TickDispatcherComponent>
    {
        public override void Awake(LSF_TickDispatcherComponent self)
        {
            self.LSF_TickHandlers.Clear();

            LSF_TickDispatcherComponent.Instance = self;

            var types = Game.EventSystem.GetTypes(typeof(LSF_TickableAttribute));
            foreach (Type type in types)
            {
                ILSF_TickHandler instance = Activator.CreateInstance(type) as ILSF_TickHandler;
                if (instance == null)
                {
                    Log.Error($"robot ai is not ILSF_TickHandler: {type.Name}");
                    continue;
                }

                LSF_TickableAttribute lsfTickableAttribute = Game.EventSystem.GetAttribute<LSF_TickableAttribute>(type);

                self.LSF_TickHandlers.Add(lsfTickableAttribute.EntityType, instance);
            }
        }
    }

    public static class LSF_TickDispatcherUtilities
    {
#if !SERVER
        public static void HandleLSF_RollBack(this LSF_TickDispatcherComponent self, Entity entity, uint frame,
            ALSF_Cmd alsfCmd)
        {
            if (self.LSF_TickHandlers.TryGetValue(entity.GetType(), out var handler))
            {
                handler.LSF_RollBackTick(entity, frame, alsfCmd);
            }
        }

        public static bool HandleLSF_CheckConsistency(this LSF_TickDispatcherComponent self, Entity entity, uint frame,
            ALSF_Cmd alsfCmd)
        {
            if (self.LSF_TickHandlers.TryGetValue(entity.GetType(), out var handler))
            {
                return handler.LSF_CheckConsistency(entity, frame, alsfCmd);
            }

            return true;
        }

        public static void HandleLSF_ViewTick(this LSF_TickDispatcherComponent self, Entity entity, long deltaTime)
        {
            if (self.LSF_TickHandlers.TryGetValue(entity.GetType(), out var handler))
            {
                handler.LSF_ViewTick(entity, deltaTime);
            }
        }
#endif

        public static void HandleLSF_TickStart(this LSF_TickDispatcherComponent self, Entity entity, uint frame, long deltaTime)
        {
            if (self.LSF_TickHandlers.TryGetValue(entity.GetType(), out var handler))
            {
                handler.LSF_TickStart(entity, frame, deltaTime);
            }
        }

        public static void HandleLSF_Tick(this LSF_TickDispatcherComponent self, Entity entity, uint currentFrame, long deltaTime)
        {
            if (self.LSF_TickHandlers.TryGetValue(entity.GetType(), out var handler))
            {
                handler.LSF_Tick(entity, currentFrame, deltaTime);
            }
        }
        
        public static void HandleLSF_TickEnd(this LSF_TickDispatcherComponent self, Entity entity, uint frame, long deltaTime)
        {
            if (self.LSF_TickHandlers.TryGetValue(entity.GetType(), out var handler))
            {
                handler.LSF_TickEnd(entity, frame, deltaTime);
            }
        }
    }
}