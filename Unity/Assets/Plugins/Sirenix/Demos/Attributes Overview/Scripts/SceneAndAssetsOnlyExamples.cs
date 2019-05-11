#if UNITY_EDITOR
namespace Sirenix.OdinInspector.Demos
{
    using UnityEngine;
    using System.Collections.Generic;

    public class SceneAndAssetsOnlyExamples : MonoBehaviour
    {
        [Title("Assets only")]
        [AssetsOnly]
        public List<GameObject> OnlyPrefabs;

        [AssetsOnly]
        public GameObject SomePrefab;

        [AssetsOnly]
        public Material MaterialAsset;

        [AssetsOnly]
        public MeshRenderer SomeMeshRendererOnPrefab;

        [Title("Scene Objects only")]
        [SceneObjectsOnly]
        public List<GameObject> OnlySceneObjects;

        [SceneObjectsOnly]
        public GameObject SomeSceneObject;

        [SceneObjectsOnly]
        public MeshRenderer SomeMeshRenderer;
    }
}
#endif
