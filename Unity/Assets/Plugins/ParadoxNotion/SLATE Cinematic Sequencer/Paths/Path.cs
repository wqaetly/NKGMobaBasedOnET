using UnityEngine;
using System.Collections;

namespace Slate
{

    ///An abstract base type for all Path types
    abstract public class Path : MonoBehaviour
    {

        abstract public float length { get; }
        abstract public Vector3 GetPointAt(float t);
        abstract public void Compute();

        public static Vector3 GetCubicCurvePoint(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4, float t) {
            t = Mathf.Clamp01(t);
            var part1 = Mathf.Pow(1 - t, 3) * p1;
            var part2 = 3 * Mathf.Pow(1 - t, 2) * t * p2;
            var part3 = 3 * ( 1 - t ) * Mathf.Pow(t, 2) * p3;
            var part4 = Mathf.Pow(t, 3) * p4;
            return part1 + part2 + part3 + part4;
        }


        public static Vector3 GetQuadraticCurvePoint(Vector3 p1, Vector3 p2, Vector3 p3, float t) {
            t = Mathf.Clamp01(t);
            var part1 = Mathf.Pow(1 - t, 2) * p1;
            var part2 = 2 * ( 1 - t ) * t * p2;
            var part3 = Mathf.Pow(t, 2) * p3;
            return part1 + part2 + part3;
        }

        public static Vector3 GetLinearPoint(Vector3 p1, Vector3 p2, float t) {
            return p1 + ( ( p2 - p1 ) * t );
        }

        public static Vector3 GetPoint(float t, params Vector3[] path) {
            if ( t <= 0 ) return path[0];
            if ( t >= 1 ) return path[path.Length - 1];
            var a = Vector3.zero;
            var b = Vector3.zero;
            var total = 0f;
            var segmentDistance = 0f;
            var pathLength = GetLength(path);
            for ( var i = 0; i < path.Length - 1; i++ ) {
                segmentDistance = Vector3.Distance(path[i], path[i + 1]) / pathLength;
                if ( total + segmentDistance > t ) {
                    a = path[i];
                    b = path[i + 1];
                    break;
                } else {
                    total += segmentDistance;
                }
            }
            t -= total;
            return Vector3.Lerp(a, b, t / segmentDistance);
        }

        public static float GetLength(Vector3[] path) {
            if ( path == null ) {
                return 0;
            }

            var dist = 0f;
            for ( var i = 0; i < path.Length; i++ ) {
                dist += Vector3.Distance(path[i], path[i == path.Length - 1 ? i : i + 1]);
            }
            return dist;
        }
    }
}