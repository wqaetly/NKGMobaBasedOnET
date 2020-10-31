using cyh.game;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

namespace ETModel
{
    public class VoxelFile
    {
        public struct VoxelSetHeader
        {
            public long magic;
            public int version;
            public int size;
            public int numVoxels;
            public int VoxelWidth;
            public int VoxelHeight;
            public float CellWidth;
            public float CellHeight;
            public Vector3 origVec;     ///< The world space origin of the navigation mesh's tile space. [(x, y, z)]
            public VoxelParams Params;
            public Vector3 bMin;
            public Vector3 bMax;
        }

        public struct VoxelParams
        {
            public float tileWidth;	    ///< The width of each tile. (Along the x-axis.)
            public float tileHeight;	///< The height of each tile. (Along the z-axis.)
            public int maxTiles;		///< The maximum number of tiles the navigation mesh can contain.
            public int maxPolys;		///< The maximum number of polygons each tile can contain.
        }
        
        public class Span
        {
            public Vector3 smin;
            public Vector3 smax;
            public int area;
        }

        public VoxelSetHeader Header => _header;
        public VoxelParams Params => _header.Params;
        public byte[,][] HeightMap => _heightMap;
        
        private VoxelSetHeader _header;
        private byte[,][] _heightMap;

		private Vector3 _orig;
        private float _cw;
        private float _ch;

        #region 文件处理

        public unsafe bool LoadVoxelFile(string pathName)
        {
            using (BinaryReader reader = new BinaryReader(new FileStream(pathName, FileMode.Open)))
            {
                var bytes = reader.ReadBytes(sizeof(VoxelSetHeader));
                fixed (void* ptr = &bytes[0])
                    _header = *(VoxelSetHeader*)ptr;

                if (_header.numVoxels < 0 || _header.numVoxels > 16777216)
                {
                    return false;
                }

                _orig = _header.origVec;
                _cw = _header.CellWidth;
                _ch = _header.CellHeight;

                int w = _header.VoxelWidth;
                int h = _header.VoxelHeight;

                _heightMap = new byte[w, h][];

                cyh.game.BitStream bitStream = new cyh.game.BitStream(reader.ReadBytes((int)(reader.BaseStream.Length - reader.BaseStream.Position)));

                int nodeCount = 0;
                for (int z = 0; z < _header.VoxelHeight; z++)
                {
                    for (int x = 0; x < _header.VoxelWidth; x++)
                    {
                        int countHeight = (int)bitStream.Read(_header.size);
                        if (countHeight < 0 || countHeight > 1024)
                            return false;
                        if (countHeight == 0)
                            continue;

                        float fx = _orig.x + x * _cw;
                        float fz = _orig.z + z * _cw;

                        var spanHeight = new byte[1];
                        int mh;
                        for (int y = 0; y < countHeight; ++y)
                        {
                            int smin = (int)bitStream.Read(_header.size);
                            int smax = (int)bitStream.Read(_header.size);
                            mh = (smax >> 3) + 1;
                            if (mh > spanHeight.Length)
                                Array.Resize(ref spanHeight, mh);
                            for (int i = smin; i <= smax; i++)
                                cyh.game.BitArray.Set(spanHeight, i, true);
                        }
                        _heightMap[x, z] = spanHeight;
                        nodeCount += countHeight;
                    }
                }
                Log.Info($"Map Size:[{w}x{h}] - Voxel Count:{_header.numVoxels} - Node count:{nodeCount}");
                return true;
            }
        }


        private static readonly int[] vertexIndex = new int[12 * 3]
            {
                0, 1, 2,
                0, 2, 3,
                3, 2, 4,
                4, 2, 5,
                4, 5, 7,
                7, 5, 6,
                7, 1, 0,
                6, 1, 7,
                4, 0, 3,
                4, 7, 0,
                2, 6, 5,
                2, 1, 6,
            };
    /// <summary>
    /// 从AABB盒生成36个顶点，渲染用
    /// </summary>
    /// <param name="minx"></param>
    /// <param name="miny"></param>
    /// <param name="minz"></param>
    /// <param name="maxx"></param>
    /// <param name="maxy"></param>
    /// <param name="maxz"></param>
    /// <returns></returns>
    public List<Vector3> duAppendBoxBuffer(float minx, float miny, float minz, float maxx, float maxy, float maxz)
        {
            Vector3[] vertices = new Vector3[8]
            {
                new Vector3(minx, miny, minz), 
                new Vector3(maxx, miny, minz),
                new Vector3(maxx, maxy, minz),
                new Vector3(minx, maxy, minz),
                new Vector3(minx, maxy, maxz),
                new Vector3(maxx, maxy, maxz),
                new Vector3(maxx, miny, maxz),
                new Vector3(minx, miny, maxz),
            };
            
            List<Vector3> vecList = new List<Vector3>();
            for (int i = 0; i < vertexIndex.Length; ++i)
            {
                var vec = vertices[vertexIndex[i]];
                vecList.Add(vec);
            }

            return vecList;
        }

