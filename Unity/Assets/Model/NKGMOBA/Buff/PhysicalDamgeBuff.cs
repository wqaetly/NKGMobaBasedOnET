//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年9月16日 21:12:21
//------------------------------------------------------------

namespace ETModel
{
    public class PhysicalDamageBuff: BuffBase
    {
        /// <summary>
        /// 最终伤害值
        /// </summary>
        public float finalDamageValue;
        
        public override void OnInit(NodeDataForSkillBuff nodeDataForSkillBuff, Unit unit)
        {
            this.MBuffTypes = nodeDataForSkillBuff.SkillBuffBases.Base_BuffTypes;
            finalDamageValue = unit.GetComponent<HeroDataComponent>().CurrentLevel;
            this.MBuffState = BuffState.Waiting;
        }

        public override void OnExecute()
        {
        }

        public override void OnFinished()
        {
        }
    }
}