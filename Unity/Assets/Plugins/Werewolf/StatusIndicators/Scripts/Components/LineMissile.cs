using UnityEngine;
using System.Linq;
using Werewolf.StatusIndicators.Services;
using System.Collections;

namespace Werewolf.StatusIndicators.Components
{
    public class LineMissile: SpellIndicator
    {
        // Fields
        public GameObject ArrowHead;
        public float MinimumRange;
        
        // public override void Update()
        // {
        //     if (Manager != null)
        //     {
        //         Vector3 v = FlattenVector(Manager.Get3DMousePosition()) - Manager.transform.position;
        //         if (v != Vector3.zero)
        //         {
        //             Manager.transform.rotation = Quaternion.LookRotation(v);
        //         }
        //
        //         Scale =
        //                 Mathf.Clamp((Manager.Get3DMousePosition() - Manager.transform.position).magnitude, MinimumRange,
        //                     Range) * 2;
        //         ArrowHead.transform.localPosition = new Vector3(0, (Scale * 0.5f)  - 0.12f, 0);
        //     }
        // }
        
    }
}