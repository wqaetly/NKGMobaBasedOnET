using ET.EventType;

namespace ET
{
    /// <summary>
    /// 监视hp数值变化，改变血条值
    /// </summary>
    [NumericWatcher(NumericType.Hp)]
    public class NumericWatcher_Hp_ShowUI : INumericWatcher
    {
        public void Run(NumericComponent numericComponent, NumericType numericType, float value)
        {
#if !SERVER
            Unit unit = numericComponent.GetParent<Unit>();
            Game.EventSystem.Publish(new EventType.UnitChangeProperty()
                {FinalValue = value, Sprite = numericComponent.GetParent<Unit>(), NumericType = NumericType.Hp});
#endif
        }
    }
}