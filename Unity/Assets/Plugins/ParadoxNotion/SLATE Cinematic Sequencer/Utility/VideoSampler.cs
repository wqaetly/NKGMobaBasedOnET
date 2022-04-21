#if UNITY_2017_2_OR_NEWER

using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Video;

namespace Slate
{

    ///Samples/Plays an VideoClip and manages VideoPlayer instances
    public static class VideoSampler
    {

        ///Destination of the video playback
        public enum VideoRenderTarget
        {
            CameraBackground = 0,
            CameraForeground = 1,
        }

        ///Settings used when sampling video
        [System.Serializable]
        public struct SampleSettings
        {
            public VideoRenderTarget renderTarget;
            public float playbackSpeed;
            public float alpha;
            public VideoAspectRatio aspectRatio;
            public float audioVolume;
            public static SampleSettings Default() {
                var settings = new SampleSettings();
                settings.renderTarget = VideoRenderTarget.CameraBackground;
                settings.playbackSpeed = 1;
                settings.alpha = 1;
                settings.aspectRatio = VideoAspectRatio.FitHorizontally;
                settings.audioVolume = 1;
                return settings;
            }
        }

        private const string ROOT_NAME = "_VideoSources";
        private static GameObject root;
        private static Dictionary<object, VideoPlayer> sources = new Dictionary<object, VideoPlayer>();

        ///Get an VideoPlayer for the specified key ID object
        public static VideoPlayer GetSourceForID(object keyID) {
            VideoPlayer source = null;
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

            var newSource = new GameObject("_VideoSource").AddComponent<VideoPlayer>();
            newSource.transform.SetParent(root.transform);
            newSource.source = VideoSource.VideoClip;
            newSource.playOnAwake = false;
            newSource.targetCamera = DirectorCamera.renderCamera;
            newSource.audioOutputMode = VideoAudioOutputMode.AudioSource;
            newSource.SetTargetAudioSource(0, AudioSampler.GetSourceForID(newSource));
            return sources[keyID] = newSource;
        }

        ///Release/Destroy an VideoPlayer for the specified key ID object
        public static void ReleaseSourceForID(object keyID) {
            VideoPlayer source = null;
            if ( sources.TryGetValue(keyID, out source) ) {
                if ( source != null ) {
                    AudioSampler.ReleaseSourceForID(source);
                    Object.DestroyImmediate(source.gameObject);
                }
                sources.Remove(keyID);
            }

            if ( sources.Count == 0 ) {
                Object.DestroyImmediate(root);
            }
        }


        ///Sample an VideoClip on the VideoPlayer of the specified key ID object
        public static void SampleForID(object keyID, VideoClip clip, float time, float previousTime, SampleSettings settings) {
            var source = GetSourceForID(keyID);
            Sample(source, clip, time, previousTime, settings);
        }

        ///Sample an VideoClip in the specified VideoPlayer directly
        public static void Sample(VideoPlayer source, VideoClip clip, float time, float previousTime, SampleSettings settings) {

            if ( source == null ) {
                return;
            }

            if ( previousTime == time ) {
                source.time = time;
                source.Pause();
                return;
            }

            source.clip = clip;
            source.renderMode = (VideoRenderMode)settings.renderTarget;
            source.playbackSpeed = settings.playbackSpeed;
            source.targetCameraAlpha = settings.alpha;
            source.aspectRatio = settings.aspectRatio;
            source.GetTargetAudioSource(0).volume = settings.audioVolume;

            time = Mathf.Repeat(time, (float)clip.length - 0.001f);

            if ( !source.isPlaying ) {
                source.Play();
                source.time = time;
            }

            if ( Mathf.Abs((float)source.time - time) > 0.1f * Time.timeScale ) {
                source.time = time;
            }
        }
    }
}

#endif