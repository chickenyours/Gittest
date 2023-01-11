using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;

public interface IAudioPlaySystem : ISystem
{
    void PlaySound(string audioName);
    void Update();
    void Init_();
}

public class AudioPlaySound : AbstractSystem, IAudioPlaySystem
{
    private static List<AudioSource> mPlayingSounds = new List<AudioSource>();
    public AudioSource audioPlay;
    private string audioName;

    protected override void OnInit()
    {

        audioPlay = GameObject.Find("AudioPlay").GetComponent<AudioSource>();
    }

    void IAudioPlaySystem.Init_()
    {
        OnInit();
    }

    void IAudioPlaySystem.PlaySound(string audioName)
    {
        this.audioName = audioName;
        AudioClip clip = Resources.Load<AudioClip>("Sounds/" + audioName);//this.audioName
        MonoBehaviour.print(clip);
        this.audioPlay.clip = clip;
        this.audioPlay.Play();
        mPlayingSounds.Add(this.audioPlay);
    }

    void IAudioPlaySystem.Update()
    {
        for (int i = mPlayingSounds.Count - 1; i >= 0; i--)
        {
            
            if (!mPlayingSounds[i].isPlaying)
            {
                var sound = mPlayingSounds[i];
                mPlayingSounds.RemoveAt(i);
                sound = null;
            }
        }
    }




    //protected override void OnExecute()
    //{
    //    this.audioPlay.clip = Resources.Load<AudioClip>("Sounds" + this.audioName);
    //}
}
