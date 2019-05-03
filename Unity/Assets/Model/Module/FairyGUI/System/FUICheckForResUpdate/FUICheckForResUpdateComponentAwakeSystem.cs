//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年4月27日 11:25:50
//------------------------------------------------------------

namespace ETModel
{
    [ObjectSystem]
    public class FUICheckForResUpdateComponentAwakeSystem: AwakeSystem<FUICheckForResUpdateComponent>
    {
        public override void Awake(FUICheckForResUpdateComponent self)
        {
            self.FUICheackForResUpdate =
                    (FUICheckForResUpdate.UI_FUICheckForResUpdate) Game.Scene.GetComponent<FUIComponent>().Get("FUICheckForResUpdate").GObject;
            self.FUICheackForResUpdate.m_processbar.value = 0;
        }
    }
}