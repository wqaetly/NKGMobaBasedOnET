using System.Collections.Generic;
using System.IO;
using ETModel;
using UnityEditor;
using UnityEngine;

namespace ETEditor
{
    public class VoxelsImpoter: Editor
    {
        private static VoxelFile _voxelFile = new VoxelFile();

        [MenuItem("Tools/NavMesh/Import Voxel")]
        public static void ImportVoxel()
        {
            var path = Application.dataPath + "/Res/Voxels";
            var pathName = EditorUtility.OpenFilePanel("Open Voxels File", path, "vox");
            if (string.IsNullOrEmpty(pathName))
                return;
            if (!_voxelFile.LoadVoxelFile(pathName))
            {
                Debug.LogError($"VoxelsImpoter ImportVoxel Failed - load file faile:{pathName}");
                return;
            }

            //using (var writer = new BinaryWriter(File.OpenWrite(pathName + ".dat")))
            //{
            //    writer.Write(_voxelFile.Header.numVoxels);
            //    for (int y = 0; y < _voxelFile.Header.VoxelHeight; y++)
            //    {
            //        for (int x = 0; x < _voxelFile.Header.VoxelWidth; x++)
            //        {
            //            var heightSpans = _voxelFile.GetSpans(x, y);
            //            if (heightSpans == null)
            //                continue;
            //            foreach (var span in heightSpans)
            //            {
            //                writer.Write(span.smin.x);
            //                writer.Write(span.smin.y);
            //                writer.Write(span.smin.z);
            //                writer.Write(span.smax.x);
            //                writer.Write(span.smax.y);
            //                writer.Write(span.smax.z);
            //            }
            //        }
            //    }
            //}
            showInUnity();
        }

        private static void showInUnity()
        {
            GameObject[] goLast = GameObject.FindGameObjectsWithTag("Voxels");
            foreach (var go in goLast)
            {
                DestroyImmediate(go);
            }

            GameObject goParent = new GameObject();
            goParent.tag = "Voxels";

            var mat = Resources.Load("Mat/Voxel") as Material;
            var matNoWalk = Resources.Load("Mat/Voxel_noWalk") as Material;

            int vertCount0 = 0; // 可行走体素的总个数
            int vertCount1 = 0; // 不可行走体素的总个数
            int groupIndex = 0; // 顶点数有最大值（65535），所以要分批次渲染
            List<Vector3> totalVerts0 = new List<Vector3>(); // 可以行走的所有三角形顶点
            //List<Vector3> totalVerts1 = new List<Vector3>(); // 不可以行走的所有三角形顶点

            int w = _voxelFile.Header.VoxelWidth;
            int h = _voxelFile.Header.VoxelHeight;
            int spanCount = 0;
            for (var x = 0; x < w; ++x)
            {
                for (var z = 0; z < h; ++z)
                {
                    var spans = _voxelFile.GetSpans(x, z);
                    foreach (var span in spans)
                    {
                        List<Vector3> vecList = _voxelFile.duAppendBoxBuffer(span.smin.x, span.smin.y, span.smin.z,
                            span.smax.x, span.smax.y, span.smax.z);

                        //if (span.area == 63)
                        //{
                        vertCount0 += vecList.Count;
                        totalVerts0.AddRange(vecList);
                        if (totalVerts0.Count >= 65500)
                        {
                            AddToParent(ref totalVerts0, groupIndex++, mat, goParent, true);
                        }
                        //}
                        //else
                        //{
                        //    vertCount1 += vecList.Count;
                        //    totalVerts1.AddRange(vecList);
                        //    if (totalVerts1.Count >= 65535)
                        //    {
                        //        AddToParent(ref totalVerts1, groupIndex++, matNoWalk, goParent, false);
                        //    }
                        //}

                        spanCount++;
                    }
                }
            }

            AddToParent(ref totalVerts0, groupIndex++, mat, goParent, true);
            //AddToParent(ref totalVerts1, groupIndex++, matNoWalk, goParent, false);

            goParent.name = $"Voxel({spanCount}) Vertex({vertCount0}:{vertCount1}/{vertCount0 + vertCount1})";
        }

        private static void AddToParent(ref List<Vector3> totalVerts, int groupIndex, Material mat, GameObject goParent, bool canWalk)
        {
            List<int> triList = new List<int>();
            for (int i = 0; i < totalVerts.Count; ++i)
            {
                triList.Add(i);
            }

            GameObject go = new GameObject();
            var mf = go.AddComponent<MeshFilter>();
            mf.mesh.SetVertices(totalVerts);
            mf.mesh.SetIndices(triList.ToArray(), MeshTopology.Triangles, 0);
            mf.mesh.RecalculateNormals();
            mf.mesh.RecalculateBounds();
            mf.mesh.RecalculateTangents();
            go.name = $"({groupIndex}) ";
            if (!canWalk)
            {
                go.name += " no walk";
            }

            var rr = go.AddComponent<MeshRenderer>();
            rr.material = mat;
            //rr.receiveShadows = false;

            go.transform.SetParent(goParent.transform);

            totalVerts.Clear();
        }

        [MenuItem("Tools/NavMesh/calc Voxel")]
        public static void CalcVoxel()
        {
            var from = new Vector3(13.89008f, 4.476905f, 1.206503f);
            var to = new Vector3(13.46899f, 4.476905f, 1.28847f);
            var ret = _voxelFile.Recaycast(from, to, 9, out Vector3 hitPos);
            Log.Error($"计算Voxel碰撞：{ret}");
        }
    }
}