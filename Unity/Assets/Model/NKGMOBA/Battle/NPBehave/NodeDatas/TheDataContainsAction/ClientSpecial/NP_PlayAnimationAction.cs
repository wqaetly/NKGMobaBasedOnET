//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年9月26日 13:36:35
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using Animancer;
using DefaultNamespace;
using ETModel.NKGMOBA.Battle.State;
using Sirenix.OdinInspector;

namespace ETModel
{
    /// <summary>
    /// 这个结点中的动画默认是不可被打断的
    /// </summary>
    [Title("播放动画", TitleAlignment = TitleAlignments.Centered)]
    public class NP_PlayAnimationAction: NP_ClassForStoreAction
    {
        [LabelText("要播放的动画数据")]
        public List<PlayAnimInfo> NodeDataForPlayAnims = new List<PlayAnimInfo>();

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

        [HideInEditorMode]
        public Unit TheUnitBelongTo;

        public override Func<bool> GetFunc1ToBeDone()
        {
            //数据初始化
            this.m_OnAnimFinished = this.OnAnimFinished;
            m_Flag = 0;

            this.TheUnitBelongTo = UnitComponent.Instance.Get(this.Unitid);

            //进行数据的装入
            foreach (var playAnimInfo in NodeDataForPlayAnims)
            {
                this.TheUnitBelongTo.GetComponent<AnimationComponent>().RuntimeAnimationClips[playAnimInfo.StateTypes] =
                        playAnimInfo.AnimationClipName;
            }

            this.Func1 = this.PlayAnimation;
            return this.Func1;
        }

        private bool PlayAnimation()
        {
            if (this.m_Flag > NodeDataForPlayAnims.Count - 1)
            {
                this.TheUnitBelongTo.GetComponent<AnimationComponent>().PlayAnimByStackFsmCurrent();
                //Log.Info("栈式状态机已刷新，应该会衔接切换到正确的动画");
                this.m_Flag = 0;
                return true;
            }

            HandlePlayAnim(NodeDataForPlayAnims[this.m_Flag].StateTypes, NodeDataForPlayAnims[this.m_Flag].FadeOutTime);

            //Log.Info("这次播放的是Q技能动画");
            return false;
        }

        /// <summary>
        /// 处理播放动画
        /// </summary>
        /// <param name="stateTypes">动画对应StateType</param>
        private void HandlePlayAnim(string stateTypes, float fadeDuration = 0.3f)
        {
            //在播放完成后，每帧都会调用OnEnd委托，由于行为树中的FixedUpdate与Unity的Update频率不一致，所以需要作特殊处理
            m_AnimancerState = this.TheUnitBelongTo.GetComponent<AnimationComponent>()
                    .PlayAnim(stateTypes, fadeDuration);

            m_AnimancerState.OnEnd = this.m_OnAnimFinished;
        }

        /// <summary>
        /// 一个动画播放完成后的回调
        /// </summary>
        private void OnAnimFinished()
        {
            if (++m_Flag <= NodeDataForPlayAnims.Count - 1)
            {
                HandlePlayAnim(NodeDataForPlayAnims[m_Flag].StateTypes, NodeDataForPlayAnims[m_Flag].FadeOutTime);
            }
            else //说明所有动画都已经播放完毕
            {
                m_AnimancerState.OnEnd = null;
                m_AnimancerState = null;
            }
        }
    }
}