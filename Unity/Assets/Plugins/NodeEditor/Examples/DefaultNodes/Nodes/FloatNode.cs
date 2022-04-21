using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GraphProcessor;

[System.Serializable, NodeMenuItem("Primitives/Float")]
public class FloatNode : BaseNode
{
    [Output("Out")] public float output;

    [Input("In")] public float input;

    public override string name => "Float";

    protected override void Process()
    {
        TryGetInputValue(nameof(input), ref input);
        output = input;
    }

    public override void TryGetOutputValue<T>(NodePort outputPort, NodePort inputPort, ref T value)
    {
        if (output is T finalValue)
        {
            value = finalValue;
        }
    }
}