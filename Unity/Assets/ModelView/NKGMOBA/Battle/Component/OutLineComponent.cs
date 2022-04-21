//此文件格式由工具自动生成

using UnityEngine;

namespace ET
{
    /// <summary>
    /// 描边组件，用于处理Unit描边
    /// </summary>
    public class OutLineComponent: Entity
    {
        public MouseTargetSelectorComponent MouseTargetSelectorComponent;

        /// <summary>
        /// 用于替换材质属性的Block
        /// </summary>
        public static MaterialPropertyBlock MaterialPropertyBlock = new MaterialPropertyBlock();

        /// <summary>
        /// 缓存的上一帧的目标Unit
        /// </summary>
        public Unit CachedUnit;

        public Unit PlayerUnit;
    }
}