        public List<Vector3> duAppendBoxBuffer(int x, int z)
        {
            byte[] heightSpans = this._heightMap[x, z];
            List<Vector3> list = new List<Vector3>();

            float minx = -(_orig.x + x * _header.CellWidth);
            float maxx = -(_orig.x + (x + 1) * _header.CellWidth);
            float minz = _orig.z + z * _header.CellWidth;
            float maxz = _orig.z + (z + 1) * _header.CellWidth;

            int count = heightSpans.Length << 3;
            bool s, v = false;
            int _y = 0;
            for (int i = 0; i < count; i++)
			{
                s = BitArray.Get(heightSpans, i);
                if (v != s)
                {
                    v = s;
                    if (!s)
					{
                        float miny = _orig.y + _y * _header.CellHeight;
                        float maxy = _orig.y + i * _header.CellHeight;
                        Vector3[] vertices = new Vector3[8]
                        {
                            new Vector3(minx, miny, minz),
                            new Vector3(maxx, miny, minz),
                            new Vector3(maxx, maxy, minz),
                            new Vector3(minx, maxy, minz),
                            new Vector3(minx, maxy, maxz),
                            new Vector3(maxx, maxy, maxz),
                            new Vector3(maxx, miny, maxz),
                            new Vector3(minx, miny, maxz),
                        };
                        for (int j = 0; j < vertexIndex.Length; ++j)
                        {
                            var vec = vertices[vertexIndex[j]];
                            list.Add(vec);
                        }
                    }
                    _y = i;
				}
			}

            return list;
        }

        public List<Span> GetSpans(int x, int z)
        {
            byte[] heightSpans = this._heightMap[x, z];
            List<Span> list = new List<Span>();

            if (heightSpans == null)
                return list;
            float minx = -(_orig.x + x * _header.CellWidth);
            float maxx = -(_orig.x + (x + 1) * _header.CellWidth);
            float minz = _orig.z + z * _header.CellWidth;
            float maxz = _orig.z + (z + 1) * _header.CellWidth;
            int count = heightSpans.Length << 3;
            bool s = false, v = false;
            int _y = 0;
            for (int i = 0; i < count; i++)
            {
                s = BitArray.Get(heightSpans, i);
                if (v != s)
                {
                    v = s;
                    if (!s)
                    {
                        float miny = _orig.y + _y * _header.CellHeight;
                        float maxy = _orig.y + i * _header.CellHeight;
                        Span span = new Span();
                        span.smin = new Vector3(minx, miny, minz);
                        span.smax = new Vector3(maxx, maxy, maxz);
                        list.Add(span);
                    }
                    _y = i;
                }
            }
            if(s)
			{
                float miny = _orig.y + _y * _header.CellHeight;
                float maxy = _orig.y + count * _header.CellHeight;
                Span span = new Span();
                span.smin = new Vector3(minx, miny, minz);
                span.smax = new Vector3(maxx, maxy, maxz);
                list.Add(span);
            }
            return list;
        }

        #endregion

        #region 碰撞检测

        /// <summary>
        /// 碰撞检测函数
        /// </summary>
        /// <param name="pos">给定的坐标，判断这个坐标的高度是否被碰撞</param>
        /// <returns></returns>
        public bool HitTest(Vector3 pos, out Vector3 hitPos)
        {
            int outX = Mathf.FloorToInt((-pos.x - _orig.x) / _cw); // 左右手坐标系的问题，X坐标还是要取反一下
            int outY = Mathf.FloorToInt((pos.y - _orig.y) / _ch);
            int outZ = Mathf.FloorToInt((pos.z - _orig.z) / _cw);
            hitPos = _orig;
            hitPos.x = -hitPos.x;
            if (outX < 0 || outX >= _header.VoxelWidth || outZ < 0 || outZ >= _header.VoxelHeight)// 水平方向出界
                return false;
            if (outY < 0)//下边缘出界
                return false;
            var spanHeight = _heightMap[outX, outZ];
            if (spanHeight == null)
                return false; // 空洞，没有碰撞
            if ((outY >> 3) >= spanHeight.Length)//上方出界（不一定到边缘）
                return false;
            hitPos.x -= ((float)outX + 0.5f) * _header.CellWidth;
            hitPos.y += ((float)outY + 0.5f) * _header.CellHeight;
            hitPos.z += ((float)outZ + 0.5f) * _header.CellWidth;
            
            return BitArray.Get(spanHeight, outY);
        }

        public bool Recaycast(Vector3 from, Vector3 to, int segmentCount, out Vector3 hitPos)
        {
            for (var i = 0; i <= segmentCount; ++i)
            {
                var pos = Vector3.Lerp(from, to, i/(float)segmentCount);
                var ret = HitTest(pos, out hitPos);
                if (ret) return true;
            }
            hitPos = _orig;
            return false;
        }
        
        #endregion
    }
}