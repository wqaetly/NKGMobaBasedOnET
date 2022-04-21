//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年10月2日 16:41:19
//------------------------------------------------------------

namespace ET
{
    /// <summary>
    /// 用以计算Buff最终数据的辅助类
    /// </summary>
    public static class BuffDataCalculateHelper
    {
        public static float CalculateCurrentData(IBuffSystem buffSystem)
        {
            //取得来源Unit的Hero数据
            UnitAttributesDataComponent theUnitFromUnitAttributesData = buffSystem.TheUnitFrom.GetComponent<UnitAttributesDataComponent>();

            float tempData = 0;

            BuffDataBase buffData = buffSystem.BuffData;
            
            //依据基础数值的加成方式来获取对应数据
            switch (buffData.BaseBuffBaseDataEffectTypes)
            {
                case BuffBaseDataEffectTypes.FromHeroLevel:
                    tempData = buffData.ValueToBeChanged[(int) theUnitFromUnitAttributesData.GetAttribute(NumericType.Level)];
                    break;
                case BuffBaseDataEffectTypes.FromSkillLevel:
                    tempData = buffData.ValueToBeChanged[buffSystem.TheUnitFrom.GetComponent<SkillCanvasManagerComponent>()
                            .GetSkillLevel(buffData.BelongToSkillId.Value)];
                    break;
                case BuffBaseDataEffectTypes.FromHasLostLifeValue:
                    tempData = buffSystem.TheUnitBelongto.GetComponent<UnitAttributesDataComponent>().GetAttribute(NumericType.MaxHp) -
                            buffSystem.TheUnitBelongto.GetComponent<UnitAttributesDataComponent>().GetAttribute(NumericType.Hp);
                    break;
                case BuffBaseDataEffectTypes.FromCurrentOverlay:
                    tempData = buffData.ValueToBeChanged[buffSystem.CurrentOverlay];
                    break;
            }

            //依据加成方式对伤害进行加成
            foreach (var additionValue in buffData.AdditionValue)
            {
                switch (additionValue.Key)
                {
                    case BuffAdditionTypes.Percentage_Physical:
                        tempData += additionValue.Value *
                                theUnitFromUnitAttributesData.GetAttribute(NumericType.Attack);
                        break;
                    case BuffAdditionTypes.Percentage_Magic:
                        tempData += additionValue.Value *
                                theUnitFromUnitAttributesData.GetAttribute(NumericType.MagicStrength);
                        break;
                    case BuffAdditionTypes.SelfOverlay_Mul:
                        tempData *= additionValue.Value * buffSystem.CurrentOverlay;
                        break;
                    case BuffAdditionTypes.SelfOverlay_Plu:
                        tempData += additionValue.Value * buffSystem.CurrentOverlay;
                        break;
                }
            }

            return tempData;
        }
    }
}