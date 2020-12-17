//此文件格式由工具自动生成

using ETModel.NKGMOBA.Battle.State;

namespace ETModel
{
    #region System

    [ObjectSystem]
    public class ReceiveDamageComponentAwakeSystem: AwakeSystem<ReceiveDamageComponent>
    {
        public override void Awake(ReceiveDamageComponent self)
        {
            self.Awake();
        }
    }

    [ObjectSystem]
    public class ReceiveDamageComponentUpdateSystem: UpdateSystem<ReceiveDamageComponent>
    {
        public override void Update(ReceiveDamageComponent self)
        {
            self.Update();
        }
    }

    [ObjectSystem]
    public class ReceiveDamageComponentFixedUpdateSystem: FixedUpdateSystem<ReceiveDamageComponent>
    {
        public override void FixedUpdate(ReceiveDamageComponent self)
        {
            self.FixedUpdate();
        }
    }

    [ObjectSystem]
    public class ReceiveDamageComponentDestroySystem: DestroySystem<ReceiveDamageComponent>
    {
        public override void Destroy(ReceiveDamageComponent self)
        {
            self.Destroy();
        }
    }

    #endregion

    public class ReceiveDamageComponent: Component
    {
        #region 私有成员

        static string ReceivePhysicalType = $"{BuffDamageTypes.Physical}_Receive";
        static string ReceiveMagicType = $"{BuffDamageTypes.Magic}_Receive";
        static string ReceiveSingleType = $"{BuffDamageTypes.Single}_Receive";
        static string ReceiveRangeType = $"{BuffDamageTypes.Range}_Receive";
        static string ReceiveAllType = $"All_Receive";
        
        #endregion

        #region 公有成员

        /// <summary>
        /// 洗礼这个伤害值
        /// </summary>
        /// <param name="damageData">伤害数据</param>
        /// <returns></returns>
        public float BaptismDamageData(DamageData damageData)
        {
            //如果当前无敌则不计算伤害
            if (this.Entity.GetComponent<StackFsmComponent>().ContainsState(StateTypes.Invincible))
            {
                return -1.0f;
            }

            DataModifierComponent dataModifierComponent = this.Entity.GetComponent<DataModifierComponent>();

            if ((damageData.BuffDamageTypes & BuffDamageTypes.Physical) == BuffDamageTypes.Physical)
            {
                damageData.DamageValue = dataModifierComponent.BaptismData(ReceivePhysicalType, damageData.DamageValue);
            }

            if ((damageData.BuffDamageTypes & BuffDamageTypes.Magic) == BuffDamageTypes.Magic)
            {
                damageData.DamageValue = dataModifierComponent.BaptismData(ReceiveMagicType, damageData.DamageValue);
            }

            if ((damageData.BuffDamageTypes & BuffDamageTypes.Single) == BuffDamageTypes.Single)
            {
                damageData.DamageValue = dataModifierComponent.BaptismData(ReceiveSingleType, damageData.DamageValue);
            }

            if ((damageData.BuffDamageTypes & BuffDamageTypes.Range) == BuffDamageTypes.Range)
            {
                damageData.DamageValue = dataModifierComponent.BaptismData(ReceiveRangeType, damageData.DamageValue);
            }

            damageData.DamageValue = dataModifierComponent.BaptismData(ReceiveAllType, damageData.DamageValue);

            return damageData.DamageValue < 0? 0 : damageData.DamageValue;
        }

        #endregion

        #region 生命周期函数

        public void Awake()
        {
            //此处填写Awake逻辑
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