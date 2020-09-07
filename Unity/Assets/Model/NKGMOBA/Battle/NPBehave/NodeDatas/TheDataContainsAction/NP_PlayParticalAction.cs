//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2020年1月16日 18:23:12
//------------------------------------------------------------

using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ETModel
{
    [Title("播放特效",TitleAlignment = TitleAlignments.Centered)]
    public class NP_PlayParticalAction: NP_ClassForStoreAction
    {
        [LabelText("要播放的特效名称")]
        public string ParticalName;

        /// <summary>
        /// 是否跟随归属的Unit，默认是跟随的，不跟随需要指定一个地点
        /// </summary>
        [LabelText("是否跟随归属的Unit")]
        public bool FollowUnit = true;

        /// <summary>
        /// 特效将要粘贴到的位置
        /// </summary>
        [LabelText("特效将要粘贴到的位置")]
        public PosType PosType;

        [HideInEditorMode]
        public Unit BelongUnit;

        /// <summary>
        /// 目标特效对象
        /// </summary>
        [HideInEditorMode]
        public Unit Partical;

        public override Action GetActionToBeDone()
        {
            BelongUnit = Game.Scene.GetComponent<UnitComponent>().Get(Unitid);
            Game.Scene.GetComponent<GameObjectPool>().Add(ParticalName,
                this.BelongUnit.GameObject.GetComponent<ReferenceCollector>().Get<GameObject>(ParticalName));
            Partical = Game.Scene.GetComponent<GameObjectPool>().FetchEntity(ParticalName);
            if (this.FollowUnit)
            {
                Partical.GameObject.transform.SetParent(this.BelongUnit.GetComponent<HeroTransformComponent>().GetTranform(PosType));
                Partical.GameObject.transform.localPosition = Vector3.zero;
            }

            this.m_Action = this.PlayPartical;
            return this.m_Action;
        }

        private void PlayPartical()
        {
            this.Partical.GameObject.GetComponent<ParticleSystem>().Play();
        }
    }
}