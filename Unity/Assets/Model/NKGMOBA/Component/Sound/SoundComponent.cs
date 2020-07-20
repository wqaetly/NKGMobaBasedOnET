using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ETModel
{
    [ObjectSystem]
    public class SoundComponentAwakeSystem : AwakeSystem<SoundComponent>
    {
        public override void Awake(SoundComponent self)
        {
            self.Awake();
        }
    }

    /// <summary>
    /// 游戏音效管理组件
    /// </summary>
    public class SoundComponent : Component
    {
        /// <summary>
        /// 控制游戏全局音量
        /// </summary>
        public float SoundVolume
        {
            get { return _soundVolume; }
            set
            {
                _soundVolume = Mathf.Clamp(value, 0, 1);
                foreach (SoundData clip in m_clips.Values)
                {
                    clip.Volume = _soundVolume * clip.volume;
                }
            }
        }

        private float _soundVolume = 0.8f;

        //所有音效
        private Dictionary<string, SoundData> m_clips = new Dictionary<string, SoundData>();

        //根据类型分类所有音效
        private Dictionary<SoundType, Dictionary<string, SoundData>> _allClips =
            new Dictionary<SoundType, Dictionary<string, SoundData>>()
            {
                {SoundType.Music, new Dictionary<string, SoundData>()},
                {SoundType.Sound, new Dictionary<string, SoundData>()}
            };

        //catch ab资源
        private static Dictionary<string, SoundData> abSounds = new Dictionary<string, SoundData>();

        //根物体
        private Transform root;

        /// <summary>
        /// 音乐静音
        /// </summary>
        public bool MusicMute
        {
            get { return _musicMute; }
            set
            {
                _musicMute = value;
                foreach (var soundData in _allClips[SoundType.Music].Values)
                {
                    soundData.Mute = _musicMute;
                }

                PlayerPrefs.SetInt("MusicMute", value ? 1 : 0);
            }
        }

        private bool _musicMute = false;

        /// <summary>
        /// 音效静音
        /// </summary>
        public bool SoundMute
        {
            get { return _soundMute; }
            set
            {
                _soundMute = value;
                foreach (var soundData in _allClips[SoundType.Sound].Values)
                {
                    soundData.Mute = _soundMute;
                }

                PlayerPrefs.SetInt("SoundMute", value ? 1 : 0);
            }
        }

        private bool _soundMute = false;

        public void Awake()
        {
            ResourcesComponent resourcesComponent = Game.Scene.GetComponent<ResourcesComponent>();
            resourcesComponent.LoadBundle("sounds.unity3d");

            _musicMute = PlayerPrefs.GetInt("MusicMute", 0) == 1;
            _soundMute = PlayerPrefs.GetInt("SoundMute", 0) == 1;

            root = new GameObject("SoundDatas").transform;
            GameObject.DontDestroyOnLoad(root.gameObject);
        }

        private bool IsContainClip(string clipName)
        {
            lock (m_clips)
            {
                if (m_clips.ContainsKey(clipName))
                    return true;
                return false;
            }
        }

        private SoundData GetAudioSource(string clipName)
        {
            if (IsContainClip(clipName))
                return m_clips[clipName];
            return null;
        }

        private void AddClip(string clipName, SoundData data, SoundType type)
        {
            lock (m_clips)
            {
                data.IsPause = false;
                data.transform.transform.SetParent(root);
                data.Sound = type;
                if (IsContainClip(clipName))
                {
                    m_clips[clipName] = data;
                    _allClips[type][clipName] = data;
                }
                else
                {
                    m_clips.Add(clipName, data);
                    _allClips[type].Add(clipName, data);
                }
            }
        }

        /// <summary>
        /// 短暂的声音和特效
        /// 无法暂停
        /// 异步加载音效
        /// </summary>
        public async void PlayClip(string clipName, float volume = 1)
        {
            SoundData sd = await LoadSound(clipName);
            if (sd != null)
            {
                sd.volume = Mathf.Clamp(volume, 0, 1);
                sd.Mute = SoundMute;
                if (!IsContainClip(clipName))
                {
                    AddClip(clipName, sd, SoundType.Sound);
                }

                PlayMusic(clipName, sd);
            }
            else
            {
                Log.Error($"没有此音效 ={clipName}");
            }
        }

        /// <summary>
        /// 播放长音乐 背景音乐等
        /// 可以暂停 继续播放
        /// 异步加载音效
        /// </summary>
        /// <param name="clipName">声音的预设名字(不包括前缀路径名)</param>
        /// <param name="delay">延迟播放 单位秒</param>
        /// <param name="volume">音量</param>
        /// <param name="isloop">是否循环播放</param>
        /// /// <param name="forceReplay">是否强制重头播放</param>
        public async void PlayMusic(string clipName, ulong delay = 0, float volume = 1, bool isloop = false,
            bool forceReplay = false)
        {
            SoundData sd = await LoadSound(clipName);
            if (sd != null)
            {
                sd.isForceReplay = forceReplay;
                sd.isLoop = isloop;
                sd.delay = delay;
                sd.volume = Mathf.Clamp(volume, 0, 1);
                sd.Mute = MusicMute;
                if (!IsContainClip(clipName))
                {
                    AddClip(clipName, sd, SoundType.Music);
                }

                PlayMusic(clipName, sd);
            }
            else
            {
                Log.Error($"没有此音效 ={clipName}");
            }
        }

        //加载声音
        private async ETTask<SoundData> LoadSound(string soundName)
        {
            ResourcesComponent resourcesComponent = Game.Scene.GetComponent<ResourcesComponent>();
            if (!abSounds.ContainsKey(soundName) || abSounds[soundName] == null)
            {
                abSounds.Add(soundName,
                    UnityEngine.Object.Instantiate(
                            ((GameObject) resourcesComponent.GetAsset("sounds.unity3d", "Sounds"))
                            .Get<GameObject>(soundName))
                        .GetComponent<SoundData>());
            }

            return abSounds[soundName];
        }

        //播放SoundData
        private void PlayMusic(string clipName, SoundData asource)
        {
            if (null == asource)
                return;
            bool forceReplay = asource.isForceReplay;
            asource.audio.volume = asource.volume * SoundVolume;
            asource.audio.loop = asource.isLoop;
            if (!forceReplay)
            {
                if (!asource.IsPlaying)
                {
                    if (!asource.IsPause)
                        asource.audio.Play(asource.delay);
                    else
                        Resume(clipName);
                }
            }
            else
            {
                asource.audio.PlayDelayed(asource.delay);
                asource.audio.PlayScheduled(0);
            }
        }

        /// <summary>
        /// 停止并销毁声音
        /// </summary>
        /// <param name="clipName"></param>
        public void Stop(string clipName)
        {
            SoundData data = GetAudioSource(clipName);
            if (null != data)
            {
                if (_allClips[data.Sound].ContainsKey(clipName))
                {
                    _allClips[data.Sound].Remove(clipName);
                }

                m_clips.Remove(clipName);
                abSounds.Remove(clipName);
                data.Dispose();
            }
        }

        /// <summary>
        /// 暂停声音
        /// </summary>
        /// <param name="clipName"></param>
        public void Pause(string clipName)
        {
            SoundData data = GetAudioSource(clipName);
            if (null != data)
            {
                data.IsPause = true;
                data.audio.Pause();
            }
        }

        /// <summary>
        /// 继续播放
        /// </summary>
        /// <param name="clipName"></param>
        public void Resume(string clipName)
        {
            SoundData data = GetAudioSource(clipName);
            if (null != data)
            {
                data.IsPause = false;
                data.audio.UnPause();
            }
        }

        /// <summary>
        /// 销毁所有声音
        /// </summary>
        public void DisposeAll()
        {
            foreach (var allClip in _allClips.Values)
            {
                allClip.Clear();
            }

            foreach (var item in m_clips)
            {
                item.Value.Dispose();
            }

            m_clips.Clear();
        }
    }
}