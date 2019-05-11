#if UNITY_EDITOR
namespace Sirenix.OdinInspector.Demos
{
    using UnityEngine;
    using Sirenix.OdinInspector;
    using Sirenix.Utilities;

    // Inheriting from SerializedMonoBehaviour is only needed if you want Odin to serialize the multi-dimensional arrays for you.
    // If you prefer doing that yourself, you can still make Odin show them in the inspector using the ShowInInspector attribute.
    [TypeInfoBox("Right-click and drag the column and row labels in order to modify the tables.")]
    public class TableMatrixExamples : SerializedMonoBehaviour
    {
        [TableMatrix(SquareCells = true)]
        public GameObject[,] PrefabMatrix = new GameObject[8, 4];

        [TableMatrix(HorizontalTitle = "Square Celled Matrix", SquareCells = true)]
        public Texture2D[,] SquareCelledMatrix = new Texture2D[8, 4];

        [TableMatrix(HorizontalTitle = "Read Only Matrix", IsReadOnly = true)]
        public int[,] ReadOnlyMatrix = new int[5, 5];

        [TableMatrix(HorizontalTitle = "X axis", VerticalTitle = "Y axis")]
        public InfoMessageType[,] LabledMatrix = new InfoMessageType[6, 6];

        [TableMatrix(HorizontalTitle = "Custom Cell Drawing", DrawElementMethod = "DrawColoredEnumElement", ResizableColumns = false, RowHeight = 16)]
        public bool[,] CustomCellDrawing = new bool[30, 30];

#if UNITY_EDITOR

        private static bool DrawColoredEnumElement(Rect rect, bool value)
        {
            if (Event.current.type == EventType.MouseDown && rect.Contains(Event.current.mousePosition))
            {
                value = !value;
                GUI.changed = true;
                Event.current.Use();
            }

            UnityEditor.EditorGUI.DrawRect(rect.Padding(1), value ? new Color(0.1f, 0.8f, 0.2f) : new Color(0, 0, 0, 0.5f));

            return value;
        }

#endif
    }
}
#endif
