//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2020年1月13日 17:25:39
//------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace ETModel.NKGMOBA.Battle.State
{
    /// <summary>
    /// 适用于动画切换的栈式状态机
    /// </summary>
    public class StackFsmComponent: Component
    {
        private LinkedList<FsmStateBase> m_FsmStateBases = new LinkedList<FsmStateBase>();

        public Action FsmLinkedListHasChanaged ;

        public FsmStateBase GetCurrentFsmState()
        {
            return this.m_FsmStateBases.First.Value;
        }

        /// <summary>
        /// 从状态机移除一个状态
        /// </summary>
        /// <param name="stateName"></param>
        public void RemoveState(string stateName)
        {
            FsmStateBase temp = GetState(stateName);
            if (temp == null)
                return;
            this.m_FsmStateBases.Remove(temp);
            FsmLinkedListHasChanaged?.Invoke();
        }

        /// <summary>
        /// 是否存在某个状态
        /// </summary>
        /// <param name="stateName"></param>
        /// <returns></returns>
        private bool HasState(string stateName)
        {
            foreach (var VARIABLE in this.m_FsmStateBases)
            {
                if (VARIABLE.StateName == stateName)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 是否存在某个状态
        /// </summary>
        /// <param name="stateName"></param>
        /// <returns></returns>
        private FsmStateBase GetState(string stateName)
        {
            foreach (var VARIABLE in this.m_FsmStateBases)
            {
                if (VARIABLE.StateName == stateName)
                {
                    return VARIABLE;
                }
            }

            //Log.Error($"所请求的状态:{stateName}不存在，将会自动创建一个");
            return null;
        }

        /// <summary>
        /// 向状态机添加一个状态，如果当前已存在，说明需要把它提到同优先级状态的前面去，让他先执行
        /// </summary>
        /// <param name="stateTypes">状态类型</param>
        /// <param name="stateName">状态名称</param>
        /// <param name="priority">状态优先级</param>
        public void AddState(StateTypes stateTypes, string stateName, int priority)
        {
            //Log.Info($"意图加入一个{stateTypes}类型的状态");
            FsmStateBase fsmStateBase = this.GetState(stateName);

            if (fsmStateBase != null)
            {
                InsertState(fsmStateBase);
                return;
            }

            fsmStateBase = ComponentFactory.Create<FsmStateBase, StateTypes, string, int>(stateTypes, stateName, priority);

            InsertState(fsmStateBase);
        }

        /// <summary>
        /// 插入State到链表中，如果当前已存在，说明需要把它提到同优先级状态的前面去，让他先执行
        /// </summary>
        /// <param name="fsmStateBase"></param>
        private void InsertState(FsmStateBase fsmStateBase)
        {
            LinkedListNode<FsmStateBase> current = this.m_FsmStateBases.First;
            while (current != null)
            {
                if (fsmStateBase.Priority >= current.Value.Priority)
                {
                    break;
                }

                current = current.Next;
            }

            if (current != null)
            {
                this.m_FsmStateBases.AddBefore(current, fsmStateBase);
            }
            else
            {
                this.m_FsmStateBases.AddLast(fsmStateBase);
            }

            FsmLinkedListHasChanaged?.Invoke();
        }

        /// <summary>
        /// 刷新状态，其实就是抛出一下链表改变事件，让动画回到该有的状态
        /// </summary>
        public void RefreshState()
        {
            FsmLinkedListHasChanaged?.Invoke();
        }
    }
}