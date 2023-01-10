using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;

public class Gold : GameControll
{
    private GameObject effect;
    void Start()
    {
        effect = this.GetSystem<IObjectPoolSystem>().Get("Effect/Particle");
        effect.transform.SetParent(this.transform);
        effect.transform.position = transform.position;
        var o = effect.GetComponent<Particle>();
        o.OnPlayFinish = () =>
        {
            this.GetSystem<IObjectPoolSystem>().Recovery(effect);
        };
    }
    void OnDisable()
    {
        effect.GetComponent<ParticleSystem>().loop = false;
    }

}
