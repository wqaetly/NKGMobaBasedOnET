#if UNITY_EDITOR
namespace Sirenix.OdinInspector.Demos
{
	using UnityEngine;

	public sealed class FilePathExamples : MonoBehaviour
	{
		[InfoBox(
			"FilePath attribute provides a neat interface for assigning paths to strings.\n" +
			"It also supports drag and drop from the project folder.")]
		// By default, FolderPath provides a path relative to the Unity project.
		[FilePath]
		public string UnityProjectPath;

		// It is possible to provide custom parent path. Parent paths can be relative to the Unity project, or absolute.
		[FilePath(ParentFolder = "Assets/Plugins/Sirenix")]
		public string RelativeToParentPath;

		// Using parent path, FilePath can also provide a path relative to a resources folder.
		[FilePath(ParentFolder = "Assets/Resources")]
		public string ResourcePath;

		// Provide a comma seperated list of allowed extensions. Dots are optional.
		[FilePath(Extensions = "cs")]
		[BoxGroup("Conditions")]
		public string ScriptFiles;

		// By setting AbsolutePath to true, the FilePath will provide an absolute path instead.
		[FilePath(AbsolutePath = true)]
		[BoxGroup("Conditions")]
		public string AbsolutePath;

		// FilePath can also be configured to show an error, if the provided path is invalid.
		[FilePath(RequireExistingPath = true)]
		[BoxGroup("Conditions")]
		public string ExistingPath;

		// By default, FilePath will enforce the use of forward slashes. It can also be configured to use backslashes instead.
		[FilePath(UseBackslashes = true)]
		[BoxGroup("Conditions")]
		public string Backslashes;

		// FilePath also supports member references with the $ symbol.
		[FilePath(ParentFolder = "$DynamicParent", Extensions = "$DynamicExtensions")]
		[BoxGroup("Member referencing")]
		public string DynamicFilePath;

		[BoxGroup("Member referencing")]
		public string DynamicParent = "Assets/Plugin/Sirenix";

		[BoxGroup("Member referencing")]
		public string DynamicExtensions = "cs, unity, jpg";

		// FilePath also supports lists and arrays.
		[FilePath(ParentFolder = "Assets/Plugins/Sirenix/Demos/Odin Inspector")]
		[BoxGroup("Lists")]
		public string[] ListOfFiles;
	}
}
#endif
