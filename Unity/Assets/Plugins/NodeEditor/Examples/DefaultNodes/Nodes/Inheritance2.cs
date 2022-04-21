using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GraphProcessor;
using System.Linq;

[System.Serializable, NodeMenuItem("Custom/Inheritance2")]
public class Inheritance2 : Inheritance1
{
    [Input(name = "In 2")] public float input2;

    [Output(name = "Out 2")] public float output2;

    public float field2;

    public override string name => "Inheritance2";

    protected override void Process()
    {
        base.Process();
        TryGetInputValue(nameof(input2), ref input2);
        output2 = input2 * 44;
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
            case nameof(output2):
                if (output2 is T finalValue2)
                {
                    value = finalValue2;
                }
                break;
        }
    }
}