using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Slate
{

    public static class RectUtility
    {

        //Get a rect that encapsulates all provided rects
        public static Rect GetBoundRect(params Rect[] rects) {
            var xMin = float.PositiveInfinity;
            var xMax = float.NegativeInfinity;
            var yMin = float.PositiveInfinity;
            var yMax = float.NegativeInfinity;

            for ( var i = 0; i < rects.Length; i++ ) {
                xMin = Mathf.Min(xMin, rects[i].xMin);
                xMax = Mathf.Max(xMax, rects[i].xMax);
                yMin = Mathf.Min(yMin, rects[i].yMin);
                yMax = Mathf.Max(yMax, rects[i].yMax);
            }

            return Rect.MinMaxRect(xMin, yMin, xMax, yMax);
        }

        //Get a rect that encapsulates all provided positions
        public static Rect GetBoundRect(params Vector2[] positions) {
            var xMin = float.PositiveInfinity;
            var xMax = float.NegativeInfinity;
            var yMin = float.PositiveInfinity;
            var yMax = float.NegativeInfinity;

            for ( var i = 0; i < positions.Length; i++ ) {
                xMin = Mathf.Min(xMin, positions[i].x);
                xMax = Mathf.Max(xMax, positions[i].x);
                yMin = Mathf.Min(yMin, positions[i].y);
                yMax = Mathf.Max(yMax, positions[i].y);
            }

            return Rect.MinMaxRect(xMin, yMin, xMax, yMax);
        }

        ///Rect a fully encapsulated b?
        public static bool Encapsulates(this Rect a, Rect b) {
            if ( a == default(Rect) || b == default(Rect) ) { return false; }
            return a.x < b.x && a.xMax > b.xMax && a.y < b.y && a.yMax > b.yMax;
        }

        ///Expands rect by margin
        public static Rect ExpandBy(this Rect rect, float margin) {
            return rect.ExpandBy(margin, margin);
        }

        public static Rect ExpandBy(this Rect rect, float xMargin, float yMargin) {
            return Rect.MinMaxRect(rect.xMin - xMargin, rect.yMin - yMargin, rect.xMax + xMargin, rect.yMax + yMargin);
        }

        //Transforms rect from one container to another container rect
        public static Rect TransformSpace(this Rect rect, Rect oldContainer, Rect newContainer) {
            var result = new Rect();
            result.xMin = Mathf.Lerp(newContainer.xMin, newContainer.xMax, Mathf.InverseLerp(oldContainer.xMin, oldContainer.xMax, rect.xMin));
            result.xMax = Mathf.Lerp(newContainer.xMin, newContainer.xMax, Mathf.InverseLerp(oldContainer.xMin, oldContainer.xMax, rect.xMax));
            result.yMin = Mathf.Lerp(newContainer.yMin, newContainer.yMax, Mathf.InverseLerp(oldContainer.yMin, oldContainer.yMax, rect.yMin));
            result.yMax = Mathf.Lerp(newContainer.yMin, newContainer.yMax, Mathf.InverseLerp(oldContainer.yMin, oldContainer.yMax, rect.yMax));
            return result;
        }

        ///----------------------------------------------------------------------------------------------

        ///Get a screen space rect from camera, out of bounds provided
        public static Rect ToViewRect(this Bounds b, Camera cam) {

            var distance = cam.WorldToViewportPoint(b.center).z;
            //The object is behind us
            if ( distance < 0 ) {
                return new Rect();
            }

            //All 8 vertices of the bounds
            var pts = new Vector2[8];
            pts[0] = cam.WorldToViewportPoint(new Vector3(b.center.x + b.extents.x, b.center.y + b.extents.y, b.center.z + b.extents.z));
            pts[1] = cam.WorldToViewportPoint(new Vector3(b.center.x + b.extents.x, b.center.y + b.extents.y, b.center.z - b.extents.z));
            pts[2] = cam.WorldToViewportPoint(new Vector3(b.center.x + b.extents.x, b.center.y - b.extents.y, b.center.z + b.extents.z));
            pts[3] = cam.WorldToViewportPoint(new Vector3(b.center.x + b.extents.x, b.center.y - b.extents.y, b.center.z - b.extents.z));
            pts[4] = cam.WorldToViewportPoint(new Vector3(b.center.x - b.extents.x, b.center.y + b.extents.y, b.center.z + b.extents.z));
            pts[5] = cam.WorldToViewportPoint(new Vector3(b.center.x - b.extents.x, b.center.y + b.extents.y, b.center.z - b.extents.z));
            pts[6] = cam.WorldToViewportPoint(new Vector3(b.center.x - b.extents.x, b.center.y - b.extents.y, b.center.z + b.extents.z));
            pts[7] = cam.WorldToViewportPoint(new Vector3(b.center.x - b.extents.x, b.center.y - b.extents.y, b.center.z - b.extents.z));

            // Calculate the min and max positions
            var min = new Vector2(float.PositiveInfinity, float.PositiveInfinity);
            var max = new Vector2(float.NegativeInfinity, float.NegativeInfinity);
            for ( var i = 0; i < pts.Length; i++ ) {
                // Get them in GUI space
                pts[i].y = 1 - pts[i].y;
                min = Vector2.Min(min, pts[i]);
                max = Vector2.Max(max, pts[i]);
            }

            return Rect.MinMaxRect(min.x, min.y, max.x, max.y);
        }
    }
}