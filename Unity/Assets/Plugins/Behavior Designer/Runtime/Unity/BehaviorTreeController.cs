using BehaviorDesigner.Runtime.Tasks;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorDesigner.Runtime
{
    [RequireComponent(typeof(BehaviorTree))]
    public class BehaviorTreeController : MonoBehaviour
    {
        public readonly List<HotfixAction> hotfixActions = new List<HotfixAction>();
        public readonly List<HotfixComposite> hotfixComposites = new List<HotfixComposite>();
        public readonly List<HotfixConditional> hotfixConditionals = new List<HotfixConditional>();
        public readonly List<HotfixDecorator> hotfixDecorators = new List<HotfixDecorator>();

        private bool isInit = false;

        void Awake()
        {
            Init();
        }

        public void SetExternalBehavior(ExternalBehavior externalBehavior)
        {
            var behaviorTree = GetComponent<BehaviorTree>();

            if (behaviorTree)
            {
                behaviorTree.ExternalBehavior = externalBehavior;
                Clear();
                Init();
            }
        }

        public void Clear()
        {
            hotfixActions.Clear();
            hotfixComposites.Clear();
            hotfixConditionals.Clear();
            hotfixDecorators.Clear();
            isInit = false;
        }

        public void Init()
        {
            if (!isInit)
            {
                var behaviorTree = GetComponent<BehaviorTree>();

                if (behaviorTree)
                {
                    var hotfixActionList = behaviorTree.FindTasks<HotfixAction>();

                    if (hotfixActionList != null)
                    {
                        hotfixActions.AddRange(hotfixActionList);
                    }

                    var hotfixCompositeList = behaviorTree.FindTasks<HotfixComposite>();

                    if (hotfixCompositeList != null)
                    {
                        hotfixComposites.AddRange(hotfixCompositeList);
                    }

                    var hotfixConditionalList = behaviorTree.FindTasks<HotfixConditional>();

                    if (hotfixConditionalList != null)
                    {
                        hotfixConditionals.AddRange(hotfixConditionalList);
                    }

                    var hotfixDecoratorList = behaviorTree.FindTasks<HotfixDecorator>();

                    if (hotfixDecoratorList != null)
                    {
                        hotfixDecorators.AddRange(hotfixDecoratorList);
                    }
                }                

                isInit = true;
            }
        }
    }
}

