using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GraphProcessor;
using System.Linq;

[System.Serializable, NodeMenuItem("Custom/CircleRadians")]
public class CircleRadians : BaseNode
{
	[Output(name = "In")]
    public List< float >		outputRadians;

	public override string		name => "CircleRadians";

	public override void TryGetOutputValue<T>(NodePort outputPort, NodePort inputPort, ref T value)
	{
		int inputPortIndexInOutputPortEdge = outputPort.GetEdges().FindIndex(edge => edge.inputPort == inputPort);
		if (outputRadians[inputPortIndexInOutputPortEdge] is T finalValue)
		{
			value = finalValue;
		}
		
	}
}
