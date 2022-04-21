//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2020年1月24日 14:14:03
//------------------------------------------------------------

using Sirenix.OdinInspector;

namespace ET
{
    /// <summary>
    /// 位置类型
    /// </summary>
    public enum PosType
    {
        /// <summary>
        /// 头顶
        /// </summary>
        [LabelText("头顶")] HEAD,

        /// <summary>
        /// 正中央
        /// </summary>
        [LabelText("正中央")] CENTER,

        /// <summary>
        /// 底部
        /// </summary>
        [LabelText("底部")] GROUND,

        /// <summary>
        /// 正前方
        /// </summary>
        [LabelText("正前方")] FRONT,

        /// <summary>
        /// 左手
        /// </summary>
        [LabelText("左手")] LEFTHAND,

        /// <summary>
        /// 右手
        /// </summary>
        [LabelText("右手")] RIGHTTHAND,

        /// <summary>
        /// 武器前端
        /// </summary>
        [LabelText("武器前端")] WEAPONSTART,

        /// <summary>
        /// 武器中间
        /// </summary>
        [LabelText("武器中间")] WEAPONCENTER,

        /// <summary>
        /// 武器末端
        /// </summary>
        [LabelText("武器末端")] WEAPONEND,
    }

    public class PlayEffectBuffData: BuffDataBase
    {
        [BoxGroup("自定义项")]
        [LabelText("要播放的特效名称")]
        public string EffectName;

        [BoxGroup("自定义项")]
        [LabelText("是否会根据当前Buff层数而改变")]
        public bool CanChangeNameByCurrentOverlay = false;
        
        /// <summary>
        /// 是否跟随归属的Unit，默认是跟随的，不跟随需要指定一个地点
        /// </summary>
        [BoxGroup("自定义项")]
        [LabelText("是否跟随归属的Unit")]
        public bool FollowUnit = true;

        /// <summary>
        /// 特效将要粘贴到的位置
        /// </summary>
        [BoxGroup("自定义项")]
        [LabelText("特效将要粘贴到的位置")]
        public PosType PosType;
    }
}