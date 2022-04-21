/* Based on the free 'Bezier Curve Editor' by 'Arkham Interactive' */

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Slate
{

    ///A Path defined out of BezierCurves
    [AddComponentMenu("SLATE/Path")]
    public class BezierPath : Path
    {

        [RangeAttribute(0, 100)]
        public int resolution = 30;
        public Color drawColor = Color.white;
        [SerializeField]
        private List<BezierPoint> _points = new List<BezierPoint>();

        private Vector3[] _sampledPathPoints;
        private float _length;
        private bool _closed;//not used right now

        public List<BezierPoint> points {
            get { return _points; }
        }

        public bool closed {
            get { return _closed; }
            set
            {
                if ( _closed != value ) {
                    _closed = value;
                    SetDirty();
                }
            }
        }

        public BezierPoint this[int index] {
            get { return points[index]; }
        }

        public int pointCount {
            get { return points.Count; }
        }

        public override float length {
            get { return _length; }
        }


        void Awake() { Compute(); }
        void OnValidate() { Compute(); }
        public override void Compute() {
            ComputeSampledPathPoints();
            ComputeLength();
        }

        public void SetDirty() {
#if UNITY_EDITOR
            UnityEditor.Undo.RecordObject(this, "Path Change");
#endif

            Compute();

#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(this);
#endif
        }

        void ComputeLength() {
            _length = GetLength(_sampledPathPoints);
        }

        void ComputeSampledPathPoints() {
            if ( points.Count == 0 ) {
                _sampledPathPoints = new Vector3[0];
                return;
            }

            var result = new List<Vector3>();
            for ( int i = 0; i < points.Count - 1; i++ ) {
                var current = points[i];
                var next = points[i + 1];
                result.AddRange(GetSampledPathPoints(current, next, resolution));
            }

            _sampledPathPoints = result.ToArray();
        }



        ///Create a new BezierPath object
        public static BezierPath Create(Transform targetParent = null) {
            var rootName = "[ PATHS ]";
            GameObject root = null;
            if ( targetParent == null ) {
                root = GameObject.Find(rootName);
                if ( root == null ) {
                    root = new GameObject(rootName);
                }
            } else {
                var child = targetParent.Find(rootName);
                if ( child != null ) {
                    root = child.gameObject;
                } else {
                    root = new GameObject(rootName);
                }
            }
            root.transform.SetParent(targetParent, false);

            var path = new GameObject("Path").AddComponent<BezierPath>();
            path.transform.SetParent(root.transform, false);
            path.transform.localPosition = Vector3.zero;
            path.transform.localRotation = Quaternion.identity;
            return path;
        }

        ///Add a new bezier point at index.
        public BezierPoint AddPointAt(Vector3 position, int index = -1) {
            var newPoint = new BezierPoint(this, position);
            if ( index == -1 ) {
                points.Add(newPoint);
            } else {
                points.Insert(index, newPoint);
            }
            SetDirty();
            return newPoint;
        }

        ///Remove a bezier point.
        public void RemovePoint(BezierPoint point) {
            points.Remove(point);
            SetDirty();
        }

        ///Get a bezier point index.
        public int GetPointIndex(BezierPoint point) {
            int result = -1;
            for ( int i = 0; i < points.Count; i++ ) {
                if ( points[i] == point ) {
                    result = i;
                    break;
                }
            }

            return result;
        }

        ///Get interpolated Vector3 positions between two control points with specified resolution
        public static Vector3[] GetSampledPathPoints(BezierPoint p1, BezierPoint p2, int resolution) {
            var result = new List<Vector3>();
            int limit = resolution + 1;
            float _res = resolution;

            for ( int i = 1; i < limit; i++ ) {
                var currentPoint = GetPoint(p1, p2, i / _res);
                result.Add(currentPoint);
            }

            return result.ToArray();
        }

        ///Get a uniform Vector3 position along the path at normalized length t.
        public override Vector3 GetPointAt(float t) {
            if ( t <= 0 ) return points[0].position;
            if ( t >= 1 ) return points[points.Count - 1].position;
            return GetPoint(t, _sampledPathPoints);
        }

        ///Get approximate Vector3 position between two control points at normalized t.
        public static Vector3 GetPoint(BezierPoint p1, BezierPoint p2, float t) {
            if ( p1.handle2 != Vector3.zero ) {
                if ( p2.handle1 != Vector3.zero ) return GetCubicCurvePoint(p1.position, p1.globalHandle2, p2.globalHandle1, p2.position, t);
                else return GetQuadraticCurvePoint(p1.position, p1.globalHandle2, p2.position, t);
            } else {
                if ( p2.handle1 != Vector3.zero ) return GetQuadraticCurvePoint(p1.position, p2.globalHandle1, p2.position, t);
                else return GetLinearPoint(p1.position, p2.position, t);
            }
        }


        ///EDITOR
#if UNITY_EDITOR
        void Reset() {
            var p1 = AddPointAt(transform.position + new Vector3(-3, 0, 0));
            p1.globalHandle1 = new Vector3(-3, 0, -1);
            var p2 = AddPointAt(transform.position + new Vector3(3, 0, 0));
            p2.globalHandle1 = new Vector3(3, 0, 1);
            SetDirty();
        }


        void OnDrawGizmos() {
            if ( points.Count > 1 ) {
                for ( int i = 0; i < points.Count - 1; i++ ) {
                    Gizmos.color = drawColor;
                    DrawPath(points[i], points[i + 1], resolution);
                    Gizmos.color = Color.black;
                    DrawVerticalGuide(points[i].position);
                }

                DrawVerticalGuide(points[points.Count - 1].position);

                if ( closed ) DrawPath(points[points.Count - 1], points[0], resolution);
            }
            Gizmos.color = Color.white;
        }

        ///Draw vertical guides
        public static void DrawVerticalGuide(Vector3 position) {
            var hit = new RaycastHit();
            Vector3 hitPos = new Vector3(position.x, 0, position.z);
            if ( Physics.Linecast(position, position - new Vector3(0, 100, 0), out hit) ) {
                hitPos = hit.point;
            }

            Gizmos.DrawLine(position, hitPos);
        }

        ///Draw the path
        public static void DrawPath(BezierPoint p1, BezierPoint p2, int resolution) {
            int limit = resolution + 1;
            float _res = resolution;
            var lastPoint = p1.position;
            var currentPoint = Vector3.zero;

            for ( int i = 1; i < limit; i++ ) {
                currentPoint = GetPoint(p1, p2, i / _res);
                Gizmos.DrawLine(lastPoint, currentPoint);
                lastPoint = currentPoint;
            }
        }
#endif

    }
}