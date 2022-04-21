using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GraphProcessor;
using System.Linq;

[System.Serializable, NodeMenuItem("Custom/MultiPorts")]
public class CustomPortsNode : BaseNode
{
    [Input]
	public List< float >       	inputs;

	[Output]
	public List< float >		outputs; // TODO: custom function for this one

	public List< object >				values = new List< object >();

	public override string		name => "CustomPorts";

    public override string      layoutStyle => "TestType";

    // We keep the max port count so it doesn't cause binding issues
    [SerializeField, HideInInspector]
	int							portCount = 1;

	protected override void Process()
	{
		foreach (var inputValue in TryGetAllInputValues<float>(nameof(inputs)))
		{
			outputs.Add(inputValue);
		}

		// do things with values
	}

	public override void TryGetOutputValue<T>(NodePort outputPort, NodePort inputPort, ref T value)
	{
		//出端口所有连线对应端口中，目标入端口所对应的index
		int inputPortIndexInOutputPortEdge = outputPort.GetEdges().FindIndex(edge => edge.inputPort == inputPort);
		if (outputs[inputPortIndexInOutputPortEdge] is T finalValue)
		{
			value = finalValue;
		}
	}

	[CustomPortBehavior(nameof(inputs))]
	IEnumerable< PortData > ListPortBehavior(List< SerializableEdge > edges)
	{
		portCount = Mathf.Max(portCount, edges.Count + 1);

		for (int i = 0; i < portCount; i++)
		{
			yield return new PortData {
				displayName = "In " + i,
				displayType = typeof(float),
				identifier = i.ToString(), // Must be unique
			};
		}
	}
}
