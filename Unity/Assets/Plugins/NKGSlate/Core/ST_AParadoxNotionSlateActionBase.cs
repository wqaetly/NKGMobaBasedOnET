//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2021年8月15日 19:11:02
//------------------------------------------------------------

using NKGSlate.Runtime;
using Sirenix.OdinInspector;
using Slate;
using UnityEngine;

namespace NKGSlate
{
    /// <summary>
    /// 表现在Slate编辑器中的Action基类
    /// </summary>
    public abstract class ST_AParadoxNotionSlateActionBase : ActionClip
    {
        [SerializeField] [HideInInspector] private float m_Originlength = 1;

        public override float length
        {
            get { return m_Originlength; }
            set { m_Originlength = value; }
        }
        
        public override string info
        {
            get
            {
                if (string.IsNullOrEmpty(ActionName))
                {
                    var nameAtt = this.GetType().RTGetAttribute<NameAttribute>(true);
                    if ( nameAtt != null ) {
                        return nameAtt.name;
                    }
                    return this.GetType().Name.SplitCamelCase();
                }

                return ActionName;
            }
        }

        [LabelText("Action名称")]
        public string ActionName;
        
        /// <summary>
        /// 绑定的数据，用于数据预览和导出
        /// </summary>
        [BoxGroup("绑定的数据", CenterLabel = true)] [HideLabel] public ST_DirectableData BindingDate;
    }
}