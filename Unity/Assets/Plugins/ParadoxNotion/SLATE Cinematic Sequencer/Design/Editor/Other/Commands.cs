#if UNITY_EDITOR

using System;
using System.Linq;
using UnityEngine;
using UnityEditor;


namespace Slate
{

    public static class Commands
    {

        [MenuItem("Tools/ParadoxNotion/SLATE/Open SLATE Editor", false, 0)]
        public static void OpenDirectorWindow() {
            CutsceneEditor.ShowWindow(null);
        }

        [MenuItem("Tools/ParadoxNotion/SLATE/Create New Cutscene", false, 0)]
        public static Cutscene CreateCutscene() {
            var cutscene = Cutscene.Create();
            CutsceneEditor.ShowWindow(cutscene);
            Selection.activeObject = cutscene;
            return cutscene;
        }

        [MenuItem("Tools/ParadoxNotion/SLATE/Visit Website", false, 0)]
        public static void VisitWebsite() {
            Help.BrowseURL("http://slate.paradoxnotion.com");
        }

        [MenuItem("Tools/ParadoxNotion/SLATE/Extra/Create Shot Camera")]
        public static ShotCamera CreateShot() {
            var shot = ShotCamera.Create();
            Selection.activeObject = shot;
            return shot;
        }

        [MenuItem("Tools/ParadoxNotion/SLATE/Extra/Create Bezier Path")]
        public static Path CreateBezierPath() {
            var path = BezierPath.Create();
            Selection.activeObject = path;
            return path;
        }
        /*
        #if !NO_UTJ
                [MenuItem("Tools/ParadoxNotion/SLATE/Extra/Import Alembic File")]
                public static void ImportAlembicDialog(){
                    UTJ.Alembic.AlembicManualImporterEditor.ShowWindow();
                }
        #endif
        */
        [MenuItem("Tools/ParadoxNotion/SLATE/Extra/Create Cutscene Starter")]
        public static GameObject CreateCutsceneStartPlayer() {
            var go = PlayCutsceneOnStart.Create();
            Selection.activeObject = go;
            return go.gameObject;
        }

        [MenuItem("Tools/ParadoxNotion/SLATE/Extra/Create Cutscene Zone Trigger")]
        public static GameObject CreateCutsceneTriggerPlayer() {
            var go = PlayCutsceneOnTrigger.Create();
            Selection.activeObject = go;
            return go.gameObject;
        }

        [MenuItem("Tools/ParadoxNotion/SLATE/Extra/Create Cutscene Click Trigger")]
        public static GameObject CreateCutsceneClickPlayer() {
            var go = PlayCutsceneOnClick.Create();
            Selection.activeObject = go;
            return go.gameObject;
        }

        [MenuItem("Tools/ParadoxNotion/SLATE/Extra/Create Cutscenes Sequence Player")]
        public static GameObject CreateCutscenesSequencePlayer() {
            var go = CutsceneSequencePlayer.Create();
            Selection.activeObject = go;
            return go.gameObject;
        }
    }
}

#endif