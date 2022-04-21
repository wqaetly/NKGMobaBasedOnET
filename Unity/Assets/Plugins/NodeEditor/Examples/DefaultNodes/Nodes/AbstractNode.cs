using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GraphProcessor;
using System.Linq;

public abstract class AbstractNode : BaseNode
{
	[Input(name = "In")]
    public float                input;

	[Output(name = "Out")]
	public float				output;

	public override string		name => "AbstractNode";

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

[System.Serializable, NodeMenuItem("Custom/Abstract Child1")]
public class AbstractNodeChild1 : AbstractNode {}
[System.Serializable, NodeMenuItem("Custom/Abstract Child2")]
public class AbstractNodeChild2 : AbstractNode {}
