namespace ET
{
    public static class UnitHelper
    {
        public static UnitInfo CreateUnitInfo(Unit unit)
        {
            UnitInfo unitInfo = new UnitInfo();
            NumericComponent nc = unit.GetComponent<NumericComponent>();
            unitInfo.X = unit.Position.x;
            unitInfo.Y = unit.Position.y;
            unitInfo.Z = unit.Position.z;
            unitInfo.UnitId = unit.Id;
            unitInfo.ConfigId = unit.ConfigId;
            unitInfo.RoleCamp = (int)unit.GetComponent<B2S_RoleCastComponent>().RoleCamp;
            unitInfo.BelongToPlayerId = unit.BelongToPlayer?.Id ?? 0;
            
            return unitInfo;
        }
    }
}