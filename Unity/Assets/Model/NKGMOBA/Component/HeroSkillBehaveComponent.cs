//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年6月30日 9:52:29
//------------------------------------------------------------

using System.Collections.Generic;
using UnityEngine;

namespace ETModel
{
    [ObjectSystem]
    public class HeroSkillBehaveomponentAwakeSystem: AwakeSystem<HeroSkillBehaveComponent>
    {
        public override void Awake(HeroSkillBehaveComponent self)
        {
            self.Awake();
        }
    }

    /// <summary>
    /// 英雄技能的表现层组件
    /// </summary>
    public class HeroSkillBehaveComponent: Component
    {
        private Unit MyHero;

        private AnimatorComponent animatorComponent;
        private Transform headPos;
        private Transform channelPos;
        private Transform groundPos;
        private Transform centerPos;

        private Transform Q_Pos;

        private ParticleSystem Q_Par;

        public void Awake()
        {
            this.MyHero = this.GetParent<Unit>();

            this.animatorComponent = this.MyHero.GetComponent<AnimatorComponent>();
            this.headPos = this.MyHero.GameObject.Get<GameObject>("C_BuffBone_Glb_Overhead_Loc").transform;
            this.groundPos = this.MyHero.GameObject.Get<GameObject>("BUFFBONE_GLB_GROUND_LOC").transform;
            this.channelPos = this.MyHero.GameObject.Get<GameObject>("BUFFBONE_GLB_CHANNEL_LOC").transform;
            this.centerPos = this.MyHero.GameObject.Get<GameObject>("C_BUFFBONE_GLB_CENTER_LOC").transform;

            this.Q_Pos = UnityEngine.Object.Instantiate(this.MyHero.GameObject.Get<GameObject>("Darius_Q_Effect")).transform;
            this.Q_Par = this.Q_Pos.gameObject.GetComponent<ParticleSystem>();
        }

        public void OnQSkillPressed()
        {
            if (this.centerPos.Find(this.Q_Pos.name) == false)
            {
                this.Q_Pos.SetParent(this.centerPos);
                this.Q_Pos.localPosition = Vector3.zero;
                this.Q_Par.Play();
            }
            else
            {
                this.Q_Par.Play();
            }

            this.animatorComponent.SetBoolValue("ToIdel", false);
            this.animatorComponent.SetBoolValue("ToRun", false);
            this.animatorComponent.SetTrigger("ToSpell1");
        }

        public void OnWSkillPressed()
        {
            this.animatorComponent.SetTrigger("ToSpell2");
            this.animatorComponent.SetBoolValue("ToIdel", false);
            this.animatorComponent.SetBoolValue("ToRun", false);
        }

        public void OnESkillPressed()
        {
            this.animatorComponent.SetTrigger("ToSpell3");
        }

        public void OnRSkillPressed()
        {
            this.animatorComponent.SetTrigger("ToSpell4");
        }
    }
}