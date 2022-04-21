using System;
using System.Collections.Generic;
using UnityEngine;

namespace ET
{
    public class ETCancellationToken
    {
        private HashSet<Action> actions = new HashSet<Action>();

        private bool cancelFinish = false;
        
        public void Add(Action callback)
        {
            if (cancelFinish)
            {
                Log.Error("这是一个已经用过的CancellationToken: ");
            }
            // 如果action是null，绝对不能添加,要抛异常，说明有协程泄漏
            this.actions.Add(callback);
        }
        
        public void Remove(Action callback)
        {
            this.actions?.Remove(callback);
        }

        public bool IsCancel()
        {
            return cancelFinish;
        }

        public void Cancel()
        {
            if (cancelFinish)
            {
                Log.Error("重复取消协程了");
                return;
            }
            cancelFinish = true;
            this.Invoke();
        }

        private void Invoke()
        {
            HashSet<Action> runActions = this.actions;
            this.actions = null;
            
            try
            {
                foreach (Action action in runActions)
                {
                    action.Invoke();
                }
            }
            catch (Exception e)
            {
#if SERVER
                Log.Error(e);
#else
                UnityEngine.Debug.LogError(e);
#endif
            }
        }
    }
}