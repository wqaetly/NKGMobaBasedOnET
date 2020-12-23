//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2020年1月12日 20:21:51
//------------------------------------------------------------

using ETModel;
using ETModel.NKGMOBA.Battle.State;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DefaultNamespace
{
    public class PlayAnimInfo
    {
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

        /// <summary>
        /// 过渡时间
        /// </summary>
        [LabelText("过渡时间")]
        [Tooltip("这个过度时间指从其他动画过渡到本身所用时间")]
        public float FadeOutTime;
    }
}