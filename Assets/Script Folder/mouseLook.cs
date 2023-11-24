using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mouseLook : MonoBehaviour
{
    // Sensitivity of the mouse movement
    public float mouseSensitivity = 100f;

    // Reference to the player's body (used for horizontal rotation)
    public Transform playerBody;

    // Initial rotation around the X-axis
    float xRotation = 0f;

    // Start is called before the first frame update
    void Start()
    {
        // Lock the cursor to the center of the screen
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        // Get mouse input for X and Y axis
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Adjust the rotation around the X-axis based on the mouseY input
        xRotation -= mouseY;

        // Clamp the vertical rotation to prevent over-rotation
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // Apply the rotation to the camera around the local X-axis
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Rotate the player's body horizontally based on the mouseX input
        playerBody.Rotate(Vector3.up * mouseX);
    }
}
