#if UNITY_EDITOR
namespace Sirenix.OdinInspector.Demos
{
    using System;
    using UnityEngine;

    public class ToggleExamples : MonoBehaviour
    {
        // Simple Toggle Group
        [ToggleGroup("MyToggle")]
        public bool MyToggle;

        [ToggleGroup("MyToggle")]
        public float A;

        [ToggleGroup("MyToggle")]
        [HideLabel, Multiline]
        public string B;

        // Toggle for custom data.
        [ToggleGroup("EnableGroupOne", "$GroupOneTitle")]
        public bool EnableGroupOne;

        [ToggleGroup("EnableGroupOne")]
        public string GroupOneTitle = "One";

        [ToggleGroup("EnableGroupOne")]
        public float GroupOneA;

        [ToggleGroup("EnableGroupOne")]
        public float GroupOneB;

        // Toggle for individual objects.
        [Toggle("Enabled")]
        public MyToggleObject Three = new MyToggleObject();

        [Toggle("Enabled")]
        public MyToggleA Four = new MyToggleA();

        [Toggle("Enabled")]
        public MyToggleB Five = new MyToggleB();

        public MyToggleC[] ToggleList;
    }

    [Serializable]
    public class MyToggleObject
    {
        public bool Enabled;

        [HideInInspector]
        public string Title;

        public int A;
        public int B;
    }

    [Serializable]
    public class MyToggleA : MyToggleObject
    {
        public float C;
        public float D;
        public float F;
    }

    [Serializable]
    public class MyToggleB : MyToggleObject
    {
        public string Text;
    }

    [Serializable]
    public class MyToggleC
    {
        [ToggleGroup("Enabled", "$Label")]
        public bool Enabled;

        public string Label { get { return this.Test.ToString(); } }

        [ToggleGroup("Enabled")]
        public float Test;
    }
}
#endif
