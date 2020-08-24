using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCharacterController : MonoBehaviour
{
    public CharacterController controller;
    public Transform cam;
    public Animator animator;

    float movement = 0f;
    bool jump = false;

    public float Speed = 6f;
    public float jumpForce = 2f;
    public float gravity = 4f;
    public float turnSmoothTime = 0.1f;
    float verticalVelocity;
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
            float targetAngle = Mathf.Atan2(playerMovement.x, playerMovement.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            moveDir = moveDir.normalized;
            movement += Speed * Time.deltaTime;

        }
        else
        {
            movement -= Speed * Time.deltaTime;
        }

        // Jump Logic
        if(controller.isGrounded)
        {
            jump = false;
            if(Input.GetButtonDown("Jump"))
            {
                verticalVelocity = jumpForce;
                jump = true;
            }
        }
        else
        {
            jump = true;
            verticalVelocity -= gravity * Time.deltaTime;
        }
        moveDir.y = verticalVelocity;
        controller.Move(moveDir * Speed * Time.deltaTime);

        movement = Mathf.Clamp(movement, 0f, 1f);
        animator.SetFloat("movementForward", movement);
        animator.SetBool("Jump", jump);
    }
}
