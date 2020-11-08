using System.Collections.Generic;
using UnityEngine;

namespace ETModel
{
    public class RecastPath: IReference
    {
        public Vector3 StartPos = Vector3.zero;
        public Vector3 EndPos = Vector3.zero;

        public List<Vector3> Results = new List<Vector3>();

        public void Clear()
        {
            StartPos = Vector3.zero;
            EndPos = Vector3.zero;
            Results.Clear();
        }
    }

    public static class RecastPathExtension
    {
        /// <summary>
        /// 修改坐标以适配Recast
        /// </summary>
        /// <param name="self"></param>
        public static void AujustSelfContentForRecast(this RecastPath self)
        {
            self.StartPos.x = -self.StartPos.x;
            self.EndPos.x = -self.EndPos.x;
        }
    }

    /// <summary>
    /// 寻路处理者
    /// </summary>
    public class RecastPathProcessor: IReference
    {
        /// <summary>
        /// 归属的地图Id
        /// </summary>
        public int MapId;

        public void CalculatePath(RecastPath recastPath)
        {
            if (RecastInterface.FindPath(this.MapId, recastPath.StartPos, recastPath.EndPos))
            {
                RecastInterface.Smooth(this.MapId, 2f, 0.5f);
                {
                    int smoothCount = 0;
                    float[] smooths = RecastInterface.GetPathSmooth(this.MapId, out smoothCount);
                    for (int i = 0; i < smoothCount; ++i)
                    {
                        Vector3 node = new Vector3(smooths[i * 3], smooths[i * 3 + 1], smooths[i * 3 + 2]);
                        recastPath.Results.Add(node);
                        //Log.Info($"路径点：{node}");
                    }
                }
            }
        }

        public void Clear()
        {
            MapId = 0;
        }

        // #region Benchmark
        //
        // public static void BenchmarkSample()
        // {
        //     BenchmarkHelper.Profile("寻路100000次", BenchmarkRecast, 100);
        // }
        //
        // private static void BenchmarkRecast()
        // {
        //     if (RecastInterface.FindPath(100,
        //         new System.Numerics.Vector3(-RandomHelper.RandomNumber(2, 50) - RandomHelper.RandFloat(),
        //             RandomHelper.RandomNumber(-1, 5) + RandomHelper.RandFloat(), RandomHelper.RandomNumber(3, 20) + RandomHelper.RandFloat()),
        //         new System.Numerics.Vector3(-RandomHelper.RandomNumber(2, 50) - RandomHelper.RandFloat(),
        //             RandomHelper.RandomNumber(-1, 5) + RandomHelper.RandFloat(), RandomHelper.RandomNumber(3, 20) + RandomHelper.RandFloat())))
        //     {
        //         RecastInterface.Smooth(100, 2f, 0.5f);
        //         {
        //             int smoothCount = 0;
        //             float[] smooths = RecastInterface.GetPathSmooth(100, out smoothCount);
        //             List<Vector3> results = new List<Vector3>();
        //             for (int i = 0; i < smoothCount; ++i)
        //             {
        //                 Vector3 node = new Vector3(smooths[i * 3], smooths[i * 3 + 1], smooths[i * 3 + 2]);
        //                 results.Add(node);
        //             }
        //         }
        //     }
        // }
        //
        // #endregion
    }
}