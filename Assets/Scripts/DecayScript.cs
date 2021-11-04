using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecayScript : MonoBehaviour
{
    public float decayTime=0f;
    private float decayTimeCounter=0f;

    void Update()
    {
        if(decayTime>0f) {
            decayTimeCounter+=Time.deltaTime;
            if(decayTimeCounter>decayTime) {
                Destroy(this.gameObject);
            }
        }
    }

    public void SetDecayTime(float _decayTime) {
        decayTime=_decayTime;
    }
}
