//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年9月26日 13:36:35
//------------------------------------------------------------

using System;
using System.Collections.Generic;
#if !SERVER
using Animancer;
#endif
using Sirenix.OdinInspector;

namespace ET
{
    /// <summary>
    /// 这个结点中的动画默认是不可被打断的
    /// </summary>
    [Title("播放动画", TitleAlignment = TitleAlignments.Centered)]
    public class NP_PlayAnimationAction : NP_ClassForStoreAction
    {
        [LabelText("要播放的动画数据")] public List<PlayAnimInfo> NodeDataForPlayAnims = new List<PlayAnimInfo>();

#if !SERVER
        /// <summary>
        /// 用于标识当前播放到哪一个动画的flag
        /// </summary>
        private int m_Flag = 0;

        /// <summary>
        /// 避免GC，缓存委托
        /// </summary>
        private Action m_OnAnimFinished;


        /// <summary>
        /// 当前动画状态
        /// </summary>
        private AnimancerState m_AnimancerState;
#endif

        public override Action GetActionToBeDone()
        {
#if !SERVER
            //数据初始化
            this.m_OnAnimFinished = this.OnAnimFinished;
            m_Flag = 0;

            //进行数据的装入
            foreach (var playAnimInfo in NodeDataForPlayAnims)
            {
                this.BelongToUnit.GetComponent<AnimationComponent>().RuntimeAnimationClips[playAnimInfo.StateTypes] =
                    playAnimInfo.AnimationClipName;
            }

#endif

            this.Action = this.PlayAnimation;
            return base.GetActionToBeDone();
        }

        private void PlayAnimation()
        {
#if !SERVER
            // TODO 说明上次动画节点的动画尚未播放完毕，所以就忽略这次重复的播放，这块逻辑应当放在Timeline中处理
            if (m_Flag != 0)
            {
                return;
            }

            m_Flag = 0;
            HandlePlayAnim(NodeDataForPlayAnims[this.m_Flag].StateTypes,
                NodeDataForPlayAnims[this.m_Flag].OccAvatarMaskType, NodeDataForPlayAnims[this.m_Flag].FadeOutTime);
            //Log.Info("这次播放的是Q技能动画");
#endif
        }

#if !SERVER
        /// <summary>
        /// 处理播放动画
        /// </summary>
        /// <param name="stateTypes">动画对应StateType</param>
        private void HandlePlayAnim(string stateTypes, PlayAnimInfo.AvatarMaskType avatarMaskType,
            float fadeDuration = 0.25f)
        {
            //在播放完成后，每帧都会调用OnEnd委托，由于行为树中的FixedUpdate与Unity的Update频率不一致，所以需要作特殊处理
            m_AnimancerState = this.BelongToUnit.GetComponent<AnimationComponent>()
                .PlaySkillAnim(stateTypes, avatarMaskType, fadeDuration);
            m_AnimancerState.Events.OnEnd = this.m_OnAnimFinished;
        }

        /// <summary>
        /// 一个动画播放完成后的回调
        /// </summary>
        private void OnAnimFinished()
        {
            m_AnimancerState.Stop();
            if (++m_Flag <= NodeDataForPlayAnims.Count - 1)
            {
                HandlePlayAnim(NodeDataForPlayAnims[m_Flag].StateTypes,
                    NodeDataForPlayAnims[this.m_Flag].OccAvatarMaskType, NodeDataForPlayAnims[m_Flag].FadeOutTime);
            }
            else //说明所有动画都已经播放完毕
            {
                this.BelongToUnit.GetComponent<AnimationComponent>().PlayAnimByStackFsmCurrent();

                //Log.Error("--------------------------");
                this.m_Flag = 0;
            }
        }

#endif
    }
}