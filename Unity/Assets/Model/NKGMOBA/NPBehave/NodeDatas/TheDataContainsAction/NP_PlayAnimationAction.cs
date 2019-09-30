//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年9月26日 13:36:35
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using Animancer;
using ETModel;
using ETModel.TheDataContainsAction;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Model.NKGMOBA.NPBehave.NodeDatas.TheDataContainsAction
{
    public class NP_PlayAnimationAction: NP_ClassForStoreAction
    {
        [LabelText("要播放的动画列表")]
        [InlineEditor(InlineEditorModes.LargePreview)]
        public List<AnimationClip> _Animations = new List<AnimationClip>();

        [HideInEditorMode]
        public Unit belongtoUnit;

        public override Action GetActionToBeDone()
        {
            this.m_Action = this.PlayAnimation;
            return this.m_Action;
        }

        public void PlayAnimation()
        {
            AnimancerComponent animancerComponent = this.belongtoUnit.GameObject.GetComponent<AnimancerComponent>();
            foreach (var VARIABLE in _Animations)
            {
                animancerComponent.Play(VARIABLE);
            }
        }
    }
}