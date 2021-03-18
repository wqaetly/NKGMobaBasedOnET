//此文件格式由工具自动生成

namespace ETModel
{
    #region System

    [ObjectSystem]
    public class B2S_RoleCastComponentAwakeSystem: AwakeSystem<B2S_RoleCastComponent, RoleCamp>
    {
        public override void Awake(B2S_RoleCastComponent self, RoleCamp roleCamp)
        {
            self.Awake(roleCamp);
        }
    }

    [ObjectSystem]
    public class B2S_RoleCastComponentUpdateSystem: UpdateSystem<B2S_RoleCastComponent>
    {
        public override void Update(B2S_RoleCastComponent self)
        {
            self.Update();
        }
    }

    [ObjectSystem]
    public class B2S_RoleCastComponentFixedUpdateSystem: FixedUpdateSystem<B2S_RoleCastComponent>
    {
        public override void FixedUpdate(B2S_RoleCastComponent self)
        {
            self.FixedUpdate();
        }
    }

    [ObjectSystem]
    public class B2S_RoleCastComponentDestroySystem: DestroySystem<B2S_RoleCastComponent>
    {
        public override void Destroy(B2S_RoleCastComponent self)
        {
            self.Destroy();
        }
    }

    #endregion

    public enum RoleCast
    {
        /// <summary>
        /// 友善的
        /// </summary>
        Friendly,

        /// <summary>
        /// 敌对的
        /// </summary>
        Adverse,

        /// <summary>
        /// 中立的
        /// </summary>
        Neutral
    }

    [System.Flags]
    public enum RoleCamp
    {
        TianZai = 0b1,
        HuiYue = 0b10,
        JunHeng = 0b100
    }

    /// <summary>
    /// 对象阵容组件，用于标识对象阵营
    /// </summary>
    public class B2S_RoleCastComponent: Component
    {
        #region 私有成员

        #endregion

        #region 公有成员

        /// <summary>
        /// 归属阵营
        /// </summary>
        public RoleCamp RoleCamp;

        /// <summary>
        /// 获取与目标的关系
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        public RoleCast GetRoleCastToTarget(Unit unit)
        {
            if (unit.GetComponent<B2S_RoleCastComponent>() == null)
            {
                return RoleCast.Friendly;
            }

            RoleCamp roleCamp = unit.GetComponent<B2S_RoleCastComponent>().RoleCamp;
            
            if (roleCamp == this.RoleCamp)
            {
                return RoleCast.Friendly;
            }

            switch (roleCamp | this.RoleCamp)
            {
                case RoleCamp.TianZai | RoleCamp.HuiYue:
                    return RoleCast.Adverse;
                case RoleCamp.TianZai | RoleCamp.JunHeng:
                case RoleCamp.HuiYue | RoleCamp.JunHeng:
                    return RoleCast.Neutral;
            }

            return RoleCast.Friendly;
        }

        #endregion

        #region 生命周期函数

        public void Awake(RoleCamp roleCamp)
        {
            //此处填写Awake逻辑
            this.RoleCamp = roleCamp;
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