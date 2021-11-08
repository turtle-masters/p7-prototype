using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MicroverseExtraSounds : MonoBehaviour
{
    public AudioSource source;
    public AudioClip[] clips;


    public bool[] bools;


    public void playClip(int lvl)
    {
        if (!bools[lvl-1])
        {
            source.PlayOneShot(clips[lvl - 1]);
            bools[lvl-1] = true;
        }
       

    }

}
