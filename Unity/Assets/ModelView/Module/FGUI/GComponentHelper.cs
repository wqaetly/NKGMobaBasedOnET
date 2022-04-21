using FairyGUI;
using UnityEngine;

namespace ET
{
    public static class GComponentHelper
    {
        /// <summary>
        /// 获取组件包围盒
        /// </summary>
        /// <returns>x:最小值 y:最小值 w:宽 h:高</returns>
        public static Rect GetRect(GComponent gcom)
        {
            if(gcom == null)
            {
                return Rect.zero;
            }

            float ax = 0f;
            float ay = 0f;
            float aw = 0f;
            float ah = 0f;

            GObject[] childs = new GObject[4];
            GObject maxDisschild = null;
            float diss = 0f;

            int cnt = gcom.numChildren;

            if (gcom.numChildren > 0)
            {
                ax = int.MaxValue;
                ay = int.MaxValue;

                float ar = int.MinValue;
                float ab = int.MinValue;

                float tmp;

                for (int i = 0; i < cnt; ++i)
                {
                    GObject child = gcom.GetChildAt(i);

                    tmp = child.xMin;

                    if (tmp < ax)
                    {
                        ax = tmp;
                        childs[0] = child;
                    }

                    tmp = child.yMin;

                    if (tmp < ay)
                    {
                        ay = tmp;
                        childs[1] = child;
                    }

                    tmp = child.xMin + child.actualWidth;

                    if (tmp > ar)
                    {
                        ar = tmp;
                        childs[2] = child;
                    }

                    tmp = child.yMin + child.actualWidth;

                    if (tmp > ab)
                    {
                        ab = tmp;
                        childs[3] = child;
                    }                   
                }

                for (int i = 0; i < childs.Length; ++i)
                {
                    if (diss <= childRadiusDiss(childs[i]))
                    {
                        diss = childRadiusDiss(childs[i]);
                        maxDisschild = childs[i];
                    }
                }

                var ratio = Mathf.Max(maxDisschild.actualWidth, maxDisschild.height) / Mathf.Min(maxDisschild.actualWidth, maxDisschild.actualHeight);

                diss = diss * ratio;

                aw = ar - ax + diss;
                ah = ab - ay + diss;

                ax = ax - diss * 0.5f;
                ay = ay - diss * 0.5f;

                return new Rect(ax, ay, aw, ah);

            }

            return new Rect(gcom.x, gcom.y, gcom.actualWidth, gcom.actualHeight);
        }

        private static float childRadiusDiss(GObject child)
        {
            var radius = Mathf.Sqrt(child.actualWidth * child.actualWidth + child.actualHeight * child.actualHeight) * 0.5f;

            var diss = radius * Mathf.Abs(Mathf.Sin(child.rotation));

            return diss;
        }
    }    
}
