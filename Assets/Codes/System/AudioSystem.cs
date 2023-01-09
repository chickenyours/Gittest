using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QFramework;
using UnityEngine;

namespace QFramework
{
    public struct StopBgmEvent
    {
        public bool isStop ;
    }
    public struct PauseBgmEvent { }
    public interface IAudioMgrSystem : ISystem
    {
        void PlayBgm(string name);
        void PlaySound(string name);
        void GetSound(string name, Action<AudioSource> callBack);
        void StopSound(AudioSource source);
        void StopPlay(AudioSource source);
        void StopPlayBgm();
        BindableProperty<float> SoundVolume { get; }
        BindableProperty<float> BgmVolume { get; }
    }
    public class AudioSystem : AbstractSystem, IAudioMgrSystem
    {
        public static AudioSystem instance;
        public BindableProperty<float> SoundVolume { get; set; } = new BindableProperty<float>(1);
        public BindableProperty<float> BgmVolume { get; set; } = new BindableProperty<float>(1);
        private AudioSource mBgm;
        private AudioSource tempSound;
        private ResPool<AudioClip> mClipPool;
        private FadeNum mFade;
        private ComponentPool<AudioSource> mSourcePool;
        protected override void OnInit()
        {
            //判断单例
            if (AudioSystem.instance == null || AudioSystem.instance == this) AudioSystem.instance = this;
            else return;
            tempSound = new AudioSource();
            mSourcePool = new ComponentPool<AudioSource>("GameSound");
            mClipPool = new ResPool<AudioClip>();
            //淡化渐入模块
            mFade = new FadeNum();
            mFade.SetMaxMin(BgmVolume.Value,0);

            this.RegisterEvent<StopBgmEvent>(OnStopBgm);

            SoundVolume.RegisterWithInitValue(OnSoundVolumeChanged); 
            BgmVolume.RegisterWithInitValue(OnBgmVolumeChanged);
        }
        private void OnBgmVolumeChanged(float v)
        {
            mFade.SetMaxMin(v,0);
            if(mBgm) mBgm.volume = BgmVolume.Value;
        }
        private void OnSoundVolumeChanged(float v)
        {
            mSourcePool.SetAllComponent(audiosource =>
            {
                audiosource.volume = SoundVolume.Value;
            });
        }
        private void OnStopBgm(StopBgmEvent e)
        {
            PublicMono.Instance.OnUpdate += Update;
            if (mBgm == null || !mBgm.isPlaying) return;
            mFade.SetState(FadeState.FadeOut, () =>
            {
                if(e.isStop) mBgm.Stop(); 
                else  mBgm.Pause(); 
            });
            
        }
        private void Update()
        {
            if (!mFade.IsEnable) return;
            mFade.Update(Time.deltaTime);
            mBgm.volume = mFade.CorrentValue;
        }
        public void PlayBgm(string name)
        {
            if(mBgm == null)
            {
                MonoBehaviour.print("creat a gameobject: GameBgm");
                GameObject o = new GameObject("GameBgm");
                GameObject.DontDestroyOnLoad(o);
                mBgm = o.AddComponent<AudioSource>();
                mBgm.loop = true;
                mBgm.volume = 0;
            }
            mClipPool.Get("Audio/Bgm/" + name, audioCilp =>
             {
                 PublicMono.Instance.OnUpdate += Update;
                 //如果没有东西在播放
                 if (!mBgm.isPlaying)
                 {
                     mFade.SetState(FadeState.FadeIn, () =>
                      {
                          PublicMono.Instance.OnUpdate -= Update;
                      });
                     mBgm.clip = audioCilp;
                     mBgm.Play();
                 }
                 else
                 {
                     mFade.SetState(FadeState.FadeOut, () =>
                     {
                         mFade.SetState(FadeState.FadeIn, () =>
                         {
                             PublicMono.Instance.OnUpdate -= Update;
                         });
                         mBgm.clip = audioCilp;
                         mBgm.Play();
                     });
                 }
             });
        }
        public void StopPlay(AudioSource source)
        {
            mSourcePool.Push(source, source.Stop);
        }
        public void GetSound(string name,Action<AudioSource> callBack)
        {
            mSourcePool.AutoPush(cp => cp.isPlaying);
            mSourcePool.Get(out tempSound);
            mClipPool.Get("Audio/Sound/" + name, clip =>
            {
                tempSound.clip = clip;
                tempSound.loop = true;
                tempSound.volume = SoundVolume.Value;
                callBack(tempSound);
            });
        }
        public void PlaySound(string name)
        {
            mSourcePool.AutoPush(cp => !cp.isPlaying);
            mSourcePool.Get(out tempSound);
            mClipPool.Get("Audio/Sound/" + name, clip =>
            {
                tempSound.clip = clip;
                tempSound.loop = false;
                tempSound.volume = SoundVolume.Value;
                tempSound.Play();
            });
        }
        public void StopSound(AudioSource source)
        {
            mSourcePool.Push(source, () =>
             {
                 if (source.isPlaying) source.Stop();
             });
        }

        public void StopPlayBgm()
        {
            if (mBgm && mBgm.isPlaying) mBgm.Stop();
        }
    }
}
