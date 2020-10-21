//此文件格式由工具自动生成

namespace ETModel
{
    public class StopAllCommandComponent: Component
    {
        #region 公有成员

        #endregion

        #region 生命周期函数
        
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