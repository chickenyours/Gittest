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
        AudioSource GetSound(string name);
        void StopSound(AudioSource source);
        void StopPlay(AudioSource source);
        void StopPlayBgm();
        void RecoverySound(AudioSource source);
    }
    public class AudioSystem : AbstractSystem, IAudioMgrSystem
    {
        public static AudioSystem instance;
        private IAudioModel mAudioModel;
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
            //游戏临时播放器指针
            tempSound = new AudioSource();
            //播放器组件池
            mSourcePool = new ComponentPool<AudioSource>("GameSound");
            //音频资源池
            mClipPool = new ResPool<AudioClip>();
            //数据模块
            mAudioModel = this.GetModel<IAudioModel>();
            //淡化渐入模块
            mFade = new FadeNum();
            mFade.SetMaxMin(mAudioModel.BgmVolume.Value,0);
            //注意：注册值改变时会首次运行委托函数，初始化时应在后面注册
            mAudioModel.SoundVolume.RegisterWithInitValue(OnSoundVolumeChanged);
            mAudioModel.BgmVolume.RegisterWithInitValue(OnBgmVolumeChanged);
            this.RegisterEvent<StopBgmEvent>(OnStopBgm);
        }
        private void OnBgmVolumeChanged(float v)
        {
            mFade.SetMaxMin(v,0);
            if(mBgm) mBgm.volume = mAudioModel.BgmVolume.Value;
        }
        private void OnSoundVolumeChanged(float v)
        {
            mSourcePool.SetAllComponent(audiosource =>
            {
                audiosource.volume =mAudioModel.SoundVolume.Value;
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
        void IAudioMgrSystem.PlayBgm(string name)
        {
            mClipPool.Get("Audio/Bgm/" + name, PlayBgm);
        }

        private void PlayBgm(AudioClip audioClip)
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
                 PublicMono.Instance.OnUpdate += Update;
                 //如果没有东西在播放
                 if (!mBgm.isPlaying)
                 {
                     mFade.SetState(FadeState.FadeIn, () =>
                      {
                          PublicMono.Instance.OnUpdate -= Update;
                      });
                     mBgm.clip = audioClip;
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
                         mBgm.clip = audioClip;
                         mBgm.Play();
                     });
                 }
        }
        public void StopPlay(AudioSource source)
        {
            mSourcePool.Push(source, source.Stop);
        }
        public AudioSource GetSound(string name)
        {
            InitSource();   // out tempSound
            mClipPool.Get("Audio/Sound/" + name, clip =>
            {
                tempSound.clip = clip;
                tempSound.loop = true;
                tempSound.volume = mAudioModel.SoundVolume.Value;
            });
            return tempSound;
        }
        public void PlaySound(string name)
        {
            InitSource();   // out tempSound
            mClipPool.Get("Audio/Sound/" + name, clip =>
            {
                tempSound.clip = clip;
                tempSound.loop = false;
                tempSound.volume =mAudioModel.SoundVolume.Value;
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
        //回收音效组件(通常用于循环音效)
        public void RecoverySound(AudioSource source)
        {
            mSourcePool.Push(source, source.Stop);
        }
        public void StopPlayBgm()
        {
            if (mBgm && mBgm.isPlaying) mBgm.Stop();
        }
        //初始化组件
        public void InitSource()
        {
            mSourcePool.AutoPush(source =>  !source.isPlaying);
            mSourcePool.Get(out tempSound);
            tempSound.volume = mAudioModel.SoundVolume.Value;
        }
    }
}
