using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;

public class AudioPlay : AbstractCommand
{
    private List<AudioSource> mPlayingSounds;
    public static AudioPlay Instance;

   
public AudioPlay(List<AudioSource> mPlayingSounds)
    {
        this.mPlayingSounds = mPlayingSounds;
    }

    protected override void OnExecute()
    {
        
    }
}
