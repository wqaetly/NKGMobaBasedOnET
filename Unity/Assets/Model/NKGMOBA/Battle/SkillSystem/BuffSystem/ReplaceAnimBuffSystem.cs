//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2020年12月24日 21:26:46
//------------------------------------------------------------

using System.Collections.Generic;

namespace ETModel
{
    public class ReplaceAnimBuffSystem: ABuffSystemBase
    {
        /// <summary>
        /// 被替换下来的动画信息
        /// </summary>
        private Dictionary<string, string> m_ReplacedAnimData = new Dictionary<string, string>();

        public override void OnInit(BuffDataBase buffData, Unit theUnitFrom, Unit theUnitBelongto)
        {
            //设置Buff来源Unit和归属Unit
            this.TheUnitFrom = theUnitFrom;
            this.TheUnitBelongto = theUnitBelongto;
            this.BuffData = buffData;

            BuffTimerAndOverlayHelper.CalculateTimerAndOverlay(this, this.BuffData);
        }

        public override void OnExecute()
        {
            ReplaceAnimBuffData replaceAnimBuffData = this.BuffData as ReplaceAnimBuffData;
            AnimationComponent animationComponent = this.GetBuffTarget().GetComponent<AnimationComponent>();
            foreach (var animMapInfo in replaceAnimBuffData.AnimReplaceInfo)
            {
                this.m_ReplacedAnimData[animMapInfo.StateType] = animationComponent.RuntimeAnimationClips[animMapInfo.StateType];
                animationComponent.RuntimeAnimationClips[animMapInfo.StateType] = animMapInfo.AnimName;
            }
            animationComponent.PlayAnimByStackFsmCurrent();

            this.BuffState = BuffState.Running;
        }

        public override void OnUpdate()
        {
            //只有不是永久Buff的情况下才会执行Update判断
            if (this.BuffData.SustainTime + 1 > 0)
            {
                if (TimeHelper.Now() > this.MaxLimitTime)
                {
                    this.BuffState = BuffState.Finished;
                }
            }
        }

        public override void OnFinished()
        {
            AnimationComponent animationComponent = this.GetBuffTarget().GetComponent<AnimationComponent>();
            foreach (var animMapInfo in m_ReplacedAnimData)
            {
                animationComponent.RuntimeAnimationClips[animMapInfo.Key] = animMapInfo.Value;
            }

            animationComponent.PlayAnimByStackFsmCurrent();
            m_ReplacedAnimData.Clear();
        }
    }
}