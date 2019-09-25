//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年9月25日 13:59:03
//------------------------------------------------------------

using Sirenix.OdinInspector;

namespace ETModel
{
    /// <summary>
    /// 与黑板节点相关的数据
    /// </summary>
    public class NP_BlackBoardRelationData
    {
        [LabelText("字典键")]
        public string DicKey;
        
        [LabelText("指定的值类型")]
        public CompareType m_CompareType;

        [ShowIf("m_CompareType", CompareType._String)]
        public string theStringValue;

        [ShowIf("m_CompareType", CompareType._Float)]
        public float theFloatValue;

        [ShowIf("m_CompareType", CompareType._Int)]
        public int theIntValue;

    }
}