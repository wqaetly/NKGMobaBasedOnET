namespace ET
{
    namespace EventType
    {
        public struct CancelMoveFromFSM
        {
            public Unit Unit;
        }

        public struct CancelAttackFromFSM
        {
            public Unit Unit;
            public bool ResetAttackTarget;
        }
    }
}