using System;
using System.Collections.Generic;
using System.Numerics;
using Box2DSharp.Collision.Shapes;
using Box2DSharp.Dynamics;

namespace ETModel
{
    public static class Separator
    {
        public static void Separate(Body body, FixtureDef fixtureDef, Vector2[] vertices)
        {
            const float scale = 30;
            int i;
            var n = vertices.Length;
            var vec = new List<Vector2>();

            for (i = 0; i < n; i++)
            {
                vec.Add(new Vector2(vertices[i].X * scale, vertices[i].Y * scale));
            }

            var figsVec = CalcShapes(vec);
            n = figsVec.Count;

            for (i = 0; i < n; i++)
            {
                vec = figsVec[i];
                vertices = new Vector2[vec.Count];
                var m = vec.Count;
                int j;
                for (j = 0; j < m; j++)
                {
                    vertices[j] = new Vector2(vec[j].X / scale, vec[j].Y / scale);
                }

                var polyShape = new PolygonShape();
                polyShape.Set(vertices);
                fixtureDef.Shape = polyShape;
                body.CreateFixture(fixtureDef);
            }
        }

        /// <summary>
        /// 单一多边形最大支持顶点数,此方法应在凹凸多边形转换之后调用，确保不会出现多余的凹多边形
        /// </summary>
        /// <param name="maxLimit"></param>
        public static List<List<Vector2>> SplitPolygonUntilLessX(int maxLimit, List<List<Vector2>> orinPolygonInfos)
        {
            List<List<Vector2>> mytempPolygonInfos = new List<List<Vector2>>();
            for (int i = 0; i < orinPolygonInfos.Count; i++)
            {
                if (orinPolygonInfos[i].Count > maxLimit)
                {
                    int lastPos = 0;

                    while (lastPos < orinPolygonInfos[i].Count && orinPolygonInfos[i].Count - lastPos + 1 >= 3)
                    {
                        var newPolygonInfo = new List<Vector2>();
                        if (lastPos == 0)
                        {
                            for (int j = lastPos;
                                j <= lastPos + maxLimit - 1; j++)
                            {
                                newPolygonInfo.Add(orinPolygonInfos[i][j]);
                            }

                            lastPos += maxLimit - 1;
                        }
                        else
                        {
                            newPolygonInfo.Add(orinPolygonInfos[i][0]);
                            if (lastPos + maxLimit - 2 < orinPolygonInfos[i].Count)
                            {
                                for (int j = lastPos;
                                    j <= lastPos + maxLimit - 2; j++)
                                {
                                    newPolygonInfo.Add(orinPolygonInfos[i][j]);
                                }
                            }
                            else
                            {
                                for (int j = lastPos;
                                    j < orinPolygonInfos[i].Count; j++)
                                {
                                    newPolygonInfo.Add(orinPolygonInfos[i][j]);
                                }
                            }

                            lastPos += maxLimit - 2;
                        }

                        mytempPolygonInfos.Add(newPolygonInfo);
                    }
                }
                else
                {
                    mytempPolygonInfos.Add(orinPolygonInfos[i]);
                }
            }

            return mytempPolygonInfos;
        }

        /// <summary>
        /// Checks whether the vertices in <code>verticesVec</code> can be properly distributed into the new fixtures (more specifically, it makes sure there are no overlapping segments and the vertices are in clockwise order). 
        /// It is recommended that you use this method for debugging only, because it may cost more CPU usage.
        /// <para>0 if the vertices can be properly processed.</para>
        /// <para>1 If there are overlapping lines.</para>
        /// <para>2 if the points are <b>not</b> in clockwise order.</para>
        /// <para>3 if there are overlapping lines <b>and</b> the points are <b>not</b> in clockwise order.</para>
        /// </summary>
        /// <param name="verticesVec">verticesVec The vertices to be validated</param>
        /// <returns>An integer which can have the following values</returns>
        public static int Validate(Vector2[] verticesVec)
        {
            var n = verticesVec.Length;
            var ret = 0;
            var fl2 = false;

            for (var i = 0; i < n; i++)
            {
                var i2 = (i < n - 1? i + 1 : 0);
                var i3 = (i > 0? i - 1 : n - 1);

                var fl = false;
                int j;
                for (j = 0; j < n; j++)
                {
                    if (j == i || j == i2)
                    {
                        continue;
                    }

                    if (!fl)
                    {
                        var d = Det(verticesVec[i].X,
                            verticesVec[i].Y,
                            verticesVec[i2].X,
                            verticesVec[i2].Y,
                            verticesVec[j].X,
                            verticesVec[j].Y);
                        if (d > 0)
                        {
                            fl = true;
                        }
                    }

                    if (j == i3)
                    {
                        continue;
                    }

                    var j2 = (j < n - 1? j + 1 : 0);
                    if (HitSegment(verticesVec[i].X,
                            verticesVec[i].Y,
                            verticesVec[i2].X,
                            verticesVec[i2].Y,
                            verticesVec[j].X,
                            verticesVec[j].Y,
                            verticesVec[j2].X,
                            verticesVec[j2].Y)
                        != null)
                    {
                        ret = 1;
                    }
                }

                if (!fl)
                {
                    fl2 = true;
                }
            }

            if (fl2)
            {
                ret = ret == 1? 3 : 2;
            }

            return ret;
        }

