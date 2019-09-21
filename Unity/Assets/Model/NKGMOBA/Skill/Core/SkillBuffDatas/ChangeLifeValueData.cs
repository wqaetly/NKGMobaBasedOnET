//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年9月21日 14:45:43
//------------------------------------------------------------

using MongoDB.Bson.Serialization.Attributes;
using Sirenix.OdinInspector;

namespace ETModel
{
    public class ChangeLifeValueData: BuffDataBase
    {
        [LabelText("是否为持续血量改变")]
        [BsonIgnore]
        public bool isSustainCHangeValue = false;

        [ShowIf("isSustainCHangeValue")]
        [LabelText("持续时间")]
        public float SustainTime = 0;

        [ShowIf("isSustainCHangeValue")]
        [LabelText("作用间隔")]
        public float WorkInternal = 0;
    }
}