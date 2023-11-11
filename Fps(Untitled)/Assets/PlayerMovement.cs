using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;

    public float speed = 12f;
    public float gravity = -9.80f;
    public float JumpHeight = 3f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    Vector3 velocity;
    bool isGrounded;

    // Update is called once per frame
    void Update()
    {
        // Check if the player is grounded using a sphere check at the specified position
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        // If the player is grounded and falling, reset the vertical velocity
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // Get horizontal and vertical input for player movement
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // Calculate the movement direction based on the input and player's orientation
        Vector3 move = transform.right * x + transform.forward * z;

        // Move the player using CharacterController with speed and time-based deltaTime
        controller.Move(move * speed * Time.deltaTime);

        // Check for the jump input and whether the player is grounded
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            // Apply the jump formula to calculate the initial jump velocity
            velocity.y = Mathf.Sqrt(JumpHeight * -2f * gravity);
        }

        // Apply gravity to the player's vertical velocity over time
        velocity.y += gravity * Time.deltaTime;

        // Move the player vertically using CharacterController with time-based deltaTime
        controller.Move(velocity * Time.deltaTime);
    }
}
