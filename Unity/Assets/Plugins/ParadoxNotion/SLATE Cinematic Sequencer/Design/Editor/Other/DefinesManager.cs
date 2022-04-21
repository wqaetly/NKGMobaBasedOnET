#if UNITY_EDITOR

using UnityEditor;

namespace Slate
{

    ///Utility for handling player setting defines
	public static class DefinesManager
    {

        ///Is define..defined in player settings?
        public static bool HasDefine(string define) {
            return PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone).Contains(define);
        }

        ///Toggle define in player settings
        public static void SetDefineActive(string define, bool value) {
            foreach ( BuildTargetGroup target in System.Enum.GetValues(typeof(BuildTargetGroup)) ) {
                if ( ValidateBuildTarget(target) ) {
                    var defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(target);
                    if ( value == true && !defines.Contains(define) ) {
                        defines += ";" + define;
                    }
                    if ( value == false ) {
                        defines = defines.Replace(define, string.Empty);
                    }
                    PlayerSettings.SetScriptingDefineSymbolsForGroup(target, defines);
                }
            }
        }

        //...
        static bool ValidateBuildTarget(BuildTargetGroup target) {
            if ( target == BuildTargetGroup.Unknown ) {
                return false;
            }
            var field = typeof(BuildTargetGroup).GetField(target.ToString());
            if ( field.IsDefined(typeof(System.ObsoleteAttribute), true) ) {
                return false;
            }
            return true;
        }
    }
}

#endif