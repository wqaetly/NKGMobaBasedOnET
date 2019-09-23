//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年8月23日 14:58:54
//------------------------------------------------------------

using NPBehave;

namespace ETModel
{
    [ObjectSystem]
    public class NP_RuntimeTreeAwakeSystem: AwakeSystem<NP_RuntimeTree, Root, long>
    {
        public override void Awake(NP_RuntimeTree self, Root mRoot, long theNP_DataSupportIdBelongTo)
        {
            self.Awake(mRoot, theNP_DataSupportIdBelongTo);
        }
    }

    public class NP_RuntimeTree: Entity
    {
        /// <summary>
        /// NP行为树根结点
        /// </summary>
        public Root m_NPRuntimeTreeRootNode;

        /// <summary>
        /// 来自哪个数据块ID
        /// </summary>
        public long theNP_DataSupportIdBelongTo;

        public void Awake(Root mRoot,long theNP_DataSupportIdBelongTo)
        {
            this.m_NPRuntimeTreeRootNode = mRoot;
            this.theNP_DataSupportIdBelongTo = theNP_DataSupportIdBelongTo;
        }

        /// <summary>
        /// 获取黑板结点
        /// </summary>
        /// <returns></returns>
        public Blackboard GetBlackboard()
        {
            return m_NPRuntimeTreeRootNode.Blackboard;
        }
    }
}