        public static List<List<Vector2>> CalcShapes(List<Vector2> verticesVec)
        {
            var k = 0;
            var h = -1;
            var hitV = new Vector2();
            var figsVec = new List<List<Vector2>>();
            var queue = new List<List<Vector2>> { verticesVec };

            while (queue.Count > 0)
            {
                var vec = queue[0];
                var n = vec.Count;
                var isConvex = true;

                int i;
                for (i = 0; i < n; i++)
                {
                    var i1 = i;
                    var i2 = (i < n - 1? i + 1 : i + 1 - n);
                    var i3 = (i < n - 2? i + 2 : i + 2 - n);

                    var p1 = vec[i1];
                    var p2 = vec[i2];
                    var p3 = vec[i3];

                    var d = Det(p1.X, p1.Y, p2.X, p2.Y, p3.X, p3.Y);
                    if (!(d < 0))
                    {
                        continue;
                    }

                    isConvex = false;
                    var minLen = float.MaxValue;

                    int j;
                    int j1;
                    int j2;
                    Vector2 v1;
                    Vector2 v2;
                    for (j = 0; j < n; j++)
                    {
                        if (j != i1 && j != i2)
                        {
                            j1 = j;
                            j2 = (j < n - 1? j + 1 : 0);

                            v1 = vec[j1];
                            v2 = vec[j2];

                            var v = HitRay(p1.X,
                                p1.Y,
                                p2.X,
                                p2.Y,
                                v1.X,
                                v1.Y,
                                v2.X,
                                v2.Y);

                            if (v.HasValue)
                            {
                                var dx = p2.X - v.Value.X;
                                var dy = p2.Y - v.Value.Y;
                                var t = dx * dx + dy * dy;

                                if (t < minLen)
                                {
                                    h = j1;
                                    k = j2;
                                    hitV = v.Value;
                                    minLen = t;
                                }
                            }
                        }
                    }

                    if (Math.Abs(minLen - float.MaxValue) < 0.01)
                    {
                        Err();
                    }

                    var vec1 = new List<Vector2>();
                    var vec2 = new List<Vector2>();

                    j1 = h;
                    j2 = k;
                    v1 = vec[j1];
                    v2 = vec[j2];

                    if (!PointsMatch(hitV.X, hitV.Y, v2.X, v2.Y))
                    {
                        vec1.Add(hitV);
                    }

                    if (!PointsMatch(hitV.X, hitV.Y, v1.X, v1.Y))
                    {
                        vec2.Add(hitV);
                    }

                    h = -1;
                    k = i1;
                    while (true)
                    {
                        if (k != j2)
                        {
                            vec1.Add(vec[k]);
                        }
                        else
                        {
                            if (h < 0 || h >= n)
                            {
                                Err();
                            }

                            if (!IsOnSegment(v2.X, v2.Y, vec[h].X, vec[h].Y, p1.X, p1.Y))
                            {
                                vec1.Add(vec[k]);
                            }

                            break;
                        }

                        h = k;
                        if (k - 1 < 0)
                        {
                            k = n - 1;
                        }
                        else
                        {
                            k--;
                        }
                    }

                    vec1.Reverse();

                    h = -1;
                    k = i2;
                    while (true)
                    {
                        if (k != j1)
                        {
                            vec2.Add(vec[k]);
                        }
                        else
                        {
                            if (h < 0 || h >= n)
                            {
                                Err();
                            }

                            if (k == j1
                                && !IsOnSegment(v1.X, v1.Y, vec[h].X, vec[h].Y, p2.X, p2.Y))
                            {
                                vec2.Add(vec[k]);
                            }

                            break;
                        }

                        h = k;
                        if (k + 1 > n - 1)
                        {
                            k = 0;
                        }
                        else
                        {
                            k++;
                        }
                    }

                    queue.Add(vec1);
                    queue.Add(vec2);
                    queue.RemoveAt(0);

                    break;
                }

                if (isConvex)
                {
                    figsVec.Add(queue[0]);
                    queue.RemoveAt(0);
                }
            }

            return figsVec;
        }

