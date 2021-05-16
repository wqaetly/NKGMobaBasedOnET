using Sirenix.OdinInspector;

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
        [LabelText("瞬时伤害Buff")]
        FlashDamageBuffSystem,

        /// <summary>
        /// 持续伤害
        /// </summary>
        [LabelText("持续伤害Buff")]
        SustainDamageBuffSystem,

        /// <summary>
        /// 监听Buff事件
        /// </summary>
        [LabelText("监听事件Buff")]
        ListenBuffCallBackBuffSystem,

        /// <summary>
        /// 绑定一个状态
        /// </summary>
        [LabelText("绑定状态Buff")]
        BindStateBuffSystem,

        /// <summary>
        /// 改变某个属性，硬性改变，不考虑任何额外影响
        /// </summary>
        [LabelText("强制改变属性Buff")]
        ChangePropertyBuffSystem,

        /// <summary>
        /// 治疗
        /// </summary>
        [LabelText("治疗Buff")]
        TreatmentBuffSystem,

        /// <summary>
        /// 播放特效
        /// </summary>
        [LabelText("播放特效Buff")]
        PlayEffectBuffSystem,

        /// <summary>
        /// 刷新指定Buff的持续时间
        /// </summary>
        [LabelText("刷新Buff")]
        RefreshTargetBuffTimeBuffSystem,

        /// <summary>
        /// 往客户端发送Buff信息Buff
        /// </summary>
        [LabelText("同步Buff信息")]
        SendBuffInfoToClientBuffSystem,

        [LabelText("替换攻击流程Buff")]
        ReplaceAttackBuffSystem,

        [LabelText("临时替换动画资源")]
        ReplaceAnimBuffSystem,
        
        [LabelText("修改RenderAsset的内容")]
        ChangeRenderAssetBuffSystem,
        
        [LabelText("修改材质Buff")]
        ChangeMaterialBuffSystem
    }
}