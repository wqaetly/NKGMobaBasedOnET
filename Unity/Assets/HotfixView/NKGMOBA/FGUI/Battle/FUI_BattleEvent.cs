using ET.EventType;

namespace ET
{
    [NumericWatcher(NumericType.Hp)]
    public class Map_ChangeHP : INumericWatcher
    {
        public void Run(NumericComponent numericComponent, NumericType numericType, float value)
        {
            Scene scene = numericComponent.DomainScene();
            UnitComponent unitComponent = scene.GetComponent<RoomManagerComponent>().GetBattleRoom()
                .GetComponent<UnitComponent>();
            if (numericComponent.GetParent<Unit>().Id != unitComponent.MyUnit.Id) return;
            FUI_Battle_Main fuiBattleMain = scene.GetComponent<FUIManagerComponent>()
                .GetFUIComponent<FUI_BattleComponent>(FUIPackage.BattleMain).FuiUIPanelBattle;

            fuiBattleMain.m_RedProBar.self.TweenValue(
                unitComponent.MyUnit.GetComponent<UnitAttributesDataComponent>().GetAttribute(NumericType.Hp), 0.2f);
            fuiBattleMain.m_RedText.text =
                $"{fuiBattleMain.m_RedProBar.self.value}/{fuiBattleMain.m_RedProBar.self.max}";
            
            Game.EventSystem.Publish(new EventType.ChangeUnitAttribute()
            {
                Unit = numericComponent.GetParent<Unit>(),
                NumericType = numericType,
                ChangeValue = value
            }).Coroutine();
        }
    }

    [NumericWatcher(NumericType.MaxHp)]
    public class Map_ChangeHPBarMax : INumericWatcher
    {
        public void Run(NumericComponent numericComponent, NumericType numericType, float value)
        {
            Scene scene = numericComponent.DomainScene();
            UnitComponent unitComponent = scene.GetComponent<RoomManagerComponent>().GetBattleRoom()
                .GetComponent<UnitComponent>();
            if (numericComponent.GetParent<Unit>().Id != unitComponent.MyUnit.Id) return;
            FUI_Battle_Main fuiBattleMain = scene.GetComponent<FUIManagerComponent>()
                .GetFUIComponent<FUI_BattleComponent>(FUIPackage.BattleMain).FuiUIPanelBattle;

            fuiBattleMain.m_RedProBar.self.max = value;
            fuiBattleMain.m_RedText.text =
                $"{fuiBattleMain.m_RedProBar.self.value}/{fuiBattleMain.m_RedProBar.self.max}";
        }
    }

    [NumericWatcher(NumericType.MaxMp)]
    public class Map_ChangeMPBar_Max : INumericWatcher
    {
        public void Run(NumericComponent numericComponent, NumericType numericType, float value)
        {
            Scene scene = numericComponent.DomainScene();
            UnitComponent unitComponent = scene.GetComponent<RoomManagerComponent>().GetBattleRoom()
                .GetComponent<UnitComponent>();
            if (numericComponent.GetParent<Unit>().Id != unitComponent.MyUnit.Id) return;
            FUI_Battle_Main fuiBattleMain = scene.GetComponent<FUIManagerComponent>()
                .GetFUIComponent<FUI_BattleComponent>(FUIPackage.BattleMain).FuiUIPanelBattle;

            fuiBattleMain.m_BlueProBar.self.max = value;
            fuiBattleMain.m_BlueText.text =
                $"{fuiBattleMain.m_BlueProBar.self.value}/{fuiBattleMain.m_BlueProBar.self.max}";
        }
    }

    [NumericWatcher(NumericType.Mp)]
    public class Map_ChangeMPValue : INumericWatcher
    {
        public void Run(NumericComponent numericComponent, NumericType numericType, float value)
        {
            Scene scene = numericComponent.DomainScene();
            UnitComponent unitComponent = scene.GetComponent<RoomManagerComponent>().GetBattleRoom()
                .GetComponent<UnitComponent>();
            if (numericComponent.GetParent<Unit>().Id != unitComponent.MyUnit.Id) return;
            FUI_Battle_Main fuiBattleMain = scene.GetComponent<FUIManagerComponent>()
                .GetFUIComponent<FUI_BattleComponent>(FUIPackage.BattleMain).FuiUIPanelBattle;

            fuiBattleMain.m_BlueProBar.self.TweenValue(
                unitComponent.MyUnit.GetComponent<UnitAttributesDataComponent>().GetAttribute(NumericType.Mp), 0.2f);
            fuiBattleMain.m_BlueText.text =
                $"{fuiBattleMain.m_BlueProBar.self.value}/{fuiBattleMain.m_BlueProBar.self.max}";
            
            Game.EventSystem.Publish(new EventType.ChangeUnitAttribute()
            {
                Unit = numericComponent.GetParent<Unit>(),
                NumericType = numericType,
                ChangeValue = value
            }).Coroutine();
        }
    }

    [NumericWatcher(NumericType.Attack)]
    public class Map_ChangeAttack : INumericWatcher
    {
        public void Run(NumericComponent numericComponent, NumericType numericType, float value)
        {
            Scene scene = numericComponent.DomainScene();
            UnitComponent unitComponent = scene.GetComponent<RoomManagerComponent>().GetBattleRoom()
                .GetComponent<UnitComponent>();
            if (numericComponent.GetParent<Unit>().Id != unitComponent.MyUnit.Id) return;
            FUI_Battle_Main fuiBattleMain = scene.GetComponent<FUIManagerComponent>()
                .GetFUIComponent<FUI_BattleComponent>(FUIPackage.BattleMain).FuiUIPanelBattle;

            fuiBattleMain.m_AttackInfo.text = ((int) value).ToString();
        }
    }

    [NumericWatcher(NumericType.AttackAdd)]
    public class Map_ChangeAttackAdd : INumericWatcher
    {
        public void Run(NumericComponent numericComponent, NumericType numericType, float value)
        {
            Scene scene = numericComponent.DomainScene();
            UnitComponent unitComponent = scene.GetComponent<RoomManagerComponent>().GetBattleRoom()
                .GetComponent<UnitComponent>();
            if (numericComponent.GetParent<Unit>().Id != unitComponent.MyUnit.Id) return;
            FUI_Battle_Main fuiBattleMain = scene.GetComponent<FUIManagerComponent>()
                .GetFUIComponent<FUI_BattleComponent>(FUIPackage.BattleMain).FuiUIPanelBattle;

            fuiBattleMain.m_ExtraAttackInfo.text = ((int) value).ToString();
        }
    }
    
    public class C2M_PingInfoRefresh : AEvent<EventType.PingChange>
    {
        protected override async ETTask Run(PingChange a)
        {
            Scene scene = a.ZoneScene;
            FUI_Battle_Main fuiBattleMain = scene.GetComponent<FUIManagerComponent>()
                .GetFUIComponent<FUI_BattleComponent>(FUIPackage.BattleMain).FuiUIPanelBattle;

            fuiBattleMain.m_Text_C2GPinginfo.text = a.C2GPing.ToString();
            fuiBattleMain.m_Text_C2MPinginfo.text = a.C2MPing.ToString();
            
            await ETTask.CompletedTask;
        }
    }
}