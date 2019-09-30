// Animancer // Copyright 2019 Kybernetik //

// Assembly Definition files were introduced in Unity 2017.3.
#if UNITY_2017_3_OR_NEWER

using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.InteropServices;

[assembly: AssemblyTitle("Animancer")]
[assembly: AssemblyDescription("An animation system for Unity which is based on the Playables API.")]
[assembly: AssemblyCompany("Kybernetik")]
[assembly: AssemblyProduct("Animancer Pro")]
[assembly: AssemblyCopyright("Copyright © Kybernetik 2019")]
[assembly: ComVisible(false)]
[assembly: AssemblyVersion("3.0.0.0")]

[assembly: SuppressMessage("Style", "IDE0016:Use 'throw' expression",
    Justification = "Not supported by older Unity versions.")]
[assembly: SuppressMessage("Style", "IDE0018:Inline variable declaration",
    Justification = "Not supported by older Unity versions.")]
[assembly: SuppressMessage("Style", "IDE0019:Use pattern matching",
    Justification = "Not supported by older Unity versions.")]
[assembly: SuppressMessage("Style", "IDE0044:Make field readonly",
    Justification = "Using the [SerializeField] attribute on a private field means Unity will set it from serialized data.")]
[assembly: SuppressMessage("Code Quality", "IDE0051:Remove unused private members",
    Justification = "Unity messages can be private, but the IDE will not know that Unity can still call them.")]
[assembly: SuppressMessage("Code Quality", "IDE0052:Remove unread private members",
    Justification = "Unity messages can be private and don't need to be called manually.")]
[assembly: SuppressMessage("Style", "IDE0060:Remove unused parameter",
    Justification = "Unity messages sometimes need specific signatures, even if you don't use all the parameters.")]
[assembly: SuppressMessage("Style", "IDE1005:Delegate invocation can be simplified.",
    Justification = "Not supported by older Unity versions.")]

// This suppression doesn't seem to actually work so we need to put #pragma warning disable in every file :(
//[assembly: SuppressMessage("Code Quality", "CS0649:Field is never assigned to, and will always have its default value",
//    Justification = "Using the [SerializeField] attribute on a private field means Unity will set it from serialized data.")]

#endif
