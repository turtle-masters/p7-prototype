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

    void Update()
    {
        rb.velocity=transform.forward*moveSpeed;
        rb.angularVelocity = -transform.forward*moveSpeed/5f;
    }

    public void SetSpeed(float _moveSpeed) {
        moveSpeed = _moveSpeed;
    }
}
