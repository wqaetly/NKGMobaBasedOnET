//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年8月23日 14:58:54
//------------------------------------------------------------

using NPBehave;

namespace ETModel
{
    [ObjectSystem]
    public class NP_RuntimeTreeAwakeSystem: AwakeSystem<NP_RuntimeTree, Root>
    {
        public override void Awake(NP_RuntimeTree self,Root mRoot)
        {
            self.Awake(mRoot);
        }
    }

    public class NP_RuntimeTree: Entity
    {
        /// <summary>
        /// NP行为树根结点
        /// </summary>
        public Root m_NPRuntimeTreeRootNode;

        public void Awake(Root mRoot)
        {
            this.m_NPRuntimeTreeRootNode = mRoot;
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