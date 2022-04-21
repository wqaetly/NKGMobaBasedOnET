namespace ET
{
    public class PingComponent: Entity
    {
        [NoMemoryCheck]
        public C2G_Ping C2G_Ping = new C2G_Ping();

        public C2M_Ping C2M_Ping = new C2M_Ping();

        public long C2GPingValue; //Gate延迟值
        public long C2MPingValue; //Map延迟值
    }
}