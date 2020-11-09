//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2020年1月13日 17:25:39
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using ETModel.NKGMOBA.Battle.Fsm;

namespace ETModel.NKGMOBA.Battle.State
{
    [ObjectSystem]
    public class StackFsmComponentAwakeSystem: AwakeSystem<StackFsmComponent>
    {
        public override void Awake(StackFsmComponent self)
        {
            self.ChangeState<IdleState>(StateTypes.Idle, "Idle", 1);
        }
    }

    
    [ObjectSystem]
    public class StackFsmComponentUpdateSystem: UpdateSystem<StackFsmComponent>
    {
        public override void Update(StackFsmComponent self)
        {
            self.Update();
        }
    }

    /// <summary>
    /// 适用于动画切换的栈式状态机
    /// </summary>
    public class StackFsmComponent: Component
    {
        private LinkedList<AFsmStateBase> m_FsmStateBases = new LinkedList<AFsmStateBase>();

        public AFsmStateBase GetCurrentFsmState()
        {
            return this.m_FsmStateBases.First?.Value;
        }

        /// <summary>
        /// 从状态机移除一个状态，如果移除的是栈顶元素，需要对新的栈顶元素进行OnEnter操作
        /// </summary>
        /// <param name="stateName"></param>
        public void RemoveState(string stateName)
        {
            AFsmStateBase temp = GetState(stateName);
            if (temp == null)
                return;
            temp.OnExit(this);
            
            bool theRemovedItemIsFirstState;
            if (CheckIsFirstState(temp))
            {
                theRemovedItemIsFirstState = true;
            }

            this.m_FsmStateBases.Remove(temp);
            ReferencePool.Release(temp);
            if (theRemovedItemIsFirstState)
            {
                this.GetCurrentFsmState()?.OnEnter(this);
            }
        }

        /// <summary>
        /// 是否存在某个状态_通过状态名称获取
        /// </summary>
        /// <param name="stateName"></param>
        /// <returns></returns>
        public bool HasState(string stateName)
        {
            foreach (var fsmStateBase in this.m_FsmStateBases)
            {
                if (fsmStateBase.StateName == stateName)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 是否存在某个状态_通过状态类型获取
        /// </summary>
        /// <param name="stateTypes"></param>
        /// <returns></returns>
        public bool HasState(StateTypes stateTypes)
        {
            foreach (var fsmStateBase in this.m_FsmStateBases)
            {
                if (fsmStateBase.StateTypes == stateTypes)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 根据状态名称获取状态
        /// </summary>
        /// <param name="stateName"></param>
        /// <returns></returns>
        private AFsmStateBase GetState(string stateName)
        {
            foreach (var aFsmStateBase in this.m_FsmStateBases)
            {
                if (aFsmStateBase.StateName == stateName)
                {
                    return aFsmStateBase;
                }
            }

            return null;
        }

        /// <summary>
        /// 切换状态，如果当前已存在，说明需要把它提到同优先级状态的前面去，让他先执行，切换成功返回成功，切换失败返回失败
        /// 这里的切换成功是指目标状态来到链表头部，插入到链表中或者插入失败都属于切换失败
        /// </summary>
        /// <param name="stateTypes">状态类型</param>
        /// <param name="stateName">状态名称</param>
        /// <param name="priority">状态优先级</param>
        public bool ChangeState<T>(StateTypes stateTypes, string stateName, int priority) where T : AFsmStateBase, new()
        {
            AFsmStateBase aFsmStateBase = this.GetState(stateName);

            if (aFsmStateBase != null)
            {
                this.InsertState(aFsmStateBase, true);
                return CheckIsFirstState(aFsmStateBase);
            }

            aFsmStateBase = ReferencePool.Acquire<T>();
            aFsmStateBase.SetData(stateTypes, stateName, priority);
            this.InsertState(aFsmStateBase);
            return CheckIsFirstState(aFsmStateBase);
        }

        /// <summary>
        /// 向状态机添加一个状态，如果当前已存在，说明需要把它提到同优先级状态的前面去，让他先执行
        /// </summary>
        /// <param name="fsmStateToInsert">目标状态</param>
        /// <param name="containsItSelf">是否包含自身</param>
        private void InsertState(AFsmStateBase fsmStateToInsert, bool containsItSelf = false)
        {
            if (!fsmStateToInsert.TryEnter(this))
            {
                //如果没有目标状态，说明是新增的状态，但是没有成功添加，需要归还给引用池
                if (!containsItSelf)
                {
                    ReferencePool.Release(fsmStateToInsert);
                }

                return;
            }

            LinkedListNode<AFsmStateBase> current = this.m_FsmStateBases.First;
            while (current != null)
            {
                if (fsmStateToInsert.Priority >= current.Value.Priority)
                {
                    break;
                }

                current = current.Next;
            }

            AFsmStateBase tempFirstState = this.GetCurrentFsmState();
            //如果包含自身，就看current是不是自己，如果是，就不对链表做改变，如果不是就提到current前面
            if (containsItSelf)
            {
                if (fsmStateToInsert.StateName == current.Value.StateName)
                {
                    return;
                }
                else
                {
                    m_FsmStateBases.Remove(fsmStateToInsert);
                    m_FsmStateBases.AddBefore(current, fsmStateToInsert);
                }
            }
            else //如果不包含自身，且current不为空，即代表非尾节点有自己的位置，就插入，否则代表所有结点优先级都大于自身，就直接插入链表最后面
            {
                if (current != null)
                {
                    this.m_FsmStateBases.AddBefore(current, fsmStateToInsert);
                }
                else
                {
                    this.m_FsmStateBases.AddLast(fsmStateToInsert);
                }
            }

            //如果这个被插入的状态成为了链表首状态，说明发生了状态变化
            if (CheckIsFirstState(fsmStateToInsert))
            {
                tempFirstState?.OnExit(this);
                fsmStateToInsert.OnEnter(this);
            }
        }

        private bool CheckIsFirstState(AFsmStateBase aFsmStateBase)
        {
            return aFsmStateBase == this.GetCurrentFsmState();
        }

        public void Update()
        {
            GetCurrentFsmState()?.OnUpdate(this);
        }
    }
}