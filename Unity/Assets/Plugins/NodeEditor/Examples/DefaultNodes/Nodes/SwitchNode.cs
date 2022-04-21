using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GraphProcessor;
using System.Linq;

[System.Serializable, NodeMenuItem("Conditional/Switch")]
public class SwitchNode : BaseNode
{
	[Input(name = "In")]
    public float                input;

	[Output(name = "Out")]
	public float				output;

	public override string		name => "Switch";
	
	protected override void Process()
	{
		TryGetInputValue(nameof(input), ref input);
		output = input * 42;
	}

	public override void TryGetOutputValue<T>(NodePort outputPort, NodePort inputPort, ref T value)
	{
		if (output is T finalValue)
		{
			value = finalValue;
		}
	}
}
