//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2020年10月14日 22:27:15
//------------------------------------------------------------

namespace ET
{
    /// <summary>
    /// AB实用函数集，主要是路径拼接
    /// </summary>
    public class XAssetPathUtilities
    {
        public static string GetTexturePath(string fileName)
        {
            return $"Assets/Res/Sprite/{fileName}.png";
        }

        public static string GetFGUIDesPath(string fileName)
        {
            return $"Assets/Res/FGUI/{fileName}.bytes";
        }
        
        public static string GetFGUIResPath(string fileName,string extension)
        {
            return $"Assets/Res/FGUI/{fileName}{extension}";
        }
        
        public static string GetNormalConfigPath(string fileName)
        {
            return $"Assets/Res/Config/{fileName}.bytes";
        }
        
        public static string GetSoundPath(string fileName)
        {
            return $"Assets/Res/Sound/{fileName}.prefab";
        }

        public static string GetEffectPath(string fileName)
        {
            return $"Assets/Res/Effect/{fileName}.prefab";
        }
        
        public static string GetSkillIndicatorPath(string fileName)
        {
            return $"Assets/Res/SkillIndicator/{fileName}.prefab";
        }
        
        public static string GetSkillConfigPath(string fileName)
        {
            return $"Assets/Res/Config/SkillConfig/{fileName}.bytes";
        }
        
        public static string GetB2SColliderConfigPath(string fileName)
        {
            return $"Assets/Res/Config/B2SColliderConfig/{fileName}.bytes";
        }
        
        public static string GetUnitPath(string fileName)
        {
            return $"Assets/Res/Unit/{fileName}.prefab";
        }
        
        public static string GetScenePath(string fileName)
        {
            return $"Assets/Res/Scene/{fileName}.unity";
        }
        
        public static string GetHotfixDllPath(string fileName)
        {
            return $"Assets/Res/Code/{fileName}.dll.bytes";
        }
        
        public static string GetHotfixPdbPath(string fileName)
        {
            return $"Assets/Res/Code/{fileName}.pdb.bytes";
        }

        public static string GetUnitAttributeConfigPath(string fileName)
        {
            return $"Assets/Res/Config/UnitAttributeConfig/{fileName}.bytes";
        }

        public static string GetRecastNavDataConfigPath(string fileName)
        {
            return $"Assets/Res/Config/RecastNavConfig/{fileName}.bin.bytes";
        }

        public static string GetUnitAvatatIcon(string UnitName, string iconName)
        {
            return GetTexturePath($"Avatars/{UnitName}/{iconName}");
        }

        public static string GetSkillIcon(string UnitName, string iconName)
        {
            return GetTexturePath($"Skills/{UnitName}/{iconName}");
        }

        public static string GetMaterialPath(string materials)
        {
            return $"Assets/Res/Material/{materials}.mat";
        }
    }
}