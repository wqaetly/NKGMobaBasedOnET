//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年10月2日 16:41:19
//------------------------------------------------------------

namespace ETModel
{
    /// <summary>
    /// 用以计算Buff最终数据的辅助类
    /// </summary>
    public static class BuffDataCalculateHelper
    {
        public static float CalculateCurrentData<A, B>(A BuffSystemBase, B BuffDataBase) where A : ABuffSystemBase where B : BuffDataBase
        {
            //取得归属Unit的Hero数据
            HeroDataComponent theUnitFromHeroData = BuffSystemBase.TheUnitFrom.GetComponent<HeroDataComponent>();

            float tempData = 0;

            //依据基础数值的加成方式来获取对应伤害数据
            switch (BuffDataBase.BaseBuffBaseDataEffectTypes)
            {
                case BuffBaseDataEffectTypes.FromHeroLevel:
                    tempData = BuffDataBase.ValueToBeChanged[theUnitFromHeroData.CurrentLevel];
                    break;
                case BuffBaseDataEffectTypes.FromSkillLevel:
                    tempData = BuffDataBase.ValueToBeChanged[theUnitFromHeroData.GetSkillLevel(BuffDataBase.BelongSkillId)];
                    break;
                case BuffBaseDataEffectTypes.FromHasLostLifeValue:
                    tempData = BuffSystemBase.TheUnitBelongto.GetComponent<HeroDataComponent>().MaxLifeValue -
                            BuffSystemBase.TheUnitBelongto.GetComponent<HeroDataComponent>().CurrentLifeValue;
                    break;
                case BuffBaseDataEffectTypes.FromCurrentOverlay:
                    tempData = BuffDataBase.ValueToBeChanged[BuffSystemBase.CurrentOverlay];
                    break;
            }

            //依据加成方式对伤害进行加成
            foreach (var additionValue in BuffDataBase.AdditionValue)
            {
                switch (additionValue.Key)
                {
                    case BuffAdditionTypes.Percentage_Physical:
                        tempData += additionValue.Value *
                                theUnitFromHeroData.CurrentAttackValue;
                        break;
                    case BuffAdditionTypes.Percentage_Magic:
                        tempData += additionValue.Value *
                                theUnitFromHeroData.CurrentSpellpower;
                        break;
                    case BuffAdditionTypes.SelfOverlay_Mul:
                        tempData *= additionValue.Value * BuffSystemBase.CurrentOverlay;
                        break;
                    case BuffAdditionTypes.SelfOverlay_Plu:
                        tempData += additionValue.Value * BuffSystemBase.CurrentOverlay;
                        break;
                }
            }

            return tempData;
        }
    }
}