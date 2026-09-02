using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public float walkSpeed = 5f;
    public float sprintSpeed = 10f;
    public float jumpHeight = 2f;
    public float gravity = -20f;

    private CharacterController controller;
    private Vector3 velocity;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // Movement
        Vector3 movement = Vector3.zero;

        if (Keyboard.current.wKey.isPressed)
            movement += transform.forward;

        if (Keyboard.current.sKey.isPressed)
            movement -= transform.forward;

        if (Keyboard.current.aKey.isPressed)
            movement -= transform.right;

        if (Keyboard.current.dKey.isPressed)
            movement += transform.right;

        // Prevent diagonal movement from being faster
        movement = movement.normalized;

        // Sprinting
        float currentSpeed = walkSpeed;

        if (Keyboard.current.leftShiftKey.isPressed)
        {
            currentSpeed = sprintSpeed;
        }

        // Move horizontally
        controller.Move(movement * currentSpeed * Time.deltaTime);

        // Keep player on the ground
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // Jump
        if (controller.isGrounded && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // Apply gravity
        velocity.y += gravity * Time.deltaTime;

        // Move vertically
        controller.Move(velocity * Time.deltaTime);
    }
}