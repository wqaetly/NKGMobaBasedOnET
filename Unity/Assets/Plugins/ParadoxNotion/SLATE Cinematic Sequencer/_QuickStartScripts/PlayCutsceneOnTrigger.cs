using UnityEngine;
using UnityEngine.Events;
using System.Collections;

namespace Slate
{

    [AddComponentMenu("SLATE/Play Cutscene On Trigger")]
    public class PlayCutsceneOnTrigger : MonoBehaviour
    {

        public Cutscene cutscene;
        public float startTime;
        public bool checkSpecificTagOnly = true;
        public string tagName = "Player";
        public bool once;
        public UnityEvent onFinish;

        void OnTriggerEnter(Collider other) {

            if ( cutscene == null ) {
                Debug.LogError("Cutscene is not provided", gameObject);
                return;
            }

            if ( checkSpecificTagOnly && !string.IsNullOrEmpty(tagName) ) {
                if ( other.gameObject.tag != tagName ) {
                    return;
                }
            }

            enabled = false;
            cutscene.Play(startTime, () =>
           {
               onFinish.Invoke();
               if ( once ) { Destroy(this.gameObject); } else { enabled = true; }
           });
        }

        void Reset() {
            var collider = gameObject.GetAddComponent<BoxCollider>();
            collider.isTrigger = true;
        }

        public static GameObject Create() {
            return new GameObject("Cutscene Trigger").AddComponent<PlayCutsceneOnTrigger>().gameObject;
        }
    }
}