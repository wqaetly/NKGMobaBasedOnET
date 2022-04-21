//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2021年8月17日 22:31:13
//------------------------------------------------------------

using NKGSlate.Runtime;
using Slate;
using UnityEngine;

namespace NKGSlate.Sample
{
    [Attachable(typeof(ST_ParadoxNotionTrack))]
    [Description("抛出一个事件")]
    [Name("抛出事件")]
    public class ST_ParadoxNotionEvent: ST_AParadoxNotionSlateActionBase
    {
        public override float length
        {
            get { return 0; }
        }

        protected override void OnCreate()
        {
            base.OnCreate();
            BindingDate = new ST_EventData();
        } 
    }
}