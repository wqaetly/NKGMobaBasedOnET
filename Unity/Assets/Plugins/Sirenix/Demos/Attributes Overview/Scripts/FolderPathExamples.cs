#if UNITY_EDITOR
namespace Sirenix.OdinInspector.Demos
{
	using UnityEngine;

	public sealed class FolderPathExamples : MonoBehaviour
	{
		[InfoBox(
			"FolderPath attribute provides a neat interface for assigning paths to strings.\n" +
			"It also supports drag and drop from the project folder.")]
		// By default, FolderPath provides a path relative to the Unity project.
		[FolderPath]
		public string UnityProjectPath;

		// It is possible to provide custom parent path. Parent paths can be relative to the Unity project, or absolute.
		[FolderPath(ParentFolder = "Assets/Plugins/Sirenix")]
		public string RelativeToParentPath;

		// Using parent path, FolderPath can also provide a path relative to a resources folder.
		[FolderPath(ParentFolder = "Assets/Resources")]
		public string ResourcePath;

		// By setting AbsolutePath to true, the FolderPath will provide an absolute path instead.
		[FolderPath(AbsolutePath = true)]
		[BoxGroup("Conditions")]
		public string AbsolutePath;

		// FolderPath can also be configured to show an error, if the provided path is invalid.
		[FolderPath(RequireExistingPath = true)]
		[BoxGroup("Conditions")]
		public string ExistingPath;

		// By default, FolderPath will enforce the use of forward slashes. It can also be configured to use backslashes instead.
		[FolderPath(UseBackslashes = true)]
		[BoxGroup("Conditions")]
		public string Backslashes;

		// FolderPath also supports member references with the $ symbol.
		[FolderPath(ParentFolder = "$DynamicParent")]
		[BoxGroup("Member referencing")]
		public string DynamicFolderPath;

		[BoxGroup("Member referencing")]
		public string DynamicParent = "Assets/Plugins/Sirenix";

		// FolderPath also supports lists and arrays.
		[FolderPath(ParentFolder = "Assets/Plugins/Sirenix")]
		[BoxGroup("Lists")]
		public string[] ListOfFolders;
	}
}
#endif
