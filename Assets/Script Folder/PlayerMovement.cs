using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;

    Animator animator;


    // runs only once when the session is begun and grabs the Animator component
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // Movement parameters
    private float speed;
    public float walkSpeed;
    public float sprintSpeed;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;

    // Ground check parameters
    public Transform groundCheck;
    public float groundDistance = 0.2f;
    public LayerMask groundMask;
    public KeyCode sprintKey = KeyCode.LeftShift;

    // Player velocity and grounded state
    Vector3 velocity;
    bool isGrounded;

    public MoveMentState state;
    public enum MoveMentState
    {
        walking,
        sprinting,
        air
    }

    private void StateHandler()
    {
        bool isSprinting = Input.GetKey(sprintKey);
        bool isJumping = Input.GetButton("Jump");

        // Mode - sprint and jump (b hop like) ;)
        if (isGrounded && isSprinting && isJumping)
        {
            state = MoveMentState.sprinting;
            speed = sprintSpeed * 1.5f; 
        }

        // Mode - sprint
        else if (isGrounded && isSprinting)
        {
            state = MoveMentState.sprinting;
            speed = sprintSpeed;
        }

        // Mode - Walking 
        else if (isGrounded)
        {
            state = MoveMentState.walking;
            speed = walkSpeed;
        }

        // Mode - Air 
        else
        {
            state = MoveMentState.air;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the player is on the ground
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        StateHandler();

        // Debug.DrawRay(groundCheck.position, Vector3.down * groundDistance, Color.red);

        // Apply gravity to make the player stick to the ground
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // Get input for horizontal and vertical movement
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // Calculate the movement direction based on player input
        Vector3 move = transform.right * x + transform.forward * z;

        // Move the player using CharacterController
        controller.Move(move * speed * Time.deltaTime);
        
        // Checks if player is moving
        if(move != Vector3.zero)
        {
            // starts walking animation
            animator.SetBool("walk", true);
        }else
        {   
            // ends walking animation
            animator.SetBool("walk", false);
        }

        // Checkfor jump input and ensure the player is on the ground before jumping
        if (Input.GetButton("Jump") && isGrounded)
        {
            // Perform the jump using the jump formula
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

            // start jumping animation
            animator.SetBool("jump", true);
        }else
        {
            // end jumping animation
            animator.SetBool("jump", false);
        }

        // Apply gravity to the player
        velocity.y += gravity * Time.deltaTime;

        // Move the player vertically based on the calculated velocity
        controller.Move(velocity * Time.deltaTime);
    }

}
