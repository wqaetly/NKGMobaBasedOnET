namespace ET
{
    // 分发数值监听
    public class NumericChangeEvent_NotifyWatcher : AEvent<EventType.NumericChange>
    {
        protected override async ETTask Run(EventType.NumericChange args)
        {
            NumericWatcherComponent.Instance.Run(args.NumericComponent, args.NumericType, args.Result);
            await ETTask.CompletedTask;
        }
    }
}