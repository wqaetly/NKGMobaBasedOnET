

using MonKey;
using MonKey.Editor;
using MonKey.Editor.Commands;
using UnityEditor;
using UnityEngine;

namespace Monkey
{
    class MonkeyMenuItems
    {
        //---------------------------
        // 
        //      Creating / Duplicating / Selecting related command start with CTRL as much as possible
        // 
        //---------------------------

        [MenuItem("Tools/MonKey Commander/Commands/Game Objects/New Instances %i")]
        [MenuItemCommandLink]
        public static void NewInstance()
        {
            MonkeyEditorUtils.CallCommand("New Instances");
        }

        [MenuItem("Tools/MonKey Commander/Commands/Game Objects/Duplicate %#d")]
        [MenuItemCommandLink]
        public static void Duplicate()
        {
            MonkeyEditorUtils.CallCommand("Duplicate");
        }

        [MenuItem("Tools/MonKey Commander/Commands/Game Objects/Duplicate Under Mouse %&#d")]
        [MenuItemCommandLink]
        public static void DuplicateMouseRay()
        {
            MonkeyEditorUtils.CallCommand("Duplicate Under Mouse");
        }

#if UNITY_EDITOR_WIN
        [MenuItem("Tools/MonKey Commander/Commands/Game Objects/New Parent %m")]
#endif
        [MenuItemCommandLink]
        public static void CreateParentForSelection()
        {
            CreationUtilities.ParentSelection();
        }

        [MenuItem("Tools/MonKey Commander/Commands/Game Objects/New Parent %m", true)]
        public static bool ValidateParentSelection()
        {
            return Selection.transforms.Length > 0;
        }

        [MenuItem("Tools/MonKey Commander/Commands/Find/Find Asset %t")]
        [MenuItemCommandLink]
        public static void SelectAssetHotKeyOverride()
        {
            //yes, you can also do it this way!!
            //however, we recommend calling the method directly, as names are subject to changes
            //use it preferably for commands with parameters, to call MonKey's interface
            MonkeyEditorUtils.CallCommand("Find Asset");
        }

        [MenuItem("Tools/MonKey Commander/Commands/Find/Find GameObject %g")]
        [MenuItemCommandLink]
        public static void SelectGameObjectHotKeyOverride()
        {
            MonkeyEditorUtils.CallCommand("Find GameObject");
        }

        [MenuItem("Tools/MonKey Commander/Commands/Prefabs/New Instances Under Mouse &i")]
        [MenuItemCommandLink]
        public static void InstantiateMousePrefab()
        {
            MonkeyEditorUtils.CallCommand("New Instances Under Mouse");
        }

        [MenuItem("Tools/MonKey Commander/Commands/Prefabs/Select Instances %#i")]
        [MenuItemCommandLink]
        public static void SelectPrefabInstances()
        {
            MonkeyEditorUtils.CallCommand("Select Instances");
        }


        [MenuItem("Tools/MonKey Commander/Commands/Game Objects/Select Common Parent %#m")]
        [MenuItemCommandLink]
        public static void SelectParent()
        {
            SelectionUtilities.SelectParent();
        }

        [MenuItem("Tools/MonKey Commander/Commands/Game Objects/Select Siblings %&s")]
        [MenuItemCommandLink]
        public static void SelectSiblings()
        {
            SelectionUtilities.SelectSiblings();
        }

        [MenuItem("Tools/MonKey Commander/Commands/Prefabs/Create Instances Between %&i")]
        [MenuItemCommandLink]
        public static void CreateInstancesBetween()
        {
            MonkeyEditorUtils.CallCommand("New Instances Between");
        }

        [MenuItem("Tools/MonKey Commander/Commands/Game Objects/New Parent For Each %&n")]
        [MenuItemCommandLink]
        public static void NewParentForEach()
        {
            MonkeyEditorUtils.CallCommand("New Parent For Each");
        }

        [MenuItem("Tools/MonKey Commander/Commands/Scenes/New Scene %u")]
        [MenuItemCommandLink]
        public static void NewScene()
        {
            CreationUtilities.CreateNewScene();
        }

        [MenuItem("Tools/MonKey Commander/Commands/Assets/New ScriptableObject %&t")]
        [MenuItemCommandLink]
        public static void NewScriptableObject()
        {
            MonkeyEditorUtils.CallCommand("New Scriptable Object");
        }

        [MenuItem("Tools/MonKey Commander/Commands/Game Objects/Duplicate On Axis %&LEFT")]
        [MenuItemCommandLink]
        public static void DuplicateOnAxis()
        {
            MonkeyEditorUtils.CallCommand("Duplicate On Axis");
        }

