using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeMovement : MonoBehaviour
{
    public float moveSpeed = 1f;
    private Rigidbody rb=null;

    private void Start() {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity=transform.forward*moveSpeed;
    }
}
