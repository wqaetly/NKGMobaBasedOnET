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
        [LabelText("要播放的特效名称")]
        public string EffectName;

        [LabelText("是否会根据当前Buff层数而改变")]
        public bool CanChangeNameByCurrentOverlay = false;
        
        /// <summary>
        /// 是否跟随归属的Unit，默认是跟随的，不跟随需要指定一个地点
        /// </summary>
        [LabelText("是否跟随归属的Unit")]
        public bool FollowUnit = true;

        /// <summary>
        /// 特效将要粘贴到的位置
        /// </summary>
        [LabelText("特效将要粘贴到的位置")]
        public PosType PosType;
    }
}