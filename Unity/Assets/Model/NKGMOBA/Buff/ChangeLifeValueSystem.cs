//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年9月19日 21:08:36
//------------------------------------------------------------

namespace ETModel
{
    /// <summary>
    /// 用于治疗和生命标记移除，伤害Buff自带改变生命值性质
    /// </summary>
    public class ChangeLifeValueSystem: BuffSystemBase
    {
        public override void OnInit(BuffDataBase BuffDataBase, Unit theUnitFrom, Unit theUnitBelongto)
        {
            //先设置Buff类型
            this.MBuffWorkTypes = BuffDataBase.Base_BuffExtraWork;
            //设置Buff来源Unit和归属Unit
            this.theUnitFrom = theUnitFrom;
            this.theUnitBelongto = theUnitBelongto;
            this.MSkillBuffDataBase = BuffDataBase;
            //强制类型转换为血量改变Buff数据
            ChangeLifeValueData temp = (ChangeLifeValueData) MSkillBuffDataBase;
            //取得归属Unit的Hero数据，用以计算数据
            HeroDataComponent theUnitFromHeroData = this.theUnitFrom.GetComponent<HeroDataComponent>();
        }

        public override void OnExecute()
        {
        }

        public override void OnFinished()
        {
        }
    }
}