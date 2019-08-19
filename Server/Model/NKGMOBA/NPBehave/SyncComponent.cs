//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年8月19日 18:04:17
//------------------------------------------------------------

using ETModel;
using NPBehave;

namespace ETModel
{
    [ObjectSystem]
    public class SyncComponentFixedUpdate: FixedUpdateSystem<SyncComponent>
    {
        public override void FixedUpdate(SyncComponent self)
        {
            self.FixedUpdate();
        }
    }
    
    public class SyncComponent: Component
    {
        public void FixedUpdate()
        {
            SyncContext.Instance.Update();
        }
    }
}