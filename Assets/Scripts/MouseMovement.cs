using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseMovement : MonoBehaviour
{
      public float mouseSensitivity = 1000f;
   
   float xRotation = 0f;
   float yRotation = 0f;

   public float topClamp= -90f;
   public float bottomClamp= 90f;
    // Start is called before the first frame update
    void Start()
    {
        //locking the cursor
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        // getting mouse inputs
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        //rotation around x
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, topClamp, bottomClamp);
        //rotation around x
        yRotation += mouseX;
        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
    }
}
