using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using BlackTree.Common;
using DG.Tweening;
using System.Runtime.InteropServices;

namespace BlackTree
{
    public enum SoundType
    {
        Fire=1,
        BGM,

        None,
    }

    public class SoundManager : MonoSingleton<SoundManager>
    {
        public float volume = 1.0f;
        public float fadeDuration = 3.0f;

        BT_Sound _table;

        class ClipCache
        {
            public BT_Sound.Param data;
            public AudioClip clip;
            public bool instant;
            public bool bgm;
        }

        readonly Dictionary<int, ClipCache> _caches = new Dictionary<int, ClipCache>();

        readonly ObjectPool<AudioSource> _audioSourcePool = new ObjectPool<AudioSource>();

        public Dictionary<float, AudioSource> _effectAudioDict = new Dictionary<float, AudioSource>();

        private const int BGCount=1;
        class PlayingAudio
        {
            public ClipCache clipCache;
            public AudioSource audioSource;
        }
        readonly LinkedList<PlayingAudio> _playingAudio = new LinkedList<PlayingAudio>();

        public IEnumerator Setup()
        {
            _table = TableManager.Instance.GetTableClass<BT_Sound>();

            for (int i = 0; i < _table.sheets[0].list.Count; i++)
                yield return StartCoroutine(Load(_table.sheets[0].list[i].Index, true, _table.sheets[0].list[i].Loop));

            if (_effectAudioDict != null)
                _effectAudioDict = new Dictionary<float, AudioSource>();

            Message.AddListener<BlackTree.Global.Event.PlayEffectSoundMsg>(OnPlaySoundMsg);
        }

        public IEnumerator Load(int id, bool instant = true, bool bgm = false)
        {
            if (id == 0)
                yield break;

            if (_caches.ContainsKey(id))
                yield break;

            if (_table == null)
            {
                _table = TableManager.Instance.GetTableClass<BT_Sound>();
                if (_table == null)
                {
                    yield return StartCoroutine(TableManager.Instance.Load());
                    _table = TableManager.Instance.GetTableClass<BT_Sound>();

                    if (_table == null)
                        yield break;
                }
            }

            var data = _table.sheets[0].list.Find(x => x.Index == id);
            if (data == null)
            {
#if UNITY_EDITOR
                Debug.LogErrorFormat("Could not found 'BT_SoundRow' : {0} of {1}", id, gameObject.name);
#endif
                yield break;
            }

            var fullpath = string.Format("{0}/{1}", data.FilePath, data.FileName);
            yield return StartCoroutine(ResourceLoader.Instance.Load<AudioClip>(fullpath,
                o => OnPostLoadProcess(o, id, data, instant, bgm)));
        }

        void OnPostLoadProcess(Object o, int id, BT_Sound.Param data, bool instant, bool bgm)
        {
            if (!_caches.ContainsKey(id))
            {
                var sound = bgm ? o as AudioClip : Instantiate(o) as AudioClip;
                _caches.Add(id, new ClipCache { data = data, clip = sound, instant = instant, bgm = bgm });
            }
        }

        private void OnPlaySoundMsg(BlackTree.Global.Event.PlayEffectSoundMsg msg)
        {
            if (msg.SoundType == SoundType.None)
                return;

            this.PlaySound((int)msg.SoundType, false);
        }

        public void PlaySound(int id, bool fade = false)
        {
            ClipCache cache;
            if (_caches.TryGetValue(id, out cache))
            {
                AudioSource source;
                if(cache.bgm)
                {
                    source = _audioSourcePool.GetObject() ?? gameObject.AddComponent<AudioSource>();
                    _playingAudio.AddLast(new PlayingAudio { clipCache = cache, audioSource = source });

                    source.clip = cache.clip;
                    source.loop = cache.data.Loop;
                    source.volume = fade ? 0.0f : volume;

                    source.Play();

                    if (fade)
                        source.DOFade(cache.data.Volum, fadeDuration);
                }
                else
                {
                    if (!_effectAudioDict.ContainsKey(cache.data.Volum))
                    {
                        var newSource = gameObject.AddComponent<AudioSource>();
                        newSource.volume = volume;
                        _effectAudioDict.Add(cache.data.Volum, newSource);
                    }

                    _effectAudioDict[cache.data.Volum].PlayOneShot(cache.clip);
                }
            }
        }

        public void StopSound(int id, bool fade)
        {
            var node = _playingAudio.First;
            while (node != null)
            {
                var audio = node.Value;
                if (audio.clipCache.data.Index == id)
                {
                    if (fade)
                    {
                        audio.audioSource.DOFade(0.0f, fadeDuration).OnComplete(
                            () =>
                            {
                                audio.audioSource.Stop();
                                audio.audioSource.clip = null;
                            });
                    }
                    else
                    {
                        audio.audioSource.Stop();
                        audio.audioSource.clip = null;
                    }
                    break;
                }
                node = node.Next;
            }
        }

        void LateUpdate()
        {
            var node = _playingAudio.First;
            while (node != null)
            {
                var audio = node.Value;
                if (audio.clipCache.instant && !audio.audioSource.isPlaying)
                {
                    // if (!audio.audioSource.loop)
                    {
                        audio.audioSource.Stop();
                        audio.audioSource.clip = null;

                        _audioSourcePool.PoolObject(audio.audioSource);
                        _playingAudio.Remove(node);
                    }
                }
                if(audio.audioSource.isPlaying)
                {
                    if(audio.audioSource.volume!=volume)
                    {
                        audio.audioSource.volume = volume;
                    }
                }

                node = node.Next;
            }
        }

        protected override void Release()
        {
            Message.RemoveListener<BlackTree.Global.Event.PlayEffectSoundMsg>(OnPlaySoundMsg);

            StopAllSound();
            UnloadAllLoadCaches();
        }

        public void StopAllSound()
        {
            var node = _playingAudio.First;
            while (node != null)
            {
                var audio = node.Value;
                audio.audioSource.Stop();
                // audio.audioSource.clip = null;
                node = node.Next;
            }
        }

        public void UnloadAllLoadCaches()
        {
            foreach (var cache in _caches)
            {
                if (!cache.Value.bgm)
                    Destroy(cache.Value.clip);

                cache.Value.clip = null;
            }

            _caches.Clear();
        }

    }
}

