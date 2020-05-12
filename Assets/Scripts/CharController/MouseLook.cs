using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class MouseLook : MonoBehaviour
{

    public float mouseSens;

    public Transform player;
    public Transform rocketLauncher;
    public Transform camera;
    private float xRotation = 0f;
    private float mouseX;
    
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        mouseX = Input.GetAxis("Mouse X") * mouseSens * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSens * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
         
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        // rotates head if standing still with degree * 0.75 so it doesn't twist at 90 degrees
        //headTransform.Rotate(0.75f * mouseX * Vector3.up); 
        if (moving())
        {
            player.Rotate(Vector3.up * mouseX);   
        }
        else
        {
            camera.Rotate(Vector3.up * mouseX);
        }
        
        rocketLauncher.Rotate( Vector3.forward * mouseY);
    }

    private bool moving()
    {
        return Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D);
    }

   
}
