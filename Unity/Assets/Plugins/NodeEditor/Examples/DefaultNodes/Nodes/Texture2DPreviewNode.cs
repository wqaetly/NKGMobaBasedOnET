using System.Collections;
using System.Collections.Generic;
using GraphProcessor;
using NodeGraphProcessor.Examples;
using UnityEngine;

[System.Serializable, NodeMenuItem("Custom/Texture2DPreview")]
public class Texture2DPreviewNode : LinearConditionalNode
{
    [Input(name = "Texture2D")] public Texture2D input;

    public override string		name => "Texture2DPreview";

    protected override void Process()
    {
        TryGetInputValue<Texture2D>(nameof(input), ref input);
    }
}