        private static Vector2? HitRay(
        float x1,
        float y1,
        float x2,
        float y2,
        float x3,
        float y3,
        float x4,
        float y4)
        {
            float t1 = x3 - x1,
                    t2 = y3 - y1,
                    t3 = x2 - x1,
                    t4 = y2 - y1,
                    t5 = x4 - x3,
                    t6 = y4 - y3,
                    t7 = t4 * t5 - t3 * t6,
                    a;
            if (Math.Abs(t7) > 0)
            {
                a = (t5 * t2 - t6 * t1) / t7;
            }
            else
            {
                a = 0;
            }

            float px = x1 + a * t3, py = y1 + a * t4;
            var b1 = IsOnSegment(x2, y2, x1, y1, px, py);
            var b2 = IsOnSegment(px, py, x3, y3, x4, y4);

            if (b1 && b2)
            {
                return new Vector2(px, py);
            }

            return null;
        }

        private static Vector2? HitSegment(
        float x1,
        float y1,
        float x2,
        float y2,
        float x3,
        float y3,
        float x4,
        float y4)
        {
            float t1 = x3 - x1,
                    t2 = y3 - y1,
                    t3 = x2 - x1,
                    t4 = y2 - y1,
                    t5 = x4 - x3,
                    t6 = y4 - y3,
                    t7 = t4 * t5 - t3 * t6;

            var a = (t5 * t2 - t6 * t1) / t7;
            float px = x1 + a * t3, py = y1 + a * t4;
            var b1 = IsOnSegment(px, py, x1, y1, x2, y2);
            var b2 = IsOnSegment(px, py, x3, y3, x4, y4);

            if (b1 && b2)
            {
                return new Vector2(px, py);
            }

            return null;
        }

        private static bool IsOnSegment(float px, float py, float x1, float y1, float x2, float y2)
        {
            var b1 = ((x1 + 0.1 >= px && px >= x2 - 0.1) || (x1 - 0.1 <= px && px <= x2 + 0.1));
            var b2 = ((y1 + 0.1 >= py && py >= y2 - 0.1) || (y1 - 0.1 <= py && py <= y2 + 0.1));
            return (b1 && b2 && IsOnLine(px, py, x1, y1, x2, y2));
        }

        private static bool PointsMatch(float x1, float y1, float x2, float y2)
        {
            float dx = (x2 >= x1? x2 - x1 : x1 - x2), dy = (y2 >= y1? y2 - y1 : y1 - y2);
            return (dx < 0.1 && dy < 0.1);
        }

        private static bool IsOnLine(float px, float py, float x1, float y1, float x2, float y2)
        {
            if (x2 - x1 > 0.1 || x1 - x2 > 0.1)
            {
                float a = (y2 - y1) / (x2 - x1),
                        possibleY = a * (px - x1) + y1,
                        diff = (possibleY > py? possibleY - py : py - possibleY);
                return (diff < 0.1);
            }

            return (px - x1 < 0.1 || x1 - px < 0.1);
        }

        private static float Det(float x1, float y1, float x2, float y2, float x3, float y3)
        {
            return x1 * y2 + x2 * y3 + x3 * y1 - y1 * x2 - y2 * x3 - y3 * x1;
        }

        private static void Err()
        {
            throw new Exception("A problem has occurred. Use the Validate() method to see where the problem is.");
        }
    }
}