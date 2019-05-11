using System;

namespace Box2DSharp.Common
{
    [Flags]
    public enum DrawFlag
    {
        /// <summary>
        /// draw shapes
        /// </summary>
        DrawShape = 1 << 0,

        /// <summary>
        /// draw joint connections
        /// </summary>
        DrawJoint = 1 << 1,

        /// <summary>
        /// draw axis aligned bounding boxes
        /// </summary>
        DrawAABB = 1 << 2,

        /// <summary>
        /// draw broad-phase pairs
        /// </summary>
        DrawPair = 1 << 3,

        /// <summary>
        /// draw center of mass frame
        /// </summary>
        DrawCenterOfMass = 1 << 4,

        /// <summary>
        /// draw body contact point
        /// </summary>
        DrawContactPoint = 1 << 5
    }
}