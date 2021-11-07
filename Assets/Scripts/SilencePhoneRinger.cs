using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SilencePhoneRinger : MonoBehaviour  // a workaround for silencing the phone on pick-up
{
    public void Activate()
    {
        this.GetComponent<Task>().playbackVolume = 0f;
    }

    public void Activate(Prompt parentPrompt)
    {
        this.Activate();
    }
}
