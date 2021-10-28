using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlePrompt : Prompt
{
    ParticleSystem localParticle;

    protected override void Start()
    {
        base.Start();

        localParticle = this.GetComponent<ParticleSystem>();
        localParticle.Pause();
    }

    protected override void TurnOn()
    {
        base.TurnOn();

        localParticle.Play();
    }

    protected override void TurnOff()
    {
        base.TurnOff();

        localParticle.Stop();
    }
}
