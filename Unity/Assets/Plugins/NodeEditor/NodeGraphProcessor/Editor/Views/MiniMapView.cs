using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.UIElements;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace GraphProcessor
{
    public class MiniMapView : MiniMap
    {
        public MiniMapView(BaseGraphView baseGraphView)
        {
            this.graphView = baseGraphView;
            SetPosition(new Rect(2, 20, 100, 100));
        }

        public override void UpdatePresenterPosition()
        {
            Rect pos = GetPosition();
            SetPosition(new Rect(2, Math.Max(20, pos.y), pos.width, pos.height));
        }
    }
}