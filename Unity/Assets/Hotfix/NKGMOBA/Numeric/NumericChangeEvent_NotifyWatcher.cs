using ETModel;

namespace ETHotfix
{
	// 分发数值监听
	[Event(ETModel.EventIdType.NumbericChange)]
	public class NumericChangeEvent_NotifyWatcher: AEvent<long, NumericType, float>
	{
		public override void Run(long id, NumericType numericType, float value)
		{
			Game.Scene.GetComponent<NumericWatcherComponent>().Run(numericType, id, value);
		}
	}
}
