using UnityEngine;
using UnityEngine.Events;
using System.Collections;

namespace Slate
{

    [AddComponentMenu("SLATE/Play Cutscene On Click")]
    public class PlayCutsceneOnClick : MonoBehaviour
    {

        public Cutscene cutscene;
        public float startTime;
        public UnityEvent onFinish;

        void OnMouseDown() {

            if ( cutscene == null ) {
                Debug.LogError("Cutscene is not provided", gameObject);
                return;
            }

            cutscene.Play(startTime, () => { onFinish.Invoke(); });
        }

        void Reset() {
            var collider = GetComponent<Collider>();
            if ( collider == null ) {
                collider = gameObject.AddComponent<BoxCollider>();
            }
        }

        public static GameObject Create() {
            var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
            go.name = "Cutscene Click Trigger";
            go.AddComponent<PlayCutsceneOnClick>();
            return go;
        }
    }
}