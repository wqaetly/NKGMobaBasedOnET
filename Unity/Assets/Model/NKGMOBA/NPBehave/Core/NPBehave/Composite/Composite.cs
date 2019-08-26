#if !UNITY_EDITOR
       using NUnit.Framework;
#else
using UnityEngine.Assertions;

#endif

namespace NPBehave
{
    public abstract class Composite : Container
    {
        protected Node[] Children;

        public Composite(string name, Node[] children) : base(name)
        {
            this.Children = children;
            Assert.IsTrue(children.Length > 0, "Composite nodes (Selector, Sequence, Parallel) need at least one child!");

            foreach (Node node in Children)
            {
                node.SetParent(this);
            }
        }

        override public void SetRoot(Root rootNode)
        {
            base.SetRoot(rootNode);

            foreach (Node node in Children)
            {
                node.SetRoot(rootNode);
            }
        }


        protected override void Stopped(bool success)
        {
            foreach (Node child in Children)
            {
                child.ParentCompositeStopped(this);
            }
            base.Stopped(success);
        }

        public abstract void StopLowerPriorityChildrenForChild(Node child, bool immediateRestart);
    }
}