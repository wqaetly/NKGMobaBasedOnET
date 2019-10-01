namespace ETModel
{
    //TODO 如果要加新的Buff逻辑类型，需要在这里拓展，本人能力的确有限。。。只能设计成这样了
    /// <summary>
    /// Buff逻辑类型
    /// </summary>
    public enum BuffSystemType
    {
        /// <summary>
        /// 生命移除
        /// </summary>
        ChangeLifeValueSystem,
        /// <summary>
        /// 瞬时伤害
        /// </summary>
        FlashDamageBuffSystem,
        /// <summary>
        /// 持续伤害
        /// </summary>
        SustainDamageBuffSystem,
        /// <summary>
        /// 监听Buff事件
        /// </summary>
        ListenBuffCallBackBuffSystem
    }
}