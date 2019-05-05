//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年5月1日 18:55:16
//------------------------------------------------------------

using MongoDB.Bson.Serialization.Attributes;

namespace ETModel
{
    [BsonIgnoreExtraElements]
    public class AccountInfo: Entity
    {
        //用户名
        public string Account { get; set; }

        //密码
        public string Password { get; set; }
    }
}