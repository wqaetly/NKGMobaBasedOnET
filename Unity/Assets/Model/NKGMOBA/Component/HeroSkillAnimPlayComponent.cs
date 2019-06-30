//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年6月30日 9:52:29
//------------------------------------------------------------

namespace ETModel
{
    [ObjectSystem]
    public class HeroSkillAnimPlayComponentAwakeSystem: AwakeSystem<HeroSkillAnimPlayComponent>
    {
        public override void Awake(HeroSkillAnimPlayComponent self)
        {
            self.Awake();
        }
    }

    [ObjectSystem]
    public class HeroSkillAnimPlayComponentUpdateSystem: UpdateSystem<HeroSkillAnimPlayComponent>
    {
        public override void Update(HeroSkillAnimPlayComponent self)
        {
            self.Update();
        }
    }

    /// <summary>
    /// 英雄释放技能的动画组件
    /// </summary>
    public class HeroSkillAnimPlayComponent: Component
    {
        private Unit MyHero;
        private UserInputComponent userInputComponent;
        private AnimatorComponent animatorComponent;

        public void Update()
        {
            if (this.userInputComponent.QDown)
            {
                this.animatorComponent.SetBoolValue("ToIdel",false);
                this.animatorComponent.SetBoolValue("ToRun",false);
                this.animatorComponent.SetTrigger("ToSpell1");
            }

            if (this.userInputComponent.WDown)
            {
                this.animatorComponent.SetTrigger("ToSpell2");
                this.animatorComponent.SetBoolValue("ToIdel",false);
                this.animatorComponent.SetBoolValue("ToRun",false);
            }
            
            if (this.userInputComponent.EDown)
            {
                this.animatorComponent.SetTrigger("ToSpell3");
            }
            
            if (this.userInputComponent.RDown)
            {
                this.animatorComponent.SetTrigger("ToSpell4");
            }
        }

        public void Awake()
        {
            this.MyHero = this.GetParent<Unit>();
            this.userInputComponent = Game.Scene.GetComponent<UserInputComponent>();
            this.animatorComponent = this.MyHero.GetComponent<AnimatorComponent>();
        }
    }
}