//此文件格式由工具自动生成

namespace ETModel
{
    #region System

    [ObjectSystem]
    public class StopAllCommandComponentAwakeSystem: AwakeSystem<StopAllCommandComponent>
    {
        public override void Awake(StopAllCommandComponent self)
        {
            self.Awake();
        }
    }

    [ObjectSystem]
    public class StopAllCommandComponentUpdateSystem: UpdateSystem<StopAllCommandComponent>
    {
        public override void Update(StopAllCommandComponent self)
        {
            self.Update();
        }
    }

    [ObjectSystem]
    public class StopAllCommandComponentFixedUpdateSystem: FixedUpdateSystem<StopAllCommandComponent>
    {
        public override void FixedUpdate(StopAllCommandComponent self)
        {
            self.FixedUpdate();
        }
    }

    [ObjectSystem]
    public class StopAllCommandComponentDestroySystem: DestroySystem<StopAllCommandComponent>
    {
        public override void Destroy(StopAllCommandComponent self)
        {
            self.Destroy();
        }
    }

    #endregion

    public class StopAllCommandComponent: Component
    {
        #region 私有成员

        private UserInputComponent m_UserInputComponent;

        #endregion

        #region 公有成员

        #endregion

        #region 生命周期函数

        public void Awake()
        {
            //此处填写Awake逻辑
            m_UserInputComponent = Game.Scene.GetComponent<UserInputComponent>();
        }

        public void Update()
        {
            //此处填写Update逻辑
            if (m_UserInputComponent.SDown)
            {
                Game.Scene.GetComponent<UnitComponent>().MyUnit.GetComponent<UnitPathComponent>().CancelMove();
            }
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
            m_UserInputComponent = null;;
        }

        #endregion
    }
}