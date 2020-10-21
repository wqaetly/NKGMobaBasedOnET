using System.Collections.Generic;
using ETModel;
using ETModel.NKGMOBA.Battle.Fsm;
using ETModel.NKGMOBA.Battle.State;
using UnityEngine;

namespace ETHotfix
{
    public static class UnitPathComponentHelper
    {
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
                await self.Entity.GetComponent<MoveComponent>().MoveToAsync(v3, self.ETCancellationTokenSource.Token);
            }
        }

        public static async ETVoid MoveTo(this UnitPathComponent self, Vector3 target)
        {
            if (!self.Entity.GetComponent<StackFsmComponent>().ChangeState<NavigateState>(StateTypes.Run, "Navigate", 1))
            {
                return;
            }

            if ((self.Target - target).magnitude < 0.1f)
            {
                return;
            }

            self.Target = target;

            Unit unit = self.GetParent<Unit>();

            PathfindingComponent pathfindingComponent = Game.Scene.GetComponent<PathfindingComponent>();
            self.ABPath = ComponentFactory.Create<ABPathWrap, Vector3, Vector3>(unit.Position, new Vector3(target.x, target.y, target.z));
            pathfindingComponent.Search(self.ABPath);
            //Log.Debug($"find result: {self.ABPath.Result.ListToString()}");

            self.ETCancellationTokenSource?.Cancel();
            self.ETCancellationTokenSource = ComponentFactory.Create<ETCancellationTokenSource>();
            await self.MoveAsync(self.ABPath.Result);
            self.ETCancellationTokenSource.Dispose();
            self.Entity.GetComponent<StackFsmComponent>().ChangeState<NavigateState>(StateTypes.Idle, "Idle", 1);
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
                if (index + i >= self.ABPath.Result.Count)
                {
                    break;
                }

                Vector3 v = self.ABPath.Result[index + i];
                m2CPathfindingResult.Xs.Add(v.x);
                m2CPathfindingResult.Ys.Add(v.y);
                m2CPathfindingResult.Zs.Add(v.z);
            }

            MessageHelper.Broadcast(m2CPathfindingResult);
        }
    }
}