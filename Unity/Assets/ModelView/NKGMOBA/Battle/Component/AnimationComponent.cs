//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2020年1月12日 16:08:44
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using Animancer;
using ET;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ET
{
    public class AnimationComponentAwakeSystem : AwakeSystem<AnimationComponent>
    {
        public override void Awake(AnimationComponent self)
        {
            GameObject gameObject = self.GetParent<Unit>().GetComponent<GameObjectComponent>().GameObject;
            self.AnimancerComponent = gameObject.GetComponent<AnimancerComponent>();
            self.StackFsmComponent = self.GetParent<Unit>().GetComponent<StackFsmComponent>();
            //如果是以Anim开头的key值，说明是动画文件，需要添加引用
            foreach (var referenceCollectorData in gameObject.GetComponent<ReferenceCollector>().data)
            {
                if (referenceCollectorData.key.StartsWith("Anim"))
                {
                    self.AnimationClips.Add(referenceCollectorData.key,
                        referenceCollectorData.gameObject as AnimationClip);
                }

                if (referenceCollectorData.key.StartsWith("AnimMask"))
                {
                    self.AvatarMasks.Add(referenceCollectorData.key, referenceCollectorData.gameObject as AvatarMask);
                }
            }

            self.AnimancerComponent.Layers[(int) PlayAnimInfo.AvatarMaskType.AnimMask_UpNotAffect]
                .SetMask(self.AvatarMasks[PlayAnimInfo.AvatarMaskType.AnimMask_UpNotAffect.ToString()]);
            self.AnimancerComponent.Layers[(int) PlayAnimInfo.AvatarMaskType.AnimMask_DownNotAffect]
                .SetMask(self.AvatarMasks[PlayAnimInfo.AvatarMaskType.AnimMask_DownNotAffect.ToString()]);


            self.PlayIdelFromStart();
        }
    }

    public class AnimationComponentDestroySystem : DestroySystem<AnimationComponent>
    {
        public override void Destroy(AnimationComponent self)
        {
            self.AnimancerComponent = null;
            self.StackFsmComponent = null;
            self.AnimationClips.Clear();
            self.RuntimeAnimationClips.Clear();

            self.Avatar_DownOnlyAnimState = null;
            self.Avatar_UpOnlyAnimState = null;
        }
    }

    /// <summary>
    /// 基于Animancer插件做的动画机系统。配合可视化编辑使用效果更佳
    /// </summary>
    public class AnimationComponent : Entity
    {
        public class SkillAnimInfo
        {
            public AnimancerState SkillAnimancerState;
            public int LayerIndex;
        }

        /// <summary>
        /// Animacner的组件
        /// </summary>
        public AnimancerComponent AnimancerComponent;

        /// <summary>
        /// 栈式状态机组件，用于辅助切换动画
        /// </summary>
        public StackFsmComponent StackFsmComponent;

        public Dictionary<string, AvatarMask> AvatarMasks = new Dictionary<string, AvatarMask>();

        /// <summary>
        /// 管理所有的动画文件
        /// </summary>
        public Dictionary<string, AnimationClip> AnimationClips = new Dictionary<string, AnimationClip>();

        /// <summary>
        /// 全身播放动画
        /// </summary>
        public AnimancerState Avatar_NoneAnimState;

        /// <summary>
        /// 仅仅在下半身播放目标动画
        /// </summary>
        public AnimancerState Avatar_DownOnlyAnimState;

        /// <summary>
        /// 仅仅在上半身播放目标动画
        /// </summary>
        public AnimancerState Avatar_UpOnlyAnimState;

        /// <summary>
        /// 用于记录技能Anim的State
        /// </summary>
        private SkillAnimInfo m_SkillAnimInfo = new SkillAnimInfo();

        /// <summary>
        /// 运行时所播放的动画文件，会动态变化
        /// 例如移动速度快到一定程度将会播放另一种跑路动画，这时候就需要动态替换RuntimeAnimationClips的Run所对应的VALUE
        /// KEY:外部调用的名称
        /// VALEU：对应AnimationClips中的KEY，可以取得相应的动画文件
        /// </summary>
        public Dictionary<string, string> RuntimeAnimationClips = new Dictionary<string, string>
        {
            {StateTypes.Run.GetStateTypeMapedString(), "Anim_Run1"},
            {StateTypes.Idle.GetStateTypeMapedString(), "Anim_Idle1"},
            {StateTypes.CommonAttack.GetStateTypeMapedString(), "Anim_Attack1"}
        };

        /// <summary>
        /// 播放技能的特定接口
        /// </summary>
        /// <param name="stateTypes"></param>
        /// <param name="avatarMaskType"></param>
        /// <param name="fadeDuration"></param>
        /// <param name="speed"></param>
        /// <param name="fadeMode"></param>
        /// <returns></returns>
        public AnimancerState PlaySkillAnim(string stateTypes,
            PlayAnimInfo.AvatarMaskType avatarMaskType = PlayAnimInfo.AvatarMaskType.None,
            float fadeDuration = 0.25f, float speed = 1.0f, FadeMode fadeMode = FadeMode.FixedDuration)
        {
            AnimancerState animancerState = null;

            // 当目前的状态为Run时才会考虑Avatar混合
            if (avatarMaskType == PlayAnimInfo.AvatarMaskType.AnimMask_DownNotAffect &&
                this.StackFsmComponent.GetCurrentFsmState().StateTypes == StateTypes.Run)
            {
                animancerState = AnimancerComponent.Layers[(int) avatarMaskType]
                    .Play(this.AnimationClips[RuntimeAnimationClips[stateTypes]], fadeDuration, fadeMode);

                this.Avatar_UpOnlyAnimState = animancerState;
            }
            else // 否则直接按无AvatarMask播放
            {
                animancerState = PlayCommonAnim_Internal(stateTypes, PlayAnimInfo.AvatarMaskType.None, fadeDuration,
                    speed, fadeMode);

                this.Avatar_NoneAnimState = animancerState;
            }

            m_SkillAnimInfo.LayerIndex = (int) avatarMaskType;
            m_SkillAnimInfo.SkillAnimancerState = animancerState;
            m_SkillAnimInfo.SkillAnimancerState.Events.OnEnd = () =>
            {
                m_SkillAnimInfo.SkillAnimancerState.StartFade(0, 0.1f);
            };
            return animancerState;
        }

        /// <summary>
        /// 播放一个动画(播放完成自动回到默认动画)
        /// </summary>
        /// <param name="stateTypes"></param>
        /// <returns></returns>
        public void PlayAnimAndReturnIdelFromStart(StateTypes stateTypes, float fadeDuration = 0.25f,
            float speed = 1.0f, FadeMode fadeMode = FadeMode.FixedDuration)
        {
            PlayCommonAnim(stateTypes, Avatar_UpOnlyAnimState is {IsPlaying: true}
                ? PlayAnimInfo.AvatarMaskType.AnimMask_UpNotAffect
                : PlayAnimInfo.AvatarMaskType.None, fadeDuration, speed, fadeMode).Events.OnEnd = PlayIdelFromStart;
        }

        /// <summary>
        /// 播放默认动画如果在此期间再次播放，则会从头开始
        /// </summary>
        public void PlayIdelFromStart()
        {
            this.Avatar_DownOnlyAnimState = AnimancerComponent.Play(
                this.AnimationClips[RuntimeAnimationClips[StateTypes.Idle.GetStateTypeMapedString()]], 0.25f,
                FadeMode.FromStart);
        }

        /// <summary>
        /// 播放默认动画如果在此期间再次播放，则会继续播放
        /// </summary>
        public void PlayIdel()
        {
            this.Avatar_DownOnlyAnimState = AnimancerComponent.Play(
                this.AnimationClips[RuntimeAnimationClips[StateTypes.Idle.GetStateTypeMapedString()]], 0.25f);
        }

        /// <summary>
        /// 根据栈式状态机来自动播放动画
        /// 这里播放的动画都默认是常规动画，比如Idle，Run，Attack等，技能动画不在此范围内（因为技能动画不会附加状态）
        /// </summary>
        public void PlayAnimByStackFsmCurrent(float fadeDuration = 0.25f, float speed = 1.0f)
        {
            StateTypes currentStateType = this.StackFsmComponent.GetCurrentFsmState().StateTypes;
            //先根据StateType进行动画播放
            if (this.RuntimeAnimationClips.ContainsKey(currentStateType.ToString()))
            {
                // 如果正在播放技能
                if (m_SkillAnimInfo.SkillAnimancerState is {IsPlaying: true})
                {
                    // 技能的LayerMask如果为只影响上半身，且要播放的为行走动画
                    if (m_SkillAnimInfo.LayerIndex == (int) PlayAnimInfo.AvatarMaskType.AnimMask_DownNotAffect &&
                        currentStateType == StateTypes.Run)
                    {
                        // 如果先释放技能再寻路，且技能尚未播放完成，就会保持上半身不变
                        Avatar_DownOnlyAnimState = PlayCommonAnim(currentStateType,
                            PlayAnimInfo.AvatarMaskType.AnimMask_UpNotAffect,
                            fadeDuration, speed);
                        Avatar_DownOnlyAnimState.Events.OnEnd = () => { Avatar_DownOnlyAnimState.StartFade(0, 0.1f); };
                    }
                }
                else
                {
                    // TODO 这里会有一个问题，如果一个技能动画不使用任何Avatar混合，并且在技能动画播放时想融合播放一个常规动画，那么这种情况下就播放不出动画
                    // 否则就直接在无AvatarMask的Layer播放
                    Avatar_NoneAnimState = PlayCommonAnim(currentStateType, PlayAnimInfo.AvatarMaskType.None,
                        fadeDuration, speed);
                }
            }
        }

        #region PRIVATE

        /// <summary>
        /// 播放一个动画,默认过渡时间为0.25s
        /// </summary>
        /// <param name="stateTypes"></param>
        /// <param name="fadeDuration">动画过渡时间</param>
        /// <returns></returns>
        private AnimancerState PlayCommonAnim_Internal(string stateTypes,
            PlayAnimInfo.AvatarMaskType avatarMaskType = PlayAnimInfo.AvatarMaskType.None,
            float fadeDuration = 0.25f, float speed = 1.0f, FadeMode fadeMode = FadeMode.FixedDuration)
        {
            AnimancerState animancerState = null;
            animancerState = AnimancerComponent.Layers[(int) avatarMaskType]
                .Play(this.AnimationClips[RuntimeAnimationClips[stateTypes]], fadeDuration, fadeMode);
            animancerState.Speed = speed;

            return animancerState;
        }


        /// <summary>
        /// 播放一个动画,默认过渡时间为0.25s，如果在此期间再次播放，则会继续播放
        /// </summary>
        /// <param name="stateTypes"></param>
        /// <param name="fadeDuration">动画过渡时间</param>
        /// <returns></returns>
        private AnimancerState PlayCommonAnim(StateTypes stateTypes,
            PlayAnimInfo.AvatarMaskType avatarMaskType = PlayAnimInfo.AvatarMaskType.None,
            float fadeDuration = 0.25f, float speed = 1.0f, FadeMode fadeMode = FadeMode.FixedDuration)
        {
            return PlayCommonAnim_Internal(stateTypes.GetStateTypeMapedString(), avatarMaskType, fadeDuration, speed,
                fadeMode);
        }

        #endregion
    }
}