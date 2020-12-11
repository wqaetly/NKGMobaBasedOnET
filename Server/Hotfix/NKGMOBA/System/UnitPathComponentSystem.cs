using System.Collections.Generic;
using System.Threading;
using ETModel;
using ETModel.NKGMOBA.Battle.Fsm;
using ETModel.NKGMOBA.Battle.State;
using UnityEngine;

namespace ETHotfix
{
    [Event(EventIdType.CancelMove)]
    public class CancelMoveEvent: AEvent<long>
    {
        public override void Run(long a)
        {
            UnitComponent.Instance.Get(a).GetComponent<UnitPathComponent>().CancelMove();
        }
    }

    [ObjectSystem]
    public class UnitPathComponentUpdateSystem: UpdateSystem<UnitPathComponent>
    {
        public override void Update(UnitPathComponent self)
        {
            self.Update();
        }
    }

    public static class UnitPathComponentHelper
    {
        public static void Update(this UnitPathComponent self)
        {
            if (self.CancellationTokenSource != null && self.TargetRange >= 0 &&
                Vector3.Distance((self.Entity as Unit).Position, self.Target) <= self.TargetRange)
            {
                self.CancellationTokenSource?.Cancel();
                self.CancellationTokenSource = null;

                if (self.NextState == null) return;
                self.Entity.GetComponent<StackFsmComponent>().ChangeState(self.NextState);
            }
        }

        private static async ETTask MoveAsync(this UnitPathComponent self, List<Vector3> path)
        {
            if (path.Count == 0)
            {
                return;
            }

            // 第一个点是unit的当前位置，所以不用发送
            for (int i = 1; i < path.Count; ++i)
            {
                // 每移动3个点发送下3个点给客户端
                if (i % 3 == 1)
                {
                    self.BroadcastPath(path, i, 3);
                }

                Vector3 v3 = path[i];
                await self.Entity.GetComponent<MoveComponent>().MoveToAsync(v3, self.CancellationTokenSource.Token);
            }
        }

        /// <summary>
        /// 正常寻路，寻路完成后进入Idle状态
        /// </summary>
        /// <param name="self"></param>
        /// <param name="target"></param>
        public static void CommonNavigate(this UnitPathComponent self, Vector3 target)
        {
            if (!self.Entity.GetComponent<StackFsmComponent>().ChangeState<NavigateState>(StateTypes.Run, "Navigate", 1))
            {
                return;
            }

            IdleState idleState = ReferencePool.Acquire<IdleState>();
            idleState.SetData(StateTypes.Idle, "Idle", 1);

            self.MoveTodoSomething(target, self.Entity.GetComponent<StackFsmComponent>().GetCurrentFsmState(), idleState).Coroutine();
        }

        /// <summary>
        /// 寻路到某个点然后做某事，如果当前距离小于目标距离则直接进入状态，否则就寻路到适合的地点再进行寻路
        /// </summary>
        /// <param name="self"></param>
        /// <param name="target">目标地点</param>
        /// <param name="targetRange">目标距离</param>
        /// <param name="targetState">目标状态</param>
        public static void NavigateTodoSomething(this UnitPathComponent self, Vector3 target, float targetRange, AFsmStateBase targetState)
        {
            if (Vector3.Distance((self.Entity as Unit).Position, target) >= targetRange)
            {
                if (!self.Entity.GetComponent<StackFsmComponent>().ChangeState<NavigateState>(StateTypes.Run, "Navigate", 1))
                {
                    ReferencePool.Release(targetState);
                    return;
                }

                self.MoveTodoSomething(target, self.Entity.GetComponent<StackFsmComponent>().GetCurrentFsmState(), targetState, targetRange)
                        .Coroutine();
            }
            else
            {
                self.Entity.GetComponent<StackFsmComponent>().ChangeState(targetState);
            }
        }

        /// <summary>
        /// 移动到目标点，随后进入指定状态（如果有的话）
        /// </summary>
        /// <param name="self"></param>
        /// <param name="target"></param>
        /// <param name="bindState"></param>
        /// <param name="nextState"></param>
        /// <param name="targetRange">目标范围，当自身与目标位置小于等于此范围时，则停止寻路，进入NextState</param>
        /// <returns></returns>
        private static async ETVoid MoveTodoSomething(this UnitPathComponent self, Vector3 target, AFsmStateBase bindState,
        AFsmStateBase nextState = null, float targetRange = 0)
        {
            self.BindState = bindState;
            self.NextState = nextState;

            self.Target = target;
            self.TargetRange = targetRange;

            Unit unit = self.GetParent<Unit>();

            RecastPathComponent recastPathComponent = Game.Scene.GetComponent<RecastPathComponent>();
            RecastPath recastPath = ReferencePool.Acquire<RecastPath>();
            recastPath.StartPos = unit.Position;
            recastPath.EndPos = new Vector3(target.x, target.y, target.z);
            self.RecastPath = recastPath;
            //TODO 因为目前阶段只有一张地图，所以默认mapId为10001
            recastPathComponent.SearchPath(10001, self.RecastPath);
            //Log.Debug($"find result: {self.ABPath.Result.ListToString()}");

            self.CancellationTokenSource?.Cancel();
            self.CancellationTokenSource = new CancellationTokenSource();
            await self.MoveAsync(self.RecastPath.Results);
            self.CancellationTokenSource.Dispose();
            self.CancellationTokenSource = null;

            if (nextState != null)
            {
                self.Entity.GetComponent<StackFsmComponent>().ChangeState(nextState);
            }

            self.Entity.GetComponent<StackFsmComponent>().RemoveState(bindState.StateName);
        }

        public static void CancelMove(this UnitPathComponent self)
        {
            self.CancellationTokenSource?.Cancel();
            self.CancellationTokenSource = null;

            self.TargetRange = 0;
            self.Target = Vector3.zero;

            if (self.BindState != null)
            {
                self.Entity.GetComponent<StackFsmComponent>().RemoveState(self.BindState.StateName);
                self.BindState = null;
            }

            if (self.NextState != null)
            {
                ReferencePool.Release(self.NextState);
                self.NextState = null;
            }

            Vector3 selfUnitPos = (self.Entity as Unit).Position;

            M2C_PathfindingResult pathfindingResult = new M2C_PathfindingResult()
            {
                X = selfUnitPos.x, Y = selfUnitPos.y, Z = selfUnitPos.z, Id = self.Entity.Id
            };
            pathfindingResult.Xs.Clear();
            pathfindingResult.Ys.Clear();
            pathfindingResult.Zs.Clear();
            MessageHelper.Broadcast(pathfindingResult);
        }

        // 从index找接下来3个点，广播
        public static void BroadcastPath(this UnitPathComponent self, List<Vector3> path, int index, int offset)
        {
            Unit unit = self.GetParent<Unit>();
            Vector3 unitPos = unit.Position;
            M2C_PathfindingResult m2CPathfindingResult = new M2C_PathfindingResult();
            m2CPathfindingResult.X = unitPos.x;
            m2CPathfindingResult.Y = unitPos.y;
            m2CPathfindingResult.Z = unitPos.z;
            m2CPathfindingResult.Id = unit.Id;

            for (int i = 0; i < offset; ++i)
            {
                if (index + i >= self.RecastPath.Results.Count)
                {
                    break;
                }

                Vector3 v = self.RecastPath.Results[index + i];
                m2CPathfindingResult.Xs.Add(v.x);
                m2CPathfindingResult.Ys.Add(v.y);
                m2CPathfindingResult.Zs.Add(v.z);
            }

            MessageHelper.Broadcast(m2CPathfindingResult);
        }
    }
}