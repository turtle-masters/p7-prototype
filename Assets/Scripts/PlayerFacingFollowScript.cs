using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFacingFollowScript : MonoBehaviour
{
    public GameObject mainCamera;
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(mainCamera.transform,Vector3.up);
    }
}
