//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年9月26日 13:36:35
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using Animancer;
using DefaultNamespace;
using ETModel;
using ETModel.NKGMOBA.Battle.State;
using ETModel.TheDataContainsAction;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Model.NKGMOBA.NPBehave.NodeDatas.TheDataContainsAction
{
    /// <summary>
    /// 这个结点中的动画默认是不可被打断的
    /// </summary>
    public class NP_PlayAnimationAction: NP_ClassForStoreAction
    {
        [LabelText("要播放的动画数据")]
        public List<NodeDataForPlayAnim> NodeDataForPlayAnims;

        /// <summary>
        /// 用于标识当前播放到哪一个动画的flag
        /// </summary>
        private int flag = 0;

        /// <summary>
        /// 避免GC
        /// </summary>
        private Action FlagIncrease;

        [HideInEditorMode]
        public Unit belongtoUnit;

        public override Func<bool> GetFunc1ToBeDone()
        {
            this.FlagIncrease = () => this.flag++;
            this.belongtoUnit = Game.Scene.GetComponent<UnitComponent>().Get(this.Unitid);
            //进行数据的装入
            foreach (var VARIABLE in NodeDataForPlayAnims)
            {
                if (this.belongtoUnit.GetComponent<AnimationComponent>().RuntimeAnimationClips.ContainsKey(VARIABLE.StateTypes))
                {
                    this.belongtoUnit.GetComponent<AnimationComponent>().RuntimeAnimationClips[VARIABLE.StateTypes] =
                            VARIABLE.AnimationClipName;
                }
                else
                {
                    this.belongtoUnit.GetComponent<AnimationComponent>().RuntimeAnimationClips
                            .Add(VARIABLE.StateTypes, VARIABLE.AnimationClipName);
                }
            }

            this.m_Func1 = this.PlayAnimation;
            return this.m_Func1;
        }

        private bool PlayAnimation()
        {
            //如果播放完成就默认播放默认动画，这里是可配置的
            if (this.flag >= NodeDataForPlayAnims.Count)
            {
                this.belongtoUnit.GetComponent<StackFsmComponent>().RefreshState();
                Log.Info("栈式状态机已刷新，应该会衔接切换到正确的动画");
                this.flag = 0;
                return true;
            }

            this.belongtoUnit.GetComponent<AnimationComponent>()
                            .PlayAnimAndAllowRegisterNext(NodeDataForPlayAnims[flag].StateTypes, this.NodeDataForPlayAnims[this.flag].FadeOutTime)
                            .OnEnd =
                    this.FlagIncrease;
            //Log.Info("这次播放的是Q技能动画");
            return false;
        }
    }
}