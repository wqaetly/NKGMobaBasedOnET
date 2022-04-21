using UnityEngine;
using GraphProcessor;

[System.Serializable, NodeMenuItem("Custom/Renamable")]
public class RenamableNode : BaseNode
{
    [Output("Out")] public float output;

    [Input("In")] public float input;

    public override string name => "Renamable";

    public override bool isRenamable => true;

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