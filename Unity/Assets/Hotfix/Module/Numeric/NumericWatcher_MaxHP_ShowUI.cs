namespace ET
{
    [NumericWatcher(NumericType.MaxHp)]
    public class NumericWatcher_MaxHP_ShowUI : INumericWatcher
    {
        public void Run(NumericComponent numericComponent, NumericType numericType, float value)
        {
#if !SERVER
            Unit unit = numericComponent.GetParent<Unit>();
            Game.EventSystem.Publish(new EventType.UnitChangeProperty()
                {FinalValue = value, Sprite = numericComponent.GetParent<Unit>(), NumericType = numericType});
#endif
        }
    }
}