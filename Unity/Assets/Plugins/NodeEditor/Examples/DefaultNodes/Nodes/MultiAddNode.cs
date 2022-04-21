using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GraphProcessor;
using System.Linq;

[System.Serializable, NodeMenuItem("Custom/MultiAdd")]
public class MultiAddNode : BaseNode
{
	[Input]
	public IEnumerable< float >	inputs = null;

	[Output]
	public float				output;

	public override string		name => "Add";

	protected override void Process()
	{
		output = 0;
		inputs = TryGetAllInputValues<float>(nameof(inputs));
		if (inputs == null)
			return ;

		foreach (float input in inputs)
			output += input;
	}

	public override void TryGetOutputValue<T>(NodePort outputPort, NodePort inputPort, ref T value)
	{
		if (output is T finalValue)
		{
			value = finalValue;
		}
	}

	[CustomPortBehavior(nameof(inputs))]
	IEnumerable< PortData > GetPortsForInputs(List< SerializableEdge > edges)
	{
		yield return new PortData{ displayName = "In ", displayType = typeof(float), acceptMultipleEdges = true};
	}
}
