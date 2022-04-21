namespace ET
{
    /// <summary>
    /// 添加这个组件的Unit的Pos会强行与UnitToSyncPos的位置进行同步
    /// </summary>
    public class SingleSyncPosToUnitComponent: Entity
    {
        public Unit UnitToSyncPos;
    }
}