namespace ET
{
    [NumericWatcher(NumericType.Attack)]
    public class NumericWatcher_AttackValue: INumericWatcher
    {
        public void Run(NumericComponent numericComponent, NumericType numericType, float value)
        {
#if SERVER
            Unit unit = numericComponent.GetParent<Unit>();
#else
            Game.EventSystem.Publish(new EventType.UnitChangeProperty()
                {FinalValue = value, Sprite = numericComponent.GetParent<Unit>(), NumericType = NumericType.MaxHp});
#endif
        }
    }
}