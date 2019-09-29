//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年9月29日 12:29:40
//------------------------------------------------------------

using ETModel;

namespace EThotfix
{
    [Event(EventIdType.CreateCollider)]
    public class NP_CreateColliderAction:AEvent<long,long,long,long>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a">unitID</param>
        /// <param name="b">运行时的行为树ID</param>
        /// <param name="c">数据结点载体ID</param>
        /// <param name="d">数据ID</param>
        public override void Run(long a, long b, long c, long d)
        {
            
        }
    }
}