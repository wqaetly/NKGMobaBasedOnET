//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年5月1日 18:55:26
//------------------------------------------------------------

using MongoDB.Bson.Serialization.Attributes;

namespace ETModel
{
    /// <summary>
    /// 用户信息
    /// </summary>
    [BsonIgnoreExtraElements]
    public class UserInfo: Entity
    {
        // 昵称
        public string NickName { get; set; }

        // 等级
        public int Level { get; set; }

        // 金币
        public int Goldens { get; set; }

        // 钻石
        public int Diamods { get; set; }

        // 点券
        public int points { get; set; }

        // 1v1胜场
        public int _1v1Wins { get; set; }

        // 1v1负场
        public int _1v1Loses { get; set; }

        // 5v5胜场
        public int _5v5Wins { get; set; }

        // 5v5负场
        public int _5v5Loses { get; set; }
    }
}