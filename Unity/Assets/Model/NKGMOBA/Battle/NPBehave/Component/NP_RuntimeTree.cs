//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年8月23日 14:58:54
//------------------------------------------------------------

using NPBehave;

namespace ETModel
{
    [ObjectSystem]
    public class NP_RuntimeTreeAwakeSystem: AwakeSystem<NP_RuntimeTree, NP_DataSupportor, long>
    {
        public override void Awake(NP_RuntimeTree self, NP_DataSupportor m_BelongNP_DataSupportor, long belongToUnitId)
        {
            self.Awake(m_BelongNP_DataSupportor, belongToUnitId);
        }
    }

    public class NP_RuntimeTree: Entity
    {
        /// <summary>
        /// NP行为树根结点
        /// </summary>
        private Root m_RootNode;

        /// <summary>
        /// 所归属的数据块
        /// </summary>
        public NP_DataSupportor BelongNP_DataSupportor;

        /// <summary>
        /// 所归属的Unit的Id
        /// </summary>
        public long BelongToUnitId;

        public void Awake(NP_DataSupportor m_BelongNP_DataSupportor, long belongToUnitId)
        {
            BelongToUnitId = belongToUnitId;
            this.BelongNP_DataSupportor = m_BelongNP_DataSupportor;
        }

        /// <summary>
        /// 设置根结点
        /// </summary>
        /// <param name="rootNode"></param>
        public void SetRootNode(Root rootNode)
        {
            this.m_RootNode = rootNode;
        }

        /// <summary>
        /// 获取黑板
        /// </summary>
        /// <returns></returns>
        public Blackboard GetBlackboard()
        {
            return this.m_RootNode.Blackboard;
        }

        /// <summary>
        /// 开始运行行为树
        /// </summary>
        public void Start()
        {
            this.m_RootNode.Start();
        }

        /// <summary>
        /// 终止行为树
        /// </summary>
        public void Finish()
        {
            this.m_RootNode.CancelWithoutReturnResult();
        }

        public override void Dispose()
        {
            if (IsDisposed)
                return;
            base.Dispose();

            this.Finish();
            BelongToUnitId = 0;
            this.m_RootNode = null;
            this.BelongNP_DataSupportor = null;
        }
    }
}