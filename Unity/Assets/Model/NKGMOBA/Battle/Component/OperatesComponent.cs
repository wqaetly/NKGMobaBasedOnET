//此文件格式由工具自动生成

using System.Collections.Generic;

namespace ETModel
{
    #region System

    [ObjectSystem]
    public class OperatedComponentAwakeSystem: AwakeSystem<OperatesComponent>
    {
        public override void Awake(OperatesComponent self)
        {
            self.Awake();
        }
    }

    [ObjectSystem]
    public class OperatedComponentUpdateSystem: UpdateSystem<OperatesComponent>
    {
        public override void Update(OperatesComponent self)
        {
            self.Update();
        }
    }

    [ObjectSystem]
    public class OperatedComponentFixedUpdateSystem: FixedUpdateSystem<OperatesComponent>
    {
        public override void FixedUpdate(OperatesComponent self)
        {
            self.FixedUpdate();
        }
    }

    [ObjectSystem]
    public class OperatedComponentDestroySystem: DestroySystem<OperatesComponent>
    {
        public override void Destroy(OperatesComponent self)
        {
            self.Destroy();
        }
    }

    //TODO 这里利用了EventSystem全局实例化一次事件实例的特性，但是这个事件不应当被直接调用，后续思考有没有更加优雅的方式
    [Event(EventIdType.ExcuteDamage)]
    public class ExcuteDamage_OperatesComponent: AEvent<DamageData>
    {
        public override void Run(DamageData a)
        {
            //伤害发起方记录此次伤害
            a.OperateCaster.GetComponent<OperatesComponent>().AddOperateData(OperatesComponent.OperateType.Damage, a);
        }
    }

    //TODO 这里利用了EventSystem全局实例化一次事件实例的特性，但是这个事件不应当被直接调用，后续思考有没有更加优雅的方式
    [Event(EventIdType.TakeDamage)]
    public class TakeDamage_OperatesComponent: AEvent<DamageData>
    {
        public override void Run(DamageData a)
        {
            //伤害承受方记录此次伤害
            a.OperateTaker.GetComponent<OperatesComponent>().AddOperateData(OperatesComponent.OperateType.BeDamaged, a);
        }
    }

    #endregion

    /// <summary>
    /// 操作组件，例如伤害，被伤害，治疗，被治疗，加速，被加速
    /// 用于游戏中统计数据和特殊需求，例如统计伤害量，护盾量，如果杀死一个单位后会获取一个Buff
    /// </summary>
    public class OperatesComponent: Component
    {
        #region 基础数据定义

        /// <summary>
        /// 操作类型定义
        /// </summary>
        public enum OperateType: byte
        {
            Damage,
            BeDamaged,
            Treatment,
            BeTreatmented,
            Speed,
            BeSpeed,
        }

        #endregion

        #region 私有成员

        private Dictionary<OperateType, Stack<OperateData>> AllOperates = new Dictionary<OperateType, Stack<OperateData>>();

        #endregion

        #region 公有成员

        /// <summary>
        /// 添加操作数据
        /// </summary>
        /// <param name="operateType"></param>
        /// <param name="operateData"></param>
        public void AddOperateData(OperateType operateType, OperateData operateData)
        {
            if (AllOperates.TryGetValue(operateType, out var operateDatasStack))
            {
                operateDatasStack.Push(operateData);
            }
            else
            {
                Stack<OperateData> operateDatas = new Stack<OperateData>();
                operateDatas.Push(operateData);
                AllOperates.Add(operateType, operateDatas);
            }
        }

        #endregion

        #region 生命周期函数

        public void Awake()
        {
            //此处填写Awake逻辑
            Game.Scene.GetComponent<BattleEventSystem>().RegisterEvent($"{EventIdType.ExcuteDamage}{this.Entity.Id}",
                Game.EventSystem.GetEvent(EventIdType.ExcuteDamage));
            Game.Scene.GetComponent<BattleEventSystem>()
                    .RegisterEvent($"{EventIdType.TakeDamage}{this.Entity.Id}", Game.EventSystem.GetEvent(EventIdType.TakeDamage));
        }

        public void Update()
        {
            //此处填写Update逻辑
        }

        public void FixedUpdate()
        {
            //此处填写FixedUpdate逻辑
        }

        public void Destroy()
        {
            //此处填写Destroy逻辑
            foreach (var operateStack in this.AllOperates)
            {
                foreach (var operate in operateStack.Value)
                {
                    ReferencePool.Release(operate);
                }
            }

            this.AllOperates.Clear();
            
            Game.Scene.GetComponent<BattleEventSystem>().UnRegisterEvent($"{EventIdType.ExcuteDamage}{this.Entity.Id}",
                Game.EventSystem.GetEvent(EventIdType.ExcuteDamage));
            Game.Scene.GetComponent<BattleEventSystem>()
                    .UnRegisterEvent($"{EventIdType.TakeDamage}{this.Entity.Id}", Game.EventSystem.GetEvent(EventIdType.TakeDamage));
        }

        public override void Dispose()
        {
            if (IsDisposed)
                return;
            base.Dispose();
            //此处填写释放逻辑,但涉及Entity的操作，请放在Destroy中
        }

        #endregion
    }
}