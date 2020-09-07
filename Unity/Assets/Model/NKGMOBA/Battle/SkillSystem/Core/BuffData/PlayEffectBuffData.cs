//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2020年1月24日 14:14:03
//------------------------------------------------------------

using Sirenix.OdinInspector;

namespace ETModel
{
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