        [MenuItem("Tools/MonKey Commander/Commands/Game Objects/Replace Components %/")]
        [MenuItemCommandLink]
        public static void ReplaceComponents()
        {
            MonkeyEditorUtils.CallCommand("Replace Components");
        }

        [MenuItem("Tools/MonKey Commander/Commands/Tools/Unselect All %END")]
        [MenuItemCommandLink]
        public static void UnSelectAll()
        {
            SelectionUtilities.ClearSelection();
        }


        //---------------------------
        //
        //     Moving / Transforming related command are with ALT, as much as possible
        //
        //----------------------------


        [MenuItem("Tools/MonKey Commander/Commands/Game Objects/Copy Transform %#q", true)]
        private static bool ValidationObjectsSelected()
        {
            return Selection.transforms.Length > 0;
        }


        [MenuItem("Tools/MonKey Commander/Commands/Game Objects/Look At &l")]
        [MenuItemCommandLink]
        public static void LookAtOverride()
        {
            if (Selection.gameObjects.Length < 2)
                return;
            MoveUtilities.LookAt(MoveUtilities.FirstSelected(), Vector3.zero, new Axis[0]);
        }

        [MenuItem("Tools/MonKey Commander/Commands/Game Objects/Move Under Mouse &UP")]
        [MenuItemCommandLink]
        public static void MoveMouseRaycast()
        {
            MonkeyEditorUtils.CallCommand("Move Under Mouse");
        }

        [MenuItem("Tools/MonKey Commander/Commands/Game Objects/Copy Position &c")]
        [MenuItemCommandLink]
        public static void CopyPosition()
        {
            MoveUtilities.CopyPosition(DefaultValuesUtilities.DefaultFirstGameObjectSelected());
        }

        [MenuItem("Tools/MonKey Commander/Commands/Game Objects/Copy Rotation &r")]
        [MenuItemCommandLink]
        public static void CopyRotation()
        {
            MoveUtilities.CopyRotation(DefaultValuesUtilities.DefaultFirstGameObjectSelected());
        }

        [MenuItem("Tools/MonKey Commander/Commands/Game Objects/Copy Transform &t")]
        [MenuItemCommandLink]
        public static void CopyTransform()
        {
            MoveUtilities.CopyTransform(DefaultValuesUtilities.DefaultFirstGameObjectSelected());
        }

        [MenuItem("Tools/MonKey Commander/Commands/Game Objects/Move Until Collision &DOWN")]
        [MenuItemCommandLink]
        public static void MoveAxisRayCast()
        {
            MonkeyEditorUtils.CallCommand("Move Until Collision");

        }

        [MenuItem("Tools/MonKey Commander/Commands/Tools/Reset Transforms &#t")]
        [MenuItemCommandLink]
        public static void ResetTransforms()
        {
            MoveUtilities.ResetTransforms();
        }

        [MenuItem("Tools/MonKey Commander/Commands/Tools/Reset Rotations &#r")]
        [MenuItemCommandLink]
        public static void ResetRotations()
        {
            MoveUtilities.ResetRotations();
        }

        [MenuItem("Tools/MonKey Commander/Commands/Game Objects/Set Parent %&m")]
        [MenuItemCommandLink]
        public static void SetParent()
        {
            MonkeyEditorUtils.CallCommand("Set Parent");
        }

        [MenuItem("Tools/MonKey Commander/Commands/Game Objects/Center Pivot &HOME")]
        [MenuItemCommandLink]
        public static void CenterPivot()
        {
            MoveUtilities.MovePivotToChildrenCenter();
        }

        [MenuItem("Tools/MonKey Commander/Commands/Game Objects/Add Components %&+")]
        [MenuItemCommandLink]
        public static void AddComponents()
        {
            MonkeyEditorUtils.CallCommand("Add Components");

        }

        [MenuItem("Tools/MonKey Commander/Commands/Game Objects/Copy Component %&c")]
        [MenuItemCommandLink]
        public static void CopyComponent()
        {
            MonkeyEditorUtils.CallCommand("Copy Component");

        }

#if UNITY_EDITOR_WIN
        [MenuItem("Tools/MonKey Commander/Commands/Game Objects/Paste Component Value %,")]
#endif
        [MenuItemCommandLink]
        public static void PasteComponentValues()
        {
            MonkeyEditorUtils.CallCommand("Paste Component Value");

        }

        [MenuItem("Tools/MonKey Commander/Commands/Game Objects/Paste Component As New %.")]
        [MenuItemCommandLink]
        public static void PasteComponentAsNew()
        {
            MonkeyEditorUtils.CallCommand("Paste Component As New");

        }

