namespace ET
{
    public class UnitSystem1 : AwakeSystem<Unit, Room>
    {
        public override void Awake(Unit self, Room belongToRoom)
        {
            self.BelongToRoom = belongToRoom;
            belongToRoom.GetComponent<UnitComponent>().Add(self);
        }
    }

    public class UnitSystem2 : AwakeSystem<Unit, Room, int>
    {
        public override void Awake(Unit self, Room belongToRoom, int configId)
        {
            self.BelongToRoom = belongToRoom;
            belongToRoom.GetComponent<UnitComponent>().Add(self);
            self.ConfigId = configId;
        }
    }
}