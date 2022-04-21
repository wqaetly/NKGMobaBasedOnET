using ET;
using Log = NPBehave_Core.Log;

namespace NPBehave
{
    public class BlackboardCondition: ObservingDecorator
    {
        private string key;
        private ANP_BBValue value;
        private Operator op;

        public string Key
        {
            get
            {
                return key;
            }
        }

        public ANP_BBValue Value
        {
            get
            {
                return value;
            }
            set
            {
                this.value = value;
            }
        }

        public Operator Operator
        {
            get
            {
                return op;
            }
        }

        public BlackboardCondition(string key, Operator op, ANP_BBValue value, Stops stopsOnChange, Node decoratee): base("BlackboardCondition",
            stopsOnChange, decoratee)
        {
            this.op = op;
            this.key = key;
            this.value = value;
            this.stopsOnChange = stopsOnChange;
        }

        public BlackboardCondition(string key, Operator op, Stops stopsOnChange, Node decoratee): base("BlackboardCondition", stopsOnChange,
            decoratee)
        {
            this.op = op;
            this.key = key;
            this.stopsOnChange = stopsOnChange;
        }

        override protected void StartObserving()
        {
            this.RootNode.Blackboard.AddObserver(key, onValueChanged);
        }

        override protected void StopObserving()
        {
            this.RootNode.Blackboard.RemoveObserver(key, onValueChanged);
        }

        private void onValueChanged(Blackboard.Type type, ANP_BBValue newValue)
        {
            Evaluate();
        }

        override protected bool IsConditionMet()
        {
            if (op == Operator.ALWAYS_TRUE)
            {
                return true;
            }

            if (!this.RootNode.Blackboard.Isset(key))
            {
                return op == Operator.IS_NOT_SET;
            }

            ANP_BBValue bbValue = this.RootNode.Blackboard.Get(key);

            return NP_BBValueHelper.Compare(this.value, bbValue, op);
        }

        override public string ToString()
        {
            return "(" + this.op + ") " + this.key + " ? " + this.value;
        }
    }
}