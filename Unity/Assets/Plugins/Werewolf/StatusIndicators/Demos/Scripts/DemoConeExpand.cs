using UnityEngine;
using Werewolf.StatusIndicators.Components;

namespace Werewolf.StatusIndicators.Demo {

  public class DemoConeExpand : MonoBehaviour {
    private Cone spellIndicator;

    void Start() {
      spellIndicator = GetComponent<Cone>();
    }

    void Update() {
      spellIndicator.Angle = Mathf.PingPong(Time.time * 100f, 320f) + 40f;
    }
  }
}