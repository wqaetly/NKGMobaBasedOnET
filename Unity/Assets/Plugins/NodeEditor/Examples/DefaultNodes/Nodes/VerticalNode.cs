using UnityEngine;
using GraphProcessor;

[System.Serializable, NodeMenuItem("Custom/Vertical")]
public class VerticalNode : BaseNode
{
	[Input, Vertical, ShowPortIcon(ShowIcon = false)]
    public float                input;

	[Output, Vertical, ShowPortIcon(ShowIcon = false)]
	public float				output;
	[Output, Vertical, ShowPortIcon(ShowIcon = false)]
	public float				output2;
	[Output, Vertical, ShowPortIcon(ShowIcon = false)]
	public float				output3;

	public override string		name => "Vertical";
	
	
	protected override void Process()
	{
		TryGetInputValue(nameof(input), ref input);
		output = input;
		output2 = input;
		output3 = input;
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
			case nameof(output3):
				if (output3 is T finaValue3)
				{
					value = finaValue3;
				}
				break;
		}
	}
}
