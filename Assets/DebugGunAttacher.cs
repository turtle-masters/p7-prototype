using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugGunAttacher : MonoBehaviour
{
    public Vector3 posOffset;
    private GameObject DebugCam;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        DebugCam = GameObject.Find("DebugCamera");
        if(DebugCam!=null) {
            transform.parent.transform.SetParent(DebugCam.transform);
            transform.parent.transform.localPosition = posOffset;
            Destroy(this);
        }
    }
}
