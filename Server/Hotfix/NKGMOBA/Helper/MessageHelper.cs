using ETModel;

namespace ETHotfix
{
    public static class MessageHelper
    {
        public static void Broadcast(IActorMessage message)
        {
            Unit[] units = UnitComponent.Instance.GetAll();
            ActorMessageSenderComponent actorLocationSenderComponent = Game.Scene.GetComponent<ActorMessageSenderComponent>();
            foreach (Unit unit in units)
            {
                UnitGateComponent unitGateComponent = unit.GetComponent<UnitGateComponent>();
                if (unitGateComponent == null || unitGateComponent.IsDisconnect)
                {
                    continue;
                }

                ActorMessageSender actorMessageSender = actorLocationSenderComponent.Get(unitGateComponent.GateSessionActorId);
                actorMessageSender.Send(message);
            }
        }
    }
}