//此文件格式由工具自动生成

namespace ETModel
{
    #region System

    [ObjectSystem]
    public class B2S_RoleCastComponentAwakeSystem: AwakeSystem<B2S_RoleCastComponent, RoleCast>
    {
        public override void Awake(B2S_RoleCastComponent self, RoleCast roleCast)
        {
            self.Awake(roleCast);
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

    /// <summary>
    /// 对象阵容组件，用于标识对象阵营
    /// </summary>
    public class B2S_RoleCastComponent: Component
    {
        #region 私有成员

        #endregion

        #region 公有成员

        /// <summary>
        /// 阵营
        /// </summary>
        public RoleCast RoleCast;

        #endregion

        #region 生命周期函数

        public void Awake(RoleCast roleCast)
        {
            //此处填写Awake逻辑
            this.RoleCast = roleCast;
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