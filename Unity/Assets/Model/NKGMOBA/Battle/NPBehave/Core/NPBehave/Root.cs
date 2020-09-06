
using System.Diagnostics;

namespace NPBehave
{
    public class Root : Decorator
    {
        public Node mainNode;

        private System.Action m_MainNodeStartActionCache;

        //private Node inProgressNode;

        public Blackboard blackboard;
        public override Blackboard Blackboard
        {
            get
            {
                return blackboard;
            }
        }


        public Clock clock;
        public override Clock Clock
        {
            get
            {
                return clock;
            }
        }

        public Root(Node mainNode) : base("Root", mainNode)
        {
            this.mainNode = mainNode;
            m_MainNodeStartActionCache = this.mainNode.Start;
            this.clock = SyncContext.Instance.GetClock();
            this.blackboard = new Blackboard(this.clock);
            this.SetRoot(this);
        }
        public Root(Blackboard blackboard, Node mainNode) : base("Root", mainNode)
        {
            this.blackboard = blackboard;
            this.mainNode = mainNode;
            m_MainNodeStartActionCache = this.mainNode.Start;
            this.clock = SyncContext.Instance.GetClock();
            this.SetRoot(this);
        }

        public Root(Blackboard blackboard, Clock clock, Node mainNode) : base("Root", mainNode)
        {
            this.blackboard = blackboard;
            this.mainNode = mainNode;
            m_MainNodeStartActionCache = this.mainNode.Start;
            this.clock = clock;
            this.SetRoot(this);
        }

        public override void SetRoot(Root rootNode)
        {
            Debug.Assert(this == rootNode);
            base.SetRoot(rootNode);
            this.mainNode.SetRoot(rootNode);
        }


        override protected void DoStart()
        {
            this.mainNode.Start();
        }

        override protected void DoCancel()
        {
            if (this.mainNode.IsActive)
            {
                this.mainNode.CancelWithoutReturnResult();
            }
            else
            {
                this.clock.RemoveTimer(this.m_MainNodeStartActionCache);
            }
        }
        
        override protected void DoChildStopped(Node node, bool success)
        {
            if (!IsStopRequested)
            {
                // wait one tick, to prevent endless recursions
                this.clock.AddTimer(0, 0, this.m_MainNodeStartActionCache);
            }
            else
            {
                this.blackboard.Disable();
                Stopped(success);
            }
        }
    }
}
