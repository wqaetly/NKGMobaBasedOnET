//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2021年8月11日 21:37:13
//------------------------------------------------------------

namespace ET
{
    public interface IBuffSystem
    {
        /// <summary>
        /// 归属的运行时行为树实例
        /// </summary>
        NP_RuntimeTree BelongtoRuntimeTree { get; set; }

        /// <summary>
        /// Buff当前状态
        /// </summary>
        BuffState BuffState { get; set; }

        /// <summary>
        /// 当前叠加数
        /// </summary>
        int CurrentOverlay { get; set; }

        /// <summary>
        /// 持续时间（目标帧）（到达这个帧就会移除）
        /// </summary>
        uint MaxLimitFrame { get; set; }

        /// <summary>
        /// Buff数据
        /// </summary>
        BuffDataBase BuffData { get; set; }

        /// <summary>
        /// Buff节点Id，用于索引具体的BuffNode
        /// </summary>
        long BuffNodeId { get; set; }

        /// <summary>
        /// 来自哪个Unit
        /// </summary>
        Unit TheUnitFrom { get; set; }

        /// <summary>
        /// 寄生于哪个Unit，并不代表当前Buff实际寄居者，需要通过GetBuffTarget来获取，因为它赋值于Buff链起源的地方，具体值取决于那个起源Buff
        /// </summary>
        Unit TheUnitBelongto { get; set; }
        
        void Init(BuffDataBase buffDataBase, Unit theUnitFrom, Unit theUnitBelongto, uint currentFrame);
        
        void Excute(uint currentFrame);

        void Update(uint currentFrame);

        void Finished(uint currentFrame);

        void Refresh(uint currentFrame);
    }
}