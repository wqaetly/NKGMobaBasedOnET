using System.Collections.Generic;
using PF;
using UnityEngine;

namespace ETModel
{
    public sealed class Unit: Entity
    {
        public Vector3 Position { get; set; }

        public Quaternion Rotation { get; set; }

        public override void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }

            base.Dispose();
        }
    }
}