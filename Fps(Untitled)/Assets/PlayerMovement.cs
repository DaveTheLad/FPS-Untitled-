using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;

    // Movement parameters
    public float speed = 12f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;

    // Ground check parameters
    public Transform groundCheck;
    public float groundDistance = 0.2f;
    public LayerMask groundMask;

    // Player velocity and grounded state
    Vector3 velocity;
    bool isGrounded;

    // Update is called once per frame
    void Update()
    {
        // Check if the player is on the ground
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

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

        // Check for jump input and ensure the player is on the ground before jumping
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            // Perform the jump using the jump formula
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // Apply gravity to the player
        velocity.y += gravity * Time.deltaTime;

        // Move the player vertically based on the calculated velocity
        controller.Move(velocity * Time.deltaTime);
    }
}
