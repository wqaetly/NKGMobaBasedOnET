//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2020年12月4日 17:06:02
//------------------------------------------------------------

using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ETModel
{
    [Title("移动到随机点", TitleAlignment = TitleAlignments.Centered)]
    public class NP_MoveToRandomPosAction: NP_ClassForStoreAction
    {
        [BoxGroup("范围")]
        public int XMin;

        [BoxGroup("范围")]
        public int YMin;

        [BoxGroup("范围")]
        public int XMax;

        [BoxGroup("范围")]
        public int YMax;

        public override Action GetActionToBeDone()
        {
            this.Action = this.MoveToRandomPos;
            return this.Action;
        }

        public void MoveToRandomPos()
        {
#if SERVER 
            Vector3 randomTarget = new Vector3(RandomHelper.RandomNumber(this.XMin, this.XMax), 0, RandomHelper.RandomNumber(this.YMin, this.YMax));
            UnitComponent.Instance.Get(this.Unitid).GetComponent<UnitPathComponent>().MoveTo(randomTarget).Coroutine();  
#endif
        }
    }
}