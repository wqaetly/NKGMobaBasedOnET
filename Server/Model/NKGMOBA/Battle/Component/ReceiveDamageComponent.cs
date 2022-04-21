//此文件格式由工具自动生成


namespace ET
{
    #region System

    [ObjectSystem]
    public class ReceiveDamageComponentAwakeSystem : AwakeSystem<ReceiveDamageComponent>
    {
        public override void Awake(ReceiveDamageComponent self)
        {
            self.DamagePrefix = 1.0f;
        }
    }

    [ObjectSystem]
    public class ReceiveDamageComponentUpdateSystem : UpdateSystem<ReceiveDamageComponent>
    {
        public override void Update(ReceiveDamageComponent self)
        {
            self.Update();
        }
    }

    [ObjectSystem]
    public class ReceiveDamageComponentFixedUpdateSystem : FixedUpdateSystem<ReceiveDamageComponent>
    {
        public override void FixedUpdate(ReceiveDamageComponent self)
        {
            self.FixedUpdate();
        }
    }

    [ObjectSystem]
    public class ReceiveDamageComponentDestroySystem : DestroySystem<ReceiveDamageComponent>
    {
        public override void Destroy(ReceiveDamageComponent self)
        {
            self.DamagePrefix = 1.0f;
        }
    }

    #endregion

    public class ReceiveDamageComponent : Entity
    {
        #region 私有成员

        static string ReceivePhysicalType = $"{BuffDamageTypes.Physical}_Receive";
        static string ReceiveMagicType = $"{BuffDamageTypes.Magic}_Receive";
        static string ReceiveSingleType = $"{BuffDamageTypes.Single}_Receive";
        static string ReceiveRangeType = $"{BuffDamageTypes.Range}_Receive";
        static string ReceiveAllType = $"All_Receive";

        #endregion

        /// <summary>
        /// 伤害修正
        /// </summary>
        public float DamagePrefix = 1.0f;

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