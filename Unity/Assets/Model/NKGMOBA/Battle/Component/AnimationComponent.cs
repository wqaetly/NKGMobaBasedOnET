//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2020年1月12日 16:08:44
//------------------------------------------------------------

using System.Collections.Generic;
using Animancer;
using UnityEngine;

namespace ETModel
{
    [ObjectSystem]
    public class AnimationComponentAwakeSystem: AwakeSystem<AnimationComponent>
    {
        public override void Awake(AnimationComponent self)
        {
            self.AnimancerComponent = self.Parent.GameObject.GetComponent<AnimancerComponent>();
            //如果是以Anim开头的key值，说明是动画文件，需要添加引用
            foreach (var VARIABLE in self.Parent.GameObject.GetComponent<ReferenceCollector>().data)
            {
                if (VARIABLE.key.StartsWith("Anim"))
                {
                    self.animationClips.Add(VARIABLE.key, VARIABLE.gameObject as AnimationClip);
                }
            }

            foreach (var VARIABLE in self.animationClips)
            {
                Log.Info($"已经包含了：{VARIABLE.Key}");
            }
        }
    }

    /// <summary>
    /// 基于Animancer插件做的动画机系统。配合可视化编辑使用效果更佳
    /// </summary>
    public class AnimationComponent: Component
    {
        public AnimancerComponent AnimancerComponent;

        public Dictionary<string, AnimationClip> animationClips = new Dictionary<string, AnimationClip>();

        /// <summary>
        /// 播放一个动画（播放完自动循环）
        /// </summary>
        /// <param name="targetAnimationClipName"></param>
        public void PlayAnim(string targetAnimationClipName)
        {
            AnimancerComponent.CrossFade(animationClips[targetAnimationClipName]);
        }
    }
}