namespace ET
{
    public interface ILockStepStateFrameSyncMessageHandler
    {
        public void Handle(Room room, ALSF_Cmd cmd);
    }
}