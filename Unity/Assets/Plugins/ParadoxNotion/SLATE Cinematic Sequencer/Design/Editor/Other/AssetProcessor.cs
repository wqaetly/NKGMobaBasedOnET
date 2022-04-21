#if UNITY_EDITOR

using UnityEditor;

namespace Slate
{

    public class AssetProcessor : AssetPostprocessor
    {

        //We stop the cutscene preview if any before importing new model because it causes crash if animations used by cutscene are re-imported.
        void OnPreprocessModel() {
            if ( CutsceneEditor.current != null ) {
                CutsceneEditor.current.Stop(true);
            }
        }
    }
}

#endif