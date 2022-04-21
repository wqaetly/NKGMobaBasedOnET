/* Based on the free Bezier Curve Editor by Arkham Interactive */

using UnityEngine;
using System;
using System.Collections;

namespace Slate
{

    [Serializable]
    ///A point used within a BezierPath
    public class BezierPoint
    {

        public enum HandleStyle
        {
            Connected,
            Broken,
            None
        }

        [SerializeField, ReadOnly]
        private BezierPath _path;
        [SerializeField]
        public Vector3 _position;
        [SerializeField]
        private Vector3 _handle1;
        [SerializeField]
        private Vector3 _handle2;
        [SerializeField]
        private HandleStyle _handleStyle;


        public BezierPoint(BezierPath path, Vector3 position) {
            this.path = path;
            this.position = position;
        }


        public HandleStyle handleStyle {
            get { return _handleStyle; }
            set { _handleStyle = value; }
        }

        public BezierPath path {
            get { return _path; }
            set { _path = value; }
        }

        public Vector3 position {
            get { return path.transform.TransformPoint(_position); }
            set { _position = path.transform.InverseTransformPoint(value); }
        }

        public Vector3 handle1 {
            get { return _handle1; }
            private set
            {
                if ( _handle1 != value ) {
                    _handle1 = value;
                    if ( handleStyle == HandleStyle.None ) handleStyle = HandleStyle.Broken;
                    else if ( handleStyle == HandleStyle.Connected ) _handle2 = -value;
                    path.SetDirty();
                }
            }
        }

        public Vector3 globalHandle1 {
            get { return path.transform.TransformPoint(_position + handle1); }
            set { handle1 = path.transform.InverseTransformPoint(value) - _position; }
        }

        public Vector3 handle2 {
            get { return _handle2; }
            private set
            {
                if ( _handle2 != value ) {
                    _handle2 = value;
                    if ( handleStyle == HandleStyle.None ) handleStyle = HandleStyle.Broken;
                    else if ( handleStyle == HandleStyle.Connected ) _handle1 = -value;
                    path.SetDirty();
                }
            }
        }

        public Vector3 globalHandle2 {
            get { return path.transform.TransformPoint(_position + handle2); }
            set { handle2 = path.transform.InverseTransformPoint(value) - _position; }
        }
    }
}