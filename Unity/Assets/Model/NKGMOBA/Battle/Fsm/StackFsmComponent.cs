//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2020年1月13日 17:25:39
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using ET;

namespace ET
{
    [ObjectSystem]
    public class StackFsmComponentAwakeSystem: AwakeSystem<StackFsmComponent>
    {
        public override void Awake(StackFsmComponent self)
        {
            self.ChangeState<IdleState>(StateTypes.Idle, "Idle", 1);
        }
    }

    /// <summary>
    /// 适用于动画切换的栈式状态机
    /// </summary>
    public class StackFsmComponent: Entity
    {
        /// <summary>
        /// 当前持有的状态，用于外部获取对比，减少遍历次数
        /// Key为状态类型，V为具体状态类
        /// </summary>
        private Dictionary<StateTypes, List<AFsmStateBase>> m_States = new Dictionary<StateTypes, List<AFsmStateBase>>();

        /// <summary>
        /// 用于内部轮询，切换的状态类
        /// </summary>
        private LinkedList<AFsmStateBase> m_FsmStateBases = new LinkedList<AFsmStateBase>();

        /// <summary>
        /// 获取栈顶状态
        /// </summary>
        /// <returns></returns>
        public AFsmStateBase GetCurrentFsmState()
        {
            return this.m_FsmStateBases.First?.Value;
        }

        /// <summary>
        /// 检查是否为栈顶状态
        /// </summary>
        /// <param name="aFsmStateBase"></param>
        /// <returns></returns>
        private bool CheckIsFirstState(AFsmStateBase aFsmStateBase)
        {
            return aFsmStateBase == this.GetCurrentFsmState();
        }

        #region 移除状态

        /// <summary>
        /// 从状态机移除一个状态（指定名称），如果移除的是栈顶元素，需要对新的栈顶元素进行OnEnter操作
        /// </summary>
        /// <param name="stateName"></param>
        public void RemoveState(string stateName)
        {
            AFsmStateBase temp = GetState(stateName);
            if (temp == null)
                return;

            bool theRemovedItemIsFirstState = this.CheckIsFirstState(temp);
            this.m_States[temp.StateTypes].Remove(temp);
            this.m_FsmStateBases.Remove(temp);
            temp.OnRemoved(this);
            ReferencePool.Release(temp);
            if (theRemovedItemIsFirstState)
            {
                this.GetCurrentFsmState()?.OnEnter(this);
            }
        }

        /// <summary>
        /// 从状态机移除一类状态（指定状态类型），如果移除的是栈顶元素，需要对新的栈顶元素进行OnEnter操作
        /// </summary>
        /// <param name="stateTypes"></param>
        public void RemoveState(StateTypes stateTypes)
        {
            if (!this.HasAbsoluteEqualsState(stateTypes))
                return;

            List<AFsmStateBase> statesToBeRemoved = new List<AFsmStateBase>();
            foreach (var state in this.m_States[stateTypes])
            {
                statesToBeRemoved.Add(state);
            }

            this.m_States[stateTypes].Clear();

            //是否移除了一个曾经是头节点的状态
            bool removedFirstState = false;
            foreach (var state in statesToBeRemoved)
            {
                if (!removedFirstState)
                {
                    removedFirstState = this.CheckIsFirstState(state);
                }

                this.m_FsmStateBases.Remove(state);
                state.OnExit(this);
                state.OnRemoved(this);
                ReferencePool.Release(state);
            }

            if (removedFirstState)
            {
                this.GetCurrentFsmState()?.OnEnter(this);
            }
        }

        #endregion

        #region 状态检测

        /// <summary>
        /// 是否包含某个状态_通过状态类型判断，需要包含targetStateTypes的超集才会返回true
        /// </summary>
        /// <param name="targetStateTypes"></param>
        /// <returns></returns>
        public bool ContainsState(StateTypes targetStateTypes)
        {
            foreach (var state in this.m_States)
            {
                if ((targetStateTypes & state.Key) == targetStateTypes && state.Value.Count > 0)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 是否完全包含某个状态，需要包含targetStateTypes一致的时候才会返回true
        /// </summary>
        /// <param name="targetStateTypes"></param>
        /// <returns></returns>
        public bool HasAbsoluteEqualsState(StateTypes targetStateTypes)
        {
            foreach (var state in this.m_States)
            {
                if (targetStateTypes == state.Key && state.Value.Count > 0)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 是否会发生状态互斥，只要包含了conflictStateTypes的子集，就返回true
        /// </summary>
        /// <param name="conflictStateTypes">互斥的状态</param>
        /// <returns></returns>
        public bool CheckConflictState(StateTypes conflictStateTypes)
        {
            foreach (var state in this.m_States)
            {
                if (state.Key != 0 && (conflictStateTypes & state.Key) == state.Key && state.Value.Count > 0)
                {
                    return true;
                }
            }

            return false;
        }

        #endregion

        #region 获取状态

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

        #endregion

        #region 切换状态

        /// <summary>
        /// 切换状态，如果当前已存在，说明需要把它提到同优先级状态的前面去，让他先执行，切换成功返回成功，切换失败返回失败
        /// 这里的切换成功是指目标状态来到链表头部，插入到链表中或者插入失败都属于切换失败
        /// </summary>
        public bool ChangeState(AFsmStateBase aFsmStateBase)
        {
            AFsmStateBase tempFsmStateBase = this.GetState(aFsmStateBase.StateName);

            if (tempFsmStateBase != null)
            {
                //因为已有此状态，所以进行回收
                ReferencePool.Release(aFsmStateBase);
                this.InsertState(tempFsmStateBase, true);
                return CheckIsFirstState(tempFsmStateBase);
            }

            this.InsertState(aFsmStateBase);
            return CheckIsFirstState(aFsmStateBase);
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

                if (this.m_States.TryGetValue(fsmStateToInsert.StateTypes, out var stateList))
                {
                    stateList.Add(fsmStateToInsert);
                }
                else
                {
                    this.m_States.Add(fsmStateToInsert.StateTypes, new List<AFsmStateBase>() { fsmStateToInsert });
                }
            }

            //如果这个被插入的状态成为了链表首状态，说明发生了状态变化
            if (CheckIsFirstState(fsmStateToInsert))
            {
                //Log.Info($"打断了{tempFirstState?.StateName},开始{fsmStateToInsert.StateName}");
                tempFirstState?.OnExit(this);
                fsmStateToInsert.OnEnter(this);
            }
        }

        #endregion
    }
}