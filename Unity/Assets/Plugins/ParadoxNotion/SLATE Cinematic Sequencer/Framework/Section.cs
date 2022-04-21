using UnityEngine;
using System.Collections;

namespace Slate
{

    ///Defines a section...
    [System.Serializable]
    public class Section
    {

        public enum ExitMode
        {
            Continue,
            Loop,
        }

        ///Default color of Sections
        public static readonly Color DEFAULT_COLOR = Color.black.WithAlpha(0.3f);

        [SerializeField]
        private string _UID;
        [SerializeField]
        private string _name;
        [SerializeField]
        private float _time;

        [SerializeField]
        private ExitMode _exitMode;
        [SerializeField]
        private int _loopCount;

        [SerializeField]
        private Color _color = DEFAULT_COLOR;
        [SerializeField]
        private bool _colorizeBackground;

        ///The current loop iteration if section is looping
        public int currentLoopIteration { get; private set; }

        ///Unique ID.
        public string UID {
            get { return _UID; }
            private set { _UID = value; }
        }

        ///The name of the section.
        public string name {
            get { return _name; }
            set { _name = value; }
        }

        ///It's time.
        public float time {
            get { return _time; }
            set { _time = value; }
        }

        ///What will happen when the section has reached it's end?
        public ExitMode exitMode {
            get { return _exitMode; }
            set { _exitMode = value; }
        }

        public int loopCount {
            get { return _loopCount; }
            set { _loopCount = value; }
        }

        ///Preferrence color.
        public Color color {
            get { return _color.a > 0.1f ? _color : DEFAULT_COLOR; }
            set { _color = value; }
        }

        ///Will the timlines bg be colorized as well?
        public bool colorizeBackground {
            get { return _colorizeBackground; }
            set { _colorizeBackground = value; }
        }

        ///A new section of name at time
        public Section(string name, float time) {
            this.name = name;
            this.time = time;
            UID = System.Guid.NewGuid().ToString();
        }

        ///Rest the looping state
        public void ResetLoops() {
            currentLoopIteration = 0;
        }

        ///Breaks out of the loop
        public void BreakLoop() {
            currentLoopIteration = int.MaxValue;
        }

        ///Updates looping state and returns if looped
        public bool TryUpdateLoop() {
            if ( loopCount <= 0 && currentLoopIteration != int.MaxValue ) {
                return true;
            }

            if ( currentLoopIteration < loopCount ) {
                currentLoopIteration++;
                return true;
            }

            return false;
        }

        public override string ToString() {
            return string.Format("'{0}' Section Time: {1}", name, time);
        }
    }
}