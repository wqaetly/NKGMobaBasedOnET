using System.Diagnostics;
using ETModel;
using ETModel.BBValues;
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

            switch (this.op)
            {
                case Operator.IS_SET: return true;
                case Operator.IS_EQUAL:
                {
                    switch (this.value)
                    {
                        case NP_BBValue_Bool npBbValue:
                            return npBbValue == bbValue as NP_BBValue_Bool;
                        case NP_BBValue_Float npBbValue:
                            return npBbValue == bbValue as NP_BBValue_Float;
                        case NP_BBValue_Int npBbValue:
                            return npBbValue == bbValue as NP_BBValue_Int;
                        case NP_BBValue_String npBbValue:
                            return npBbValue == bbValue as NP_BBValue_String;
                        case NP_BBValue_Vector3 npBbValue:
                            return npBbValue == bbValue as NP_BBValue_Vector3;
                        case NP_BBValue_Long npBbValue:
                            return npBbValue == bbValue as NP_BBValue_Long;
                        default:
                            Log.Error($"类型为{this.value.GetType()}的数未注册为NP_BBValue");
                            return false;
                    }
                }
                case Operator.IS_NOT_EQUAL:
                {
                    switch (this.value)
                    {
                        case NP_BBValue_Bool npBbValue:
                            return npBbValue != bbValue as NP_BBValue_Bool;
                        case NP_BBValue_Float npBbValue:
                            return npBbValue != bbValue as NP_BBValue_Float;
                        case NP_BBValue_Int npBbValue:
                            return npBbValue != bbValue as NP_BBValue_Int;
                        case NP_BBValue_String npBbValue:
                            return npBbValue != bbValue as NP_BBValue_String;
                        case NP_BBValue_Long npBbValue:
                            return npBbValue != bbValue as NP_BBValue_Long;
                        case NP_BBValue_Vector3 npBbValue:
                            return npBbValue != bbValue as NP_BBValue_Vector3;
                        default:
                            Log.Error($"类型为{this.value.GetType()}的数未注册为NP_BBValue");
                            return false;
                    }
                }

                case Operator.IS_GREATER_OR_EQUAL:
                {
                    switch (this.value)
                    {
                        case NP_BBValue_Bool npBbValue:
                            return npBbValue >= (bbValue as NP_BBValue_Bool);
                        case NP_BBValue_Float npBbValue:
                            return npBbValue >= (bbValue as NP_BBValue_Float);
                        case NP_BBValue_Int npBbValue:
                            return npBbValue >= (bbValue as NP_BBValue_Int);
                        case NP_BBValue_String npBbValue:
                            return npBbValue >= (bbValue as NP_BBValue_String);
                        case NP_BBValue_Long npBbValue:
                            return npBbValue >= bbValue as NP_BBValue_Long;
                        case NP_BBValue_Vector3 npBbValue:
                            return npBbValue >= (bbValue as NP_BBValue_Vector3);
                        default:
                            Log.Error($"类型为{this.value.GetType()}的数未注册为NP_BBValue");
                            return false;
                    }
                }

                case Operator.IS_GREATER:
                {
                    switch (this.value)
                    {
                        case NP_BBValue_Bool npBbValue:
                            return npBbValue > (bbValue as NP_BBValue_Bool);
                        case NP_BBValue_Float npBbValue:
                            return npBbValue > (bbValue as NP_BBValue_Float);
                        case NP_BBValue_Int npBbValue:
                            return npBbValue > (bbValue as NP_BBValue_Int);
                        case NP_BBValue_String npBbValue:
                            return npBbValue > (bbValue as NP_BBValue_String);
                        case NP_BBValue_Long npBbValue:
                            return npBbValue > (bbValue as NP_BBValue_Long);
                        case NP_BBValue_Vector3 npBbValue:
                            return npBbValue > (bbValue as NP_BBValue_Vector3);
                        default:
                            Log.Error($"类型为{this.value.GetType()}的数未注册为NP_BBValue");
                            return false;
                    }
                }

                case Operator.IS_SMALLER_OR_EQUAL:
                    switch (this.value)
                    {
                        case NP_BBValue_Bool npBbValue:
                            return npBbValue <= (bbValue as NP_BBValue_Bool);
                        case NP_BBValue_Float npBbValue:
                            return npBbValue <= (bbValue as NP_BBValue_Float);
                        case NP_BBValue_Int npBbValue:
                            return npBbValue <= (bbValue as NP_BBValue_Int);
                        case NP_BBValue_String npBbValue:
                            return npBbValue <= (bbValue as NP_BBValue_String);
                        case NP_BBValue_Long npBbValue:
                            return npBbValue <= (bbValue as NP_BBValue_Long);
                        case NP_BBValue_Vector3 npBbValue:
                            return npBbValue <= (bbValue as NP_BBValue_Vector3);
                        default:
                            Log.Error($"类型为{this.value.GetType()}的数未注册为NP_BBValue");
                            return false;
                    }
                case Operator.IS_SMALLER:
                    switch (this.value)
                    {
                        case NP_BBValue_Bool npBbValue:
                            return npBbValue < (bbValue as NP_BBValue_Bool);
                        case NP_BBValue_Float npBbValue:
                            return npBbValue < (bbValue as NP_BBValue_Float);
                        case NP_BBValue_Int npBbValue:
                            return npBbValue < (bbValue as NP_BBValue_Int);
                        case NP_BBValue_String npBbValue:
                            return npBbValue < (bbValue as NP_BBValue_String);
                        case NP_BBValue_Long npBbValue:
                            return npBbValue < (bbValue as NP_BBValue_Long);
                        case NP_BBValue_Vector3 npBbValue:
                            return npBbValue < (bbValue as NP_BBValue_Vector3);
                        default:
                            Log.Error($"类型为{this.value.GetType()}的数未注册为NP_BBValue");
                            return false;
                    }

                default: return false;
            }
        }

        override public string ToString()
        {
            return "(" + this.op + ") " + this.key + " ? " + this.value;
        }
    }
}