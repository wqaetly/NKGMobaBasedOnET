using System.Collections;
using System.Linq;
using UnityEngine;
using Werewolf.StatusIndicators.Services;

namespace Werewolf.StatusIndicators.Components
{
    /// <summary>
    /// Apply this to the GameObject which holds all your Splats. Make sure the origin is correctly centered at the base of the Character.
    /// </summary>
    public class SplatManager
    {
        /// <summary>
        /// Finds the mouse position from the screen point to the 3D world.
        /// </summary>
        public static Vector3 Get3DMousePosition()
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 300.0f))
                return hit.point;
            else
                return Vector3.zero;
        }
    }
}