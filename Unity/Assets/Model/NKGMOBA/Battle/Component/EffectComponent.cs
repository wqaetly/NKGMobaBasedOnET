//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2020年1月24日 15:07:27
//------------------------------------------------------------

using System.Collections.Generic;
using UnityEngine;

namespace ETModel
{
    /// <summary>
    /// 特效组件，用于管理Unit身上的特效
    /// </summary>
    public class EffectComponent: Component
    {
        private Dictionary<string, Unit> AllEffects = new Dictionary<string, Unit>();

        /// <summary>
        /// 特效组，用于处理互斥特效，例如诺克的一个血滴，两个血滴，三个血滴这种，里面的数据应由excel表来配置
        /// </summary>
        private List<List<string>> effectGroup = new List<List<string>>
        {
            new List<string>()
            {
                "Darius_Passive_Bleed_Effect_1",
                "Darius_Passive_Bleed_Effect_2",
                "Darius_Passive_Bleed_Effect_3",
                "Darius_Passive_Bleed_Effect_4",
                "Darius_Passive_Bleed_Effect_5"
            }
        };

        /// <summary>
        /// 添加一个特效
        /// </summary>
        /// <param name="name"></param>
        /// <param name="unit"></param>
        public void Add(string name, Unit unit)
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
                AllEffects.Remove(name);
                Game.Scene.GetComponent<GameObjectPool<Unit>>().Recycle(tempUnit);
            }
        }

        /// <summary>
        /// 播放一个特效
        /// </summary>
        /// <param name="name"></param>
        public void Play(string name, Unit unit)
        {
            //查看他是否与特效组里面的一些特效冲突，如果是就停止当前冲突的特效，而播放他
            foreach (var VARIABLE in this.effectGroup)
            {
                if (VARIABLE.Contains(name))
                {
                    foreach (var VARIABLE1 in VARIABLE)
                    {
                        if (VARIABLE1 != name)
                        {
                            if (AllEffects.ContainsKey(VARIABLE1))
                            {
                                if (this.AllEffects[VARIABLE1].GameObject.GetComponent<ParticleSystem>().isPlaying)
                                {
                                    this.AllEffects[VARIABLE1].GameObject.SetActive(false);
                                    //Log.Info($"停止了{VARIABLE1}");
                                    break;
                                }
                            }
                        }
                    }
                }
            }

            if (this.AllEffects.TryGetValue(name, out var tempUnit))
            {
                tempUnit.GameObject.GetComponent<ParticleSystem>().Play();
            }
            else
            {
                Add(name, unit);
                unit.GameObject.GetComponent<ParticleSystem>().Play();
            }
        }
    }
}