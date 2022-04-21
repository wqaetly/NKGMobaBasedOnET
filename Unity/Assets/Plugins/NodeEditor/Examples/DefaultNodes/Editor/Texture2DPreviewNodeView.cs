//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2021年6月16日 13:52:24
//------------------------------------------------------------

using GraphProcessor;
using UnityEngine;
using UnityEngine.UIElements;

[NodeCustomEditor(typeof(Texture2DPreviewNode))]
public class Texture2DPreviewNodeView: BaseNodeView
{
    public override void Enable()
    {
        var node = nodeTarget as Texture2DPreviewNode;
        Image image = new Image()
        {
            image = node.input,
            scaleMode = ScaleMode.ScaleToFit
        };

        image.AddToClassList("NodeTexturePreview");
        
        node.onProcessed += () => image.image = node.input;

        // Create your fields using node's variables and add them to the controlsContainer
        VisualElement divider = new VisualElement() {name = "divider"};
        divider.AddToClassList("horizontal");
        controlsContainer.Add(divider);
        controlsContainer.Add(image);
    }
}