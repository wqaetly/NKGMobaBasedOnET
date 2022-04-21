namespace ET
{
    [NumericWatcher(NumericType.Speed)]
    public class NumericWatcher_Speed : INumericWatcher
    {
        public void Run(NumericComponent numericComponent, NumericType numericType, float value)
        {
            Unit unit = numericComponent.GetParent<Unit>();
            if (unit.GetComponent<MoveComponent>().ShouldMove)
            {
                unit.GetComponent<MoveComponent>().ChangeSpeed(value / 100f);
            }
#if !SERVER
            Game.EventSystem.Publish(new EventType.UnitChangeProperty()
                {FinalValue = value, Sprite = numericComponent.GetParent<Unit>(), NumericType = numericType});
#else

#endif
        }
    }
}