        [MenuItem("Tools/MonKey Commander/Commands/Game Objects/Move Pivot &m")]
        [MenuItemCommandLink]
        public static void MovePivot()
        {
            MonkeyEditorUtils.CallCommand("Move Pivot");

        }


        [MenuItem("Tools/MonKey Commander/Commands/Game Objects/Randomize Position &#1")]
        [MenuItemCommandLink]
        public static void RandomizePosition()
        {
            MonkeyEditorUtils.CallCommand("Randomize Position");
        }

        [MenuItem("Tools/MonKey Commander/Commands/Game Objects/Randomize Rotation &#2")]
        [MenuItemCommandLink]
        public static void RandomizeRotation()
        {
            MonkeyEditorUtils.CallCommand("Randomize Rotation");
        }

        [MenuItem("Tools/MonKey Commander/Commands/Game Objects/Randomize Scale &#3")]
        [MenuItemCommandLink]
        public static void RandomizeScale()
        {
            MonkeyEditorUtils.CallCommand("Randomize Scale");
        }

        [MenuItem("Tools/MonKey Commander/Commands/Game Objects/Set Local Position &1")]
        [MenuItemCommandLink]
        public static void SetLocalPosition()
        {
            MonkeyEditorUtils.CallCommand("Set Local Position");
        }

        [MenuItem("Tools/MonKey Commander/Commands/Game Objects/Set Local Rotation &2")]
        [MenuItemCommandLink]
        public static void SetLocalRotation()
        {
            MonkeyEditorUtils.CallCommand("Set Local Rotation");
        }

        [MenuItem("Tools/MonKey Commander/Commands/Game Objects/Set Local Scale &3")]
        [MenuItemCommandLink]
        public static void SetLocalScale()
        {
            MonkeyEditorUtils.CallCommand("Set Local Scale");
        }


        //---------------------------
        //
        //    Others may start with something related to their names, or not, we did our best.
        //
        //----------------------------

        [MenuItem("Tools/MonKey Commander/Commands/Tools/QuickPause _F8")]
        [MenuItemCommandLink]
        public static void QuickPause()
        {
            PlayUtilities.EasyAccessEditorPause();
        }

        [MenuItem("Tools/MonKey Commander/Commands/Game Objects/Toggle Lock %l")]
        [MenuItemCommandLink]
        public static void ToggleLock()
        {
            VisibilityUtilities.ToggleLock();
        }

        [MenuItem("Tools/MonKey Commander/Commands/Game Objects/ Enabled-Disable %&a")]
        [MenuItemCommandLink]
        public static void Tenable()
        {
            VisibilityUtilities.ToggleEnable();
        }

#if !UNITY_2018_1_OR_NEWER

        [MenuItem("Tools/MonKey Commander/Commands/Game Objects/Expand All %&DOWN")]
        [MenuItemCommandLink]
        public static void ExpandAll()
        {
            VisibilityUtilities.ExpandAllChildren();
        }

        [MenuItem("Tools/MonKey Commander/Commands/Game Objects/Collapse All %&UP")]
        [MenuItemCommandLink]
        public static void CollapseAll()
        {
            VisibilityUtilities.CollapseAll();
        }
#endif

        [MenuItem("Tools/MonKey Commander/Commands/Scene/Open Scene %HOME")]
        [MenuItemCommandLink]
        public static void OpenScene()
        {
            MonkeyEditorUtils.CallCommand("Open Scene");
        }

        [MenuItem("Tools/MonKey Commander/Commands/Scene/Activate Scene &#HOME")]
        [MenuItemCommandLink]
        public static void ActivateScene()
        {
            MonkeyEditorUtils.CallCommand("Activate Scene");

        }

        [MenuItem("Tools/MonKey Commander/Commands/Tools/Select Previous %&q")]
        [MenuItemCommandLink]
        public static void SelectPreviousSelection()
        {
            MonkeyEditorUtils.CallCommand("Select Previous Selection");

        }

        [MenuItem("Tools/MonKey Commander/Commands/Find/Select GameObjects With Terms %#g")]
        [MenuItemCommandLink]
        public static void SelectWithTerms()
        {
            MonkeyEditorUtils.CallCommand("Find GameObjects With Terms");
        }

        [MenuItem("Tools/MonKey Commander/Commands/Find/Find GameObject In Children %#&g")]
        [MenuItemCommandLink]
        public static void SelectWithTermsChildren()
        {
            MonkeyEditorUtils.CallCommand("Find GameObject In Children");
        }


