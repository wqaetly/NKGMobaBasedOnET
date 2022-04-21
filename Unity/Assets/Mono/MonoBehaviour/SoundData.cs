using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace ET
{
    [RequireComponent(typeof(AudioSource))]
    public class SoundData : MonoBehaviour
    {
        //音频源控件
        public new AudioSource AudioSource;

        /// <summary>
        /// 是否强制重新播放
        /// </summary>
        [HideInInspector]
        public bool isForceReplay = false;

        /// <summary>
        /// 是否循环播放
        /// </summary>
        [HideInInspector]
        public bool isLoop = false;

        /// <summary>
        /// 音量
        /// </summary>
        [HideInInspector]
        public float volume = 1;

        /// <summary>
        /// 延迟
        /// </summary>
        [HideInInspector]
        public ulong delay = 0;

        public AudioSource GetAudio()
        {
            return AudioSource;
        }

        public bool IsPlaying
        {
            get
            {
                return AudioSource != null && AudioSource.isPlaying;
            }
        }
        public bool IsPause
        {
            get;
            set;
        }

        /// <summary>
        /// 音效类型
        /// </summary>
        public SoundType Sound { get; set; }

        public bool Mute
        {
            get { return AudioSource.mute; }
            set { AudioSource.mute = value; }
        }

        public float Volume
        {
            get { return AudioSource.volume; }
            set { AudioSource.volume = value; }
        }

        private void Reset()
        {
            this.AudioSource = this.gameObject.GetComponent<AudioSource>();
        }
    }

    /// <summary>
    /// 音效类型
    /// </summary>
    public enum SoundType
    {
        Music,//长音乐
        Sound,//短音乐
    }
}