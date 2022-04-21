using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GraphProcessor;
using System.Linq;

[System.Serializable, NodeMenuItem("Custom/Prefab")]
public class PrefabNode : BaseNode
{
	[Output(name = "Out"), SerializeField]
	public GameObject			output;

	public override string		name => "Prefab";

	public override void TryGetOutputValue<T>(NodePort outputPort, NodePort inputPort, ref T value)
	{
		if (output is T finalValue)
		{
			value = finalValue;
		}
	}
}
