using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed = 12f;
    public float gravity = -9.81f * 2;
    public float jumpHeight = 3f;


    public Transform groundCheck;
    public float groundDistance = 1.5f;
    public LayerMask groundMask;
    private CharacterController controller;

    Vector3 velocity;


    bool isGrounded;
    bool isMoving;


    private Vector3 lastPosition = new Vector3(0f, 0f, 0f);
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        //ground check
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float x =Input.GetAxis("Horizontal");
        float z =Input.GetAxis("Vertical");
        // CREATING MOVE VECTOR 
        Vector3 move = transform.right * x + transform.forward * z;
        // ERM ACUALLTY MOVING THE PLAYER
        controller.Move(move * speed * Time.deltaTime);

        //Checking if player can jump
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
           // jumping fr rn
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // HELP im falling 
        velocity.y += gravity * Time.deltaTime;
        // excute order JUMP
        controller.Move(velocity * Time.deltaTime);

        if (lastPosition !=gameObject.transform.position && isGrounded == true)
        {
            isMoving = true;
            // For later use
        }
        else
        {
            isMoving = false;
            // For later use 
        }

        lastPosition = gameObject.transform.position;
    }
}
