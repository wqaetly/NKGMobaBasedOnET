using System;

namespace ET
{
    public static class GlobalLifeCycle
    {
        public static Action StartAction;
		
        public static Action UpdateAction;
        
        public static Action FixedUpdateAction;
		
        public static Action LateUpdateAction;

        public static Action FrameFinishAction;
		
        public static Action OnApplicationQuitAction;
    }
}
