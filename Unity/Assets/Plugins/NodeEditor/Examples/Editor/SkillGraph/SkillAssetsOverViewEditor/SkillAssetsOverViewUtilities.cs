//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2021年6月17日 20:12:52
//------------------------------------------------------------

using System.Collections.Generic;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace Plugins.NodeEditor
{
    public static class SkillAssetsOverViewUtilities
    {
        /// <summary>
        /// Key为SkillAssets的全路径
        /// </summary>
        private static readonly Dictionary<string, SkillAssetsOverViewItem> AllSkillAssetsOverViewItems =
                new Dictionary<string, SkillAssetsOverViewItem>();

        private static readonly SkillAssetsOverViewUtilities.CategoryComparer CategorySorter =
                new SkillAssetsOverViewUtilities.CategoryComparer();

        public static void Init()
        {
        }

        static SkillAssetsOverViewUtilities()
        {
            string[] targetsGuids = AssetDatabase.FindAssets("t:SkillGraph");
            foreach (var guid in targetsGuids)
            {
                string skillAssetPath = AssetDatabase.GUIDToAssetPath(guid);
                AllSkillAssetsOverViewItems[skillAssetPath] = new SkillAssetsOverViewItem() { Name = skillAssetPath };
            }
        }

        public static void BuildMenuTree(OdinMenuTree tree)
        {
            foreach (var allTrickOverViewInfo in AllSkillAssetsOverViewItems)
            {
                string[] splitedPath = allTrickOverViewInfo.Key.Split('/');
                OdinMenuItem menuItem =
                        new OdinMenuItem(tree, splitedPath[splitedPath.Length - 1].Split('.')[0], allTrickOverViewInfo.Key)
                        {
                            OnRightClick = HandleRightClick
                        };
                string[] splitedAssetName = splitedPath[splitedPath.Length - 1].Split('_');
                if (splitedAssetName.Length >= 2)
                {
                    tree.AddMenuItemAtPath(splitedAssetName[1], menuItem);
                }
                else
                {
                    tree.AddMenuItemAtPath("Others", menuItem);
                }
            }

            tree.MenuItems.Sort(SkillAssetsOverViewUtilities.CategorySorter);
            tree.MarkDirty();
        }

        private static void HandleRightClick(OdinMenuItem obj)
        {
            GenericMenu genericMenu = new GenericMenu();
            genericMenu.AddItem(new GUIContent("在技能编辑器中打开"), false,
                () =>
                {
                    EditorWindow.GetWindow<SkillGraphWindow>().InitializeGraph(AssetDatabase.LoadAssetAtPath<SkillGraph>(obj.Value as string));
                });
            genericMenu.AddItem(new GUIContent("在项目资源管理器高亮"), false, () =>
            {
                SkillAssetsOverViewItem skillAssetsOverViewItem = GetSkillAssetsOverViewItemByPath(obj.Value as string);
                skillAssetsOverViewItem.SkillGraph = AssetDatabase.LoadAssetAtPath<SkillGraph>(obj.Value as string);
                EditorGUIUtility.PingObject(GetSkillAssetsOverViewItemByPath(obj.Value as string).SkillGraph);
            });
            genericMenu.DropDown(new Rect(Event.current.mousePosition, Vector2.zero));
        }

        private class CategoryComparer: IComparer<OdinMenuItem>
        {
            // Token: 0x06001195 RID: 4501 RVA: 0x00055E98 File Offset: 0x00054098
            public int Compare(OdinMenuItem x, OdinMenuItem y)
            {
                int num;
                if (!SkillAssetsOverViewUtilities.CategoryComparer.Order.TryGetValue(x.Name, out num))
                {
                    num = 0;
                }

                int num2;
                if (!SkillAssetsOverViewUtilities.CategoryComparer.Order.TryGetValue(y.Name, out num2))
                {
                    num2 = 0;
                }

                if (num == num2)
                {
                    return x.Name.CompareTo(y.Name);
                }

                return num.CompareTo(num2);
            }

            // Token: 0x040009C3 RID: 2499
            private static readonly Dictionary<string, int> Order = new Dictionary<string, int>
            {
                { "Essentials", -10 },
                { "Misc", 8 },
                { "Meta", 9 },
                { "Unity", 10 },
                { "Debug", 50 }
            };
        }

        public static SkillAssetsOverViewItem GetSkillAssetsOverViewItemByPath(string name)
        {
            if (AllSkillAssetsOverViewItems.TryGetValue(name, out var skillAssetsOverViewItem))
            {
                return skillAssetsOverViewItem;
            }
            else
            {
                Debug.LogError($"未找到{name}");
                return null;
            }
        }

        public static Dictionary<string, SkillAssetsOverViewItem> GetAllSkillAssetsOverViewItems()
        {
            return AllSkillAssetsOverViewItems;
        }
    }
}