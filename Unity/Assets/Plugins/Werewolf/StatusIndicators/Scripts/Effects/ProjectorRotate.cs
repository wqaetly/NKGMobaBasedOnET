using UnityEngine;
using System.Collections;

namespace Werewolf.StatusIndicators.Effects {
  public class ProjectorRotate : MonoBehaviour {
    public float RotationSpeed;

    void LateUpdate() {
      transform.eulerAngles = new Vector3(0, Mathf.Repeat(Time.time * RotationSpeed, 360), 0);
    }
  }
}