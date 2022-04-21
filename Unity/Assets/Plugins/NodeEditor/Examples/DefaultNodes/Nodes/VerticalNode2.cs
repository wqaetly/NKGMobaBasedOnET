using UnityEngine;
using GraphProcessor;

[System.Serializable, NodeMenuItem("Custom/Vertical 2")]
public class VerticalNode2 : BaseNode
{
	[Input, Vertical, ShowPortIcon(ShowIcon = false)]
    public float                input;

	[Input]
    public float                input2;

	[Output, Vertical, ShowPortIcon(ShowIcon = false)]
	public float				output;

	[Output]
	public float				output2;

	public override string		name => "Vertical 2";

	protected override void Process()
	{
		TryGetInputValue(nameof(input), ref input);
		TryGetInputValue(nameof(input2), ref input2);
		output = input;
		output2 = input;
	}

	public override void TryGetOutputValue<T>(NodePort outputPort, NodePort inputPort, ref T value)
	{
		switch (outputPort.fieldName)
		{
			case nameof(output):
				if (output is T finaValue)
				{
					value = finaValue;
				}
				break;
			case nameof(output2):
				if (output2 is T finaValue2)
				{
					value = finaValue2;
				}
				break;
		}
	}
}
