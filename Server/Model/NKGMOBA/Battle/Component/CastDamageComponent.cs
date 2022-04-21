//此文件格式由工具自动生成

using ET;

namespace ET
{
    #region System

    [ObjectSystem]
    public class CastDamageComponentAwakeSystem: AwakeSystem<CastDamageComponent>
    {
        public override void Awake(CastDamageComponent self)
        {
            self.Awake();
        }
    }

    [ObjectSystem]
    public class CastDamageComponentUpdateSystem: UpdateSystem<CastDamageComponent>
    {
        public override void Update(CastDamageComponent self)
        {
            self.Update();
        }
    }

    [ObjectSystem]
    public class CastDamageComponentFixedUpdateSystem: FixedUpdateSystem<CastDamageComponent>
    {
        public override void FixedUpdate(CastDamageComponent self)
        {
            self.FixedUpdate();
        }
    }

    [ObjectSystem]
    public class CastDamageComponentDestroySystem: DestroySystem<CastDamageComponent>
    {
        public override void Destroy(CastDamageComponent self)
        {
            self.Destroy();
        }
    }

    #endregion

    public class CastDamageComponent: Entity
    {
        #region 私有成员

        static string CastPhysicalType = $"{BuffDamageTypes.Physical}_Cast";
        static string CastMagicType = $"{BuffDamageTypes.Magic}_Cast";
        static string CastSingleType = $"{BuffDamageTypes.Single}_Cast";
        static string CastRangeType = $"{BuffDamageTypes.Range}_Cast";
        static string CastAllType = $"All_Cast";
        
        #endregion

        #region 公有成员

        /// <summary>
        /// 洗礼这个伤害值
        /// </summary>
        /// <param name="damageData">伤害数据</param>
        /// <returns></returns>
        public float BaptismDamageData(DamageData damageData)
        {
            DataModifierComponent dataModifierComponent = this.GetParent<Unit>().GetComponent<DataModifierComponent>();

            if ((damageData.BuffDamageTypes & BuffDamageTypes.Physical) == BuffDamageTypes.Physical)
            {
                damageData.DamageValue = dataModifierComponent.BaptismData(CastPhysicalType, damageData.DamageValue);
            }

            if ((damageData.BuffDamageTypes & BuffDamageTypes.Magic) == BuffDamageTypes.Magic)
            {
                damageData.DamageValue = dataModifierComponent.BaptismData(CastMagicType, damageData.DamageValue);
            }

            if ((damageData.BuffDamageTypes & BuffDamageTypes.Single) == BuffDamageTypes.Single)
            {
                damageData.DamageValue = dataModifierComponent.BaptismData(CastSingleType, damageData.DamageValue);
            }

            if ((damageData.BuffDamageTypes & BuffDamageTypes.Range) == BuffDamageTypes.Range)
            {
                damageData.DamageValue = dataModifierComponent.BaptismData(CastRangeType, damageData.DamageValue);
            }

            damageData.OperateCaster = this.GetParent<Unit>();
            damageData.DamageValue = dataModifierComponent.BaptismData(CastAllType, damageData.DamageValue);
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