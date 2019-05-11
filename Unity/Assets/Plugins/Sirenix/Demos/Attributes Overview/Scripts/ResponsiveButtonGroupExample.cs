#if UNITY_EDITOR
namespace Sirenix.OdinInspector.Demos
{
    using UnityEngine;
    using Sirenix.OdinInspector;
    using Sirenix.OdinInspector.Editor;

    public class ResponsiveButtonGroupExample : MonoBehaviour
    {
        [Button(ButtonSizes.Large), GUIColor(0, 1, 0)]
        private void OpenDockableWindowExample()
        {
            var window = UnityEditor.EditorWindow.GetWindow<MyDockableGameDashboard>();
            window.WindowPadding = new Vector4();
        }

        [OnInspectorGUI] private void Space1() { GUILayout.Space(20); }

        [ResponsiveButtonGroup] public void Foo() { }
        [ResponsiveButtonGroup] public void Bar() { }
        [ResponsiveButtonGroup] public void Baz() { }

        [OnInspectorGUI] private void Space2() { GUILayout.Space(20); }

        [ResponsiveButtonGroup("UniformGroup", UniformLayout = true)] public void Foo1() { }
        [ResponsiveButtonGroup("UniformGroup")]                       public void Foo2() { }
        [ResponsiveButtonGroup("UniformGroup")]                       public void LongesNameWins() { }
        [ResponsiveButtonGroup("UniformGroup")]                       public void Foo4() { }
        [ResponsiveButtonGroup("UniformGroup")]                       public void Foo5() { }
        [ResponsiveButtonGroup("UniformGroup")]                       public void Foo6() { }

        [OnInspectorGUI] private void Space3() { GUILayout.Space(20); }

        [ResponsiveButtonGroup("DefaultButtonSize", DefaultButtonSize = ButtonSizes.Small)] public void Bar1() { }
        [ResponsiveButtonGroup("DefaultButtonSize")]                                        public void Bar2() { }
        [ResponsiveButtonGroup("DefaultButtonSize")]                                        public void Bar3() { }
        [Button(ButtonSizes.Large), ResponsiveButtonGroup("DefaultButtonSize")]             public void Bar4() { }
        [Button(ButtonSizes.Large), ResponsiveButtonGroup("DefaultButtonSize")]             public void Bar5() { }
        [ResponsiveButtonGroup("DefaultButtonSize")]                                        public void Bar6() { }

        [OnInspectorGUI] private void Space4() { GUILayout.Space(20); }

        [FoldoutGroup("SomeOtherGroup")]
        [ResponsiveButtonGroup("SomeOtherGroup/SomeBtnGroup")] public void Baz1() { }
        [ResponsiveButtonGroup("SomeOtherGroup/SomeBtnGroup")] public void Baz2() { }
        [ResponsiveButtonGroup("SomeOtherGroup/SomeBtnGroup")] public void Baz3() { }
    }

    public class MyDockableGameDashboard : OdinEditorWindow
    {
        const string DEFAULT_GROUP = "TabGroup/Default/BtnGroup";
        const string UNIFORM_GROUP = "TabGroup/Uniform/BtnGroup";

        [TabGroup("TabGroup", "Default")]
        [TabGroup("TabGroup", "Uniform")]
        public bool Toggle;

        [TabGroup("TabGroup", "Default", Paddingless = false)]
        [ResponsiveButtonGroup(DEFAULT_GROUP, DefaultButtonSize = ButtonSizes.Large)]
        public void PepperPepperPepper() { }

        [ResponsiveButtonGroup(DEFAULT_GROUP)]
        public void Thud() { }

        [ResponsiveButtonGroup(DEFAULT_GROUP)]
        public void WaldoWaldo() { }

        [ResponsiveButtonGroup(DEFAULT_GROUP)]
        public void Fred() { }

        [DisableIf("Toggle")]
        [ResponsiveButtonGroup(DEFAULT_GROUP)]
        public void FooFoo() { }

        [ResponsiveButtonGroup(DEFAULT_GROUP)]
        public void BarBar() { }

        [ResponsiveButtonGroup(DEFAULT_GROUP)]
        public void BazBazBaz() { }

        [DisableIf("Toggle")]
        [ResponsiveButtonGroup(DEFAULT_GROUP)]
        public void QuxQux() { }

        [ResponsiveButtonGroup(DEFAULT_GROUP)]
        public void QuuxQuuxQuux() { }

        [EnableIf("Toggle")]
        [ResponsiveButtonGroup(DEFAULT_GROUP)]
        public void CorgeCorge() { }

        [ResponsiveButtonGroup(DEFAULT_GROUP)]
        public void Uier() { }

        [EnableIf("Toggle")]
        [Button(ButtonSizes.Small)]
        [ResponsiveButtonGroup(DEFAULT_GROUP)]
        public void A() { }

        [Button(ButtonSizes.Small)]
        [EnableIf("Toggle")]
        [ResponsiveButtonGroup(DEFAULT_GROUP)]
        public void B() { }

        [Button(ButtonSizes.Small)]
        [ShowIf("Toggle", false)]
        [ResponsiveButtonGroup(DEFAULT_GROUP)]
        public void C() { }

        [EnableIf("Toggle")]
        [ResponsiveButtonGroup(DEFAULT_GROUP)]
        public void Henk() { }

        [ResponsiveButtonGroup(DEFAULT_GROUP)]
        public void Def() { }

        [ResponsiveButtonGroup(DEFAULT_GROUP)]
        public void DefDefDef() { }

        [TabGroup("TabGroup", "Uniform")]
        [ResponsiveButtonGroup(UNIFORM_GROUP, UniformLayout = true)]
        public void FooPepper() { }

        [ResponsiveButtonGroup(UNIFORM_GROUP)]
        public void FooThud() { }

        [ResponsiveButtonGroup(UNIFORM_GROUP)]
        public void WaldoFoo() { }

        [ResponsiveButtonGroup(UNIFORM_GROUP)]
        public void FredFoo() { }

        [DisableIf("Toggle")]
        [ResponsiveButtonGroup(UNIFORM_GROUP)]
        public void Fooooo() { }

        [ResponsiveButtonGroup(UNIFORM_GROUP)]
        public void BarFoo() { }

        [ResponsiveButtonGroup(UNIFORM_GROUP)]
        public void BazFoo() { }

        [DisableIf("Toggle")]
        [ResponsiveButtonGroup(UNIFORM_GROUP)]
        public void FooQux() { }

        [ResponsiveButtonGroup(UNIFORM_GROUP)]
        public void QuuxFoo() { }

        [ResponsiveButtonGroup(UNIFORM_GROUP)]
        public void UierFoo() { }

        [EnableIf("Toggle")]
        [ResponsiveButtonGroup(UNIFORM_GROUP)]
        public void CorgeFoo() { }

        [EnableIf("Toggle")]
        [ResponsiveButtonGroup(UNIFORM_GROUP)]
        public void FooGrapl() { }

        [Button(ButtonSizes.Large)]
        [ResponsiveButtonGroup(UNIFORM_GROUP)]
        public void FooDef() { }

        [Button(ButtonSizes.Large)]
        [ResponsiveButtonGroup(UNIFORM_GROUP)]
        public void DefFoo() { }
    }
}
#endif
