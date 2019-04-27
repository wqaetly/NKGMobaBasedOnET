using System;
using FairyGUI;

namespace ETModel
{
    public class GWindow : Window
    {
        private Action doShowAnimationEvent;
        
        public event Action DoShowAnimationEvent
        {
            add
            {
                doShowAnimationEvent += value;
            }
            remove
            {
                doShowAnimationEvent -= value;
            }
        }
        
        protected override void DoShowAnimation()
        {
            base.DoShowAnimation();
            doShowAnimationEvent?.Invoke();
        }
        
        private Action doHideAnimationEvent;
        
        public event Action DoHideAnimationEvent
        {
            add
            {
                doHideAnimationEvent += value;
            }
            remove
            {
                doHideAnimationEvent -= value;
            }
        }
        
        protected override void DoHideAnimation()
        {
            base.DoHideAnimation();
            doHideAnimationEvent?.Invoke();
        }
        
        private Action onShownEvent;
        
        public event Action OnShownEvent
        {
            add
            {
                onShownEvent += value;
            }
            remove
            {
                onShownEvent -= value;
            }
        }
        
        protected override void OnShown()
        {
            base.OnShown();
            onShownEvent?.Invoke();
        }
        
        private Action onHideEvent;
        
        public event Action OnHideEvent
        {
            add
            {
                onHideEvent += value;
            }
            remove
            {
                onHideEvent -= value;
            }
        }
        
        protected override void OnHide()
        {
            base.OnHide();
            onHideEvent?.Invoke();
        }
    }
}