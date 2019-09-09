// Animancer // Copyright 2019 Kybernetik //

using System;
using UnityEngine;

namespace Animancer
{
    /// <summary>
    /// An <see cref="AnimancerComponent"/> which uses a callback to react to animation events called "Event".
    /// </summary>
    [AddComponentMenu("Animancer/Eventful Animancer Component")]
    [HelpURL(AnimancerPlayable.APIDocumentationURL + "/EventfulAnimancerComponent")]
    public class EventfulAnimancerComponent : AnimancerComponent
    {
        /************************************************************************************************************************/

        /// <summary>A callback for Animation Events with the function name "Event".</summary>
        public AnimationEventReceiver onEvent;

        /// <summary>
        /// Constructs a new <see cref="EventfulAnimancerComponent"/> and sets the
        /// <see cref="AnimationEventReceiver.FunctionName"/> of <see cref="onEvent"/>.
        /// </summary>
        public EventfulAnimancerComponent()
        {
            onEvent.SetFunctionName("Event");
        }

        /// <summary>Called by Animation Events.</summary>
        private void Event(AnimationEvent animationEvent)
        {
            onEvent.HandleEvent(animationEvent);
        }

        /************************************************************************************************************************/
    }
}
