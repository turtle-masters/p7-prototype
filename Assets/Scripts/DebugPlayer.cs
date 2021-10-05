using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Allows for rudimentary control of the Player object for debugging purposes
 */
public class DebugPlayer : MonoBehaviour
{
    public static bool isActive = false;
    public static float sensitivity = 4f;
    public static float speed = 2f;
    public static float height = 1.5f;

    private GameObject playerObject;

    protected void Awake()
    {
        // kill self if the game is not running from the editor, preventing any more of the code in this file from executing (including Start)
        #if !UNITY_EDITOR
        this.gameObject.GetComponent<DebugPlayer>().enabled = false;
        #endif
    }

    protected void Start()
    {
        this.playerObject = GameObject.FindGameObjectsWithTag("Player")[0];  // there should only ever be one GameObject tagged with "Player"
    }

    protected void FixedUpdate()
    {
        if (isActive)
        {
            // mouse input
            float rotateHorizontal = Input.GetAxis("Mouse X");
            float rotateVertical = Input.GetAxis("Mouse Y");
            Transform transform = this.playerObject.transform.Find("DebugCamera");
            transform.Rotate(Vector3.up, rotateHorizontal * sensitivity, Space.World);
            transform.Rotate(-this.playerObject.transform.right, rotateVertical * sensitivity);
        }
        // keyboard input
        Vector3 movement = new Vector3(0, 0, 0);
        if (Input.GetKey(KeyCode.W))
            movement.z += 1;
        if (Input.GetKey(KeyCode.A))
            movement.x += 1;
        if (Input.GetKey(KeyCode.S))
            movement.z -= 1;
        if (Input.GetKey(KeyCode.D))
            movement.x -= 1;
        if (!isActive && !movement.Equals(new Vector3(0, 0, 0)))
        {
            // reset the view (direction)
            GameObject.FindGameObjectWithTag("MainCamera").SetActive(false);
            GameObject newCamera = new GameObject("DebugCamera");
            newCamera.transform.SetParent(this.playerObject.transform);
            newCamera.AddComponent<Camera>();
            newCamera.transform.rotation = Quaternion.LookRotation(new Vector3(1, 0, 0));

            // reset the view (height)
            newCamera.transform.Translate(new Vector3(0, height, 0));

            // break out after first movement input
            isActive = true;
        }
        // move the Player
        this.playerObject.transform.Translate(movement * speed * Time.deltaTime, this.playerObject.transform.Find("DebugCamera"));
    }
}
