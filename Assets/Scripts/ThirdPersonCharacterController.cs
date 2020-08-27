using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCharacterController : MonoBehaviour
{
    // Component Inputs
    public CharacterController controller;
    public Transform cam;
    public Animator animator;

    // Input variables
    public float Speed = 6f;
    public float turnSmoothTime = 0.1f;
    public float jumpForce = 2f;
    public float gravity = 4f;

    // Private variables
    float movement = 0f;
    bool jump = false;
    float yVelocity = 0f;
    float turnSmoothVelocity;
    Vector3 moveDir;

    // Update is called once per frame
    void Update()
    {
        moveDir = Vector3.zero;
        float hor = Input.GetAxis("Horizontal");
        float ver = Input.GetAxis("Vertical");
        Vector3 playerMovement = new Vector3(hor, 0f, ver).normalized;

        // Movement Logic
        
        if(playerMovement.magnitude >= 0.1f)
        {
            // Update player's location if the magnitude of the input is significant enough
            float targetAngle = Mathf.Atan2(playerMovement.x, playerMovement.z) * Mathf.Rad2Deg + cam.eulerAngles.y; // Find angle to move towards relative to camera direction
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime); // Dampen Angle for character rotation
            transform.rotation = Quaternion.Euler(0f, angle, 0f); // Rotate character (Mesh) towards direction of movement

            moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward; //Convert angular direction to XYZ coordinate units to move towards
            moveDir = moveDir.normalized; // Normalize units to make diagonal movement to be the same speed as movement on axis
            movement += Speed * Time.deltaTime; // Change the value of the idle-walk-run blend control variable.

        }
        else
        {
            movement -= Speed * Time.deltaTime; // Decay blend variable as no horizontal plane movement detected.
        }

        // Jump Logic
        if(controller.isGrounded)
        {
            jump = false; // Character is on the ground, set jump boolean to false
            if(Input.GetButtonDown("Jump")) // Listen for "Jump" button to be pressed
            {
                yVelocity = jumpForce; // Update jump velocity
                jump = true; // Toggle jump boolean for animation switch
            }
        }
        else
        {
            // Character is not on the ground; character is in the air.
            jump = true;
            yVelocity -= gravity * Time.deltaTime; // Update vertical velocity for gravity.
        }
        moveDir.y = yVelocity; // Add the vertical velocity component to the overall movement
        controller.Move(moveDir * Speed * Time.deltaTime); // Move the actual character

        movement = Mathf.Clamp(movement, 0f, 1f); // Clamp the value for animation blending.

        // Store Values for animation blending and transitions
        animator.SetFloat("movementForward", movement);
        animator.SetBool("Jump", jump);
    }
}
