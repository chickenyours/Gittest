using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Particle : MonoBehaviour
{
    public Action OnPlayFinish;
    private ParticleSystem mParticle;
    private void Start()
    {
        mParticle = this.GetComponent<ParticleSystem>();
    }
    private void Update() 
    {  
        if (mParticle && !mParticle.isPlaying) OnPlayFinish?.Invoke();
    }
}
