using UnityEngine;

namespace Slate
{

    ///An interface for TimePointers (since structs can't be abstract)
    public interface IDirectableTimePointer
    {
        IDirectable target { get; }
        float time { get; }
        void TriggerForward(float currentTime, float previousTime);
        void TriggerBackward(float currentTime, float previousTime);
        void Update(float currentTime, float previousTime);
    }

    ///----------------------------------------------------------------------------------------------

    ///Wraps the startTime of a group, track or clip (IDirectable) along with it's relevant execution
    public struct StartTimePointer : IDirectableTimePointer
    {

        private bool triggered;
        private float lastTargetStartTime;
        public IDirectable target { get; private set; }
        float IDirectableTimePointer.time { get { return target.startTime; } }

        public StartTimePointer(IDirectable target) {
            this.target = target;
            triggered = false;
            lastTargetStartTime = target.startTime;
        }

        //...
        void IDirectableTimePointer.TriggerForward(float currentTime, float previousTime) {
            if ( currentTime >= target.startTime ) {
                if ( !triggered ) {
                    triggered = true;
                    target.Enter();
                    target.Update(target.ToLocalTime(currentTime), 0);
                }
            }
        }

        //...
        void IDirectableTimePointer.Update(float currentTime, float previousTime) {

            //update target and try auto-key
            if ( currentTime >= target.startTime && currentTime < target.endTime && currentTime > 0 && currentTime < target.root.length ) {

                var deltaMoveClip = target.startTime - lastTargetStartTime;
                var localCurrentTime = target.ToLocalTime(currentTime);
                var localPreviousTime = target.ToLocalTime(previousTime + deltaMoveClip);

#if UNITY_EDITOR
                if ( target is IKeyable && localCurrentTime == localPreviousTime ) {
                    ( (IKeyable)target ).TryAutoKey(localCurrentTime);
                }
#endif

                target.Update(localCurrentTime, localPreviousTime);
                lastTargetStartTime = target.startTime;
            }

            //root updated callback
            target.RootUpdated(currentTime, previousTime);
        }

        //...
        void IDirectableTimePointer.TriggerBackward(float currentTime, float previousTime) {
            if ( currentTime < target.startTime || currentTime <= 0 ) {
                if ( triggered ) {
                    triggered = false;
                    target.Update(0, target.ToLocalTime(previousTime));
                    target.Reverse();
                }
            }
        }
    }

    ///----------------------------------------------------------------------------------------------

    ///Wraps the endTime of a group, track or clip (IDirectable) along with it's relevant execution
    public struct EndTimePointer : IDirectableTimePointer
    {

        private bool triggered;
        public IDirectable target { get; private set; }
        float IDirectableTimePointer.time { get { return target.endTime; } }

        public EndTimePointer(IDirectable target) {
            this.target = target;
            triggered = false;
        }

        //...
        void IDirectableTimePointer.TriggerForward(float currentTime, float previousTime) {
            if ( currentTime >= target.endTime || ( currentTime == target.root.length && target.startTime < target.root.length ) ) {
                if ( !triggered ) {
                    triggered = true;
                    target.Update(target.GetLength(), target.ToLocalTime(previousTime));
                    target.Exit();
                }
            }
        }

        //...
        void IDirectableTimePointer.Update(float currentTime, float previousTime) {
            //Update is/should never be called in TimeOutPointers
            throw new System.NotImplementedException();
        }

        //...
        void IDirectableTimePointer.TriggerBackward(float currentTime, float previousTime) {
            if ( ( currentTime < target.endTime || currentTime <= 0 ) && currentTime != target.root.length ) {
                if ( triggered ) {
                    triggered = false;
                    target.ReverseEnter();
                    target.Update(target.ToLocalTime(currentTime), target.GetLength());
                }
            }
        }
    }
}