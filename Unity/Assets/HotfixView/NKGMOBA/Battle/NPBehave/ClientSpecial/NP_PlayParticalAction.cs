//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2020年1月16日 18:23:12
//------------------------------------------------------------

using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ET
{
    [Title("播放特效", TitleAlignment = TitleAlignments.Centered)]
    public class NP_PlayParticalAction : NP_ClassForStoreAction
    {
        [LabelText("要播放的特效名称(与Res/Effect的相对路径)")] public string ParticalName;

        /// <summary>
        /// 是否跟随归属的Unit，默认是跟随的，不跟随需要指定一个地点
        /// </summary>
        [LabelText("是否跟随归属的Unit")] public bool FollowUnit = true;

        /// <summary>
        /// 特效将要粘贴到的位置
        /// </summary>
        [LabelText("特效将要粘贴到的位置")] public PosType PosType;

#if !SERVER
                /// <summary>
        /// 目标特效对象
        /// </summary>
        [HideInEditorMode] public GameObject Partical;

#endif

        public override Action GetActionToBeDone()
        {
#if !SERVER
            Partical = GameObjectPoolComponent.Instance.FetchGameObject($"{ParticalName}",
                GameObjectType.Effect);
            if (this.FollowUnit)
            {
                Partical.transform.SetParent(this.BelongToUnit.GetComponent<UnitTransformComponent>()
                    .GetTranform(PosType));
                Partical.transform.localPosition = Vector3.zero;
            }
            Partical.SetActive(false);
#endif
            
            this.Action = this.PlayPartical;
            return this.Action;
        }

        private void PlayPartical()
        {
#if !SERVER
            Partical.SetActive(true);
            this.Partical.GetComponent<ParticleSystem>().Play();
#endif
        }
    }
}