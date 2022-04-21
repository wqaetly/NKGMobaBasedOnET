using UnityEngine;
using System.Collections;

namespace Werewolf.StatusIndicators.Effects {
  public class FixedRotation : MonoBehaviour {
    public Vector3 Rotation;

    void LateUpdate() {
      transform.eulerAngles = Rotation;
    }
  }
}