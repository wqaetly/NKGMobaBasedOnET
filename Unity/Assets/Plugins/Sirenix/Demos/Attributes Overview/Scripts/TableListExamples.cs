#if UNITY_EDITOR
namespace Sirenix.OdinInspector.Demos
{
    using UnityEngine;
    using Sirenix.OdinInspector;
    using System.Collections.Generic;
    using System;

    public class TableListExamples : MonoBehaviour
    {
        [TableList(ShowIndexLabels = true)]
        public List<SomeCustomClass> TableListWithIndexLabels = new List<SomeCustomClass>();

        [TableList(DrawScrollView = true, MaxScrollViewHeight = 200, MinScrollViewHeight = 100)]
        public List<SomeCustomClass> MinMaxScrollViewTable = new List<SomeCustomClass>();

        [TableList(DrawScrollView = false)]
        public List<SomeCustomClass> AlwaysExpandedTable = new List<SomeCustomClass>();

        [TableList(ShowPaging = true)]
        public List<SomeCustomClass> TableWithPaging = new List<SomeCustomClass>();
    }

    [Serializable]
    public class SomeCustomClass
    {
        [TableColumnWidth(57, Resizable = false)]
        [PreviewField(Alignment = ObjectFieldAlignment.Center)]
        public Texture Icon;

        [TextArea]
        public string Description;

        [VerticalGroup("Combined Column"), LabelWidth(22)]
        public string A, B, C;

        [TableColumnWidth(60)]
        [Button, VerticalGroup("Actions")]
        public void Test1() { }
        
        [TableColumnWidth(60)]
        [Button, VerticalGroup("Actions")]
        public void Test2() { }
    }
}
#endif
