//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年8月23日 14:58:54
//------------------------------------------------------------

using NPBehave;

namespace ETModel
{
    [ObjectSystem]
    public class NP_RuntimeTreeAwakeSystem: AwakeSystem<NP_RuntimeTree, Root, NP_DataSupportor>
    {
        public override void Awake(NP_RuntimeTree self, Root mRoot, NP_DataSupportor m_BelongNP_DataSupportor)
        {
            self.Awake(mRoot, m_BelongNP_DataSupportor);
        }
    }

    public class NP_RuntimeTree: Entity
    {
        /// <summary>
        /// NP行为树根结点
        /// </summary>
        public Root RootNode;

        /// <summary>
        /// 所归属的数据块
        /// </summary>
        public NP_DataSupportor BelongNP_DataSupportor;

        public void Awake(Root mRoot,NP_DataSupportor m_BelongNP_DataSupportor)
        {
            this.RootNode = mRoot;
            this.BelongNP_DataSupportor = m_BelongNP_DataSupportor;
        }

        /// <summary>
        /// 获取黑板结点
        /// </summary>
        /// <returns></returns>
        public Blackboard GetBlackboard()
        {
            return this.RootNode.Blackboard;
        }

        public override void Dispose()
        {
            if(IsDisposed)
                return;
            base.Dispose();
            this.RootNode.CancelWithoutReturnResult();
            this.RootNode = null;
            this.BelongNP_DataSupportor = null;
        }
    }
}