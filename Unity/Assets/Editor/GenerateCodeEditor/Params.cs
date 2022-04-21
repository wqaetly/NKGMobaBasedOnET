//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2020年9月7日 14:13:46
//------------------------------------------------------------

using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;

namespace ET
{
    public class TemplateAssetPaths
    {
        public const string ETModelComponentTemplatePath = "Assets/Editor/GenerateCodeEditor/ETModelComponentTemplate.txt";
        public const string BuffNodeTemplatePath = "Assets/Editor/GenerateCodeEditor/BuffNodeTemplate.txt";
        public const string BuffDataTemplatePath = "Assets/Editor/GenerateCodeEditor/BuffDataTemplate.txt";
        public const string BuffSystemTemplatePath = "Assets/Editor/GenerateCodeEditor/BuffSystemTemplate.txt";
        public const string NPBehaveActionNodeTemplatePath = "Assets/Editor/GenerateCodeEditor/NPBehaveActionNodeTemplate.txt";
        public const string NPBehaveActionTemplatePath = "Assets/Editor/GenerateCodeEditor/NPBehaveActionTemplate.txt";
    }

    public abstract class AParams_GenerateBase
    {
        [LabelText("生成文件名称")]
        public string FileName;

        [LabelText("目标文件夹")]
        [FolderPath]
        public string FoldName;

        public abstract string TemplateAssetPath { get; }

        public abstract Dictionary<string, string> GetAllParams();
    }

    #region Component

    /// <summary>
    /// 用于生成Component的结构体
    /// </summary>
    public class Params_GenerateETModelComponent: AParams_GenerateBase
    {
        [LabelText("组件名")]
        [InfoBox("示例：Move")]
        public string ComponentName;

        public override string TemplateAssetPath
        {
            get
            {
                return TemplateAssetPaths.ETModelComponentTemplatePath;
            }
        }

        public override Dictionary<string, string> GetAllParams()
        {
            if (string.IsNullOrEmpty(ComponentName))
            {
                Log.Error("组件名不能为空");
                return null;
            }

            return new Dictionary<string, string>() { { "_ComponentName_", ComponentName } };
        }
    }

    #endregion

    #region Buff

    /// <summary>
    /// 用于生成Buff数据结点的结构体
    /// </summary>
    public class Params_GenerateBuffNode: AParams_GenerateBase
    {
        [LabelText("Buff名")]
        [InfoBox("示例：FlashDamgeBuff")]
        public string BuffName;

        [LabelText("Buff描述")]
        [InfoBox("示例：这是一个瞬时伤害Buff")]
        public string BuffDes;

        public override string TemplateAssetPath
        {
            get
            {
                return TemplateAssetPaths.BuffNodeTemplatePath;
            }
        }

        public override Dictionary<string, string> GetAllParams()
        {
            if (string.IsNullOrEmpty(BuffName) || string.IsNullOrEmpty(this.BuffDes))
            {
                Log.Error("Buff名或Buff描述不能为空");
                return null;
            }

            return new Dictionary<string, string>() { { "_BUFFNAME_", BuffName }, { "_BUFFDES_", BuffDes } };
        }
    }

    /// <summary>
    /// 用于生成Buff数据的结构体
    /// </summary>
    public class Params_GenerateBuffData: AParams_GenerateBase
    {
        [LabelText("Buff名")]
        [InfoBox("示例：FlashDamgeBuff")]
        public string BuffName;

        [LabelText("Buff描述")]
        [InfoBox("示例：这是一个瞬时伤害Buff")]
        public string BuffDes;

        public override string TemplateAssetPath
        {
            get
            {
                return TemplateAssetPaths.BuffDataTemplatePath;
            }
        }

        public override Dictionary<string, string> GetAllParams()
        {
            if (string.IsNullOrEmpty(BuffName) || string.IsNullOrEmpty(this.BuffDes))
            {
                Log.Error("Buff名或Buff描述不能为空");
                return null;
            }

            return new Dictionary<string, string>() { { "_BUFFNAME_", BuffName }, { "_BUFFDES_", BuffDes } };
        }
    }

    /// <summary>
    /// 用于生成Buff数据的结构体
    /// </summary>
    public class Params_GenerateBuffSystem: AParams_GenerateBase
    {
        [LabelText("Buff名")]
        [InfoBox("示例：FlashDamgeBuff")]
        public string BuffName;

        public override string TemplateAssetPath
        {
            get
            {
                return TemplateAssetPaths.BuffSystemTemplatePath;
            }
        }

        public override Dictionary<string, string> GetAllParams()
        {
            if (string.IsNullOrEmpty(BuffName))
            {
                Log.Error("Buff名或Buff描述不能为空");
                return null;
            }

            return new Dictionary<string, string>() { { "_BUFFNAME_", BuffName } };
        }
    }

    #endregion

    #region NPBehaveAction

    /// <summary>
    /// 用于生成NPBehave Action Node数据结点的结构体
    /// </summary>
    public class Params_GenerateNPBehaveActionNode: AParams_GenerateBase
    {
        [LabelText("Action名")]
        [InfoBox("示例：LogAction")]
        public string ActionName;

        [LabelText("Action描述")]
        [InfoBox("示例：打印一条消息")]
        public string ActionDes;

        public override string TemplateAssetPath
        {
            get
            {
                return TemplateAssetPaths.NPBehaveActionNodeTemplatePath;
            }
        }

        public override Dictionary<string, string> GetAllParams()
        {
            if (string.IsNullOrEmpty(ActionName) || string.IsNullOrEmpty(this.ActionDes))
            {
                Log.Error("Action名或Action描述不能为空");
                return null;
            }

            return new Dictionary<string, string>() { { "_ACTIONNAME_", ActionName }, { "_ACTIONDES_", ActionDes } };
        }
    }

    /// <summary>
    /// 用于生成NPBehave Action数据结点的结构体
    /// </summary>
    public class Params_GenerateNPBehaveAction: AParams_GenerateBase
    {
        [LabelText("Action名")]
        [InfoBox("示例：LogAction")]
        public string ActionName;

        [LabelText("Action描述")]
        [InfoBox("示例：打印一条消息")]
        public string ActionDes;

        public override string TemplateAssetPath
        {
            get
            {
                return TemplateAssetPaths.NPBehaveActionTemplatePath;
            }
        }

        public override Dictionary<string, string> GetAllParams()
        {
            if (string.IsNullOrEmpty(ActionName) || string.IsNullOrEmpty(this.ActionDes))
            {
                Log.Error("Action名或Action描述不能为空");
                return null;
            }

            return new Dictionary<string, string>() { { "_ACTIONNAME_", ActionName }, { "_ACTIONDES_", ActionDes } };
        }
    }

    #endregion
}