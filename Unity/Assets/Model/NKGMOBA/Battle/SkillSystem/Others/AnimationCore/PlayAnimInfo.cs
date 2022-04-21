//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2020年1月12日 20:21:51
//------------------------------------------------------------

using Sirenix.OdinInspector;
using UnityEngine;

namespace ET
{
    public class PlayAnimInfo
    {
        /// <summary>
        /// 三种AvatarMask，指越大，优先级越高
        /// </summary>
        public enum AvatarMaskType
        {
            /// <summary>
            /// 默认
            /// </summary>
            [LabelText("默认")] None,

            /// <summary>
            /// 上半身不听使唤
            /// </summary>
            [LabelText("上半身不听使唤")] AnimMask_UpNotAffect,

            /// <summary>
            /// 下半身不听使唤
            /// </summary>
            [LabelText("下半身不听使唤")] AnimMask_DownNotAffect
        }
        
        /// <summary>
        /// 要设置的运行时动画机的类型
        /// </summary>
        [LabelText("要设置的运行时动画机的类型")]
        public string StateTypes;

        /// <summary>
        /// 要播放的动画名称
        /// </summary>
        [LabelText("要播放的动画名称")]
        public string AnimationClipName;

        [LabelText("要占用的AvatarMask")]
        public AvatarMaskType OccAvatarMaskType;

        /// <summary>
        /// 过渡时间
        /// </summary>
        [LabelText("过渡时间")]
        [Tooltip("这个过度时间指从其他动画过渡到本身所用时间")]
        public float FadeOutTime;
    }
}