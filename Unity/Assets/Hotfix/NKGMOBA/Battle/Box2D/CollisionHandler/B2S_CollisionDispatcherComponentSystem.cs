using System;

namespace ET
{
    public class B2S_CollisionDispatcherComponentAwakeSystem : AwakeSystem<B2S_CollisionDispatcherComponent>
    {
        public override void Awake(B2S_CollisionDispatcherComponent self)
        {
            self.B2SCollisionHandlers.Clear();

            B2S_CollisionDispatcherComponent.Instance = self;
            
            var types = Game.EventSystem.GetTypes(typeof(B2S_CollisionHandlerAttribute));
            foreach (Type type in types)
            {
                AB2S_CollisionHandler bAb2SCollisionHandler = Activator.CreateInstance(type) as AB2S_CollisionHandler;
                if (bAb2SCollisionHandler == null)
                {
                    Log.Error($"robot ai is not AB2S_CollisionHandler: {type.Name}");
                    continue;
                }

                self.B2SCollisionHandlers.Add(type.Name, bAb2SCollisionHandler);
            }
        }
    }
}