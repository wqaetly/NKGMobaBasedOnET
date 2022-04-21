using UnityEngine;
using System.Linq;
using Werewolf.StatusIndicators.Services;
using System.Collections;

namespace Werewolf.StatusIndicators.Components
{
    public class AngleMissile: SpellIndicator
    {
        public override void Update()
        {
            Vector3 v = FlattenVector(SplatManager.Get3DMousePosition()) - this.transform.position;
            if (v != Vector3.zero)
            {
                this.transform.rotation = Quaternion.LookRotation(v);
            }
        }
    }
}