        [MenuItem("Tools/MonKey Commander/Commands/Find/Select Assets with Terms %#t")]
        [MenuItemCommandLink]
        public static void SelectWithTermsAssets()
        {
            MonkeyEditorUtils.CallCommand("Select Assets With Terms");
        }

#if UNITY_2017_1_OR_NEWER

        [MenuItem("Tools/MonKey Commander/Commands/Tools/Play Timelines _F5")]
        [MenuItemCommandLink]
        public static void PlayTimelines()
        {
            EditorUtilities.PlayTimelines(true);
        }

        [MenuItem("Tools/MonKey Commander/Commands/Tools/Editor Physics _F4")]
        [MenuItemCommandLink]
        public static void ToggleEditorPhysics()
        {
            EditorUtilities.ToggleEditorPhysics();
        }

        [MenuItem("Tools/MonKey Commander/Commands/Tools/Editor Physics Selected _F3")]
        [MenuItemCommandLink]
        public static void ToggleEditorPhysicsSelected()
        {
            EditorUtilities.ToggleEditorPhysicsSelected();
        }
#endif

        [MenuItem("Tools/MonKey Commander/Commands/Tools/Play Pause Physics _F10")]
        [MenuItemCommandLink]
        public static void PlayPausePhysics()
        {
            PlayUtilities.TogglePausePhysics();
        }

        [MenuItem("Tools/MonKey Commander/Commands/Tools/Slow Motion _F9")]
        [MenuItemCommandLink]
        public static void SlowMotion()
        {
            PlayUtilities.ToggleSloMo(.33f);
        }


        //---------------------------
        //
        //   Renaming related commands are connected to F2 as that's F2's goal in life
        //
        //----------------------------

        [MenuItem("Tools/MonKey Commander/Commands/Renaming/Rename Replace &F2")]
        [MenuItemCommandLink]
        public static void RenameReplace()
        {
            MonkeyEditorUtils.CallCommand("Rename Replace Term");
        }


        [MenuItem("Tools/MonKey Commander/Commands/Renaming/Rename Add Order Number %F2")]
        [MenuItemCommandLink]
        public static void RenameAddOrderNumber()
        {
            MonkeyEditorUtils.CallCommand("Rename Add Order Number");
        }

        [MenuItem("Tools/MonKey Commander/Commands/Renaming/Rename Selection %#F2")]
        [MenuItemCommandLink]
        public static void RenameSelection()
        {
            MonkeyEditorUtils.CallCommand("Rename Selection");
        }

        [MenuItem("Tools/MonKey Commander/Commands/Renaming/Rename Add Order Number %&F2")]
        [MenuItemCommandLink]
        public static void RenameUpdateOrderNumber()
        {
            MonkeyEditorUtils.CallCommand("Rename Add Order Number");
        }

        //---------------------------
        //
        //   UI Related Commands are connected to brackets
        //
        //----------------------------


        [MenuItem("Tools/MonKey Commander/Commands/UI/Toggle Screen UI Visibility &;")]
        [MenuItemCommandLink]
        public static void ToggleScreenUIVisibility()
        {
           UIUtilities.ToggleScreenUIElements();
        }

        [MenuItem("Tools/MonKey Commander/Commands/UI/Anchors To Corners &]")]
        [MenuItemCommandLink]
        public static void AnchorsToCorners()
        {
            UIUtilities.AnchorsToCorners();
        }

        [MenuItem("Tools/MonKey Commander/Commands/UI/Collapse Anchors %&]")]
        [MenuItemCommandLink]
        public static void CollapseAnchors()
        {
            UIUtilities.CollapseAnchors();
        }

        [MenuItem("Tools/MonKey Commander/Commands/UI/Corners To Anchors &[")]
        [MenuItemCommandLink]
        public static void CornersToAnchors()
        {
            UIUtilities.CornersToAnchors();
        }

        [MenuItem("Tools/MonKey Commander/Commands/UI/Expand Anchors &#]")]
        [MenuItemCommandLink]
        public static void ExpandAnchors()
        {
            UIUtilities.ExpandAnchors();
        }

        [MenuItem("Tools/MonKey Commander/Commands/UI/Mirror Horizontally &,")]
        [MenuItemCommandLink]
        public static void MirrorHorizontally()
        {
            MonkeyEditorUtils.CallCommand("UI Mirror Horizontally");
        }

        [MenuItem("Tools/MonKey Commander/Commands/UI/Mirror Vertically &.")]
        [MenuItemCommandLink]
        public static void MirrorVertically()
        {
            MonkeyEditorUtils.CallCommand("UI Mirror Vertically");
        }
    }
}
