using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GraphProcessor;
using System.Linq;

[System.Serializable, NodeMenuItem("Custom/Inheritance1")]
public class Inheritance1 : InheritanceBase
{
    [Input(name = "In 1")] public float input1;

    [Output(name = "Out 1")] public float output1;

    public float field1;

    public override string name => "Inheritance1";

    protected override void Process()
    {
        base.Process();
        TryGetInputValue(nameof(input1), ref input1);
        output1 = input1 * 43;
    }

    public override void TryGetOutputValue<T>(NodePort outputPort, NodePort inputPort, ref T value)
    {
        base.TryGetOutputValue(outputPort, inputPort, ref value);
        switch (outputPort.fieldName)
        {
            case nameof(output1):
                if (output1 is T finalValue1)
                {
                    value = finalValue1;
                }
                break;
        }
    }
}