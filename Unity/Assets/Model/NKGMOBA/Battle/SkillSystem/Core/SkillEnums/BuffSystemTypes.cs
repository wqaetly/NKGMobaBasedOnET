namespace ETModel
{
    //TODO 如果要加新的Buff逻辑类型，需要在这里拓展，本人能力的确有限。。。只能设计成这样了
    /// <summary>
    /// Buff逻辑类型
    /// </summary>
    public enum BuffSystemType
    {
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
        ListenBuffCallBackBuffSystem,
        
        /// <summary>
        /// 绑定一个状态
        /// </summary>
        BindStateBuffSystem,

        /// <summary>
        /// 改变某个属性，硬性改变，不考虑任何额外影响
        /// </summary>
        ChangePlayerPropertyBuffSystem,

        /// <summary>
        /// 治疗
        /// </summary>
        TreatmenBuffSystem,

        /// <summary>
        /// 播放特效
        /// </summary>
        PlayEffectBuffSystem,
        
        /// <summary>
        /// 刷新指定Buff的持续时间
        /// </summary>
        RefreshTargetBuffTimeBuffSystem,
    }
}