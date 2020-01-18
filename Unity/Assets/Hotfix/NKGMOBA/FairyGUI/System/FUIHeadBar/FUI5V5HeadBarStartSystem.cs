//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年5月31日 10:55:35
//------------------------------------------------------------

using ETModel;
using FairyGUI;
using UnityEngine;

namespace ETHotfix
{
    [ObjectSystem]
    public class FUIHeadBarStartSystem: StartSystem<FUIHeadBar.HeadBar>
    {
        public override void Start(FUIHeadBar.HeadBar self)
        {
            //self.HPGapList.itemRenderer += (index, item) => { Log.Info("血条更新了"); };
        }
    }
}