#if UNITY_EDITOR
namespace Sirenix.OdinInspector.Demos
{
    using UnityEngine;

    [TypeInfoBox("You can apply attributes on type definitions instead of on individual attributes.")]
    public class AttributesOnClassesExample : SerializedMonoBehaviour
    {
        public MyClass<int> A;
        public MyClass<string> B;
        public MyClass<GameObject> C;
    }

    [Required]
    [LabelWidth(70)]
    [Toggle("IsEnabled")]
    [HideReferenceObjectPicker]
    public class MyClass<T>
    {
        public bool IsEnabled;
        public T Foo, Bar;
    }

    /* This code produces the same result as the code above, and is what would have been required prior to Odin 2.0. */
    //public class AttributesOnClassesExample : SerializedMonoBehaviour
    //{
    //    [Required]
    //    [LabelWidth(70)]
    //    [Toggle("IsEnabled")]
    //    [HideReferenceObjectPicker]
    //    public MyClass<int> A;
    //
    //    [Required]
    //    [LabelWidth(70)]
    //    [Toggle("IsEnabled")]
    //    [HideReferenceObjectPicker]
    //    public MyClass<string> B;
    //
    //    [Required]
    //    [LabelWidth(70)]
    //    [Toggle("IsEnabled")]
    //    [HideReferenceObjectPicker]
    //    public MyClass<GameObject> C;
    //}
    //
    //public class MyClass<T>
    //{
    //    public bool IsEnabled;
    //    public T Foo, Bar;
    //}
}
#endif
