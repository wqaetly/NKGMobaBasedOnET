namespace ET
{
    public abstract class ALockStepStateFrameSyncMessageHandler<T> : ILockStepStateFrameSyncMessageHandler
        where T : ALSF_Cmd
    {
        public void Handle(Room room, ALSF_Cmd cmd)
        {
            if (room == null)
            {
                Log.Error("请指定战斗Room");
                return;
            }

            if (cmd == null)
            {
                Log.Error($"帧同步消息类型：{typeof(T)}, 内容为空");
                return;
            }

            Unit unit = room.GetComponent<UnitComponent>().Get(cmd.UnitId);

            if (unit == null)
            {
                Log.Error($"未找到Id为{cmd.UnitId}的Unit，消息{cmd}不被处理");
                return;
            }

            this.Run(unit, cmd as T).Coroutine();
        }

        protected abstract ETVoid Run(Unit unit, T cmd);
    }
}