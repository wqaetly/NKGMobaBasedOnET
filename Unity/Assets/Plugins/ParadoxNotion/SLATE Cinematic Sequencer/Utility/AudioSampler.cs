using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Slate
{

    ///Samples/Plays an AudioClip and manages AudioSource instances
    public static class AudioSampler
    {

        ///Settings used when sampling audio
        [System.Serializable]
        public struct SampleSettings
        {
            public float volume;
            public float pitch;
            public float pan;
            public float spatialBlend;
            public bool ignoreTimescale;
            public static SampleSettings Default() {
                var settings = new SampleSettings();
                settings.volume = 1;
                settings.pitch = 1;
                settings.pan = 0;
                settings.spatialBlend = 0;
                settings.ignoreTimescale = false;
                return settings;
            }
        }

        private const string ROOT_NAME = "_AudioSources";
        private static GameObject root;
        private static Dictionary<object, AudioSource> sources = new Dictionary<object, AudioSource>();

        ///Get an AudioSource for the specified key ID object
        public static AudioSource GetSourceForID(object keyID) {
            AudioSource source = null;
            if ( sources.TryGetValue(keyID, out source) ) {
                if ( source != null ) {
                    return source;
                }
            }

            if ( root == null ) {
                root = GameObject.Find(ROOT_NAME);
                if ( root == null ) {
                    root = new GameObject(ROOT_NAME);
                }
            }

            var newSource = new GameObject("_AudioSource").AddComponent<AudioSource>();
            newSource.transform.SetParent(root.transform);
            newSource.playOnAwake = false;
            return sources[keyID] = newSource;
        }

        ///Release/Destroy an AudioSource for the specified key ID object
        public static void ReleaseSourceForID(object keyID) {
            AudioSource source = null;
            if ( sources.TryGetValue(keyID, out source) ) {
                if ( source != null ) {
                    Object.DestroyImmediate(source.gameObject);
                }
                sources.Remove(keyID);
            }

            if ( sources.Count == 0 ) {
                Object.DestroyImmediate(root);
            }
        }


        ///Sample an AudioClip on the AudioSource of the specified key ID object
        public static void SampleForID(object keyID, AudioClip clip, float time, float previousTime, float volume) {
            var settings = SampleSettings.Default();
            settings.volume = volume;
            SampleForID(keyID, clip, time, previousTime, settings);
        }

        ///Sample an AudioClip on the AudioSource of the specified key ID object
        public static void SampleForID(object keyID, AudioClip clip, float time, float previousTime, SampleSettings settings) {
            var source = GetSourceForID(keyID);
            Sample(source, clip, time, previousTime, settings);
        }

        ///Sample an AudioClip in the specified AudioSource directly
        public static void Sample(AudioSource source, AudioClip clip, float time, float previousTime, float volume) {
            var settings = SampleSettings.Default();
            settings.volume = volume;
            Sample(source, clip, time, previousTime, settings);
        }

        ///Sample an AudioClip in the specified AudioSource directly
        public static void Sample(AudioSource source, AudioClip clip, float time, float previousTime, SampleSettings settings) {

            if ( source == null ) {
                return;
            }

            if ( previousTime == time ) {
                source.Stop();
                return;
            }

            // if ( !settings.loop && ( time < 0 || time > clip.length ) ) {
            //     if ( source.isPlaying ) { source.Stop(); }
            //     return;
            // }

            source.clip = clip;
            source.volume = Mathf.Clamp(settings.volume, 0, 1);
            source.pitch = Mathf.Clamp(settings.ignoreTimescale ? settings.pitch : settings.pitch * Time.timeScale, -3, 3);
            source.panStereo = Mathf.Clamp(settings.pan, -1, 1);

            time = Mathf.Repeat(time, clip.length - 0.001f);

            if ( !source.isPlaying ) {
                source.time = time;
                source.Play();
            }

            if ( !settings.ignoreTimescale ) {
                if ( Mathf.Abs(source.time - time) > 0.1f * Time.timeScale ) {
                    source.time = time;
                }
            }
        }
    }
}