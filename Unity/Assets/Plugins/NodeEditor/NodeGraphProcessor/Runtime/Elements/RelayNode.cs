using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GraphProcessor;
using System.Linq;
using System;
using Sirenix.OdinInspector;
using Sirenix.Utilities;

[System.Serializable, NodeMenuItem("Utils/Relay")]
public class RelayNode : BaseNode
{
	const string packIdentifier = "_Pack";

	public struct PackedRelayData
	{
		public List<object>	values;
		public List<string>	names;
		public List<Type>	types;
	}

	[Input(name = "In"), ShowPortIcon(ShowIcon = false)]
    public PackedRelayData	input = new PackedRelayData(){values = new List<object>(), names =new List<string>(), types = new List<Type>()};

	[Output(name = "Out"), ShowPortIcon(ShowIcon = false)]
	public PackedRelayData	output = new PackedRelayData(){values = new List<object>(), names =new List<string>(), types = new List<Type>()};

	[InfoBox("是否对输出值进行展开处理，如果为True则返回实际的值，否则返回PackedRelayData", InfoMessageType.Warning)]
	public bool		unpackOutput = false;
	public bool		packInput = false;
	public int		inputEdgeCount = 0;

	public SerializableType inputType = new SerializableType(typeof(object));

	const int		k_MaxPortSize = 14;

	protected override void Process()
	{
		input.values.Clear();
		List<SerializableEdge> edges = this.inputPorts[0].GetEdges();
		inputEdgeCount = edges.Count;

		// If the relay is only connected to another relay:
		if (inputEdgeCount == 1 && edges.First().outputNode.GetType() == typeof(RelayNode))
		{
			TryGetInputValue(nameof(input), ref input);
		}
		else
		{
			IEnumerable<object> inputValues = TryGetAllInputValues<object>(nameof(input));
			inputValues.ForEach(value => input.values.Add(value));
			input.names = edges.Select(e => e.outputPort.portData.displayName).ToList();
			input.types = edges.Select(e => e.outputPort.portData.displayType ?? e.outputPort.fieldInfo.FieldType).ToList();
		}
		
		output = input;
	}

	public override void TryGetOutputValue<T>(NodePort outputPort, NodePort inputPort, ref T value)
	{
		if (inputPorts.Count == 0)
			return;
	
		var inputPortEdges = inputPorts[0].GetEdges();
		if (outputPort.portData.identifier != packIdentifier && (unpackOutput || inputPortEdges.Count == 1))
		{
			if (output.values == null)
				return;

			int inputPortIndexInOutputPortEdge = outputPort.GetEdges().FindIndex(edge => edge.inputPort == inputPort);
			if (output.values[inputPortIndexInOutputPortEdge] is T finalValue)
			{
				value = finalValue;
			}
		}
		else
		{
			if (output is T finalValue)
			{
				value = finalValue;
			}
		}
	}

	public override string layoutStyle => "GraphProcessorStyles/RelayNode";

	[CustomPortBehavior(nameof(input))]
	IEnumerable< PortData > InputPortBehavior(List< SerializableEdge > edges)
	{
		// When the node is initialized, the input ports is empty because it's this function that generate the ports
		int sizeInPixel = 0;
		if (inputPorts.Count != 0)
		{
			// Add the size of all input edges:
			var inputEdges = inputPorts[0]?.GetEdges();
			sizeInPixel = inputEdges.Sum(e => Mathf.Max(0, e.outputPort.portData.sizeInPixel - 8));
		}
		
		if (edges.Count == 1 && !packInput)
			inputType.type = edges[0].outputPort.portData.displayType;
		else
			inputType.type = typeof(object);
		
		yield return new PortData {
			displayName = "",
			displayType = inputType.type,
			identifier = "0",
			acceptMultipleEdges = true,
			sizeInPixel = Mathf.Min(k_MaxPortSize, sizeInPixel + 8),
			showPortIcon = false
		};
	}

	[CustomPortBehavior(nameof(output))]
	IEnumerable< PortData > OutputPortBehavior(List< SerializableEdge > edges)
	{
		if (inputPorts.Count == 0)
		{
			// Default dummy port to avoid having a relay without any output:
			yield return new PortData {
				displayName = "",
				displayType = typeof(object),
				identifier = "0",
				acceptMultipleEdges = true,
				showPortIcon = false
			};
			yield break;
		}

		var inputPortEdges = inputPorts[0].GetEdges();
		var underlyingPortData = GetUnderlyingPortDataList();
		if (unpackOutput && inputPortEdges.Count == 1)
		{
			yield return new PortData
			{
				displayName = "Pack",
				identifier = packIdentifier,
				displayType = inputType.type,
				acceptMultipleEdges = true,
				sizeInPixel = Mathf.Min(k_MaxPortSize, Mathf.Max(underlyingPortData.Count, 1) + 7), // TODO: function
				showPortIcon = false
			};

			// We still keep the packed data as output when unpacking just in case we want to continue the relay after unpacking
			for (int i = 0; i < underlyingPortData.Count; i++)
			{
				yield return new PortData {
					displayName = underlyingPortData?[i].name ?? "",
					displayType = underlyingPortData?[i].type ?? typeof(object),
					identifier = i.ToString(),
					acceptMultipleEdges = true,
					sizeInPixel = 0,
					showPortIcon = false
				};
			}
		}
		else
		{
			yield return new PortData {
				displayName = "",
				displayType = inputType.type,
				identifier = "0",
				acceptMultipleEdges = true,
				sizeInPixel = Mathf.Min(k_MaxPortSize, Mathf.Max(underlyingPortData.Count, 1) + 7),
				showPortIcon = false
			};
		}
	}

	static List<(Type, string)> s_empty = new List<(Type, string)>();
	public List<(Type type, string name)> GetUnderlyingPortDataList()
	{
		// get input edges:
		if (inputPorts.Count == 0)
			return s_empty;

		var inputEdges = GetNonRelayEdges();

		if (inputEdges != null)
			return inputEdges.Select(e => (e.outputPort.portData.displayType ?? e.outputPort.fieldInfo.FieldType, e.outputPort.portData.displayName)).ToList();

		return s_empty;
	}

	public List<SerializableEdge> GetNonRelayEdges()
	{
		var inputEdges = inputPorts?[0]?.GetEdges();

		// Iterate until we don't have a relay node in input
		while (inputEdges.Count == 1 && inputEdges.First().outputNode.GetType() == typeof(RelayNode))
			inputEdges = inputEdges.First().outputNode.inputPorts[0]?.GetEdges();

		return inputEdges;
	}
}