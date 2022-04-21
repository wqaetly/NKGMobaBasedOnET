namespace ET
{
    public class MapClickCompoent : Entity
    {
        public UserInputComponent m_UserInputComponent;

        public MouseTargetSelectorComponent m_MouseTargetSelectorComponent;

        public override void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }

            base.Dispose();
            m_UserInputComponent = null;
            m_MouseTargetSelectorComponent = null;
        }
    }
}