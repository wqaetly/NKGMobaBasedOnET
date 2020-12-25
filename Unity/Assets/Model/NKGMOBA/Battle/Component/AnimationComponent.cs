//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2020年1月12日 16:08:44
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using Animancer;
using ETModel.NKGMOBA.Battle.Fsm;
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

            self.PlayAnimByStackFsmCurrent();
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
        public Dictionary<string, string> RuntimeAnimationClips = new Dictionary<string, string>
        {
            { StateTypes.Run.GetStateTypeMapedString(), "Anim_Run1" },
            { StateTypes.Idle.GetStateTypeMapedString(), "Anim_Idle1" },
            { StateTypes.CommonAttack.GetStateTypeMapedString(), "Anim_Attack1" }
        };

        /// <summary>
        /// 播放一个动画,默认过渡时间为0.3s，如果在此期间再次播放，则会继续播放
        /// </summary>
        /// <param name="stateTypes"></param>
        /// <param name="fadeDuration">动画过渡时间</param>
        /// <returns></returns>
        public AnimancerState PlayAnim(StateTypes stateTypes, float fadeDuration = 0.3f, float speed = 1.0f)
        {
            return PlayAnim(stateTypes.GetStateTypeMapedString(), fadeDuration, speed);
        }

        /// <summary>
        /// 播放一个动画,默认过渡时间为0.3s，如果在此期间再次播放，则会继续播放
        /// </summary>
        /// <param name="stateTypes"></param>
        /// <param name="fadeDuration">动画过渡时间</param>
        /// <returns></returns>
        public AnimancerState PlayAnim(string stateTypes, float fadeDuration = 0.3f, float speed = 1.0f)
        {
            AnimancerState animancerState = AnimancerComponent.CrossFade(this.AnimationClips[RuntimeAnimationClips[stateTypes]], fadeDuration);
            animancerState.Speed = speed;
            return animancerState;
        }

        /// <summary>
        /// 播放一个动画,默认过渡时间为0.3s,如果在此期间再次播放，则会从头开始
        /// </summary>
        /// <param name="stateTypes"></param>
        /// <param name="fadeDuration">动画过渡时间</param>
        /// <returns></returns>
        public AnimancerState PlayAnimFromStart(StateTypes stateTypes, float fadeDuration = 0.3f, float speed = 1.0f)
        {
            AnimancerState animancerState =
                    AnimancerComponent.CrossFadeFromStart(this.AnimationClips[RuntimeAnimationClips[stateTypes.GetStateTypeMapedString()]],
                        fadeDuration);
            animancerState.Speed = speed;
            return animancerState;
        }

        /// <summary>
        /// 播放一个动画(播放完成自动回到默认动画),如果在此期间再次播放，则会从头开始
        /// </summary>
        /// <param name="stateTypes"></param>
        /// <returns></returns>
        public void PlayAnimAndReturnIdelFromStart(StateTypes stateTypes, float fadeDuration = 0.3f, float speed = 1.0f)
        {
            PlayAnimFromStart(stateTypes, fadeDuration, speed).OnEnd = PlayIdelFromStart;
        }

        /// <summary>
        /// 播放默认动画（非正式版）,如果在此期间再次播放，则会从头开始
        /// </summary>
        public void PlayIdelFromStart()
        {
            AnimancerComponent.CrossFadeFromStart(this.AnimationClips[RuntimeAnimationClips[StateTypes.Idle.GetStateTypeMapedString()]]);
        }

        /// <summary>
        /// 根据栈式状态机来自动播放动画
        /// </summary>
        public void PlayAnimByStackFsmCurrent(float fadeDuration = 0.3f, float speed = 1.0f)
        {
            //Log.Info($"动画组件收到通知，当前状态{this.StackFsmComponent.GetCurrentFsmState().StateTypes}");
            //先根据StateType进行动画播放
            if (this.RuntimeAnimationClips.ContainsKey(this.StackFsmComponent.GetCurrentFsmState().StateTypes.ToString()))
            {
                PlayAnim(this.StackFsmComponent.GetCurrentFsmState().StateTypes, fadeDuration, speed);
            }
            //如果没有的话就根据StateName进行动画播放
            else if (this.RuntimeAnimationClips.ContainsKey(this.StackFsmComponent.GetCurrentFsmState().StateName.ToString()))
            {
                PlayAnim(this.StackFsmComponent.GetCurrentFsmState().StateName, fadeDuration, speed);
            }
            //否则播放默认动画
            else
            {
                this.PlayIdelFromStart();
            }
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
            this.StackFsmComponent = null;
        }
    }
}