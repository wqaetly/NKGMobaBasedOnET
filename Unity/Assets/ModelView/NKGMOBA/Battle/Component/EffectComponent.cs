//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2020年1月24日 15:07:27
//------------------------------------------------------------

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

namespace ET
{
    /// <summary>
    /// 特效组件，用于管理Unit身上的特效
    /// </summary>
    public class EffectComponent : Entity
    {
        private Dictionary<string, GameObject> AllEffects = new Dictionary<string, GameObject>();

        /// <summary>
        /// 特效组，用于处理互斥特效，例如诺克的一个血滴，两个血滴，三个血滴这种，里面的数据应由excel表来配置
        /// </summary>
        private List<string> effectGroup = new List<string>
        {
            "Darius/Darius_Passive_Bleed_Effect_1",
            "Darius/Darius_Passive_Bleed_Effect_2",
            "Darius/Darius_Passive_Bleed_Effect_3",
            "Darius/Darius_Passive_Bleed_Effect_4",
            "Darius/Darius_Passive_Bleed_Effect_5"
        };

        /// <summary>
        /// 添加一个特效
        /// </summary>
        /// <param name="name"></param>
        /// <param name="unit"></param>
        public void Add(string name, GameObject unit)
        {
            if (this.AllEffects.ContainsKey(name))
            {
                return;
            }

            this.AllEffects.Add(name, unit);
        }

        /// <summary>
        /// 移除一个特效
        /// </summary>
        /// <param name="name"></param>
        public void Remove(string name)
        {
            if (this.AllEffects.TryGetValue(name, out var tempUnit))
            {
                if (tempUnit.GetComponent<ParticleSystem>() != null)
                {
                    tempUnit.GetComponent<ParticleSystem>().Stop();
                }
                else
                {
                    tempUnit.GetComponent<VisualEffect>().Stop();
                }

                GameObjectPoolComponent.Instance.RecycleGameObject(name, tempUnit);
                AllEffects.Remove(name);
            }
        }

        /// <summary>
        /// 播放一个特效
        /// </summary>
        /// <param name="name"></param>
        public void Play(string name, GameObject unit)
        {
            //处理特效冲突
            HandleConflict(name);

            //播放特效
            if (this.AllEffects.TryGetValue(name, out var tempUnit))
            {
                if (tempUnit.GetComponent<ParticleSystem>() != null)
                {
                    tempUnit.GetComponent<ParticleSystem>().Play();
                }
                else
                {
                    tempUnit.GetComponent<VisualEffect>().Play();
                }
            }
            else
            {
                Add(name, unit);
                if (unit.GetComponent<ParticleSystem>() != null)
                {
                    unit.GetComponent<ParticleSystem>().Play();
                }
                else
                {
                    unit.GetComponent<VisualEffect>().Play();
                }
            }
        }

        /// <summary>
        /// 检查一个特效的状态，如果正在播放就返回True
        /// </summary>
        /// <param name="effectNameToBeChecked"></param>
        /// <returns></returns>
        public bool CheckState(string effectNameToBeChecked)
        {
            if (this.AllEffects.TryGetValue(effectNameToBeChecked, out var unit))
            {
                if (unit.GetComponent<ParticleSystem>() != null)
                {
                    return unit.GetComponent<ParticleSystem>().isPlaying;
                }
                else if (unit.GetComponent<VisualEffect>() != null)
                {
                    return !unit.GetComponent<VisualEffect>().pause;
                }

                return false;
            }

            return false;
        }

        /// <summary>
        /// 处理特效冲突
        /// </summary>
        /// <param name="name"></param>
        public void HandleConflict(string name)
        {
            //如果互斥列表中不包含此项，说明不与其他特效互斥
            if (!effectGroup.Contains(name)) return;
            //查看他是否与特效组里面的一些特效冲突，如果是就移除当前冲突的特效，而播放他
            foreach (var vfxName in this.effectGroup)
            {
                //是同一个特效，就不需要做操作
                if (vfxName == name)
                {
                    continue;
                }

                //如果当前播放的特效中不含VARIABLE，就不需要做操作
                if (!this.AllEffects.ContainsKey(vfxName))
                {
                    continue;
                }

                //如果它并没有在播放，就不需要操作
                if (!CheckState(vfxName))
                {
                    continue;
                }

                //将目标特效移除
                Remove(vfxName);
                //Log.Info($"停止了{VARIABLE1}");
            }
        }
    }
}