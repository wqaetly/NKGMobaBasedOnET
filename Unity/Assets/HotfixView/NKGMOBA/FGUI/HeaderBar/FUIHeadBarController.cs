//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年5月31日 10:51:11
//------------------------------------------------------------

using UnityEngine;

namespace ET
{
    [NumericWatcher(NumericType.MaxHp)]
    public class ChangeHPBar_Density : INumericWatcher
    {
        public void Run(NumericComponent numericComponent, NumericType numericType, float value)
        {
            numericComponent.GetParent<Unit>().GetComponent<HeroHeadBarComponent>().SetDensityOfBar(value);
        }
    }

    [NumericWatcher(NumericType.Hp)]
    public class ChangeHPValue : INumericWatcher
    {
        public void Run(NumericComponent numericComponent, NumericType numericType, float value)
        {
            Unit unit = numericComponent.GetParent<Unit>();

            FUI_HeadBar headBar =
                numericComponent.DomainScene().GetComponent<FUIManagerComponent>()
                    .GetFUIComponent<FUI_HeadBar>($"{unit.Id}_HeadBar");

            headBar.m_Bar_HP.self.TweenValue(
                unit.GetComponent<UnitAttributesDataComponent>().GetAttribute(NumericType.Hp),
                0.2f);
        }
    }

    [NumericWatcher(NumericType.MaxMp)]
    public class ChangeMPBar_Max : INumericWatcher
    {
        public void Run(NumericComponent numericComponent, NumericType numericType, float value)
        {
            Unit unit = numericComponent.GetParent<Unit>();

            FUI_HeadBar headBar =
                numericComponent.DomainScene().GetComponent<FUIManagerComponent>()
                    .GetFUIComponent<FUI_HeadBar>($"{unit.Id}_HeadBar");
            
            headBar.m_Bar_MP.self.max = value;
        }
    }

    [NumericWatcher(NumericType.Mp)]
    public class ChangeMPValue : INumericWatcher
    {
        public void Run(NumericComponent numericComponent, NumericType numericType, float value)
        {
            Unit unit = numericComponent.GetParent<Unit>();

            FUI_HeadBar headBar =
                numericComponent.DomainScene().GetComponent<FUIManagerComponent>()
                    .GetFUIComponent<FUI_HeadBar>($"{unit.Id}_HeadBar");
            
            headBar.m_Bar_MP.self.TweenValue(unit.GetComponent<UnitAttributesDataComponent>().GetAttribute(NumericType.Mp),
                0.2f);

        }
    }
}