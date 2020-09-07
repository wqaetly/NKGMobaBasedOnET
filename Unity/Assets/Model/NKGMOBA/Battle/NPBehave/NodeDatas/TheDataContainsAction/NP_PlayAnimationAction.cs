//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年9月26日 13:36:35
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using DefaultNamespace;
using ETModel.NKGMOBA.Battle.State;
using Sirenix.OdinInspector;

namespace ETModel
{
    /// <summary>
    /// 这个结点中的动画默认是不可被打断的
    /// </summary>
    [Title("播放动画",TitleAlignment = TitleAlignments.Centered)]
    public class NP_PlayAnimationAction: NP_ClassForStoreAction
    {
        [LabelText("要播放的动画数据")]
        public List<PlayAnimInfo> NodeDataForPlayAnims;

        /// <summary>
        /// 用于标识当前播放到哪一个动画的flag
        /// </summary>
        private int m_Flag = 0;

        /// <summary>
        /// 是否已经被调用过一次
        /// </summary>
        private bool m_HasInvoked = false;
        
        /// <summary>
        /// 避免GC，缓存委托
        /// </summary>
        private Action m_FlagIncrease;

        [HideInEditorMode]
        public Unit TheUnitBelongTo;

        public override Func<bool> GetFunc1ToBeDone()
        {
            this.m_FlagIncrease = () =>
            {
                if (this.m_HasInvoked)
                {
                    return;
                }
                this.m_Flag++;
                Log.Info($"第{this.m_Flag}个动画播放完成");
                this.m_HasInvoked = true;
            };
            this.TheUnitBelongTo = Game.Scene.GetComponent<UnitComponent>().Get(this.Unitid);
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
            this.m_HasInvoked = false;

            if (this.m_Flag >= NodeDataForPlayAnims.Count)
            {
                this.TheUnitBelongTo.GetComponent<StackFsmComponent>().RefreshState();
                Log.Info("栈式状态机已刷新，应该会衔接切换到正确的动画");
                this.m_Flag = 0;
                return true;
            }

            //在播放完成后，每帧都会调用OnEnd委托，由于行为树中的FixedUpdate与Unity的Update频率不一致，所以需要作特殊处理
            this.TheUnitBelongTo.GetComponent<AnimationComponent>()
                            .PlayAnimAndAllowRegisterNext(NodeDataForPlayAnims[this.m_Flag].StateTypes)
                            .OnEnd =
                    this.m_FlagIncrease;
            //Log.Info("这次播放的是Q技能动画");
            return false;
        }
    }
}