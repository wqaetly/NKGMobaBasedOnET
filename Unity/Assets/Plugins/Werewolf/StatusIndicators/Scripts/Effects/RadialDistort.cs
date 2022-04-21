using UnityEngine;
using System.Collections;

namespace Werewolf.StatusIndicators.Effects {
  public class RadialDistort : MonoBehaviour {
    public float Speed;
    private Material Material;

    void Start() {
      Material = GetComponent<Renderer>().material;
    }

    void Update() {
      Material.SetFloat("_Offset", Mathf.Repeat(Time.time * Speed, 1));
    }
  }
}