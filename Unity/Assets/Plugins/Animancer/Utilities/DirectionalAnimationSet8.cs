// Animancer // Copyright 2019 Kybernetik //

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Animancer
{
    /// <summary>
    /// A set of up/down/left/right animations with diagonals as well.
    /// </summary>
    [CreateAssetMenu(menuName = "Animancer/Directional Animation Set 8", order = AnimancerComponent.AssetMenuOrder + 3)]
    public class DirectionalAnimationSet8 : DirectionalAnimationSet
    {
        /************************************************************************************************************************/

        [SerializeField]
        private AnimationClip _UpRight;

        /// <summary>The animation facing diagonally up-right.</summary>
        public AnimationClip UpRight { get { return _UpRight; } }

        /// <summary>Sets the <see cref="UpRight"/> animation.</summary>
        /// <remarks>This is not simply a property setter because the animations will usually not need to be changed by scripts.</remarks>
        public void SetUpRight(AnimationClip clip)
        {
            _UpRight = clip;
            AnimancerUtilities.SetDirty(this);
        }

        /************************************************************************************************************************/

        [SerializeField]
        private AnimationClip _DownRight;

        /// <summary>The animation facing diagonally down-right.</summary>
        public AnimationClip DownRight { get { return _DownRight; } }

        /// <summary>Sets the <see cref="DownRight"/> animation.</summary>
        /// <remarks>This is not simply a property setter because the animations will usually not need to be changed by scripts.</remarks>
        public void SetDownRight(AnimationClip clip)
        {
            _DownRight = clip;
            AnimancerUtilities.SetDirty(this);
        }

        /************************************************************************************************************************/

        [SerializeField]
        private AnimationClip _DownLeft;

        /// <summary>The animation facing diagonally down-left.</summary>
        public AnimationClip DownLeft { get { return _DownLeft; } }

        /// <summary>Sets the <see cref="DownLeft"/> animation.</summary>
        /// <remarks>This is not simply a property setter because the animations will usually not need to be changed by scripts.</remarks>
        public void SetDownLeft(AnimationClip clip)
        {
            _DownLeft = clip;
            AnimancerUtilities.SetDirty(this);
        }

        /************************************************************************************************************************/

        [SerializeField]
        private AnimationClip _UpLeft;

        /// <summary>The animation facing diagonally up-left.</summary>
        public AnimationClip UpLeft { get { return _UpLeft; } }

        /// <summary>Sets the <see cref="UpLeft"/> animation.</summary>
        /// <remarks>This is not simply a property setter because the animations will usually not need to be changed by scripts.</remarks>
        public void SetUpLeft(AnimationClip clip)
        {
            _UpLeft = clip;
            AnimancerUtilities.SetDirty(this);
        }

        /************************************************************************************************************************/

        /// <summary>Returns the animation closest to the specified 'direction'.</summary>
        public override AnimationClip GetClip(Vector2 direction)
        {
            var angle = Mathf.Atan2(direction.y, direction.x);
            var octant = Mathf.RoundToInt(8 * angle / (2 * Mathf.PI) + 8) % 8;
            switch (octant)
            {
                case 0: return Right;
                case 1: return _UpRight;
                case 2: return Up;
                case 3: return _UpLeft;
                case 4: return Left;
                case 5: return _DownLeft;
                case 6: return Down;
                case 7: return _DownRight;
                default: throw new ArgumentOutOfRangeException("Invalid octant");
            }
        }

        /************************************************************************************************************************/
        #region Directions
        /************************************************************************************************************************/

        /// <summary>The number of animations in this set.</summary>
        public override int ClipCount { get { return 8; } }

        /************************************************************************************************************************/

        /// <summary>Up, Down, Left Right, or their diagonals.</summary>
        public new enum Direction
        {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member.
            Up,
            Right,
            Down,
            Left,
            UpRight,
            DownRight,
            DownLeft,
            UpLeft,
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member.
        }

        /************************************************************************************************************************/

        /// <summary>Returns the name of the specified 'direction'.</summary>
        protected override string GetDirectionName(int direction) { return ((Direction)direction).ToString(); }

        /************************************************************************************************************************/

        /// <summary>Returns the animation associated with the specified 'direction'.</summary>
        public AnimationClip GetClip(Direction direction)
        {
            switch (direction)
            {
                case Direction.Up: return Up;
                case Direction.Right: return Right;
                case Direction.Down: return Down;
                case Direction.Left: return Left;
                case Direction.UpRight: return _UpRight;
                case Direction.DownRight: return _DownRight;
                case Direction.DownLeft: return _DownLeft;
                case Direction.UpLeft: return _UpLeft;
                default: throw new ArgumentException("Unhandled direction: " + direction);
            }
        }

        /// <summary>Returns the animation associated with the specified 'direction'.</summary>
        public override AnimationClip GetClip(int direction)
        {
            return GetClip((Direction)direction);
        }

        /************************************************************************************************************************/

        /// <summary>Sets the animation associated with the specified 'direction'.</summary>
        public void SetClip(Direction direction, AnimationClip clip)
        {
            switch (direction)
            {
                case Direction.Up: SetUp(clip); break;
                case Direction.Right: SetRight(clip); break;
                case Direction.Down: SetDown(clip); break;
                case Direction.Left: SetLeft(clip); break;
                case Direction.UpRight: _UpRight = clip; break;
                case Direction.DownRight: _DownRight = clip; break;
                case Direction.DownLeft: _DownLeft = clip; break;
                case Direction.UpLeft: _UpLeft = clip; break;
                default: throw new ArgumentException("Unhandled direction: " + direction);
            }

            AnimancerUtilities.SetDirty(this);
        }

        /// <summary>Sets the animation associated with the specified 'direction'.</summary>
        public override void SetClip(int direction, AnimationClip clip)
        {
            SetClip((Direction)direction, clip);
        }

        /************************************************************************************************************************/

        /// <summary>Returns a vector representing the specified 'direction'.</summary>
        public static Vector2 DirectionToVector(Direction direction)
        {
            switch (direction)
            {
                case Direction.Up: return Vector2.up;
                case Direction.Right: return Vector2.right;
                case Direction.Down: return Vector2.down;
                case Direction.Left: return Vector2.left;
                case Direction.UpRight: return AnimancerUtilities.UpRight;
                case Direction.DownRight: return AnimancerUtilities.DownRight;
                case Direction.DownLeft: return AnimancerUtilities.DownLeft;
                case Direction.UpLeft: return AnimancerUtilities.UpLeft;
                default: throw new ArgumentException("Unhandled direction: " + direction);
            }
        }

        /// <summary>Returns a vector representing the specified 'direction'.</summary>
        public override Vector2 GetDirection(int direction)
        {
            return DirectionToVector((Direction)direction);
        }

        /************************************************************************************************************************/

        /// <summary>Returns the direction closest to the specified 'vector'.</summary>
        public new static Direction VectorToDirection(Vector2 vector)
        {
            var angle = Mathf.Atan2(vector.y, vector.x);
            var octant = Mathf.RoundToInt(8 * angle / (2 * Mathf.PI) + 8) % 8;
            switch (octant)
            {
                case 0: return Direction.Right;
                case 1: return Direction.UpRight;
                case 2: return Direction.Up;
                case 3: return Direction.UpLeft;
                case 4: return Direction.Left;
                case 5: return Direction.DownLeft;
                case 6: return Direction.Down;
                case 7: return Direction.DownRight;
                default: throw new ArgumentOutOfRangeException("Invalid octant");
            }
        }

        /************************************************************************************************************************/

        /// <summary>Returns a copy of the 'vector' pointing in the closest direction this set type has an animation for.</summary>
        public new static Vector2 SnapVectorToDirection(Vector2 vector)
        {
            var magnitude = vector.magnitude;
            var direction = VectorToDirection(vector);
            vector = DirectionToVector(direction) * magnitude;
            return vector;
        }

        /// <summary>Returns a copy of the 'vector' pointing in the closest direction this set has an animation for.</summary>
        public override Vector2 Snap(Vector2 vector)
        {
            return SnapVectorToDirection(vector);
        }

        /************************************************************************************************************************/
        #endregion
        /************************************************************************************************************************/
        #region Name Based Operations
        /************************************************************************************************************************/
#if UNITY_EDITOR
        /************************************************************************************************************************/

        /// <summary>
        /// Attempts to assign the 'clip' to one of this set's fields based on its name and returns the direction index
        /// of that field (or -1 if it was unable to determine the direction).
        /// </summary>
        public override int SetClipByName(AnimationClip clip)
        {
            var name = clip.name;

            var directionCount = ClipCount;
            for (int i = directionCount - 1; i >= 0; i--)
            {
                if (name.Contains(GetDirectionName(i)))
                {
                    SetClip(i, clip);
                    return i;
                }
            }

            return -1;
        }

        /************************************************************************************************************************/
#endif
        /************************************************************************************************************************/
        #endregion
        /************************************************************************************************************************/
    }

    /************************************************************************************************************************/

    public partial class AnimancerUtilities
    {
        /************************************************************************************************************************/

        /// <summary>1 / (Square Root of 2).</summary>
        public const float OneOverSqrt2 = 0.70710678118f;

        /// <summary>
        /// A vector with a magnitude of 1 pointing up to the right.
        /// <para></para>
        /// The value is approximately (0.707, 0.707).
        /// </summary>
        public static Vector2 UpRight { get { return new Vector2(OneOverSqrt2, OneOverSqrt2); } }

        /// <summary>
        /// A vector with a magnitude of 1 pointing down to the right.
        /// <para></para>
        /// The value is approximately (0.707, -0.707).
        /// </summary>
        public static Vector2 DownRight { get { return new Vector2(OneOverSqrt2, -OneOverSqrt2); } }

        /// <summary>
        /// A vector with a magnitude of 1 pointing down to the left.
        /// <para></para>
        /// The value is approximately (-0.707, -0.707).
        /// </summary>
        public static Vector2 DownLeft { get { return new Vector2(-OneOverSqrt2, -OneOverSqrt2); } }

        /// <summary>
        /// A vector with a magnitude of 1 pointing up to the left.
        /// <para></para>
        /// The value is approximately (-0.707, 0.707).
        /// </summary>
        public static Vector2 UpLeft { get { return new Vector2(-OneOverSqrt2, OneOverSqrt2); } }

        /************************************************************************************************************************/
    }
}
