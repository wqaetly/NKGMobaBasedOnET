using UnityEngine;

namespace ETModel
{
    [Event(EventIdType.CheckForUpdateBegin)]
    public class CheckForUpdateBeginEvent_CreateCheckForUpdateUI : AEvent
    {
        public override void Run()
        {
            UI ui = CheckForUpdateUIFactory.Create();
			Game.Scene.GetComponent<UIComponent>().Add(ui);
        }
    }
}
