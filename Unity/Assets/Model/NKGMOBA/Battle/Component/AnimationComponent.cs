//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2020年1月12日 16:08:44
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using Animancer;
using ETModel.NKGMOBA.Battle.State;
using UnityEngine;

namespace ETModel
{
    [ObjectSystem]
    public class AnimationComponentAwakeSystem: AwakeSystem<AnimationComponent>
    {
        public override void Awake(AnimationComponent self)
        {
            self.AnimancerComponent = self.Parent.GameObject.GetComponent<AnimancerComponent>();
            self.StackFsmComponent = self.Entity.GetComponent<StackFsmComponent>();
            //如果是以Anim开头的key值，说明是动画文件，需要添加引用
            foreach (var VARIABLE in self.Parent.GameObject.GetComponent<ReferenceCollector>().data)
            {
                if (VARIABLE.key.StartsWith("Anim"))
                {
                    self.AnimationClips.Add(VARIABLE.key, VARIABLE.gameObject as AnimationClip);
                }
            }

            self.StackFsmComponent.FsmLinkedListHasChanaged += self.PlayAnimByStackFsmCurrent;
            self.StackFsmComponent.AddState(StateTypes.Idle, "Idle", 1);
        }
    }

    /// <summary>
    /// 基于Animancer插件做的动画机系统。配合可视化编辑使用效果更佳
    /// 暂时只提供基本的跑，默认外部接口，技能动画的播放应该交给可视化编辑器来做
    /// </summary>
    public class AnimationComponent: Component
    {
        /// <summary>
        /// Animacner的组件
        /// </summary>
        public AnimancerComponent AnimancerComponent;

        /// <summary>
        /// 栈式状态机组件，用于辅助切换动画
        /// </summary>
        public StackFsmComponent StackFsmComponent;

        /// <summary>
        /// 管理所有的动画文件
        /// </summary>
        public Dictionary<string, AnimationClip> AnimationClips = new Dictionary<string, AnimationClip>();

        /// <summary>
        /// 运行时所播放的动画文件，会动态变化
        /// 例如移动速度快到一定程度将会播放另一种跑路动画，这时候就需要动态替换RuntimeAnimationClips的Run所对应的VALUE
        /// KEY:外部调用的名称
        /// VALEU：对应AnimationClips中的KEY，可以取得相应的动画文件
        /// </summary>
        public Dictionary<StateTypes, string> RuntimeAnimationClips = new Dictionary<StateTypes, string>
        {
            { StateTypes.Run, "Anim_Run1" }, { StateTypes.Idle, "Anim_Idle1" },
        };

        /// <summary>
        /// 播放一个动画并且允许设置好下一个要播放的动画，在外部调用.OnEnd即可添加委托
        /// </summary>
        /// <param name="currentStateTypes"></param>
        /// <param name="fadeOutTime"></param>
        /// <returns></returns>
        public AnimancerState PlayAnimAndAllowRegisterNext(StateTypes currentStateTypes, float fadeOutTime)
        {
            return AnimancerComponent.CrossFade(this.AnimationClips[RuntimeAnimationClips[currentStateTypes]], fadeOutTime);
        }

        /// <summary>
        /// 播放一个动画(播放完成自动循环)
        /// </summary>
        /// <param name="stateTypes"></param>
        /// <returns></returns>
        public void PlayAnim(StateTypes stateTypes)
        {
            AnimancerComponent.CrossFade(this.AnimationClips[RuntimeAnimationClips[stateTypes]]);
        }

        /// <summary>
        /// 播放一个动画(播放完成自动回到默认动画)
        /// </summary>
        /// <param name="stateTypes"></param>
        /// <returns></returns>
        public void PlayAnimAndReturnIdel(StateTypes stateTypes)
        {
            AnimancerComponent.CrossFade(this.AnimationClips[RuntimeAnimationClips[stateTypes]]).OnEnd =
                    PlayIdel;
        }

        /// <summary>
        /// 播放跑路动画（非正式版）
        /// </summary>
        public void PlayRun()
        {
            if (this.AnimancerComponent.IsPlayingClip(this.AnimationClips[RuntimeAnimationClips[StateTypes.Idle]]))
                AnimancerComponent.CrossFade(this.AnimationClips[RuntimeAnimationClips[StateTypes.Run]]);
        }

        /// <summary>
        /// 播放默认动画（非正式版）
        /// </summary>
        public void PlayIdel()
        {
            AnimancerComponent.CrossFade(this.AnimationClips[RuntimeAnimationClips[StateTypes.Idle]]);
        }

        /// <summary>
        /// 根据栈式状态机来自动播放动画
        /// </summary>
        public void PlayAnimByStackFsmCurrent()
        {
            //Log.Info($"动画组件收到通知，当前状态{this.StackFsmComponent.GetCurrentFsmState().StateTypes}");
            PlayAnim(this.StackFsmComponent.GetCurrentFsmState().StateTypes);
        }

        public override void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }

            base.Dispose();

            this.AnimancerComponent = null;
            this.AnimationClips.Clear();
            this.AnimationClips = null;
            RuntimeAnimationClips.Clear();
            this.RuntimeAnimationClips = null;
            StackFsmComponent.FsmLinkedListHasChanaged -= PlayAnimByStackFsmCurrent;
            this.StackFsmComponent = null;
        }
    }
}