using System.Diagnostics;
using ETModel;
using MonKey;
using UnityEditor;

namespace ETEditor
{
	internal class OpcodeInfo
	{
		public string Name;
		public int Opcode;
	}

	public class Proto2CSEditor: EditorWindow
	{
		[Command("ETEditor_AllProto2CS","Proto转CS",Category = "ETEditor")]
		public static void AllProto2CS()
		{
			Process process = ProcessHelper.Run("dotnet", "Proto2CS.dll", "../Proto/", true);
			Log.Info(process.StandardOutput.ReadToEnd());
			AssetDatabase.Refresh();
		}
	}
}
