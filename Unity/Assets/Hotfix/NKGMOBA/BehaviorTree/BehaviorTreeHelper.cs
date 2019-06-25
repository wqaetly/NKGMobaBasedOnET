using BehaviorDesigner.Runtime;
using UnityEngine;

namespace ETHotfix
{
    public class BehaviorTreeHelper
    {
        public static void Init(UnityEngine.Object _object)
        {
            if(_object is GameObject)
            {
                var go = _object as GameObject;

                if (go)
                {
                    var bts = go.GetComponentsInChildren<BehaviorDesigner.Runtime.BehaviorTree>();

                    if (bts != null)
                    {
                        foreach (var bt in bts)
                        {
                            if (bt)
                            {
                                bt.gameObject.Ensure<BehaviorTreeController>().Init();
                            }
                        }
                    }
                }
            }
            else if(_object is ExternalBehavior)
            {
                var externalBehavior = _object as ExternalBehavior;

                if (externalBehavior)
                {
                    externalBehavior.Init();
                }
            }            
        }
    }
}
