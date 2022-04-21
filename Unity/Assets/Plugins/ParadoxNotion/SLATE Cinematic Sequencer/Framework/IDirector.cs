using UnityEngine;
using System.Collections.Generic;

namespace Slate
{

    ///Interface for an IDirectable player. e.g. the Cutscene component
    ///This is used for IDirectables to interface with their root
    public interface IDirector
    {
        IEnumerable<IDirectable> children { get; }
        GameObject context { get; }
        float length { get; }
        float currentTime { get; set; }
        float previousTime { get; }
        float playbackSpeed { get; set; }
        bool isActive { get; }
        bool isPaused { get; }
        bool isReSampleFrame { get; }
        IEnumerable<GameObject> GetAffectedActors();
        void Play();
        void Pause();
        void Stop();
        void Sample(float time);
        void ReSample();
        void Validate();
        void SendGlobalMessage(string message, object value);
    }
}