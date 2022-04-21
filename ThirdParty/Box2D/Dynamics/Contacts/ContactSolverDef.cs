using System;

namespace Box2DSharp.Dynamics.Contacts
{
    public ref struct ContactSolverDef
    {
        public readonly TimeStep Step;

        public readonly int ContactCount;

        public readonly Contact[] Contacts;

        public readonly Position[] Positions;

        public readonly Velocity[] Velocities;

        public ContactSolverDef(in TimeStep step, int contactCount, Contact[] contacts, Position[] positions, Velocity[] velocities)
        {
            Step = step;
            Contacts = contacts;
            ContactCount = contactCount;
            Positions = positions;
            Velocities = velocities;
        }
    }
}