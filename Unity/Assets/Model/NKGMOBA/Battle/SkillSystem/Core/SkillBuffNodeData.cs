//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年5月17日 21:03:43
//------------------------------------------------------------

using Sirenix.OdinInspector;

namespace ETModel
{
    [Title("Buff节点数据块",TitleAlignment = TitleAlignments.Centered)]
    [HideLabel]
    public class ForBuffNodeDataBuff: BuffNodeDataBase
    {
        [LabelText("Buff描述")]
        public string BuffDes;
        
        public BuffDataBase BuffData;
    }
}