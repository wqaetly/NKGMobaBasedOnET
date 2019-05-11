#if UNITY_EDITOR
namespace Sirenix.OdinInspector.Demos
{
    using UnityEngine;
    using System.Collections.Generic;

    [TypeInfoBox("The AssetList attribute work on both lists of UnityEngine.Object types and UnityEngine.Object types, but have different behaviour.")]
    public class AssetListExamples : MonoBehaviour
    {
        [AssetList]
        [PreviewField(70, ObjectFieldAlignment.Center)]
        public Texture2D SingleObject;

        [AssetList(Path = "/Plugins/Sirenix/")]
        public List<ScriptableObject> AssetList;

        [FoldoutGroup("Filtered AssetLists examples", expanded: false)]
        [AssetList(Path = "Plugins/Sirenix/")]
        public UnityEngine.Object Object;

        [AssetList(AutoPopulate = true)]
        [FoldoutGroup("Filtered AssetLists examples")]
        public List<PrefabRelatedAttributesExamples> AutoPopulatedWhenInspected;

        [AssetList(LayerNames = "MyLayerName")]
        [FoldoutGroup("Filtered AssetLists examples")]
        public GameObject[] AllPrefabsWithLayerName;

        [AssetList(AssetNamePrefix = "Rock")]
        [FoldoutGroup("Filtered AssetLists examples")]
        public List<GameObject> PrefabsStartingWithRock;

        [FoldoutGroup("Filtered AssetLists examples")]
        [AssetList(Tags = "MyTagA, MyTabB", Path = "/Plugins/Sirenix/")]
        public List<GameObject> GameObjectsWithTag;

        [FoldoutGroup("Filtered AssetLists examples")]
        [AssetList(CustomFilterMethod = "HasRigidbodyComponent")]
        public List<GameObject> MyRigidbodyPrefabs;

        private bool HasRigidbodyComponent(GameObject obj)
        {
            return obj.GetComponent<Rigidbody>() != null;
        }
    }
}
#endif
