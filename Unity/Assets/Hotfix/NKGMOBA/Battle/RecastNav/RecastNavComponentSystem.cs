using System;
using System.Collections.Generic;
using UnityEngine;

namespace ET
{
    [ObjectSystem]
    public class AwakeSystem : AwakeSystem<RecastNavComponent, string>
    {
        public override void Awake(RecastNavComponent self, string name)
        {
            self.Name = name;
            self.NavMesh = RecastNavMeshManagerComponent.Instance.Get(name);

            if (self.NavMesh == IntPtr.Zero)
            {
                throw new Exception($"nav load fail: {name}");
            }
        }
    }

    [ObjectSystem]
    public class DestroySystem : DestroySystem<RecastNavComponent>
    {
        public override void Destroy(RecastNavComponent self)
        {
            self.Name = string.Empty;
            self.NavMesh = IntPtr.Zero;
        }
    }


    public static class RecastNavComponentSystem
    {
        public static void Find(this RecastNavComponent self, Vector3 start, Vector3 target, List<Vector3> result)
        {
            if (self.NavMesh == IntPtr.Zero)
            {
                Log.Debug("寻路| Find 失败 pathfinding ptr is zero");
                throw new Exception($"pathfinding ptr is zero: {self.DomainScene().Name}");
            }

            self.StartPos[0] = -start.x;
            self.StartPos[1] = start.y;
            self.StartPos[2] = start.z;

            self.EndPos[0] = -target.x;
            self.EndPos[1] = target.y;
            self.EndPos[2] = target.z;
            //Log.Debug($"start find path: {self.GetParent<Unit>().Id}");
            int n = RecastInterface.RecastFind(self.NavMesh, RecastNavComponent.extents, self.StartPos, self.EndPos,
                self.Result);
            for (int i = 0; i < n; ++i)
            {
                int index = i * 3;
                result.Add(new Vector3(-self.Result[index], self.Result[index + 1], self.Result[index + 2]));
            }
            //Log.Debug($"finish find path: {self.GetParent<Unit>().Id} {result.ListToString()}");
        }

        public static void FindWithAdjust(this RecastNavComponent self, Vector3 start, Vector3 target,
            List<Vector3> result, float adjustRaduis)
        {
            self.Find(start, target, result);
            for (int i = 0; i < result.Count; i++)
            {
                Vector3 adjust = self.FindRandomPointWithRaduis(result[i], adjustRaduis);
                result[i] = adjust;
            }
        }

        public static Vector3 FindRandomPointWithRaduis(this RecastNavComponent self, Vector3 pos, float raduis)
        {
            if (self.NavMesh == IntPtr.Zero)
            {
                throw new Exception($"pathfinding ptr is zero: {self.DomainScene().Name}");
            }

            if (raduis > RecastNavComponent.FindRandomNavPosMaxRadius * 0.001f)
            {
                throw new Exception(
                    $"pathfinding raduis is too large，cur: {raduis}, max: {RecastNavComponent.FindRandomNavPosMaxRadius}");
            }

            int degrees = RandomHelper.RandomNumber(0, 360);
            float r = RandomHelper.RandomNumber(0, (int) (raduis * 1000)) / 1000f;

            float x = r * Mathf.Cos(MathHelper.DegToRad(degrees));
            float z = r * Mathf.Sin(MathHelper.DegToRad(degrees));

            Vector3 findpos = new Vector3(pos.x + x, pos.y, pos.z + z);

            return self.RecastFindNearestPoint(findpos);
        }

        /// <summary>
        /// 以pos为中心各自在宽和高的左右 前后两个方向延伸
        /// </summary>
        /// <param name="self"></param>
        /// <param name="pos"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static Vector3 FindRandomPointWithRectangle(this RecastNavComponent self, Vector3 pos, int width,
            int height)
        {
            if (self.NavMesh == IntPtr.Zero)
            {
                throw new Exception($"pathfinding ptr is zero: {self.DomainScene().Name}");
            }

            if (width > RecastNavComponent.FindRandomNavPosMaxRadius * 0.001f ||
                height > RecastNavComponent.FindRandomNavPosMaxRadius * 0.001f)
            {
                throw new Exception(
                    $"pathfinding rectangle is too large，width: {width} height: {height}, max: {RecastNavComponent.FindRandomNavPosMaxRadius}");
            }

            float x = RandomHelper.RandomNumber(-width, width);
            float z = RandomHelper.RandomNumber(-height, height);

            Vector3 findpos = new Vector3(pos.x + x, pos.y, pos.z + z);

            return self.RecastFindNearestPoint(findpos);
        }

        public static Vector3 FindRandomPointWithRaduis(this RecastNavComponent self, Vector3 pos, float minRadius,
            float maxRadius)
        {
            if (self.NavMesh == IntPtr.Zero)
            {
                throw new Exception($"pathfinding ptr is zero: {self.DomainScene().Name}");
            }

            if (maxRadius > RecastNavComponent.FindRandomNavPosMaxRadius * 0.001f)
            {
                throw new Exception(
                    $"pathfinding raduis is too large，cur: {maxRadius}, max: {RecastNavComponent.FindRandomNavPosMaxRadius}");
            }

            int degrees = RandomHelper.RandomNumber(0, 360);
            float r = RandomHelper.RandomNumber((int) (minRadius * 1000), (int) (maxRadius * 1000)) / 1000f;

            float x = r * Mathf.Cos(MathHelper.DegToRad(degrees));
            float z = r * Mathf.Sin(MathHelper.DegToRad(degrees));

            Vector3 findpos = new Vector3(pos.x + x, pos.y, pos.z + z);

            return self.RecastFindNearestPoint(findpos);
        }

        public static Vector3 RecastFindNearestPoint(this RecastNavComponent self, Vector3 pos)
        {
            if (self.NavMesh == IntPtr.Zero)
            {
                throw new Exception($"pathfinding ptr is zero: {self.DomainScene().Name}");
            }

            self.StartPos[0] = -pos.x;
            self.StartPos[1] = pos.y;
            self.StartPos[2] = pos.z;

            int ret = RecastInterface.RecastFindNearestPoint(self.NavMesh, RecastNavComponent.extents, self.StartPos,
                self.EndPos);
            if (ret == 0)
            {
                throw new Exception(
                    $"RecastFindNearestPoint fail, 可能是位置配置有问题: sceneName:{self.DomainScene().Name} {pos} {self.Name} {self.GetParent<Unit>().Id} {self.GetParent<Unit>().ConfigId} {self.EndPos.ArrayToString()}");
            }

            return new Vector3(-self.EndPos[0], self.EndPos[1], self.EndPos[2]);
        }
    }
}