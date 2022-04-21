using ET;
using FairyGUI;

namespace ET
{
    public class FUIInitComponent: Entity
    {
        public async ETTask Init()
        {
            await ETTask.CompletedTask;
        }

        public override void Dispose()
        {
            if (IsDisposed)
            {
                return;
            }

            base.Dispose();
            
        }
    }
}